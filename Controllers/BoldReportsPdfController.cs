using BoldReports.ServerProcessor;
using BusinessWeb.Models.Perso;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Json;

namespace BusinessWeb.Controllers
{
    [ApiController]
    [Route("api/reports/")]
    public class BoldReportsPdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ReportService _reportService;

        public BoldReportsPdfController(ReportService reportService)
        {
            _reportService = reportService;
        }

        /*
        {
		   "reportPath": "/resources/reports/personnels/OrdreFabrication.rdl",
		   "parameters": [
		     { "Name": "id", "Values": ["23"] }
		   ],
        "Societe": 4,
        "IsQuery": true,
                "Data": "",
              "Return": "File",
              "DownloadFileName": "Releve de compte 14-11-2025.pdf"
		 }
		 */

        [HttpPost("pdf1")]
        public async Task<IActionResult> ReportsPdf1([FromBody] ReportDataRequest request)
        {
            if (string.IsNullOrEmpty(request.ReportPath))
                return BadRequest("Le chemin du rapport est requis.");
            var stream = await _reportService.GeneratePdfReport(request);


            if(request.Return == "File")
            {
                // Download Pdf
                return File(stream.ToArray(), "application/pdf", request.DownloadFileName);
            } 
            else if (request.Return == "Base64")
            {
                // Base64
                string base64 = Convert.ToBase64String(stream.ToArray());
                return Ok(new { Base64Pdf = base64 });
            }
            else if (request.Return == "Byte")
            {
                // Byte[]
                return Ok(new { BytePdf = stream.ToArray() });
            }
            else
            {
                // Preview
                return File(stream.ToArray(), "application/pdf");
            }
        }

    }

    public static class PdfHelper
    {

        public static MemoryStream MergePdfStreams(List<Stream> pdfStreams)
        {
            var outputStream = new MemoryStream();
            var safeStream = new NonClosingStream(outputStream); // prevents closure

            var document = new iTextSharp.text.Document();
            var pdfCopy = new PdfCopy(document, safeStream);

            document.Open();

            foreach (var stream in pdfStreams)
            {
                stream.Position = 0;
                using (var reader = new PdfReader(stream))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = pdfCopy.GetImportedPage(reader, i);
                        pdfCopy.AddPage(page);
                    }
                }
            }

            document.Close(); // closes PdfCopy, but NOT our outputStream

            outputStream.Position = 0;

            return outputStream;
        }

    }

    public class NonClosingStream : Stream
    {
        private readonly Stream _innerStream;

        public NonClosingStream(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public override bool CanRead => _innerStream.CanRead;
        public override bool CanSeek => _innerStream.CanSeek;
        public override bool CanWrite => _innerStream.CanWrite;
        public override long Length => _innerStream.Length;
        public override long Position { get => _innerStream.Position; set => _innerStream.Position = value; }

        public override void Flush() => _innerStream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);
        public override void SetLength(long value) => _innerStream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

        // 👇 Prevent underlying stream from being closed
        public override void Close() { /* Do nothing */ }
        protected override void Dispose(bool disposing) { /* Do nothing */ }
    }

}

