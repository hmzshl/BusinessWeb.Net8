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
 

    public partial class F_DOCREGLController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_DOCREGLController(BusinessWebDBContext sdb)
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
        // GET: api/F_DOCREGL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_DOCREGL>>> GetF_DOCREGL()
        {
            setDB(); return await _db.F_DOCREGL.ToListAsync();
        }

        // GET: api/F_DOCREGL/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_DOCREGL>> GetF_DOCREGL(int id)
        {
            setDB(); var item = await _db.F_DOCREGL.FindAsync(id);

            if (item == null)
            {
                return new F_DOCREGL();
            }

            return item;
        }

		// PUT: api/F_DOCREGL/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_DOCREGL(int id, F_DOCREGL item)
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
                if (!F_DOCREGLExists(id))
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

        // POST: api/F_DOCREGL
        [HttpPost]
        public async Task<ActionResult<F_DOCREGL>> PostF_DOCREGL(F_DOCREGL item)
        {
            setDB(); _db.F_DOCREGL.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_DOCREGL", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_DOCREGL/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_DOCREGL(int id)
        {
            setDB(); var item = await _db.F_DOCREGL.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_DOCREGL.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_DOCREGLExists(int id)
        {
            setDB(); return _db.F_DOCREGL.Any(e => e.cbMarq == id);
        }
    }
}