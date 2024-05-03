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
 

    public partial class F_FAMFOURNISSController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_FAMFOURNISSController(BusinessWebDBContext sdb)
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
        // GET: api/F_FAMFOURNISS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_FAMFOURNISS>>> GetF_FAMFOURNISS()
        {
            setDB(); return await _db.F_FAMFOURNISS.ToListAsync();
        }
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_FAMFOURNISS>>> GetF_FAMFOURNISSByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_FAMFOURNISS.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("FA_CodeFamille/{FA_CodeFamille}")]
		public async Task<ActionResult<IEnumerable<F_FAMFOURNISS>>> GetF_FAMFOURNISSByFA_CodeFamille(string FA_CodeFamille)
		{
			setDB(); return await _db.F_FAMFOURNISS.Where(a => a.FA_CodeFamille == FA_CodeFamille).ToListAsync();
		}
		// GET: api/F_FAMFOURNISS/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_FAMFOURNISS>> GetF_FAMFOURNISS(int id)
        {
            setDB(); var item = await _db.F_FAMFOURNISS.FindAsync(id);

            if (item == null)
            {
                return new F_FAMFOURNISS();
            }

            return item;
        }

		// PUT: api/F_FAMFOURNISS/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_FAMFOURNISS(int id, F_FAMFOURNISS item)
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
                if (!F_FAMFOURNISSExists(id))
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

        // POST: api/F_FAMFOURNISS
        [HttpPost]
        public async Task<ActionResult<F_FAMFOURNISS>> PostF_FAMFOURNISS(F_FAMFOURNISS item)
        {
            setDB(); _db.F_FAMFOURNISS.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_FAMFOURNISS", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_FAMFOURNISS/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_FAMFOURNISS(int id)
        {
            setDB(); var item = await _db.F_FAMFOURNISS.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_FAMFOURNISS.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_FAMFOURNISSExists(int id)
        {
            setDB(); return _db.F_FAMFOURNISS.Any(e => e.cbMarq == id);
        }
    }
}