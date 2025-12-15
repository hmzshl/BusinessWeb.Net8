using BusinessWeb.Data;
using BusinessWeb.Models.DB;

public class SageComptetService
{
	private readonly DB _context;

	public SageComptetService(DB context)
	{
		_context = context;
	}

	// ===== MAIN METHOD: Add single F_COMPTET row =====
	public async Task<F_COMPTET> AddComptet(F_COMPTET row)
	{
		// Validate required fields
		if (string.IsNullOrEmpty(row.CT_Num))
			throw new ArgumentException("CT_Num is required");

		if (string.IsNullOrEmpty(row.CT_Intitule))
			throw new ArgumentException("CT_Intitule is required");

		// Check if code already exists
		if (_context.F_COMPTET.Any(c => c.CT_Num == row.CT_Num))
			throw new ArgumentException($"Code '{row.CT_Num}' already exists");

		// Set default values based on your data pattern
		SetDefaultValues(row);

		// Set cbCT_Intitule/Raccourci if not provided
		if (string.IsNullOrEmpty(row.CT_Raccourci))
			row.CT_Raccourci = GenerateRaccourci(row.CT_Intitule);

		if (row.cbCT_Raccourci == null || row.cbCT_Raccourci.Length == 0)
			row.cbCT_Raccourci = GetBytes(row.CT_Raccourci);

		// Add to database
		_context.F_COMPTET.Add(row);
		await _context.SaveChangesAsync();

		return row;
	}

	// ===== ADD MULTIPLE ROWS =====
	public async Task<List<F_COMPTET>> AddComptets(List<F_COMPTET> rows)
	{
		var results = new List<F_COMPTET>();

		for (int i = 0; i < rows.Count; i++)
		{
			var row = rows[i];

			// Validate
			if (string.IsNullOrEmpty(row.CT_Num))
				throw new ArgumentException($"Row {i}: CT_Num is required");

			if (string.IsNullOrEmpty(row.CT_Intitule))
				throw new ArgumentException($"Row {i}: CT_Intitule is required");

			// Check duplicate
			if (results.Any(r => r.CT_Num == row.CT_Num) ||
				 _context.F_COMPTET.Any(c => c.CT_Num == row.CT_Num))
				throw new ArgumentException($"Row {i}: Code '{row.CT_Num}' already exists");

			// Set defaults
			SetDefaultValues(row);

			// Set byte arrays
			row.cbCT_Num = GetBytes(row.CT_Num);

			if (string.IsNullOrEmpty(row.CT_Raccourci))
				row.CT_Raccourci = GenerateRaccourci(row.CT_Intitule);

			if (row.cbCT_Raccourci == null || row.cbCT_Raccourci.Length == 0)
				row.cbCT_Raccourci = GetBytes(row.CT_Raccourci);

			_context.F_COMPTET.Add(row);
			results.Add(row);
		}

		await _context.SaveChangesAsync();
		return results;
	}

	// ===== SET DEFAULT VALUES based on your data =====
	private void SetDefaultValues(F_COMPTET row)
	{
		// Default CT_Type if not specified (0 = Client, 1 = Fournisseur, 2 = Salarié)
		row.CT_Type = row.CT_Type ?? 0;

		// Default account numbers based on type
		if (string.IsNullOrEmpty(row.CG_NumPrinc))
		{
			row.CG_NumPrinc = row.CT_Type switch
			{
				0 => "41100000", // Clients
				1 => "40100000", // Fournisseurs
				2 => "42100000", // Salariés
				_ => "41100000"
			};
		}

		// Default values from your sample data
		var defaultDate = new DateTime(1900, 1, 1);
		row.CT_SvDateCreate = defaultDate;
		row.CT_SvDateIncid = defaultDate;
		row.CT_SvDateMaj = defaultDate;
		row.CT_SvDateBilan = defaultDate;
		row.CT_DateFermeDebut = defaultDate;
		row.CT_DateFermeFin = defaultDate;

		// Default numeric values
		row.CT_Encours = row.CT_Encours ?? 0;
		row.CT_Assurance = row.CT_Assurance ?? 0;
		row.CT_Taux01 = row.CT_Taux01 ?? 0;
		row.CT_Taux02 = row.CT_Taux02 ?? 0;
		row.CT_Taux03 = row.CT_Taux03 ?? 0;
		row.CT_Taux04 = row.CT_Taux04 ?? 0;
		row.CT_SvCA = row.CT_SvCA ?? 0;
		row.CT_SvResultat = row.CT_SvResultat ?? 0;

		// Default flags
		row.N_Risque = row.N_Risque ?? 1;
		row.N_CatTarif = row.N_CatTarif ?? 1;
		row.N_CatCompta = row.N_CatCompta ?? 1;
		row.N_Period = row.N_Period ?? 1;
		row.CT_Facture = row.CT_Facture ?? 1;
		row.CT_BLFact = row.CT_BLFact ?? 0;
		row.CT_Langue = row.CT_Langue ?? 0;
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

		// Set classement from intitule if not provided
		if (string.IsNullOrEmpty(row.CT_Classement))
			row.CT_Classement = row.CT_Intitule;

		if (row.cbCT_Classement == null || row.cbCT_Classement.Length == 0)
			row.cbCT_Classement = GetBytes(row.CT_Classement);
	}

	// ===== CODE GENERATION METHODS =====
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
		int longueur = config.T_Lg ?? 8; // Default to 8 if null

		// Get the last code that starts with the racine for this entity type
		var lastCode = _context.F_COMPTET
			.Where(c => c.CT_Type == entityType && c.CT_Num.StartsWith(racine))
			.OrderByDescending(c => c.CT_Num)
			.Select(c => c.CT_Num)
			.FirstOrDefault();

		int nextNumber;

		if (string.IsNullOrEmpty(lastCode))
		{
			// First code for this type
			nextNumber = 1;
		}
		else
		{
			// Extract the numeric part after the racine
			string numericPart = lastCode.Substring(racine.Length);

			// Try to parse the numeric part
			if (int.TryParse(numericPart, out int lastNumber))
			{
				nextNumber = lastNumber + 1;
			}
			else
			{
				// If parsing fails, start from 1
				nextNumber = 1;
			}
		}

		// Format with leading zeros to match the required length
		int numberDigits = longueur - racine.Length;
		if (numberDigits <= 0) numberDigits = 1; // Ensure at least 1 digit

		string format = new string('0', numberDigits);
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
			return $"{prefix}0001";
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
		return $"{defaultPrefix}{(maxNumber + 1):0000}";
	}

	// ===== QUICK ADD METHODS =====
	public async Task<string> QuickAddClient(string name, string email = null, string phone = null)
	{
		string code = GetNextClientCode();

		var client = new F_COMPTET
		{
			CT_Num = code,
			CT_Intitule = name,
			CT_Type = 0, // Client
			CT_EMail = email,
			CT_Telephone = phone,
			CT_Raccourci = GenerateRaccourci(name),
			CT_Classement = name
		};

		return (await AddComptet(client)).CT_Num;
	}

	public async Task<string> QuickAddFournisseur(string name, string email = null, string phone = null)
	{
		string code = GetNextFournisseurCode();

		var fournisseur = new F_COMPTET
		{
			CT_Num = code,
			CT_Intitule = name,
			CT_Type = 1, // Fournisseur
			CT_EMail = email,
			CT_Telephone = phone,
			CT_Raccourci = GenerateRaccourci(name),
			CT_Classement = name
		};

		return (await AddComptet(fournisseur)).CT_Num;
	}

	private string GenerateRaccourci(string intitule)
	{
		// Generate short version (max 8 chars based on your data)
		if (string.IsNullOrEmpty(intitule)) return "";

		// Take first 8 characters in uppercase
		return intitule.Length <= 8
			? intitule.ToUpper()
			: intitule.Substring(0, 8).ToUpper();
	}

	private byte[] GetBytes(string text)
	{
		if (string.IsNullOrEmpty(text)) return new byte[0];
		return System.Text.Encoding.UTF8.GetBytes(text);
	}

	// ===== QUERY METHODS =====
	public F_COMPTET GetComptet(string ctNum)
	{
		return _context.F_COMPTET.FirstOrDefault(c => c.CT_Num == ctNum);
	}

	public F_COMPTET GetComptetByIntitule(string intitule)
	{
		return _context.F_COMPTET.FirstOrDefault(c => c.CT_Intitule == intitule);
	}

	public List<F_COMPTET> GetAllClients()
	{
		return _context.F_COMPTET
			.Where(c => c.CT_Type == 0)
			.OrderBy(c => c.CT_Intitule)
			.ToList();
	}

	public List<F_COMPTET> GetAllFournisseurs()
	{
		return _context.F_COMPTET
			.Where(c => c.CT_Type == 1)
			.OrderBy(c => c.CT_Intitule)
			.ToList();
	}

	public List<F_COMPTET> SearchComptets(string searchTerm, int? type = null)
	{
		var query = _context.F_COMPTET.AsQueryable();

		if (type.HasValue)
			query = query.Where(c => c.CT_Type == type.Value);

		return query
			.Where(c => c.CT_Intitule.Contains(searchTerm) ||
					   c.CT_Num.Contains(searchTerm) ||
					   c.CT_Raccourci.Contains(searchTerm))
			.Take(100)
			.ToList();
	}

	// ===== UPDATE METHOD =====
	public async Task<bool> UpdateComptet(F_COMPTET updatedRow)
	{
		var existing = GetComptet(updatedRow.CT_Num);
		if (existing == null) return false;

		// Update fields (preserve CT_Num and cbMarq)
		existing.CT_Intitule = updatedRow.CT_Intitule ?? existing.CT_Intitule;
		existing.CT_EMail = updatedRow.CT_EMail ?? existing.CT_EMail;
		existing.CT_Telephone = updatedRow.CT_Telephone ?? existing.CT_Telephone;
		existing.CT_Adresse = updatedRow.CT_Adresse ?? existing.CT_Adresse;
		existing.CT_CodePostal = updatedRow.CT_CodePostal ?? existing.CT_CodePostal;
		existing.CT_Ville = updatedRow.CT_Ville ?? existing.CT_Ville;

		// Update byte arrays if text changed
		if (updatedRow.CT_Intitule != existing.CT_Intitule && !string.IsNullOrEmpty(updatedRow.CT_Intitule))
		{
			existing.cbCT_Raccourci = GetBytes(GenerateRaccourci(updatedRow.CT_Intitule));
		}

		existing.CT_DateMAJ = DateTime.Now;
		existing.cbModification = DateTime.Now;

		await _context.SaveChangesAsync();
		return true;
	}

	// ===== DELETE METHOD =====
	public async Task<bool> DeleteComptet(string ctNum)
	{
		var comptet = GetComptet(ctNum);
		if (comptet == null) return false;

		_context.F_COMPTET.Remove(comptet);
		await _context.SaveChangesAsync();
		return true;
	}

	// ===== VALIDATION METHODS =====
	public bool CodeExists(string ctNum)
	{
		return _context.F_COMPTET.Any(c => c.CT_Num == ctNum);
	}

	public bool IsValidClientCode(string code)
	{
		var config = _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 1);
		if (config == null) return true; // No config, accept any format

		return ValidateCodeFormat(code, config);
	}

	public bool IsValidFournisseurCode(string code)
	{
		var config = _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 2);
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

	// ===== GET CONFIGURATION =====
	public P_TIERS GetClientConfig()
	{
		return _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 1);
	}

	public P_TIERS GetFournisseurConfig()
	{
		return _context.P_TIERS.FirstOrDefault(p => p.cbIndice == 2);
	}
}