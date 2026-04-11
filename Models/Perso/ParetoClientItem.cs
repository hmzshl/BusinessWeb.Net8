namespace BusinessWeb.Models.Perso;

public class ParetoClientItem
{
    public int Rang { get; set; }
    public string RangStr { get; set; }
    public string CT_Num { get; set; }
    public string CT_Intitule { get; set; }
    public decimal CA { get; set; }
    public decimal CAPercent { get; set; }
    public decimal CumulCA { get; set; }
    public decimal CumulPercent { get; set; }
    public bool IsIn80 => CumulPercent <= 80m;
}
