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
 

    public partial class F_ARTFOURNISSController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ARTFOURNISSController(BusinessWebDBContext sdb)
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
        // GET: api/F_ARTFOURNISS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ARTFOURNISS>>> GetF_ARTFOURNISS()
        {
            setDB(); return await _db.F_ARTFOURNISS.ToListAsync();
        }

        // GET: api/F_ARTFOURNISS/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_ARTFOURNISS>> GetF_ARTFOURNISS(int id)
        {
            setDB(); var item = await _db.F_ARTFOURNISS.FindAsync(id);

            if (item == null)
            {
                return new F_ARTFOURNISS();
            }

            return item;
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<IEnumerable<F_ARTFOURNISS>>> GetF_ARTFOURNISSByAR_Ref(string AR_Ref)
		{
			setDB(); return await _db.F_ARTFOURNISS.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_ARTFOURNISS>>> GetF_ARTFOURNISSByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_ARTFOURNISS.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		// PUT: api/F_ARTFOURNISS/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_ARTFOURNISS(int id, F_ARTFOURNISS item)
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
                if (!F_ARTFOURNISSExists(id))
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

        // POST: api/F_ARTFOURNISS
        [HttpPost]
        public async Task<ActionResult<F_ARTFOURNISS>> PostF_ARTFOURNISS(F_ARTFOURNISS item)
        {
            setDB(); _db.F_ARTFOURNISS.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ARTFOURNISS", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ARTFOURNISS/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ARTFOURNISS(int id)
        {
            setDB(); var item = await _db.F_ARTFOURNISS.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ARTFOURNISS.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ARTFOURNISSExists(int id)
        {
            setDB(); return _db.F_ARTFOURNISS.Any(e => e.cbMarq == id);
        }
    }
}