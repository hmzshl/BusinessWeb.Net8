using Microsoft.Extensions.Configuration;
using Objets100cLib;
using Radzen;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace BusinessWeb
{
	public class SageOM
	{
		private string CompanyServer { get; set; }
		private string CompanyDatabaseName { get; set; }
		private string UserName { get; set; }
		private string Password { get; set; }

		// Constructor to initialize from configuration
		public SageOM()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory()) // Ensure it finds appsettings.json
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			CompanyServer = configuration["SageDB:CompanyServer"];
			CompanyDatabaseName = configuration["SageDB:CompanyDatabaseName"];
			UserName = configuration["SageDB:UserName"];
			Password = configuration["SageDB:Password"];
		}

		public bool OuvreBaseCpta(ref BSCPTAApplication100c BaseCpta)
		{
			try
			{
				BaseCpta.CompanyServer = CompanyServer;
				BaseCpta.CompanyDatabaseName = CompanyDatabaseName;
				BaseCpta.Loggable.UserName = UserName;
				BaseCpta.Loggable.UserPwd = Password;
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
				BaseCial.Loggable.UserName = UserName;
				BaseCial.Loggable.UserPwd = Password;
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
		public string NextClient()
		{
			string rs = "CLT00001";
			if(this.CPTA().FactoryClient.List.Count > 0)
			{
				rs = this.NextString(this.CPTA().FactoryClient.List.Cast<IBOClient3>().OrderByDescending(a => a.CT_Num).First().CT_Num);
			}
			return rs;
		}
		public string NextFournisseur()
		{
			string rs = "FRS00001";
			if (this.CPTA().FactoryFournisseur.List.Count > 0)
			{
				rs = this.NextString(this.CPTA().FactoryFournisseur.List.Cast<IBOFournisseur3>().OrderByDescending(a => a.CT_Num).First().CT_Num);
			}
			return rs;
		}
		public string NextAffaire()
		{
			string rs = "PRJ00001";
			if (this.CPTA().FactoryCompteA.List.Count > 0)
			{
				rs = this.NextString(this.CPTA().FactoryCompteA.List.Cast<IBOCompteA3>().Where(a => a.Analytique.A_Intitule == "Affaires").OrderByDescending(a => a.CA_Num).First().CA_Num);
			}
			return rs;
		}
		public BSCPTAApplication100c CPTA()
		{
			BSCPTAApplication100c BaseCpta = new BSCPTAApplication100c();
			bool ok = this.OuvreBaseCpta(ref BaseCpta);
			try
			{
				if (ok)
				{
					return BaseCpta;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return BaseCpta;
		}
		public BSCIALApplication100c CIAL()
		{
			BSCIALApplication100c BaseCial = new BSCIALApplication100c();
			bool ok = this.OuvreBaseCial(ref BaseCial);
			try
			{
				if (ok)
				{
					return BaseCial;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return BaseCial;
		}
		public string NextString(string input)
		{
			// Regular expression to match a numeric part at the end of the string
			var regex = new Regex(@"^(.*?)(\d*)$");
			var match = regex.Match(input);

			if (match.Success)
			{
				// Extract the non-numeric prefix and the numeric part
				string prefix = match.Groups[1].Value;
				string numericPart = match.Groups[2].Value;

				if (string.IsNullOrEmpty(numericPart))
				{
					// If the string doesn't end with a numeric part, append "1"
					return prefix + "1";
				}
				else
				{
					// Convert the numeric part to an integer and increment it
					int number = int.Parse(numericPart) + 1;

					// Replace the old numeric part with the incremented one
					string incrementedPart = number.ToString().PadLeft(numericPart.Length, '0');
					return prefix + incrementedPart;
				}
			}
			else
			{
				// If the string doesn't match the pattern, return the original string
				return input;
			}
		}
	}
}
