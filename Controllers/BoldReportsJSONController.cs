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
using BusinessWeb.Models.Perso;
using BoldReports.Writer;
using BoldReports.RDL.DOM;
namespace BusinessWeb.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
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
		public object SendEmail([FromBody] Dictionary<string, object> jsonResult)
		{
			string _token = jsonResult["reportViewerToken"].ToString();
			var stream = ReportHelper.GetReport(_token, jsonResult["exportType"].ToString(), this, _cache);
			stream.Position = 0;

			EmailConfiguration emailConfig = new EmailConfiguration
			{
				SmtpServer = jsonResult["smtpServer"].ToString(),
				Port = jsonResult["port"].ToString(),
				SenderEmail = jsonResult["senderEmail"].ToString(),
				Password = jsonResult["password"].ToString(),
				RecipientEmail = jsonResult["recipientEmail"].ToString(),
                Objet = jsonResult["objet"].ToString(),
                FileName = jsonResult["fileName"].ToString()
            };

			if (!ComposeEmail(stream, jsonResult["ReportName"].ToString(), emailConfig))
			{
				return new { success = false, message = "E-mail non envoyé !!!" };
			}

			return new { success = true, message = "E-mail envoyé avec succès !!!" };
		}
		public bool ComposeEmail(Stream stream, string reportName, EmailConfiguration emailConfig)
		{
			try
			{
				MailMessage mail = new MailMessage
				{
					IsBodyHtml = true,
					From = new MailAddress(emailConfig.SenderEmail),
					Subject = emailConfig.Objet
				};
				mail.To.Add(emailConfig.RecipientEmail);
				mail.Body = @"<p>Madame, Monsieur,</p>
              <p>La vérification de votre compte nous permet de vous présenter, sauf erreur ou omission de notre part, un relevé général des factures émises à ce jour et dont le règlement ne nous est toujours pas parvenu.</p>
              <p>Nous vous serions reconnaissants de bien vouloir procéder à l'envoi de votre règlement dans les meilleurs délais.</p>
              <p>Cordialement</p>
              <p>SAS Global Emballages<br />
              Service facturation<br />
              Tel : 0134701025</p>";
				stream.Position = 0;
				if (stream != null)
				{
					ContentType ct = new ContentType
					{
						Name = emailConfig.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf"
					};
					System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(stream, ct);
					mail.Attachments.Add(attachment);
				}

				SmtpClient SmtpServer = new SmtpClient(emailConfig.SmtpServer)
				{
					Port = Int16.Parse(emailConfig.Port),
					Credentials = new System.Net.NetworkCredential(emailConfig.SenderEmail, emailConfig.Password),
					EnableSsl = true
				};

				SmtpServer.Send(mail);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public class ExportRequest
        {
            public string ReportName { get; set; }
            public string Data { get; set; }
        }
        public IActionResult Export([FromBody] Dictionary<string, object> jsonResult)
        {
            string reportName = jsonResult["reportName"].ToString();
            string basePath = _hostingEnvironment.WebRootPath;
            //reportOption.ReportModel.ProcessingMode = BoldReports.Web.ReportViewer.ProcessingMode.Local;
            // Here, we have loaded the sales-order-detail.rdl report from application the folder wwwroot\Resources. sales-order-detail.rdl should be there in wwwroot\Resources application folder.
            FileStream inputStream = new FileStream(basePath + @"\Resources\" + reportName + ".rdl", FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();

            string fileName = null;
            WriterFormat format;
            string type = null;

            fileName = "sales-order-detail.pdf";
            type = "pdf";
            format = WriterFormat.PDF;

            writer.LoadReport(reportStream);
            MemoryStream memoryStream = new MemoryStream();
            writer.Save(memoryStream, format);

            // Download the generated export document to the client side.
            memoryStream.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/" + type);
            fileStreamResult.FileDownloadName = fileName;
            return fileStreamResult;
        }

    }

}