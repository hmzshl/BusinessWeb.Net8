namespace BusinessWeb.Models.LT
{
	public partial class API_LT_DOCENTETE
	{
		public int cbMarq { get; set; }
		public short? DO_Domaine { get; set; }
		public short? DO_Type { get; set; }
		public string DO_Piece { get; set; }
		public DateTime? DO_Date { get; set; }
		public string DO_Ref { get; set; }
		public string DO_Tiers { get; set; }
		public int? CO_No { get; set; }
		public int? DE_No { get; set; }
		public short? DO_Imprim { get; set; }
		public string CA_Num { get; set; }
		public string DO_Coord01 { get; set; }
		public string DO_Coord02 { get; set; }
		public string DO_Coord03 { get; set; }
		public string DO_Coord04 { get; set; }

		public short? DO_Statut { get; set; }
		public short? DO_Cloture { get; set; }
		public short? DO_Provenance { get; set; }
		public short? DO_Valide { get; set; }
		public decimal? DO_TotalHT { get; set; }
		public string CT_Intitule { get; set; }
		public string CA_Intitule { get; set; }
		public string CO_Nom { get; set; }
		public string DE_Intitule { get; set; }
		public string TF_Intitule { get; set; }
		public decimal DL_MontantHT { get; set; }
		public decimal DL_MontantTTC { get; set; }
		public decimal? DL_MontantTVA { get; set; }
		public decimal RC_Montant { get; set; }
		public decimal? Reste { get; set; }
		public string EtatReglement { get; set; }
		public string DomaineIntitule { get; set; }
		public string TypeIntitule { get; set; }
		public int? CO_No2 { get; set; }
	}
}
