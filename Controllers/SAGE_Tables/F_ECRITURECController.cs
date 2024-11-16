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


    public partial class F_ECRITURECController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public F_ECRITURECController(BusinessWebDBContext sdb)
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
        // GET: api/F_ECRITUREC
        [HttpGet]
        public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITUREC()
        {
            setDB(); return await _db.F_ECRITUREC.ToListAsync();
        }

		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByDO_Type(string CT_Num)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("JO_Num/{JO_Num}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByJO_Num(string JO_Num)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.JO_Num == JO_Num).ToListAsync();
		}
		[HttpGet("CG_Num/{CG_Num}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByCG_Num(string CG_Num)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.CG_Num == CG_Num).ToListAsync();
		}
		[HttpGet("EC_RefPiece/{EC_RefPiece}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByEC_RefPiece(string EC_RefPiece)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.EC_RefPiece == EC_RefPiece).ToListAsync();
		}
		[HttpGet("EC_Piece/{EC_Piece}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByEC_Piece(string EC_Piece)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.EC_Piece == EC_Piece).ToListAsync();
		}
		[HttpGet("EC_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByEC_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.JM_Date >= DateDebut && a.JM_Date <= DateFin).ToListAsync();
		}
		[HttpGet("EC_Montant/{MontantDebut}/{MontantFin}")]
		public async Task<ActionResult<IEnumerable<F_ECRITUREC>>> GetF_ECRITURECByEC_Montant(Decimal MontantDebut, Decimal MontantFin)
		{
			setDB(); return await _db.F_ECRITUREC.Where(a => a.EC_Montant >= MontantDebut && a.EC_Montant <= MontantFin).ToListAsync();
		}




		// GET: api/F_ECRITUREC/5
		[HttpGet("{id}")]
        public async Task<ActionResult<F_ECRITUREC>> GetF_ECRITUREC(int id)
        {
            setDB(); var item = await _db.F_ECRITUREC.FindAsync(id);

            if (item == null)
            {
                return new F_ECRITUREC();
            }

            return item;
        }

        // PUT: api/F_ECRITUREC/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutF_ECRITUREC(int id, F_ECRITUREC item)
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
                if (!F_ECRITURECExists(id))
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

        // POST: api/F_ECRITUREC
        [HttpPost]
        public async Task<ActionResult<F_ECRITUREC>> PostF_ECRITUREC(F_ECRITUREC item)
        {
            setDB(); _db.F_ECRITUREC.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetF_ECRITUREC", new { id = item.cbMarq }, item);
        }

        // DELETE: api/F_ECRITUREC/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteF_ECRITUREC(int id)
        {
            setDB(); var item = await _db.F_ECRITUREC.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _db.F_ECRITUREC.Remove(item);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        private bool F_ECRITURECExists(int id)
        {
            setDB(); return _db.F_ECRITUREC.Any(e => e.cbMarq == id);
        }
    }
}