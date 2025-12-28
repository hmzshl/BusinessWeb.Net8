
using BusinessWeb.Data;
using BusinessWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BusinessWeb.Models.PersoAPI.Document;
namespace BusinessWeb.Controllers.SAGE_Save
{
	[Route("api/{Societe}/[controller]")]
	[ApiController]
	public partial class SaveDocumentController : ControllerBase
	{
		public DB _db = new DB();
		private BusinessWebDBContext _sdb = new BusinessWebDBContext();
		private Helpers fn = new Helpers();

		public SaveDocumentController(BusinessWebDBContext sdb)
		{
			_sdb = sdb;
		}

		private void setDB()
		{
			int Societe = Int16.Parse((RouteData.Values["Societe"] as string));
			var ste = _sdb.TSocietes.Where(a => a.id == Societe).SingleOrDefault();
			if (ste != null)
			{
				this._db = fn.getDb(ste);
			}
		}

		[HttpPost("Add")]
		public IActionResult Add([FromBody] DocumentEntete data)
		{
			try
			{
				var result = AddDocument(data);
				return Ok(new
				{
					result = result,
					Statut = "Success"
				});
			}
			catch (Exception ex)
			{
				// Return a consistent error response
				return Ok(new
				{
					result = new
					{
						Erreur = ex.Message,
						DO_Piece = (string)null
					},
					Statut = "Erreur"
				});
			}
		}

		private object AddDocument(DocumentEntete data)
		{
			try
			{
				setDB();
				SageService sage = new SageService(_db);
				var result = sage.Document.CreateDocument(data).GetAwaiter().GetResult();

				// Return the result wrapped in a consistent format
				return new
				{
					Erreur = (string)null,
					DO_Piece = result?.DocumentNumber, // Adjust this based on what CreateDocument returns
					Statut = "Ok"
				};
			}
			catch (Exception ex)
			{
				// Re-throw the exception so it can be caught by the controller
				throw new Exception($"Error creating document: {ex.Message}", ex);
			}
		}
	}
}