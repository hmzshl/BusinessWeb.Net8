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
 

    public partial class P_REGLEMENTController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public P_REGLEMENTController(BusinessWebDBContext sdb)
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
        // GET: api/P_REGLEMENT
        [HttpGet]
        public async Task<ActionResult<IEnumerable<P_REGLEMENT>>> GetP_REGLEMENT()
        {
            setDB(); return await _db.P_REGLEMENT.ToListAsync();
        }

        // GET: api/P_REGLEMENT/5
        [HttpGet("{id}")]
        public async Task<ActionResult<P_REGLEMENT>> GetP_REGLEMENT(int id)
        {
            setDB(); var item = await _db.P_REGLEMENT.FindAsync(id);

            if (item == null)
            {
                return new P_REGLEMENT();
            }

            return item;
        }

		// PUT: api/P_REGLEMENT/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutP_REGLEMENT(int id, P_REGLEMENT item)
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
                if (!P_REGLEMENTExists(id))
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

        // POST: api/P_REGLEMENT
        [HttpPost]
        public async Task<ActionResult<P_REGLEMENT>> PostP_REGLEMENT(P_REGLEMENT item)
        {
            setDB(); _db.P_REGLEMENT.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetP_REGLEMENT", new { id = item.cbMarq }, item);
        }

        // DELETE: api/P_REGLEMENT/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteP_REGLEMENT(int id)
        {
            setDB(); var item = await _db.P_REGLEMENT.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.P_REGLEMENT.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool P_REGLEMENTExists(int id)
        {
            setDB(); return _db.P_REGLEMENT.Any(e => e.cbMarq == id);
        }
    }
}