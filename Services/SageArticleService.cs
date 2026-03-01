using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Perso;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BusinessWeb.Services
{
	public class SageArticleService
	{
		private readonly DB _context;

		public SageArticleService(DB context)
		{
			_context = context;
		}

		// ===== MÉTHODE PRINCIPALE : Ajouter un article =====
		public async Task<OperationResult<F_ARTICLE>> AddArticle(F_ARTICLE row)
		{
			var errors = new List<OperationError>();

			// Validation des champs obligatoires
			if (string.IsNullOrEmpty(row.AR_Ref))
				errors.Add(new OperationError
				{
					Code = "REQUIRED",
					Field = "AR_Ref",
					Message = "La référence article est obligatoire"
				});

			if (string.IsNullOrEmpty(row.AR_Design))
				errors.Add(new OperationError
				{
					Code = "REQUIRED",
					Field = "AR_Design",
					Message = "La désignation de l'article est obligatoire"
				});

			// Validate format if needed
			if (!string.IsNullOrEmpty(row.AR_Ref) && row.AR_Ref.Length > 20)
				errors.Add(new OperationError
				{
					Code = "INVALID_LENGTH",
					Field = "AR_Ref",
					Message = "La référence ne peut pas dépasser 20 caractères"
				});

			// Check if reference already exists
			if (!string.IsNullOrEmpty(row.AR_Ref))
			{
				var exists = await _context.F_ARTICLE.AnyAsync(a => a.AR_Ref == row.AR_Ref);
				if (exists)
					errors.Add(new OperationError
					{
						Code = "DUPLICATE",
						Field = "AR_Ref",
						Message = $"La référence '{row.AR_Ref}' existe déjà"
					});
			}

			// Return all validation errors if any
			if (errors.Any())
				return OperationResult<F_ARTICLE>.Failure(errors);

			try
			{
				// Définir les valeurs par défaut selon le contexte marocain
				SetDefaultValues(row);

				// Ajouter à la base de données avec LinqToDB
				using var db = _context.CreateLinqToDBConnection();
				await db.BulkCopyAsync(new List<F_ARTICLE> { row });

				return OperationResult<F_ARTICLE>.Success(row);
			}
			catch (Exception ex)
			{
				return OperationResult<F_ARTICLE>.Failure(
					$"Erreur lors de l'ajout de l'article: {ex.Message}",
					"DATABASE_ERROR"
				);
			}
		}

		// ===== AJOUTER PLUSIEURS ARTICLES =====
		public async Task<List<F_ARTICLE>> AddArticles(List<F_ARTICLE> rows)
		{
			var results = new List<F_ARTICLE>();

			// Vérifier les doublons en lot
			var existingRefs = await _context.F_ARTICLE
				.Where(a => rows.Select(r => r.AR_Ref).Contains(a.AR_Ref))
				.Select(a => a.AR_Ref)
				.ToListAsync();

			if (existingRefs.Any())
			{
				throw new ArgumentException($"Références dupliquées trouvées: {string.Join(", ", existingRefs)}");
			}

			// Vérifier les doublons dans la liste fournie
			var duplicateRefs = rows
				.GroupBy(r => r.AR_Ref)
				.Where(g => g.Count() > 1)
				.Select(g => g.Key)
				.ToList();

			if (duplicateRefs.Any())
			{
				throw new ArgumentException($"Références dupliquées dans la liste: {string.Join(", ", duplicateRefs)}");
			}

			foreach (var row in rows)
			{
				// Validation
				if (string.IsNullOrEmpty(row.AR_Ref))
					throw new ArgumentException($"Réference est obligatoire");

				if (string.IsNullOrEmpty(row.AR_Design))
					throw new ArgumentException($"Désignation est obligatoire");

				// Définir les valeurs par défaut
				SetDefaultValues(row);
				results.Add(row);
			}

			// Insertion en masse avec LinqToDB
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(results);

			return results;
		}

		// ===== DÉFINIR LES VALEURS PAR DÉFAUT POUR LE MAROC =====
		private void SetDefaultValues(F_ARTICLE row)
		{
			row.FA_CodeFamille = row.FA_CodeFamille ?? _context.F_FAMILLE.First()?.FA_CodeFamille; // Famille par défaut
														   // Type d'article par défaut (0 = Standard, 1 = Service, 2 = Matière première, etc.)
			row.AR_Type = row.AR_Type ?? 0;


			// Pays par défaut ()
			if (string.IsNullOrEmpty(row.AR_Pays))
				row.AR_Pays = "";

			// Code fiscal par défaut (TVA marocaine)
			if (string.IsNullOrEmpty(row.AR_CodeFiscal))
				row.AR_CodeFiscal = "";

			// Unités par défaut
			row.AR_UniteVen = row.AR_UniteVen ?? 1; // Unité de vente (1 = Pièce)
			row.AR_UnitePoids = row.AR_UnitePoids ?? 3; // Unité de poids (3 = Gramme)

			// Prix par défaut
			row.AR_PrixAch = row.AR_PrixAch ?? 0;
			row.AR_PrixVen = row.AR_PrixVen ?? 0;
			row.AR_PUNet = row.AR_PUNet ?? 0;
			row.AR_Coef = row.AR_Coef ?? 1; // Coefficient multiplicateur par défaut

			// Gestion des stocks par défaut
			row.AR_SuiviStock = row.AR_SuiviStock ?? 2; // 2 = Suivi de stock actif
			row.AR_Nomencl = row.AR_Nomencl ?? 0; // 0 = Pas de nomenclature
			row.AR_Sommeil = row.AR_Sommeil ?? 0; // 0 = Article actif

			// Dates par défaut
			var defaultDate = new DateTime(1753, 1, 1);
			row.AR_DateModif = DateTime.Now;
			row.AR_DateApplication = defaultDate;
			row.cbCreation = DateTime.Now;
			row.cbModification = DateTime.Now;

			// Raccourci par défaut (basé sur la référence si non fourni)
			if (string.IsNullOrEmpty(row.AR_Raccourci) && !string.IsNullOrEmpty(row.AR_Ref))
				row.AR_Raccourci = row.AR_Ref.Length > 6 ? row.AR_Ref.Substring(0, 6) : row.AR_Ref;

			// Champs statistiques par défaut pour le 
			SetDefaultStatFields(row);

			// Champs de frais par défaut
			SetDefaultFraisFields(row);

			// Flags et indicateurs par défaut
			SetDefaultFlags(row);

			// Champs binaires (cbAR_Ref, cbAR_Design, etc.) sont gérés automatiquement par la base de données
			// Ne pas les définir manuellement car ce sont des colonnes calculées ou gérées par le système
		}

		// ===== DÉFINIR LES CHAMPS STATISTIQUES POUR LE MAROC =====
		private void SetDefaultStatFields(F_ARTICLE row)
		{
			// Champs statistiques pour les rapports marocains
			if (string.IsNullOrEmpty(row.AR_Stat01))
				row.AR_Stat01 = ""; // Saison/Catégorie

			if (string.IsNullOrEmpty(row.AR_Stat02))
				row.AR_Stat02 = ""; // Type de produit

			if (string.IsNullOrEmpty(row.AR_Stat03))
				row.AR_Stat03 = ""; // Origine (Local/Importé)

			if (string.IsNullOrEmpty(row.AR_Stat04))
				row.AR_Stat04 = ""; // Secteur d'activité

			if (string.IsNullOrEmpty(row.AR_Stat05))
				row.AR_Stat05 = ""; // Marque/Catégorie fiscale
		}






		// ===== DÉFINIR LES CHAMPS DE FRAIS POUR LE MAROC =====
		private void SetDefaultFraisFields(F_ARTICLE row)
		{
			// Frais 01 - Coût de stockage
			if (string.IsNullOrEmpty(row.AR_Frais01FR_Denomination))
				row.AR_Frais01FR_Denomination = "";

			row.AR_Frais01FR_Rem01REM_Valeur = row.AR_Frais01FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais01FR_Rem01REM_Type = row.AR_Frais01FR_Rem01REM_Type ?? 1; // 1 = Pourcentage

			// Frais 02 - Transport/Douane
			if (string.IsNullOrEmpty(row.AR_Frais02FR_Denomination))
				row.AR_Frais02FR_Denomination = "";

			row.AR_Frais02FR_Rem01REM_Valeur = row.AR_Frais02FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais02FR_Rem01REM_Type = row.AR_Frais02FR_Rem01REM_Type ?? 1;

			// Frais 03 - Autres frais
			if (string.IsNullOrEmpty(row.AR_Frais03FR_Denomination))
				row.AR_Frais03FR_Denomination = "";

			row.AR_Frais03FR_Rem01REM_Valeur = row.AR_Frais03FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais03FR_Rem01REM_Type = row.AR_Frais03FR_Rem01REM_Type ?? 1;
		}


		// ===== DÉFINIR LES FLAGS PAR DÉFAUT =====
		private void SetDefaultFlags(F_ARTICLE row)
		{
			// Flags de base
			row.AR_Garantie = row.AR_Garantie ?? 12; // Garantie en mois
			row.AR_Escompte = row.AR_Escompte ?? 0; // Pas d'escompte par défaut
			row.AR_Delai = row.AR_Delai ?? 0; // Délai de livraison
			row.AR_HorsStat = row.AR_HorsStat ?? 0; // Inclus dans les statistiques
			row.AR_VteDebit = row.AR_VteDebit ?? 0; // Pas de vente à débit
			row.AR_NotImp = row.AR_NotImp ?? 0; // Imprimable
			row.AR_PrixTTC = row.AR_PrixTTC ?? 0; // Prix TTC par défaut
			row.AR_Gamme1 = row.AR_Gamme1 ?? 0; // Pas de gamme
			row.AR_Gamme2 = row.AR_Gamme2 ?? 0; // Pas de gamme
			row.AR_Condition = row.AR_Condition ?? 1; // Conditionnement standard
			row.AR_Contremarque = row.AR_Contremarque ?? 0; // Pas de contremarque
			row.AR_FactPoids = row.AR_FactPoids ?? 0; // Pas de facturation au poids
			row.AR_FactForfait = row.AR_FactForfait ?? 0; // Pas de forfait
			row.AR_SaisieVar = row.AR_SaisieVar ?? 0; // Pas de saisie variable
			row.AR_Transfere = row.AR_Transfere ?? 0; // Non transféré
			row.AR_Publie = row.AR_Publie ?? 1; // Publié par défaut
			row.AR_PoidsNet = row.AR_PoidsNet ?? 0; // Poids net par défaut
			row.AR_PoidsBrut = row.AR_PoidsBrut ?? 0; // Poids brut par défaut

			// Nouveaux champs de gestion
			row.AR_Nature = row.AR_Nature ?? 0;
			row.AR_DelaiFabrication = row.AR_DelaiFabrication ?? 0;
			row.AR_NbColis = row.AR_NbColis ?? 1;
			row.AR_DelaiPeremption = row.AR_DelaiPeremption ?? 0;
			row.AR_DelaiSecurite = row.AR_DelaiSecurite ?? 0;
			row.AR_Fictif = row.AR_Fictif ?? 0;
			row.AR_SousTraitance = row.AR_SousTraitance ?? 0;
			row.AR_TypeLancement = row.AR_TypeLancement ?? 0;
			row.AR_Cycle = row.AR_Cycle ?? 0;
			row.AR_Criticite = row.AR_Criticite ?? 0;

			// ===== NOUVEAUX CHAMPS AJOUTÉS (basés sur la ligne ZACOMPTE) =====

			// Champs de coûts supplémentaires
			row.AR_CoutStd = row.AR_CoutStd ?? 0; // Coût standard
			row.AR_QteComp = row.AR_QteComp ?? 1; // Quantité de composants (valeur de ZACOMPTE: 1.000000)
			row.AR_QteOperatoire = row.AR_QteOperatoire ?? 1; // Quantité opératoire (valeur de ZACOMPTE: 1.000000)

			// Champs de type et classification
			row.CO_No = row.CO_No ?? 0; // Numéro de commande
			row.cbCO_No = row.cbCO_No ?? 0;

			// Champs de catalogue (CL_No) - valeurs de ZACOMPTE: 3, 3, 0
			row.CL_No1 = row.CL_No1 ?? 0;
			row.cbCL_No1 = row.cbCL_No1 ?? 0;
			row.CL_No2 = row.CL_No2 ?? 0;
			row.cbCL_No2 = row.cbCL_No2 ?? 0;
			row.CL_No3 = row.CL_No3 ?? 0;
			row.cbCL_No3 = row.cbCL_No3 ?? 0;
			row.CL_No4 = row.CL_No4 ?? 0;
			row.cbCL_No4 = row.cbCL_No4 ?? 0;

			// Code de ressource par défaut
			if (string.IsNullOrEmpty(row.RP_CodeDefaut))
				row.RP_CodeDefaut = null; // NULL par défaut comme dans ZACOMPTE

			// Champs de prévision et planning
			row.AR_Prevision = row.AR_Prevision ?? 0;

			// ===== CHAMPS DE TAUX ET FRAIS MANQUANTS =====

			// Compléter les champs de frais 01 (Coût de stockage)
			if (string.IsNullOrEmpty(row.AR_Frais01FR_Denomination))
				row.AR_Frais01FR_Denomination = "Coût de stockage"; // Valeur de ZACOMPTE

			row.AR_Frais01FR_Rem01REM_Valeur = row.AR_Frais01FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais01FR_Rem01REM_Type = row.AR_Frais01FR_Rem01REM_Type ?? 1; // 1 = Pourcentage (valeur de ZACOMPTE)
			row.AR_Frais01FR_Rem02REM_Valeur = row.AR_Frais01FR_Rem02REM_Valeur ?? 0;
			row.AR_Frais01FR_Rem02REM_Type = row.AR_Frais01FR_Rem02REM_Type ?? 1;
			row.AR_Frais01FR_Rem03REM_Valeur = row.AR_Frais01FR_Rem03REM_Valeur ?? 0;
			row.AR_Frais01FR_Rem03REM_Type = row.AR_Frais01FR_Rem03REM_Type ?? 1;

			// Compléter les champs de frais 02 (Coût de transport)
			if (string.IsNullOrEmpty(row.AR_Frais02FR_Denomination))
				row.AR_Frais02FR_Denomination = "Coût de transport"; // Valeur de ZACOMPTE

			row.AR_Frais02FR_Rem01REM_Valeur = row.AR_Frais02FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais02FR_Rem01REM_Type = row.AR_Frais02FR_Rem01REM_Type ?? 1;
			row.AR_Frais02FR_Rem02REM_Valeur = row.AR_Frais02FR_Rem02REM_Valeur ?? 0;
			row.AR_Frais02FR_Rem02REM_Type = row.AR_Frais02FR_Rem02REM_Type ?? 1;
			row.AR_Frais02FR_Rem03REM_Valeur = row.AR_Frais02FR_Rem03REM_Valeur ?? 0;
			row.AR_Frais02FR_Rem03REM_Type = row.AR_Frais02FR_Rem03REM_Type ?? 1;

			// Compléter les champs de frais 03
			if (string.IsNullOrEmpty(row.AR_Frais03FR_Denomination))
				row.AR_Frais03FR_Denomination = ""; // Vide comme dans ZACOMPTE

			row.AR_Frais03FR_Rem01REM_Valeur = row.AR_Frais03FR_Rem01REM_Valeur ?? 0;
			row.AR_Frais03FR_Rem01REM_Type = row.AR_Frais03FR_Rem01REM_Type ?? 1;
			row.AR_Frais03FR_Rem02REM_Valeur = row.AR_Frais03FR_Rem02REM_Valeur ?? 0;
			row.AR_Frais03FR_Rem02REM_Type = row.AR_Frais03FR_Rem02REM_Type ?? 1;
			row.AR_Frais03FR_Rem03REM_Valeur = row.AR_Frais03FR_Rem03REM_Valeur ?? 0;
			row.AR_Frais03FR_Rem03REM_Type = row.AR_Frais03FR_Rem03REM_Type ?? 1;

			// Prix unitaire net
			row.AR_PUNet = row.AR_PUNet ?? 0;

			// Champs de nouvelles valeurs de prix (pour mises à jour futures)
			row.AR_PrixAchNouv = row.AR_PrixAchNouv ?? 0;
			row.AR_CoefNouv = row.AR_CoefNouv ?? 0;
			row.AR_PrixVenNouv = row.AR_PrixVenNouv ?? 0;

			// ===== CHAMPS DE DATES MANQUANTS =====

			// Date d'application (déjà définie ailleurs avec 1753-01-01)
			// Les autres dates sont déjà gérées dans SetDefaultValuesMaroc

			// ===== CHAMPS DE TEXTES MANQUANTS =====

			// Champs de langue
			if (string.IsNullOrEmpty(row.AR_Langue1))
				row.AR_Langue1 = ""; // NULL comme dans ZACOMPTE

			if (string.IsNullOrEmpty(row.AR_Langue2))
				row.AR_Langue2 = ""; // NULL comme dans ZACOMPTE

			// Code EDI
			if (string.IsNullOrEmpty(row.AR_EdiCode))
				row.AR_EdiCode = null; // NULL comme dans ZACOMPTE

			// Photo (laissez vide par défaut)
			if (string.IsNullOrEmpty(row.AR_Photo))
				row.AR_Photo = null; // NULL comme dans ZACOMPTE

			// ===== CHAMPS DE SUBSTITUTION =====

			// Article de substitution
			if (string.IsNullOrEmpty(row.AR_Substitut))
				row.AR_Substitut = null; // NULL comme dans ZACOMPTE

			// ===== CHAMPS BINAIRES (cbAR_...) =====
			// Ces champs sont généralement gérés automatiquement par la base de données
			// Ne pas les définir manuellement sauf si nécessaire

			// Champs système
			row.cbProt = row.cbProt ?? 0;
			row.cbFlag = row.cbFlag ?? 0;

			// Créateur
			if (string.IsNullOrEmpty(row.cbCreateur))
				row.cbCreateur = "BWB";

			// Identifiant de création utilisateur (GUID)
			if (row.cbCreationUser == null)
				row.cbCreationUser = null; // NULL comme dans aezrzr, ou mettre un GUID par défaut

			// Champ de réplication
			row.cbReplication = row.cbReplication ?? 0;
		}

		// ===== GÉNÉRATION DE RÉFÉRENCES =====
		public async Task<string> GetNextArticleReference(string famille, string prefix = "ART")
		{
			// Récupérer la configuration de numérotation depuis P_TIERS ou une table de paramétrage
			var config = await _context.P_TIERS.FirstOrDefaultAsync(p => p.cbIndice == 10); // Utiliser un indice pour articles

			if (config == null || config.T_Numerotation != 1)
			{
				return await GenerateManualReference(famille, prefix);
			}

			return await GenerateAutoReference(config, famille);
		}

		private async Task<string> GenerateAutoReference(P_TIERS config, string famille)
		{
			string racine = !string.IsNullOrEmpty(famille) ? famille : config.T_Racine;
			int longueur = config.T_Lg ?? 8;

			// Obtenir la dernière référence qui commence par la racine
			var lastRef = await _context.F_ARTICLE
				.Where(a => a.AR_Ref.StartsWith(racine))
				.OrderByDescending(a => a.AR_Ref)
				.Select(a => a.AR_Ref)
				.FirstOrDefaultAsync();

			int prochainNumero;

			if (string.IsNullOrEmpty(lastRef))
			{
				prochainNumero = 1;
			}
			else
			{
				string partieNumerique = lastRef.Substring(racine.Length);
				if (int.TryParse(partieNumerique, out int dernierNumero))
				{
					prochainNumero = dernierNumero + 1;
				}
				else
				{
					prochainNumero = 1;
				}
			}

			int nombreChiffres = longueur - racine.Length;
			if (nombreChiffres <= 0) nombreChiffres = 4;

			string format = new string('0', nombreChiffres);
			return $"{racine}{prochainNumero.ToString(format)}";
		}

		private async Task<string> GenerateManualReference(string famille, string defaultPrefix)
		{
			string prefix = !string.IsNullOrEmpty(famille) ? famille : defaultPrefix;

			var existingRefs = await _context.F_ARTICLE
				.Where(a => a.AR_Ref.StartsWith(prefix))
				.Select(a => a.AR_Ref)
				.ToListAsync();

			if (!existingRefs.Any())
			{
				return $"{prefix}0001";
			}

			int maxNumero = 0;
			foreach (var refe in existingRefs)
			{
				var match = Regex.Match(refe, @"(\d+)$");
				if (match.Success && int.TryParse(match.Value, out int numero))
				{
					if (numero > maxNumero) maxNumero = numero;
				}
			}

			return $"{prefix}{(maxNumero + 1):0000}";
		}
		private string GenerateEAN13(string reference)
		{
			// Génération simplifiée d'un code EAN-13
			// Dans un environnement réel, utilisez une bibliothèque dédiée
			string baseCode = "611" + reference.GetHashCode().ToString().Replace("-", "").Substring(0, 9);
			return baseCode.PadRight(13, '0');
		}

		private byte[] GetBytes(string text)
		{
			if (string.IsNullOrEmpty(text)) return new byte[0];
			return System.Text.Encoding.UTF8.GetBytes(text);
		}

		// ===== MÉTHODES DE RECHERCHE =====

		public async Task<F_ARTICLE> GetArticle(string arRef)
		{
			return await _context.F_ARTICLE
				.FirstOrDefaultAsync(a => a.AR_Ref == arRef);
		}

		public async Task<F_ARTICLE> GetArticleByCodeBarre(string codeBarre)
		{
			return await _context.F_ARTICLE
				.FirstOrDefaultAsync(a => a.AR_CodeBarre == codeBarre);
		}

		public async Task<List<F_ARTICLE>> GetAllArticles()
		{
			return await _context.F_ARTICLE
				.OrderBy(a => a.AR_Ref)
				.ToListAsync();
		}

		public async Task<List<F_ARTICLE>> GetArticlesByFamille(string codeFamille)
		{
			return await _context.F_ARTICLE
				.Where(a => a.FA_CodeFamille == codeFamille)
				.OrderBy(a => a.AR_Design)
				.ToListAsync();
		}

		public async Task<List<F_ARTICLE>> GetArticlesByType(int type)
		{
			return await _context.F_ARTICLE
				.Where(a => a.AR_Type == type)
				.OrderBy(a => a.AR_Design)
				.ToListAsync();
		}

		public async Task<List<F_ARTICLE>> SearchArticles(string searchTerm, string famille = null, int? type = null)
		{
			var query = _context.F_ARTICLE.AsQueryable();

			if (!string.IsNullOrEmpty(famille))
				query = query.Where(a => a.FA_CodeFamille == famille);

			if (type.HasValue)
				query = query.Where(a => a.AR_Type == type.Value);

			return await query
				.Where(a => a.AR_Ref.Contains(searchTerm) ||
						   a.AR_Design.Contains(searchTerm) ||
						   a.AR_Raccourci.Contains(searchTerm) ||
						   a.AR_CodeBarre.Contains(searchTerm))
				.Take(100)
				.ToListAsync();
		}

		// ===== MÉTHODE DE MISE À JOUR =====
		public async Task<bool> UpdateArticle(F_ARTICLE updatedRow)
		{
			var existing = await GetArticle(updatedRow.AR_Ref);
			if (existing == null) return false;

			// Mettre à jour les champs (préserver AR_Ref et cbMarq)
			existing.AR_Design = updatedRow.AR_Design ?? existing.AR_Design;
			existing.FA_CodeFamille = updatedRow.FA_CodeFamille ?? existing.FA_CodeFamille;
			existing.AR_Type = updatedRow.AR_Type ?? existing.AR_Type;
			existing.AR_PrixVen = updatedRow.AR_PrixVen ?? existing.AR_PrixVen;
			existing.AR_PrixAch = updatedRow.AR_PrixAch ?? existing.AR_PrixAch;
			existing.AR_CodeBarre = updatedRow.AR_CodeBarre ?? existing.AR_CodeBarre;
			existing.AR_UniteVen = updatedRow.AR_UniteVen ?? existing.AR_UniteVen;
			existing.AR_SuiviStock = updatedRow.AR_SuiviStock ?? existing.AR_SuiviStock;
			existing.AR_Stat01 = updatedRow.AR_Stat01 ?? existing.AR_Stat01;
			existing.AR_Stat02 = updatedRow.AR_Stat02 ?? existing.AR_Stat02;
			existing.AR_Stat03 = updatedRow.AR_Stat03 ?? existing.AR_Stat03;
			existing.AR_Stat04 = updatedRow.AR_Stat04 ?? existing.AR_Stat04;
			existing.AR_Stat05 = updatedRow.AR_Stat05 ?? existing.AR_Stat05;

			existing.AR_DateModif = DateTime.Now;
			existing.cbModification = DateTime.Now;

			await _context.SaveChangesAsync();
			return true;
		}

		// ===== MÉTHODE DE SUPPRESSION =====
		public async Task<bool> DeleteArticle(string arRef)
		{
			// Vérifier si l'article est utilisé dans des documents
			bool isUsed = await _context.F_DOCLIGNE.AnyAsync(d => d.AR_Ref == arRef) ||
						 await _context.F_ARTCOMPO.AnyAsync(c => c.AR_Ref == arRef);

			if (isUsed)
			{
				throw new InvalidOperationException("Impossible de supprimer un article utilisé dans des documents ou nomenclatures");
			}

			var article = await GetArticle(arRef);
			if (article == null) return false;

			_context.F_ARTICLE.Remove(article);
			await _context.SaveChangesAsync();
			return true;
		}

		// ===== MÉTHODES DE VALIDATION =====
		public bool ReferenceExists(string arRef)
		{
			return _context.F_ARTICLE.Any(a => a.AR_Ref == arRef);
		}


		// ===== MÉTHODES D'INSERTION EN MASSE =====
		public async Task BulkInsertArticles(List<F_ARTICLE> rows, bool validateDuplicates = true)
		{
			if (validateDuplicates)
			{
				var existingRefs = await _context.F_ARTICLE
					.Where(a => rows.Select(r => r.AR_Ref).Contains(a.AR_Ref))
					.Select(a => a.AR_Ref)
					.ToListAsync();

				if (existingRefs.Any())
				{
					throw new ArgumentException($"Références dupliquées: {string.Join(", ", existingRefs)}");
				}
			}

			// Traiter chaque ligne
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row.AR_Ref))
					throw new ArgumentException("AR_Ref est obligatoire");

				if (string.IsNullOrEmpty(row.AR_Design))
					throw new ArgumentException("AR_Design est obligatoire");

				SetDefaultValues(row);
			}

			// Insérer en masse avec LinqToDB
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(rows);
		}



		// ===== GESTION DES FAMILLES =====
		public async Task<List<string>> GetAllFamilles()
		{
			return await _context.F_ARTICLE
				.Where(a => a.FA_CodeFamille != null)
				.Select(a => a.FA_CodeFamille)
				.Distinct()
				.OrderBy(f => f)
				.ToListAsync();
		}

		public async Task<Dictionary<string, int>> GetArticlesCountByFamille()
		{
			return await _context.F_ARTICLE
				.Where(a => a.FA_CodeFamille != null)
				.GroupBy(a => a.FA_CodeFamille)
				.Select(g => new { Famille = g.Key, Count = g.Count() })
				.ToDictionaryAsync(g => g.Famille, g => g.Count);
		}

	}
}