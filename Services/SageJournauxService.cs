namespace BusinessWeb.Services
{
    using BusinessWeb.Data;
    using BusinessWeb.Models.DB;
    using LinqToDB.Data;
    using LinqToDB.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SageJournauxService
    {
        private readonly DB _context;

        public SageJournauxService(DB context)
        {
            _context = context;
        }

        // ===== MÉTHODE PRINCIPALE : Ajouter une ligne F_JOURNAUX =====
        public async Task<F_JOURNAUX> AddJournal(F_JOURNAUX row)
        {
            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(row.JO_Num))
                throw new ArgumentException("Le code journal est obligatoire");

            if (string.IsNullOrEmpty(row.JO_Intitule))
                throw new ArgumentException("L'intitulé du journal est obligatoire");

            // Vérifier si le code journal existe déjà
            if (await _context.F_JOURNAUX.AnyAsync(j => j.JO_Num == row.JO_Num))
                throw new ArgumentException($"Le code journal '{row.JO_Num}' existe déjà");

            // Définir les valeurs par défaut selon le contexte marocain
            SetDefaultValuesMaroc(row);

            // Définir les champs système
            row.cbCreateur = "BWB";

            // Définir les tableaux d'octets
            row.cbJO_Num = GetBytes(row.JO_Num);
            row.cbJO_Intitule = GetBytes(row.JO_Intitule);

            // Définir le tableau d'octets pour CG_Num si fourni
            if (!string.IsNullOrEmpty(row.CG_Num))
                row.cbCG_Num = GetBytes(row.CG_Num);

            // Ajouter à la base de données avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(new List<F_JOURNAUX> { row });

            return row;
        }

        // ===== AJOUTER PLUSIEURS JOURNAUX =====
        public async Task<List<F_JOURNAUX>> AddJournals(List<F_JOURNAUX> rows)
        {
            var results = new List<F_JOURNAUX>();

            // Vérifier les doublons en lot
            var existingCodes = await _context.F_JOURNAUX
                .Where(j => rows.Select(r => r.JO_Num).Contains(j.JO_Num))
                .Select(j => j.JO_Num)
                .ToListAsync();

            if (existingCodes.Any())
            {
                throw new ArgumentException($"Codes journal dupliqués: {string.Join(", ", existingCodes)}");
            }

            foreach (var row in rows)
            {
                // Validation
                if (string.IsNullOrEmpty(row.JO_Num))
                    throw new ArgumentException($"JO_Num est obligatoire");

                if (string.IsNullOrEmpty(row.JO_Intitule))
                    throw new ArgumentException($"JO_Intitule est obligatoire");

                // Définir les valeurs par défaut
                SetDefaultValuesMaroc(row);

                // Définir les champs système
                row.cbCreateur = "BWB";

                // Définir les tableaux d'octets
                row.cbJO_Num = GetBytes(row.JO_Num);
                row.cbJO_Intitule = GetBytes(row.JO_Intitule);

                if (!string.IsNullOrEmpty(row.CG_Num))
                    row.cbCG_Num = GetBytes(row.CG_Num);

                results.Add(row);
            }

            // Insertion en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(results);

            return results;
        }

        // ===== DÉFINIR LES VALEURS PAR DÉFAUT POUR LE MAROC =====
        private void SetDefaultValuesMaroc(F_JOURNAUX row)
        {
            // Valeurs par défaut selon les spécifications marocaines
            row.JO_Type = row.JO_Type ?? DetermineJournalTypeMaroc(row.JO_Num, row.JO_Intitule);
            row.JO_NumPiece = row.JO_NumPiece ?? 2;  // Toujours 2 selon vos données
            row.JO_Contrepartie = row.JO_Contrepartie ?? 0;  // 0 ou 1 selon vos données
            row.JO_SaisAnal = row.JO_SaisAnal ?? DetermineAnalyticalEntryMaroc(row.JO_Type ?? 0);
            row.JO_NotCalcTot = row.JO_NotCalcTot ?? 0;  // Toujours 0 selon vos données
            row.JO_Rappro = row.JO_Rappro ?? DetermineReconciliationTypeMaroc(row.JO_Type ?? 0);
            row.JO_Sommeil = row.JO_Sommeil ?? 0;  // Toujours 0 selon vos données
            row.JO_IFRS = row.JO_IFRS ?? 0;  // Toujours 0 selon vos données
            row.JO_Reglement = row.JO_Reglement ?? DeterminePaymentTypeMaroc(row.JO_Type ?? 0);
            row.JO_SuiviTreso = row.JO_SuiviTreso ?? DetermineTreasuryTrackingMaroc(row.JO_Type ?? 0);

            // Définir le compte par défaut selon le type de journal
            if (string.IsNullOrEmpty(row.CG_Num))
            {
                row.CG_Num = GetDefaultAccountMaroc(row.JO_Type ?? 0);
            }

            // Champs CB
            row.cbProt = row.cbProt ?? 0;
            row.cbReplication = row.cbReplication ?? 0;
            row.cbFlag = row.cbFlag ?? 0;
            row.cbCreateur = "BWB";
            row.JO_LettrageSaisie = 0;
        }

        // ===== DÉTERMINER LE TYPE DE JOURNAL POUR LE MAROC =====
        private short DetermineJournalTypeMaroc(string journalCode, string journalName)
        {
            if (string.IsNullOrEmpty(journalCode) && string.IsNullOrEmpty(journalName))
                return 0; // Par défaut: Général

            // Vérifier par code (spécifique Maroc)
            if (!string.IsNullOrEmpty(journalCode))
            {
                var codeUpper = journalCode.ToUpper();

                // Journaux bancaires marocains
                if (codeUpper.Contains("BMCE") || codeUpper.Contains("ATTIJARI") ||
                    codeUpper.Contains("CIH") || codeUpper.Contains("BANK") ||
                    codeUpper.StartsWith("B") || codeUpper.Contains("BQ"))
                    return 2; // Journal bancaire

                // Journaux de caisse
                if (codeUpper == "CAIS" || codeUpper.Contains("CAISSE"))
                    return 2; // Journal de caisse

                // Journaux d'achat
                if (codeUpper == "ACH" || codeUpper.Contains("ACHAT"))
                    return 0; // Journal général (achats)

                // Journaux de vente
                if (codeUpper == "VTE" || codeUpper.Contains("VENTE"))
                    return 0; // Journal général (ventes)

                // Journaux divers
                if (codeUpper == "OD" || codeUpper.Contains("DIVERSE"))
                    return 3; // Opérations diverses
            }

            // Vérifier par nom
            if (!string.IsNullOrEmpty(journalName))
            {
                var nameUpper = journalName.ToUpper();

                // Noms en français/arabe pour le Maroc
                if (nameUpper.Contains("BANQUE") || nameUpper.Contains("بنك") ||
                    nameUpper.Contains("REMISE") || nameUpper.Contains("إيداع") ||
                    nameUpper.Contains("VIREMENT") || nameUpper.Contains("تحويل"))
                    return 2; // Journal bancaire

                if (nameUpper.Contains("CAISSE") || nameUpper.Contains("صندوق"))
                    return 2; // Journal de caisse

                if (nameUpper.Contains("ACHAT") || nameUpper.Contains("شراء"))
                    return 0; // Journal d'achat

                if (nameUpper.Contains("VENTE") || nameUpper.Contains("بيع"))
                    return 0; // Journal de vente

                if (nameUpper.Contains("DIVERSE") || nameUpper.Contains("متنوع"))
                    return 3; // Journal divers
            }

            return 0; // Par défaut: Journal général
        }

        // ===== DÉTERMINER LA SAISIE ANALYTIQUE POUR LE MAROC =====
        private short DetermineAnalyticalEntryMaroc(short journalType)
        {
            // Au Maroc, certains journaux nécessitent une saisie analytique
            return journalType switch
            {
                0 => 1,  // Général: saisie analytique souvent requise
                2 => 0,  // Bancaire: pas de saisie analytique
                3 => 1,  // Divers: saisie analytique
                _ => 0
            };
        }

        // ===== DÉTERMINER LE TYPE DE RAPPROCHEMENT POUR LE MAROC =====
        private short DetermineReconciliationTypeMaroc(short journalType)
        {
            return journalType switch
            {
                2 => 1,  // Journaux bancaires: 1 point (rapprochement bancaire)
                0 => 0,  // Journaux généraux: 0 point
                3 => 0,  // Journaux divers: 0 point
                _ => 0
            };
        }

        // ===== DÉTERMINER LE TYPE DE RÈGLEMENT POUR LE MAROC =====
        private short DeterminePaymentTypeMaroc(short journalType)
        {
            // Au Maroc, certains journaux sont spécifiques aux règlements
            return journalType switch
            {
                2 => 1,  // Journaux bancaires: utilisés pour les règlements
                0 => 0,  // Autres: pas spécifique aux règlements
                _ => 0
            };
        }

        // ===== DÉTERMINER LE SUIVI TRÉSORERIE POUR LE MAROC =====
        private short DetermineTreasuryTrackingMaroc(short journalType)
        {
            // Suivi trésorerie important pour les journaux bancaires au Maroc
            return journalType switch
            {
                2 => 1,  // Journaux bancaires: suivi trésorerie activé
                _ => 0   // Autres: pas de suivi
            };
        }

        // ===== OBTENIR LE COMPTE PAR DÉFAUT POUR LE MAROC =====
        private string GetDefaultAccountMaroc(short journalType)
        {
            // Comptes selon le plan comptable marocain
            return journalType switch
            {
                2 => "51410000", // Banques (classe 5)
                0 => "61100000", // Achats/Ventes (classe 6/7)
                3 => "47100000", // Divers (classe 4)
                _ => "47100000"  // Par défaut
            };
        }

        // ===== MÉTHODES DE CRÉATION DE JOURNAUX SPÉCIFIQUES MAROC =====

        // Créer un journal bancaire marocain
        public async Task<string> CreateBankJournalMaroc(
            string bankName,
            string branch = null,
            string accountNumber = null)
        {
            string journalCode = GenerateBankJournalCodeMaroc(bankName, branch);
            string journalName = $"BANQUE {bankName}" + (string.IsNullOrEmpty(branch) ? "" : $" - {branch}");

            return await QuickAddJournalMaroc(
                journalCode,
                journalName,
                accountNumber ?? "51410000", // Compte banque par défaut
                2  // Type journal bancaire
            );
        }

        // Créer un journal de caisse marocain
        public async Task<string> CreateCashJournalMaroc(string location = "PRINCIPALE")
        {
            string journalCode = $"CAIS-{location.Substring(0, Math.Min(3, location.Length)).ToUpper()}";
            string journalName = $"CAISSE {location}";

            return await QuickAddJournalMaroc(
                journalCode,
                journalName,
                "53100000", // Compte caisse par défaut
                2  // Type journal caisse
            );
        }

        // Créer un journal d'achat marocain
        public async Task<string> CreatePurchaseJournalMaroc(string supplierType = null)
        {
            string typeSuffix = string.IsNullOrEmpty(supplierType) ? "" : $" {supplierType}";
            string journalCode = $"ACH{typeSuffix.Substring(0, Math.Min(3, typeSuffix.Length)).ToUpper()}";
            string journalName = $"ACHATS{typeSuffix}";

            return await QuickAddJournalMaroc(
                journalCode,
                journalName,
                null,  // Pas de compte spécifique
                0  // Type journal général
            );
        }

        // Créer un journal de vente marocain
        public async Task<string> CreateSalesJournalMaroc(string customerType = null)
        {
            string typeSuffix = string.IsNullOrEmpty(customerType) ? "" : $" {customerType}";
            string journalCode = $"VTE{typeSuffix.Substring(0, Math.Min(3, typeSuffix.Length)).ToUpper()}";
            string journalName = $"VENTES{typeSuffix}";

            return await QuickAddJournalMaroc(
                journalCode,
                journalName,
                null,  // Pas de compte spécifique
                0  // Type journal général
            );
        }

        // Créer un journal TVA marocain
        public async Task<string> CreateTvaJournalMaroc(string tvaType = "COLLECTEE")
        {
            string journalCode = $"TVA-{tvaType.Substring(0, Math.Min(3, tvaType.Length)).ToUpper()}";
            string journalName = $"TVA {tvaType}";

            return await QuickAddJournalMaroc(
                journalCode,
                journalName,
                "44510000", // Compte TVA collectée
                0  // Type journal général
            );
        }

        // Créer un journal de paie marocain
        public async Task<string> CreatePayrollJournalMaroc()
        {
            return await QuickAddJournalMaroc(
                "PAIE",
                "JOURNAL DE PAIE",
                "64110000", // Compte salaires
                0  // Type journal général
            );
        }

        // Créer un journal de régularisation marocain
        public async Task<string> CreateAdjustmentJournalMaroc()
        {
            return await QuickAddJournalMaroc(
                "REG",
                "REGULARISATIONS",
                "47100000", // Compte régularisation
                3  // Type journal divers
            );
        }

        // ===== MÉTHODE D'AJOUT RAPIDE AVEC PARAMÈTRES MAROC =====
        public async Task<string> QuickAddJournalMaroc(
            string journalCode,
            string journalName,
            string accountNumber = null,
            short? journalType = null)
        {
            var journal = new F_JOURNAUX
            {
                JO_Num = journalCode,
                JO_Intitule = journalName,
                CG_Num = accountNumber,
                JO_Type = journalType
            };

            return (await AddJournal(journal)).JO_Num;
        }

        // ===== MÉTHODES UTILITAIRES MAROC =====
        private string GenerateBankJournalCodeMaroc(string bankName, string branch)
        {
            // Générer un code pour une banque marocaine
            string bankPrefix = GetBankPrefixMaroc(bankName);
            string branchSuffix = string.IsNullOrEmpty(branch)
                ? "001"
                : branch.Substring(0, Math.Min(3, branch.Length)).ToUpper();

            return $"{bankPrefix}-{branchSuffix}";
        }

        private string GetBankPrefixMaroc(string bankName)
        {
            // Préfixes pour les banques marocaines courantes
            var bankNameUpper = bankName.ToUpper();

            if (bankNameUpper.Contains("ATTIJARI") || bankNameUpper.Contains("WAFA"))
                return "ATB";
            if (bankNameUpper.Contains("BMCE") || bankNameUpper.Contains("BANK OF AFRICA"))
                return "BMCE";
            if (bankNameUpper.Contains("CIH"))
                return "CIH";
            if (bankNameUpper.Contains("BMCI"))
                return "BMCI";
            if (bankNameUpper.Contains("SOCIETE GENERALE"))
                return "SGMA";
            if (bankNameUpper.Contains("CREDIT DU MAROC"))
                return "CDM";
            if (bankNameUpper.Contains("ARAB BANK"))
                return "ABM";
            if (bankNameUpper.Contains("CREDIT AGRICOLE"))
                return "CAM";

            // Par défaut, prendre les 3-4 premières lettres
            return bankName.Length >= 4
                ? bankName.Substring(0, 4).ToUpper()
                : bankName.ToUpper().PadRight(4, 'X');
        }

        private byte[] GetBytes(string text)
        {
            if (string.IsNullOrEmpty(text)) return new byte[0];
            return Encoding.UTF8.GetBytes(text);
        }

        // ===== MÉTHODES DE RECHERCHE SPÉCIFIQUES MAROC =====

        public async Task<F_JOURNAUX> GetJournal(string joNum)
        {
            return await _context.F_JOURNAUX.FirstOrDefaultAsync(j => j.JO_Num == joNum);
        }

        public async Task<F_JOURNAUX> GetJournalByIntitule(string intitule)
        {
            return await _context.F_JOURNAUX.FirstOrDefaultAsync(j => j.JO_Intitule == intitule);
        }

        public async Task<List<F_JOURNAUX>> GetJournalsByTypeMaroc(short journalType)
        {
            return await _context.F_JOURNAUX
                .Where(j => j.JO_Type == journalType)
                .OrderBy(j => j.JO_Num)
                .ToListAsync();
        }

        public async Task<List<F_JOURNAUX>> GetBankJournalsMaroc()
        {
            return await _context.F_JOURNAUX
                .Where(j => j.JO_Type == 2) // Journaux bancaires/caisse
                .OrderBy(j => j.JO_Num)
                .ToListAsync();
        }

        public async Task<List<F_JOURNAUX>> GetGeneralJournalsMaroc()
        {
            return await _context.F_JOURNAUX
                .Where(j => j.JO_Type == 0) // Journaux généraux
                .OrderBy(j => j.JO_Num)
                .ToListAsync();
        }

        public async Task<List<F_JOURNAUX>> GetMiscJournalsMaroc()
        {
            return await _context.F_JOURNAUX
                .Where(j => j.JO_Type == 3) // Journaux divers
                .OrderBy(j => j.JO_Num)
                .ToListAsync();
        }

        public async Task<List<F_JOURNAUX>> GetTvaJournalsMaroc()
        {
            return await _context.F_JOURNAUX
                .Where(j => j.JO_Intitule.Contains("TVA") || j.JO_Num.Contains("TVA"))
                .OrderBy(j => j.JO_Num)
                .ToListAsync();
        }

        public async Task<List<F_JOURNAUX>> SearchJournalsMaroc(
            string searchTerm,
            short? journalType = null,
            bool includeArabic = true)
        {
            var query = _context.F_JOURNAUX.AsQueryable();

            if (journalType.HasValue)
                query = query.Where(j => j.JO_Type == journalType.Value);

            // Recherche en français et arabe si demandé
            return await query
                .Where(j => j.JO_Intitule.Contains(searchTerm) ||
                           j.JO_Num.Contains(searchTerm))
                .Take(100)
                .ToListAsync();
        }

        // ===== MÉTHODE DE MISE À JOUR =====
        public async Task<bool> UpdateJournal(F_JOURNAUX updatedRow)
        {
            var existing = await GetJournal(updatedRow.JO_Num);
            if (existing == null) return false;

            // Mettre à jour les champs
            existing.JO_Intitule = updatedRow.JO_Intitule ?? existing.JO_Intitule;
            existing.CG_Num = updatedRow.CG_Num ?? existing.CG_Num;
            existing.JO_Type = updatedRow.JO_Type ?? existing.JO_Type;
            existing.JO_NumPiece = updatedRow.JO_NumPiece ?? existing.JO_NumPiece;
            existing.JO_Contrepartie = updatedRow.JO_Contrepartie ?? existing.JO_Contrepartie;
            existing.JO_SaisAnal = updatedRow.JO_SaisAnal ?? existing.JO_SaisAnal;
            existing.JO_Rappro = updatedRow.JO_Rappro ?? existing.JO_Rappro;
            existing.JO_Sommeil = updatedRow.JO_Sommeil ?? existing.JO_Sommeil;
            existing.JO_Reglement = updatedRow.JO_Reglement ?? existing.JO_Reglement;
            existing.JO_SuiviTreso = updatedRow.JO_SuiviTreso ?? existing.JO_SuiviTreso;

            // Mettre à jour les tableaux d'octets si le texte a changé
            if (updatedRow.JO_Intitule != existing.JO_Intitule && !string.IsNullOrEmpty(updatedRow.JO_Intitule))
            {
                existing.cbJO_Intitule = GetBytes(updatedRow.JO_Intitule);
            }

            if (updatedRow.CG_Num != existing.CG_Num && !string.IsNullOrEmpty(updatedRow.CG_Num))
            {
                existing.cbCG_Num = GetBytes(updatedRow.CG_Num);
            }

            existing.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODE DE SUPPRESSION =====
        public async Task<bool> DeleteJournal(string joNum)
        {
            var journal = await GetJournal(joNum);
            if (journal == null) return false;

            // Vérifier si le journal a des transactions
            var hasTransactions = await _context.F_ECRITUREC.AnyAsync(e => e.JO_Num == joNum);
            if (hasTransactions)
                throw new InvalidOperationException($"Le journal '{joNum}' a des écritures et ne peut être supprimé");

            _context.F_JOURNAUX.Remove(journal);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODES D'INSERTION EN MASSE =====
        public async Task BulkInsertJournals(List<F_JOURNAUX> rows, bool validateDuplicates = true)
        {
            if (validateDuplicates)
            {
                var existingCodes = await _context.F_JOURNAUX
                    .Where(j => rows.Select(r => r.JO_Num).Contains(j.JO_Num))
                    .Select(j => j.JO_Num)
                    .ToListAsync();

                if (existingCodes.Any())
                {
                    throw new ArgumentException($"Codes journal dupliqués: {string.Join(", ", existingCodes)}");
                }
            }

            // Traiter chaque ligne
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.JO_Num))
                    throw new ArgumentException("JO_Num est obligatoire");

                if (string.IsNullOrEmpty(row.JO_Intitule))
                    throw new ArgumentException("JO_Intitule est obligatoire");

                SetDefaultValuesMaroc(row);
                row.cbCreateur = "BWB";
                row.cbJO_Num = GetBytes(row.JO_Num);
                row.cbJO_Intitule = GetBytes(row.JO_Intitule);

                if (!string.IsNullOrEmpty(row.CG_Num))
                    row.cbCG_Num = GetBytes(row.CG_Num);
            }

            // Insérer en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(rows);
        }

        // ===== MÉTHODES DE CRÉATION DES JOURNAUX STANDARDS MAROC =====
        public async Task CreateStandardJournalsMaroc()
        {
            var standardJournals = new List<F_JOURNAUX>
            {
                // Journaux bancaires
                new() { JO_Num = "BANQ-PRIN", JO_Intitule = "BANQUE PRINCIPALE", JO_Type = 2, CG_Num = "51410000" },
                new() { JO_Num = "CAIS-PRIN", JO_Intitule = "CAISSE PRINCIPALE", JO_Type = 2, CG_Num = "53100000" },
                
                // Journaux d'achat
                new() { JO_Num = "ACH-MARCH", JO_Intitule = "ACHATS MARCHANDISES", JO_Type = 0 },
                new() { JO_Num = "ACH-MAT", JO_Intitule = "ACHATS MATIERES", JO_Type = 0 },
                new() { JO_Num = "ACH-SERV", JO_Intitule = "ACHATS SERVICES", JO_Type = 0 },
                
                // Journaux de vente
                new() { JO_Num = "VTE-MARCH", JO_Intitule = "VENTES MARCHANDISES", JO_Type = 0 },
                new() { JO_Num = "VTE-SERV", JO_Intitule = "VENTES SERVICES", JO_Type = 0 },
                new() { JO_Num = "VTE-EXPORT", JO_Intitule = "VENTES EXPORT", JO_Type = 0 },
                
                // Journaux TVA
                new() { JO_Num = "TVA-COL", JO_Intitule = "TVA COLLECTEE", JO_Type = 0, CG_Num = "44510000" },
                new() { JO_Num = "TVA-DED", JO_Intitule = "TVA DEDUCTIBLE", JO_Type = 0, CG_Num = "44520000" },
                
                // Journaux de paie
                new() { JO_Num = "PAIE-SAL", JO_Intitule = "PAIE SALAIRES", JO_Type = 0, CG_Num = "64110000" },
                new() { JO_Num = "PAIE-CNSS", JO_Intitule = "PAIE CNSS", JO_Type = 0, CG_Num = "43110000" },
                
                // Journaux divers
                new() { JO_Num = "OD-GEN", JO_Intitule = "OPERATIONS DIVERSES", JO_Type = 3 },
                new() { JO_Num = "REG-ANO", JO_Intitule = "REGULARISATIONS ANNUES", JO_Type = 3 }
            };

            await BulkInsertJournals(standardJournals, false);
        }

        // ===== VALIDATION MAROC =====
        public bool JournalExists(string joNum)
        {
            return _context.F_JOURNAUX.Any(j => j.JO_Num == joNum);
        }

        public bool ValidateJournalCodeMaroc(string journalCode)
        {
            // Validation du code journal selon les standards marocains
            if (string.IsNullOrEmpty(journalCode) || journalCode.Length > 10)
                return false;

            // Codes courants au Maroc
            var validPrefixes = new[] { "BANQ", "CAIS", "ACH", "VTE", "TVA", "PAIE", "OD", "REG", "BMCE", "ATB", "CIH" };

            return validPrefixes.Any(prefix => journalCode.StartsWith(prefix)) ||
                   journalCode.Contains('-'); // Format avec séparateur
        }

        // ===== STATISTIQUES MAROC =====
        public async Task<Dictionary<string, int>> GetJournalStatisticsMaroc()
        {
            return new Dictionary<string, int>
            {
                ["TotalJournaux"] = await _context.F_JOURNAUX.CountAsync(),
                ["JournauxBancaires"] = await _context.F_JOURNAUX.CountAsync(j => j.JO_Type == 2),
                ["JournauxAchat"] = await _context.F_JOURNAUX.CountAsync(j => j.JO_Intitule.Contains("ACHAT")),
                ["JournauxVente"] = await _context.F_JOURNAUX.CountAsync(j => j.JO_Intitule.Contains("VENTE")),
                ["JournauxTVA"] = await _context.F_JOURNAUX.CountAsync(j => j.JO_Intitule.Contains("TVA")),
                ["JournauxActifs"] = await _context.F_JOURNAUX.CountAsync(j => j.JO_Sommeil == 0)
            };
        }

        // ===== ACTIVER/DÉSACTIVER JOURNAL =====
        public async Task<bool> ActivateJournal(string joNum)
        {
            var journal = await GetJournal(joNum);
            if (journal == null) return false;

            journal.JO_Sommeil = 0; // 0 = Actif
            journal.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateJournal(string joNum)
        {
            var journal = await GetJournal(joNum);
            if (journal == null) return false;

            journal.JO_Sommeil = 1; // 1 = Inactif (Sommeil)
            journal.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== EXPORT/IMPORT POUR LE MAROC =====
        public async Task<List<Dictionary<string, string>>> ExportJournalsForMaroc()
        {
            var journals = await _context.F_JOURNAUX.ToListAsync();

            return journals.Select(j => new Dictionary<string, string>
            {
                ["Code"] = j.JO_Num,
                ["Nom"] = j.JO_Intitule,
                ["Type"] = GetJournalTypeNameMaroc(j.JO_Type ?? 0),
                ["Compte"] = j.CG_Num ?? "",
                ["Statut"] = j.JO_Sommeil == 0 ? "Actif" : "Inactif"
            }).ToList();
        }

        private string GetJournalTypeNameMaroc(short type)
        {
            return type switch
            {
                0 => "Général",
                2 => "Bancaire/Caisse",
                3 => "Divers",
                _ => "Inconnu"
            };
        }
    }
}