namespace BusinessWeb.Models.Perso;

public class CollabPortefeuilleItem
{
    public string CO_Nom { get; set; }
    public int NbClients { get; set; }
    public decimal CA { get; set; }
    public decimal Marge { get; set; }
    public decimal MargeP { get; set; }
    public decimal CAMoyenParClient => NbClients > 0 ? CA / NbClients : 0;
}
