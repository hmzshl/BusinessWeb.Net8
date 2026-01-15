using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Enum;
using BusinessWeb.Models.PersoAPI;
using BusinessWeb.Pages.Traitement.Outils.Tracabilite;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BusinessWeb.Models.PersoAPI.Document;

namespace BusinessWeb.Services
{
	public class DocumentService
	{
		private readonly DB _context;

		public DocumentService(DB context)
		{
			_context = context;
		}

		/// <summary>
		/// Creates a complete document with entête and lignes
		/// </summary>
		public async Task<DocumentCreateResult> CreateDocument(Document.DocumentEntete document)
		{
			var result = new DocumentCreateResult();

			try
			{
				// 1. Create the document entête
				document.Numero = GetCurrentPieceNumber(document);
				var fDocEntete = await CreateDocEntete(document);

				// 2. Update result
				result.Success = true;
				result.DocumentNumber = fDocEntete.DO_Piece;
				result.DocEnteteId = fDocEntete.cbMarq;

				result.Message = "Document créé avec succès";
				await UpdateCurrentPieceNumberInTable(document);


				try
				{
					// 3. Create the document lignes
					var fDocLignes = await CreateDocLignes(document.Lignes, fDocEntete, document.documentType.Mvt);
					result.LigneIds = fDocLignes.Select(l => l.cbMarq).ToList();
					result.Message += $", avec {fDocLignes.Count} lignes.";

					try
					{
						await UpdateTotals(fDocEntete);
					}
					catch (Exception ex)
					{
						result.Message += $" Cependant, une erreur est survenue lors de la mise à jour des totaux: {ex.Message}";
					}
				}
				catch (Exception ex)
				{
					result.Message += $" Cependant, une erreur est survenue lors de la création des lignes: {ex.Message}";
				}

			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = $"Erreur lors de la création du document: {ex.Message}";
				result.Error = ex;
			}

			return result;
		}
		public string GetCurrentPieceNumber(DocumentEntete document)
		{
			string pieceNumber = string.Empty;
			var dt = _context.F_DOCCURRENTPIECE.Where(a => a.DC_Souche == document.Souche && a.DC_IdCol == (int)document.documentType.DC_Id &&
			a.DC_Domaine == (short?)(document.documentType.Domaine));
			if (dt.Any())
			{
				pieceNumber = dt.First().DC_Piece;
			}
			else
			{
				pieceNumber = "00001";
			}
			var allPieces = GetAllPieces(document);
			if (allPieces.Any() && allPieces.Contains(pieceNumber))
			{
				pieceNumber = IncrementPieceNumber(allPieces.Max());
			}

			return pieceNumber;
		}
		public List<string> GetAllPieces(DocumentEntete document)
		{
			var pieceNumbers = new List<string>();
			var dt1 = _context.F_DOCENTETE
				.Where(a => a.DO_Type == (short?)document.TypeDO && a.DO_Domaine == (short?)(document.documentType.Domaine))
				.Select(a => a.DO_Piece)
				.ToList();

			var dt2 = _context.F_DOCLIGNE
			.Where(a => a.DO_Type == (short?)document.TypeDO && a.DO_Domaine == (short?)(document.documentType.Domaine))
			.Select(a => a.DO_Piece)
			.Distinct()
			.ToList();

			pieceNumbers.AddRange(dt1);
			pieceNumbers.AddRange(dt2);

			return pieceNumbers;
		}
		private async Task UpdateCurrentPieceNumberInTable(DocumentEntete document)
		{
			var currentPiece = _context.F_DOCCURRENTPIECE
				.FirstOrDefault(d =>
					d.DC_Domaine == (short?)(document.documentType.Domaine) &&
					d.DC_IdCol == (int)document.documentType.DC_Id &&
					d.DC_Souche == document.Souche);

			if (currentPiece != null)
			{
				currentPiece.DC_Piece = IncrementPieceNumber(document.Numero);
				_context.SaveChanges();
			}
			else
			{
				var newCurrentPiece = new F_DOCCURRENTPIECE
				{
					DC_Domaine = (short?)(document.documentType.Domaine),
					DC_IdCol = (short?)document.documentType.DC_Id,
					DC_Souche = document.Souche,
					DC_Piece = IncrementPieceNumber(document.Numero),
					cbCreateur = "BWB",
					cbProt = 0,
					cbReplication = 0,
					cbFlag = 0
				};
				await _context.F_DOCCURRENTPIECE.AddAsync(newCurrentPiece);
				await _context.SaveChangesAsync();
			}
		}

		public string IncrementPieceNumber(string currentPiece)
		{
			if (string.IsNullOrEmpty(currentPiece))
				return "00001";

			// Extract the numeric part
			var match = Regex.Match(currentPiece, @"\d+$");
			if (match.Success)
			{
				string prefix = currentPiece.Substring(0, match.Index);
				string numericPartStr = match.Value;

				if (long.TryParse(numericPartStr, out long numericPart))
				{
					numericPart++;
					return prefix + numericPart.ToString(new string('0', numericPartStr.Length));
				}
			}

			// If no numeric part found, append "00001"
			return currentPiece + "00001";
		}
		/// <summary>
		/// Creates document entête
		/// </summary>
		public async Task<F_DOCENTETE> CreateDocEntete(Document.DocumentEntete document)
		{
			var now = DateTime.Now;

			// Create the basic entity
			var fDocEntete = new F_DOCENTETE
			{
				// Required fields from input
				DO_Date = document.Date,
				DO_Ref = document.Reference ?? String.Empty,
				DO_Tiers = document.Tiers,
				DO_Souche = document.Souche,
				// External references
				DE_No = document.Depot,
				DO_Type = (short?)document.TypeDO,
				DO_Domaine = (short?)(document.documentType.Domaine),
				DO_DocType = (short?)document.TypeDO,
				DO_Piece = document.Numero,
				DO_DateLivr = document.DateLivraison,
				DO_Coord01 = document.Entete1 ?? string.Empty,
				DO_Coord02 = document.Entete2 ?? string.Empty,
				DO_Coord03 = document.Entete3 ?? string.Empty,
				DO_Coord04 = document.Entete4 ?? string.Empty,
				CA_Num = document.Affaire,
				DO_Statut = document.Statut


			};
			var de = _context.F_DEPOT.FirstOrDefault(d => d.DE_No == d.DE_Principal);
			if (de != null)
			{
				fDocEntete.DE_No = (fDocEntete.DE_No ?? de.DE_No) ?? 1;
			}
			var ct = _context.F_COMPTET.FirstOrDefault(c => c.CT_Num == fDocEntete.DO_Tiers);
			if (ct != null)
			{
				fDocEntete.N_CatCompta = ct.N_CatCompta ?? 1;
				fDocEntete.CA_Num = fDocEntete.CA_Num ?? ct.CA_Num ?? string.Empty;
				fDocEntete.CO_No = ct.CO_No ?? 0;
				fDocEntete.DO_Devise = ct.N_Devise ?? 0;
				fDocEntete.DO_Tarif = ct.N_CatTarif ?? 1;
				fDocEntete.CG_Num = ct.CG_NumPrinc ?? string.Empty;
			}
			var li = _context.F_LIVRAISON.FirstOrDefault(a => a.CT_Num == document.Tiers);
			if (li != null)
			{
				fDocEntete.LI_No = li.LI_No;
				fDocEntete.cbLI_No = li.LI_No;
			}
			// Set default values
			SetDocEnteteDefaults(fDocEntete);

			// Insert using BulkCopy
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(new List<F_DOCENTETE> { fDocEntete });

			return fDocEntete;
		}

		/// <summary>
		/// Sets default values for F_DOCENTETE
		/// </summary>
		public void SetDocEnteteDefaults(F_DOCENTETE docEntete)
		{
			var now = DateTime.Now;
			var defaultDate = new DateTime(1753, 1, 1);

			// Set nullable fields with defaults only if null
			docEntete.CO_No = docEntete.CO_No ?? 0;
			docEntete.DE_No = docEntete.DE_No ?? 1;
			docEntete.DO_Statut = docEntete.DO_Statut ?? 2;
			docEntete.N_CatCompta = docEntete.N_CatCompta ?? 1;
			docEntete.DO_Tarif = docEntete.DO_Tarif ?? 1;
			docEntete.DO_Domaine = docEntete.DO_Domaine ?? 0;
			docEntete.DO_Type = docEntete.DO_Type ?? 0;
			docEntete.DO_Period = docEntete.DO_Period ?? 1;
			docEntete.DO_Devise = docEntete.DO_Devise ?? 0;
			docEntete.DO_Cours = docEntete.DO_Cours ?? 0;
			docEntete.DO_Provenance = docEntete.DO_Provenance ?? 0;
			docEntete.DO_EStatut = docEntete.DO_EStatut ?? 0;

			// Set mandatory defaults only if null
			docEntete.DO_Regime = docEntete.DO_Regime ?? 21;
			docEntete.DO_Transaction = docEntete.DO_Transaction ?? 11;
			docEntete.DO_Expedit = docEntete.DO_Expedit ?? 1;
			docEntete.DO_Condition = docEntete.DO_Condition ?? 1;
			docEntete.DO_TypeColis = docEntete.DO_TypeColis ?? 1;

			// Set dates only if null
			docEntete.DO_DateLivr = docEntete.DO_DateLivr ?? defaultDate;
			docEntete.DO_DebutAbo = docEntete.DO_DebutAbo ?? defaultDate;
			docEntete.DO_FinAbo = docEntete.DO_FinAbo ?? defaultDate;
			docEntete.DO_DebutPeriod = docEntete.DO_DebutPeriod ?? defaultDate;
			docEntete.DO_FinPeriod = docEntete.DO_FinPeriod ?? defaultDate;
			docEntete.DO_DateLivrRealisee = docEntete.DO_DateLivrRealisee ?? defaultDate;
			docEntete.DO_DateExpedition = docEntete.DO_DateExpedition ?? defaultDate;

			// Set string fields only if null
			docEntete.DO_Ref = docEntete.DO_Ref ?? string.Empty;
			docEntete.DO_Heure = docEntete.DO_Heure ?? now.ToString("HHmmss");
			docEntete.DO_Motif = docEntete.DO_Motif ?? string.Empty;
			docEntete.DO_Contact = docEntete.DO_Contact ?? string.Empty;
			docEntete.DO_FactureFrs = docEntete.DO_FactureFrs ?? string.Empty;
			docEntete.DO_PieceOrig = docEntete.DO_PieceOrig ?? "            ";
			docEntete.DO_NoWeb = docEntete.DO_NoWeb ?? string.Empty;
			docEntete.CA_NumIFRS = docEntete.CA_NumIFRS ?? string.Empty;
			docEntete.CT_NumOld = docEntete.CT_NumOld ?? string.Empty;
			docEntete.ChefChantier = docEntete.ChefChantier ?? string.Empty;
			docEntete.Demandeur = docEntete.Demandeur ?? string.Empty;
			docEntete.DO_AdressePaiement = docEntete.DO_AdressePaiement ?? string.Empty;
			docEntete.CA_Num = docEntete.CA_Num ?? string.Empty;
			docEntete.DO_Coord01 = docEntete.DO_Coord01 ?? string.Empty;
			docEntete.DO_Coord02 = docEntete.DO_Coord02 ?? string.Empty;
			docEntete.DO_Coord03 = docEntete.DO_Coord03 ?? string.Empty;
			docEntete.DO_Coord04 = docEntete.DO_Coord04 ?? string.Empty;

			// Set numeric defaults only if null
			docEntete.DO_NbFacture = docEntete.DO_NbFacture ?? 1;
			docEntete.DO_BLFact = docEntete.DO_BLFact ?? 0;
			docEntete.DO_TxEscompte = docEntete.DO_TxEscompte ?? 0.000000m;
			docEntete.DO_Imprim = docEntete.DO_Imprim ?? 0;
			docEntete.DO_Reliquat = docEntete.DO_Reliquat ?? 0;
			docEntete.DO_ValFrais = docEntete.DO_ValFrais ?? 0.000000m;
			docEntete.DO_ValFranco = docEntete.DO_ValFranco ?? 0.000000m;
			docEntete.DO_Taxe1 = docEntete.DO_Taxe1 ?? 0.000000m;
			docEntete.DO_Escompte = docEntete.DO_Escompte ?? 0;
			docEntete.CA_No = docEntete.CA_No ?? 0;
			docEntete.CO_NoCaissier = docEntete.CO_NoCaissier ?? 0;
			docEntete.MR_No = docEntete.MR_No ?? 0;
			docEntete.ET_No = docEntete.ET_No ?? 0;

			// Set additional defaults only if null
			docEntete.DO_Langue = docEntete.DO_Langue ?? 0;
			docEntete.DO_Ecart = docEntete.DO_Ecart ?? 0;
			docEntete.DO_Ventile = docEntete.DO_Ventile ?? 0;
			docEntete.DO_Transfere = docEntete.DO_Transfere ?? 0;
			docEntete.DO_Cloture = docEntete.DO_Cloture ?? 0;
			docEntete.DO_Attente = docEntete.DO_Attente ?? 0;
			docEntete.DO_TypeFrais = docEntete.DO_TypeFrais ?? 0;
			docEntete.DO_TypeLigneFrais = docEntete.DO_TypeLigneFrais ?? 0;
			docEntete.DO_TypeFranco = docEntete.DO_TypeFranco ?? 0;
			docEntete.DO_TypeLigneFranco = docEntete.DO_TypeLigneFranco ?? 0;
			docEntete.DO_TypeTaux1 = docEntete.DO_TypeTaux1 ?? 0;
			docEntete.DO_TypeTaxe1 = docEntete.DO_TypeTaxe1 ?? 0;
			docEntete.DO_Taxe2 = docEntete.DO_Taxe2 ?? 0;
			docEntete.DO_TypeTaux2 = docEntete.DO_TypeTaux2 ?? 0;
			docEntete.DO_TypeTaxe2 = docEntete.DO_TypeTaxe2 ?? 0;
			docEntete.DO_Taxe3 = docEntete.DO_Taxe3 ?? 0;
			docEntete.DO_TypeTaux3 = docEntete.DO_TypeTaux3 ?? 0;
			docEntete.DO_TypeTaxe3 = docEntete.DO_TypeTaxe3 ?? 0;
			docEntete.DO_MajCpta = docEntete.DO_MajCpta ?? 0;
			docEntete.DO_FactureElec = docEntete.DO_FactureElec ?? 0;
			docEntete.DO_TypeTransac = docEntete.DO_TypeTransac ?? 0;
			docEntete.DO_DemandeRegul = docEntete.DO_DemandeRegul ?? 0;
			docEntete.DO_Valide = docEntete.DO_Valide ?? 0;
			docEntete.DO_Coffre = docEntete.DO_Coffre ?? 0;
			docEntete.DO_StatutBAP = docEntete.DO_StatutBAP ?? 0;
			docEntete.DO_DocType = docEntete.DO_DocType ?? 0;
			docEntete.DO_TypeCalcul = docEntete.DO_TypeCalcul ?? 0;
			docEntete.DO_PaiementLigne = docEntete.DO_PaiementLigne ?? 0;
			docEntete.DO_MotifDevis = docEntete.DO_MotifDevis ?? 0;

			// Set monetary fields only if null
			docEntete.DO_TotalHT = docEntete.DO_TotalHT ?? 0;
			docEntete.DO_TotalHTNet = docEntete.DO_TotalHTNet ?? 0;
			docEntete.DO_TotalTTC = docEntete.DO_TotalTTC ?? 0;
			docEntete.DO_NetAPayer = docEntete.DO_NetAPayer ?? 0;
			docEntete.DO_MontantRegle = docEntete.DO_MontantRegle ?? 0;

			// Set audit fields only if null
			docEntete.cbCreateur = docEntete.cbCreateur ?? "BWB";
			docEntete.cbProt = docEntete.cbProt ?? 0;
			docEntete.cbReplication = docEntete.cbReplication ?? 0;
			docEntete.cbFlag = docEntete.cbFlag ?? 0;
			docEntete.cbCO_No = docEntete.CO_No;
			docEntete.cbDE_No = docEntete.DE_No;
			docEntete.LI_No = docEntete.LI_No ?? 0;
			docEntete.cbLI_No = docEntete.LI_No ?? 0;
			docEntete.CT_NumPayeur = docEntete.DO_Tiers;
			docEntete.DO_Colisage = docEntete.DO_Colisage ?? 1;
			docEntete.AB_No = docEntete.AB_No ?? 0;


			if (docEntete.DO_Domaine > 1)
			{
				if (docEntete.DO_Type != 23)
				{
					docEntete.DE_No = 0;
					docEntete.CO_No = 0;
					docEntete.cbDE_No = null;
					docEntete.cbCO_No = null;
					docEntete.CT_NumPayeur = null;
					docEntete.CT_NumPayeur = null;
					docEntete.LI_No = 0;
					docEntete.cbLI_No = null;
					docEntete.DO_Tarif = 0;
					docEntete.DO_Devise = 0;
					docEntete.DO_Cours = 0;
					docEntete.DO_Condition = 0;
					docEntete.DO_Expedit = 0;
					docEntete.DO_Regime = 0;
					docEntete.DO_Transaction = 0;
					docEntete.DO_Regime = 0;
					docEntete.N_CatCompta = 0;
					docEntete.DO_Period = 0;
					docEntete.DO_NbFacture = 0;



				}
			}

		}

		/// <summary>
		/// Creates document lignes
		/// </summary>
		public async Task<List<F_DOCLIGNE>> CreateDocLignes(List<Document.DocumentLigne> lignes, F_DOCENTETE entete, short? Mvt)
		{
			var fDocLignes = new List<F_DOCLIGNE>();
			var now = DateTime.Now;
			int DL_No = (_context.F_DOCLIGNE.Max(a => a.DL_No) + 1) ?? 0;
			int DL_Ligne = 1000;
			foreach (var ligne in lignes)
			{
				DefaultsAR defaultsAR = GetDefaultsAR(entete, ligne.RefArticle);
				var fDocLigne = new F_DOCLIGNE
				{
					// Document identification
					DO_Piece = entete.DO_Piece,
					DO_Date = entete.DO_Date,
					DO_Type = entete.DO_Type ?? 0,
					DO_DocType = entete.DO_Type ?? 0,
					DO_Domaine = entete.DO_Domaine ?? 0,
					EU_Enumere = defaultsAR.Unite,
					CT_Num = entete.DO_Tiers,
					DE_No = defaultsAR.DE_No,
					DO_Ref = entete.DO_Ref,
					CA_Num = entete.CA_Num,
					CO_No = entete.CO_No ?? 0,
					DL_MvtStock = Mvt,
					// Line information
					DL_Ligne = DL_Ligne,
					AR_Ref = ligne.RefArticle,
					DL_Design = ligne.Designation ?? defaultsAR.Designation,
					DL_Qte = ligne.Qte,
					DL_PrixUnitaire = ligne.PU ?? defaultsAR.PU,
					DL_PUBC = ligne.PU ?? defaultsAR.PU,
					DL_Taxe1 = ligne.TauxTaxe ?? defaultsAR.TauxTaxe,
					DL_No = DL_No,
					DO_DateLivr = entete.DO_DateLivr,
					DL_CodeTaxe1 = ligne.CodeTVA ?? defaultsAR.CodeTaxe,

					// Audit fields
					cbProt = 0,
					cbCreateur = "BWB",
					cbReplication = 0,
					cbFlag = 0,
					cbCreation = now,
					DL_CMUP = defaultsAR.CMUP ?? 0,
					DL_PrixRU = defaultsAR.CMUP ?? 0
				};




				// Set default values
				SetDocLigneDefaults(fDocLigne);

				// Calculate line amounts
				fDocLigne.DL_PUTTC = fDocLigne.DL_PrixUnitaire + (((fDocLigne.DL_Taxe1 ?? 0) / 100) * fDocLigne.DL_PrixUnitaire);
				fDocLigne.DL_MontantHT = fDocLigne.DL_Qte * fDocLigne.DL_PrixUnitaire;
				fDocLigne.DL_MontantTTC = fDocLigne.DL_MontantHT + (((fDocLigne.DL_Taxe1 ?? 0) / 100) * fDocLigne.DL_MontantHT);

				fDocLignes.Add(fDocLigne);
				DL_No = DL_No + 1;
				DL_Ligne = DL_Ligne + 1000;
			}

			// Insert all lines using BulkCopy
			if (fDocLignes.Any())
			{
				using var connection = _context.CreateLinqToDBConnection();
				await connection.BulkCopyAsync(fDocLignes);
			}
			foreach (var ligne in fDocLignes)
			{
				if (ligne.AR_Ref != null && ligne.DE_No != null && ligne.DL_MvtStock != 0)
				{
					UpdateStock(ligne.AR_Ref, ligne.DE_No ?? 0);
				}
			}
			return fDocLignes;
		}
		public class DefaultsAR()
		{

			public decimal? PU { get; set; }
			public decimal? CMUP { get; set; }
			public int? DE_No { get; set; }
			public string Unite { get; set; }
			public string CodeTaxe { get; set; }
			public decimal? TauxTaxe { get; set; }
			public string Designation { get; set; }


		}
		public async Task UpdateTotals(F_DOCENTETE doc)
		{
			var lignes = _context.F_DOCLIGNE.Where(l => l.DO_Piece == doc.DO_Piece && l.DO_Type == doc.DO_Type && l.DL_Valorise == 1).ToList();
			var entete = _context.F_DOCENTETE.FirstOrDefault(e => e.DO_Piece == doc.DO_Piece && e.DO_Type == doc.DO_Type);
			entete.DO_TotalHT = lignes.Sum(l => l.DL_MontantHT) ?? 0;
			entete.DO_TotalHTNet = entete.DO_TotalHT;
			entete.DO_TotalTTC = lignes.Sum(l => l.DL_MontantTTC) ?? 0;
			entete.DO_NetAPayer = entete.DO_TotalTTC;
			_context.SaveChanges();
		}
		public void UpdateLigneTotals(F_DOCLIGNE ligne)
		{
			ligne.DL_MontantHT = ligne.DL_Qte * ligne.DL_PrixUnitaire;
			ligne.DL_MontantTTC = ligne.DL_MontantHT + (((ligne.DL_Taxe1 ?? 0) / 100) * ligne.DL_MontantHT);
		}
		public void UpdateStock(string AR_Ref, int DE_No)
		{

			// Calculate stock: Entree (1) adds to stock, Sortie (3) subtracts from stock
			var stock = _context.F_DOCLIGNE
				.Where(a => a.AR_Ref == AR_Ref && a.DE_No == DE_No)
				.Sum(a => a.DL_MvtStock == 1 ? a.DL_Qte :
						  a.DL_MvtStock == 3 ? -a.DL_Qte : 0);
			// Calculate total value and total quantity for entries only
			var entries = _context.F_DOCLIGNE
				.Where(a => a.AR_Ref == AR_Ref && a.DE_No == DE_No && a.DL_MvtStock == 1)
				.GroupBy(a => 1)
				.Select(g => new
				{
					TotalQte = g.Sum(x => x.DL_Qte),
					TotalMontant = g.Sum(x => x.DL_MontantHT)
				})
				.FirstOrDefault();

			decimal? cmup = 0;
			if (entries != null && entries.TotalQte > 0)
			{
				cmup = entries.TotalMontant / entries.TotalQte;
			}
			var articleDepot = _context.F_ARTSTOCK.Where(a => a.AR_Ref == AR_Ref && a.DE_No == DE_No).FirstOrDefault();
			if (articleDepot != null)
			{
				articleDepot.AS_QteSto = stock ?? 0;
				articleDepot.AS_MontSto = cmup * stock;
				_context.SaveChanges();
			}
			else
			{
				var newArticleDepot = new F_ARTSTOCK
				{
					AR_Ref = AR_Ref,
					DE_No = DE_No,
					AS_QteSto = stock ?? 0,
					AS_MontSto = cmup * stock,
					AS_QteAControler = 0,
					AS_QteCom = 0,
					AS_QteComCM = 0,
					AS_QtePrepa = 0,
					AS_QteRes = 0,
					AS_QteResCM = 0,
					AS_QteMaxi = 0,
					AS_QteMini = 0,
					AS_Mouvemente = 0,
					AS_Principal = 0,
					DP_NoControle = null,
					cbCreateur = "BWB",
					cbProt = 0,
					cbReplication = 0,
					cbFlag = 0
				};
				_context.F_ARTSTOCK.Add(newArticleDepot);
				_context.SaveChanges();
			}
		}
		public DefaultsAR GetDefaultsAR(F_DOCENTETE entete, string AR_Ref)
		{
			DefaultsAR rs = new DefaultsAR();
			var ar = _context.F_ARTICLE.FirstOrDefault(a => a.AR_Ref == AR_Ref);
			if (ar != null)
			{
				if (entete.DO_Domaine == 0)
				{
					rs.PU = ar.AR_PrixVen ?? 0;
				}
				else
				{
					rs.PU = ar.AR_PrixAch ?? 0;

				}
				rs.Designation = ar.AR_Design;
				if (ar.AR_SuiviStock == 0)
				{
					rs.DE_No = 0;
				}
				else
				{
					var de = _context.F_DEPOT.FirstOrDefault(d => d.DP_NoDefaut == 1);
					if (de != null)
					{
						rs.DE_No = de.DE_No;
					}
				}

				var un = _context.P_UNITE.FirstOrDefault(u => u.cbIndice == ar.AR_UniteVen);
				if (un != null)
				{
					rs.Unite = un.U_Intitule;
				}
				if (entete.DO_Domaine == 0)
				{
					var cl = _context.F_ARTCLIENT.FirstOrDefault(c => c.CT_Num == entete.DO_Tiers && c.AC_Categorie == entete.DO_Tarif);
					if (cl != null)
					{
						rs.PU = (cl.AC_PrixVen != 0) ? cl.AC_PrixVen : rs.PU;
					}

				}
				if (entete.DO_Domaine == 1)
				{
					var fr = _context.F_ARTFOURNISS.FirstOrDefault(c => c.CT_Num == entete.DO_Tiers);
					if (fr != null)
					{
						rs.PU = (fr.AF_PrixAch != 0) ? fr.AF_PrixAch : rs.PU;
						var un1 = _context.P_UNITE.FirstOrDefault(u => u.cbIndice == fr.AF_Unite);
						if (un1 != null)
						{
							rs.Unite = un1.U_Intitule;
						}
					}
				}

				var tx = _context.API_V_ARTCOMPTA.FirstOrDefault(t => t.AR_Ref == AR_Ref && t.Type == entete.DO_Domaine && t.Champ == entete.N_CatCompta);
				if (tx != null)
				{
					rs.CodeTaxe = tx.CodeTaxe1;
					rs.TauxTaxe = tx.Taux1;
				}
			}
			else
			{
				rs.DE_No = entete.DE_No ?? 0;
			}
			if (AR_Ref == null)
			{
				rs.DE_No = 0;
			}
			var cmup = _context.F_ARTSTOCK.FirstOrDefault(a => a.AR_Ref == AR_Ref && a.DE_No == entete.DE_No);
			if (cmup != null)
			{
				rs.CMUP = cmup.AS_MontSto / (cmup.AS_QteSto == 0 ? 1 : cmup.AS_QteSto);
				if(entete.DO_Domaine > 1)
				{
					rs.PU = rs.CMUP;
				}
			}
			return rs;
		}       /// <summary>
				/// Sets default values for F_DOCLIGNE
				/// </summary>
		public void SetDocLigneDefaults(F_DOCLIGNE docLigne)
		{
			var defaultDate = new DateTime(1753, 1, 1);
			// Set defaults for null numeric fields
			docLigne.DO_Domaine = docLigne.DO_Domaine ?? 0;
			docLigne.DL_Qte = docLigne.DL_Qte ?? 0;
			docLigne.DL_PrixUnitaire = docLigne.DL_PrixUnitaire ?? 0;
			docLigne.DL_PUBC = docLigne.DL_PUBC ?? 0;
			docLigne.DL_Taxe1 = docLigne.DL_Taxe1 ?? 0;
			docLigne.DL_Taxe2 = docLigne.DL_Taxe2 ?? 0;
			docLigne.DL_Taxe3 = docLigne.DL_Taxe3 ?? 0;
			docLigne.DL_MontantHT = docLigne.DL_MontantHT ?? 0;
			docLigne.DL_MontantTTC = docLigne.DL_MontantTTC ?? 0;

			// Note: Removed DL_QteDE = 0, DL_QtePL = 0, DE_No = 1 - these will only be set if null
			docLigne.DL_QteDE = docLigne.DL_QteDE ?? 0;
			docLigne.DL_QtePL = docLigne.DL_QtePL ?? 0;

			// Set string fields only if null
			docLigne.DL_Design = docLigne.DL_Design ?? string.Empty;
			docLigne.DL_PieceBC = docLigne.DL_PieceBC ?? string.Empty;
			docLigne.DL_PieceBL = docLigne.DL_PieceBL ?? string.Empty;
			docLigne.DL_PiecePL = docLigne.DL_PiecePL ?? string.Empty;
			docLigne.DL_PieceDE = docLigne.DL_PieceDE ?? string.Empty;
			docLigne.AC_RefClient = docLigne.AC_RefClient ?? string.Empty;
			docLigne.RP_Code = docLigne.RP_Code ?? string.Empty;
			docLigne.PF_Num = docLigne.PF_Num ?? string.Empty;
			docLigne.DL_Operation = docLigne.DL_Operation ?? string.Empty;

			// Set other nullable fields
			docLigne.EU_Qte = docLigne.EU_Qte ?? docLigne.DL_Qte;
			docLigne.DL_QteBC = docLigne.DL_QteBC ?? docLigne.DL_Qte;
			docLigne.DL_QteBL = docLigne.DL_QteBL ?? 0;
			docLigne.CO_No = docLigne.CO_No ?? 0;
			docLigne.AG_No1 = docLigne.AG_No1 ?? 0;
			docLigne.AG_No2 = docLigne.AG_No2 ?? 0;
			docLigne.DL_PrixRU = docLigne.DL_PrixRU ?? 0;

			// Note: Removed DL_CMUP = 0, DL_MvtStock = 0, etc. - these will only be set if null
			docLigne.DL_CMUP = docLigne.DL_CMUP ?? 0;
			docLigne.DL_MvtStock = docLigne.DL_MvtStock ?? 0;
			docLigne.DL_TTC = docLigne.DL_TTC ?? 0;
			docLigne.DL_PUTTC = docLigne.DL_PUTTC ?? 0;
			docLigne.DL_Frais = docLigne.DL_Frais ?? 0;
			docLigne.DL_Valorise = docLigne.DL_Valorise ?? 1;

			// Set dates only if null
			docLigne.DL_DateBC = docLigne.DL_DateBC ?? defaultDate;
			docLigne.DL_DateBL = docLigne.DL_DateBL ?? defaultDate;
			docLigne.DL_DatePL = docLigne.DL_DatePL ?? defaultDate;
			docLigne.DL_DateDE = docLigne.DL_DateDE ?? docLigne.DO_Date;
			docLigne.DL_DateAvancement = docLigne.DL_DateAvancement ?? defaultDate;

			// Set numeric defaults only if null
			docLigne.DL_PoidsNet = docLigne.DL_PoidsNet ?? 0;
			docLigne.DL_PoidsBrut = docLigne.DL_PoidsBrut ?? 0;
			docLigne.DL_Remise01REM_Valeur = docLigne.DL_Remise01REM_Valeur ?? 0;
			docLigne.DL_Remise01REM_Type = docLigne.DL_Remise01REM_Type ?? 0;
			docLigne.DL_Remise02REM_Valeur = docLigne.DL_Remise02REM_Valeur ?? 0;
			docLigne.DL_Remise02REM_Type = docLigne.DL_Remise02REM_Type ?? 0;
			docLigne.DL_Remise03REM_Valeur = docLigne.DL_Remise03REM_Valeur ?? 0;
			docLigne.DL_Remise03REM_Type = docLigne.DL_Remise03REM_Type ?? 0;
			docLigne.DL_TypeTaux1 = docLigne.DL_TypeTaux1 ?? 0;
			docLigne.DL_TypeTaxe1 = docLigne.DL_TypeTaxe1 ?? 0;
			docLigne.DL_TypeTaux2 = docLigne.DL_TypeTaux2 ?? 0;
			docLigne.DL_TypeTaxe2 = docLigne.DL_TypeTaxe2 ?? 0;
			docLigne.DL_TypeTaux3 = docLigne.DL_TypeTaux3 ?? 0;
			docLigne.DL_TypeTaxe3 = docLigne.DL_TypeTaxe3 ?? 0;
			docLigne.DL_NoRef = docLigne.DL_NoRef ?? 1;
			docLigne.DL_TypePL = docLigne.DL_TypePL ?? 0;
			docLigne.DL_PUDevise = docLigne.DL_PUDevise ?? 0;
			docLigne.DL_NonLivre = docLigne.DL_NonLivre ?? 0;
			docLigne.DL_FactPoids = docLigne.DL_FactPoids ?? 0;
			docLigne.DL_Escompte = docLigne.DL_Escompte ?? 0;
			docLigne.DL_QteRessource = docLigne.DL_QteRessource ?? 0;
			docLigne.DL_NoSousTotal = docLigne.DL_NoSousTotal ?? 0;
			docLigne.DL_TNomencl = docLigne.DL_TNomencl ?? 0;
			docLigne.DL_TRemPied = docLigne.DL_TRemPied ?? 0;
			docLigne.DL_TRemExep = docLigne.DL_TRemExep ?? 0;
			docLigne.DT_No = docLigne.DT_No ?? 0;
			docLigne.CA_No = docLigne.CA_No ?? 0;
			docLigne.AF_RefFourniss = docLigne.AF_RefFourniss ?? string.Empty;
			docLigne.DL_NoColis = docLigne.DL_NoColis ?? string.Empty;
			docLigne.DL_NoLink = docLigne.DL_NoLink ?? 0;
			docLigne.DL_PieceOFProd = docLigne.DL_PieceOFProd ?? 0;
			docLigne.cbCO_No = docLigne.CO_No;
			docLigne.cbDE_No = docLigne.DE_No;


			// Set audit fields only if null
			docLigne.cbCreateur = docLigne.cbCreateur ?? "BWB";
			docLigne.cbModification = docLigne.cbModification ?? DateTime.Now;
			docLigne.cbCreation = docLigne.cbCreation ?? DateTime.Now;

			if (docLigne.AR_Ref == null)
			{
				docLigne.DL_MvtStock = 0;
				docLigne.DE_No = 0;
				docLigne.EU_Enumere = string.Empty;
				docLigne.CO_No = 0;
				docLigne.cbCO_No = 0;
			}
			if (docLigne.DO_Domaine > 1)
			{
				docLigne.DL_DateBL = docLigne.DO_Date;
				docLigne.DL_CMUP = docLigne.DL_PrixUnitaire;
				docLigne.DL_PrixRU = docLigne.DL_PrixUnitaire;
			}

		}

		// ===== HELPER METHODS =====

		public async Task<string> GenerateDocumentNumber()
		{
			// Simple document number generation - you might want to implement your own logic
			var maxNumber = _context.F_DOCENTETE
				.Max(e => (int?)Convert.ToInt32(e.DO_Piece)) ?? 0;

			return (maxNumber + 1).ToString("D6");
		}

		public decimal? CalculateTVA(Document.DocumentLigne ligne)
		{
			if (string.IsNullOrEmpty(ligne.CodeTVA))
				return ligne.TotalHT * 0.20m;

			return ligne.CodeTVA.ToUpper() switch
			{
				"TVA20" or "20" or "20%" => ligne.TotalHT * 0.20m,
				"TVA10" or "10" or "10%" => ligne.TotalHT * 0.10m,
				"TVA5.5" or "5.5" or "5.5%" or "5,5" => ligne.TotalHT * 0.055m,
				"TVA0" or "0" or "0%" or "EXO" or "EXONERE" => 0,
				_ => ligne.TotalHT * 0.20m
			};
		}

		public decimal CalculateTotalTVA(List<Document.DocumentLigne> lignes)
		{
			return lignes.Sum(l => CalculateTVA(l) ?? 0);
		}
	}
}