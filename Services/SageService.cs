using BusinessWeb.Data;

namespace BusinessWeb.Services
{
	public class SageService
	{
		private readonly DB _context;

		public SageJournauxService Journaux;
		public SageComptegService CompteG;
		public ISageComptetService CompteT;
		public SageEcritureService Ecriture;
		public DocumentService Document;
		public ISageArticleService Article;

		public SageService(DB context)
		{
			_context = context;
			Journaux = new SageJournauxService(context);
			CompteG = new SageComptegService(context);
			CompteT = new SageComptetService(context);
			Ecriture = new SageEcritureService(context);
			Document = new DocumentService(context);
			Article = new SageArticleService(context);
		}
	}
}
