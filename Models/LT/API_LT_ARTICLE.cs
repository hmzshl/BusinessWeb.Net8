namespace BusinessWeb.Models.LT
{
	public partial class API_LT_ARTICLE
	{
		public string AR_Ref { get; set; }
		public string AR_Design { get; set; }
		public string FA_CodeFamille { get; set; }
		public decimal? AR_PrixAch { get; set; }
		public decimal? AR_PrixVen { get; set; }
		public short? AR_Sommeil { get; set; }
		public string AR_CodeBarre { get; set; }
		public string AR_CodeFiscal { get; set; }
		public string AR_Pays { get; set; }
		public decimal? AR_PUNet { get; set; }
		public decimal? AR_CoutStd { get; set; }
		public int cbMarq { get; set; }
		public string FA_Intitule { get; set; }
		public string U_Intitule { get; set; }
		public string SuiviStockIntitule { get; set; }
		public string SommeilIntitule { get; set; }
		public decimal? AS_QteSto { get; set; }
		public decimal? AS_MontSto { get; set; }
		public string EtatStock { get; set; }
	}
}
