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
 

    public partial class P_DOSSIERController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public P_DOSSIERController(BusinessWebDBContext sdb)
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
        // GET: api/P_DOSSIER
        [HttpGet]
        public async Task<ActionResult<IEnumerable<P_DOSSIER>>> GetP_DOSSIER()
        {
            setDB(); return await _db.P_DOSSIER.ToListAsync();
        }

        // GET: api/P_DOSSIER/5
        [HttpGet("{id}")]
        public async Task<ActionResult<P_DOSSIER>> GetP_DOSSIER(int id)
        {
            setDB(); var item = await _db.P_DOSSIER.FindAsync(id);

            if (item == null)
            {
                return new P_DOSSIER();
            }

            return item;
        }

		// PUT: api/P_DOSSIER/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutP_DOSSIER(int id, P_DOSSIER item)
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
                if (!P_DOSSIERExists(id))
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

        // POST: api/P_DOSSIER
        [HttpPost]
        public async Task<ActionResult<P_DOSSIER>> PostP_DOSSIER(P_DOSSIER item)
        {
            setDB(); _db.P_DOSSIER.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetP_DOSSIER", new { id = item.cbMarq }, item);
        }

        // DELETE: api/P_DOSSIER/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteP_DOSSIER(int id)
        {
            setDB(); var item = await _db.P_DOSSIER.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.P_DOSSIER.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool P_DOSSIERExists(int id)
        {
            setDB(); return _db.P_DOSSIER.Any(e => e.cbMarq == id);
        }
    }
}