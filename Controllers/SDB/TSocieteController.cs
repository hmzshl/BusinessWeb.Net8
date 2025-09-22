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

    public class TSocieteController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext(); Helpers fn = new Helpers();

		public TSocieteController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSociete>>> GetTSocietes()
        {
			List<TSociete> dt = await _sdb.TSocietes.ToListAsync();

			try
			{
				var rs = new List<TSociete>();
				foreach (var item in dt)
				{
					var lt = new TSociete();
					fn.CopyData(item, lt);
                    lt.Serveur = null;
                    lt.Passe = null;
                    lt.Base1 = null;
                    lt.Web = null;
                    lt.id = item.id;
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			//return await _sdb.TSocietes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TSociete>> GetTSocietes(int id)
        {
            var item = await _sdb.TSocietes.FindAsync(id);
			item.Serveur = null;
			item.Passe = null;
			item.Base1 = null;
			item.Web = null;

			if (item == null)
            {
                return new TSociete();
            }

            return item;
        }

        
        private bool TSocietesExists(int id)
        {
            return _sdb.TSocietes.Any(e => e.id == id);
        }
    }
}