using BoldReports.RDL.Data;
using BoldReports.ServerProcessor;
using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using BoldReports.Writer;
using BusinessWeb;
using BusinessWeb.Controllers;
using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Perso;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.JSInterop;
using Microsoft.SqlServer.ReportingServices2010;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using DataSourceCredentials = BoldReports.Web.DataSourceCredentials;

public class ReportService
{
    BusinessWebDBContext _sdb;
    private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _config;


    public ReportService(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IConfiguration config, BusinessWebDBContext sdb)
    {
        _hostingEnvironment = hostingEnvironment;
        _config = config;
        _sdb = sdb;
    }

    public async Task<MemoryStream> GeneratePdfReport(ReportDataRequest request)
    {
        FileStream reportStream = new FileStream(_hostingEnvironment.WebRootPath + request.ReportPath, FileMode.Open, FileAccess.Read);
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


        if (request.IsQuery)
        {

            var userParameters = new List<BoldReports.Web.ReportParameter>();
            if (request.Parameters != null)
            {
                foreach (var param in request.Parameters)
                {
                    if (param.TryGetValue("Name", out var nameObj) && param.TryGetValue("Values", out var valuesObj))
                    {
                        var name = nameObj.ToString();
                        var values = valuesObj is JsonElement jsonValues ? jsonValues.Deserialize<List<string>>() : new List<string> { valuesObj.ToString() };

                        userParameters.Add(new BoldReports.Web.ReportParameter
                        {
                            Name = name,
                            Values = values
                        });
                    }
                }
            }
            writer.SetParameters(userParameters);

            Helpers fn = new Helpers();
            DataSourceCredentials dataSourceCredentials = new DataSourceCredentials();
            dataSourceCredentials.Name = "DataSource1";
            dataSourceCredentials.ConnectionString = fn.getDb(_sdb.TSocietes.Where(a => a.id == request.Societe).SingleOrDefault())?.Database?.GetConnectionString();
            writer.SetDataSourceCredentials(new List<DataSourceCredentials> { dataSourceCredentials });
        }
        else
        {
            FileDataModel model = new FileDataModel();
            model.DataMode = "inline";
            model.Data = request.Data;

            DataSourceCredentials DataSourceCredentials = new DataSourceCredentials();
            DataSourceCredentials.Name = "DataSource1";
            DataSourceCredentials.UserId = null;
            DataSourceCredentials.Password = null;
            DataSourceCredentials.ConnectionString = JsonConvert.SerializeObject(model);
            DataSourceCredentials.IntegratedSecurity = false;
            writer.SetDataSourceCredentials(new List<DataSourceCredentials> { DataSourceCredentials });

        }
        MemoryStream memoryStream = new MemoryStream();
        writer.Save(memoryStream, WriterFormat.PDF);
        memoryStream.Position = 0;
        return memoryStream;
    }
    internal class FileDataModel
    {
        public string DataMode { get; set; }
        public string Data { get; set; }
    }
}
