namespace BusinessWeb.Models
{
    public class AuthItems
    {
        public int? SelectedAPP { get; set; }
        public string UserID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
		public string TitleIcon { get; set; }
		public string Description { get; set; }
        public string SousGroupe { get; set; }
        public bool HasSousGroupe { get; set; }
    }
}
