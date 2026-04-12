namespace BusinessWeb.Models.Perso;

public class ClientInactifItem
{
    public string CT_Num { get; set; }
    public string CT_Intitule { get; set; }
    public DateTime? DernierAchat { get; set; }
    public int DernierAchatAnnee => DernierAchat?.Year ?? 0;
    public int JoursSansAchat { get; set; }
    public decimal CAHistorique { get; set; }
}
