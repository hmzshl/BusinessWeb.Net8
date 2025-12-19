using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Services
{
    public class SageComptegService
    {
        private readonly DB _context;

        public SageComptegService(DB context)
        {
            _context = context;
        }

        // ===== PRINCIPALES MÉTHODES : Ajouter une ligne F_COMPTEG =====
        public async Task<F_COMPTEG> AddCompteg(F_COMPTEG row)
        {
            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(row.CG_Num))
                throw new ArgumentException("Le numéro de compte est obligatoire");

            if (string.IsNullOrEmpty(row.CG_Intitule))
                throw new ArgumentException("L'intitulé du compte est obligatoire");

            // Vérifier si le numéro de compte existe déjà
            if (await _context.F_COMPTEG.AnyAsync(c => c.CG_Num == row.CG_Num))
                throw new ArgumentException($"Le compte '{row.CG_Num}' existe déjà");

            // Définir les valeurs par défaut selon le plan comptable marocain
            SetDefaultValuesMaroc(row);

            // Générer les IDs uniques
            row.cbCreateur = "BWB";

            // Définir Classement depuis Intitule si non fourni
            if (string.IsNullOrEmpty(row.CG_Classement))
                row.CG_Classement = row.CG_Intitule.Substring(0, Math.Min(row.CG_Intitule.Length, 17));


            // Ajouter à la base de données avec LinqToDB (pas de problème OUTPUT clause)
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(new List<F_COMPTEG> { row });

            return row;
        }

        // ===== DÉFINIR LES VALEURS PAR DÉFAUT POUR LE MAROC =====
        private void SetDefaultValuesMaroc(F_COMPTEG row)
        {
            // Type de compte selon le plan comptable marocain
            row.CG_Type = DetermineAccountTypeMaroc(row.CG_Num);

            // Nature du compte selon le plan comptable marocain
            if (row.N_Nature == null || row.N_Nature == 0)
            {
                row.N_Nature = DetermineAccountNatureMaroc(row.CG_Num);
            }

            // Valeurs par défaut communes
            row.CG_Report = row.CG_Report ?? 0;
            row.CG_Saut = row.CG_Saut ?? 1;
            row.CG_Regroup = row.CG_Regroup ?? 0;
            row.CG_Analytique = row.CG_Analytique ?? 1;
            row.CG_Echeance = row.CG_Echeance ?? 0;
            row.CG_Quantite = row.CG_Quantite ?? 0;
            row.CG_Lettrage = row.CG_Lettrage ?? 0;
            row.CG_Tiers = row.CG_Tiers ?? 0;
            row.CG_Devise = row.CG_Devise ?? 0;
            row.N_Devise = row.N_Devise ?? 0;
            row.CG_Sommeil = row.CG_Sommeil ?? 0;
            row.CG_ReportAnal = row.CG_ReportAnal ?? 1;

            // Champs CB
            row.cbProt = row.cbProt ?? 0;
            row.cbReplication = row.cbReplication ?? 0;
            row.cbFlag = row.cbFlag ?? 0;
            row.CG_Raccourci = "";
            row.cbCreateur = "BWB";
        }

        // ===== DÉTERMINER LE TYPE DE COMPTE (PLAN COMPTABLE MAROCAIN) =====
        private short DetermineAccountTypeMaroc(string cgNum)
        {
            if (string.IsNullOrEmpty(cgNum) || cgNum.Length < 2)
                return 0;

            var firstTwoDigits = cgNum.Length >= 2 ? cgNum.Substring(0, 2) : cgNum;

            // Plan Comptable Marocain (CGI)
            return firstTwoDigits switch
            {
                "1" => 1,  // Comptes de capitaux
                "2" => 2,  // Comptes d'immobilisations
                "3" => 3,  // Comptes de stocks
                "4" => 4,  // Comptes de tiers
                "5" => 5,  // Comptes financiers
                "6" => 6,  // Comptes de charges
                "7" => 7,  // Comptes de produits
                "8" => 8,  // Comptes spéciaux
                "9" => 9,  // Comptes d'inventaire
                _ => 0
            };
        }

        // ===== DÉTERMINER LA NATURE DU COMPTE POUR LE MAROC =====
        private short DetermineAccountNatureMaroc(string cgNum)
        {
            if (string.IsNullOrEmpty(cgNum))
                return 6; // Par défaut: Charges

            if (cgNum.StartsWith("1")) return 1; // Capitaux propres
            if (cgNum.StartsWith("2")) return 2; // Immobilisations
            if (cgNum.StartsWith("3")) return 3; // Stocks
            if (cgNum.StartsWith("4")) return 4; // Tiers (Clients, Fournisseurs)
            if (cgNum.StartsWith("5")) return 5; // Financier
            if (cgNum.StartsWith("6")) return 6; // Charges
            if (cgNum.StartsWith("7")) return 7; // Produits
            if (cgNum.StartsWith("8")) return 8; // Spéciaux
            if (cgNum.StartsWith("9")) return 9; // Inventaire

            return 6; // Par défaut: Charges
        }

        // ===== CODES TVA MAROCAINS =====
        public class TvaCodesMaroc
        {
            public const string Tva20 = "T20";      // TVA 20% (taux normal)
            public const string Tva10 = "T10";      // TVA 10% (taux réduit)
            public const string Tva14 = "T14";      // TVA 14% (ancien taux)
            public const string Tva7 = "T7";        // TVA 7% (taux spécifique)
            public const string TvaExonere = "TEX"; // Exonéré
            public const string TvaExport = "TEXP"; // Export (0%)
            public const string TvaImport = "TIM";  // Import
        }

        // ===== MÉTHODES DE CRÉATION DE COMPTES SPÉCIFIQUES AU MAROC =====

        // Créer un compte de charge avec TVA
        public async Task<string> CreateChargeAccountMaroc(string accountName, string tvaCode = null, string secteur = null)
        {
            // Numéro de compte selon la nomenclature marocaine
            string accountNumber = await GetNextAccountNumberByClassAsync(6, secteur);

            return await QuickAddAccount(
                accountNumber,
                accountName,
                6, // Nature Charges
                tvaCode,
                secteur
            );
        }

        // Créer un compte de produit avec TVA
        public async Task<string> CreateRevenueAccountMaroc(string accountName, string tvaCode = null, string secteur = null)
        {
            // Numéro de compte selon la nomenclature marocaine
            string accountNumber = await GetNextAccountNumberByClassAsync(7, secteur);

            return await QuickAddAccount(
                accountNumber,
                accountName,
                7, // Nature Produits
                tvaCode,
                secteur
            );
        }

        // Créer un compte TVA (classe 445)
        public async Task<string> CreateTvaAccountMaroc(string typeTva, decimal tauxTva = 20)
        {
            string accountName = $"TVA {tauxTva}% {typeTva}";
            string accountNumber = await GetNextTvaAccountNumberMarocAsync(typeTva, tauxTva);

            string tvaCode = GetTvaCodeFromRateMaroc(tauxTva);

            return await QuickAddAccount(
                accountNumber,
                accountName,
                4, // Nature Tiers (comptes TVA sont en classe 4)
                tvaCode,
                "TVA"
            );
        }

        // Créer un compte client/fournisseur marocain
        public async Task<string> CreateTiersAccountMaroc(string nom, string typeTiers, string ville, string tvaCode = null)
        {
            // TypeTiers: "CL" pour Client, "FR" pour Fournisseur
            string prefix = typeTiers == "CL" ? "411" : "401";
            string accountNumber = await GetNextTiersAccountNumberAsync(prefix);

            string accountName = $"{typeTiers} - {nom} - {ville}";

            return await QuickAddAccount(
                accountNumber,
                accountName,
                4, // Nature Tiers
                tvaCode,
                typeTiers
            );
        }

        // ===== MÉTHODE D'AJOUT RAPIDE AVEC PARAMÈTRES MAROC =====
        public async Task<string> QuickAddAccount(string accountNumber, string accountName,
            short? nature = null, string tvaCode = null, string secteur = null)
        {
            var account = new F_COMPTEG
            {
                CG_Num = accountNumber,
                CG_Intitule = accountName,
                N_Nature = nature,
                TA_Code = tvaCode,
                CG_Classement = secteur
            };

            return (await AddCompteg(account)).CG_Num;
        }

        // ===== OBTENIR LES NUMÉROS DE COMPTES SUIVANTS (SPÉCIFIQUE MAROC) =====

        private async Task<string> GetNextAccountNumberByClassAsync(int classe, string sousClasse = null)
        {
            var query = _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith(classe.ToString()));

            if (!string.IsNullOrEmpty(sousClasse) && sousClasse.Length >= 2)
            {
                var sousClassePrefix = classe.ToString() + sousClasse.Substring(0, 2);
                query = query.Where(c => c.CG_Num.StartsWith(sousClassePrefix));
            }

            var lastAccount = await query
                .OrderByDescending(c => c.CG_Num)
                .Select(c => c.CG_Num)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastAccount))
            {
                // Premier compte de la classe
                return $"{classe}00000";
            }

            if (int.TryParse(lastAccount, out int lastNumber))
            {
                return (lastNumber + 1).ToString();
            }

            return $"{classe}00001";
        }

        private async Task<string> GetNextTvaAccountNumberMarocAsync(string typeTva, decimal taux)
        {
            // Comptes TVA marocains: 4451 à 4459
            // 4451 - TVA collectée
            // 4452 - TVA déductible
            // 4453 - TVA due
            // 4455 - TVA à régulariser
            // 4457 - TVA import
            // 4458 - Crédit de TVA

            string prefix = "445";
            switch (typeTva.ToUpper())
            {
                case "COLLECTEE":
                    prefix = "4451";
                    break;
                case "DEDUCTIBLE":
                    prefix = "4452";
                    break;
                case "DUE":
                    prefix = "4453";
                    break;
                case "REGULARISATION":
                    prefix = "4455";
                    break;
                case "IMPORT":
                    prefix = "4457";
                    break;
                case "CREDIT":
                    prefix = "4458";
                    break;
                default:
                    prefix = "4459"; // Autres TVA
                    break;
            }

            var lastAccount = await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith(prefix))
                .OrderByDescending(c => c.CG_Num)
                .Select(c => c.CG_Num)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastAccount))
                return prefix + "000";

            if (int.TryParse(lastAccount.Substring(prefix.Length), out int lastNumber))
                return prefix + (lastNumber + 1).ToString("000");

            return prefix + "001";
        }

        private async Task<string> GetNextTiersAccountNumberAsync(string prefix)
        {
            var lastAccount = await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith(prefix))
                .OrderByDescending(c => c.CG_Num)
                .Select(c => c.CG_Num)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastAccount))
                return prefix + "00001";

            if (int.TryParse(lastAccount.Substring(prefix.Length), out int lastNumber))
                return prefix + (lastNumber + 1).ToString("00000");

            return prefix + "00001";
        }

        // ===== OBTENIR LE CODE TVA MAROCAIN À PARTIR DU TAUX =====
        public string GetTvaCodeFromRateMaroc(decimal taux)
        {
            return taux switch
            {
                20m => TvaCodesMaroc.Tva20,      // Taux normal
                10m => TvaCodesMaroc.Tva10,      // Taux réduit (produits de première nécessité)
                14m => TvaCodesMaroc.Tva14,      // Ancien taux
                7m => TvaCodesMaroc.Tva7,        // Taux spécifique
                0m => TvaCodesMaroc.TvaExonere,  // Exonéré
                _ => "TAX"                       // Autre taxe
            };
        }

        // ===== OBTENIR LE TAUX TVA À PARTIR DU CODE =====
        public decimal GetTvaRateFromCodeMaroc(string tvaCode)
        {
            return tvaCode?.ToUpper() switch
            {
                TvaCodesMaroc.Tva20 => 20m,
                TvaCodesMaroc.Tva10 => 10m,
                TvaCodesMaroc.Tva14 => 14m,
                TvaCodesMaroc.Tva7 => 7m,
                TvaCodesMaroc.TvaExonere => 0m,
                TvaCodesMaroc.TvaExport => 0m,
                _ => 20m // Taux normal par défaut
            };
        }

        // ===== MÉTHODES DE CRÉATION DE COMPTES PRÉDÉFINIS POUR LE MAROC =====

        // Créer les comptes TVA standards
        public async Task CreateStandardTvaAccountsMaroc()
        {
            var tvaAccounts = new List<F_COMPTEG>
            {
                new() { CG_Num = "44510000", CG_Intitule = "TVA COLLECTEE 20%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva20 },
                new() { CG_Num = "44520000", CG_Intitule = "TVA DEDUCTIBLE 20%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva20 },
                new() { CG_Num = "44510001", CG_Intitule = "TVA COLLECTEE 10%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva10 },
                new() { CG_Num = "44520001", CG_Intitule = "TVA DEDUCTIBLE 10%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva10 },
                new() { CG_Num = "44510002", CG_Intitule = "TVA COLLECTEE 7%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva7 },
                new() { CG_Num = "44520002", CG_Intitule = "TVA DEDUCTIBLE 7%", N_Nature = 4, TA_Code = TvaCodesMaroc.Tva7 },
                new() { CG_Num = "44530000", CG_Intitule = "TVA DUE", N_Nature = 4, TA_Code = "TVA" },
                new() { CG_Num = "44580000", CG_Intitule = "CREDIT DE TVA", N_Nature = 4, TA_Code = "TVA" }
            };

            await BulkInsertComptegs(tvaAccounts, false);
        }

        // Créer les comptes de charges courantes
        public async Task CreateStandardChargeAccountsMaroc()
        {
            var chargeAccounts = new List<F_COMPTEG>
            {
                // Salaires et charges sociales
                new() { CG_Num = "64110000", CG_Intitule = "SALAIRES", N_Nature = 6 },
                new() { CG_Num = "64120000", CG_Intitule = "CHARGES SOCIALES", N_Nature = 6 },
                
                // Loyers
                new() { CG_Num = "61320000", CG_Intitule = "LOYERS", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 },
                
                // Électricité, eau, téléphone
                new() { CG_Num = "61410000", CG_Intitule = "ELECTRICITE", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva10 },
                new() { CG_Num = "61420000", CG_Intitule = "EAU", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva10 },
                new() { CG_Num = "61450000", CG_Intitule = "TELEPHONE/INTERNET", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 },
                
                // Fournitures de bureau
                new() { CG_Num = "61110000", CG_Intitule = "FOURNITURES DE BUREAU", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 },
                
                // Frais de transport
                new() { CG_Num = "62510000", CG_Intitule = "CARBURANT", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 },
                
                // Honoraires
                new() { CG_Num = "62210000", CG_Intitule = "HONORAIRES COMPTABLE", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 },
                new() { CG_Num = "62220000", CG_Intitule = "HONORAIRES AVOCAT", N_Nature = 6, TA_Code = TvaCodesMaroc.Tva20 }
            };

            await BulkInsertComptegs(chargeAccounts, false);
        }

        // Créer les comptes de produits courants
        public async Task CreateStandardRevenueAccountsMaroc()
        {
            var revenueAccounts = new List<F_COMPTEG>
            {
                // Ventes de marchandises
                new() { CG_Num = "71110000", CG_Intitule = "VENTES MARCHANDISES 20%", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva20 },
                new() { CG_Num = "71110001", CG_Intitule = "VENTES MARCHANDISES 10%", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva10 },
                new() { CG_Num = "71110002", CG_Intitule = "VENTES MARCHANDISES 7%", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva7 },
                
                // Prestations de services
                new() { CG_Num = "71210000", CG_Intitule = "PRESTATIONS SERVICES 20%", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva20 },
                new() { CG_Num = "71210001", CG_Intitule = "PRESTATIONS SERVICES 10%", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva10 },
                
                // Produits financiers
                new() { CG_Num = "76110000", CG_Intitule = "INTERETS BANCAIRES", N_Nature = 7 },
                new() { CG_Num = "76610000", CG_Intitule = "REVENUS LOCATIFS", N_Nature = 7, TA_Code = TvaCodesMaroc.Tva10 }
            };

            await BulkInsertComptegs(revenueAccounts, false);
        }

        // ===== MÉTHODES D'INSERTION EN MASSE =====
        public async Task BulkInsertComptegs(List<F_COMPTEG> rows, bool validateDuplicates = true)
        {
            if (validateDuplicates)
            {
                var existingNumbers = await _context.F_COMPTEG
                    .Where(c => rows.Select(r => r.CG_Num).Contains(c.CG_Num))
                    .Select(c => c.CG_Num)
                    .ToListAsync();

                if (existingNumbers.Any())
                {
                    throw new ArgumentException($"Codes de compte dupliqués: {string.Join(", ", existingNumbers)}");
                }
            }

            // Traiter chaque ligne
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.CG_Num))
                    throw new ArgumentException("CG_Num est obligatoire");

                if (string.IsNullOrEmpty(row.CG_Intitule))
                    throw new ArgumentException("CG_Intitule est obligatoire");

                SetDefaultValuesMaroc(row);
                row.cbCreateur = "BWB";


                if (string.IsNullOrEmpty(row.CG_Classement))
                    row.CG_Classement = row.CG_Intitule.Substring(0, Math.Min(row.CG_Intitule.Length, 17));

            }

            // Insérer en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(rows);
        }


        private byte[] GetBytes(string text)
        {
            if (string.IsNullOrEmpty(text)) return new byte[0];
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        // ===== MÉTHODES DE RECHERCHE SPÉCIFIQUES MAROC =====

        public async Task<List<F_COMPTEG>> GetTvaAccountsMaroc()
        {
            return await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith("445"))
                .OrderBy(c => c.CG_Num)
                .ToListAsync();
        }

        public async Task<List<F_COMPTEG>> GetClientsAccountsMaroc()
        {
            return await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith("411"))
                .OrderBy(c => c.CG_Num)
                .ToListAsync();
        }

        public async Task<List<F_COMPTEG>> GetFournisseursAccountsMaroc()
        {
            return await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith("401"))
                .OrderBy(c => c.CG_Num)
                .ToListAsync();
        }

        public async Task<List<F_COMPTEG>> GetAccountsByClasseMaroc(int classe)
        {
            return await _context.F_COMPTEG
                .Where(c => c.CG_Num.StartsWith(classe.ToString()))
                .OrderBy(c => c.CG_Num)
                .ToListAsync();
        }

        public async Task<List<F_COMPTEG>> SearchAccountsBySecteurMaroc(string secteur)
        {
            return await _context.F_COMPTEG
                .Where(c => c.CG_Classement != null && c.CG_Classement.Contains(secteur))
                .OrderBy(c => c.CG_Num)
                .Take(100)
                .ToListAsync();
        }

        // ===== MÉTHODE DE MISE À JOUR =====
        public async Task<bool> UpdateCompteg(F_COMPTEG updatedRow)
        {
            var existing = await GetCompteg(updatedRow.CG_Num);
            if (existing == null) return false;

            // Mettre à jour les champs
            existing.CG_Intitule = updatedRow.CG_Intitule ?? existing.CG_Intitule;
            existing.CG_Classement = updatedRow.CG_Classement ?? existing.CG_Classement;
            existing.N_Nature = updatedRow.N_Nature ?? existing.N_Nature;
            existing.TA_Code = updatedRow.TA_Code ?? existing.TA_Code;
            existing.CG_Report = updatedRow.CG_Report ?? existing.CG_Report;
            existing.CG_Sommeil = updatedRow.CG_Sommeil ?? existing.CG_Sommeil;

            existing.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODE DE SUPPRESSION =====
        public async Task<bool> DeleteCompteg(string cgNum)
        {
            var compteg = await GetCompteg(cgNum);
            if (compteg == null) return false;

            // Vérifier si le compte a des transactions
            var hasTransactions = await _context.F_ECRITUREC.AnyAsync(e => e.CG_Num == cgNum);
            if (hasTransactions)
                throw new InvalidOperationException($"Le compte '{cgNum}' a des écritures et ne peut être supprimé");

            _context.F_COMPTEG.Remove(compteg);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== VALIDATION SPÉCIFIQUE MAROC =====
        public bool IsValidAccountNumberMaroc(string cgNum)
        {
            if (string.IsNullOrEmpty(cgNum) || cgNum.Length < 5 || cgNum.Length > 8)
                return false;

            // Doit commencer par un chiffre de 1 à 9
            if (!char.IsDigit(cgNum[0]) || cgNum[0] == '0')
                return false;

            // Tous les caractères doivent être des chiffres
            return cgNum.All(char.IsDigit);
        }

        public bool AccountExists(string cgNum)
        {
            return _context.F_COMPTEG.Any(c => c.CG_Num == cgNum);
        }

        private async Task<F_COMPTEG> GetCompteg(string cgNum)
        {
            return await _context.F_COMPTEG.FirstOrDefaultAsync(c => c.CG_Num == cgNum);
        }
    }
}