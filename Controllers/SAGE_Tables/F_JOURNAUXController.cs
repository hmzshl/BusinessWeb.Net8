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
 

    public partial class F_JOURNAUXController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_JOURNAUXController(BusinessWebDBContext sdb)
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
        // GET: api/F_JOURNAUX
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_JOURNAUX>>> GetF_JOURNAUX()
        {
            setDB(); return await _db.F_JOURNAUX.ToListAsync();
        }

        // GET: api/F_JOURNAUX/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_JOURNAUX>> GetF_JOURNAUX(int id)
        {
            setDB(); var item = await _db.F_JOURNAUX.FindAsync(id);

            if (item == null)
            {
                return new F_JOURNAUX();
            }

            return item;
        }
		[HttpGet("JO_Num/{JO_Num}")]
		public async Task<ActionResult<F_JOURNAUX>> GetF_JOURNAUXByJO_Num(string JO_Num)
		{
			setDB(); var item = _db.F_JOURNAUX.Where(a => a.JO_Num == JO_Num).SingleOrDefault();

			if (item == null)
			{
				return new F_JOURNAUX();
			}

			return item;
		}

		// PUT: api/F_JOURNAUX/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_JOURNAUX(int id, F_JOURNAUX item)
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
                if (!F_JOURNAUXExists(id))
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

        // POST: api/F_JOURNAUX
        [HttpPost]
        public async Task<ActionResult<F_JOURNAUX>> PostF_JOURNAUX(F_JOURNAUX item)
        {
            setDB(); _db.F_JOURNAUX.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_JOURNAUX", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_JOURNAUX/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_JOURNAUX(int id)
        {
            setDB(); var item = await _db.F_JOURNAUX.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_JOURNAUX.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_JOURNAUXExists(int id)
        {
            setDB(); return _db.F_JOURNAUX.Any(e => e.cbMarq == id);
        }
    }
}