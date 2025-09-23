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

    public class TCollaborateurController : ControllerBase
    {
        public BusinessWebDBContext _sdb = new BusinessWebDBContext();

        public TCollaborateurController(BusinessWebDBContext sdb)
        {
            _sdb = sdb;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCollaborateur>>> GetTCollaborateurs()
        {
            return await _sdb.TCollaborateurs.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TCollaborateur>> GetTCollaborateurs(int id)
        {
            var item = await _sdb.TCollaborateurs.FindAsync(id);

            if (item == null)
            {
                return new TCollaborateur();
            }

            return item;
        }
        
        private bool TCollaborateursExists(int id)
        {
            return _sdb.TCollaborateurs.Any(e => e.id == id);
        }
		[HttpGet("User/{Id}")]
		public async Task<ActionResult<IEnumerable<TCollaborateur>>> GetTCollaborateurByUser(string Id)
		{
			return await _sdb.TCollaborateurs.Where(a => a.UserName == Id).ToListAsync();
		}
	}
}