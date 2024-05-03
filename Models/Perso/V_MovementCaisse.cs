using BusinessWeb.Models.DB;
namespace BusinessWeb.Models.Perso
 
{
    public class V_MovementCaisse : API_V_CAISSEENTETE
    {
        public Decimal Debit { get; set; }
        public Decimal Credit { get; set; }
        public Decimal Solde { get; set; }
    }
}
