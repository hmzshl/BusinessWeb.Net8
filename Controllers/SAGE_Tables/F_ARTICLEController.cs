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
 

    public partial class F_ARTICLEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ARTICLEController(BusinessWebDBContext sdb)
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
        // GET: api/F_ARTICLE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ARTICLE>>> GetF_ARTICLE()
        {
            setDB(); return await _db.F_ARTICLE.ToListAsync();
        }

        // GET: api/F_ARTICLE/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_ARTICLE>> GetF_ARTICLE(int id)
        {
            setDB(); var item = await _db.F_ARTICLE.FindAsync(id);

            if (item == null)
            {
                return new F_ARTICLE();
            }

            return item;
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<F_ARTICLE>> GetF_ARTICLEByAR_Ref(string AR_Ref)
		{
			setDB(); var item = _db.F_ARTICLE.Where(a => a.AR_Ref == AR_Ref).SingleOrDefault();

			if (item == null)
			{
				return new F_ARTICLE();
			}

			return item;
		}

		// PUT: api/F_ARTICLE/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_ARTICLE(int id, F_ARTICLE item)
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
                if (!F_ARTICLEExists(id))
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

        // POST: api/F_ARTICLE
        [HttpPost]
        public async Task<ActionResult<F_ARTICLE>> PostF_ARTICLE(F_ARTICLE item)
        {
            setDB(); _db.F_ARTICLE.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ARTICLE", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ARTICLE/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ARTICLE(int id)
        {
            setDB(); var item = await _db.F_ARTICLE.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ARTICLE.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ARTICLEExists(int id)
        {
            setDB(); return _db.F_ARTICLE.Any(e => e.cbMarq == id);
        }
    }
}