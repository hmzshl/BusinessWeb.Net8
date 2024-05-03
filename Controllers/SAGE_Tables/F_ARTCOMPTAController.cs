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
 

    public partial class F_ARTCOMPTAController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ARTCOMPTAController(BusinessWebDBContext sdb)
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
        // GET: api/F_ARTCOMPTA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ARTCOMPTA>>> GetF_ARTCOMPTA()
        {
            setDB(); return await _db.F_ARTCOMPTA.ToListAsync();
        }

        // GET: api/F_ARTCOMPTA/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_ARTCOMPTA>> GetF_ARTCOMPTA(int id)
        {
            setDB(); var item = await _db.F_ARTCOMPTA.FindAsync(id);

            if (item == null)
            {
                return new F_ARTCOMPTA();
            }

            return item;
        }

		// PUT: api/F_ARTCOMPTA/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_ARTCOMPTA(int id, F_ARTCOMPTA item)
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
                if (!F_ARTCOMPTAExists(id))
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

        // POST: api/F_ARTCOMPTA
        [HttpPost]
        public async Task<ActionResult<F_ARTCOMPTA>> PostF_ARTCOMPTA(F_ARTCOMPTA item)
        {
            setDB(); _db.F_ARTCOMPTA.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ARTCOMPTA", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ARTCOMPTA/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ARTCOMPTA(int id)
        {
            setDB(); var item = await _db.F_ARTCOMPTA.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ARTCOMPTA.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ARTCOMPTAExists(int id)
        {
            setDB(); return _db.F_ARTCOMPTA.Any(e => e.cbMarq == id);
        }
    }
}