using BusinessWeb.Models.DB;
namespace BusinessWeb.Models.Perso
{
    public partial class RAPPRO_CREGLEMENT : F_CREGLEMENT
    {
        public string CT_NumPayeur2 { get; set; }
        public short? RG_Type2 { get; set; }
        public string RG_Piece2 { get; set; }
        public string RG_Reference2 { get; set; }
        public int? DE_No2 { get; set; }
        public DateTime?  RG_Date2 { get; set; }
		public DateTime? RG_DateEchCont2 { get; set; }
		public Decimal? RG_Montant2 { get; set; }
		public string RG_Libelle2 { get; set; }
		public string JO_Num2 { get; set; }
		public string Statut { get; set; }
	}
}

