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
 

    public partial class F_FAMCLIENTController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_FAMCLIENTController(BusinessWebDBContext sdb)
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
        // GET: api/F_FAMCLIENT
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_FAMCLIENT>>> GetF_FAMCLIENT()
        {
            setDB(); return await _db.F_FAMCLIENT.ToListAsync();
        }
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_FAMCLIENT>>> GetF_FAMCLIENTByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_FAMCLIENT.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("FA_CodeFamille/{FA_CodeFamille}")]
		public async Task<ActionResult<IEnumerable<F_FAMCLIENT>>> GetF_FAMCLIENTByFA_CodeFamille(string FA_CodeFamille)
		{
			setDB(); return await _db.F_FAMCLIENT.Where(a => a.FA_CodeFamille == FA_CodeFamille).ToListAsync();
		}
		// GET: api/F_FAMCLIENT/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_FAMCLIENT>> GetF_FAMCLIENT(int id)
        {
            setDB(); var item = await _db.F_FAMCLIENT.FindAsync(id);

            if (item == null)
            {
                return new F_FAMCLIENT();
            }

            return item;
        }

		// PUT: api/F_FAMCLIENT/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_FAMCLIENT(int id, F_FAMCLIENT item)
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
                if (!F_FAMCLIENTExists(id))
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

        // POST: api/F_FAMCLIENT
        [HttpPost]
        public async Task<ActionResult<F_FAMCLIENT>> PostF_FAMCLIENT(F_FAMCLIENT item)
        {
            setDB(); _db.F_FAMCLIENT.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_FAMCLIENT", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_FAMCLIENT/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_FAMCLIENT(int id)
        {
            setDB(); var item = await _db.F_FAMCLIENT.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_FAMCLIENT.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_FAMCLIENTExists(int id)
        {
            setDB(); return _db.F_FAMCLIENT.Any(e => e.cbMarq == id);
        }
    }
}