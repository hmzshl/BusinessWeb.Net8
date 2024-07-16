using BusinessWeb.Models.BusinessWebDB;
using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using LogicNP.CryptoLicensing;
namespace BusinessWeb
{
    public class SessionDT
    {
        public AspNetUser User { get; set; } = new AspNetUser();
        public List<TSociete> Societes { get; set; } = new List<TSociete>();
        public TSociete Societe { get; set; } = new TSociete();
        public List<API_T_Site> Sites { get; set; } = new List<API_T_Site>();
        public API_T_Site Site { get; set; } = new API_T_Site();
        public DB db
        {
            get
            {
                return (new Helpers()).getDb(this.Societe);
            }

            set
            {

            }
        }
        public bool IsMobile { get; set; }
        public bool IsConnectionWorking()
        {
            try
            {
                return this.db.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }
        public int SelectedAPP { get; set; }
        public CryptoLicense license { get; set; }
        public List<TAuthorize> Authorizes { get; set; }
		public string zoomLevel { get; set; } = "1";
        public double scaleLevel { get; set; } = 1;
        public string gridHeight { get; set; } = "100%";

    }
}
