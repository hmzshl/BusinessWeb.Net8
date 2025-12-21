using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Enum;
using BusinessWeb.Models.PersoAPI;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BusinessWeb.Models.PersoAPI.Document;

namespace BusinessWeb.Services
{
	public class DocumentService
	{
		private readonly DB _context;
		private readonly DocCurrentPieceService _pieceService;

		public DocumentService(DB context, DocCurrentPieceService pieceService)
		{
			_context = context;
			_pieceService = pieceService;
		}

		// ===== MAIN DOCUMENT CREATION METHOD =====

		/// <summary>
		/// Creates a complete document with entête and lignes
		/// </summary>
		public async Task<DocumentCreateResult> CreateDocument(Document.DocumentEntete document)
		{
			var result = new DocumentCreateResult();

			try
			{
				// 1. Get document type information
				var docInfo = GetDocumentInfoFromTypeDO(document.Type);
				short domaine = DocumentTypeHelper.GetDomaineFromDocumentTypeDO(document.Type);
				short doType = DocumentTypeHelper.GetDONumberFromDocumentTypeDO(document.Type);

				// Get DocumentTypeDC from DocumentTypeDO
				var dcType = DocumentTypeHelper.GetDocumentTypeDC(domaine, document.Type, docInfo?.Provenance);

				if (!dcType.HasValue)
				{
					throw new ArgumentException($"Impossible de déterminer le type de document pour {document.Type}");
				}

				// 2. Reserve the next piece number
				string pieceNumber = await _pieceService.GetCurrentPieceNumber(
					(DocumentDomaine)domaine,
					dcType.Value,
					document.Souche);

				// 3. Create the document entête
				var fDocEntete = await CreateDocEntete(domaine, doType, pieceNumber, document, docInfo);
				string nextNumber = await _pieceService.ReserveNextPieceNumber(
				(DocumentDomaine)domaine,
				dcType.Value,
				document.Souche);
				// 4. Create document lignes
				var fDocLignes = await CreateDocLignes(domaine, doType, pieceNumber, document.Lignes);

				// 5. Update result
				result.Success = true;
				result.DocumentNumber = pieceNumber;
				result.DocEnteteId = fDocEntete.cbMarq;
				result.LigneIds = fDocLignes.Select(l => l.cbMarq).ToList();
				result.Message = "Document créé avec succès";
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = $"Erreur lors de la création du document: {ex.Message}";
				result.Error = ex;
			}

			return result;
		}

		/// <summary>
		/// Creates multiple documents in batch
		/// </summary>
		public async Task<List<DocumentCreateResult>> CreateDocumentsBatch(List<Document.DocumentEntete> documents)
		{
			var results = new List<DocumentCreateResult>();

			// Process each document
			foreach (var document in documents)
			{
				var result = await CreateDocument(document);
				results.Add(result);
			}

			return results;
		}

		// ===== DOCUMENT INFO HELPER =====

		private DocumentTypeInfo GetDocumentInfoFromTypeDO(DocumentTypeDO typeDO)
		{
			short domaine = DocumentTypeHelper.GetDomaineFromDocumentTypeDO(typeDO);

			// Try to get info by domaine and typeDO
			var infos = DocumentTypeHelper.GetByDomaine(domaine)
				.Where(info => info.TypeDO == typeDO)
				.ToList();

			if (infos.Count == 1)
				return infos[0];

			if (infos.Count > 1)
			{
				// For invoices, return the first one (Normal provenance)
				return infos.FirstOrDefault(info => info.Provenance == DocumentProvenance.Normal)
					?? infos.First();
			}

			// If no exact match, create a basic info object
			return new DocumentTypeInfo
			{
				Domaine = domaine,
				TypeDO = typeDO,
				Label = typeDO.ToString(),
				Prefix = GetDefaultPrefixFromTypeDO(typeDO),
				Description = $"Document de type {typeDO}",
				DC_Id = (short)((int)typeDO % 10) // Last digit as DC_Id
			};
		}

		private string GetDefaultPrefixFromTypeDO(DocumentTypeDO typeDO)
		{
			// Map DocumentTypeDO to standard prefixes
			return typeDO switch
			{
				// Vente
				DocumentTypeDO.DevisClient => "DE",
				DocumentTypeDO.BonCommandeClient => "BC",
				DocumentTypeDO.PreparationLivraisonClient => "PL",
				DocumentTypeDO.BonLivraisonClient => "BL",
				DocumentTypeDO.BonRetourClient => "BR",
				DocumentTypeDO.BonAvoirClient => "BA",
				DocumentTypeDO.FactureVente => "FA",

				// Achat
				DocumentTypeDO.DemandeAchatFournisseur => "DA",
				DocumentTypeDO.PreparationCommandeFournisseur => "PC",
				DocumentTypeDO.BonCommandeFournisseur => "FBC",
				DocumentTypeDO.BonLivraisonFournisseur => "FBL",
				DocumentTypeDO.BonRetourFournisseur => "FBR",
				DocumentTypeDO.BonAvoirFournisseur => "FBA",
				DocumentTypeDO.FactureAchat => "FFA",

				// Stock
				DocumentTypeDO.MouvementEntree => "ME",
				DocumentTypeDO.MouvementSortie => "MS",
				DocumentTypeDO.Declassement => "DS",
				DocumentTypeDO.Transfert => "MT",
				DocumentTypeDO.PreparationFabrication => "PF",
				DocumentTypeDO.OrdreFabrication => "OF",
				DocumentTypeDO.BonFabrication => "BF",

				// Autre
				DocumentTypeDO.DocumentInterne1 => "CA",
				DocumentTypeDO.DocumentInterne2 => "BI",
				DocumentTypeDO.DocumentInterne3 => "RC",
				DocumentTypeDO.DocumentInterne4 => "SP",
				DocumentTypeDO.DocumentInterne5 => "RP",
				DocumentTypeDO.DocumentInterne6 => "SR",

				_ => "DOC"
			};
		}

		// ===== DOCUMENT ENTETE CREATION =====

		private async Task<F_DOCENTETE> CreateDocEntete(short domaine, short doType, string pieceNumber,
			Document.DocumentEntete document, DocumentTypeInfo docInfo)
		{
			var now = DateTime.Now;

			// Create the F_DOCENTETE entity
			var fDocEntete = new F_DOCENTETE
			{
				// Document identification
				DO_Domaine = domaine,
				DO_Type = doType,
				DO_Piece = pieceNumber,
				DO_Date = document.Date,
				DO_Ref = document.Reference,
				DO_Tiers = document.Tiers,
				DO_Souche = document.Souche,

				// External references
				DE_No = document.Depot > 0 ? document.Depot : (int?)null,
				CO_No = !string.IsNullOrEmpty(document.Affaire) ? int.Parse(document.Affaire) : (int?)null,

				// Audit fields
				cbProt = 0,
				cbCreateur = "API",
				cbModification = now,
				cbReplication = 0,
				cbFlag = 0,
				cbCreation = now,
				cbMarq = await GetNextDocEnteteMarq()
			};

			// Set default values
			SetDocEnteteDefaults(fDocEntete, document.Date);

			// Set provenance if available
			if (docInfo?.Provenance.HasValue == true)
			{
				fDocEntete.DO_Provenance = (short)docInfo.Provenance.Value;

				// For invoices, calculate totals
				decimal totalHT = document.Lignes.Sum(l => l.TotalHT);
				fDocEntete.DO_TotalHT = totalHT;
				fDocEntete.DO_TotalHTNet = totalHT;

				// Calculate TVA and TTC
				decimal totalTVA = CalculateTotalTVA(document.Lignes);
				fDocEntete.DO_TotalTTC = totalHT + totalTVA;
				fDocEntete.DO_NetAPayer = totalHT + totalTVA;
			}

			// Insert using BulkCopy for performance
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(new List<F_DOCENTETE> { fDocEntete });

			return fDocEntete;
		}

		// ===== SET DEFAULT VALUES FOR DOCENTETE =====

		private void SetDocEnteteDefaults(F_DOCENTETE docEntete, DateTime documentDate)
		{
			// Document defaults
			docEntete.DO_Expedit = 0;
			docEntete.DO_NbFacture = 0;
			docEntete.DO_BLFact = 0;
			docEntete.DO_TxEscompte = 0;
			docEntete.DO_Reliquat = 0;
			docEntete.DO_Imprim = 0;
			docEntete.DO_DateLivr = documentDate;
			docEntete.DO_Condition = 0;
			docEntete.DO_Tarif = 0;
			docEntete.DO_Colisage = 0;
			docEntete.DO_TypeColis = 0;
			docEntete.DO_Transaction = 0;
			docEntete.DO_Langue = 0;
			docEntete.DO_Ecart = 0;
			docEntete.DO_Regime = 0;
			docEntete.DO_Ventile = 0;
			docEntete.DO_Statut = 0;
			docEntete.DO_Transfere = 0;
			docEntete.DO_Cloture = 0;
			docEntete.DO_Attente = 0;
			docEntete.DO_TypeFrais = 0;
			docEntete.DO_ValFrais = 0;
			docEntete.DO_TypeLigneFrais = 0;
			docEntete.DO_TypeFranco = 0;
			docEntete.DO_ValFranco = 0;
			docEntete.DO_TypeLigneFranco = 0;
			docEntete.DO_Taxe1 = 0;
			docEntete.DO_TypeTaux1 = 0;
			docEntete.DO_TypeTaxe1 = 0;
			docEntete.DO_Taxe2 = 0;
			docEntete.DO_TypeTaux2 = 0;
			docEntete.DO_TypeTaxe2 = 0;
			docEntete.DO_Taxe3 = 0;
			docEntete.DO_TypeTaux3 = 0;
			docEntete.DO_TypeTaxe3 = 0;
			docEntete.DO_MajCpta = 0;
			docEntete.DO_FactureElec = 0;
			docEntete.DO_TypeTransac = 0;
			docEntete.DO_DemandeRegul = 0;
			docEntete.DO_Valide = 1; // Document validé
			docEntete.DO_Coffre = 0;
			docEntete.DO_StatutBAP = 0;
			docEntete.DO_Escompte = 0;
			docEntete.DO_DocType = 0;
			docEntete.DO_TypeCalcul = 0;
			docEntete.DO_PaiementLigne = 0;
			docEntete.DO_MotifDevis = 0;

			// Default dates (Sage minimum date)
			var defaultDate = new DateTime(1753, 1, 1);
			docEntete.DO_DebutAbo = defaultDate;
			docEntete.DO_FinAbo = defaultDate;
			docEntete.DO_DebutPeriod = defaultDate;
			docEntete.DO_FinPeriod = defaultDate;
			docEntete.DO_DateLivrRealisee = defaultDate;
			docEntete.DO_DateExpedition = defaultDate;
		}

		// ===== DOCUMENT LIGNE CREATION =====

		private async Task<List<F_DOCLIGNE>> CreateDocLignes(short domaine, short doType, string pieceNumber,
			List<Document.DocumentLigne> lignes)
		{
			var fDocLignes = new List<F_DOCLIGNE>();
			var now = DateTime.Now;
			int lineNumber = 1;

			foreach (var ligne in lignes)
			{
				// Use ligne.Type if provided, otherwise use document type
				short ligneDoType = ligne.Type != default ?
					DocumentTypeHelper.GetDONumberFromDocumentTypeDO(ligne.Type) :
					doType;

				var fDocLigne = new F_DOCLIGNE
				{
					// Document identification
					DO_Domaine = domaine,
					DO_Type = ligneDoType,
					DO_Piece = pieceNumber,
					DO_Date = DateTime.Now,

					// Line information
					DL_Ligne = lineNumber,
					DO_Ref = "", // Could be set from document reference
					AR_Ref = ligne.RefArticle,
					DL_Design = ligne.Designation,
					DL_Qte = ligne.Qte,
					DL_PrixUnitaire = ligne.PU,
					DL_PUBC = ligne.PU,
					DL_Taxe1 = CalculateTVA(ligne),
					DL_No = lineNumber,
					DO_DateLivr = DateTime.Now,
					DL_CodeTaxe1 = ligne.CodeTVA,

					// Audit fields
					cbProt = 0,
					cbCreateur = "API",
					cbModification = now,
					cbReplication = 0,
					cbFlag = 0,
					cbCreation = now,
					cbMarq = await GetNextDocLigneMarq()
				};

				// Set default values for ligne
				SetDocLigneDefaults(fDocLigne);

				// Calculate line amounts
				fDocLigne.DL_MontantHT = ligne.Qte * ligne.PU;
				fDocLigne.DL_MontantTTC = fDocLigne.DL_MontantHT + (fDocLigne.DL_Taxe1 ?? 0);

				// Set specific piece fields based on document type
				SetLignePieceFields(fDocLigne, ligneDoType, pieceNumber, ligne.Qte);

				fDocLignes.Add(fDocLigne);
				lineNumber++;
			}

			// Insert all lines using BulkCopy
			if (fDocLignes.Any())
			{
				using var db = _context.CreateLinqToDBConnection();
				await db.BulkCopyAsync(fDocLignes);
			}

			return fDocLignes;
		}

		// ===== SET DEFAULT VALUES FOR DOCLIGNE =====

		private void SetDocLigneDefaults(F_DOCLIGNE docLigne)
		{
			// Line defaults
			docLigne.DL_QteBC = 0;
			docLigne.DL_QteBL = 0;
			docLigne.DL_PoidsNet = 0;
			docLigne.DL_PoidsBrut = 0;
			docLigne.DL_Remise01REM_Valeur = 0;
			docLigne.DL_Remise01REM_Type = 0;
			docLigne.DL_Remise02REM_Valeur = 0;
			docLigne.DL_Remise02REM_Type = 0;
			docLigne.DL_Remise03REM_Valeur = 0;
			docLigne.DL_Remise03REM_Type = 0;
			docLigne.DL_TypeTaux1 = 0;
			docLigne.DL_TypeTaxe1 = 0;
			docLigne.DL_Taxe2 = 0;
			docLigne.DL_TypeTaux2 = 0;
			docLigne.DL_TypeTaxe2 = 0;
			docLigne.DL_Taxe3 = 0;
			docLigne.DL_TypeTaux3 = 0;
			docLigne.DL_TypeTaxe3 = 0;
			docLigne.DL_PrixRU = docLigne.DL_PrixUnitaire;
			docLigne.DL_CMUP = 0;
			docLigne.DL_MvtStock = 1; // Movement in stock
			docLigne.DL_TTC = 0;
			docLigne.DL_NoRef = 0;
			docLigne.DL_TypePL = 0;
			docLigne.DL_PUDevise = 0;
			docLigne.DL_PUTTC = 0;
			docLigne.DL_Taxe3 = 0;
			docLigne.DL_TypeTaux3 = 0;
			docLigne.DL_TypeTaxe3 = 0;
			docLigne.DL_Frais = 0;
			docLigne.DL_Valorise = 1;
			docLigne.DL_NonLivre = 0;
			docLigne.DL_FactPoids = 0;
			docLigne.DL_Escompte = 0;
			docLigne.DL_NoLink = null;
			docLigne.DL_QteRessource = 0;
			docLigne.DL_CodeTaxe2 = null;
			docLigne.DL_CodeTaxe3 = null;
			docLigne.DL_NoSousTotal = 0;

			// Default decimal values
			docLigne.DL_QteDE = 0;
			docLigne.DL_QtePL = 0;
		}

		// ===== SET LIGNE PIECE FIELDS =====

		private void SetLignePieceFields(F_DOCLIGNE docLigne, short doType, string pieceNumber, decimal quantity)
		{
			var now = DateTime.Now;

			// Set the appropriate piece field based on document type
			switch (doType)
			{
				case 0: // Devis (DE)
					docLigne.DL_PieceDE = pieceNumber;
					docLigne.DL_DateDE = now;
					docLigne.DL_QteDE = quantity;
					break;
				case 1: // Bon Commande (BC)
					docLigne.DL_PieceBC = pieceNumber;
					docLigne.DL_DateBC = now;
					docLigne.DL_QteBC = quantity;
					break;
				case 2: // Préparation Livraison (PL)
					docLigne.DL_PiecePL = pieceNumber;
					docLigne.DL_DatePL = now;
					docLigne.DL_QtePL = quantity;
					break;
				case 3: // Bon Livraison (BL)
				case 4: // Bon Retour (BR)
				case 5: // Bon Avoir (BA)
					docLigne.DL_PieceBL = pieceNumber;
					docLigne.DL_DateBL = now;
					docLigne.DL_QteBL = quantity;
					break;
				case 10: // Demande Achat (DA)
					docLigne.DL_PieceDE = pieceNumber;
					docLigne.DL_DateDE = now;
					docLigne.DL_QteDE = quantity;
					break;
				case 11: // Préparation Commande (PC)
					docLigne.DL_PieceBC = pieceNumber;
					docLigne.DL_DateBC = now;
					docLigne.DL_QteBC = quantity;
					break;
				case 12: // Bon Commande Fournisseur (FBC)
					docLigne.DL_PiecePL = pieceNumber;
					docLigne.DL_DatePL = now;
					docLigne.DL_QtePL = quantity;
					break;
				case 13: // Bon Livraison Fournisseur (FBL)
				case 14: // Bon Retour Fournisseur (FBR)
				case 15: // Bon Avoir Fournisseur (FBA)
					docLigne.DL_PieceBL = pieceNumber;
					docLigne.DL_DateBL = now;
					docLigne.DL_QteBL = quantity;
					break;
					// For other types (invoices, stock movements, etc.)
					// Only DO_Piece is used
			}
		}

		// ===== TVA CALCULATION METHODS =====

		private decimal? CalculateTVA(Document.DocumentLigne ligne)
		{
			// Simplified TVA calculation
			decimal tvaRate = GetTVARateFromCode(ligne.CodeTVA);
			return ligne.TotalHT * tvaRate / 100;
		}

		private decimal CalculateTotalTVA(List<Document.DocumentLigne> lignes)
		{
			return lignes.Sum(l => CalculateTVA(l) ?? 0);
		}

		private decimal GetTVARateFromCode(string codeTVA)
		{
			if (string.IsNullOrEmpty(codeTVA))
				return 20.00m;

			return codeTVA.ToUpper() switch
			{
				"TVA20" or "20" or "20%" => 20.00m,
				"TVA10" or "10" or "10%" => 10.00m,
				"TVA5.5" or "5.5" or "5.5%" or "5,5" => 5.50m,
				"TVA0" or "0" or "0%" => 0.00m,
				"EXO" or "EXONERE" => 0.00m,
				_ => 20.00m
			};
		}

		// ===== UTILITY METHODS =====

		private async Task<int> GetNextDocEnteteMarq()
		{
			var maxMarq = await _context.F_DOCENTETE
				.MaxAsync(e => (int?)e.cbMarq) ?? 0;
			return maxMarq + 1;
		}

		private async Task<int> GetNextDocLigneMarq()
		{
			var maxMarq = await _context.F_DOCLIGNE
				.MaxAsync(l => (int?)l.cbMarq) ?? 0;
			return maxMarq + 1;
		}

		// ===== VALIDATION METHODS =====

		public async Task<ValidationResult> ValidateDocument(Document.DocumentEntete document)
		{
			var result = new ValidationResult();

			if (document.Type == default)
			{
				result.Errors.Add("Le type de document est obligatoire");
				result.IsValid = false;
				return result;
			}

			// Validate required fields
			if (document.Date == default)
			{
				result.Errors.Add("La date du document est obligatoire");
			}

			if (string.IsNullOrEmpty(document.Tiers))
			{
				result.Errors.Add("Le tiers est obligatoire");
			}

			if (document.Lignes == null || !document.Lignes.Any())
			{
				result.Errors.Add("Le document doit contenir au moins une ligne");
			}
			else
			{
				// Validate each line
				for (int i = 0; i < document.Lignes.Count; i++)
				{
					var ligne = document.Lignes[i];

					if (string.IsNullOrEmpty(ligne.RefArticle))
					{
						result.Errors.Add($"Ligne {i + 1}: La référence article est obligatoire");
					}

					if (ligne.Qte <= 0)
					{
						result.Errors.Add($"Ligne {i + 1}: La quantité doit être positive");
					}

					if (ligne.PU < 0)
					{
						result.Errors.Add($"Ligne {i + 1}: Le prix unitaire ne peut pas être négatif");
					}
				}
			}

			// Check if tiers exists
			if (!string.IsNullOrEmpty(document.Tiers))
			{
				bool tiersExists = await _context.F_COMPTET
					.AnyAsync(c => c.CT_Num == document.Tiers);

				if (!tiersExists)
				{
					result.Warnings.Add($"Le tiers {document.Tiers} n'existe pas dans la base");
				}
			}

			// Check if article references exist
			var articleRefs = document.Lignes.Select(l => l.RefArticle).Distinct();
			var existingArticles = await _context.F_ARTICLE
				.Where(a => articleRefs.Contains(a.AR_Ref))
				.Select(a => a.AR_Ref)
				.ToListAsync();

			foreach (var refArticle in articleRefs)
			{
				if (!existingArticles.Contains(refArticle))
				{
					result.Warnings.Add($"L'article {refArticle} n'existe pas dans la base");
				}
			}

			result.IsValid = !result.Errors.Any();
			return result;
		}

		// ===== DOCUMENT QUERY METHODS =====

		public async Task<Document.DocumentEntete> GetDocumentByNumber(string pieceNumber)
		{
			var docEntete = await _context.F_DOCENTETE
				.FirstOrDefaultAsync(d => d.DO_Piece == pieceNumber);

			if (docEntete == null)
				return null;

			var docLignes = await _context.F_DOCLIGNE
				.Where(d => d.DO_Piece == pieceNumber)
				.OrderBy(d => d.DL_Ligne)
				.ToListAsync();

			return ConvertToSimpleDocument(docEntete, docLignes);
		}

		public async Task<List<Document.DocumentEntete>> GetDocumentsByDateRange(DocumentTypeDO? typeDO, DateTime startDate, DateTime endDate)
		{
			IQueryable<F_DOCENTETE> query = _context.F_DOCENTETE
				.Where(d => d.DO_Date >= startDate && d.DO_Date <= endDate);

			if (typeDO.HasValue)
			{
				short domaine = DocumentTypeHelper.GetDomaineFromDocumentTypeDO(typeDO.Value);
				short doType = DocumentTypeHelper.GetDONumberFromDocumentTypeDO(typeDO.Value);
				query = query.Where(d => d.DO_Domaine == domaine && d.DO_Type == doType);
			}

			var docEntetes = await query
				.OrderByDescending(d => d.DO_Date)
				.ThenBy(d => d.DO_Piece)
				.ToListAsync();

			var result = new List<Document.DocumentEntete>();

			foreach (var docEntete in docEntetes)
			{
				var docLignes = await _context.F_DOCLIGNE
					.Where(d => d.DO_Piece == docEntete.DO_Piece)
					.OrderBy(d => d.DL_Ligne)
					.ToListAsync();

				result.Add(ConvertToSimpleDocument(docEntete, docLignes));
			}

			return result;
		}

		private Document.DocumentEntete ConvertToSimpleDocument(F_DOCENTETE docEntete, List<F_DOCLIGNE> docLignes)
		{
			// Convert DO_Domaine and DO_Type to DocumentTypeDO
			DocumentTypeDO typeDO = DocumentTypeHelper.GetDocumentTypeDOFromDONumbers(
				docEntete.DO_Domaine ?? 0,
				docEntete.DO_Type ?? 0);

			var simpleDoc = new Document.DocumentEntete
			{
				Date = docEntete.DO_Date ?? DateTime.Now,
				Numero = docEntete.DO_Piece,
				Reference = docEntete.DO_Ref,
				Tiers = docEntete.DO_Tiers,
				Affaire = docEntete.CO_No?.ToString(),
				Depot = docEntete.DE_No ?? 0,
				Souche = docEntete.DO_Souche ?? 0,
				Type = typeDO
			};

			foreach (var ligne in docLignes)
			{
				simpleDoc.Lignes.Add(new Document.DocumentLigne
				{
					RefArticle = ligne.AR_Ref,
					Designation = ligne.DL_Design,
					Qte = ligne.DL_Qte ?? 0,
					PU = ligne.DL_PrixUnitaire ?? 0,
					CodeTVA = ligne.DL_CodeTaxe1,
					// Optionally set Type from ligne if different from document
					Type = simpleDoc.Type
				});
			}

			return simpleDoc;
		}
	}


}