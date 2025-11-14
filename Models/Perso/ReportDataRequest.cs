using BoldReports.Shared.Serializer;

namespace BusinessWeb.Models.Perso
{
    public class ReportDataRequest
    {
        public string ReportPath { get; set; }
        public List<string> Ids { get; set; }

        public List<Dictionary<string, object>> Parameters { get; set; }
        public int Societe { get; set; }
        public bool IsQuery { get; set; }
        public string Data { get; set; }
        public string Return { get; set; }
        public string DownloadFileName { get; set; } = "report.pdf";
        }
}
