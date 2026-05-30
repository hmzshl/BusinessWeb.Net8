namespace BusinessWeb.Models.Perso;

public class CptBalanceItem
{
    public string CG_Num { get; set; }
    public string CG_Intitule { get; set; }
    public string ClasseLabel => string.IsNullOrEmpty(CG_Num) ? "?" : "Classe " + CG_Num[0];

    public decimal AnDebit { get; set; }
    public decimal AnCredit { get; set; }
    public decimal MvDebit { get; set; }
    public decimal MvCredit { get; set; }
    public decimal SoldeDebit { get; set; }
    public decimal SoldeCredit { get; set; }

    public decimal TotalDebit => AnDebit + MvDebit;
    public decimal TotalCredit => AnCredit + MvCredit;
    public decimal Solde => SoldeDebit - SoldeCredit;
    public string SoldeSens => Solde >= 0 ? "Débiteur" : "Créditeur";

    public void ComputeSolde()
    {
        var net = (AnDebit + MvDebit) - (AnCredit + MvCredit);
        SoldeDebit = net > 0 ? net : 0m;
        SoldeCredit = net < 0 ? -net : 0m;
    }
}
