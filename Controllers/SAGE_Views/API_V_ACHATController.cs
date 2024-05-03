using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Newtonsoft.Json.Linq;
using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.BusinessWebDB;


namespace BusinessWeb.Controllers.SAGE_Tables
{
    [Route("api/{Societe}/[controller]")]
    [ApiController]

    public class API_V_ACHATController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
		public API_V_ACHATController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
		
        private void setDB()
		{
			int Societe = Int16.Parse((RouteData.Values["Societe"] as string));
			var ste = _sdb.TSocietes.Where(a => a.id == Societe).SingleOrDefault();
			if(ste != null)
			{
				this._db = fn.getDb(ste);
			}
		}
        // GET: api/API_V_ACHAT
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHAT()
        {
            setDB(); return await _db.API_V_ACHAT.ToListAsync();
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByAR_Ref(string AR_Ref)
		{
			setDB(); return await _db.API_V_ACHAT.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
		}
        [HttpGet("CA_Num/{CA_Num}")]
        public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByCA_Num(string CA_Num)
        {
            setDB(); return await _db.API_V_ACHAT.Where(a => a.CA_Num == CA_Num).ToListAsync();
        }
        [HttpGet("DE_Intitule/{DE_Intitule}")]
        public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByDE_Intitule(string DE_Intitule)
        {
            setDB(); return await _db.API_V_ACHAT.Where(a => a.DE_Intitule == DE_Intitule).ToListAsync();
        }
        [HttpGet("MoisAnnee/{MoisAnnee}")]
		public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByMoisAnnee(string MoisAnnee)
		{
			setDB(); return await _db.API_V_ACHAT.Where(a => a.MoisAnnee == MoisAnnee).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByCT_Num(string CT_Num)
		{
			setDB(); return await _db.API_V_ACHAT.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("CO_No/{CO_No}")]
		public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByCO_No(int CO_No)
		{
			setDB(); return await _db.API_V_ACHAT.Where(a => a.CO_No == CO_No).ToListAsync();
		}
		[HttpGet("DO_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<API_V_ACHAT>>> GetAPI_V_ACHATByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.API_V_ACHAT.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();
		}




	}
}