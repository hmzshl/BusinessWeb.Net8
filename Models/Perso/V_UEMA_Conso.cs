namespace BusinessWeb.Models.Perso
{
	public class V_UEMA_Conso
	{
		public string Numero { get; set; }
		public string Affectation { get; set; }
		public string AR_Ref { get; set; }
		public decimal? Qte { get; set; }  // Changed to nullable
		public decimal? PC { get; set; }   // Changed to nullable
		public decimal? BC { get; set; }   // Changed to nullable
		public decimal? BL { get; set; }   // Changed to nullable
		public decimal? FA { get; set; }   // Changed to nullable
		public decimal? SO { get; set; }   // Changed to nullable
	}
}
