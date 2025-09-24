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
		public async Task<ActionResult<IEnumerable<TSociete>>> GetTSocieteUserByUser(string Id)
		{
            var allowed_stes = _sdb.TSocieteUsers.Where(a => a.UserID == Id).Select(a => a.Societe);
            Helpers fn = new Helpers();
            List<TSociete> dt = await _sdb.TSocietes.Where(a => allowed_stes.Contains(a.id)).ToListAsync();

            try
            {
                var rs = new List<TSociete>();
                foreach (var item in dt)
                {
                    var lt = new TSociete();
                    fn.CopyData(item, lt);
                    lt.Serveur = null;
                    lt.Passe = null;
                    lt.Base1 = null;
                    lt.Web = null;
                    lt.id = item.id;
                    rs.Add(lt);

                }
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            //return await _sdb.TSocietes.ToListAsync();


        }
    }
}