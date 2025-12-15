using BusinessWeb.Data;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Services
{
	public class SageComptegService
	{
		private readonly DB _context;

		public SageComptegService(DB context)
		{
			_context = context;
		}

		// ===== MAIN METHOD: Add single F_COMPTEG row =====
		public async Task<F_COMPTEG> AddCompteg(F_COMPTEG row)
		{
			// Validate required fields
			if (string.IsNullOrEmpty(row.CG_Num))
				throw new ArgumentException("CG_Num is required");

			if (string.IsNullOrEmpty(row.CG_Intitule))
				throw new ArgumentException("CG_Intitule is required");

			// Check if account code already exists
			if (_context.F_COMPTEG.Any(c => c.CG_Num == row.CG_Num))
				throw new ArgumentException($"Account code '{row.CG_Num}' already exists");

			// Set default values based on your data
			SetDefaultValues(row);

			// Generate unique IDs (ignore cbMarq since it's auto-generated)
			row.cbCreateur = "BWB";

			// Set byte arrays
			row.cbCG_Num = GetBytes(row.CG_Num);

			// Set CG_Raccourci if not provided
			if (string.IsNullOrEmpty(row.CG_Raccourci))
				row.CG_Raccourci = GenerateRaccourci(row.CG_Intitule);

			if (row.cbCG_Raccourci == null || row.cbCG_Raccourci.Length == 0)
				row.cbCG_Raccourci = GetBytes(row.CG_Raccourci);

			// Set Classement from Intitule if not provided
			if (string.IsNullOrEmpty(row.CG_Classement))
				row.CG_Classement = row.CG_Intitule.Substring(0, Math.Min(row.CG_Intitule.Length, 20));

			if (row.cbCG_Classement == null || row.cbCG_Classement.Length == 0)
				row.cbCG_Classement = GetBytes(row.CG_Classement);

			// Add to database
			_context.F_COMPTEG.Add(row);
			await _context.SaveChangesAsync();

			return row;
		}

		// ===== ADD MULTIPLE ROWS =====
		public async Task<List<F_COMPTEG>> AddComptegs(List<F_COMPTEG> rows)
		{
			var results = new List<F_COMPTEG>();

			foreach (var row in rows)
			{
				// Validate
				if (string.IsNullOrEmpty(row.CG_Num))
					throw new ArgumentException($"Row: CG_Num is required");

				if (string.IsNullOrEmpty(row.CG_Intitule))
					throw new ArgumentException($"Row: CG_Intitule is required");

				// Check duplicate
				if (_context.F_COMPTEG.Any(c => c.CG_Num == row.CG_Num))
					throw new ArgumentException($"Row: Account code '{row.CG_Num}' already exists");

				// Set defaults
				SetDefaultValues(row);

				// Set system fields
				row.cbCreateur = "BWB";

				// Set byte arrays
				row.cbCG_Num = GetBytes(row.CG_Num);

				if (string.IsNullOrEmpty(row.CG_Raccourci))
					row.CG_Raccourci = GenerateRaccourci(row.CG_Intitule);

				if (row.cbCG_Raccourci == null || row.cbCG_Raccourci.Length == 0)
					row.cbCG_Raccourci = GetBytes(row.CG_Raccourci);

				if (string.IsNullOrEmpty(row.CG_Classement))
					row.CG_Classement = row.CG_Intitule.Substring(0, Math.Min(row.CG_Intitule.Length, 20));

				if (row.cbCG_Classement == null || row.cbCG_Classement.Length == 0)
					row.cbCG_Classement = GetBytes(row.CG_Classement);

				_context.F_COMPTEG.Add(row);
				results.Add(row);
			}

			await _context.SaveChangesAsync();
			return results;
		}

		// ===== SET DEFAULT VALUES based on your data =====
		private void SetDefaultValues(F_COMPTEG row)
		{
			// Default CG_Type is 0 from your data
			row.CG_Type = row.CG_Type ?? 0;

			// Determine N_Nature based on account number
			if (row.N_Nature == null || row.N_Nature == 0)
			{
				row.N_Nature = DetermineAccountNature(row.CG_Num);
			}

			// Default values from your data
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

			// CB fields
			row.cbProt = row.cbProt ?? 0;
			row.cbReplication = row.cbReplication ?? 0;
			row.cbFlag = row.cbFlag ?? 0;
		}

		// ===== DETERMINE ACCOUNT NATURE =====
		private short DetermineAccountNature(string cgNum)
		{
			if (string.IsNullOrEmpty(cgNum))
				return 8; // Default to Charges

			// Based on your data:
			// 8 = Charges (6xxx accounts)
			// 9 = Produits (7xxx accounts) 
			// 0 = TVA/Bilan accounts (4xxx accounts)

			if (cgNum.StartsWith("6")) return 8; // Charges
			if (cgNum.StartsWith("7")) return 9; // Produits
			if (cgNum.StartsWith("4")) return 0; // TVA/Comptes de bilan
			if (cgNum.StartsWith("1") || cgNum.StartsWith("2") || cgNum.StartsWith("3")) return 0; // Bilan

			return 8; // Default to Charges
		}

		// ===== QUICK ADD METHODS =====
		public async Task<string> QuickAddAccount(string accountNumber, string accountName,
			short? nature = null, string taxCode = null)
		{
			var account = new F_COMPTEG
			{
				CG_Num = accountNumber,
				CG_Intitule = accountName,
				N_Nature = nature,
				TA_Code = taxCode
			};

			return (await AddCompteg(account)).CG_Num;
		}

		// Common account creation helpers
		public async Task<string> CreateChargeAccount(string accountName, decimal? taxRate = null)
		{
			// Find next available charge account number (6xxxxx)
			string accountNumber = GetNextChargeAccountNumber();

			string taxCode = null;
			if (taxRate.HasValue)
			{
				taxCode = taxRate.Value switch
				{
					20m => "C20",
					5.5m => "C5.5",
					10m => "C10",
					_ => null
				};
			}

			return await QuickAddAccount(
				accountNumber,
				accountName,
				8, // Nature Charges
				taxCode
			);
		}

		public async Task<string> CreateRevenueAccount(string accountName, decimal? taxRate = null)
		{
			// Find next available revenue account number (7xxxxx)
			string accountNumber = GetNextRevenueAccountNumber();

			string taxCode = null;
			if (taxRate.HasValue)
			{
				taxCode = taxRate.Value switch
				{
					20m => "C20",
					5.5m => "C5.5",
					10m => "C10",
					_ => null
				};
			}

			return await QuickAddAccount(
				accountNumber,
				accountName,
				9, // Nature Produits
				taxCode
			);
		}

		public async Task<string> CreateTvaAccount(string accountName)
		{
			// Find next available TVA account number (445xxxx)
			string accountNumber = GetNextTvaAccountNumber();

			return await QuickAddAccount(
				accountNumber,
				accountName,
				0  // Nature TVA/Bilan
			);
		}

		// ===== GET NEXT ACCOUNT NUMBERS =====
		private string GetNextChargeAccountNumber()
		{
			var lastAccount = _context.F_COMPTEG
				.Where(c => c.CG_Num.StartsWith("6"))
				.OrderByDescending(c => c.CG_Num)
				.Select(c => c.CG_Num)
				.FirstOrDefault();

			if (string.IsNullOrEmpty(lastAccount))
				return "600000";

			if (int.TryParse(lastAccount, out int lastNumber))
				return (lastNumber + 1).ToString();

			return "600001";
		}

		private string GetNextRevenueAccountNumber()
		{
			var lastAccount = _context.F_COMPTEG
				.Where(c => c.CG_Num.StartsWith("7"))
				.OrderByDescending(c => c.CG_Num)
				.Select(c => c.CG_Num)
				.FirstOrDefault();

			if (string.IsNullOrEmpty(lastAccount))
				return "700000";

			if (int.TryParse(lastAccount, out int lastNumber))
				return (lastNumber + 1).ToString();

			return "700001";
		}

		private string GetNextTvaAccountNumber()
		{
			var lastAccount = _context.F_COMPTEG
				.Where(c => c.CG_Num.StartsWith("445"))
				.OrderByDescending(c => c.CG_Num)
				.Select(c => c.CG_Num)
				.FirstOrDefault();

			if (string.IsNullOrEmpty(lastAccount))
				return "44500000";

			if (int.TryParse(lastAccount, out int lastNumber))
				return (lastNumber + 1).ToString();

			return "44500001";
		}

		// ===== HELPER METHODS =====
		private string GenerateRaccourci(string intitule)
		{
			if (string.IsNullOrEmpty(intitule)) return "";

			// Take first part of the name
			string[] words = intitule.Split(' ');
			if (words.Length > 1)
			{
				// Take first letters of first two words
				return (words[0].Substring(0, Math.Min(4, words[0].Length)) +
					   words[1].Substring(0, Math.Min(4, words[1].Length))).ToUpper();
			}

			// Or just first 8 characters
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
		public F_COMPTEG GetCompteg(string cgNum)
		{
			return _context.F_COMPTEG.FirstOrDefault(c => c.CG_Num == cgNum);
		}

		public F_COMPTEG GetComptegByIntitule(string intitule)
		{
			return _context.F_COMPTEG.FirstOrDefault(c => c.CG_Intitule == intitule);
		}

		public List<F_COMPTEG> GetAccountsByNature(short nature)
		{
			return _context.F_COMPTEG
				.Where(c => c.N_Nature == nature)
				.OrderBy(c => c.CG_Num)
				.ToList();
		}

		public List<F_COMPTEG> GetChargeAccounts()
		{
			return _context.F_COMPTEG
				.Where(c => c.N_Nature == 8) // Charges
				.OrderBy(c => c.CG_Num)
				.ToList();
		}

		public List<F_COMPTEG> GetRevenueAccounts()
		{
			return _context.F_COMPTEG
				.Where(c => c.N_Nature == 9) // Produits
				.OrderBy(c => c.CG_Num)
				.ToList();
		}

		public List<F_COMPTEG> GetTvaAccounts()
		{
			return _context.F_COMPTEG
				.Where(c => c.CG_Num.StartsWith("445"))
				.OrderBy(c => c.CG_Num)
				.ToList();
		}

		public List<F_COMPTEG> SearchAccounts(string searchTerm, short? nature = null)
		{
			var query = _context.F_COMPTEG.AsQueryable();

			if (nature.HasValue)
				query = query.Where(c => c.N_Nature == nature.Value);

			return query
				.Where(c => c.CG_Intitule.Contains(searchTerm) ||
						   c.CG_Num.Contains(searchTerm) ||
						   c.CG_Classement.Contains(searchTerm))
				.Take(100)
				.ToList();
		}

		// ===== UPDATE METHOD =====
		public async Task<bool> UpdateCompteg(F_COMPTEG updatedRow)
		{
			var existing = GetCompteg(updatedRow.CG_Num);
			if (existing == null) return false;

			// Update fields
			existing.CG_Intitule = updatedRow.CG_Intitule ?? existing.CG_Intitule;
			existing.CG_Classement = updatedRow.CG_Classement ?? existing.CG_Classement;
			existing.N_Nature = updatedRow.N_Nature ?? existing.N_Nature;
			existing.TA_Code = updatedRow.TA_Code ?? existing.TA_Code;
			existing.CG_Report = updatedRow.CG_Report ?? existing.CG_Report;
			existing.CG_Sommeil = updatedRow.CG_Sommeil ?? existing.CG_Sommeil;

			// Update byte arrays if text changed
			if (updatedRow.CG_Intitule != existing.CG_Intitule && !string.IsNullOrEmpty(updatedRow.CG_Intitule))
			{
				existing.CG_Raccourci = GenerateRaccourci(updatedRow.CG_Intitule);
				existing.cbCG_Raccourci = GetBytes(existing.CG_Raccourci);
			}

			existing.cbModification = DateTime.Now;

			await _context.SaveChangesAsync();
			return true;
		}

		// ===== DELETE METHOD =====
		public async Task<bool> DeleteCompteg(string cgNum)
		{
			var compteg = GetCompteg(cgNum);
			if (compteg == null) return false;

			// Check if account has transactions
			var hasTransactions = _context.F_ECRITUREC.Any(e => e.CG_Num == cgNum);
			if (hasTransactions)
				throw new InvalidOperationException($"Account '{cgNum}' has transactions and cannot be deleted");

			_context.F_COMPTEG.Remove(compteg);
			await _context.SaveChangesAsync();
			return true;
		}

		// ===== VALIDATION METHODS =====
		public bool AccountExists(string cgNum)
		{
			return _context.F_COMPTEG.Any(c => c.CG_Num == cgNum);
		}

		public bool IsValidAccountNumber(string cgNum)
		{
			// Account numbers are variable length in your data (6-8 digits)
			if (string.IsNullOrEmpty(cgNum) || cgNum.Length < 3)
				return false;

			return int.TryParse(cgNum, out _);
		}

		// ===== GET TAX CODE FROM RATE =====
		public string GetTaxCodeFromRate(decimal rate)
		{
			return rate switch
			{
				20m => "C20",
				5.5m => "C5.5",
				10m => "C10",
				8.5m => "C8.5",
				_ => null
			};
		}

		// ===== BULK UPDATE =====
		public async Task<int> UpdateMultipleAccounts(List<F_COMPTEG> accounts)
		{
			int updatedCount = 0;

			foreach (var account in accounts)
			{
				if (await UpdateCompteg(account))
					updatedCount++;
			}

			return updatedCount;
		}
	}
}
