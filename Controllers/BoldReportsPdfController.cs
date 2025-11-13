using BoldReports.ServerProcessor;
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

		// Pdf1 : JSON Data 
		// {
		//   "reportPath": "/resources/reports/documents/BonCommande.rdl",
		//   "parameters": [
		//     { "Name": "DO_Piece", "Values": ["25BC02345"] }
		//   ]
		// }

		[HttpPost("pdf1")]
		public async Task<IActionResult> ReportsPdf1([FromBody] DataRequest request)
		{
			if (string.IsNullOrEmpty(request.ReportPath))
				return BadRequest("Le chemin du rapport est requis.");

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

			var stream = await _reportService.GeneratePdfReport(request.ReportPath, userParameters);

			// Byte[]
			//return Ok(new { BytePdf = stream.ToArray() });

			// Base64
			//string base64 = Convert.ToBase64String(stream.ToArray());
			//return Ok(new { Base64Pdf = base64 });

			// Preview Pdf
			return File(stream.ToArray(), "application/pdf");

			// Download Pdf
			//return File(mergedPdfStream.ToArray(), "application/pdf", "MergedReport.pdf");
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

	public class DataRequest
	{
		public string ReportPath { get; set; }

		//public List<Dictionary<string, object>> Query { get; set; }

		public List<string> Ids { get; set; }

		public List<Dictionary<string, object>> Parameters { get; set; }

	}

}

