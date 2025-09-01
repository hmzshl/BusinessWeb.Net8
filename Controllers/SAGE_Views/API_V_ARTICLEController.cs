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
using System.Text.Json;


namespace BusinessWeb.Controllers.SAGE_Tables
{
    [Route("api/{Societe}/[controller]")]
    [ApiController]

    public class API_V_ARTICLEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_V_ARTICLEController(BusinessWebDBContext sdb)
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
        // GET: api/API_V_ARTICLE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_V_ARTICLE>>> GetAPI_V_ARTICLE()
        {
            setDB(); return await _db.API_V_ARTICLE.ToListAsync();
        }

        // GET: api/API_V_ARTICLE/5
        [HttpGet("{id}")]
        public async Task<ActionResult<API_V_ARTICLE>> GetAPI_V_ARTICLE(int id)
        {
            setDB(); var item = _db.API_V_ARTICLE.Where(a => a.cbMarq == id).SingleOrDefault();

            if (item == null)
            {
                return new API_V_ARTICLE();
            }

            return item;
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<API_V_ARTICLE>> GetAPI_V_ARTICLEByAR_Ref(string AR_Ref)
		{
			setDB(); var item = _db.API_V_ARTICLE.Where(a => a.AR_Ref == AR_Ref).SingleOrDefault();

			if (item == null)
			{
				return new API_V_ARTICLE();
			}

			return item;
		}
        [HttpGet("AR_Refs/{AR_Refs}")]
        public async Task<ActionResult<List<API_V_ARTICLE>>> GetAPI_V_ARTICLEByAR_Refs(string AR_Refs)
        {
            var refs = JsonSerializer.Deserialize<List<string>>(AR_Refs);
            setDB();  
            return _db.API_V_ARTICLE.Where(a => (refs.Contains(a.AR_Ref))).ToList();
        }


    }
}