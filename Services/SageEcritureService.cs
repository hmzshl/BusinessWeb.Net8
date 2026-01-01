using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BusinessWeb.Services
{
    public class SageEcritureService
    {
        private readonly DB _context;

        public SageEcritureService(DB context)
        {
            _context = context;
        }

        // ===== MÉTHODE PRINCIPALE : Ajouter une ligne F_ECRITUREC =====
        public async Task<F_ECRITUREC> AddEcriture(F_ECRITUREC row)
        {
            // 1. S'assurer que JMOUV existe pour ce journal/mois
            await EnsureJmouvExistsMaroc(row.JO_Num, row.JM_Date);

            // 2. Obtenir le prochain EC_No global
            row.EC_No = await GetNextGlobalEcNo();

            // 3. Définir JM_Date (premier jour du mois)
            row.JM_Date = new DateTime((row.JM_Date).Year,
                                      (row.JM_Date).Month, 1);

            // 4. Définir les valeurs par défaut selon le contexte marocain
            SetDefaultValuesMaroc(row);

            // 5. Ajouter à la base de données avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(new List<F_ECRITUREC> { row });

            return row;
        }

        // ===== AJOUTER PLUSIEURS ÉCRITURES =====
        public async Task<List<F_ECRITUREC>> AddEcritures(List<F_ECRITUREC> rows)
        {
            var results = new List<F_ECRITUREC>();

            // Obtenir le prochain numéro d'écriture
            int startEcNo = await GetNextGlobalEcNo();

            // Grouper par journal/mois pour JMOUV
            var journalMonths = rows
                .Select(r => new { r.JO_Num, Month = new DateTime((r.JM_Date).Year, (r.JM_Date).Month, 1) })
                .Distinct();

            foreach (var journalMonth in journalMonths)
            {
                await EnsureJmouvExistsMaroc(journalMonth.JO_Num, journalMonth.Month);
            }

            // Préparer les écritures
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                // Définir les numéros séquentiels
                row.EC_No = startEcNo + i;
                row.JM_Date = new DateTime((row.EC_Date ?? DateTime.Now).Year,
                                          (row.EC_Date ?? DateTime.Now).Month, 1);

                // Définir les valeurs par défaut
                SetDefaultValuesMaroc(row);

                results.Add(row);
            }

            // Insertion en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(results);

            return results;
        }

        // ===== DÉFINIR LES VALEURS PAR DÉFAUT POUR LE MAROC =====
        private void SetDefaultValuesMaroc(F_ECRITUREC row)
        {
            // Valeurs par défaut selon les spécifications marocaines
            row.EC_NoLink = row.EC_NoLink ?? 0;
            row.EC_Jour = row.EC_Jour ?? (short?)(row.JM_Date.Day);

            // Dates par défaut pour Sage (1753-01-01)
            var defaultDate = new DateTime(1753, 1, 1);
            row.EC_Echeance = row.EC_Echeance ?? defaultDate;
            row.EC_DatePenal = row.EC_DatePenal ?? defaultDate;
            row.EC_DateRelance = row.EC_DateRelance ?? defaultDate;
            row.EC_DateRappro = row.EC_DateRappro ?? defaultDate;
            row.EC_DateRegle = row.EC_DateRegle ?? defaultDate;

            // Valeurs numériques par défaut
            row.EC_Parite = row.EC_Parite ?? 0;
            row.EC_Quantite = row.EC_Quantite ?? 0;
            row.N_Devise = row.N_Devise ?? 1; // 1 = MAD (Dirham Marocain) par défaut
            row.EC_Montant = row.EC_Montant ?? 0;
            row.EC_Devise = row.EC_Devise ?? 1; // 1 = MAD
            row.EC_MontantRegle = row.EC_MontantRegle ?? 0;

            // Flags par défaut
            row.EC_Lettre = row.EC_Lettre ?? 0;
            row.EC_Point = row.EC_Point ?? 0;
            row.EC_Impression = row.EC_Impression ?? 0;
            row.EC_Cloture = row.EC_Cloture ?? 0;
            row.EC_Rappel = row.EC_Rappel ?? 0;
            row.EC_LettreQ = row.EC_LettreQ ?? 0;
            row.EC_ANType = row.EC_ANType ?? 0;
            row.EC_RType = row.EC_RType ?? 0;
            row.EC_Remise = row.EC_Remise ?? 0;
            row.EC_ExportExpert = row.EC_ExportExpert ?? 0;
            row.EC_Norme = row.EC_Norme ?? 0;
            row.TA_Provenance = row.TA_Provenance ?? 0;
            row.EC_PenalType = row.EC_PenalType ?? 0;
            row.EC_NoCloture = row.EC_NoCloture ?? 0;
            row.EC_ExportRappro = row.EC_ExportRappro ?? 0;

            // Champs CB
            row.cbProt = row.cbProt ?? 0;
            row.cbReplication = row.cbReplication ?? 0;
            row.cbFlag = row.cbFlag ?? 0;
            row.cbCreateur = "BWB";
            row.EC_Lettrage = "";
            row.EC_Lettre = 0;


			// Supprimer les propriétés de navigation pour éviter les problèmes
			ClearNavigationProperties(row);
        }

        // ===== DÉTERMINER LE TAUX DE TVA POUR LE MAROC =====
        private decimal DetermineTauxTvaMaroc(string compteGeneral)
        {
            if (string.IsNullOrEmpty(compteGeneral))
                return 0.00m;

            // Déterminer le taux de TVA selon le compte
            if (compteGeneral.StartsWith("445"))
            {
                // Comptes TVA
                return compteGeneral switch
                {
                    "4451" => 20.00m, // TVA collectée 20%
                    "4452" => 20.00m, // TVA déductible 20%
                    "44510" => 10.00m, // TVA collectée 10%
                    "44520" => 10.00m, // TVA déductible 10%
                    "4457" => 20.00m, // TVA import
                    _ => 20.00m // Par défaut 20%
                };
            }
            else if (compteGeneral.StartsWith("6"))
            {
                // Comptes de charges - TVA selon la nature
                return DetermineTvaForChargeAccount(compteGeneral);
            }
            else if (compteGeneral.StartsWith("7"))
            {
                // Comptes de produits - TVA selon la nature
                return DetermineTvaForProductAccount(compteGeneral);
            }

            return 0.00m; // Pas de TVA par défaut
        }

        private decimal DetermineTvaForChargeAccount(string compte)
        {
            // TVA sur les charges au Maroc
            // Certaines charges sont exonérées ou à taux réduit
            if (compte.StartsWith("61")) // Achats
                return 20.00m; // Taux normal
            if (compte.StartsWith("62")) // Services extérieurs
                return 20.00m;
            if (compte.StartsWith("63")) // Impôts et taxes
                return 0.00m; // Pas de TVA
            if (compte.StartsWith("64")) // Personnel
                return 0.00m; // Pas de TVA
            if (compte.StartsWith("65")) // Autres charges
                return 20.00m;

            return 20.00m; // Taux normal par défaut
        }

        private decimal DetermineTvaForProductAccount(string compte)
        {
            // TVA sur les produits au Maroc
            // Certains produits sont à taux réduit ou exonérés
            if (compte.StartsWith("71")) // Ventes de marchandises
                return 20.00m; // Taux normal
            if (compte.StartsWith("72")) // Ventes de produits
                return 20.00m;
            if (compte.StartsWith("73")) // Ventes de services
                return 20.00m;
            if (compte.StartsWith("75")) // Autres produits
                return 20.00m;

            return 20.00m; // Taux normal par défaut
        }

        // ===== GESTION JMOUV POUR LE MAROC =====
        private async Task EnsureJmouvExistsMaroc(string journalCode, DateTime date)
        {
            DateTime firstOfMonth = new DateTime(date.Year, date.Month, 1);

            bool exists = await _context.F_JMOUV
                .AnyAsync(j => j.JO_Num == journalCode && j.JM_Date == firstOfMonth);

            if (!exists)
            {
                var jmouv = new F_JMOUV
                {
                    JO_Num = journalCode,
                    JM_Date = firstOfMonth,
                    JM_Cloture = 0, // Non clôturé
                    JM_Impression = 0, // Non imprimé
                    cbProt = 0,
                    cbCreateur = "BWB",
                    cbMarq = await GetNextJmouvMarq()
                };

                // Ajouter avec LinqToDB
                using var db = _context.CreateLinqToDBConnection();
                await db.BulkCopyAsync(new List<F_JMOUV> { jmouv });
            }
        }

        // ===== MÉTHODES UTILITAIRES =====
        private async Task<int> GetNextGlobalEcNo()
        {
            var maxEcNo = await _context.F_ECRITUREC
                .MaxAsync(e => (int?)e.EC_No) ?? 0;

            return maxEcNo + 1;
        }

        private async Task<int> GetNextMarq()
        {
            var maxMarq = await _context.F_ECRITUREC
                .MaxAsync(e => (int?)e.cbMarq) ?? 0;

            return maxMarq + 1;
        }

        private async Task<int> GetNextJmouvMarq()
        {
            var maxMarq = await _context.F_JMOUV
                .MaxAsync(j => (int?)j.cbMarq) ?? 0;

            return maxMarq + 1;
        }

        // ===== SUPPRIMER LES PROPRIÉTÉS DE NAVIGATION =====
        private void ClearNavigationProperties(F_ECRITUREC row)
        {
            // Empêcher les références circulaires
            row.F_BONAPAYERHISTO = null;
            row.F_ECRITUREA = null;
            row.F_ECRITURECMEDIA = null;
            row.CG_NumNavigation = null;
            row.CT_NumNavigation = null;
            row.TA_CodeNavigation = null;
            row.F_DRECOUVREMENTEC = null;
            row.F_REGREVISION = null;
            row.F_REGTAXE = null;
        }

        // ===== MÉTHODES DE CRÉATION D'ÉCRITURES SPÉCIFIQUES MAROC =====

        // Créer une écriture d'achat avec TVA
        public async Task<F_ECRITUREC> CreateAchatMaroc(
            string journalCode,
            DateTime date,
            string numeroPiece,
            string fournisseurCode,
            string compteAchat,
            string libelle,
            decimal montantHT,
            decimal tauxTVA = 20.00m,
            string refPiece = null)
        {
            var ecriture = new F_ECRITUREC
            {
                JO_Num = journalCode,
                JM_Date = date,
                EC_Piece = numeroPiece,
                EC_RefPiece = refPiece,
                CG_Num = compteAchat,
                CT_Num = fournisseurCode,
                EC_Intitule = libelle,
                EC_Montant = montantHT,
                EC_Sens = 1
            };

            return await AddEcriture(ecriture);
        }

        // Créer une écriture de vente avec TVA
        public async Task<F_ECRITUREC> CreateVenteMaroc(
            string journalCode,
            DateTime date,
            string numeroFacture,
            string clientCode,
            string compteVente,
            string libelle,
            decimal montantHT,
            decimal tauxTVA = 20.00m,
            string refPiece = null)
        {
            var ecriture = new F_ECRITUREC
            {
                JO_Num = journalCode,
                JM_Date = date,
                EC_Piece = numeroFacture,
                EC_RefPiece = refPiece,
                CG_Num = compteVente,
                CT_Num = clientCode,
                EC_Intitule = libelle,
                EC_Montant = montantHT,
                EC_Sens = 2
            };

            return await AddEcriture(ecriture);
        }

        // Créer une écriture bancaire
        public async Task<F_ECRITUREC> CreateEcritureBancaireMaroc(
            string journalBancaire,
            DateTime date,
            string numeroOperation,
            string compteBancaire,
            string libelle,
            decimal montant,
            int sens, // 1 = Débit, 2 = Crédit
            string contrepartie = null)
        {
            var ecriture = new F_ECRITUREC
            {
                JO_Num = journalBancaire,
                JM_Date = date,
                EC_Piece = numeroOperation,
                CG_Num = compteBancaire,
                CT_Num = contrepartie,
                EC_Intitule = libelle,
                EC_Montant = montant,
                EC_Sens = (short?)sens
            };

            return await AddEcriture(ecriture);
        }

        // Créer une écriture de TVA
        public async Task<F_ECRITUREC> CreateEcritureTvaMaroc(
            DateTime date,
            string typeTva, // "COLLECTEE" ou "DEDUCTIBLE"
            string compteTva,
            decimal montantTva,
            string journal = "TVA-COL")
        {
            var ecriture = new F_ECRITUREC
            {
                JO_Num = journal,
                JM_Date = date,
                EC_Piece = $"TVA-{typeTva}-{date:yyyyMMdd}",
                CG_Num = compteTva,
                EC_Intitule = $"TVA {typeTva} {date:MMMM yyyy}",
                EC_Montant = montantTva,
                EC_Sens = typeTva == "COLLECTEE" ? (short?)2 : (short?)1
            };

            return await AddEcriture(ecriture);
        }

        // Créer une écriture de paie
        public async Task<F_ECRITUREC> CreateEcriturePaieMaroc(
            DateTime date,
            string comptePaie,
            string libelle,
            decimal montant,
            string journal = "PAIE-SAL")
        {
            var ecriture = new F_ECRITUREC
            {
                JO_Num = journal,
                JM_Date = date,
                EC_Piece = $"PAIE-{date:yyyyMM}",
                CG_Num = comptePaie,
                EC_Intitule = libelle,
                EC_Montant = montant,
                EC_Sens = 1
            };

            return await AddEcriture(ecriture);
        }

        // ===== MÉTHODES DE RECHERCHE SPÉCIFIQUES MAROC =====

        public async Task<F_ECRITUREC> GetEcriture(int ecNo)
        {
            return await _context.F_ECRITUREC
                .FirstOrDefaultAsync(e => e.EC_No == ecNo);
        }

        public async Task<List<F_ECRITUREC>> GetEcrituresByJournalMaroc(
            string journalCode,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _context.F_ECRITUREC
                .Where(e => e.JO_Num == journalCode);

            if (fromDate.HasValue)
                query = query.Where(e => e.EC_Date >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(e => e.EC_Date <= toDate.Value);

            return await query
                .OrderBy(e => e.EC_Date)
                .ThenBy(e => e.EC_No)
                .ToListAsync();
        }

        public async Task<List<F_ECRITUREC>> GetEcrituresByPiece(string pieceNumber)
        {
            return await _context.F_ECRITUREC
                .Where(e => e.EC_Piece == pieceNumber)
                .OrderBy(e => e.EC_Date)
                .ToListAsync();
        }

        public async Task<List<F_ECRITUREC>> GetEcrituresByClientMaroc(
            string clientCode,
            DateTime? fromDate = null,
            bool nonRegleesOnly = false)
        {
            var query = _context.F_ECRITUREC
                .Where(e => e.CT_Num == clientCode);

            if (fromDate.HasValue)
                query = query.Where(e => e.EC_Date >= fromDate.Value);

            return await query
                .OrderBy(e => e.EC_Date)
                .ToListAsync();
        }

        public async Task<List<F_ECRITUREC>> GetEcrituresByCompteMaroc(
            string compteGeneral,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _context.F_ECRITUREC
                .Where(e => e.CG_Num == compteGeneral);

            if (fromDate.HasValue)
                query = query.Where(e => e.EC_Date >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(e => e.EC_Date <= toDate.Value);

            return await query
                .OrderBy(e => e.EC_Date)
                .ToListAsync();
        }


        public async Task<List<F_ECRITUREC>> GetEcrituresARapprocherMaroc(
            string journalCode = null)
        {
            var query = _context.F_ECRITUREC
                .Where(e => e.EC_DateRappro > new DateTime(1900, 1, 1));

            if (!string.IsNullOrEmpty(journalCode))
                query = query.Where(e => e.JO_Num == journalCode);

            return await query
                .OrderBy(e => e.EC_Date)
                .ToListAsync();
        }

        // ===== MÉTHODE DE MISE À JOUR =====
        public async Task<bool> UpdateEcriture(F_ECRITUREC updatedRow)
        {
            var existing = await GetEcriture(updatedRow.EC_No);
            if (existing == null) return false;

            // Mettre à jour les champs modifiables
            existing.JO_Num = updatedRow.JO_Num;
            existing.EC_Piece = updatedRow.EC_Piece;
            existing.EC_RefPiece = updatedRow.EC_RefPiece;
            existing.CG_Num = updatedRow.CG_Num;
            existing.CT_Num = updatedRow.CT_Num;
            existing.EC_Intitule = updatedRow.EC_Intitule;
            existing.EC_Montant = updatedRow.EC_Montant;
            existing.EC_Sens = updatedRow.EC_Sens;

            existing.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODE DE SUPPRESSION =====
        public async Task<bool> DeleteEcriture(int ecNo)
        {
            var ecriture = await GetEcriture(ecNo);
            if (ecriture == null) return false;

            _context.F_ECRITUREC.Remove(ecriture);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODES D'INSERTION EN MASSE =====
        public async Task BulkInsertEcritures(List<F_ECRITUREC> rows, bool validateJournals = true)
        {
            // Préparer JMOUV pour tous les journaux/mois
            var journalMonths = rows
                .Select(r => new {
                    r.JO_Num,
                    Month = new DateTime((r.EC_Date ?? DateTime.Now).Year,
                                       (r.EC_Date ?? DateTime.Now).Month, 1)
                })
                .Distinct();

            foreach (var journalMonth in journalMonths)
            {
                await EnsureJmouvExistsMaroc(journalMonth.JO_Num, journalMonth.Month);
            }

            // Obtenir le prochain numéro d'écriture
            int startEcNo = await GetNextGlobalEcNo();

            // Préparer les écritures
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                // Définir les numéros
                row.EC_No = startEcNo + i;
                row.JM_Date = new DateTime((row.EC_Date ?? DateTime.Now).Year,
                                          (row.EC_Date ?? DateTime.Now).Month, 1);

                // Définir les valeurs par défaut
                SetDefaultValuesMaroc(row);
            }

            // Insérer en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(rows);
        }

        // ===== STATISTIQUES MAROC =====
        public async Task<Dictionary<string, decimal>> GetEcrituresStatisticsMaroc(
            DateTime dateDebut,
            DateTime dateFin)
        {
            var ecritures = await _context.F_ECRITUREC
                .Where(e => e.EC_Date >= dateDebut && e.EC_Date <= dateFin)
                .ToListAsync();

            return new Dictionary<string, decimal>
            {
                ["TotalEcritures"] = ecritures.Count,
                ["TotalDebit"] = ecritures.Where(e => e.EC_Sens == 1).Sum(e => e.EC_Montant ?? 0),
                ["TotalCredit"] = ecritures.Where(e => e.EC_Sens == 2).Sum(e => e.EC_Montant ?? 0)
            };
        }

        // ===== MÉTHODES DE RAPPROCHEMENT =====
        public async Task<bool> MarquerCommeRapprochee(int ecNo, DateTime dateRapprochement)
        {
            var ecriture = await GetEcriture(ecNo);
            if (ecriture == null) return false;

            ecriture.EC_DateRappro = dateRapprochement;
            ecriture.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}