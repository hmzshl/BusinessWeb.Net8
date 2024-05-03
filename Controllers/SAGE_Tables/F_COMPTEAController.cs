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
 

    public partial class F_COMPTEAController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_COMPTEAController(BusinessWebDBContext sdb)
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
        // GET: api/F_COMPTEA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_COMPTEA>>> GetF_COMPTEA()
        {
            setDB(); return await _db.F_COMPTEA.ToListAsync();
        }

        // GET: api/F_COMPTEA/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_COMPTEA>> GetF_COMPTEA(int id)
        {
            setDB(); var item = await _db.F_COMPTEA.FindAsync(id);

            if (item == null)
            {
                return new F_COMPTEA();
            }

            return item;
        }
		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<F_COMPTEA>> GetF_COMPTEAByCA_Num(string CA_Num)
		{
			setDB(); var item = _db.F_COMPTEA.Where(a => a.CA_Num == CA_Num).SingleOrDefault();

			if (item == null)
			{
				return new F_COMPTEA();
			}

			return item;
		}

		// PUT: api/F_COMPTEA/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_COMPTEA(int id, F_COMPTEA item)
        {
            if (id != item.cbMarq)
            {
                return BadRequest();
            }

            setDB(); _db.Entry(item).State = EntityState.Modified;

            try
            {
                setDB(); await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!F_COMPTEAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/F_COMPTEA
        [HttpPost]
        public async Task<ActionResult<F_COMPTEA>> PostF_COMPTEA(F_COMPTEA item)
        {
            setDB(); _db.F_COMPTEA.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_COMPTEA", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_COMPTEA/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_COMPTEA(int id)
        {
            setDB(); var item = await _db.F_COMPTEA.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_COMPTEA.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_COMPTEAExists(int id)
        {
            setDB(); return _db.F_COMPTEA.Any(e => e.cbMarq == id);
        }
    }
}