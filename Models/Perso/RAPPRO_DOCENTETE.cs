using BusinessWeb.Models.DB;
namespace BusinessWeb.Models.Perso
{
    public partial class RAPPRO_DOCENTETE : F_DOCENTETE
    {
        public string DO_Tiers2 { get; set; }
        public short? DO_Type2 { get; set; }
        public string DO_Piece2 { get; set; }
        public string DO_Ref2 { get; set; }
        public int? DE_No2 { get; set; }
        public DateTime?  DO_Date2 { get; set; }
        public Decimal? DO_TotalHT2 { get; set; }
        public int Statut { get; set; }
    }
}

