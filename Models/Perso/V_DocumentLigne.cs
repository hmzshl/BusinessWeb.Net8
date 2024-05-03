using BusinessWeb.Models.DB;

namespace BusinessWeb.Models.Perso
{
    public class V_DocumentLigne
    {
        public short? DO_Domaine { get; set; }
        public short DO_Type { get; set; }
        public string CT_Num { get; set; }
        public string DO_Piece { get; set; }
        public DateTime? DO_Date { get; set; }
        public int? DL_Ligne { get; set; }
        public string DO_Ref { get; set; }
        public string AR_Ref { get; set; }
        public string DL_Design { get; set; }
        public decimal? DL_Qte { get; set; }
        public decimal? DL_Remise01REM_Valeur { get; set; }
        public decimal? DL_PrixUnitaire { get; set; }
        public int? CO_No { get; set; }
        public int? DE_No { get; set; }
        public decimal? DL_PUDevise { get; set; }
        public string CA_Num { get; set; }
        public string DL_CodeTaxe1 { get; set; }
    }
}
