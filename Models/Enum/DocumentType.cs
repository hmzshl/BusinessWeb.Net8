// DocumentTypeEnum.cs
namespace BusinessWeb.Models.Enum
{
	/// <summary>
	/// Document domaine types
	/// </summary>


	/// <summary>
	/// Document types for F_DOCCURRENTPIECE (DC_IdCol)
	/// Values 0-8 are reused for each domain
	/// </summary>
	public enum DocumentTypeDC
	{
		// Vente (Domaine = 0)
		DevisClient = 0,          // DE
		BonCommandeClient = 1,    // BC
		PreparationLivraisonClient = 2,       // PL
		BonLivraisonClient = 3,   // BL
		BonRetourClient = 4,      // BR
		BonAvoirClient = 5,       // BA
		FactureClient = 6,        // FA
		FactureRetourClient = 7, // FR
		FactureAvoirClient = 8,   // FV

		// Achat (Domaine = 1)
		DemandeAchatFournisseur = 0,   // DA
		PreparationCommandeFournisseur = 1, // PC
		BonCommandeFournisseur = 2,      // FBC
		BonLivraisonFournisseur = 3,      // FBL
		BonRetourFournisseur = 4,      // FBR
		BonAvoirFournisseur = 5,      // FBA
		FactureFournisseur = 6,      // FFA
		FactureRetourFournisseur = 7, // FRF
		FactureAvoirFournisseur = 8, // FRV

		// Stock (Domaine = 2)
		MouvementEntree = 0, // ME
		MouvementSortie = 1, // MS
		Declassement = 2,    // DS
		Transfert = 3,       // MT
		PreparationFabrication = 4,     // PF
		OrdreFabrication = 5, // OF
		BonFabrication = 6,  // BF

		// Autre (Domaine = 4)
		DocumentInterne1 = 0,          // CA
		DocumentInterne2 = 1,          // BI
		DocumentInterne3 = 2,         // RC
		DocumentInterne4 = 3, // SP
		DocumentInterne5 = 4, // RP
		DocumentInterne6 = 6  // SR
	}
	public enum DocumentTypeMVT
	{
		// Vente (Domaine = 0)
		DevisClient = 0,          // DE
		BonCommandeClient = 0,    // BC
		PreparationLivraisonClient = 0,       // PL
		BonLivraisonClient = 3,   // BL
		BonRetourClient = 1,      // BR
		BonAvoirClient = 0,       // BA
		FactureClient = 3,        // FA
		FactureRetourClient = 1, // FR
		FactureAvoirClient = 0,   // FV

		// Achat (Domaine = 1)
		DemandeAchatFournisseur = 0,   // DA
		PreparationCommandeFournisseur = 0, // PC
		BonCommandeFournisseur = 0,      // FBC
		BonLivraisonFournisseur = 1,      // FBL
		BonRetourFournisseur = 3,      // FBR
		BonAvoirFournisseur = 0,      // FBA
		FactureFournisseur = 1,      // FFA
		FactureRetourFournisseur = 3, // FRF
		FactureAvoirFournisseur = 0, // FRV

		// Stock (Domaine = 2)
		MouvementEntree = 1, // ME
		MouvementSortie = 3, // MS
		Declassement = 0,    // DS
		Transfert = 0,       // MT
		PreparationFabrication = 0,     // PF
		OrdreFabrication = 0, // OF
		BonFabrication = 0,  // BF

		// Autre (Domaine = 4)
		DocumentInterne1 = 0,          // CA
		DocumentInterne2 = 0,          // BI
		DocumentInterne3 = 0,         // RC
		DocumentInterne4 = 0, // SP
		DocumentInterne5 = 0, // RP
		DocumentInterne6 = 0  // SR
	}

	/// <summary>
	/// Document types for F_DOCENTETE (DO_Type) - Unique across all domains
	/// </summary>
	public enum DocumentTypeDO
	{
		// === VENTE (Domaine = 0) ===
		DevisClient = 0,
		BonCommandeClient = 1,
		PreparationLivraisonClient = 2,
		BonLivraisonClient = 3,
		BonRetourClient = 4,
		BonAvoirClient = 5,
		FactureVente = 6,  // Uses DO_Provenance: 0=Normal, 1=Retour, 2=Avoir

		// === ACHAT (Domaine = 1) ===
		DemandeAchatFournisseur = 10,
		PreparationCommandeFournisseur = 11,
		BonCommandeFournisseur = 12,
		BonLivraisonFournisseur = 13,
		BonRetourFournisseur = 14,
		BonAvoirFournisseur = 15,
		FactureAchat = 16,  // Uses DO_Provenance: 0=Normal, 1=Retour, 2=Avoir

		// === STOCK (Domaine = 2) ===
		MouvementEntree = 20,
		MouvementSortie = 21,
		Declassement = 22,
		Transfert = 23,
		PreparationFabrication = 24,
		OrdreFabrication = 25,
		BonFabrication = 26,

		// === AUTRE (Domaine = 4) ===
		DocumentInterne1 = 40,
		DocumentInterne2 = 41,
		DocumentInterne3 = 42,
		DocumentInterne4 = 43,
		DocumentInterne5 = 44,
		DocumentInterne6 = 45
	}

	/// <summary>
	/// Provenance for invoices (DO_Provenance in F_DOCENTETE)
	/// </summary>
	public enum DocumentProvenance
	{
		Normal = 0,      // Facture normale
		Retour = 1,      // Facture de retour
		Avoir = 2        // Facture d'avoir
	}
	public enum DocumentDomaine
	{
		Vente = 0,
		Achat = 1,
		Stock = 2,
		Autre = 4
	}
	public class DocumentType 
	{
		public short? Domaine { get; set; }
		public short? DC_Id { get; set; }
		public short? DO_Id { get; set; }
		public short? Provenance { get; set; }
		public short? Mvt { get; set; }
	}
	/// <summary>
	/// Combined key for DocumentTypeDC (requires domaine + dcId)
	/// </summary>
	public struct DocumentTypeDCKey : IEquatable<DocumentTypeDCKey>
	{
		public short Domaine { get; }
		public short DC_Id { get; }

		public DocumentTypeDCKey(short domaine, short dcId)
		{
			Domaine = domaine;
			DC_Id = dcId;
		}

		public DocumentTypeDCKey(DocumentDomaine domaine, DocumentTypeDC dcType)
		{
			Domaine = (short)domaine;
			DC_Id = (short)dcType;
		}

		public bool Equals(DocumentTypeDCKey other)
		{
			return Domaine == other.Domaine && DC_Id == other.DC_Id;
		}

		public override bool Equals(object obj)
		{
			return obj is DocumentTypeDCKey other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Domaine, DC_Id);
		}

		public static bool operator ==(DocumentTypeDCKey left, DocumentTypeDCKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DocumentTypeDCKey left, DocumentTypeDCKey right)
		{
			return !left.Equals(right);
		}
	}
}