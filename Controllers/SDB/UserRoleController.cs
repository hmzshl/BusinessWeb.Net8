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

    public class UserRoleController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public UserRoleController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserRole>>> GetUserRoles()
        {
            return await _sdb.AspNetUserRoles.ToListAsync();
        }
		[HttpGet("UserID/{UserID}")]
		public async Task<ActionResult<IEnumerable<AspNetUserRole>>> GetAspNetUserRoleByUserID(string UserID)
		{
			return await _sdb.AspNetUserRoles.Where(a => a.UserId == UserID).ToListAsync();
		}

	}
}