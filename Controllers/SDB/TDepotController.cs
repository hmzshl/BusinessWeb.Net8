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


namespace BusinessWeb.Controllers.SDB
{
    [Route("api/[controller]")]
    [ApiController]

    public class TDepotController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public TDepotController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TDepot>>> GetTDepots()
        {
            return await _sdb.TDepots.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TDepot>> GetTDepots(int id)
        {
            var item = await _sdb.TDepots.FindAsync(id);

            if (item == null)
            {
                return new TDepot();
            }

            return item;
        }
        
        private bool TDepotsExists(int id)
        {
            return _sdb.TDepots.Any(e => e.id == id);
        }
		[HttpGet("User/{Id}")]
		public async Task<ActionResult<IEnumerable<TDepot>>> GetTDepotByUser(string Id)
		{
			return await _sdb.TDepots.Where(a => a.UserName == Id).ToListAsync();
		}
	}
}