using BusinessWeb.Models.DB;

namespace BusinessWeb.Models.Perso
{
	public class V_StockGlobal : API_V_ARTICLEMVT
	{
		public decimal Initial { get; set; }
		public decimal Achat { get; set; }
		public decimal Vente { get; set; }
		public decimal Entree { get; set; }
		public decimal Sortie { get; set; }
		public decimal Transfert { get; set; }
		public decimal Interne { get; set; }
		public decimal Theorique { get; set; }
		public string StockStatus
		{
			get
			{
				if (Theorique > 0)
					return "En stock";
				else if (Theorique < 0)
					return "Stock négatif";
				else
					return "Stock epuisé";
			}
		}
	}
}
