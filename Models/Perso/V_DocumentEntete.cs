using BusinessWeb.Models.DB;

namespace BusinessWeb.Models.Perso
{
    public class V_DocumentEntete
    {
        public short? DO_Domaine { get; set; }
        public short? DO_Type { get; set; }
        public string DO_Piece { get; set; }
        public DateTime? DO_Date { get; set; }
        public string DO_Ref { get; set; }
        public string DO_Tiers { get; set; }
        public int? CO_No { get; set; }
        public short? DO_Devise { get; set; }
        public decimal? DO_Cours { get; set; }
        public int? DE_No { get; set; }
        public decimal? DO_TxEscompte { get; set; }
        public short? DO_Imprim { get; set; }
        public string CA_Num { get; set; }
        public string DO_Coord01 { get; set; }
        public string DO_Coord02 { get; set; }
        public string DO_Coord03 { get; set; }
        public string DO_Coord04 { get; set; }
        public short? DO_Souche { get; set; }
        public DateTime? DO_DateLivr { get; set; }
        public short? DO_Tarif { get; set; }
        public short? N_CatCompta { get; set; }
        public string CG_Num { get; set; }
        public short? DO_Statut { get; set; }
        public DateTime? DO_DateLivrRealisee { get; set; }
        public DateTime? DO_DateExpedition { get; set; }
    }
}
