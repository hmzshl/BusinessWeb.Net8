using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessWeb.Data
{
    public class BoldReportViewerOptions
    {
        public string ReportName { get; set; }
        public string ServiceURL { get; set; }
        public string Data { get; set; }
        public List<JSReportParameter> Parameters { get; set; } = new List<JSReportParameter>();
    }
    public class BoldReportDesignerOptions
    {
        public string ServiceURL { get; set; }
    }
    public class JSReportParameter
    {
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();
    }
}