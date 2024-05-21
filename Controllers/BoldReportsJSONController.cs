using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessWeb.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
    [ApiController]
    public class BoldReportsJSON : ControllerBase, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client requests and
        // the rendered report viewer in the server.
        private IMemoryCache _cache;

        // IHostingEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;
        Dictionary<string, object> jsonResult = null;
        private string reportData;
        public BoldReportsJSON(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
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
            //reportOption.ReportModel.ProcessingMode = BoldReports.Web.ReportViewer.ProcessingMode.Local;
            // Here, we have loaded the sales-order-detail.rdl report from application the folder wwwroot\Resources. sales-order-detail.rdl should be there in wwwroot\Resources application folder.
            FileStream inputStream = new FileStream(basePath + @"\Resources\" + reportOption.ReportModel.ReportPath + ".rdl", FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
        }
        // Method will be called when report is loaded internally to start the layout process with ReportHelper.
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            List<DataSourceInfo> datasources = ReportHelper.GetDataSources(jsonResult, this, _cache);

            foreach (DataSourceInfo item in datasources)
            {
                FileDataModel model = new FileDataModel();
                model.DataMode = "inline";
                model.Data = reportData;
                item.DataProvider = "JSON";

                DataSourceCredentials DataSourceCredentials = new DataSourceCredentials();
                DataSourceCredentials.Name = item.DataSourceName;
                DataSourceCredentials.UserId = null;
                DataSourceCredentials.Password = null;
                DataSourceCredentials.ConnectionString = JsonConvert.SerializeObject(model);
                DataSourceCredentials.IntegratedSecurity = false;
                reportOption.ReportModel.DataSourceCredentials = new List<DataSourceCredentials>
                        {
                                DataSourceCredentials
                        };
            }
        }
        internal class FileDataModel
        {
            public string DataMode { get; set; }
            public string Data { get; set; }
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
            if (jsonArray != null)

            {

                if (jsonArray.ContainsKey("customData"))

                {

                    reportData = jsonArray["customData"].ToString();

                }

            }
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }
    }

}