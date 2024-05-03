using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BusinessWeb
{

    public class BusinessWebService
    {
        private IWebHostEnvironment _hostEnvironment;
        public BusinessWebService(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

        public string GetPath(string filename)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath, filename);
            return path;
        }
        public string GetRoot()
        {
            return _hostEnvironment.WebRootPath;
        }
    }
}