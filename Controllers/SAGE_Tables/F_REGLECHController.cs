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
 

    public partial class F_REGLECHController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_REGLECHController(BusinessWebDBContext sdb)
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
        // GET: api/F_REGLECH
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_REGLECH>>> GetF_REGLECH()
        {
            setDB(); return await _db.F_REGLECH.ToListAsync();
        }

        // GET: api/F_REGLECH/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_REGLECH>> GetF_REGLECH(int id)
        {
            setDB(); var item = await _db.F_REGLECH.FindAsync(id);

            if (item == null)
            {
                return new F_REGLECH();
            }

            return item;
        }

		// PUT: api/F_REGLECH/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_REGLECH(int id, F_REGLECH item)
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
                if (!F_REGLECHExists(id))
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

        // POST: api/F_REGLECH
        [HttpPost]
        public async Task<ActionResult<F_REGLECH>> PostF_REGLECH(F_REGLECH item)
        {
            setDB(); _db.F_REGLECH.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_REGLECH", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_REGLECH/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_REGLECH(int id)
        {
            setDB(); var item = await _db.F_REGLECH.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_REGLECH.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_REGLECHExists(int id)
        {
            setDB(); return _db.F_REGLECH.Any(e => e.cbMarq == id);
        }
    }
}