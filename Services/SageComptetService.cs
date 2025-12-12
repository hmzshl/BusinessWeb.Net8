using BusinessWeb.Data;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Services
{
    public class SageComptetService
    {
        private readonly DB _context;

        public SageComptetService(DB context)
        {
            _context = context;
        }

        // ===== CODE GENERATION =====
        public string GetNextClientCode()
        {
            // Get CLIENT config: cbIndice = 1
            var config = _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 1);

            return GetNextCode(config, 0); // Client type = 0
        }

        public string GetNextFournisseurCode()
        {
            // Get FOURNISSEUR config: cbIndice = 2  
            var config = _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 2);

            return GetNextCode(config, 1); // Fournisseur type = 1
        }

        private string GetNextCode(P_TIERS config, int entityType)
        {
            if (config == null)
            {
                // If no config found, fallback to manual generation
                return GenerateManualCode(entityType);
            }

            // Check if auto-numeration is enabled (T_Numerotation = 1)
            if (config.T_Numerotation == 1 && config.T_Lg > 0 && !string.IsNullOrEmpty(config.T_Racine))
            {
                // AUTO-NUMERATION
                return GenerateAutoCode(config, entityType);
            }
            else
            {
                // MANUAL NUMERATION or fallback
                return GenerateManualCode(entityType);
            }
        }

        private string GenerateAutoCode(P_TIERS config, int entityType)
        {
            string racine = config.T_Racine; // Variable from user config
            int longueur = config.T_Lg ?? 8; // Length from config

            // Get ALL codes for this entity type (not just those starting with racine)
            // We need to find the highest number even if old codes had different format
            var allCodes = _context.F_COMPTET
                .Where(c => c.CT_Type == entityType)
                .Select(c => c.CT_Num)
                .ToList();

            // Find the maximum number from all existing codes
            int maxNumber = 0;

            foreach (var code in allCodes)
            {
                // Try to extract numeric suffix
                var match = System.Text.RegularExpressions.Regex.Match(code, @"(\d+)$");
                if (match.Success && int.TryParse(match.Value, out int number))
                {
                    if (number > maxNumber) maxNumber = number;
                }
            }

            // Next number is max + 1
            int nextNumber = maxNumber + 1;

            // Format with leading zeros based on available digits
            int availableDigits = longueur - racine.Length;
            if (availableDigits <= 0) availableDigits = 1;

            // Check if nextNumber fits in available digits
            int maxNumberForLength = (int)Math.Pow(10, availableDigits) - 1;

            if (nextNumber > maxNumberForLength)
            {
                // If number is too big for the new length, we have a problem
                // You need to decide what to do - here I'll throw an exception
                throw new InvalidOperationException(
                    $"Le numéro {nextNumber} ne rentre pas dans {availableDigits} chiffres. " +
                    $"Maximum possible: {maxNumberForLength}");
            }

            string format = new string('0', availableDigits);
            string nextCode = $"{racine}{nextNumber.ToString(format)}";

            return nextCode;
        }

        private string GenerateManualCode(int entityType)
        {
            // Get all codes for this entity type
            var existingCodes = _context.F_COMPTET
                .Where(c => c.CT_Type == entityType)
                .Select(c => c.CT_Num)
                .ToList();

            if (!existingCodes.Any())
            {
                // First one - use default prefix
                string prefix = entityType == 0 ? "CLT" : "FRS";
                return $"{prefix}001";
            }

            // Find max number
            int maxNumber = 0;
            foreach (var code in existingCodes)
            {
                // Extract trailing numbers
                var match = System.Text.RegularExpressions.Regex.Match(code, @"(\d+)$");
                if (match.Success && int.TryParse(match.Value, out int number))
                {
                    if (number > maxNumber) maxNumber = number;
                }
            }

            string defaultPrefix = entityType == 0 ? "CLT" : "FRS";
            return $"{defaultPrefix}{(maxNumber + 1):000}";
        }

        // ===== CREATE with auto-generated code =====
        public async Task<string> CreateClientAuto(string name, string email = null, string phone = null)
        {
            string code = GetNextClientCode();
            return await CreateClient(code, name, email, phone);
        }

        public async Task<string> CreateFournisseurAuto(string name, string email = null, string phone = null)
        {
            string code = GetNextFournisseurCode();
            return await CreateFournisseur(code, name, email, phone);
        }

        // ===== GET CONFIGURATION =====
        public P_TIERS GetClientConfig()
        {
            return _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 1);
        }

        public P_TIERS GetFournisseurConfig()
        {
            return _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 2);
        }

        // ===== VALIDATE code format =====
        public bool IsValidClientCode(string code)
        {
            var config = GetClientConfig();
            if (config == null) return true; // No config, accept any format

            return ValidateCodeFormat(code, config);
        }

        public bool IsValidFournisseurCode(string code)
        {
            var config = GetFournisseurConfig();
            if (config == null) return true; // No config, accept any format

            return ValidateCodeFormat(code, config);
        }

        private bool ValidateCodeFormat(string code, P_TIERS config)
        {
            if (config.T_Numerotation != 1) return true; // Not auto-numeration

            // Check length
            if (config.T_Lg.HasValue && code.Length != config.T_Lg.Value)
                return false;

            // Check prefix (Racine)
            if (!string.IsNullOrEmpty(config.T_Racine) && !code.StartsWith(config.T_Racine))
                return false;

            // Check if it ends with numbers
            if (config.T_Lg.HasValue && !string.IsNullOrEmpty(config.T_Racine))
            {
                string numericPart = code.Substring(config.T_Racine.Length);
                if (!int.TryParse(numericPart, out _))
                    return false;
            }

            return true;
        }

        // ===== UPDATE your existing CREATE methods with validation =====
        public async Task<string> CreateClient(string code, string name, string email = null, string phone = null)
        {
            // Get config for validation
            var config = GetClientConfig();

            // Validate code format if auto-numeration is enabled
            if (config != null && config.T_Numerotation == 1 && !ValidateCodeFormat(code, config))
            {
                throw new ArgumentException($"Format de code invalide. Attendu: {config.T_Racine} suivi de {config.T_Lg - config.T_Racine.Length} chiffres");
            }

            // Check if code already exists
            if (_context.F_COMPTET.Any(c => c.CT_Num == code))
            {
                throw new ArgumentException($"Le code '{code}' existe déjà");
            }

            var client = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = name,
                CT_Type = 0, // Client
                CT_EMail = email,
                CT_Telephone = phone,
                CT_DateMAJ = DateTime.Now
            };

            _context.F_COMPTET.Add(client);
            await _context.SaveChangesAsync();
            return code;
        }

        public async Task<string> CreateFournisseur(string code, string name, string email = null, string phone = null)
        {
            // Get config for validation
            var config = GetFournisseurConfig();

            // Validate code format if auto-numeration is enabled
            if (config != null && config.T_Numerotation == 1 && !ValidateCodeFormat(code, config))
            {
                throw new ArgumentException($"Format de code invalide. Attendu: {config.T_Racine} suivi de {config.T_Lg - config.T_Racine.Length} chiffres");
            }

            // Check if code already exists
            if (_context.F_COMPTET.Any(c => c.CT_Num == code))
            {
                throw new ArgumentException($"Le code '{code}' existe déjà");
            }

            var fournisseur = new F_COMPTET
            {
                CT_Num = code,
                CT_Intitule = name,
                CT_Type = 1, // Fournisseur
                CT_EMail = email,
                CT_Telephone = phone,
                CT_DateMAJ = DateTime.Now
            };

            _context.F_COMPTET.Add(fournisseur);
            await _context.SaveChangesAsync();
            return code;
        }

        // ===== SUGGEST multiple codes =====
        public List<string> SuggestClientCodes(int count = 5)
        {
            var config = GetClientConfig();
            if (config == null || config.T_Numerotation != 1)
                return new List<string> { GetNextClientCode() };

            return SuggestCodes(config, 0, count);
        }

        public List<string> SuggestFournisseurCodes(int count = 5)
        {
            var config = GetFournisseurConfig();
            if (config == null || config.T_Numerotation != 1)
                return new List<string> { GetNextFournisseurCode() };

            return SuggestCodes(config, 1, count);
        }

        private List<string> SuggestCodes(P_TIERS config, int entityType, int count)
        {
            var suggestions = new List<string>();
            string nextCode = GetNextCode(config, entityType);

            // Extract the numeric part
            if (!string.IsNullOrEmpty(config.T_Racine))
            {
                string numericPart = nextCode.Substring(config.T_Racine.Length);

                if (int.TryParse(numericPart, out int startNumber))
                {
                    string format = new string('0', numericPart.Length);

                    for (int i = 0; i < count; i++)
                    {
                        string suggestion = $"{config.T_Racine}{(startNumber + i).ToString(format)}";
                        suggestions.Add(suggestion);
                    }
                }
            }

            return suggestions;
        }

        // ===== CHECK if auto-numeration is enabled =====
        public bool IsAutoNumerationClient()
        {
            var config = GetClientConfig();
            return config?.T_Numerotation == 1;
        }

        public bool IsAutoNumerationFournisseur()
        {
            var config = GetFournisseurConfig();
            return config?.T_Numerotation == 1;
        }

        // ===== GET required code format description =====
        public string GetClientCodeFormatDescription()
        {
            var config = GetClientConfig();
            if (config == null) return "Format libre";

            if (config.T_Numerotation == 1)
            {
                return $"{config.T_Racine} + {config.T_Lg - config.T_Racine.Length} chiffres (total {config.T_Lg} caractères)";
            }

            return "Format libre";
        }

        public string GetFournisseurCodeFormatDescription()
        {
            var config = GetFournisseurConfig();
            if (config == null) return "Format libre";

            if (config.T_Numerotation == 1)
            {
                return $"{config.T_Racine} + {config.T_Lg - config.T_Racine.Length} chiffres (total {config.T_Lg} caractères)";
            }

            return "Format libre";
        }
    }
}
