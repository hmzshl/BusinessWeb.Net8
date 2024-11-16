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
 

    public partial class F_FAMCOMPTAController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_FAMCOMPTAController(BusinessWebDBContext sdb)
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
        // GET: api/F_FAMCOMPTA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_FAMCOMPTA>>> GetF_FAMCOMPTA()
        {
            setDB(); return await _db.F_FAMCOMPTA.ToListAsync();
        }

        // GET: api/F_FAMCOMPTA/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_FAMCOMPTA>> GetF_FAMCOMPTA(int id)
        {
            setDB(); var item = await _db.F_FAMCOMPTA.FindAsync(id);

            if (item == null)
            {
                return new F_FAMCOMPTA();
            }

            return item;
        }

		// PUT: api/F_FAMCOMPTA/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_FAMCOMPTA(int id, F_FAMCOMPTA item)
        {
            if (id != item.cbMarq)
            {
                return BadRequest();
            }

            setDB(); _db.Entry(item).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!F_FAMCOMPTAExists(id))
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

        // POST: api/F_FAMCOMPTA
        [HttpPost]
        public async Task<ActionResult<F_FAMCOMPTA>> PostF_FAMCOMPTA(F_FAMCOMPTA item)
        {
            setDB(); _db.F_FAMCOMPTA.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_FAMCOMPTA", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_FAMCOMPTA/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_FAMCOMPTA(int id)
        {
            setDB(); var item = await _db.F_FAMCOMPTA.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_FAMCOMPTA.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_FAMCOMPTAExists(int id)
        {
            setDB(); return _db.F_FAMCOMPTA.Any(e => e.cbMarq == id);
        }
    }
}