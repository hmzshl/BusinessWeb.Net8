using BusinessWeb.Models.Enum;

namespace BusinessWeb.Models.PersoAPI
{
	public class Document
	{
		public class DocumentEntete
		{
			// Document properties
			public DateTime Date { get; set; }
			public DateTime DateLivraison { get; set; }
			public string Numero { get; set; }
			public string Reference { get; set; }
			public string Entete1 { get; set; }
			public string Entete2 { get; set; }
			public string Entete3 { get; set; }
			public string Entete4 { get; set; }
			public string Tiers { get; set; }
			public string Affaire { get; set; }
			public int? Depot { get; set; }
			public short? Statut { get; set; }
			public short? Souche { get; set; }
			public DocumentTypeDO TypeDO { get; set; }
			public DocumentType documentType
			{
				get
				{
					return ConvertFromDOType(this.TypeDO);
				}

				set
				{

				}
			}

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
			public decimal? Qte { get; set; }
			public decimal? PU { get; set; }
			public decimal? TauxTaxe { get; set; }
			public string CodeTVA { get; set; }

			// Optional: Calculated property
			public decimal? TotalHT => Qte * PU;
			public DocumentTypeDO TypeDO { get; set; }
			public DocumentType documentType
			{
				get
				{
					return ConvertFromDOType(this.TypeDO);
				}

				set
				{

				}
			}
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
		public static DocumentType ConvertFromDOType(DocumentTypeDO doType)
		{
			var documentType = new DocumentType();

			// Determine the domain based on the DO type value
			if ((int)doType >= 0 && (int)doType <= 9)
			{
				// Vente domain
				documentType.Domaine = (short)DocumentDomaine.Vente;
				documentType.DO_Id = (short)doType;

				// Map DO type to DC type for Vente domain and set Mvt value
				switch (doType)
				{
					case DocumentTypeDO.DevisClient:
						documentType.DC_Id = (short)DocumentTypeDC.DevisClient;
						documentType.Mvt = (short)DocumentTypeMVT.DevisClient;
						break;
					case DocumentTypeDO.BonCommandeClient:
						documentType.DC_Id = (short)DocumentTypeDC.BonCommandeClient;
						documentType.Mvt = (short)DocumentTypeMVT.BonCommandeClient;
						break;
					case DocumentTypeDO.PreparationLivraisonClient:
						documentType.DC_Id = (short)DocumentTypeDC.PreparationLivraisonClient;
						documentType.Mvt = (short)DocumentTypeMVT.PreparationLivraisonClient;
						break;
					case DocumentTypeDO.BonLivraisonClient:
						documentType.DC_Id = (short)DocumentTypeDC.BonLivraisonClient;
						documentType.Mvt = (short)DocumentTypeMVT.BonLivraisonClient;
						break;
					case DocumentTypeDO.BonRetourClient:
						documentType.DC_Id = (short)DocumentTypeDC.BonRetourClient;
						documentType.Mvt = (short)DocumentTypeMVT.BonRetourClient;
						break;
					case DocumentTypeDO.BonAvoirClient:
						documentType.DC_Id = (short)DocumentTypeDC.BonAvoirClient;
						documentType.Mvt = (short)DocumentTypeMVT.BonAvoirClient;
						break;
					case DocumentTypeDO.FactureVente:
						// FactureVente needs to be handled specially based on provenance
						// Default to normal facture (DO_Id already set to 6)
						documentType.DC_Id = (short)DocumentTypeDC.FactureClient;
						documentType.Provenance = (short)DocumentProvenance.Normal;
						documentType.Mvt = (short)DocumentTypeMVT.FactureClient;
						break;
				}
			}
			else if ((int)doType >= 10 && (int)doType <= 19)
			{
				// Achat domain
				documentType.Domaine = (short)DocumentDomaine.Achat;
				documentType.DO_Id = (short)doType;

				// Adjust DO_Id to match the offset in F_DOCENTETE
				short doId = (short)((int)doType - 10);
				documentType.DO_Id = doId;

				// Map DO type to DC type for Achat domain and set Mvt value
				switch (doType)
				{
					case DocumentTypeDO.DemandeAchatFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.DemandeAchatFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.DemandeAchatFournisseur;
						break;
					case DocumentTypeDO.PreparationCommandeFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.PreparationCommandeFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.PreparationCommandeFournisseur;
						break;
					case DocumentTypeDO.BonCommandeFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.BonCommandeFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.BonCommandeFournisseur;
						break;
					case DocumentTypeDO.BonLivraisonFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.BonLivraisonFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.BonLivraisonFournisseur;
						break;
					case DocumentTypeDO.BonRetourFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.BonRetourFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.BonRetourFournisseur;
						break;
					case DocumentTypeDO.BonAvoirFournisseur:
						documentType.DC_Id = (short)DocumentTypeDC.BonAvoirFournisseur;
						documentType.Mvt = (short)DocumentTypeMVT.BonAvoirFournisseur;
						break;
					case DocumentTypeDO.FactureAchat:
						// FactureAchat needs to be handled specially based on provenance
						// Default to normal facture (DO_Id already set to 16)
						documentType.DC_Id = (short)DocumentTypeDC.FactureFournisseur;
						documentType.Provenance = (short)DocumentProvenance.Normal;
						documentType.Mvt = (short)DocumentTypeMVT.FactureFournisseur;
						break;
				}
			}
			else if ((int)doType >= 20 && (int)doType <= 29)
			{
				// Stock domain
				documentType.Domaine = (short)DocumentDomaine.Stock;
				documentType.DO_Id = (short)doType;

				// Adjust DO_Id to match the offset in F_DOCENTETE
				short doId = (short)((int)doType - 20);
				documentType.DO_Id = doId;

				// Map DO type to DC type for Stock domain and set Mvt value
				switch (doType)
				{
					case DocumentTypeDO.MouvementEntree:
						documentType.DC_Id = (short)DocumentTypeDC.MouvementEntree;
						documentType.Mvt = (short)DocumentTypeMVT.MouvementEntree;
						break;
					case DocumentTypeDO.MouvementSortie:
						documentType.DC_Id = (short)DocumentTypeDC.MouvementSortie;
						documentType.Mvt = (short)DocumentTypeMVT.MouvementSortie;
						break;
					case DocumentTypeDO.Declassement:
						documentType.DC_Id = (short)DocumentTypeDC.Declassement;
						documentType.Mvt = (short)DocumentTypeMVT.Declassement;
						break;
					case DocumentTypeDO.Transfert:
						documentType.DC_Id = (short)DocumentTypeDC.Transfert;
						documentType.Mvt = (short)DocumentTypeMVT.Transfert;
						break;
					case DocumentTypeDO.PreparationFabrication:
						documentType.DC_Id = (short)DocumentTypeDC.PreparationFabrication;
						documentType.Mvt = (short)DocumentTypeMVT.PreparationFabrication;
						break;
					case DocumentTypeDO.OrdreFabrication:
						documentType.DC_Id = (short)DocumentTypeDC.OrdreFabrication;
						documentType.Mvt = (short)DocumentTypeMVT.OrdreFabrication;
						break;
					case DocumentTypeDO.BonFabrication:
						documentType.DC_Id = (short)DocumentTypeDC.BonFabrication;
						documentType.Mvt = (short)DocumentTypeMVT.BonFabrication;
						break;
				}
			}
			else if ((int)doType >= 40 && (int)doType <= 49)
			{
				// Autre domain
				documentType.Domaine = (short)DocumentDomaine.Autre;
				documentType.DO_Id = (short)doType;

				// Adjust DO_Id to match the offset in F_DOCENTETE
				short doId = (short)((int)doType - 40);
				documentType.DO_Id = doId;

				// Map DO type to DC type for Autre domain and set Mvt value
				switch (doType)
				{
					case DocumentTypeDO.DocumentInterne1:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne1;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne1;
						break;
					case DocumentTypeDO.DocumentInterne2:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne2;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne2;
						break;
					case DocumentTypeDO.DocumentInterne3:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne3;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne3;
						break;
					case DocumentTypeDO.DocumentInterne4:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne4;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne4;
						break;
					case DocumentTypeDO.DocumentInterne5:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne5;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne5;
						break;
					case DocumentTypeDO.DocumentInterne6:
						documentType.DC_Id = (short)DocumentTypeDC.DocumentInterne6;
						documentType.Mvt = (short)DocumentTypeMVT.DocumentInterne6;
						break;
				}
			}
			return documentType;
		}
	}
}
