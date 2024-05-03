namespace BusinessWeb.Models.LT
{
	public partial class API_LT_COMPTET
	{
		public int cbMarq { get; set; }
		public string CT_Num { get; set; }
		public string CT_Intitule { get; set; }
		public short CT_Type { get; set; }
		public string CG_NumPrinc { get; set; }
		public string CT_Qualite { get; set; }
		public string CT_Contact { get; set; }
		public string CT_Adresse { get; set; }
		public string CT_Complement { get; set; }
		public string CT_CodePostal { get; set; }
		public string CT_Ville { get; set; }
		public string CT_Pays { get; set; }
		public string CT_Identifiant { get; set; }
		public string CT_Siret { get; set; }
		public string CT_Commentaire { get; set; }
		public decimal? CT_Encours { get; set; }
		public short? CT_Sommeil { get; set; }
		public string CT_Telephone { get; set; }
		public string CT_EMail { get; set; }
		public string CT_Site { get; set; }
		public DateTime? DL_DateBL { get; set; }
		public decimal? DL_MontantHT { get; set; }
		public decimal? DL_MontantTTC { get; set; }
		public decimal? RG_Montant { get; set; }
		public decimal? SoldeCommercial { get; set; }
		public decimal? SoldeComptable { get; set; }
		public string Controle { get; set; }
		public string SommeilIntitule { get; set; }
		public string EtatSolde { get; set; }
	}
}
