using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using BusinessWeb.Data;

namespace BusinessWeb.Controllers
{
    public partial class ExportBusinessWebDBController : ExportController
    {
        private readonly BusinessWebDBContext context;
        private readonly BusinessWebDBService service;

        public ExportBusinessWebDBController(BusinessWebDBContext context, BusinessWebDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/BusinessWebDB/aspnetroleclaims/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetroleclaims/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRoleClaimsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetRoleClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetroleclaims/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetroleclaims/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRoleClaimsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetRoleClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetroles/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetroles/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserclaims/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetuserclaims/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserClaimsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserclaims/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetuserclaims/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserClaimsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserlogins/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetuserlogins/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserLoginsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserLogins(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserlogins/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetuserlogins/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserLoginsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserLogins(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserroles/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetuserroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetuserroles/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetuserroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetusers/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetusers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetusers/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetusers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetusertokens/csv")]
        [HttpGet("/export/BusinessWebDB/aspnetusertokens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserTokensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserTokens(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/aspnetusertokens/excel")]
        [HttpGet("/export/BusinessWebDB/aspnetusertokens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserTokensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserTokens(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tlicenses/csv")]
        [HttpGet("/export/BusinessWebDB/tlicenses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTLicensesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTLicenses(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tlicenses/excel")]
        [HttpGet("/export/BusinessWebDB/tlicenses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTLicensesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTLicenses(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tsocietes/csv")]
        [HttpGet("/export/BusinessWebDB/tsocietes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTSocietesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTSocietes(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tsocietes/excel")]
        [HttpGet("/export/BusinessWebDB/tsocietes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTSocietesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTSocietes(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tauthorizes/csv")]
        [HttpGet("/export/BusinessWebDB/tauthorizes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTAuthorizesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTAuthorizes(), Request.Query), fileName);
        }

        [HttpGet("/export/BusinessWebDB/tauthorizes/excel")]
        [HttpGet("/export/BusinessWebDB/tauthorizes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTAuthorizesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTAuthorizes(), Request.Query), fileName);
        }
    }
}
