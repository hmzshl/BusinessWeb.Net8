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

    public class AspNetUserController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public AspNetUserController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetAspNetUsers()
        {
            return await _sdb.AspNetUsers.ToListAsync();
        }

        [HttpGet("{UserName}")]
        public async Task<ActionResult<AspNetUser>> GetAspNetUsers(string UserName)
        {
            UserName = (UserName??"").ToUpper();
            var item = _sdb.AspNetUsers.Where(a => a.NormalizedUserName == UserName).SingleOrDefault();

            if (item == null)
            {
                return new AspNetUser();
            }

            return item;
        }

    }
}