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
 

    public partial class F_COMPTEGController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_COMPTEGController(BusinessWebDBContext sdb)
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
        // GET: api/F_COMPTEG
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_COMPTEG>>> GetF_COMPTEG()
        {
            setDB(); return await _db.F_COMPTEG.ToListAsync();
        }

        // GET: api/F_COMPTEG/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_COMPTEG>> GetF_COMPTEG(int id)
        {
            setDB(); var item = await _db.F_COMPTEG.FindAsync(id);

            if (item == null)
            {
                return new F_COMPTEG();
            }

            return item;
        }
		[HttpGet("CG_Num/{CG_Num}")]
		public async Task<ActionResult<F_COMPTEG>> GetF_COMPTEGByCG_Num(string CG_Num)
		{
			setDB(); var item = _db.F_COMPTEG.Where(a => a.CG_Num == CG_Num).SingleOrDefault();

			if (item == null)
			{
				return new F_COMPTEG();
			}

			return item;
		}

		// PUT: api/F_COMPTEG/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_COMPTEG(int id, F_COMPTEG item)
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
                if (!F_COMPTEGExists(id))
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

        // POST: api/F_COMPTEG
        [HttpPost]
        public async Task<ActionResult<F_COMPTEG>> PostF_COMPTEG(F_COMPTEG item)
        {
            setDB(); _db.F_COMPTEG.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_COMPTEG", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_COMPTEG/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_COMPTEG(int id)
        {
            setDB(); var item = await _db.F_COMPTEG.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_COMPTEG.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_COMPTEGExists(int id)
        {
            setDB(); return _db.F_COMPTEG.Any(e => e.cbMarq == id);
        }
    }
}