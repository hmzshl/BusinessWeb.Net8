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
using DocumentFormat.OpenXml.Office2010.Excel;


namespace BusinessWeb.Controllers.SAGE_Tables
{
    [Route("api/{Societe}/[controller]")]
    [ApiController]

    public class API_V_CREGLEMENTController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_V_CREGLEMENTController(BusinessWebDBContext sdb)
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
        // GET: api/API_V_CREGLEMENT
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_V_CREGLEMENT>>> GetAPI_V_CREGLEMENT()
        {
            setDB(); return await _db.API_V_CREGLEMENT.ToListAsync();
        }

		[HttpGet("JO_Num/{JO_Num}")]
		public async Task<ActionResult<IEnumerable<API_V_CREGLEMENT>>> GetAPI_V_CREGLEMENTByJO_Num(string JO_Num)
		{
			setDB(); return await _db.API_V_CREGLEMENT.Where(a => a.JO_Num == JO_Num).ToListAsync();
		}
        [HttpGet("RG_Type/{RG_Type}")]
        public async Task<ActionResult<IEnumerable<API_V_CREGLEMENT>>> GetAPI_V_CREGLEMENTByRG_Type(int RG_Type)
        {
            setDB(); return await _db.API_V_CREGLEMENT.Where(a => a.RG_Type == RG_Type).ToListAsync();
        }
        [HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<API_V_CREGLEMENT>>> GetAPI_V_CREGLEMENTByCT_Num(string CT_Num)
		{
			setDB(); return await _db.API_V_CREGLEMENT.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("RG_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<API_V_CREGLEMENT>>> GetAPI_V_CREGLEMENTByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.API_V_CREGLEMENT.Where(a => a.RG_Date >= DateDebut && a.RG_Date <= DateFin).ToListAsync();
		}
		[HttpGet("RG_No/{RG_No}")]
		public async Task<ActionResult<API_V_CREGLEMENT>> GetAPI_V_CREGLEMENTByRG_No(int RG_No)
		{
			setDB(); var item = _db.API_V_CREGLEMENT.Where(a => a.RG_No == RG_No).SingleOrDefault();

			if (item == null)
			{
				return new API_V_CREGLEMENT();
			}

			return item;
		}


	}
}