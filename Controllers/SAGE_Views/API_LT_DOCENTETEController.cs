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
using BusinessWeb.Models.LT;
using BusinessWeb.Models.BusinessWebDB;


namespace BusinessWeb.Controllers.SAGE_Tables
{
    [Route("api/{Societe}/[controller]")]
    [ApiController]

    public class API_LT_DOCENTETEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_LT_DOCENTETEController(BusinessWebDBContext sdb)
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
        // GET: api/API_LT_DOCENTETE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETE()
        {
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach(var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item,lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("DO_Piece/{DO_Piece}/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByDO_Piece(string DO_Piece, int DO_Type)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DO_Type == DO_Type && a.DO_Piece == DO_Piece).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
		[HttpGet("DO_Type/{DO_Type}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByDO_Type(int DO_Type)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DO_Type == DO_Type).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}
		[HttpGet("DO_Domaine/{DO_Domaine}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByDO_Domaine(int DO_Domaine)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DO_Domaine == DO_Domaine).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}
		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByCA_Num(string CA_Num)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.CA_Num == CA_Num).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		 
		}
		[HttpGet("CT_Num/{CT_Num}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByCT_Num(string CT_Num)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DO_Tiers == CT_Num).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}
		[HttpGet("DE_No/{DE_No}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByDE_No(int DE_No)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DE_No == DE_No).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}
		[HttpGet("CO_No/{CO_No}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByCO_No(int CO_No)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.CO_No == CO_No).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}
        [HttpGet("DO_Date/{DateDebut}/{DateFin}")]
		public async Task<ActionResult<IEnumerable<API_LT_DOCENTETE>>> GetAPI_LT_DOCENTETEByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB();
			List<API_V_DOCENTETE> dt = await _db.API_V_DOCENTETE.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();

			try
			{
				var rs = new List<API_LT_DOCENTETE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_DOCENTETE();
					fn.CopyData(item, lt);
					rs.Add(lt);

				}
				return Ok(rs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			 
		}



    }
}