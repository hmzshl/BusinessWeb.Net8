namespace BusinessWeb.Models.Perso;

public class CptTierBalanceItem
{
    public string CT_Num { get; set; }
    public string CT_Intitule { get; set; }
    public string CG_NumPrinc { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal Solde => TotalDebit - TotalCredit;
    public string SoldeSens => Solde >= 0 ? "Débiteur" : "Créditeur";
}
