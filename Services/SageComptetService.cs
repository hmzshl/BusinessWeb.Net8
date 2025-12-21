using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BusinessWeb.Services
{
    public class SageComptetService
    {
        private readonly DB _context;

        public SageComptetService(DB context)
        {
            _context = context;
        }

        // ===== MÉTHODE PRINCIPALE : Ajouter une ligne F_COMPTET =====
        public async Task<F_COMPTET> AddComptet(F_COMPTET row)
        {
            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(row.CT_Num))
                throw new ArgumentException("Le code tiers est obligatoire");

            if (string.IsNullOrEmpty(row.CT_Intitule))
                throw new ArgumentException("L'intitulé du tiers est obligatoire");

            // Vérifier si le code existe déjà
            if (await _context.F_COMPTET.AnyAsync(c => c.CT_Num == row.CT_Num))
                throw new ArgumentException($"Le code '{row.CT_Num}' existe déjà");

            // Définir les valeurs par défaut selon le contexte marocain
            SetDefaultValuesMaroc(row);
            if (string.IsNullOrEmpty(row.CT_Classement))
                row.CT_Classement = row.CT_Intitule.Substring(0, Math.Min(row.CT_Intitule.Length, 17));
            // Ajouter à la base de données avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(new List<F_COMPTET> { row });

            return row;
        }

        // ===== AJOUTER PLUSIEURS LIGNES =====
        public async Task<List<F_COMPTET>> AddComptets(List<F_COMPTET> rows)
        {
            var results = new List<F_COMPTET>();

            // Vérifier les doublons en lot
            var existingCodes = await _context.F_COMPTET
                .Where(c => rows.Select(r => r.CT_Num).Contains(c.CT_Num))
                .Select(c => c.CT_Num)
                .ToListAsync();

            if (existingCodes.Any())
            {
                throw new ArgumentException($"Codes dupliqués trouvés: {string.Join(", ", existingCodes)}");
            }

            // Vérifier les doublons dans la liste fournie
            var duplicateCodes = rows
                .GroupBy(r => r.CT_Num)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateCodes.Any())
            {
                throw new ArgumentException($"Codes dupliqués dans la liste: {string.Join(", ", duplicateCodes)}");
            }

            foreach (var row in rows)
            {
                // Validation
                if (string.IsNullOrEmpty(row.CT_Num))
                    throw new ArgumentException($"CT_Num est obligatoire");

                if (string.IsNullOrEmpty(row.CT_Intitule))
                    throw new ArgumentException($"CT_Intitule est obligatoire");

                // Définir les valeurs par défaut
                SetDefaultValuesMaroc(row);
                if (string.IsNullOrEmpty(row.CT_Classement))
                    row.CT_Classement = row.CT_Intitule.Substring(0, Math.Min(row.CT_Intitule.Length, 17));
                results.Add(row);
            }

            // Insertion en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(results);

            return results;
        }

        // ===== DÉFINIR LES VALEURS PAR DÉFAUT POUR LE MAROC =====
        private void SetDefaultValuesMaroc(F_COMPTET row)
        {
            // Type par défaut (0 = Client, 1 = Fournisseur, 2 = Salarié)
            row.CT_Type = row.CT_Type ?? 0;

            // Numéro de compte par défaut selon le type
            if (string.IsNullOrEmpty(row.CG_NumPrinc))
            {
                row.CG_NumPrinc = row.CT_Type switch
                {
                    0 => "41100000", // Clients (selon plan comptable marocain)
                    1 => "40100000", // Fournisseurs (selon plan comptable marocain)
                    2 => "42100000", // Salariés
                    3 => "46700000", // État et collectivités
                    4 => "40800000", // Associés
                    5 => "45500000", // Dirigeants
                    _ => "41100000"
                };
            }

            // Définir les dates par défaut
            var defaultDate = new DateTime(1753, 1, 1);
            row.CT_SvDateCreate = defaultDate;
            row.CT_SvDateIncid = defaultDate;
            row.CT_SvDateMaj = defaultDate;
            row.CT_SvDateBilan = defaultDate;
            row.CT_DateFermeDebut = defaultDate;
            row.CT_DateFermeFin = defaultDate;
            row.CT_DateMAJ = DateTime.Now;
            row.cbCreation = DateTime.Now;
            row.cbModification = DateTime.Now;

            // Définir les valeurs numériques par défaut
            row.CT_Encours = row.CT_Encours ?? 0;
            row.CT_Assurance = row.CT_Assurance ?? 0;
            row.CT_Taux01 = row.CT_Taux01 ?? 0;
            row.CT_Taux02 = row.CT_Taux02 ?? 0;
            row.CT_Taux03 = row.CT_Taux03 ?? 0;
            row.CT_Taux04 = row.CT_Taux04 ?? 0;
            row.CT_SvCA = row.CT_SvCA ?? 0;
            row.CT_SvResultat = row.CT_SvResultat ?? 0;

            // Champs marocains spécifiques
            if (string.IsNullOrEmpty(row.CT_Identifiant))
                row.CT_Identifiant = GenerateIdentifiantMaroc(row);

            // Valeurs par défaut pour les flags
            SetDefaultFlags(row);
        }

        // ===== GÉNÉRER L'IDENTIFIANT MAROCAIN =====
        private string GenerateIdentifiantMaroc(F_COMPTET row)
        {
            // Selon le type de tiers au Maroc:
            switch (row.CT_Type)
            {
                case 0: // Client
                case 1: // Fournisseur
                    // Pour les entreprises: ID Fiscale (ICE) ou Patente
                    // Pour les particuliers: CIN
                    return $"MA-{row.CT_Type}-{DateTime.Now:yyyyMMddHHmmss}";

                case 3: // État et collectivités
                    return $"GOV-{DateTime.Now:yyyyMMdd}";

                default:
                    return $"T{row.CT_Type}-{DateTime.Now:yyyyMMddHHmmss}";
            }
        }

        // ===== DÉFINIR LES FLAGS PAR DÉFAUT =====
        private void SetDefaultFlags(F_COMPTET row)
        {
            row.N_Risque = row.N_Risque ?? 1;
            row.N_CatTarif = row.N_CatTarif ?? 1;
            row.N_CatCompta = row.N_CatCompta ?? 1;
            row.N_Period = row.N_Period ?? 1;
            row.CT_Facture = row.CT_Facture ?? 1;
            row.CT_BLFact = row.CT_BLFact ?? 0;
            row.CT_Langue = row.CT_Langue ?? 2; // 2 = Arabe/Français pour le Maroc
            row.N_Expedition = row.N_Expedition ?? 1;
            row.N_Condition = row.N_Condition ?? 1;
            row.CT_Saut = row.CT_Saut ?? 1;
            row.CT_Lettrage = row.CT_Lettrage ?? 1;
            row.CT_ValidEch = row.CT_ValidEch ?? 1;
            row.CT_Sommeil = row.CT_Sommeil ?? 0;
            row.CT_ControlEnc = row.CT_ControlEnc ?? 0;
            row.CT_NotRappel = row.CT_NotRappel ?? 0;
            row.CT_Surveillance = row.CT_Surveillance ?? 0;
            row.CT_SvIncident = row.CT_SvIncident ?? 0;
            row.CT_SvPrivil = row.CT_SvPrivil ?? 0;
            row.CT_SvNbMoisBilan = row.CT_SvNbMoisBilan ?? 0;
            row.CT_PrioriteLivr = row.CT_PrioriteLivr ?? 0;
            row.CT_LivrPartielle = row.CT_LivrPartielle ?? 0;
            row.CT_NotPenal = row.CT_NotPenal ?? 0;
            row.CT_FactureElec = row.CT_FactureElec ?? 0;
            row.CT_ProfilSoc = row.CT_ProfilSoc ?? 0;
            row.CT_StatutContrat = row.CT_StatutContrat ?? 0;
            row.CT_EchangeRappro = row.CT_EchangeRappro ?? 0;
            row.CT_EchangeCR = row.CT_EchangeCR ?? 0;
            row.CT_BonAPayer = row.CT_BonAPayer ?? 0;
            row.CT_DelaiTransport = row.CT_DelaiTransport ?? 0;
            row.CT_DelaiAppro = row.CT_DelaiAppro ?? 0;
            row.cbCreateur = "BWB";
            row.CT_AnnulationCR = 0;
            row.CT_Facebook = "";
            row.CT_CessionCreance = 0;
            row.CT_LinkedIn = "";
            row.CT_ExclureTrait = 0;
            row.CT_GDPR = 0;
            row.CT_Prospect = row.CT_Prospect ?? 0;
            row.MR_No = 0;

        }

        // ===== TYPES DE TVA POUR LE MAROC =====
        private string GetDefaultTypeTvaMaroc(int? typeTiers)
        {
            return typeTiers switch
            {
                0 => "C",  // Client: assujetti à TVA
                1 => "F",  // Fournisseur: assujetti à TVA
                2 => "E",  // Exonéré
                3 => "S",  // État: non assujetti
                _ => "C"   // Par défaut: assujetti
            };
        }

        private decimal GetDefaultTauxTvaMaroc(int? typeTiers)
        {
            return typeTiers switch
            {
                0 => 20.00m,  // Clients: TVA 20%
                1 => 20.00m,  // Fournisseurs: TVA 20%
                2 => 0.00m,   // Salariés: pas de TVA
                3 => 0.00m,   // État: pas de TVA
                _ => 20.00m   // Par défaut: TVA 20%
            };
        }

        // ===== MÉTHODES DE GÉNÉRATION DE CODE =====
        public async Task<string> GetNextClientCodeMaroc()
        {
            // Configuration pour les clients marocains
            var config = await _context.P_TIERS.FirstOrDefaultAsync(p => p.cbIndice == 1);

            return await GetNextCodeMaroc(config, 0, "CL"); // Type Client = 0
        }

        public async Task<string> GetNextFournisseurCodeMaroc()
        {
            // Configuration pour les fournisseurs marocains
            var config = await _context.P_TIERS.FirstOrDefaultAsync(p => p.cbIndice == 2);

            return await GetNextCodeMaroc(config, 1, "FR"); // Type Fournisseur = 1
        }

        public async Task<string> GetNextSalarieCodeMaroc()
        {
            // Configuration pour les salariés marocains
            var config = await _context.P_TIERS.FirstOrDefaultAsync(p => p.cbIndice == 3);

            return await GetNextCodeMaroc(config, 2, "SA"); // Type Salarié = 2
        }

        private async Task<string> GetNextCodeMaroc(P_TIERS config, int entityType, string defaultPrefix)
        {
            if (config == null)
            {
                // Si pas de configuration, génération manuelle
                return await GenerateManualCodeMaroc(entityType, defaultPrefix);
            }

            // Vérifier si la numérotation automatique est activée
            if (config.T_Numerotation == 1 && config.T_Lg > 0 && !string.IsNullOrEmpty(config.T_Racine))
            {
                // NUMÉROTATION AUTOMATIQUE
                return await GenerateAutoCodeMaroc(config, entityType);
            }
            else
            {
                // NUMÉROTATION MANUELLE
                return await GenerateManualCodeMaroc(entityType, defaultPrefix);
            }
        }

        private async Task<string> GenerateAutoCodeMaroc(P_TIERS config, int entityType)
        {
            string racine = config.T_Racine;
            int longueur = config.T_Lg ?? 8;

            // Obtenir le dernier code qui commence par la racine pour ce type d'entité
            var lastCode = await _context.F_COMPTET
                .Where(c => c.CT_Type == entityType && c.CT_Num.StartsWith(racine))
                .OrderByDescending(c => c.CT_Num)
                .Select(c => c.CT_Num)
                .FirstOrDefaultAsync();

            int prochainNumero;

            if (string.IsNullOrEmpty(lastCode))
            {
                // Premier code pour ce type
                prochainNumero = 1;
            }
            else
            {
                // Extraire la partie numérique après la racine
                string partieNumerique = lastCode.Substring(racine.Length);

                // Essayer de parser la partie numérique
                if (int.TryParse(partieNumerique, out int dernierNumero))
                {
                    prochainNumero = dernierNumero + 1;
                }
                else
                {
                    // Si le parsing échoue, commencer à 1
                    prochainNumero = 1;
                }
            }

            // Formater avec des zéros non significatifs pour correspondre à la longueur requise
            int nombreChiffres = longueur - racine.Length;
            if (nombreChiffres <= 0) nombreChiffres = 1;

            string format = new string('0', nombreChiffres);
            return $"{racine}{prochainNumero.ToString(format)}";
        }

        private async Task<string> GenerateManualCodeMaroc(int entityType, string prefix)
        {
            // Obtenir tous les codes pour ce type d'entité
            var codesExistants = await _context.F_COMPTET
                .Where(c => c.CT_Type == entityType)
                .Select(c => c.CT_Num)
                .ToListAsync();

            if (!codesExistants.Any())
            {
                // Premier - utiliser le préfixe par défaut
                return $"{prefix}0001";
            }

            // Trouver le numéro maximum
            int maxNumero = 0;
            foreach (var code in codesExistants)
            {
                // Extraire les chiffres à la fin
                var match = Regex.Match(code, @"(\d+)$");
                if (match.Success && int.TryParse(match.Value, out int numero))
                {
                    if (numero > maxNumero) maxNumero = numero;
                }
            }

            return $"{prefix}{(maxNumero + 1):0000}";
        }

        // ===== MÉTHODES D'AJOUT RAPIDE POUR LE MAROC =====

        // Ajouter un client marocain
        public async Task<string> QuickAddClientMaroc(
            string nom,
            string ville,
            string ice = null, // Identifiant Commun de l'Entreprise
            string cin = null, // Carte d'Identité Nationale
            string patente = null, // Numéro de patente
            string email = null,
            string telephone = null,
            string secteur = null)
        {
            string code = await GetNextClientCodeMaroc();

            var client = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = nom,
                CT_Type = 0, // Client
                CT_Ville = ville,
                CT_Identifiant = ice ?? cin,
                CT_EMail = email,
                CT_Telephone = telephone,
            };

            return (await AddComptet(client)).CT_Num;
        }

        // Ajouter un fournisseur marocain
        public async Task<string> QuickAddFournisseurMaroc(
            string nom,
            string ville,
            string ice = null,
            string patente = null,
            string email = null,
            string telephone = null,
            string secteur = null)
        {
            string code = await GetNextFournisseurCodeMaroc();

            var fournisseur = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = nom,
                CT_Type = 1, // Fournisseur
                CT_Ville = ville,
                CT_Identifiant = ice,
                CT_EMail = email,
                CT_Telephone = telephone,
                CT_Classement = secteur ?? "INDUSTRIE"
            };

            return (await AddComptet(fournisseur)).CT_Num;
        }

        // Ajouter un salarié marocain
        public async Task<string> QuickAddSalarieMaroc(
            string nomPrenom,
            string cin,
            string ville,
            string email = null,
            string telephone = null,
            string poste = null)
        {
            string code = await GetNextSalarieCodeMaroc();

            var salarie = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = nomPrenom,
                CT_Type = 2, // Salarié
                CT_Ville = ville,
                CT_Identifiant = cin,
                CT_EMail = email,
                CT_Telephone = telephone,
                CT_Classement = poste ?? "EMPLOYE"
            };

            return (await AddComptet(salarie)).CT_Num;
        }

        // Ajouter une administration marocaine
        public async Task<string> QuickAddAdministrationMaroc(
            string nom,
            string typeAdmin, // Ex: "DOUANE", "TAXE", "CNSS", "MOBILITE"
            string ville,
            string reference = null)
        {
            string code = $"ADM-{typeAdmin}-{DateTime.Now:yyMMdd}";

            var admin = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = nom,
                CT_Type = 3, // Administration
                CT_Ville = ville,
                CT_Identifiant = reference,
                CT_Classement = typeAdmin
            };

            return (await AddComptet(admin)).CT_Num;
        }

        // ===== MÉTHODES UTILITAIRES MAROC =====


        private byte[] GetBytes(string text)
        {
            if (string.IsNullOrEmpty(text)) return new byte[0];
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        // ===== MÉTHODES DE RECHERCHE SPÉCIFIQUES MAROC =====

        public async Task<F_COMPTET> GetComptet(string ctNum)
        {
            return await _context.F_COMPTET.FirstOrDefaultAsync(c => c.CT_Num == ctNum);
        }

        public async Task<F_COMPTET> GetComptetByIdentifiantMaroc(string identifiant)
        {
            // Recherche par ICE, CIN ou patente
            return await _context.F_COMPTET
                .FirstOrDefaultAsync(c => c.CT_Identifiant == identifiant);
        }

        public async Task<List<F_COMPTET>> GetAllClientsMaroc()
        {
            return await _context.F_COMPTET
                .Where(c => c.CT_Type == 0)
                .OrderBy(c => c.CT_Intitule)
                .ToListAsync();
        }

        public async Task<List<F_COMPTET>> GetAllFournisseursMaroc()
        {
            return await _context.F_COMPTET
                .Where(c => c.CT_Type == 1)
                .OrderBy(c => c.CT_Intitule)
                .ToListAsync();
        }

        public async Task<List<F_COMPTET>> GetAllSalariesMaroc()
        {
            return await _context.F_COMPTET
                .Where(c => c.CT_Type == 2)
                .OrderBy(c => c.CT_Intitule)
                .ToListAsync();
        }

        public async Task<List<F_COMPTET>> GetClientsByVilleMaroc(string ville)
        {
            return await _context.F_COMPTET
                .Where(c => c.CT_Type == 0 && c.CT_Ville == ville)
                .OrderBy(c => c.CT_Intitule)
                .ToListAsync();
        }

        public async Task<List<F_COMPTET>> GetFournisseursBySecteurMaroc(string secteur)
        {
            return await _context.F_COMPTET
                .Where(c => c.CT_Type == 1 &&
                           (c.CT_Classement != null && c.CT_Classement.Contains(secteur)))
                .OrderBy(c => c.CT_Intitule)
                .ToListAsync();
        }

        public async Task<List<F_COMPTET>> SearchComptetsMaroc(
            string searchTerm,
            int? type = null,
            string ville = null)
        {
            var query = _context.F_COMPTET.AsQueryable();

            if (type.HasValue)
                query = query.Where(c => c.CT_Type == type.Value);

            if (!string.IsNullOrEmpty(ville))
                query = query.Where(c => c.CT_Ville == ville);

            return await query
                .Where(c => c.CT_Intitule.Contains(searchTerm) ||
                           c.CT_Num.Contains(searchTerm) ||
                           c.CT_Raccourci.Contains(searchTerm) ||
                           c.CT_Identifiant.Contains(searchTerm) ||
                           c.CT_Ville.Contains(searchTerm))
                .Take(100)
                .ToListAsync();
        }

        // ===== MÉTHODE DE MISE À JOUR =====
        public async Task<bool> UpdateComptet(F_COMPTET updatedRow)
        {
            var existing = await GetComptet(updatedRow.CT_Num);
            if (existing == null) return false;

            // Mettre à jour les champs (préserver CT_Num et cbMarq)
            existing.CT_Intitule = updatedRow.CT_Intitule ?? existing.CT_Intitule;
            existing.CT_EMail = updatedRow.CT_EMail ?? existing.CT_EMail;
            existing.CT_Telephone = updatedRow.CT_Telephone ?? existing.CT_Telephone;
            existing.CT_Adresse = updatedRow.CT_Adresse ?? existing.CT_Adresse;
            existing.CT_CodePostal = updatedRow.CT_CodePostal ?? existing.CT_CodePostal;
            existing.CT_Ville = updatedRow.CT_Ville ?? existing.CT_Ville;
            existing.CT_Identifiant = updatedRow.CT_Identifiant ?? existing.CT_Identifiant;


            existing.CT_DateMAJ = DateTime.Now;
            existing.cbModification = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODE DE SUPPRESSION =====
        public async Task<bool> DeleteComptet(string ctNum)
        {
            var comptet = await GetComptet(ctNum);
            if (comptet == null) return false;

            _context.F_COMPTET.Remove(comptet);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTHODES DE VALIDATION MAROC =====
        public bool CodeExists(string ctNum)
        {
            return _context.F_COMPTET.Any(c => c.CT_Num == ctNum);
        }

        public bool ValidateIceMaroc(string ice)
        {
            // Validation basique de l'ICE (Identifiant Commun de l'Entreprise)
            // Format: 15 chiffres pour le Maroc
            if (string.IsNullOrEmpty(ice) || ice.Length != 15)
                return false;

            return ice.All(char.IsDigit);
        }

        public bool ValidateCinMaroc(string cin)
        {
            // Validation basique du CIN marocain
            // Format: 1 lettre + 6 chiffres (ex: K123456)
            if (string.IsNullOrEmpty(cin) || cin.Length != 7)
                return false;

            return char.IsLetter(cin[0]) && cin.Substring(1).All(char.IsDigit);
        }

        // ===== MÉTHODES D'INSERTION EN MASSE =====
        public async Task BulkInsertComptets(List<F_COMPTET> rows, bool validateDuplicates = true)
        {
            if (validateDuplicates)
            {
                var existingCodes = await _context.F_COMPTET
                    .Where(c => rows.Select(r => r.CT_Num).Contains(c.CT_Num))
                    .Select(c => c.CT_Num)
                    .ToListAsync();

                if (existingCodes.Any())
                {
                    throw new ArgumentException($"Codes dupliqués: {string.Join(", ", existingCodes)}");
                }
            }

            // Traiter chaque ligne
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.CT_Num))
                    throw new ArgumentException("CT_Num est obligatoire");

                if (string.IsNullOrEmpty(row.CT_Intitule))
                    throw new ArgumentException("CT_Intitule est obligatoire");

                SetDefaultValuesMaroc(row);
            }

            // Insérer en masse avec LinqToDB
            using var db = _context.CreateLinqToDBConnection();
            await db.BulkCopyAsync(rows);
        }

        // ===== MÉTHODES D'IMPORT POUR LE MAROC =====

        // Importer des clients depuis un fichier Excel/CSV
        public async Task<int> ImportClientsFromCsv(List<Dictionary<string, string>> data)
        {
            var clients = new List<F_COMPTET>();

            foreach (var row in data)
            {
                string code = await GetNextClientCodeMaroc();

                var client = new F_COMPTET
                {
                    CT_Num = code,
                    CT_Intitule = row.ContainsKey("Nom") ? row["Nom"] : "Client",
                    CT_Type = 0,
                    CT_Ville = row.ContainsKey("Ville") ? row["Ville"] : "Casablanca",
                    CT_Identifiant = row.ContainsKey("ICE") ? row["ICE"] : null,
                    CT_EMail = row.ContainsKey("Email") ? row["Email"] : null,
                    CT_Telephone = row.ContainsKey("Telephone") ? row["Telephone"] : null,
                    CT_Adresse = row.ContainsKey("Adresse") ? row["Adresse"] : null
                };

                SetDefaultValuesMaroc(client);
                clients.Add(client);
            }

            await BulkInsertComptets(clients, false);
            return clients.Count;
        }

        // ===== STATISTIQUES MAROC =====
        public async Task<Dictionary<string, int>> GetStatisticsMaroc()
        {
            return new Dictionary<string, int>
            {
                ["TotalClients"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 0),
                ["TotalFournisseurs"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 1),
                ["TotalSalaries"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 2),
                ["TotalAdministrations"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 3),
                ["ClientsCasablanca"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 0 && c.CT_Ville == "Casablanca"),
                ["ClientsRabat"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 0 && c.CT_Ville == "Rabat"),
                ["ClientsMarrakech"] = await _context.F_COMPTET.CountAsync(c => c.CT_Type == 0 && c.CT_Ville == "Marrakech")
            };
        }
    }
}