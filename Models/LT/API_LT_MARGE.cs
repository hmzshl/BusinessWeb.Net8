namespace BusinessWeb.Models.LT
{
	public partial class API_LT_MARGE
	{
		public string Annee { get; set; }
		public string MoisAnnee { get; set; }
		public string DO_Piece { get; set; }
		public DateTime? DO_Date { get; set; }
		public string CT_Num { get; set; }
		public string CT_Intitule { get; set; }
		public string AR_Ref { get; set; }
		public string AR_Design { get; set; }
		public decimal? DL_Qte { get; set; }
		public decimal? PU { get; set; }
		public decimal? PUNet { get; set; }
		public decimal? DL_MontantHT { get; set; }
		public decimal? DL_MontantTVA { get; set; }
		public decimal? DL_MontantTTC { get; set; }
		public decimal? CMUPMarge { get; set; }
		public string CO_Nom { get; set; }
		public string FA_CodeFamille { get; set; }
		public string FA_Intitule { get; set; }
		public int CO_No { get; set; }
		public string DE_Intitule { get; set; }
		public string CA_Num { get; set; }
		public string CA_Intitule { get; set; }
		public string CT_Ville { get; set; }
		public string CT_CodeRegion { get; set; }
	}
}
