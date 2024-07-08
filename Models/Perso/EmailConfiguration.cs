namespace BusinessWeb.Models.Perso
{
	public class EmailConfiguration
	{
		public bool Visible { get; set; } = false;
		public string SmtpServer { get; set; }
		public string Port { get; set; }
		public string SenderEmail { get; set; }
		public string Password { get; set; }
		public string RecipientEmail { get; set; }
		public string Objet { get; set; }
		public string FileName { get; set; }
	}
}
