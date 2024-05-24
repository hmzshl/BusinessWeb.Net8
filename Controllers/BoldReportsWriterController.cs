using BoldReports.Writer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/{controller}/{action}/{id?}")]
    public class BoldReportsWriterController : ControllerBase
    {
        // IWebHostEnvironment used with sample to get the applicationdata from wwwroot.
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        // IWebHostEnvironment initialized with controller to get the data from application data folder.
        public BoldReportsWriterController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Obsolete]
        public IActionResult Export(string writerFormat)
        {
            // Here, we have loaded the sales-order-detail sample report from application the folder wwwroot\Resources.
            FileStream inputStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\sales-order-detail.rdl", FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();

            string fileName = null;
            WriterFormat format;
            string type = null;

            if (writerFormat == "PDF")
            {
                fileName = "sales-order-detail.pdf";
                type = "pdf";
                format = WriterFormat.PDF;
            }
            else if (writerFormat == "Word")
            {
                fileName = "sales-order-detail.doc";
                type = "doc";
                format = WriterFormat.Word;
            }
            else if (writerFormat == "CSV")
            {
                fileName = "sales-order-detail.csv";
                type = "csv";
                format = WriterFormat.CSV;
            }
            else
            {
                fileName = "sales-order-detail.xls";
                type = "xls";
                format = WriterFormat.Excel;
            }

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