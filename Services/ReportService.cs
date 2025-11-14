using BoldReports.ServerProcessor;
using BoldReports.Web;
using BoldReports.Writer;
using BusinessWeb.Controllers;
using iTextSharp.text.pdf.qrcode;
using BusinessWeb.Controllers;
using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

public class ReportService
{

	private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;
	private readonly IConfiguration _config;

	public ReportService(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IConfiguration config)
	{
		_hostingEnvironment = hostingEnvironment;
		_config = config;
	}

	public async Task<MemoryStream> GeneratePdfReport(string reportPath, List<ReportParameter> userParameters = null)
	{
		//FileStream reportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\resources\reports\documents\BonCommande.rdl", FileMode.Open, FileAccess.Read);
		FileStream reportStream = new FileStream(_hostingEnvironment.WebRootPath + reportPath, FileMode.Open, FileAccess.Read);
		BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();
		writer.ReportProcessingMode = BoldReports.Writer.ProcessingMode.Remote;
		writer.LoadReport(reportStream);
		writer.ExportResources.Scripts = new List<string>
				{
					_hostingEnvironment.WebRootPath  + @"/js/bold.reports.common.min.js",
					_hostingEnvironment.WebRootPath  + @"/js/bold.reports.widgets.min.js",
                    //Report Viewer Script
                    _hostingEnvironment.WebRootPath  + @"/js/bold.report-viewer.min.js"
				};

		writer.ExportResources.DependentScripts = new List<string>
				{
					_hostingEnvironment.WebRootPath  + @"/js/jquery-1.10.2.min.js"
				};

		if (writer.FontSettings == null)
		{
			writer.FontSettings = new BoldReports.RDL.Data.FontSettings();
		}
		writer.FontSettings.BasePath = Path.Combine(_hostingEnvironment.WebRootPath, "fonts");
		writer.SetParameters(userParameters);
		
		foreach (var ds in writer.GetDataSources())
		{
			DataSourceCredentials dataSourceCredentials = new DataSourceCredentials();
			dataSourceCredentials.Name = ds.Name;
			dataSourceCredentials.ConnectionString = _config["Breport:CS"];
			writer.SetDataSourceCredentials(new List<DataSourceCredentials> { dataSourceCredentials });
		}
		MemoryStream memoryStream = new MemoryStream();
		writer.Save(memoryStream, WriterFormat.PDF);
		memoryStream.Position = 0;
		return memoryStream;
		//return new Attachment(memoryStream, $"Bon de commande N {doPiece}.pdf", MediaTypeNames.Application.Pdf);
	}

	public async Task<Attachment> GetPdfReport(string fileName, string reportPath, List<ReportParameter> userParameters = null)
	{
		//string fname = $"Bon de commande N {doPiece}.pdf";
		Stream memoryStream = await GeneratePdfReport(reportPath, userParameters);
		return new Attachment(memoryStream, fileName, MediaTypeNames.Application.Pdf);
	}

}
