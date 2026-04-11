
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
		private DB _db = new DB();
		private readonly BusinessWebDBContext _sdb;
		private readonly Helpers _fn;

		public SaveDocumentController(BusinessWebDBContext sdb, Helpers fn)
		{
			_sdb = sdb;
			_fn = fn;
		}

		private void setDB()
		{
			int societe = Int16.Parse(RouteData.Values["Societe"] as string);
			var ste = _fn.GetCachedSociete(societe, _sdb);
			if (ste != null)
				_db = _fn.getDb(ste);
		}

		[HttpPost("Add")]
		public async Task<IActionResult> Add([FromBody] DocumentEntete data)
		{
			try
			{
				var result = await AddDocumentAsync(data);
				return Ok(new { result, Statut = "Success" });
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					result = new { Erreur = ex.Message, DO_Piece = (string)null },
					Statut = "Erreur"
				});
			}
		}

		private async Task<object> AddDocumentAsync(DocumentEntete data)
		{
			setDB();
			var sage = new SageService(_db);
			var result = await sage.Document.CreateDocument(data);
			return new
			{
				Erreur = (string)null,
				DO_Piece = result?.DocumentNumber,
				Statut = "Ok"
			};
		}
	}
}