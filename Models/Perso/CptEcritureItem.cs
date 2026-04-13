namespace BusinessWeb.Models.Perso;

public class CptEcritureItem
{
    public string JO_Num { get; set; }
    public string JO_Intitule { get; set; }
    public DateTime? MV_Date { get; set; }
    public string EC_Piece { get; set; }
    public string CG_Num { get; set; }
    public string CG_Intitule { get; set; }
    public string CompteLabel => $"{CG_Num}  {CG_Intitule}";
    public string CT_Num { get; set; }
    public string CT_Intitule { get; set; }
    public string EC_Intitule { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public short? EC_Lettre { get; set; }
    public string EC_Lettrage { get; set; }
    public string LettreLabel => EC_Lettre switch { 0 => "Non lettré", 1 => "Partiel", 2 => "Lettré", _ => "" };
}
