using BusinessWeb.Models.DB;
namespace BusinessWeb.Models.Perso
{
    public partial class RAPPRO_ARTICLE : F_ARTICLE
    {
        public string AR_Ref2 { get; set; }
        public string FA_CodeFamille2 { get; set; }
        public string AR_Design2 { get; set; }
        public short? AR_UniteVen2 { get; set; }
        public short? AR_SuiviStock2 { get; set; }
        public Decimal? AR_PrixAch2 { get; set; }
        public Decimal? AR_PrixVen2 { get; set; }
        public string Statut { get; set; }
    }
}

