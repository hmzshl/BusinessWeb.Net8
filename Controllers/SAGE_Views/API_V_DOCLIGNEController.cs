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

    public class API_V_DOCLIGNEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_V_DOCLIGNEController(BusinessWebDBContext sdb)
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
        // GET: api/API_V_DOCLIGNE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNE()
        {
            setDB(); return await _db.API_V_DOCLIGNE.ToListAsync();
        }

		[HttpGet("DO_Piece/{DO_Piece}/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByDO_Piece(string DO_Piece, int DO_Type)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.DO_Type == DO_Type && a.DO_Piece == DO_Piece).ToListAsync();
		}
		[HttpGet("DO_Type/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByDO_Type(int DO_Type)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.DO_Type == DO_Type).ToListAsync();
		}
		[HttpGet("DO_Domaine/{DO_Domaine}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByDO_Domaine(int DO_Domaine)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.DO_Domaine == DO_Domaine).ToListAsync();
		}
		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByCA_Num(string CA_Num)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.CA_Num == CA_Num).ToListAsync();
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByCT_Num(string CT_Num)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.CT_Num == CT_Num).ToListAsync();
		}
		[HttpGet("DE_No/{DE_No}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByDE_No(int DE_No)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.DE_No == DE_No).ToListAsync();
		}
		[HttpGet("CO_No/{CO_No}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByCO_No(int CO_No)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.CO_No == CO_No).ToListAsync();
		}
        [HttpGet("AR_Ref/{AR_Ref}")]
        public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByAR_Ref(string AR_Ref)
        {
            setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.AR_Ref == AR_Ref).ToListAsync();
        }
        [HttpGet("DO_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<API_V_DOCLIGNE>>> GetAPI_V_DOCLIGNEByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB(); return await _db.API_V_DOCLIGNE.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();
		}




		// GET: api/API_V_DOCLIGNE/5
		[HttpGet("{id}")]
        public async Task<ActionResult<API_V_DOCLIGNE>> GetAPI_V_DOCLIGNE(int id)
        {
            setDB(); var item = _db.API_V_DOCLIGNE.Where(a => a.cbMarq == id).SingleOrDefault();

            if (item == null)
            {
                return new API_V_DOCLIGNE();
            }

            return item;
        }


    }
}