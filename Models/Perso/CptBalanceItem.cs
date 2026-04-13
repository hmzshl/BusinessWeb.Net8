namespace BusinessWeb.Models.Perso;

public class CptBalanceItem
{
    public string CG_Num { get; set; }
    public string CG_Intitule { get; set; }
    public string ClasseLabel => string.IsNullOrEmpty(CG_Num) ? "?" : "Classe " + CG_Num[0];
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal Solde => TotalDebit - TotalCredit;
    public string SoldeSens => Solde >= 0 ? "Débiteur" : "Créditeur";
}
