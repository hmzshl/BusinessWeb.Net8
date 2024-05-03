using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ReportViewerController : Controller, IReportController
    {
        private IMemoryCache _cache;

        private IWebHostEnvironment _hostingEnvironment;

        public ReportViewerController(IMemoryCache memoryCache,
            IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }

        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            FileStream reportStream = new FileStream(basePath + @".\Resources\" + reportOption.ReportModel.ReportPath + ".rdl", FileMode.Open, FileAccess.Read);
            reportOption.ReportModel.Stream = reportStream;
        }

        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [HttpGet]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        [HttpPost]
        public object PostReportAction([FromBody]Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this, _cache);
        }

        [HttpPost]
        public object Pdf([FromBody] Dictionary<string, object> exportDetails)
        {
            string _token = exportDetails["reportViewerToken"].ToString();
            var stream = ReportHelper.GetReport(_token, exportDetails["exportType"].ToString(), this, _cache);
            stream.Position = 0;
            // Steps to generate PDF report using Report Writer.
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            byte[] data = memoryStream.ToArray();
            string file = Convert.ToBase64String(data, 0, data.Length);
            return file;
        }
    }
}
