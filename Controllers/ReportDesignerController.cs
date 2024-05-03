using BoldReports.Web.ReportDesigner;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ReportDesignerController : ControllerBase, IReportDesignerController
    {
        private IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;

        public ReportDesignerController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }
        [NonAction]
        public ResourceInfo GetData(string key, string itemId)
        {
            var resource = new ResourceInfo();
            try
            {
                var filePath = this.GetFilePath(itemId, key);
                if (itemId.Equals(Path.GetFileName(filePath), StringComparison.InvariantCultureIgnoreCase) && System.IO.File.Exists(filePath))
                {
                    resource.Data = System.IO.File.ReadAllBytes(filePath);
                }
                else
                {
                    resource.ErrorMessage = "File not found from the specified path";
                }
            }
            catch (Exception ex)
            {
                resource.ErrorMessage = ex.Message;
            }
            return resource;
        }

        public object GetImage(string key, string image)
        {
            return ReportDesignerHelper.GetImage(key, image, this);
        }

        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            //You can update report options here
        }

        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            //You can update report options here
        }

        [HttpPost]
        public object PostDesignerAction([FromBody] Dictionary<string, object> jsonResult)
        {
            return ReportDesignerHelper.ProcessDesigner(jsonResult, this, null, this._cache);
        }

        public object PostFormDesignerAction()
        {
            return ReportDesignerHelper.ProcessDesigner(null, this, null, this._cache);
        }

        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, this._cache);
        }

        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this, this._cache);
        }

        [NonAction]
        bool IReportDesignerController.SetData(string key, string itemId, ItemInfo itemData, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (itemData.Data != null)
            {
                System.IO.File.WriteAllBytes(this.GetFilePath(itemId, key), itemData.Data);
            }
            else if (itemData.PostedFile != null)
            {
                var fileName = itemId;
                if (string.IsNullOrEmpty(itemId))
                {
                    fileName = Path.GetFileName(itemData.PostedFile.FileName);
                }

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    itemData.PostedFile.OpenReadStream().CopyTo(stream);
                    byte[] bytes = stream.ToArray();
                    var writePath = this.GetFilePath(fileName, key);

                    System.IO.File.WriteAllBytes(writePath, bytes);
                    stream.Close();
                    stream.Dispose();
                }
            }
            return true;
        }

        [HttpPost]
        public void UploadReportAction()
        {
            ReportDesignerHelper.ProcessDesigner(null, this, this.Request.Form.Files[0], this._cache);
        }

        [NonAction]
        private string GetFilePath(string itemName, string key)
        {
            string dirPath = Path.Combine(this._hostingEnvironment.WebRootPath + "\\" + "Cache", key);

            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }

            return Path.Combine(dirPath, itemName);
        }
    }
}