using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
//File Manager's base functions are available in the below namespace.
using Syncfusion.EJ2.FileManager.Base;
//File Manager's operations are available in the below namespace.
using Syncfusion.EJ2.FileManager.PhysicalFileProvider;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Syncfusion.Blazor.FileManager;
using PhysicalFileProvider = Syncfusion.EJ2.FileManager.PhysicalFileProvider.PhysicalFileProvider;
using FileManagerDirectoryContent = Syncfusion.EJ2.FileManager.Base.FileManagerDirectoryContent;


namespace BusinessWeb.Controllers
{
    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class FilesController : Controller
    {

        public string basePath;
        string root = "wwwroot\\data";
        [Obsolete]
        public FilesController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this.basePath = hostingEnvironment.ContentRootPath;
            /*operation = new PhysicalFileProvider();
            operation.RootFolder(this.basePath + "\\" + this.root); // It denotes in which files and folders are */

        }

        // Processing the File Manager operations.
        [Route("FileOperations/{societe}/{table}/{id}")]
        //
        public object FileOperations([FromBody] FileManagerDirectoryContent args,string societe, string table, string id)
        {
            PhysicalFileProvider operation = new PhysicalFileProvider();

            string url = this.basePath + this.root + "\\" + Int16.Parse(societe).ToString("000000000000") + "\\" + table + "\\" + Int16.Parse(id).ToString("000000000000") + "\\Files";
            System.IO.Directory.CreateDirectory(url);
            operation.RootFolder(url);
            if (args.Action == "delete" || args.Action == "rename")
            {
                if ((args.TargetPath == null) && (args.Path == ""))
                {
                    FileManagerResponse response = new FileManagerResponse();
                    response.Error = new ErrorDetails { Code = "401", Message = "Restricted to modify the root folder." };
                    return operation.ToCamelCase(response);
                }
            }
            if (table == "Projet")
            {
                System.IO.Directory.CreateDirectory(url + "\\Appel d'offre");
                System.IO.Directory.CreateDirectory(url + "\\Bordereau des prix");
                System.IO.Directory.CreateDirectory(url + "\\Assurances");
                System.IO.Directory.CreateDirectory(url + "\\Retenue de garantie");
            }
            switch (args.Action)
            {
                // Add your custom action here.
                case "read":
                    // Path - Current path; ShowHiddenItems - Boolean value to show/hide hidden items.
                    return operation.ToCamelCase(operation.GetFiles(args.Path, args.ShowHiddenItems));
                case "delete":
                    // Path - Current path where the folder to be deleted; Names - Name of the files to be deleted
                    return operation.ToCamelCase(operation.Delete(args.Path, args.Names));
                case "copy":
                    //  Path - Path from where the file was copied; TargetPath - Path where the file/folder is to be copied; RenameFiles - Files with same name in the copied location that is confirmed for renaming; TargetData - Data of the copied file
                    return operation.ToCamelCase(operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));
                case "move":
                    // Path - Path from where the file was cut; TargetPath - Path where the file/folder is to be moved; RenameFiles - Files with same name in the moved location that is confirmed for renaming; TargetData - Data of the moved file
                    return operation.ToCamelCase(operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));
                case "details":
                    // Path - Current path where details of file/folder is requested; Name - Names of the requested folders
                    return operation.ToCamelCase(operation.Details(args.Path, args.Names));
                case "create":
                    // Path - Current path where the folder is to be created; Name - Name of the new folder
                    return operation.ToCamelCase(operation.Create(args.Path, args.Name));
                case "search":
                    // Path - Current path where the search is performed; SearchString - String typed in the searchbox; CaseSensitive - Boolean value which specifies whether the search must be casesensitive
                    return operation.ToCamelCase(operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive));
                case "rename":
                    // Path - Current path of the renamed file; Name - Old file name; NewName - New file name
                    return operation.ToCamelCase(operation.Rename(args.Path, args.Name, args.NewName));
            }
            return null;
        }
        [Route("Download/{societe}/{table}/{id}")]
        public IActionResult Download(string downloadInput, string societe, string table, string id)
        {
            PhysicalFileProvider operation = new PhysicalFileProvider();
            string url = this.basePath + this.root + "\\" + Int16.Parse(societe).ToString("000000000000") + "\\" + table + "\\" + Int16.Parse(id).ToString("000000000000") + "\\Files";
            System.IO.Directory.CreateDirectory(url);
            operation.RootFolder(url);
            //Invoking download operation with the required parameters.
            // path - Current path where the file is downloaded; Names - Files to be downloaded;
            FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
            return operation.Download(args.Path, args.Names);
        }

        [Route("Upload/{societe}/{table}/{id}")]
        public IActionResult Upload(string path, IList<IFormFile> uploadFiles, string action, string societe, string table, string id)
        {
            PhysicalFileProvider operation = new PhysicalFileProvider();
            string url = this.basePath + this.root + "\\" + Int16.Parse(societe).ToString("000000000000") + "\\" + table + "\\" + Int16.Parse(id).ToString("000000000000") + "\\Files";
            System.IO.Directory.CreateDirectory(url);
            operation.RootFolder(url);
            //Invoking upload operation with the required parameters.
            // path - Current path where the file is to uploaded; uploadFiles - Files to be uploaded; action - name of the operation(upload)
            FileManagerResponse uploadResponse;
            uploadResponse = operation.Upload(path, uploadFiles, action, null);
            if (uploadResponse.Error != null)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = Convert.ToInt32(uploadResponse.Error.Code);
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = uploadResponse.Error.Message;
            }
            return Content("");
        }
        // Processing the GetImage operation.
        [Route("GetImage/{societe}/{table}/{id}")]
        public IActionResult GetImage(FileManagerDirectoryContent args, string societe, string table, string id)
        {
            PhysicalFileProvider operation = new PhysicalFileProvider();
            string url = this.basePath + this.root + "\\" + Int16.Parse(societe).ToString("000000000000") + "\\" + table + "\\" + Int16.Parse(id).ToString("000000000000") + "\\Files";
            System.IO.Directory.CreateDirectory(url);
            operation.RootFolder(url);
            //Invoking GetImage operation with the required parameters.
            // path - Current path of the image file; Id - Image file id;
            return operation.GetImage(args.Path, args.Id, false, null, null);
        }
    }
}