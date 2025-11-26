using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using BusinessWeb.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace BusinessWeb.Controllers
{
    [Route("api/{controller}/{ste}/{action}/{id?}")]
    public class BoldReportsSQLController : ControllerBase, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client requests and
        // the rendered report viewer in the server.
        private IMemoryCache _cache;

        // IHostingEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;
        Dictionary<string, object> jsonResult = null;
        BusinessWebDBContext _sdb;
        public BoldReportsSQLController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment, BusinessWebDBContext sdb)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _sdb = sdb;
        }
        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

		// Method will be called to initialize the report information to load the report with ReportHelper for processing.
		public void OnInitReportOptions(ReportViewerOptions reportOption)
		{
			string basePath = _hostingEnvironment.WebRootPath;
			// Here, we have loaded the sales-order-detail.rdl report from the application folder wwwroot\Resources. sales-order-detail.rdl should be in the wwwroot\Resources application folder.
			System.IO.FileStream reportStream = new System.IO.FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
			reportOption.ReportModel.Stream = reportStream;
            Helpers fn = new Helpers();

			DataSourceCredentials dataSourceCredentials = new DataSourceCredentials();
			//You have to provide the shared data source name used with the report or the data source name available with the report.
			dataSourceCredentials.Name = "DataSource1";
            dataSourceCredentials.ConnectionString = fn.getDb(_sdb.TSocietes.Where(a => a.id == Int16.Parse(RouteData.Values["ste"].ToString())).SingleOrDefault())?.Database?.GetConnectionString(); ;
			reportOption.ReportModel.DataSourceCredentials = new List<DataSourceCredentials> { dataSourceCredentials };
		}
		// Method will be called when report is loaded internally to start the layout process with ReportHelper.
		public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            //    var reportParameters = ReportHelper.GetParameters(jsonResult, this, _cache);
            //    List<BoldReports.Web.ReportParameter> setParameters = new List<BoldReports.Web.ReportParameter>();

            //    if (reportParameters != null)
            //    {
            //        foreach (var rptParameter in reportParameters)
            //        {
            //            setParameters.Add(new BoldReports.Web.ReportParameter()
            //            {
            //                Name = rptParameter.Name,
            //                Values = new List<string>() { "SO50756" }
            //            });
            //        }

            //        reportOption.ReportModel.Parameters = setParameters;

            //}
           
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        // Post action to process the report from the server based on json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            jsonResult = jsonArray;
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }
    }

}