// DocCurrentPieceService.cs (simplified version)
using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Enum;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessWeb.Services
{
	public class DocCurrentPieceService
	{
		private readonly DB _context;

		public DocCurrentPieceService(DB context)
		{
			_context = context;
		}

		// ===== MAIN METHODS =====

		/// <summary>
		/// Gets the current piece number by checking existing documents
		/// </summary>
		public async Task<string> GetCurrentPieceNumber(short domaine, short dcId, short? souche = 0)
		{
			var info = DocumentTypeHelper.GetInfo(domaine, dcId);

			// First check if there's an entry in F_DOCCURRENTPIECE
			var currentPieceRecord = await _context.F_DOCCURRENTPIECE
				.FirstOrDefaultAsync(d =>
					d.DC_Domaine == domaine &&
					d.DC_IdCol == dcId &&
					d.DC_Souche == souche);

			// Now check if we need to get the highest existing piece number
			string highestPiece = await GetHighestExistingPieceNumber(info, souche);

			if (highestPiece != null)
			{
				// If we found a higher piece number in documents, use it
				return highestPiece;
			}

			// If no higher piece found in documents, use the value from F_DOCCURRENTPIECE or default
			return currentPieceRecord?.DC_Piece ?? $"{info.Prefix}00001";
		}

		/// <summary>
		/// Gets and reserves the next piece number
		/// </summary>
		public async Task<string> ReserveNextPieceNumber(short domaine, short dcId, short? souche = 0)
		{
			var info = DocumentTypeHelper.GetInfo(domaine, dcId);

			// Get the current piece (which will check if we need to use max from documents)
			string currentPiece = await GetCurrentPieceNumber(domaine, dcId, souche);

			// Increment the piece number
			string nextNumber = IncrementPieceNumber(currentPiece);

			// Update F_DOCCURRENTPIECE with the new number
			await UpdateCurrentPieceNumberInTable(domaine, dcId, souche, nextNumber);
			return nextNumber;
		}

		/// <summary>
		/// Gets the highest existing piece number from F_DOCENTETE and F_DOCLIGNE
		/// </summary>
		private async Task<string> GetHighestExistingPieceNumber(DocumentTypeInfo info, short? souche = 0)
		{
			// First, get the current piece from F_DOCCURRENTPIECE
			var currentPieceRecord = await _context.F_DOCCURRENTPIECE
				.FirstOrDefaultAsync(d =>
					d.DC_Domaine == info.Domaine &&
					d.DC_IdCol == info.DC_Id &&
					d.DC_Souche == souche);

			// If no entry exists in F_DOCCURRENTPIECE, return null
			if (currentPieceRecord == null || string.IsNullOrEmpty(currentPieceRecord.DC_Piece))
			{
				return null;
			}

			string currentPiece = currentPieceRecord.DC_Piece;

			// Convert DocumentTypeDO to DO_Type number
			short doType = DocumentTypeHelper.GetDONumberFromDocumentTypeDO(info.TypeDO);

			// Check if this piece exists in either table
			bool existsInEntete = await CheckIfPieceExistsInEntete(info.Domaine, doType, currentPiece, info.Provenance, souche);
			bool existsInLigne = await CheckIfPieceExistsInLigne(info.Domaine, doType, currentPiece, info);

			// If the current piece doesn't exist in documents, we can use it directly
			if (!existsInEntete && !existsInLigne)
			{
				return null;
			}

			// Get all piece numbers from both tables
			var allPieceNumbers = await GetAllPieceNumbersFromDocuments(info, doType, souche);

			if (!allPieceNumbers.Any())
			{
				return null;
			}
			else
			{
				return allPieceNumbers.Max();
			}

		}

		private async Task<List<string>> GetAllPieceNumbersFromDocuments(DocumentTypeInfo info, short doType, short? souche)
		{
			var pieceNumbers = new List<string>();

			// Get pieces from F_DOCENTETE
			var entetePieces = _context.F_DOCENTETE
				.Where(d => d.DO_Domaine == info.Domaine &&
						   d.DO_Type == doType &&
						   (souche == null || d.DO_Souche == souche) &&
						   d.DO_Piece != null).Select(a => a.DO_Piece);


			pieceNumbers.AddRange(entetePieces);

			// Get pieces from F_DOCLIGNE
			var lignePieces = await GetPieceNumbersFromLigne(info.Domaine, doType, info);
			pieceNumbers.AddRange(lignePieces);

			return pieceNumbers.Distinct().ToList();
		}

		private async Task<bool> CheckIfPieceExistsInEntete(short domaine, short doType, string pieceNumber, DocumentProvenance? provenance, short? souche)
		{
			var query = _context.F_DOCENTETE
				.Where(d => d.DO_Domaine == domaine &&
						   d.DO_Type == doType &&
						   d.DO_Piece == pieceNumber &&
						   (souche == null || d.DO_Souche == souche));
			return await query.AnyAsync();
		}

		private async Task<bool> CheckIfPieceExistsInLigne(short domaine, short doType, string pieceNumber, DocumentTypeInfo info)
		{
			// Combine all checks into a single query with OR conditions
			var query = _context.F_DOCLIGNE
				.Where(d => d.DO_Domaine == domaine)
				.Where(d => (d.DO_Piece == pieceNumber && d.DO_Type == doType) ||
						   (doType == 0 && d.DL_PieceDE == pieceNumber) ||
						   (doType == 1 && d.DL_PieceBC == pieceNumber) ||
						   (doType == 2 && d.DL_PiecePL == pieceNumber) ||
						   ((doType == 3 || doType == 4 || doType == 5) && d.DL_PieceBL == pieceNumber) ||
						   (doType == 10 && d.DL_PieceDE == pieceNumber) ||
						   (doType == 11 && d.DL_PieceBC == pieceNumber) ||
						   (doType == 12 && d.DL_PiecePL == pieceNumber) ||
						   ((doType == 13 || doType == 14 || doType == 15) && d.DL_PieceBL == pieceNumber));

			return await query.AnyAsync();
		}

		private async Task<List<string>> GetPieceNumbersFromLigne(short domaine, short doType, DocumentTypeInfo info)
		{
			var query = _context.F_DOCLIGNE
				.Where(d => d.DO_Domaine == domaine)
				.Where(d => ( d.DO_Type == doType) ||
						   (doType == 0 && d.DL_PieceDE != "") ||
						   (doType == 1 && d.DL_PieceBC != "") ||
						   (doType == 2 && d.DL_PiecePL != "") ||
						   ((doType == 3 || doType == 4 || doType == 5) && d.DL_PieceBL != "") ||
						   (doType == 10 && d.DL_PieceDE != "") ||
						   (doType == 11 && d.DL_PieceBC != "") ||
						   (doType == 12 && d.DL_PiecePL != "") ||
						   ((doType == 13 || doType == 14 || doType == 15) && d.DL_PieceBL != ""));

			var pieceNumbers = new List<string>();


			// Get values from specific piece fields based on doType
			switch (doType)
			{
				case 0: // Devis (DE)
					var dePieces = await query
						.Where(d => d.DL_PieceDE != "")
						.Select(d => d.DL_PieceDE)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(dePieces);
					break;
				case 1: // Bon Commande (BC)
					var bcPieces = await query
						.Where(d => d.DL_PieceBC != "")
						.Select(d => d.DL_PieceBC)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(bcPieces);
					break;
				case 2: // Préparation Livraison (PL)
					var plPieces = await query
						.Where(d => d.DL_PiecePL != "")
						.Select(d => d.DL_PiecePL)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(plPieces);
					break;
				case 3: // Bon Livraison (BL)
				case 4: // Bon Retour (BR)
				case 5: // Bon Avoir (BA)
					var blPieces = await query
						.Where(d => d.DL_PieceBL != "")
						.Select(d => d.DL_PieceBL)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(blPieces);
					break;
				case 10: // Demande Achat (DA)
					var daPieces = await query
						.Where(d => d.DL_PieceDE != "")
						.Select(d => d.DL_PieceDE)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(daPieces);
					break;
				case 11: // Préparation Commande (PC)
					var pcPieces = await query
						.Where(d => d.DL_PieceBC != "")
						.Select(d => d.DL_PieceBC)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(pcPieces);
					break;
				case 12: // Bon Commande Fournisseur (FBC)
					var fbcPieces = await query
						.Where(d => d.DL_PiecePL != "")
						.Select(d => d.DL_PiecePL)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(fbcPieces);
					break;
				case 13: // Bon Livraison Fournisseur (FBL)
				case 14: // Bon Retour Fournisseur (FBR)
				case 15: // Bon Avoir Fournisseur (FBA)
					var fblPieces = await query
						.Where(d => d.DL_PieceBL != "")
						.Select(d => d.DL_PieceBL)
						.Distinct()
						.ToListAsync();
					pieceNumbers.AddRange(fblPieces);
					break;
			}

			return pieceNumbers.Distinct().ToList();
		}





		// ===== CONVENIENCE METHODS =====

		/// <summary>
		/// Gets current piece number using DocumentDomaine and DocumentTypeDC
		/// </summary>
		public async Task<string> GetCurrentPieceNumber(DocumentDomaine domaine, DocumentTypeDC dcType, short? souche = 0)
		{
			short domaineNum = (short)domaine;
			short dcId = (short)dcType;
			return await GetCurrentPieceNumber(domaineNum, dcId, souche);
		}

		/// <summary>
		/// Reserves next piece number using DocumentDomaine and DocumentTypeDC
		/// </summary>
		public async Task<string> ReserveNextPieceNumber(DocumentDomaine domaine, DocumentTypeDC dcType, short? souche = 0)
		{
			short domaineNum = (short)domaine;
			short dcId = (short)dcType;
			return await ReserveNextPieceNumber(domaineNum, dcId, souche);
		}

		// ===== HELPER METHODS =====

		private string IncrementPieceNumber(string currentPiece)
		{
			if (string.IsNullOrEmpty(currentPiece))
				return "00001";

			// Extract the numeric part
			var match = Regex.Match(currentPiece, @"\d+$");
			if (match.Success)
			{
				string prefix = currentPiece.Substring(0, match.Index);
				string numericPartStr = match.Value;

				if (long.TryParse(numericPartStr, out long numericPart))
				{
					numericPart++;
					return prefix + numericPart.ToString(new string('0', numericPartStr.Length));
				}
			}

			// If no numeric part found, append "00001"
			return currentPiece + "00001";
		}

		private long ExtractNumericPart(string pieceNumber)
		{
			if (string.IsNullOrEmpty(pieceNumber))
				return 0;

			var match = Regex.Match(pieceNumber, @"\d+$");
			if (match.Success && long.TryParse(match.Value, out long result))
				return result;

			return 0;
		}

		// ===== DATABASE OPERATIONS =====

		private async Task UpdateCurrentPieceNumberInTable(short domaine, short idCol, short? souche, string pieceNumber)
		{
			var currentPiece = await _context.F_DOCCURRENTPIECE
				.FirstOrDefaultAsync(d =>
					d.DC_Domaine == domaine &&
					d.DC_IdCol == idCol &&
					d.DC_Souche == souche);

			var now = DateTime.Now;

			if (currentPiece == null)
			{
				var newEntry = new F_DOCCURRENTPIECE
				{
					DC_Domaine = domaine,
					DC_IdCol = idCol,
					DC_Souche = souche,
					DC_Piece = pieceNumber,
					cbProt = 0,
					cbCreateur = "BWB",
					cbModification = now,
					cbReplication = 0,
					cbFlag = 0,
					cbCreation = now,
					cbMarq = await GetNextMarq()
				};

				await CreateNewDocCurrentPieceWithBulkCopy(newEntry);
			}
			else
			{
				currentPiece.DC_Piece = pieceNumber;
				currentPiece.cbModification = now;
				currentPiece.cbCreateur = "BWB";
				await UpdateDocumentNumberWithBulkCopy(currentPiece);
			}
		}

		private async Task BulkInsertDocCurrentPieces(List<F_DOCCURRENTPIECE> pieces)
		{
			if (!pieces.Any()) return;
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(pieces);
		}

		private async Task BulkUpdateDocCurrentPieces(List<F_DOCCURRENTPIECE> updatedPieces)
		{
			if (!updatedPieces.Any()) return;

			var keys = updatedPieces
				.Select(p => new { p.DC_Domaine, p.DC_IdCol, p.DC_Souche })
				.Distinct()
				.ToList();

			using var db = _context.CreateLinqToDBConnection();

			foreach (var key in keys)
			{
				await db.GetTable<F_DOCCURRENTPIECE>()
					.Where(p => p.DC_Domaine == key.DC_Domaine &&
							   p.DC_IdCol == key.DC_IdCol &&
							   p.DC_Souche == key.DC_Souche)
					.DeleteAsync();
			}

			await db.BulkCopyAsync(updatedPieces);
		}

		private async Task CreateNewDocCurrentPieceWithBulkCopy(F_DOCCURRENTPIECE newPiece)
		{
			using var db = _context.CreateLinqToDBConnection();
			await db.BulkCopyAsync(new List<F_DOCCURRENTPIECE> { newPiece });
		}

		private async Task UpdateDocumentNumberWithBulkCopy(F_DOCCURRENTPIECE updatedPiece)
		{
			using var db = _context.CreateLinqToDBConnection();

			await db.GetTable<F_DOCCURRENTPIECE>()
				.Where(p => p.DC_Domaine == updatedPiece.DC_Domaine &&
						   p.DC_IdCol == updatedPiece.DC_IdCol &&
						   p.DC_Souche == updatedPiece.DC_Souche)
				.DeleteAsync();

			await db.BulkCopyAsync(new List<F_DOCCURRENTPIECE> { updatedPiece });
		}

		private async Task<int> GetNextMarq()
		{
			var maxMarq = await _context.F_DOCCURRENTPIECE
				.MaxAsync(d => (int?)d.cbMarq) ?? 0;
			return maxMarq + 1;
		}

		// ===== UTILITY METHODS =====

		/// <summary>
		/// Gets all document types for a specific domaine
		/// </summary>
		public List<DocumentTypeInfo> GetDocumentTypesByDomaine(short domaine)
		{
			return DocumentTypeHelper.GetByDomaine(domaine);
		}

		/// <summary>
		/// Gets document type information
		/// </summary>
		public DocumentTypeInfo GetDocumentTypeInfo(short domaine, short dcId)
		{
			return DocumentTypeHelper.GetInfo(domaine, dcId);
		}

		/// <summary>
		/// Gets the next piece number without reserving it
		/// </summary>
		public async Task<string> GetNextPieceNumber(short domaine, short dcId, short? souche = 0)
		{
			var currentNumber = await GetCurrentPieceNumber(domaine, dcId, souche);
			return IncrementPieceNumber(currentNumber);
		}

		/// <summary>
		/// Validates if a piece number is in the correct format
		/// </summary>
		public async Task<bool> ValidatePieceNumber(short domaine, short dcId, string pieceNumber, short? souche = 0)
		{
			var info = DocumentTypeHelper.GetInfo(domaine, dcId);

			// Check if piece number starts with the standard prefix
			if (!pieceNumber.StartsWith(info.Prefix))
				return false;

			// Check if it has a numeric part
			var match = Regex.Match(pieceNumber, @"\d+$");
			return match.Success;
		}

		/// <summary>
		/// Syncs all document numbers with actual documents
		/// </summary>
		public async Task SyncAllDocumentNumbers()
		{
			var allTypes = DocumentTypeHelper.GetAllDocumentTypes();

			foreach (var typeInfo in allTypes)
			{
				string highestPiece = await GetHighestExistingPieceNumber(typeInfo, 0);

				if (highestPiece != null)
				{
					await UpdateCurrentPieceNumberInTable(
						typeInfo.Domaine,
						typeInfo.DC_Id,
						0,
						highestPiece
					);
				}
			}
		}
	}
}