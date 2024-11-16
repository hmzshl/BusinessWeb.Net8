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
 

    public partial class F_ARTSTOCKController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ARTSTOCKController(BusinessWebDBContext sdb)
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
        // GET: api/F_ARTSTOCK
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ARTSTOCK>>> GetF_ARTSTOCK()
        {
            setDB(); return await _db.F_ARTSTOCK.ToListAsync();
        }

        // GET: api/F_ARTSTOCK/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_ARTSTOCK>> GetF_ARTSTOCK(int id)
        {
            setDB(); var item = await _db.F_ARTSTOCK.FindAsync(id);

            if (item == null)
            {
                return new F_ARTSTOCK();
            }

            return item;
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<IEnumerable<F_ARTSTOCK>>> GetF_ARTSTOCKByAR_Ref(string AR_Ref)
		{
			setDB(); return await _db.F_ARTSTOCK.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
		}
		[HttpGet("DE_No/{DE_No}")]
		public async Task<ActionResult<IEnumerable<F_ARTSTOCK>>> GetF_ARTSTOCKByDE_No(int DE_No)
		{
			setDB(); return await _db.F_ARTSTOCK.Where(a => a.DE_No == DE_No).ToListAsync();
		}
		// PUT: api/F_ARTSTOCK/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_ARTSTOCK(int id, F_ARTSTOCK item)
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
                if (!F_ARTSTOCKExists(id))
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

        // POST: api/F_ARTSTOCK
        [HttpPost]
        public async Task<ActionResult<F_ARTSTOCK>> PostF_ARTSTOCK(F_ARTSTOCK item)
        {
            setDB(); _db.F_ARTSTOCK.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ARTSTOCK", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ARTSTOCK/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ARTSTOCK(int id)
        {
            setDB(); var item = await _db.F_ARTSTOCK.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ARTSTOCK.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ARTSTOCKExists(int id)
        {
            setDB(); return _db.F_ARTSTOCK.Any(e => e.cbMarq == id);
        }
    }
}