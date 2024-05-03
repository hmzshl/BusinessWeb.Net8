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

    public class API_LT_MARGEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();
        public API_LT_MARGEController(BusinessWebDBContext sdb)
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
        // GET: api/API_LT_MARGE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGE()
        {
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach(var item in dt)
				{
					var lt = new API_LT_MARGE();
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

		[HttpGet("CA_Num/{CA_Num}")]
		public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGEByCA_Num(string CA_Num)
		{
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.Where(a => a.CA_Num == CA_Num).ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_MARGE();
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
		public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGEByCT_Num(string CT_Num)
		{
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.Where(a => a.CT_Num == CT_Num).ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_MARGE();
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
		public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGEByCO_No(int CO_No)
		{
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.Where(a => a.CO_No == CO_No).ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_MARGE();
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
        [HttpGet("AR_Ref/{AR_Ref}")]
        public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGEByAR_Ref(string AR_Ref)
        {
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.Where(a => a.AR_Ref == AR_Ref).ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_MARGE();
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
		public async Task<ActionResult<IEnumerable<API_LT_MARGE>>> GetAPI_LT_MARGEByDO_Date(DateTime DateDebut, DateTime DateFin)
		{
			setDB();
			List<API_V_MARGE> dt = await _db.API_V_MARGE.Where(a => a.DO_Date >= DateDebut && a.DO_Date <= DateFin).ToListAsync();

			try
			{
				var rs = new List<API_LT_MARGE>();
				foreach (var item in dt)
				{
					var lt = new API_LT_MARGE();
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