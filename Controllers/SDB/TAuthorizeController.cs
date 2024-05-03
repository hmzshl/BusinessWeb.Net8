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


namespace BusinessWeb.Controllers.SDB
{
    [Route("api/[controller]")]
    [ApiController]

    public class TAuthorizeController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public TAuthorizeController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TAuthorize>>> GetTAuthorizes()
        {
            return await _sdb.TAuthorizes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TAuthorize>> GetTAuthorizes(int id)
        {
            var item = await _sdb.TAuthorizes.FindAsync(id);

            if (item == null)
            {
                return new TAuthorize();
            }

            return item;
        }
        
        private bool TAuthorizesExists(int id)
        {
            return _sdb.TAuthorizes.Any(e => e.id == id);
        }
		[HttpGet("User/{Id}")]
		public async Task<ActionResult<IEnumerable<TAuthorize>>> GetTAuthorizeByUser(string Id)
		{
			return await _sdb.TAuthorizes.Where(a => a.UserID == Id).ToListAsync();
		}
	}
}