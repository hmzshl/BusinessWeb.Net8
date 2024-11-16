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
 

    public partial class P_DEVISEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public P_DEVISEController(BusinessWebDBContext sdb)
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
        // GET: api/P_DEVISE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<P_DEVISE>>> GetP_DEVISE()
        {
            setDB(); return await _db.P_DEVISE.ToListAsync();
        }

        // GET: api/P_DEVISE/5
        [HttpGet("{id}")]
        public async Task<ActionResult<P_DEVISE>> GetP_DEVISE(int id)
        {
            setDB(); var item = await _db.P_DEVISE.FindAsync(id);

            if (item == null)
            {
                return new P_DEVISE();
            }

            return item;
        }

		// PUT: api/P_DEVISE/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutP_DEVISE(int id, P_DEVISE item)
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
                if (!P_DEVISEExists(id))
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

        // POST: api/P_DEVISE
        [HttpPost]
        public async Task<ActionResult<P_DEVISE>> PostP_DEVISE(P_DEVISE item)
        {
            setDB(); _db.P_DEVISE.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetP_DEVISE", new { id = item.cbMarq }, item);
        }

        // DELETE: api/P_DEVISE/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteP_DEVISE(int id)
        {
            setDB(); var item = await _db.P_DEVISE.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.P_DEVISE.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool P_DEVISEExists(int id)
        {
            setDB(); return _db.P_DEVISE.Any(e => e.cbMarq == id);
        }
    }
}