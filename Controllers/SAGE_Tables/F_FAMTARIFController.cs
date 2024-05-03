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
 

    public partial class F_FAMTARIFController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_FAMTARIFController(BusinessWebDBContext sdb)
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
        // GET: api/F_FAMTARIF
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_FAMTARIF>>> GetF_FAMTARIF()
        {
            setDB(); return await _db.F_FAMTARIF.ToListAsync();
        }
		[HttpGet("FA_CodeFamille/{FA_CodeFamille}")]
		public async Task<ActionResult<IEnumerable<F_FAMTARIF>>> GetF_FAMTARIFByFA_CodeFamille(string FA_CodeFamille)
		{
			setDB(); return await _db.F_FAMTARIF.Where(a => a.FA_CodeFamille == FA_CodeFamille).ToListAsync();
		}
		// GET: api/F_FAMTARIF/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_FAMTARIF>> GetF_FAMTARIF(int id)
        {
            setDB(); var item = await _db.F_FAMTARIF.FindAsync(id);

            if (item == null)
            {
                return new F_FAMTARIF();
            }

            return item;
        }

		// PUT: api/F_FAMTARIF/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_FAMTARIF(int id, F_FAMTARIF item)
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
                if (!F_FAMTARIFExists(id))
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

        // POST: api/F_FAMTARIF
        [HttpPost]
        public async Task<ActionResult<F_FAMTARIF>> PostF_FAMTARIF(F_FAMTARIF item)
        {
            setDB(); _db.F_FAMTARIF.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_FAMTARIF", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_FAMTARIF/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_FAMTARIF(int id)
        {
            setDB(); var item = await _db.F_FAMTARIF.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_FAMTARIF.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_FAMTARIFExists(int id)
        {
            setDB(); return _db.F_FAMTARIF.Any(e => e.cbMarq == id);
        }
    }
}