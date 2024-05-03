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
using BusinessWeb.Models.LT;
using BusinessWeb.Models.BusinessWebDB;


namespace BusinessWeb.Controllers.SAGE_Tables
{
    [Route("api/{Societe}/[controller]")]
    [ApiController]

    public class API_LT_COMPTETController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
		public API_LT_COMPTETController(BusinessWebDBContext sdb)
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_LT_COMPTET>>> GetAPI_LT_COMPTET()
        {
			setDB();
			List<API_V_COMPTET> dt = await _db.API_V_COMPTET.ToListAsync();

			try
			{
				var rs = new List<API_LT_COMPTET>();
				foreach (var item in dt)
				{
					var lt = new API_LT_COMPTET();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

        }
		[HttpGet("CT_Type/{CT_Type}")]
		public async Task<ActionResult<IEnumerable<API_LT_COMPTET>>> GetAPI_LT_COMPTETByCT_Type(int CT_Type)
		{
			setDB();
			List<API_V_COMPTET> dt = await _db.API_V_COMPTET.Where(a => a.CT_Type == CT_Type).ToListAsync();

			try
			{
				var rs = new List<API_LT_COMPTET>();
				foreach (var item in dt)
				{
					var lt = new API_LT_COMPTET();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}

	}
}