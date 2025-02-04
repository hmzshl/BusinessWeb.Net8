using Objets100cLib;

namespace BusinessWeb
{
	public class SageOM
	{
		private string CompanyServer { get; set; }
		private string CompanyDatabaseName { get; set; }

		// Constructor to initialize CompanyServer and CompanyDatabaseName
		public SageOM(string companyServer, string companyDatabaseName)
		{
			CompanyServer = companyServer;
			CompanyDatabaseName = companyDatabaseName;
		}
		public bool OuvreBaseCpta(ref BSCPTAApplication100c BaseCpta)
		{
			try
			{
				BaseCpta.CompanyServer = CompanyServer;
				BaseCpta.CompanyDatabaseName = CompanyDatabaseName;
				BaseCpta.Open();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur en ouverture de base comptable : {ex.Message}");
				return false;
			}
		}
		public bool FermeBaseCpta(ref BSCPTAApplication100c BaseCpta)
		{
			try
			{
				BaseCpta.Close();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur en fermeture de base comptable : {ex.Message}");
				return false;
			}
		}
		public bool OuvreBaseCial(ref BSCIALApplication100c BaseCial)
		{
			try
			{
				BaseCial.CompanyServer = CompanyServer;
				BaseCial.CompanyDatabaseName = CompanyDatabaseName;
				BaseCial.Open();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur en ouverture de base comptable : {ex.Message}");
				return false;
			}
		}
		public bool FermeBaseCial(ref BSCIALApplication100c BaseCial)
		{
			try
			{
				BaseCial.Close();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur en fermeture de base comptable : {ex.Message}");
				return false;
			}
		}
	}


}
