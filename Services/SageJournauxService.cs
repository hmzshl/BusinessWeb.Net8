namespace BusinessWeb.Services
{
	using BusinessWeb.Data;
	using BusinessWeb.Models.DB;
	// Services/SageJournauxService.cs
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

		// ===== MAIN METHOD: Add single F_JOURNAUX row =====
		public async Task<F_JOURNAUX> AddJournal(F_JOURNAUX row)
		{
			// Validate required fields
			if (string.IsNullOrEmpty(row.JO_Num))
				throw new ArgumentException("JO_Num is required");

			if (string.IsNullOrEmpty(row.JO_Intitule))
				throw new ArgumentException("JO_Intitule is required");

			// Check if journal code already exists
			if (_context.F_JOURNAUX.Any(j => j.JO_Num == row.JO_Num))
				throw new ArgumentException($"Journal code '{row.JO_Num}' already exists");

			// Set default values based on your data
			SetDefaultValues(row);

			// Set system fields
			row.cbCreateur = "SYSTEM";

			// Set byte arrays
			row.cbJO_Num = GetBytes(row.JO_Num);
			row.cbJO_Intitule = GetBytes(row.JO_Intitule);

			// Set byte array for CG_Num if provided
			if (!string.IsNullOrEmpty(row.CG_Num))
				row.cbCG_Num = GetBytes(row.CG_Num);

			// Add to database
			_context.F_JOURNAUX.Add(row);
			await _context.SaveChangesAsync();

			return row;
		}

		// ===== ADD MULTIPLE JOURNALS =====
		public async Task<List<F_JOURNAUX>> AddJournals(List<F_JOURNAUX> rows)
		{
			var results = new List<F_JOURNAUX>();

			foreach (var row in rows)
			{
				// Validate
				if (string.IsNullOrEmpty(row.JO_Num))
					throw new ArgumentException($"Row: JO_Num is required");

				if (string.IsNullOrEmpty(row.JO_Intitule))
					throw new ArgumentException($"Row: JO_Intitule is required");

				// Check duplicate
				if (_context.F_JOURNAUX.Any(j => j.JO_Num == row.JO_Num))
					throw new ArgumentException($"Row: Journal code '{row.JO_Num}' already exists");

				// Set defaults
				SetDefaultValues(row);

				// Set system fields
				row.cbCreateur = "SYSTEM";

				// Set byte arrays
				row.cbJO_Num = GetBytes(row.JO_Num);
				row.cbJO_Intitule = GetBytes(row.JO_Intitule);

				if (!string.IsNullOrEmpty(row.CG_Num))
					row.cbCG_Num = GetBytes(row.CG_Num);

				_context.F_JOURNAUX.Add(row);
				results.Add(row);
			}

			await _context.SaveChangesAsync();
			return results;
		}

		// ===== SET DEFAULT VALUES based on your data =====
		private void SetDefaultValues(F_JOURNAUX row)
		{
			// Default values from your sample data
			row.JO_Type = row.JO_Type ?? 0;  // 0,2,3 from your data
			row.JO_NumPiece = row.JO_NumPiece ?? 2;  // Always 2 in your data
			row.JO_Contrepartie = row.JO_Contrepartie ?? 0;  // 0 or 1 in your data
			row.JO_SaisAnal = row.JO_SaisAnal ?? 0;  // 0 or 1 in your data
			row.JO_NotCalcTot = row.JO_NotCalcTot ?? 0;  // Always 0 in your data
			row.JO_Rappro = row.JO_Rappro ?? 0;  // 0,1,2 in your data
			row.JO_Sommeil = row.JO_Sommeil ?? 0;  // Always 0 in your data
			row.JO_IFRS = row.JO_IFRS ?? 0;  // Always 0 in your data
			row.JO_Reglement = row.JO_Reglement ?? 0;  // 0 or 1 in your data
			row.JO_SuiviTreso = row.JO_SuiviTreso ?? 0;  // 0 or 1 in your data

			// Determine JO_Type based on journal code or name if not provided
			if (row.JO_Type == 0)
			{
				row.JO_Type = DetermineJournalType(row.JO_Num, row.JO_Intitule);
			}

			// Set JO_Rappro based on journal type
			if (row.JO_Rappro == 0)
			{
				row.JO_Rappro = DetermineReconciliationType(row.JO_Type ?? 0);
			}

			// CB fields
			row.cbProt = row.cbProt ?? 0;
			row.cbReplication = row.cbReplication ?? 0;
			row.cbFlag = row.cbFlag ?? 0;

		}

		// ===== DETERMINE JOURNAL TYPE =====
		private short DetermineJournalType(string journalCode, string journalName)
		{
			if (string.IsNullOrEmpty(journalCode) && string.IsNullOrEmpty(journalName))
				return 0; // Default

			// Check by code
			if (!string.IsNullOrEmpty(journalCode))
			{
				switch (journalCode.ToUpper())
				{
					case "ACH":
					case "VTE":
						return 0; // General accounting
					case "BEU":
					case "BRD":
					case "CAIS":
					case "ENCBEU":
					case "ENCBRD":
					case "ESBEU":
					case "ESBRD":
					case "PRB":
						return 2; // Bank/Cash journal
					case "OD":
						return 3; // Miscellaneous operations
				}
			}

			// Check by name
			if (!string.IsNullOrEmpty(journalName))
			{
				journalName = journalName.ToUpper();
				if (journalName.Contains("BANQUE") ||
					journalName.Contains("CAISSE") ||
					journalName.Contains("REMISE") ||
					journalName.Contains("PRÉLÈVEMENT"))
					return 2; // Bank/Cash
				if (journalName.Contains("ACHAT") || journalName.Contains("VENTE"))
					return 0; // General
				if (journalName.Contains("DIVERSE"))
					return 3; // Miscellaneous
			}

			return 0; // Default to general accounting
		}

		// ===== DETERMINE RECONCILIATION TYPE =====
		private short DetermineReconciliationType(short journalType)
		{
			return journalType switch
			{
				2 => 1,  // Bank journals: 1 point (most in your data)
				0 => 0,  // General journals: 0 point
				_ => 0   // Default: 0 point
			};
		}

		// ===== QUICK ADD JOURNAL METHODS =====
		public async Task<string> QuickAddJournal(
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

		// ===== CREATE SPECIFIC TYPES OF JOURNALS =====
		public async Task<string> CreateBankJournal(string bankName, string bankAccountNumber)
		{
			// Generate journal code from bank name (first 3-4 letters)
			string journalCode = GenerateJournalCode(bankName, "B");

			return await QuickAddJournal(
				journalCode,
				bankName,
				bankAccountNumber,
				2  // Bank journal type
			);
		}

		public async Task<string> CreateCashJournal()
		{
			return await QuickAddJournal(
				"CAIS",
				"Caisse",
				"5310",  // Standard cash account
				2  // Cash journal type
			);
		}

		public async Task<string> CreatePurchaseJournal(string supplierName = null)
		{
			string name = string.IsNullOrEmpty(supplierName) ? "Achats" : $"Achats {supplierName}";
			string code = string.IsNullOrEmpty(supplierName) ? "ACH" : GenerateJournalCode(supplierName, "ACH");

			return await QuickAddJournal(
				code,
				name,
				null,  // No specific account
				0  // General accounting
			);
		}

		public async Task<string> CreateSalesJournal(string customerName = null)
		{
			string name = string.IsNullOrEmpty(customerName) ? "Ventes" : $"Ventes {customerName}";
			string code = string.IsNullOrEmpty(customerName) ? "VTE" : GenerateJournalCode(customerName, "VTE");

			return await QuickAddJournal(
				code,
				name,
				null,  // No specific account
				0  // General accounting
			);
		}

		public async Task<string> CreateMiscJournal(string journalName)
		{
			string code = GenerateJournalCode(journalName, "OD");

			return await QuickAddJournal(
				code,
				journalName,
				null,  // No specific account
				3  // Miscellaneous operations
			);
		}

		// ===== HELPER METHODS =====
		private string GenerateJournalCode(string name, string prefix)
		{
			if (string.IsNullOrEmpty(name))
				return prefix + "001";

			// Take first letters of words
			string[] words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (words.Length >= 2)
			{
				return (words[0].Substring(0, Math.Min(2, words[0].Length)).ToUpper() +
					   words[1].Substring(0, Math.Min(2, words[1].Length)).ToUpper());
			}

			// Or take first 3-4 characters
			if (name.Length >= 4)
				return name.Substring(0, 4).ToUpper();

			return prefix + name.Substring(0, Math.Min(3, name.Length)).ToUpper();
		}

		private byte[] GetBytes(string text)
		{
			if (string.IsNullOrEmpty(text)) return new byte[0];
			return Encoding.UTF8.GetBytes(text);
		}

		// ===== QUERY METHODS =====
		public F_JOURNAUX GetJournal(string joNum)
		{
			return _context.F_JOURNAUX.FirstOrDefault(j => j.JO_Num == joNum);
		}

		public F_JOURNAUX GetJournalByIntitule(string intitule)
		{
			return _context.F_JOURNAUX.FirstOrDefault(j => j.JO_Intitule == intitule);
		}

		public List<F_JOURNAUX> GetJournalsByType(short journalType)
		{
			return _context.F_JOURNAUX
				.Where(j => j.JO_Type == journalType)
				.OrderBy(j => j.JO_Num)
				.ToList();
		}

		public List<F_JOURNAUX> GetBankJournals()
		{
			return _context.F_JOURNAUX
				.Where(j => j.JO_Type == 2) // Bank/Cash journals
				.OrderBy(j => j.JO_Num)
				.ToList();
		}

		public List<F_JOURNAUX> GetGeneralJournals()
		{
			return _context.F_JOURNAUX
				.Where(j => j.JO_Type == 0) // General accounting
				.OrderBy(j => j.JO_Num)
				.ToList();
		}

		public List<F_JOURNAUX> GetMiscJournals()
		{
			return _context.F_JOURNAUX
				.Where(j => j.JO_Type == 3) // Miscellaneous
				.OrderBy(j => j.JO_Num)
				.ToList();
		}

		public List<F_JOURNAUX> SearchJournals(string searchTerm, short? journalType = null)
		{
			var query = _context.F_JOURNAUX.AsQueryable();

			if (journalType.HasValue)
				query = query.Where(j => j.JO_Type == journalType.Value);

			return query
				.Where(j => j.JO_Intitule.Contains(searchTerm) ||
						   j.JO_Num.Contains(searchTerm))
				.Take(100)
				.ToList();
		}

		// ===== UPDATE METHOD =====
		public async Task<bool> UpdateJournal(F_JOURNAUX updatedRow)
		{
			var existing = GetJournal(updatedRow.JO_Num);
			if (existing == null) return false;

			// Update fields
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

			// Update byte arrays if text changed
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

		// ===== DELETE METHOD =====
		public async Task<bool> DeleteJournal(string joNum)
		{
			var journal = GetJournal(joNum);
			if (journal == null) return false;

			// Check if journal has transactions
			var hasTransactions = _context.F_ECRITUREC.Any(e => e.JO_Num == joNum);
			if (hasTransactions)
				throw new InvalidOperationException($"Journal '{joNum}' has transactions and cannot be deleted");

			_context.F_JOURNAUX.Remove(journal);
			await _context.SaveChangesAsync();
			return true;
		}

		// ===== VALIDATION METHODS =====
		public bool JournalExists(string joNum)
		{
			return _context.F_JOURNAUX.Any(j => j.JO_Num == joNum);
		}

		// ===== GET JOURNAL CODES BY TYPE =====
		public List<string> GetJournalCodesByType(short journalType)
		{
			return _context.F_JOURNAUX
				.Where(j => j.JO_Type == journalType)
				.Select(j => j.JO_Num)
				.OrderBy(j => j)
				.ToList();
		}

		// ===== BULK UPDATE =====
		public async Task<int> UpdateMultipleJournals(List<F_JOURNAUX> journals)
		{
			int updatedCount = 0;

			foreach (var journal in journals)
			{
				if (await UpdateJournal(journal))
					updatedCount++;
			}

			return updatedCount;
		}

		// ===== GET JOURNAL INFO =====
		public Dictionary<string, string> GetJournalInfo(string joNum)
		{
			var journal = GetJournal(joNum);
			if (journal == null) return null;

			return new Dictionary<string, string>
		{
			{ "Code", journal.JO_Num },
			{ "Name", journal.JO_Intitule },
			{ "Type", GetJournalTypeName(journal.JO_Type ?? 0) },
			{ "Account", journal.CG_Num ?? "N/A" },
			{ "Reconciliation", journal.JO_Rappro?.ToString() ?? "0" }
		};
		}

		private string GetJournalTypeName(short type)
		{
			return type switch
			{
				0 => "General Accounting",
				2 => "Bank/Cash",
				3 => "Miscellaneous",
				_ => "Unknown"
			};
		}

		// ===== ACTIVATE/DEACTIVATE JOURNAL =====
		public async Task<bool> ActivateJournal(string joNum)
		{
			var journal = GetJournal(joNum);
			if (journal == null) return false;

			journal.JO_Sommeil = 0; // 0 = Active
			journal.cbModification = DateTime.Now;

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeactivateJournal(string joNum)
		{
			var journal = GetJournal(joNum);
			if (journal == null) return false;

			journal.JO_Sommeil = 1; // 1 = Inactive (Sommeil)
			journal.cbModification = DateTime.Now;

			await _context.SaveChangesAsync();
			return true;
		}
	}
}
