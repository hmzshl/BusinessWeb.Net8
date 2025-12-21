// DocumentTypeHelper.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessWeb.Models.Enum
{
	/// <summary>
	/// Complete information about a document type including labels
	/// </summary>
	public class DocumentTypeInfo
	{
		public short Domaine { get; set; }
		public short DC_Id { get; set; }  // For F_DOCCURRENTPIECE (0-8 within each domain)
		public DocumentTypeDO TypeDO { get; set; }  // For F_DOCENTETE (unique across all domains)
		public DocumentProvenance? Provenance { get; set; }
		public string Label { get; set; }
		public string Prefix { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }

		public DocumentTypeDCKey GetDCKey() => new DocumentTypeDCKey(Domaine, DC_Id);
		public DocumentDomaine GetDomaineEnum() => (DocumentDomaine)Domaine;
		public DocumentTypeDC GetDCTypEnum() => (DocumentTypeDC)DC_Id;
	}

	/// <summary>
	/// Document type helper with all type information
	/// </summary>
	public static class DocumentTypeHelper
	{
		// Map from (Domaine, DC_Id) to DocumentTypeInfo
		private static readonly Dictionary<DocumentTypeDCKey, DocumentTypeInfo> _documentTypeInfos;

		// Map from DocumentTypeDO to DocumentTypeInfo (for quick lookup from F_DOCENTETE)
		private static readonly Dictionary<DocumentTypeDO, List<DocumentTypeInfo>> _documentTypeDOToInfos;

		static DocumentTypeHelper()
		{
			_documentTypeInfos = new Dictionary<DocumentTypeDCKey, DocumentTypeInfo>();
			_documentTypeDOToInfos = new Dictionary<DocumentTypeDO, List<DocumentTypeInfo>>();

			InitializeDocumentTypes();
		}

		private static void InitializeDocumentTypes()
		{
			// === VENTE (Domaine = 0) ===
			AddDocumentType(0, 0, DocumentTypeDO.DevisClient, "DE", "Document de devis pour clients", "Devis Client");
			AddDocumentType(0, 1, DocumentTypeDO.BonCommandeClient, "BC", "Bon de commande client", "Bon de Commande Client");
			AddDocumentType(0, 2, DocumentTypeDO.PreparationLivraisonClient, "PL", "Préparation de livraison", "Préparation de Livraison Client");
			AddDocumentType(0, 3, DocumentTypeDO.BonLivraisonClient, "BL", "Bon de livraison", "Bon de Livraison Client");
			AddDocumentType(0, 4, DocumentTypeDO.BonRetourClient, "BR", "Bon de retour client", "Bon de Retour Client");
			AddDocumentType(0, 5, DocumentTypeDO.BonAvoirClient, "BA", "Bon d'avoir client", "Bon d'Avoir Client");

			// Invoices for Vente
			AddDocumentType(0, 6, DocumentTypeDO.FactureVente, "FA", "Facture client", "Facture Client", DocumentProvenance.Normal);
			AddDocumentType(0, 7, DocumentTypeDO.FactureVente, "FR", "Facture de retour", "Facture Retour Client", DocumentProvenance.Retour);
			AddDocumentType(0, 8, DocumentTypeDO.FactureVente, "FV", "Facture d'avoir", "Facture Avoir Client", DocumentProvenance.Avoir);

			// === ACHAT (Domaine = 1) ===
			AddDocumentType(1, 0, DocumentTypeDO.DemandeAchatFournisseur, "DA", "Demande d'achat fournisseur", "Demande d'Achat Fournisseur");
			AddDocumentType(1, 1, DocumentTypeDO.PreparationCommandeFournisseur, "PC", "Préparation de commande fournisseur", "Préparation Commande Fournisseur");
			AddDocumentType(1, 2, DocumentTypeDO.BonCommandeFournisseur, "FBC", "Bon de commande fournisseur", "Bon de Commande Fournisseur");
			AddDocumentType(1, 3, DocumentTypeDO.BonLivraisonFournisseur, "FBL", "Bon de livraison fournisseur", "Bon de Livraison Fournisseur");
			AddDocumentType(1, 4, DocumentTypeDO.BonRetourFournisseur, "FBR", "Bon de retour fournisseur", "Bon de Retour Fournisseur");
			AddDocumentType(1, 5, DocumentTypeDO.BonAvoirFournisseur, "FBA", "Bon d'avoir fournisseur", "Bon d'Avoir Fournisseur");

			// Invoices for Achat
			AddDocumentType(1, 6, DocumentTypeDO.FactureAchat, "FFA", "Facture fournisseur", "Facture Fournisseur", DocumentProvenance.Normal);
			AddDocumentType(1, 7, DocumentTypeDO.FactureAchat, "FRF", "Facture de retour", "Facture Retour Fournisseur", DocumentProvenance.Retour);
			AddDocumentType(1, 8, DocumentTypeDO.FactureAchat, "FRV", "Facture d'avoir", "Facture Avoir Fournisseur", DocumentProvenance.Avoir);

			// === STOCK (Domaine = 2) ===
			AddDocumentType(2, 0, DocumentTypeDO.MouvementEntree, "ME", "Mouvement d'entrée en stock", "Mouvement d'Entrée");
			AddDocumentType(2, 1, DocumentTypeDO.MouvementSortie, "MS", "Mouvement de sortie de stock", "Mouvement de Sortie");
			AddDocumentType(2, 2, DocumentTypeDO.Declassement, "DS", "Déclassement de stock", "Déclassement");
			AddDocumentType(2, 3, DocumentTypeDO.Transfert, "MT", "Transfert entre dépôts", "Transfert");
			AddDocumentType(2, 4, DocumentTypeDO.PreparationFabrication, "PF", "Préparation de fabrication", "Préparation de Fabrication");
			AddDocumentType(2, 5, DocumentTypeDO.OrdreFabrication, "OF", "Ordre de fabrication", "Ordre de Fabrication");
			AddDocumentType(2, 6, DocumentTypeDO.BonFabrication, "BF", "Bon de fabrication", "Bon de Fabrication");

			// === AUTRE (Domaine = 4) ===
			AddDocumentType(4, 0, DocumentTypeDO.DocumentInterne1, "CA", "Document interne caisse", "Document Interne 1");
			AddDocumentType(4, 1, DocumentTypeDO.DocumentInterne2, "BI", "Document interne banque", "Document Interne 2");
			AddDocumentType(4, 2, DocumentTypeDO.DocumentInterne3, "RC", "Document interne recette", "Document Interne 3");
			AddDocumentType(4, 3, DocumentTypeDO.DocumentInterne4, "SP", "Document interne sortie permanente", "Document Interne 4");
			AddDocumentType(4, 4, DocumentTypeDO.DocumentInterne5, "RP", "Document interne règlement permanent", "Document Interne 5");
			AddDocumentType(4, 6, DocumentTypeDO.DocumentInterne6, "SR", "Document interne situation recette", "Document Interne 6");
		}

		private static void AddDocumentType(short domaine, short dcId, DocumentTypeDO typeDO,
			string prefix, string description, string label, DocumentProvenance? provenance = null)
		{
			// Generate code from domaine and dcId
			string code = $"D{domaine}T{dcId}";

			var info = new DocumentTypeInfo
			{
				Domaine = domaine,
				DC_Id = dcId,
				TypeDO = typeDO,
				Label = label,
				Prefix = prefix,
				Description = description,
				Provenance = provenance,
				Code = code
			};

			var key = info.GetDCKey();
			_documentTypeInfos[key] = info;

			// Add to DO type mapping
			if (!_documentTypeDOToInfos.ContainsKey(typeDO))
			{
				_documentTypeDOToInfos[typeDO] = new List<DocumentTypeInfo>();
			}
			_documentTypeDOToInfos[typeDO].Add(info);
		}

		// ===== PUBLIC METHODS =====

		/// <summary>
		/// Gets DocumentTypeInfo by domaine and DC_Id (for F_DOCCURRENTPIECE)
		/// </summary>
		public static DocumentTypeInfo GetInfo(short domaine, short dcId)
		{
			var key = new DocumentTypeDCKey(domaine, dcId);
			if (_documentTypeInfos.TryGetValue(key, out var info))
				return info;

			throw new ArgumentException($"No DocumentTypeInfo found for Domaine={domaine}, DC_Id={dcId}");
		}

		/// <summary>
		/// Gets DocumentTypeInfo by DocumentDomaine and DocumentTypeDC
		/// </summary>
		public static DocumentTypeInfo GetInfo(DocumentDomaine domaine, DocumentTypeDC dcType)
		{
			return GetInfo((short)domaine, (short)dcType);
		}

		/// <summary>
		/// Gets DocumentTypeInfo by DocumentTypeDO (for F_DOCENTETE)
		/// If multiple exist (e.g., invoices), use provenance to differentiate
		/// </summary>
		public static DocumentTypeInfo GetInfoByDO(short domaine, DocumentTypeDO typeDO, short? provenance = null)
		{
			if (_documentTypeDOToInfos.TryGetValue(typeDO, out var infos))
			{
				// Filter by domaine
				var filteredInfos = infos.Where(i => i.Domaine == domaine).ToList();

				if (filteredInfos.Count == 1)
					return filteredInfos[0];

				if (filteredInfos.Count > 1 && provenance.HasValue)
				{
					// For invoices, use provenance to differentiate
					var provenanceEnum = (DocumentProvenance)provenance.Value;
					return filteredInfos.FirstOrDefault(i => i.Provenance == provenanceEnum)
						?? filteredInfos.FirstOrDefault(i => i.Provenance == null)
						?? filteredInfos.First();
				}

				return filteredInfos.FirstOrDefault();
			}

			throw new ArgumentException($"No DocumentTypeInfo found for DocumentTypeDO={typeDO}");
		}

		/// <summary>
		/// Gets DocumentTypeInfo from F_DOCENTETE columns
		/// </summary>
		public static DocumentTypeInfo GetInfoFromDOColumns(short domaine, short doType, short? provenance = null)
		{
			var typeDO = GetDocumentTypeDOFromDONumbers(domaine, doType);
			return GetInfoByDO(domaine, typeDO, provenance);
		}

		/// <summary>
		/// Converts DO_Domaine and DO_Type to DocumentTypeDO
		/// </summary>
		public static DocumentTypeDO GetDocumentTypeDOFromDONumbers(short domaine, short doType)
		{
			return (domaine, doType) switch
			{
				// Vente (Domaine = 0)
				(0, 0) => DocumentTypeDO.DevisClient,
				(0, 1) => DocumentTypeDO.BonCommandeClient,
				(0, 2) => DocumentTypeDO.PreparationLivraisonClient,
				(0, 3) => DocumentTypeDO.BonLivraisonClient,
				(0, 4) => DocumentTypeDO.BonRetourClient,
				(0, 5) => DocumentTypeDO.BonAvoirClient,
				(0, 6) => DocumentTypeDO.FactureVente,

				// Achat (Domaine = 1)
				(1, 0) => DocumentTypeDO.DemandeAchatFournisseur,
				(1, 1) => DocumentTypeDO.PreparationCommandeFournisseur,
				(1, 2) => DocumentTypeDO.BonCommandeFournisseur,
				(1, 3) => DocumentTypeDO.BonLivraisonFournisseur,
				(1, 4) => DocumentTypeDO.BonRetourFournisseur,
				(1, 5) => DocumentTypeDO.BonAvoirFournisseur,
				(1, 6) => DocumentTypeDO.FactureAchat,

				// Stock (Domaine = 2)
				(2, 0) => DocumentTypeDO.MouvementEntree,
				(2, 1) => DocumentTypeDO.MouvementSortie,
				(2, 2) => DocumentTypeDO.Declassement,
				(2, 3) => DocumentTypeDO.Transfert,
				(2, 4) => DocumentTypeDO.PreparationFabrication,
				(2, 5) => DocumentTypeDO.OrdreFabrication,
				(2, 6) => DocumentTypeDO.BonFabrication,

				// Autre (Domaine = 4)
				(4, 0) => DocumentTypeDO.DocumentInterne1,
				(4, 1) => DocumentTypeDO.DocumentInterne2,
				(4, 2) => DocumentTypeDO.DocumentInterne3,
				(4, 3) => DocumentTypeDO.DocumentInterne4,
				(4, 4) => DocumentTypeDO.DocumentInterne5,
				(4, 6) => DocumentTypeDO.DocumentInterne6,

				_ => throw new ArgumentException($"Unknown DO_Domaine={domaine}, DO_Type={doType}")
			};
		}

		/// <summary>
		/// Converts DocumentTypeDO to DO_Type number
		/// </summary>
		public static short GetDONumberFromDocumentTypeDO(DocumentTypeDO typeDO)
		{
			return (short)((int)typeDO % 10);
		}

		/// <summary>
		/// Gets the domaine from DocumentTypeDO
		/// </summary>
		public static short GetDomaineFromDocumentTypeDO(DocumentTypeDO typeDO)
		{
			return (short)((int)typeDO / 10);
		}

		/// <summary>
		/// Gets all document types for a specific domaine
		/// </summary>
		public static List<DocumentTypeInfo> GetByDomaine(short domaine)
		{
			return _documentTypeInfos.Values
				.Where(info => info.Domaine == domaine)
				.OrderBy(info => info.DC_Id)
				.ToList();
		}

		/// <summary>
		/// Gets all document types for a specific DocumentDomaine
		/// </summary>
		public static List<DocumentTypeInfo> GetByDomaine(DocumentDomaine domaine)
		{
			return GetByDomaine((short)domaine);
		}

		/// <summary>
		/// Gets label by domaine and DC_Id
		/// </summary>
		public static string GetLabel(short domaine, short dcId)
		{
			return GetInfo(domaine, dcId).Label;
		}

		/// <summary>
		/// Gets prefix by domaine and DC_Id
		/// </summary>
		public static string GetPrefix(short domaine, short dcId)
		{
			return GetInfo(domaine, dcId).Prefix;
		}

		/// <summary>
		/// Gets all DocumentTypeDO options for a domaine
		/// </summary>
		public static List<DocumentTypeDO> GetDOTypeOptions(short domaine)
		{
			return GetByDomaine(domaine)
				.Select(info => info.TypeDO)
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// Gets all DocumentTypeInfo for F_DOCENTETE dropdown
		/// </summary>
		public static List<DocumentTypeInfo> GetDOTypeInfosForDropdown(short domaine)
		{
			var result = new List<DocumentTypeInfo>();
			var infos = GetByDomaine(domaine);

			foreach (var info in infos)
			{
				// For non-invoice types, add directly
				if (!IsInvoiceType(info.TypeDO))
				{
					result.Add(info);
				}
				else
				{
					// For invoices, we already have separate entries for each provenance
					result.Add(info);
				}
			}

			return result.OrderBy(i => i.DC_Id).ToList();
		}

		private static bool IsInvoiceType(DocumentTypeDO typeDO)
		{
			return typeDO == DocumentTypeDO.FactureVente || typeDO == DocumentTypeDO.FactureAchat;
		}

		/// <summary>
		/// Gets the standard DocumentTypeDC for a DocumentTypeDO within a domaine
		/// </summary>
		public static DocumentTypeDC? GetDocumentTypeDC(short domaine, DocumentTypeDO typeDO, DocumentProvenance? provenance = null)
		{
			var infos = GetByDomaine(domaine)
				.Where(info => info.TypeDO == typeDO)
				.ToList();

			if (infos.Count == 0)
				return null;

			if (provenance.HasValue)
			{
				var infoWithProvenance = infos.FirstOrDefault(i => i.Provenance == provenance.Value);
				if (infoWithProvenance != null)
					return infoWithProvenance.GetDCTypEnum();
			}

			return infos.First().GetDCTypEnum();
		}

		/// <summary>
		/// Gets all DocumentTypeInfo entries
		/// </summary>
		public static List<DocumentTypeInfo> GetAllDocumentTypes()
		{
			return _documentTypeInfos.Values
				.OrderBy(info => info.Domaine)
				.ThenBy(info => info.DC_Id)
				.ToList();
		}

		/// <summary>
		/// Gets document type by code
		/// </summary>
		public static DocumentTypeInfo GetInfoByCode(string code)
		{
			return _documentTypeInfos.Values
				.FirstOrDefault(info => info.Code == code);
		}

		/// <summary>
		/// Gets document type by prefix
		/// </summary>
		public static DocumentTypeInfo GetInfoByPrefix(string prefix)
		{
			return _documentTypeInfos.Values
				.FirstOrDefault(info => info.Prefix == prefix);
		}

		/// <summary>
		/// Gets the default provenance for a document type
		/// </summary>
		public static DocumentProvenance? GetDefaultProvenance(short domaine, short dcId)
		{
			var info = GetInfo(domaine, dcId);
			return info.Provenance;
		}

		/// <summary>
		/// Checks if a document type requires provenance
		/// </summary>
		public static bool RequiresProvenance(short domaine, short dcId)
		{
			var info = GetInfo(domaine, dcId);
			return info.TypeDO == DocumentTypeDO.FactureVente || info.TypeDO == DocumentTypeDO.FactureAchat;
		}

		/// <summary>
		/// Gets all provenance options for a document type
		/// </summary>
		public static List<DocumentProvenance> GetProvenanceOptions(short domaine, short dcId)
		{
			var info = GetInfo(domaine, dcId);

			if (info.TypeDO == DocumentTypeDO.FactureVente || info.TypeDO == DocumentTypeDO.FactureAchat)
			{
				return new List<DocumentProvenance>
				{
					DocumentProvenance.Normal,
					DocumentProvenance.Retour,
					DocumentProvenance.Avoir
				};
			}

			return new List<DocumentProvenance>();
		}

		/// <summary>
		/// Gets the DocumentTypeInfo for an invoice by domaine and provenance
		/// </summary>
		public static DocumentTypeInfo GetInvoiceInfo(short domaine, DocumentProvenance provenance)
		{
			var invoiceTypeDO = domaine == 0 ? DocumentTypeDO.FactureVente : DocumentTypeDO.FactureAchat;
			return GetInfoByDO(domaine, invoiceTypeDO, (short)provenance);
		}
	}
}