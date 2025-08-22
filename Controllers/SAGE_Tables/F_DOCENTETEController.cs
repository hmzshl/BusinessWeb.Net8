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


    public partial class Save_DOCENTETEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public Save_DOCENTETEController(BusinessWebDBContext sdb)
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
        // GET: api/F_DOCENTETE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETE()
        {
            setDB(); return await _db.F_DOCENTETE.ToListAsync();
        }


		[HttpGet("DO_Piece/{DO_Piece}/{DO_Type}")]
		public async Task<ActionResult<F_DOCENTETE>> GetF_DOCENTETEByDO_Piece(string DO_Piece, int DO_Type)
		{
			setDB(); var item = _db.F_DOCENTETE.Where(a => a.DO_Type == DO_Type && a.DO_Piece == DO_Piece).SingleOrDefault();

			if (item == null)
			{
				return new F_DOCENTETE();
			}

			return item;
		}
		[HttpGet("DO_Type/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByDO_Type(int DO_Type)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.DO_Type == DO_Type).ToListAsync();
		}
		[HttpGet("DO_Domaine/{DO_Domaine}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByDO_Domaine(int DO_Domaine)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.DO_Domaine == DO_Domaine).ToListAsync();
		}
		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByCA_Num(string CA_Num)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.CA_Num == CA_Num).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.DO_Tiers == CT_Num).ToListAsync();
		}
		[HttpGet("DE_No/{DE_No}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByDE_No(int DE_No)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.DE_No == DE_No).ToListAsync();
		}
		[HttpGet("CO_No/{CO_No}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByCO_No(int CO_No)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.CO_No == CO_No).ToListAsync();
		}
		[HttpGet("DO_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<F_DOCENTETE>>> GetF_DOCENTETEByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.F_DOCENTETE.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();
		}





		// GET: api/F_DOCENTETE/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_DOCENTETE>> GetF_DOCENTETE(int id)
        {
            setDB(); var item = await _db.F_DOCENTETE.FindAsync(id);

            if (item == null)
            {
                return new F_DOCENTETE();
            }

            return item;
        }

        // PUT: api/F_DOCENTETE/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutF_DOCENTETE(int id, F_DOCENTETE item)
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
                if (!F_DOCENTETEExists(id))
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

        // POST: api/F_DOCENTETE
        [HttpPost]
        public async Task<ActionResult<F_DOCENTETE>> PostF_DOCENTETE(F_DOCENTETE item)
        {
            setDB(); _db.F_DOCENTETE.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_DOCENTETE", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_DOCENTETE/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_DOCENTETE(int id)
        {
            setDB(); var item = await _db.F_DOCENTETE.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_DOCENTETE.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_DOCENTETEExists(int id)
        {
            setDB(); return _db.F_DOCENTETE.Any(e => e.cbMarq == id);
        }
    }
}