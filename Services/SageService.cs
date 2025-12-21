using BusinessWeb.Data;

namespace BusinessWeb.Services
{
	public class SageService
	{
		private readonly DB _context;

		// Individual services as private fields
		public SageJournauxService Journaux;
		public SageComptegService CompteG;
		public SageComptetService CompteT;
		public SageEcritureService Ecriture;
		public DocCurrentPieceService CurrentPiece;

		// Other services can be added as you create them
		// private readonly SageArticlesService _articlesService;
		// private readonly SageClientsService _clientsService;

		public SageService(DB context)
		{
			_context = context;
			Journaux = new SageJournauxService(context);
			CompteG = new SageComptegService(context);
			CompteT = new SageComptetService(context);
			Ecriture = new SageEcritureService(context);
			CurrentPiece = new DocCurrentPieceService(context);

		}
	}
}
