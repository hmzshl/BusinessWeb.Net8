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


    public partial class F_COMPTETController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_COMPTETController(BusinessWebDBContext sdb)
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
        // GET: api/F_COMPTET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_COMPTET>>> GetF_COMPTET()
        {
            setDB(); return await _db.F_COMPTET.ToListAsync();
        }
		[HttpGet("CT_Type/{CT_Type}")]
		public async Task<ActionResult<IEnumerable<F_COMPTET>>> GetF_COMPTETByCT_Type(int CT_Type)
		{
			setDB(); return await _db.F_COMPTET.Where(a => a.CT_Type == CT_Type).ToListAsync();
		}
		// GET: api/F_COMPTET/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_COMPTET>> GetF_COMPTET(int id)
        {
            setDB(); var item = await _db.F_COMPTET.FindAsync(id);

            if (item == null)
            {
                return new F_COMPTET();
            }

            return item;
        }
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<F_COMPTET>> GetF_COMPTETByAR_Ref(string CT_Num)
		{
			setDB(); var item = _db.F_COMPTET.Where(a => a.CT_Num == CT_Num).SingleOrDefault();

			if (item == null)
			{
				return new F_COMPTET();
			}

			return item;
		}

		// PUT: api/F_COMPTET/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_COMPTET(int id, F_COMPTET item)
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
                if (!F_COMPTETExists(id))
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

        // POST: api/F_COMPTET
        [HttpPost]
        public async Task<ActionResult<F_COMPTET>> PostF_COMPTET(F_COMPTET item)
        {
            setDB(); _db.F_COMPTET.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_COMPTET", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_COMPTET/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_COMPTET(int id)
        {
            setDB(); var item = await _db.F_COMPTET.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_COMPTET.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_COMPTETExists(int id)
        {
            setDB(); return _db.F_COMPTET.Any(e => e.cbMarq == id);
        }
    }
}