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

    public class API_V_COLLABORATEURController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_V_COLLABORATEURController(BusinessWebDBContext sdb)
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
        // GET: api/API_V_COLLABORATEUR
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_V_COLLABORATEUR>>> GetAPI_V_COLLABORATEUR()
        {
            setDB(); return await _db.API_V_COLLABORATEUR.ToListAsync();
        }

        // GET: api/API_V_COLLABORATEUR/5
        [HttpGet("{id}")]
        public async Task<ActionResult<API_V_COLLABORATEUR>> GetAPI_V_COLLABORATEUR(int id)
        {
            setDB(); var item = _db.API_V_COLLABORATEUR.Where(a => a.cbMarq == id).SingleOrDefault();

            if (item == null)
            {
                return new API_V_COLLABORATEUR();
            }

            return item;
        }


    }
}