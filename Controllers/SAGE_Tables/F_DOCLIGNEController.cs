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


    public partial class F_DOCLIGNEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_DOCLIGNEController(BusinessWebDBContext sdb)
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
        // GET: api/F_DOCLIGNE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNE()
        {
            setDB(); return await _db.F_DOCLIGNE.ToListAsync();
        }


		[HttpGet("DO_Piece/{DO_Piece}/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByDO_Piece(string DO_Piece, int DO_Type)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.DO_Type == DO_Type && a.DO_Piece == DO_Piece).ToListAsync();
		}
		[HttpGet("DO_Type/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByDO_Type(int DO_Type)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.DO_Type == DO_Type).ToListAsync();
		}
		[HttpGet("DO_Domaine/{DO_Domaine}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByDO_Domaine(int DO_Domaine)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.DO_Domaine == DO_Domaine).ToListAsync();
		}
		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByCA_Num(string CA_Num)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.CA_Num == CA_Num).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByCT_Num(string CT_Num)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("DE_No/{DE_No}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByDE_No(int DE_No)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.DE_No == DE_No).ToListAsync();
		}
		[HttpGet("CO_No/{CO_No}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByCO_No(int CO_No)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.CO_No == CO_No).ToListAsync();
		}
        [HttpGet("AR_Ref/{AR_Ref}")]
        public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByAR_Ref(string AR_Ref)
        {
            setDB(); return await _db.F_DOCLIGNE.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
        }
        [HttpGet("DO_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<F_DOCLIGNE>>> GetF_DOCLIGNEByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.F_DOCLIGNE.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();
		}


		// GET: api/F_DOCLIGNE/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_DOCLIGNE>> GetF_DOCLIGNE(int id)
        {
            setDB(); var item = await _db.F_DOCLIGNE.FindAsync(id);

            if (item == null)
            {
                return new F_DOCLIGNE();
            }

            return item;
        }

        // PUT: api/F_DOCLIGNE/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutF_DOCLIGNE(int id, F_DOCLIGNE item)
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
                if (!F_DOCLIGNEExists(id))
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

        // POST: api/F_DOCLIGNE
        [HttpPost]
        public async Task<ActionResult<F_DOCLIGNE>> PostF_DOCLIGNE(F_DOCLIGNE item)
        {
            setDB(); _db.F_DOCLIGNE.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_DOCLIGNE", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_DOCLIGNE/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_DOCLIGNE(int id)
        {
            setDB(); var item = await _db.F_DOCLIGNE.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_DOCLIGNE.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_DOCLIGNEExists(int id)
        {
            setDB(); return _db.F_DOCLIGNE.Any(e => e.cbMarq == id);
        }
    }
}