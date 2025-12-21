using BusinessWeb.Models.Enum;

namespace BusinessWeb.Models.PersoAPI
{
	public class Document
	{
		public class DocumentEntete
		{
			// Document properties
			public DateTime Date { get; set; }
			public string Numero { get; set; }
			public string Reference { get; set; }
			public string Tiers { get; set; }
			public string Affaire { get; set; }
			public int Depot { get; set; }
			public short Souche { get; set; }
			public DocumentTypeDO Type { get; set; }

			// Document lines collection
			public List<DocumentLigne> Lignes { get; set; }

			public DocumentEntete()
			{
				Lignes = new List<DocumentLigne>();
			}

		}

		public class DocumentLigne
		{
			// Line properties
			public string RefArticle { get; set; }
			public string Designation { get; set; }
			public decimal Qte { get; set; }
			public decimal PU { get; set; }
			public string CodeTVA { get; set; }

			// Optional: Calculated property
			public decimal TotalHT => Qte * PU;
			public DocumentTypeDO Type { get; set; }
		}
		// ===== SUPPORTING CLASSES =====

		public class DocumentCreateResult
		{
			public bool Success { get; set; }
			public string Message { get; set; }
			public string DocumentNumber { get; set; }
			public int DocEnteteId { get; set; }
			public List<int> LigneIds { get; set; } = new List<int>();
			public Exception Error { get; set; }
		}

		public class ValidationResult
		{
			public bool IsValid { get; set; }
			public List<string> Errors { get; set; } = new List<string>();
			public List<string> Warnings { get; set; } = new List<string>();
		}
	}
}
