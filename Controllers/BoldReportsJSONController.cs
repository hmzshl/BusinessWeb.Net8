using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using BusinessWeb.Pages.Traitement.Outils.Tracabilite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Json.Nodes;

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
        private List<BoldReports.Web.ReportParameter> _parameters = new List<BoldReports.Web.ReportParameter>();
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
            var reportParameters = ReportHelper.GetParameters(jsonResult, this, _cache);
            if (reportParameters != null)
            {
                foreach (var rptParameter in reportParameters)
                {
                    _parameters.Add(new BoldReports.Web.ReportParameter()
                    {
                        Name = rptParameter.Name,
                        Values = rptParameter.Values.ToList()
                    });
                }
            }

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
        public object SendEmail([FromBody] Dictionary<string, object> jsonResult)
        {
            string _token = jsonResult["reportViewerToken"].ToString();
            var stream = ReportHelper.GetReport(_token, jsonResult["exportType"].ToString(), this, _cache);
            stream.Position = 0;

            if (!ComposeEmail(stream, jsonResult["ReportName"].ToString()))
            {
                return "Mail not sent !!!";
            }

            return "Mail Sent !!!";
        }
        public bool ComposeEmail(Stream stream, string reportName)
        {
            try
            {
                var dt = _parameters;
                if(dt.Count() != 0)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.privateemail.com");     // Change the Smtp server based on your mail
                    mail.IsBodyHtml = true;
                    mail.From = new MailAddress("info@aica.ma");                   //add the sender mail id
                    mail.To.Add(dt?.First()?.Values?.First());                                 //add the receiver mail id
                    mail.Subject = "Report Name : " + reportName;
                    stream.Position = 0;

                    if (stream != null)
                    {
                        ContentType ct = new ContentType();
                        ct.Name = reportName + DateTime.Now.ToString() + ".pdf";
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(stream, ct);
                        mail.Attachments.Add(attachment);
                    }

                    SmtpServer.Port = 587;                                                                                //change the Port number based on your mail
                    SmtpServer.Credentials = new System.Net.NetworkCredential("info@aica.ma", "Aicha2000"); //give the sender mail id and it application password
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                    SmtpServer.UseDefaultCredentials = true;
                }



                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
    }

}