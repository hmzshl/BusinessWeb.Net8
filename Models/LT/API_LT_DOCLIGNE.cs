namespace BusinessWeb.Models.LT
{
	public partial class API_LT_DOCLIGNE
	{
		public int cbMarq { get; set; }
		public short? DO_Domaine { get; set; }
		public short DO_Type { get; set; }
		public string CT_Num { get; set; }
		public string DO_Piece { get; set; }
		public DateTime? DO_Date { get; set; }
		public string DO_Ref { get; set; }
		public string AR_Ref { get; set; }
		public string DL_Design { get; set; }
		public decimal? DL_Qte { get; set; }
		public decimal? DL_PrixUnitaire { get; set; }
		public decimal? DL_PUBC { get; set; }
		public decimal? DL_Taxe1 { get; set; }
		public int? CO_No { get; set; }
		public string EU_Enumere { get; set; }
		public int? DE_No { get; set; }
		public string CA_Num { get; set; }
		public short? DL_Valorise { get; set; }
		public decimal? DL_MontantHT { get; set; }
		public decimal? DL_MontantTTC { get; set; }

		public string DL_CodeTaxe1 { get; set; }
		public string CT_Intitule { get; set; }
		public string DE_Intitule { get; set; }
		public string DomaineIntitule { get; set; }
		public string TypeIntitule { get; set; }
		public string CA_Intitule { get; set; }
		public string CO_Nom { get; set; }
		public decimal? PUTTC { get; set; }
		public decimal? MontantTVA { get; set; }
		public decimal? Remise { get; set; }
		public decimal? PUNet { get; set; }
	}
}
