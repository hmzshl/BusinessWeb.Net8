using BusinessWeb.Models.DB;
using BusinessWeb.Models.LT;

namespace BusinessWeb.Models
{
    public class SageData
    {
        public API_LT_DOCENTETE DocEntete { get; set; }

        public List<API_LT_DOCLIGNE> DocLignes { get; set; }

        public int TransferTo { get; set; }
    }
}