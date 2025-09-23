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

    public class TSocieteUserController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public TSocieteUserController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSocieteUser>>> GetTSocieteUsers()
        {
            return await _sdb.TSocieteUsers.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TSocieteUser>> GetTSocieteUsers(int id)
        {
            var item = await _sdb.TSocieteUsers.FindAsync(id);

            if (item == null)
            {
                return new TSocieteUser();
            }

            return item;
        }
        
        private bool TSocieteUsersExists(int id)
        {
            return _sdb.TSocieteUsers.Any(e => e.id == id);
        }
		[HttpGet("User/{Id}")]
		public async Task<ActionResult<IEnumerable<TSocieteUser>>> GetTSocieteUserByUser(string Id)
		{
			return await _sdb.TSocieteUsers.Where(a => a.UserID == Id).ToListAsync();
		}
	}
}