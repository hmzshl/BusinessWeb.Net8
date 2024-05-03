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
 

    public partial class F_ARTCLIENTController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ARTCLIENTController(BusinessWebDBContext sdb)
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
        // GET: api/F_ARTCLIENT
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ARTCLIENT>>> GetF_ARTCLIENT()
        {
            setDB(); return await _db.F_ARTCLIENT.ToListAsync();
        }

        // GET: api/F_ARTCLIENT/5
        [HttpGet("{id}")]
        public async Task<ActionResult<F_ARTCLIENT>> GetF_ARTCLIENT(int id)
        {
            setDB(); var item = await _db.F_ARTCLIENT.FindAsync(id);

            if (item == null)
            {
                return new F_ARTCLIENT();
            }

            return item;
        }
		[HttpGet("AR_Ref/{AR_Ref}")]
		public async Task<ActionResult<IEnumerable<F_ARTCLIENT>>> GetF_ARTCLIENTByAR_Ref(string AR_Ref)
		{
			setDB(); return await _db.F_ARTCLIENT.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_ARTCLIENT>>> GetF_ARTCLIENTByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_ARTCLIENT.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		// PUT: api/F_ARTCLIENT/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutF_ARTCLIENT(int id, F_ARTCLIENT item)
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
                if (!F_ARTCLIENTExists(id))
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

        // POST: api/F_ARTCLIENT
        [HttpPost]
        public async Task<ActionResult<F_ARTCLIENT>> PostF_ARTCLIENT(F_ARTCLIENT item)
        {
            setDB(); _db.F_ARTCLIENT.Add(item);
            setDB(); await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ARTCLIENT", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ARTCLIENT/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ARTCLIENT(int id)
        {
            setDB(); var item = await _db.F_ARTCLIENT.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ARTCLIENT.Remove(item);
            setDB(); await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ARTCLIENTExists(int id)
        {
            setDB(); return _db.F_ARTCLIENT.Any(e => e.cbMarq == id);
        }
    }
}