using BusinessWeb.Data;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Services
{
	public class SageEcritureService
	{
		private readonly DB _context;

		public SageEcritureService(DB context)
		{
			_context = context;
		}

		// ===== MAIN METHOD: Add single F_ECRITUREC row =====
		public async Task<F_ECRITUREC> AddEcriture(F_ECRITUREC row)
		{
			// 1. Ensure JMOUV exists for this journal/month
			await EnsureJmouvExists(row.JO_Num, row.EC_Date ?? DateTime.Now);

			// 2. Get next EC_No from entire table
			row.EC_No = await GetNextGlobalEcNo();

			// 3. Set JM_Date (first day of the month)
			row.JM_Date = new DateTime((row.EC_Date ?? DateTime.Now).Year,
									  (row.EC_Date ?? DateTime.Now).Month, 1);

			// 4. Set default values based on your data pattern
			SetDefaultValues(row);


			// 6. Add to database
			_context.F_ECRITUREC.Add(row);
			await _context.SaveChangesAsync();

			return row;
		}

		// ===== ADD MULTIPLE ROWS =====
		public async Task<List<F_ECRITUREC>> AddEcritures(List<F_ECRITUREC> rows)
		{
			var results = new List<F_ECRITUREC>();
			int startEcNo = await GetNextGlobalEcNo();
			int startMarq = await GetNextMarq();

			for (int i = 0; i < rows.Count; i++)
			{
				var row = rows[i];

				// Ensure JMOUV exists
				await EnsureJmouvExists(row.JO_Num, row.EC_Date ?? DateTime.Now);

				// Set sequential numbers
				row.EC_No = startEcNo + i;
				row.JM_Date = new DateTime((row.EC_Date ?? DateTime.Now).Year,
										  (row.EC_Date ?? DateTime.Now).Month, 1);

				// Set defaults
				SetDefaultValues(row);

				_context.F_ECRITUREC.Add(row);
				results.Add(row);
			}

			await _context.SaveChangesAsync();
			return results;
		}

		// ===== SET DEFAULT VALUES based on your data =====
		private void SetDefaultValues(F_ECRITUREC row)
		{
			// Default values from your sample data
			row.EC_NoLink = row.EC_NoLink ?? 0;
			row.EC_Jour = row.EC_Jour ?? (short?)(row.EC_Date?.Day ?? DateTime.Now.Day);

			// Default dates to 1753-01-01 (Sage default for empty dates)
			var defaultDate = new DateTime(1753, 1, 1);
			row.EC_Echeance = row.EC_Echeance ?? defaultDate;
			row.EC_DatePenal = row.EC_DatePenal ?? defaultDate;
			row.EC_DateRelance = row.EC_DateRelance ?? defaultDate;
			row.EC_DateRappro = row.EC_DateRappro ?? defaultDate;
			row.EC_DateRegle = row.EC_DateRegle ?? defaultDate;
			row.EC_DateOp = row.EC_DateOp ?? defaultDate;

			// Default numeric values
			row.EC_Parite = row.EC_Parite ?? 0;
			row.EC_Quantite = row.EC_Quantite ?? 0;
			row.N_Devise = row.N_Devise ?? 0;
			row.EC_Montant = row.EC_Montant ?? 0;
			row.EC_Devise = row.EC_Devise ?? 0;
			row.EC_MontantRegle = row.EC_MontantRegle ?? 0;

			// Default flags
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
			row.EC_StatusRegle = row.EC_StatusRegle ?? 0;
			row.EC_RIB = row.EC_RIB ?? 0;
			row.EC_NoCloture = row.EC_NoCloture ?? 0;
			row.EC_ExportRappro = row.EC_ExportRappro ?? 0;

			// CB fields
			row.cbProt = row.cbProt ?? 0;
			row.cbReplication = row.cbReplication ?? 0;
			row.cbFlag = row.cbFlag ?? 0;

			// TEMPORARY: Clear all navigation properties before saving
			// This helps isolate if the problem is in F_ECRITUREC or related entities
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

		// ===== JMOUV MANAGEMENT =====
		private async Task EnsureJmouvExists(string journalCode, DateTime date)
		{
			DateTime firstOfMonth = new DateTime(date.Year, date.Month, 1);

			bool exists = _context.F_JMOUV
				.Any(j => j.JO_Num == journalCode && j.JM_Date == firstOfMonth);

			if (!exists)
			{
				var jmouv = new F_JMOUV
				{
					JO_Num = journalCode,
					JM_Date = firstOfMonth,
					JM_Cloture = 0, // Non clôturé
					JM_Impression = 0, // Non imprimé
					cbProt = 0,
					cbCreateur = "BWB"
				};

				_context.F_JMOUV.Add(jmouv);
				await _context.SaveChangesAsync();
			}
		}

		// ===== HELPER METHODS =====
		private async Task<int> GetNextGlobalEcNo()
		{
			return _context.F_ECRITUREC
				.Max(e => (int?)e.EC_No) ?? 0 + 1;
		}

		private async Task<int> GetNextMarq()
		{
			return _context.F_ECRITUREC
				.Max(e => (int?)e.cbMarq) ?? 0 + 1;
		}

		private async Task<int> GetNextJmouvMarq()
		{
			return _context.F_JMOUV
				.Max(j => (int?)j.cbMarq) ?? 0 + 1;
		}

		// ===== QUERY METHODS =====
		public F_ECRITUREC GetEcriture(int ecNo)
		{
			return _context.F_ECRITUREC.FirstOrDefault(e => e.EC_No == ecNo);
		}

		public List<F_ECRITUREC> GetEcrituresByJournal(string journalCode, DateTime? fromDate = null)
		{
			var query = _context.F_ECRITUREC.Where(e => e.JO_Num == journalCode);

			if (fromDate.HasValue)
				query = query.Where(e => e.EC_Date >= fromDate.Value);

			return query.OrderBy(e => e.EC_Date).ThenBy(e => e.EC_No).ToList();
		}

		public List<F_ECRITUREC> GetEcrituresByPiece(string pieceNumber)
		{
			return _context.F_ECRITUREC
				.Where(e => e.EC_Piece == pieceNumber)
				.OrderBy(e => e.EC_Date)
				.ToList();
		}

		public List<F_ECRITUREC> GetEcrituresByClient(string clientCode, DateTime? fromDate = null)
		{
			var query = _context.F_ECRITUREC.Where(e => e.CT_Num == clientCode);

			if (fromDate.HasValue)
				query = query.Where(e => e.EC_Date >= fromDate.Value);

			return query.OrderBy(e => e.EC_Date).ToList();
		}

		public List<F_ECRITUREC> GetEcrituresByCompte(string compteGeneral, DateTime? fromDate = null)
		{
			var query = _context.F_ECRITUREC.Where(e => e.CG_Num == compteGeneral);

			if (fromDate.HasValue)
				query = query.Where(e => e.EC_Date >= fromDate.Value);

			return query.OrderBy(e => e.EC_Date).ToList();
		}

		// ===== UPDATE METHOD =====
		public async Task<bool> UpdateEcriture(F_ECRITUREC updatedRow)
		{
			var existing = GetEcriture(updatedRow.EC_No);
			if (existing == null) return false;

			// Update all fields except EC_No, cbMarq, cbCreation
			existing.JO_Num = updatedRow.JO_Num;
			existing.EC_Date = updatedRow.EC_Date;
			existing.EC_Piece = updatedRow.EC_Piece;
			existing.EC_RefPiece = updatedRow.EC_RefPiece;
			existing.CG_Num = updatedRow.CG_Num;
			existing.CT_Num = updatedRow.CT_Num;
			existing.EC_Intitule = updatedRow.EC_Intitule;
			existing.EC_Montant = updatedRow.EC_Montant;
			existing.EC_Sens = updatedRow.EC_Sens;
			// ... update other fields as needed

			existing.cbModification = DateTime.Now;

			await _context.SaveChangesAsync();
			return true;
		}

		// ===== DELETE METHOD =====
		public async Task<bool> DeleteEcriture(int ecNo)
		{
			var ecriture = GetEcriture(ecNo);
			if (ecriture == null) return false;

			_context.F_ECRITUREC.Remove(ecriture);
			await _context.SaveChangesAsync();
			return true;
		}

		// ===== QUICK ADD METHODS (optional helpers) =====
		public async Task<F_ECRITUREC> QuickAdd(
			string journal, DateTime date, string piece, string compte,
			string libelle, decimal montant, int sens, string clientFournisseur = null)
		{
			var row = new F_ECRITUREC
			{
				JO_Num = journal,
				EC_Date = date,
				EC_Piece = piece,
				CG_Num = compte,
				CT_Num = clientFournisseur,
				EC_Intitule = libelle,
				EC_Montant = montant,
				EC_Sens = (short?)sens
			};

			return await AddEcriture(row);
		}
	}
}
