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
 

    public partial class P_CATTARIFController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public P_CATTARIFController(BusinessWebDBContext sdb)
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
        // GET: api/P_CATTARIF
        [HttpGet]
        public async Task<ActionResult<IEnumerable<P_CATTARIF>>> GetP_CATTARIF()
        {
            setDB(); return await _db.P_CATTARIF.ToListAsync();
        }

        // GET: api/P_CATTARIF/5
        [HttpGet("{id}")]
        public async Task<ActionResult<P_CATTARIF>> GetP_CATTARIF(int id)
        {
            setDB(); var item = await _db.P_CATTARIF.FindAsync(id);

            if (item == null)
            {
                return new P_CATTARIF();
            }

            return item;
        }

		// PUT: api/P_CATTARIF/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutP_CATTARIF(int id, P_CATTARIF item)
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
                if (!P_CATTARIFExists(id))
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

        // POST: api/P_CATTARIF
        [HttpPost]
        public async Task<ActionResult<P_CATTARIF>> PostP_CATTARIF(P_CATTARIF item)
        {
            setDB(); _db.P_CATTARIF.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetP_CATTARIF", new { id = item.cbMarq }, item);
        }

        // DELETE: api/P_CATTARIF/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteP_CATTARIF(int id)
        {
            setDB(); var item = await _db.P_CATTARIF.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.P_CATTARIF.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool P_CATTARIFExists(int id)
        {
            setDB(); return _db.P_CATTARIF.Any(e => e.cbMarq == id);
        }
    }
}