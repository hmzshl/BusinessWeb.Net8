using BusinessWeb.Models.DB;

namespace BusinessWeb.Models.Perso;

public class ArticleMvtLigne
{
    public bool IsSoldeInitial { get; set; }
    public DateTime? DO_Date { get; set; }
    public string DO_Piece { get; set; }
    public DateTime? DL_DateBL { get; set; }
    public string DL_PieceBL { get; set; }
    public string DomaineIntitule { get; set; }
    public string TypeIntitule { get; set; }
    public string Sense { get; set; }
    public string DE_Intitule { get; set; }
    public string CT_Num { get; set; }
    public string TiersIntitule { get; set; }
    public decimal? Mvt { get; set; }
    public decimal? DL_Qte { get; set; }
    public decimal Qte { get; set; }
    public string U_Intitule { get; set; }
    public decimal? DL_MontantHT { get; set; }
    public decimal? DL_MontantTTC { get; set; }
    public decimal? PuHt { get; set; }
    public decimal? PuTtc { get; set; }
    public decimal? AR_PrixAch { get; set; }
    public short DO_Type { get; set; }

    private static decimal? PuFromMontant(decimal? montant, decimal? mvt) =>
        montant.HasValue && mvt.HasValue && mvt.Value != 0m ? montant.Value / mvt.Value : null;

    public static ArticleMvtLigne From(API_V_ARTICLEMVT m, decimal qte)
    {
        return new ArticleMvtLigne
        {
            DO_Date = m.DO_Date,
            DO_Piece = m.DO_Piece,
            DL_DateBL = m.DL_DateBL,
            DL_PieceBL = m.DL_PieceBL,
            DomaineIntitule = m.DomaineIntitule,
            TypeIntitule = m.TypeIntitule,
            Sense = m.Sense,
            DE_Intitule = m.DE_Intitule,
            CT_Num = m.CT_Num,
            TiersIntitule = m.TiersIntitule,
            Mvt = m.Mvt,
            DL_Qte = m.DL_Qte,
            Qte = qte,
            U_Intitule = m.U_Intitule,
            DL_MontantHT = m.DL_MontantHT,
            DL_MontantTTC = m.DL_MontantTTC,
            PuHt = PuFromMontant(m.DL_MontantHT, m.Mvt),
            PuTtc = PuFromMontant(m.DL_MontantTTC, m.Mvt),
            AR_PrixAch = m.AR_PrixAch,
            DO_Type = m.DO_Type
        };
    }
}
