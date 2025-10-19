using AntDesign;
using BusinessWeb.Data;
using BusinessWeb.Models;
using BusinessWeb.Models.BusinessWebDB;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Perso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessWeb
{
	public class Helpers
	{
		public Syncfusion.Blazor.Data.Query LocalDataQuery = new Syncfusion.Blazor.Data.Query().Take(10);
		public NumericEditCellParams parameters = new NumericEditCellParams() { Params = new NumericTextBoxModel<object>() { Decimals = 2, Format = "### ### ##0.00;-### ### ##0.00;#" } };
		public ExcelExportProperties ExportToExcelAsync()
		{
			ExcelExportProperties rs = new ExcelExportProperties();
			rs.IncludeTemplateColumn = true;
			rs.IncludeHeaderRow = true;

			return rs;
		}
        public string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input, "<.*?>", string.Empty).Trim();
        }

        public bool PieceExist(int DO_Type, DB db, string DO_Piece)
		{
			bool rs = false;
			var dt1 = db.F_DOCLIGNE.Where(a => a.DO_Type == 0 && a.DO_Piece == DO_Piece).Select(a => a.DO_Piece);
			var dt2 = db.F_DOCLIGNE.Where(a => a.DO_Type != 0 && a.DL_PieceDE == DO_Piece).Select(a => a.DL_PieceDE);
			var dt3 = db.F_DOCENTETE.Where(a => a.DO_Type == 0 && a.DO_Piece == DO_Piece).Select(a => a.DO_Piece);
			var dt = dt1.Concat(dt2).Concat(dt3);

			if (dt.Any())
			{
				if (dt.Contains(DO_Piece))
				{
					rs = true;
				}
			}

			return rs;
		}
		public string getPiece(int DO_Type, DB db)
		{
			string rs = "DE00001";
			if (DO_Type == 0)
			{
				var dt1 = db.F_DOCLIGNE.Where(a => a.DO_Type == 0).Select(a => a.DO_Piece);
				var dt2 = db.F_DOCLIGNE.Where(a => a.DO_Type != 0).Select(a => a.DL_PieceDE);
				var dt3 = db.F_DOCENTETE.Where(a => a.DO_Type == 0).Select(a => a.DO_Piece);
				var dt = dt1.Concat(dt2).Concat(dt3);
				var cur = db.F_DOCCURRENTPIECE.Where(a => a.DC_Souche == 0 && a.DC_Domaine == 0 && a.DC_IdCol == 0).SingleOrDefault();
				if (cur != null)
				{
					if (!dt.Any())
					{
						rs = cur.DC_Piece;
					}
					else
					{
						if (dt.Contains(cur.DC_Piece))
						{
							rs = this.getNextCode(dt.Max());
						}
						else
						{
							rs = cur.DC_Piece;
						}
					}
					//cur.DC_Piece = this.getNextCode(rs);
					//db.SaveChanges();
				}
				else
				{
					if (dt.Any())
					{
						rs = this.getNextCode(dt.Max());
					}
					else
					{
						rs = "DE00001";
					}
				}


			}

			return rs;
		}
		public bool CheckDB(DB dB)
		{
			if(dB.Database.CanConnect())
			{
				return true;
			}
			else
			{
				return false;
            }
        }
        public bool CheckSteDB(TSociete ste)
        {
			bool rs = false;
            try
            {
                // Test the connection
                using (var db = getDbCheck(ste))
                {
                    // Try to execute a simple query to test the connection
                    var testQuery = db.P_DOSSIER.Take(1).Any();

                    // If we reach here, connection is working
                    rs = true;
                }
            }
            catch (Exception)
            {
                // If any exception occurs, connection is not working
                rs = false;
            }
			return rs;
        }
        public string GetFrenchMonth(string monthCode)
		{
			var monthMap = new Dictionary<string, string>
		{
			{ "M01", "Janvier" },
			{ "M02", "Février" },
			{ "M03", "Mars" },
			{ "M04", "Avril" },
			{ "M05", "Mai" },
			{ "M06", "Juin" },
			{ "M07", "Juillet" },
			{ "M08", "Août" },
			{ "M09", "Septembre" },
			{ "M10", "Octobre" },
			{ "M11", "Novembre" },
			{ "M12", "Décembre" }
		};

			if (monthMap.TryGetValue(monthCode, out var frenchMonth))
			{
				return frenchMonth;
			}
			else
			{
				throw new ArgumentException("Invalid month code");
			}
		}
		public List<AuthItems> AuthItems()
		{
			List<AuthItems> list = new List<AuthItems>();

			//ACHATS
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Structure", Description = "Fournisseurs", Url = "fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Structure", Description = "Familles", Url = "familles" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Structure", Description = "Articles", Url = "articles" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Structure", Description = "Dépots", Url = "depots" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Structure", Description = "Collaborateurs", Url = "collaborateurs" });

			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Demande d'achat", Url = "achats/10" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Préparation de commande", Url = "achats/1" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Bon de commande", Url = "achats/12" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Bon de livraison", Url = "achats/13" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Bon de retour", Url = "achats/14" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Facture", Url = "achats/16" });
			list.Add(new AuthItems { SelectedAPP = 1, Title = "Traitement", Description = "Facture comptabilisée", Url = "achats/17" });

			//VENTES
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Structure", Description = "Clients", Url = "clients" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Structure", Description = "Familles", Url = "familles" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Structure", Description = "Articles", Url = "articles" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Structure", Description = "Dépots", Url = "depots" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Structure", Description = "Collaborateurs", Url = "collaborateurs" });

			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Devis", Url = "ventes/0" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Bon de commande", Url = "ventes/1" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Préparation de livraison", Url = "ventes/2" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Bon de livraison", Url = "ventes/3" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Bon de retour", Url = "ventes/4" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Facture", Url = "ventes/6" });
			list.Add(new AuthItems { SelectedAPP = 2, Title = "Traitement", Description = "Facture comptabilisée", Url = "ventes/7" });


			//STOCK
			list.Add(new AuthItems { SelectedAPP = 3, Title = "Structure", Description = "Familles", Url = "familles" });
			list.Add(new AuthItems { SelectedAPP = 3, Title = "Structure", Description = "Articles", Url = "articles" });
			list.Add(new AuthItems { SelectedAPP = 3, Title = "Structure", Description = "Dépots", Url = "depots" });

			list.Add(new AuthItems { SelectedAPP = 3, Title = "Traitement", Description = "Mouvement d'entrée", Url = "stocks/20" });
			list.Add(new AuthItems { SelectedAPP = 3, Title = "Traitement", Description = "Mouvement de sortie", Url = "stocks/21" });
			list.Add(new AuthItems { SelectedAPP = 3, Title = "Traitement", Description = "Mouvement de transfert", Url = "stocks/23" });


			//MATERIELS
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Liste des materiels", Url = "materiels" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Pointage journalier", Url = "pointage-materiel" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Consommation gasoil", Url = "entretien/0" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Assurance", Url = "entretien/1" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Vignette", Url = "entretien/2" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Visite technique", Url = "entretien/3" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Vidange", Url = "entretien/6" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Entretien et réparation", Url = "entretien/7" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Carnet de circulation", Url = "entretien/10" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Carnet du disque", Url = "entretien/11" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Taxe sur tonnage", Url = "entretien/12" });
			list.Add(new AuthItems { SelectedAPP = 5, Title = "Traitement", Description = "Maintenance des extincteurs", Url = "entretien/13" });



			//PERSONNELS
			list.Add(new AuthItems { SelectedAPP = 6, Title = "Traitement", Description = "Liste des personnels", Url = "personnels" });
			//list.Add(new AuthItems { SelectedAPP = 6, Title = "Traitement", Description = "Liste des nomenclatures", Url = "nomenclatures" });
			list.Add(new AuthItems { SelectedAPP = 6, Title = "Traitement", Description = "Ordres de fabrications", Url = "ordres-fabrication" });
			list.Add(new AuthItems { SelectedAPP = 6, Title = "Traitement", Description = "Documents de sorties", Url = "fabrication-sorties" });

			list.Add(new AuthItems { SelectedAPP = 6, Title = "Traitement", Description = "Pointage Personnels", Url = "pointage-personnel" });



			list.Add(new AuthItems { SelectedAPP = 6, Title = "Etats", Description = "Pointage par personnel", Url = "depots" });
			list.Add(new AuthItems { SelectedAPP = 6, Title = "Etats", Description = "Pointage par projet", Url = "" });
			list.Add(new AuthItems { SelectedAPP = 6, Title = "Etats", Description = "Recap pointange par personnel", Url = "" });


			//CAISSE
			list.Add(new AuthItems { SelectedAPP = 7, Title = "Structure", Description = "Caisse", Url = "caisses" });

			list.Add(new AuthItems { SelectedAPP = 7, Title = "Traitement", Description = "Demandes d'alimentation", Url = "caisse/demandes" });
			list.Add(new AuthItems { SelectedAPP = 7, Title = "Traitement", Description = "Alimentations de caisse", Url = "caisse/alimentations" });
			list.Add(new AuthItems { SelectedAPP = 7, Title = "Traitement", Description = "Dépenses de caisse", Url = "caisse/depenses" });
			list.Add(new AuthItems { SelectedAPP = 7, Title = "Traitement", Description = "Movements de caisse", Url = "caisse/movements-caisse" });


			//PROJETS
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Maitres Ouvrage", Url = "pr-clients" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Fournisseurs", Url = "pr-fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Familles", Url = "familles" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Articles", Url = "articles" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Dépots", Url = "depots" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Collaborateurs", Url = "collaborateurs" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Personnels", Url = "personnels" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Materiels", Url = "materiels" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Zones", Url = "sites" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Structure", Description = "Villes", Url = "villes" });


			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Appels d'offre", Url = "appels-offre" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Marchés", Url = "marches" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Bordereaux des prix", Url = "bordereaux-des-prix" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Avancements", Url = "avancements" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Consommations chantiers", Url = "consommations-chantiers" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Consommations gasoil", Url = "entretien/0" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Attachements", Url = "attachements" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Décomptes", Url = "decomptes" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Factures", Url = "factures" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Demandes d'achats", Url = "demandes-achats" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Assurances", Url = "assurances" });
			list.Add(new AuthItems { SelectedAPP = 4, Title = "Traitement", Description = "Cautions", Url = "cautions" });


			//AGENCE VOYAGE
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Structure", Description = "Clients", Url = "av-clients" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Structure", Description = "Fournisseurs", Url = "av-fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Structure", Description = "Devises", Url = "devises" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Structure", Description = "Unités", Url = "unites" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Structure", Description = "Articles", Url = "av-articles" });


			list.Add(new AuthItems { SelectedAPP = 10, Title = "Traitement", Description = "Contrats Fournisseurs", Url = "av-contrats-fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Traitement", Description = "Contrats Clients", Url = "av-contrats-clients" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Traitement", Description = "Bookings", Url = "av-bookings" });
			list.Add(new AuthItems { SelectedAPP = 10, Title = "Traitement", Description = "Tableau de bord", Url = "av-factures" });

			list.Add(new AuthItems { SelectedAPP = 10, Title = "Etats", Description = "Dépots", Url = "av-tableau-bord" });


			//ISC CAISSE
			list.Add(new AuthItems { SelectedAPP = 12, Title = "Structure", Description = "Caisses" });
			list.Add(new AuthItems { SelectedAPP = 12, Title = "Structure", Description = "Bénéficiaires" });
			list.Add(new AuthItems { SelectedAPP = 12, Title = "Structure", Description = "Affectations" });

			list.Add(new AuthItems { SelectedAPP = 12, Title = "Traitement", Description = "Recettes de caisse", Url = "caisse/isc/recettes" });
			list.Add(new AuthItems { SelectedAPP = 12, Title = "Traitement", Description = "Dépenses de caisse", Url = "caisse/isc/depenses" });
			list.Add(new AuthItems { SelectedAPP = 12, Title = "Traitement", Description = "Movements de caisse", Url = "caisse/isc/movements-caisse" });


			//ISC BANQUE
			list.Add(new AuthItems { SelectedAPP = 13, Title = "Traitement", Description = "A mettre en banque", Url = "banque/isc/mettre-en-banque" });
			list.Add(new AuthItems { SelectedAPP = 13, Title = "Traitement", Description = "En banque", Url = "banque/isc/en-banque" });
			list.Add(new AuthItems { SelectedAPP = 13, Title = "Traitement", Description = "Impayés", Url = "banque/isc/impayes" });
			list.Add(new AuthItems { SelectedAPP = 13, Title = "Traitement", Description = "Payés", Url = "banque/isc/payes" });


			//ETATS
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Liste des clients", Url = "clients" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Relevé documents ventes", Url = "releve-ventes" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Balance agée", Url = "et-balanceagee-client" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Réglements clients", Url = "reglements-clients" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Tableau de bord Journalier", Url = "tb-commercial-journalier" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Tableau de bord Mensuel", Url = "tb-commercial-mensuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Tableau de bord Annuel", Url = "tb-commercial-annuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Tableau de bord Global", Url = "tb-commercial-global" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Journée", Url = "et-ca-journee" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Mois", Url = "et-ca-mois" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Client", Url = "et-ca-client" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Article", Url = "et-ca-article" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Représentant", Url = "et-ca-representant" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Famille", Url = "et-ca-famille" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Region", Url = "et-ca-region" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Chiffre d'affaire Par Affaire", Url = "et-ca-affaire" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Comparatif Par Année", Url = "et-vente-annuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Comparatif Par Mois", Url = "et-vente-mensuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Ventes", Description = "Echéances Clients", Url = "et-echeances-clients" });

			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Liste des fournisseurs", Url = "fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Relevé documents achats", Url = "releve-achats" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Balance agée", Url = "et-balanceagee-fournisseur" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Réglements fournisseurs", Url = "reglements-fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Tableau de bord Journalier", Url = "tb-achat-journalier" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Tableau de bord Mensuel", Url = "tb-achat-mensuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Tableau de bord Annuel", Url = "tb-achat-annuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Tableau de bord Global", Url = "tb-achat-global" });

			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Journée", Url = "et-achat-journee" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Mois", Url = "et-achat-mois" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Fournisseur", Url = "et-achat-client" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Article", Url = "et-achat-article" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Acheteur", Url = "et-achat-representant" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Famille", Url = "et-achat-famille" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Achats Par Affaire", Url = "et-achat-affaire" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Comparatif Par Année", Url = "et-achat-annuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Comparatif Par Mois", Url = "et-achat-mensuel" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Achats", Description = "Echéances Fournisseurs", Url = "et-echeances-fournisseurs" });


			list.Add(new AuthItems { SelectedAPP = 14, Title = "Articles", Description = "Liste des articles", Url = "et-articles" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Articles", Description = "CA article", Url = "et-ca-article" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Articles", Description = "Stock global par date", Url = "et-stock-global-article" });

			list.Add(new AuthItems { SelectedAPP = 14, Title = "Collaborateurs", Description = "Liste des collaborateurs", Url = "et-collaborateurs" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Collaborateurs", Description = "CA représentant", Url = "et-ca-representant" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Collaborateurs", Description = "Charges collaborateurs", Url = "et-frais" });


			list.Add(new AuthItems { SelectedAPP = 14, Title = "Projets", Description = "Liste des projets", Url = "et-affaires" });
			list.Add(new AuthItems { SelectedAPP = 14, Title = "Projets", Description = "Avancement travaux" });


			//OUTILS

			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Documents achats", Url = "ot-achats" });
			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Documents ventes", Url = "ot-ventes" });
			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Documents stocks", Url = "ot-stocks" });
			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Réglements clients", Url = "ot-reglements-clients" });
			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Réglements fournisseurs", Url = "ot-reglements-fournisseurs" });
			list.Add(new AuthItems { SelectedAPP = 9, Title = "Outils", Description = "Ecritures comptables", Url = "ot-ecritures" });

			//UTILISATEURS
			list.Add(new AuthItems { SelectedAPP = 18, Title = "Utilisateurs", Description = "Utilisateurs connectés", Url = "utilisateurs-connectes" });
			list.Add(new AuthItems { SelectedAPP = 18, Title = "Utilisateurs", Description = "Historique des connexions", Url = "historique-connexions" });
			list.Add(new AuthItems { SelectedAPP = 18, Title = "Utilisateurs", Description = "Réinitialiser un mot de passe", Url = "reinitialiser-passe" });

			//TRACABILITE
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Entetes documents", Url = "tr-entete" });
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Lignes documents", Url = "tr-ligne" });
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Réglements", Url = "tr-reglement" });
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Clients", Url = "tr-client" });
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Fournisseurs", Url = "tr-fournisseur" });
			list.Add(new AuthItems { SelectedAPP = 19, Title = "Traçabilité", Description = "Ecritures comptables", Url = "tr-ecriture" });

			//CERTIFICATION
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Structure", Description = "Liste des clients", Url = "clients" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Structure", Description = "Liste des articles", Url = "et-articles" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Structure", Description = "Liste des collaborateurs", Url = "et-collaborateurs" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Structure", Description = "Liste des instruments", Url = "et-instruments" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Structure", Description = "Responsables", Url = "cr-responsables" });


			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Bons de livraisons", Url = "cr-bl" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Factures", Url = "cr-fa" });

			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Grille de dialogue", Url = "cr-grille-dialogue" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Ouverture dossier", Url = "cr-ouverture-dossier" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Réceptions instruments", Url = "cr-reception" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Décharges instruments", Url = "cr-decharge" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Ordres de mission", Url = "cr-ordre-mission" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Rapports de mission", Url = "cr-rapport-mission" });
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Suivi dossiers clients", Url = "cr-suivi-dossier" });
			
			list.Add(new AuthItems { SelectedAPP = 15, Title = "Traitement", Description = "Tableau de bord etalonnage", Url = "cr-tb-etalonnage" });


			//SYNCHRONISATION
			list.Add(new AuthItems { SelectedAPP = 16, Title = "Traitement", Description = "Ventes", Url = "imp-ventes" });
			list.Add(new AuthItems { SelectedAPP = 16, Title = "Traitement", Description = "Achats", Url = "imp-achats" });
			list.Add(new AuthItems { SelectedAPP = 16, Title = "Traitement", Description = "Réglements", Url = "imp-regl" });
			list.Add(new AuthItems { SelectedAPP = 16, Title = "Traitement", Description = "Séquentialité Factures", Url = "seq-fa" });
            list.Add(new AuthItems { SelectedAPP = 16, Title = "Traitement", Description = "Solde clients", Url = "solde-clients" });

            //FOOD COST
            list.Add(new AuthItems { SelectedAPP = 20, Title = "Traitement", Description = "Affaires", Url = "fc-affaires" });
            list.Add(new AuthItems { SelectedAPP = 20, Title = "Traitement", Description = "Charges", Url = "fc-charges" });

            return list;
		}
		public string getTiers(int type)
		{
			if (type == 0)
			{
				return "Client";
			}
			else if (type == 1)
			{
				return "Fournisseur";
			}
			else
			{
				return "Autre";
			}
		}
		public void DisableTabSelect(SelectingEventArgs args)
		{
			if (args.IsSwiped)
			{
				args.Cancel = true;
			}
		}
		public string DeleteTrigger(string trigger)
		{
			string q1 = @"IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'" + trigger + @"' AND [type] = 'TR')
                                    BEGIN
                                          DROP TRIGGER [dbo].[" + trigger + @"];
                                    END;";


			return q1;
		}
		public string AddVarchar(string table, string col, string len)
		{
			string q1 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = '" + table + @"'
                                                            AND upper(COLUMN_NAME) = '" + col + @"')
                                                    BEGIN
                                                        ALTER TABLE dbo." + table + @" ADD " + col + @" VARCHAR(" + len + @");
                                                    END
                                                    ";

			return q1;
		}
		public string AddDecimal(string table, string col)
		{
			string q1 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = '" + table + @"'
                                                            AND upper(COLUMN_NAME) = '" + col + @"')
                                                    BEGIN
                                                        ALTER TABLE dbo." + table + @" ADD " + col + @" DECIMAL(24,6) NOT NULL Default 0;
                                                    END
                                                    ";

			return q1;
		}
		public string AddDate(string table, string col)
		{
			string q1 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = '" + table + @"'
                                                            AND upper(COLUMN_NAME) = '" + col + @"')
                                                    BEGIN
                                                        ALTER TABLE dbo." + table + @" ADD " + col + @" SMALLDATETIME;
                                                    END
                                                    ";

			return q1;
		}
		public string AddInt(string table, string col)
		{
			string q1 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = '" + table + @"'
                                                            AND upper(COLUMN_NAME) = '" + col + @"')
                                                    BEGIN
                                                        ALTER TABLE dbo." + table + @" ADD " + col + @" INT NOT NULL Default 0;
                                                    END
                                                    ";

			return q1;
		}
		public string AddBool(string table, string col)
		{
			string q1 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = '" + table + @"'
                                                            AND upper(COLUMN_NAME) = '" + col + @"')
                                                    BEGIN
                                                        ALTER TABLE dbo." + table + @" ADD " + col + @" BIT NOT NULL Default 0;
                                                    END
                                                    ";

			return q1;
		}
		public string validationKey = "AMAAMACIp6vDgRDnHDWJKjuzJl0yZcr58xPSuYH0g3jkeR6uRFZAbLMQ8H+XucyNThtbMI0DAAEAAQ==";
		public string PieceCaisse(int type)
		{
			if (type == 0)
			{
				return "RC-";
			}
			else
			{
				return "DS-";
			}
		}
		public Dictionary<string, object> Lenth(int lenth)
		{
			return new Dictionary<string, object>() { { "maxlength", lenth.ToString() } };
		}
		public NumericEditCellParams cellParams()
		{
			NumericEditCellParams parameters = new NumericEditCellParams() { Params = new NumericTextBoxModel<object>() { Decimals = 2, Format = "N2", ShowSpinButton = false } };
			return parameters;
		}
		public bool IsAuthorized(ApplicationUser user, List<string> roles)
		{
			user = user ?? new ApplicationUser();
			if (user.Roles == null)
			{
				return false;
			}
			else
			{
				return user.Roles.Select(a => a.Name).Intersect(roles).Any();
			}

		}
		public void CopyData(Object parent, Object child)
		{
			var parentProperties = parent.GetType().GetProperties();
			var childProperties = child.GetType().GetProperties();
			foreach (var parentProperty in parentProperties)

			{
				foreach (var childProperty in childProperties)
				{
					if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType && parentProperty.Name != "cbMarq" && parentProperty.Name != "id" && parentProperty.Name != "DL_No" && (!parentProperty.Name.ToUpper().Contains("NAVIGATION")))
					{
						childProperty.SetValue(child, parentProperty.GetValue(parent));
						break;
					}
				}
			}
		}
		public string getPath(string societe, string table, string id)
		{

			string root = "wwwroot/data";
			string url = root + "/" + Int16.Parse(societe).ToString("000000000000") + "/" + table + "/" + Int16.Parse(id).ToString("000000000000") + "/Files";
			return url;
		}
		public string getConnectionString(TSociete ste)
		{
			if (ste == null)
			{
				ste = new TSociete();
			}
			return "Server=" + (ste.Serveur ?? "") + ";Connection Timeout=360;Persist Security Info=False;TrustServerCertificate=True;User ID=" + (ste.Web ?? "sa") + ";Password=" + (ste.Passe ?? "") + ";Initial Catalog=" + (ste.Base1 ?? "") + ";MultipleActiveResultSets=False;";
		}
        public string getConnectionStringCheck(TSociete ste)
        {
            if (ste == null)
            {
                ste = new TSociete();
            }
            return "Server=" + (ste.Serveur ?? "") + ";Connection Timeout=10;Persist Security Info=False;TrustServerCertificate=True;User ID=" + (ste.Web ?? "sa") + ";Password=" + (ste.Passe ?? "") + ";Initial Catalog=" + (ste.Base1 ?? "") + ";MultipleActiveResultSets=False;";
        }
        public DB getDbCheck(TSociete ste)
        {
            var optionBuilder = new DbContextOptionsBuilder<DB>();
            optionBuilder.UseSqlServer(getConnectionString(ste), o =>
            {
                o.UseCompatibilityLevel(100);
                o.CommandTimeout(10); // Timeout in seconds (default is 30)
                                        // Add this line to handle triggers with OUTPUT clause:
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                 .UseRelationalNulls();
            });

            optionBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            DB db = new DB(optionBuilder.Options);
            return db;
        }
        public string getDevise(DB db)
		{
			string rs = "MAD";
			if (db.P_DEVISE.Count() != 0)
				rs = db.P_DEVISE.First().D_Monnaie;

			return rs;
		}
		public DB getDb(TSociete ste)
		{
			var optionBuilder = new DbContextOptionsBuilder<DB>();
			optionBuilder.UseSqlServer(getConnectionString(ste), o =>
			{
				o.UseCompatibilityLevel(100);
				o.CommandTimeout(3600); // Timeout in seconds (default is 30)
										// Add this line to handle triggers with OUTPUT clause:
				o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
				 .UseRelationalNulls();
			});

			optionBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

			DB db = new DB(optionBuilder.Options);
			db.Database.Migrate();
			db.Database.ExecuteSqlRaw(this.AddCol("F_DOCENTETE", "ChefChantier", "VARCHAR(100)"));
			db.Database.ExecuteSqlRaw(this.AddCol("F_DOCENTETE", "Demandeur", "VARCHAR(100)"));

			return db;
		}
		public string AddCol(string table, string col, string type)
		{
			if (type.ToUpper().Contains("INT") || type.ToUpper().Contains("DECIMAL") || type.ToUpper().Contains("BOOL"))
			{
				return @"IF NOT EXISTS (SELECT 1
                                        FROM INFORMATION_SCHEMA.COLUMNS
                                        WHERE upper(TABLE_NAME) = '" + table + "'"
											+ "AND upper(COLUMN_NAME) = '" + col + "')"
									+ @"BEGIN
                                    ALTER TABLE dbo." + table + " ADD " + col + " " + type + " DEFAULT 0;"
									+ @"END
                                ";
			}
			else
			{
				return @"IF NOT EXISTS (SELECT 1
                                        FROM INFORMATION_SCHEMA.COLUMNS
                                        WHERE upper(TABLE_NAME) = '" + table + "'"
							+ "AND upper(COLUMN_NAME) = '" + col + "')"
					+ @"BEGIN
                                    ALTER TABLE dbo." + table + " ADD " + col + " " + type + " ;"
					+ @"END
                                ";
			}

		}
		public string getArticleRef(string FA_CodeFamille, SessionDT session)
		{
			string rs = "";
			var par = session.db.P_GENAUTO.First();
			F_ARTICLE ar = new F_ARTICLE();

			if (par.GE_ArtNumerot == 0)
			{
				ar = session.db.F_ARTICLE.OrderByDescending(a => a.cbMarq).First();
				rs = getNextCode(ar.AR_Ref);
			}

			if (par.GE_ArtNumerot == 1)
			{
				if (par.GE_ArtLen == 0)
				{
					par.GE_ArtLen = 15;
				}

				if (par.GE_ArtTypeRacine == 0)
				{
					var dt = session.db.F_ARTICLE.Where(a => a.AR_Ref.Length == par.GE_ArtLen && a.AR_Ref.StartsWith(par.GE_ArtRacine));
					if (dt.Count() != 0)
					{
						ar = dt.OrderByDescending(a => a.cbMarq).First();
						rs = getNextCode(ar.AR_Ref);
					}
					else
					{
						string end = "";
						for (int i = 0; i < par.GE_ArtLen - par.GE_ArtRacine.Length; i++)
						{
							end = end + "0";
						}
						rs = getNextCode(par.GE_ArtRacine + end);
					}
				}
				else
				{
					if (FA_CodeFamille != null)
					{
						F_FAMILLE fa = session.db.F_FAMILLE.Where(a => a.FA_CodeFamille == FA_CodeFamille).SingleOrDefault();
						if (fa != null)
						{
							var dt = session.db.F_ARTICLE.Where(a => a.AR_Ref.Length == par.GE_ArtLen && a.AR_Ref.StartsWith(fa.FA_RacineRef));
							if (dt.Count() != 0)
							{
								ar = dt.OrderByDescending(a => a.cbMarq).First();
								rs = getNextCode(ar.AR_Ref);
							}
							else
							{
								string end = "";
								for (int i = 0; i < par.GE_ArtLen - fa.FA_RacineRef.Length; i++)
								{
									end = end + "0";
								}
								rs = getNextCode(fa.FA_RacineRef + end);
							}
						}

					}
				}


			}



			return rs;
		}
		public string getNextCode(string serie)
		{
			string result = "";
			result = Regex.Replace(serie, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
			return result;
		}
		public string IncrementString(string input)
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
		public void DisableAllTriggers(DB db, string Table)
		{
			string q1 = @"DECLARE @tableName NVARCHAR(MAX);" +
							@"SET @tableName = '" + Table + "';" +

							@"DECLARE @disableTriggerScript NVARCHAR(MAX) = '';

                            SELECT @disableTriggerScript = @disableTriggerScript + 
                                'DISABLE TRIGGER ' + name + ' ON ' + @tableName + ';' + CHAR(13)
                            FROM sys.triggers
                            WHERE parent_class = 1 -- Table object
                                AND parent_id = OBJECT_ID(@tableName);

                            EXEC(@disableTriggerScript);
                            ";
			db.Database.ExecuteSqlRaw(q1);
		}
		public void EnableAllTriggers(DB db, string Table)
		{
			string q1 = @"DECLARE @tableName NVARCHAR(MAX);" +
							@"SET @tableName = '" + Table + "';" +

							@"DECLARE @disableTriggerScript NVARCHAR(MAX) = '';

                            SELECT @disableTriggerScript = @disableTriggerScript + 
                                'ENABLE TRIGGER ' + name + ' ON ' + @tableName + ';' + CHAR(13)
                            FROM sys.triggers
                            WHERE parent_class = 1 -- Table object
                                AND parent_id = OBJECT_ID(@tableName);

                            EXEC(@disableTriggerScript);
                            ";
			db.Database.ExecuteSqlRaw(q1);
		}
		public void DisableTriggers(DB db, string Table, string Operation)
		{
			if (Table == "F_CREGLEMENT" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER TG_CBUPD_F_CREGLEMENT ON F_CREGLEMENT");
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER TG_UPD_CPTAF_CREGLEMENT ON F_CREGLEMENT");
			}
			else
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER TG_" + Operation + "_" + Table + " ON " + Table);
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER TG_CB" + Operation + "_" + Table + " ON " + Table);
			}

			if (Table == "F_DOCENTETE" && Operation == "INS")
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER [TG_INS_CPTAF_DOCENTETE] ON F_DOCENTETE");
			}
			if (Table == "F_DOCLIGNE" && Operation == "INS")
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER [TG_INS_CPTAF_DOCLIGNE] ON F_DOCLIGNE");
			}
			if (Table == "F_DOCENTETE" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER [TG_UPD_CPTAF_DOCENTETE] ON F_DOCENTETE");
			}
			if (Table == "F_DOCLIGNE" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("DISABLE TRIGGER [TG_UPD_CPTAF_DOCLIGNE] ON F_DOCLIGNE");
			}
		}
		public void EnableTriggers(DB db, string Table, string Operation)
		{
			if (Table == "F_CREGLEMENT" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER TG_CBUPD_F_CREGLEMENT ON F_CREGLEMENT");
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER TG_UPD_CPTAF_CREGLEMENT ON F_CREGLEMENT");
			}
			else
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER TG_" + Operation + "_" + Table + " ON " + Table);
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER TG_CB" + Operation + "_" + Table + " ON " + Table);
			}

			if (Table == "F_DOCENTETE" && Operation == "INS")
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER [TG_INS_CPTAF_DOCENTETE] ON F_DOCENTETE");
			}
			if (Table == "F_DOCLIGNE" && Operation == "INS")
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER [TG_INS_CPTAF_DOCLIGNE] ON F_DOCLIGNE");
			}
			if (Table == "F_DOCENTETE" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER [TG_UPD_CPTAF_DOCENTETE] ON F_DOCENTETE");
			}
			if (Table == "F_DOCLIGNE" && Operation == "UPD")
			{
				db.Database.ExecuteSqlRaw("ENABLE TRIGGER [TG_UPD_CPTAF_DOCLIGNE] ON F_DOCLIGNE");
			}
		}
		public int DC_IdCol(int DO_Type, int DO_Provenance)
		{
			int rs = 0;
			if (DO_Type != 6 && DO_Type != 16)
			{
				if (DO_Type <= 6)
				{
					rs = DO_Type;
				}
				else
				{
					rs = DO_Type - 10;
				}

			}
			else
			{
				if (DO_Type == 6)
				{
					if (DO_Provenance == 0)
					{
						rs = 6;
					}
					else if (DO_Provenance == 1)
					{
						rs = 7;
					}
					else
					{
						rs = 8;
					}
				}
				else
				{
					if (DO_Provenance == 0)
					{
						rs = 16;
					}
					else if (DO_Provenance == 1)
					{
						rs = 17;
					}
					else
					{
						rs = 18;
					}
				}
			}

			return rs;
		}
		public void FillAR(F_ARTICLE a)
		{
			a.AR_Garantie = 0;
			a.AR_UnitePoids = 0;
			a.AR_PoidsNet = 0;
			a.AR_PoidsBrut = 0;
			a.AR_PrixAch = 0;
			a.AR_Coef = 0;
			a.AR_PrixVen = 0;
			a.AR_Gamme1 = 0;
			a.AR_Gamme2 = 0;
			a.AR_Nomencl = 0;
			a.AR_Stat01 = "";
			a.AR_Stat02 = "";
			a.AR_Stat03 = "";
			a.AR_Stat04 = "";
			a.AR_Stat05 = "";
			a.AR_Escompte = 0;
			a.AR_Delai = 0;
			a.AR_HorsStat = 0;
			a.AR_VteDebit = 0;
			a.AR_NotImp = 0;
			a.AR_Sommeil = 0;
			a.AR_Langue1 = "";
			a.AR_Langue2 = "";
			a.AR_CodeFiscal = "";
			a.AR_Pays = "";

			a.AR_Frais01FR_Denomination = "";
			a.AR_Frais01FR_Rem01REM_Valeur = 0;
			a.AR_Frais01FR_Rem01REM_Type = 0;
			a.AR_Frais01FR_Rem02REM_Valeur = 0;
			a.AR_Frais01FR_Rem02REM_Type = 0;
			a.AR_Frais01FR_Rem03REM_Valeur = 0;
			a.AR_Frais01FR_Rem03REM_Type = 0;

			a.AR_Frais02FR_Denomination = "";
			a.AR_Frais02FR_Rem01REM_Valeur = 0;
			a.AR_Frais02FR_Rem01REM_Type = 0;
			a.AR_Frais02FR_Rem02REM_Valeur = 0;
			a.AR_Frais02FR_Rem02REM_Type = 0;
			a.AR_Frais02FR_Rem02REM_Valeur = 0;
			a.AR_Frais02FR_Rem02REM_Type = 0;

			a.AR_Frais03FR_Denomination = "";
			a.AR_Frais03FR_Rem01REM_Valeur = 0;
			a.AR_Frais03FR_Rem01REM_Type = 0;
			a.AR_Frais03FR_Rem02REM_Valeur = 0;
			a.AR_Frais03FR_Rem02REM_Type = 0;
			a.AR_Frais03FR_Rem03REM_Valeur = 0;
			a.AR_Frais03FR_Rem03REM_Type = 0;

			a.AR_Condition = 0;
			a.AR_PUNet = 0;
			a.AR_Contremarque = 0;
			a.AR_FactForfait = 0;
			a.AR_FactPoids = 0;
			a.AR_SaisieVar = 0;
			a.AR_Transfere = 0;
			a.AR_Publie = 0;
			a.AR_PrixAchNouv = 0;
			a.AR_PrixVenNouv = 0;
			a.AR_CoefNouv = 0;
			a.AR_CoutStd = 0;
			a.AR_QteComp = 0;
			a.AR_QteOperatoire = 0;
			a.CO_No = 0;
			a.AR_Prevision = 0;
			a.CL_No1 = 0;
			a.CL_No2 = 0;
			a.CL_No3 = 0;
			a.CL_No4 = 0;
			a.AR_Type = 0;
			a.AR_Nature = 0;
			a.AR_DelaiFabrication = 0;
			a.AR_NbColis = 0;
			a.AR_DelaiPeremption = 0;
			a.AR_DelaiSecurite = 0;
			a.AR_Fictif = 0;
			a.AR_SousTraitance = 0;
			a.AR_TypeLancement = 0;
			a.AR_SuiviStock = 0;


		}
		public void FillCT(F_COMPTET a)
		{
			//CT
			a.N_Devise = 0;
			a.BT_Num = 0;
			a.CT_Encours = 0;
			a.CT_Assurance = 0;
			a.N_Risque = 1;
			a.N_Devise = 0;
			a.N_CatTarif = 3;
			a.CT_Taux01 = 0;
			a.CT_Taux02 = 0;
			a.CT_Taux03 = 0;
			a.CT_Taux04 = 0;
			a.N_CatCompta = 1;
			a.CT_Contact = "";
			a.CT_CodePostal = "";
			a.CT_CodeRegion = "";
			a.CT_Ape = "";
			a.CT_Identifiant = "";
			a.CO_No = 0;
			a.N_Period = 1;
			a.CT_Facture = 0;
			a.CT_BLFact = 0;
			a.N_Expedition = 1;
			a.N_Condition = 1;
			a.CT_Saut = 1;
			a.CT_Lettrage = 1;
			a.CT_ValidEch = 0;
			a.CT_Sommeil = 0;
			a.DE_No = 0;
			a.CT_ControlEnc = 0;
			a.CT_NotPenal = 0;
			a.N_Analytique = 0;
			a.CT_Telecopie = "";
			a.CT_EMail = "";
			a.CT_Site = "";
			a.CT_Coface = "";
			a.CT_Surveillance = 0;
			a.CT_Langue = 0;
			a.CT_NotRappel = 0;
			a.EB_No = 0;
		}
		public void FillDL(F_DOCLIGNE a)
		{
			//DL
			a.DL_TNomencl = 0;
			a.DL_TRemExep = 0;
			a.DL_TRemPied = 0;
			a.DL_PoidsBrut = 0;
			a.DL_PoidsNet = 0;
			a.DL_Remise01REM_Type = 0;
			a.DL_Remise01REM_Valeur = 0;
			a.DL_Remise02REM_Type = 0;
			a.DL_Remise02REM_Valeur = 0;
			a.DL_Remise03REM_Type = 0;
			a.DL_Remise03REM_Valeur = 0;
			a.DL_PUBC = 0;
			a.DL_TypeTaux1 = 0;
			a.DL_TypeTaux2 = 0;
			a.DL_TypeTaux3 = 0;
			a.DL_TypeTaxe1 = 1;
			a.DL_TypeTaxe2 = 1;
			a.DL_TypeTaxe3 = 1;
			a.AG_No1 = 0;
			a.AG_No2 = 0;
			a.CO_No = 0;
			a.DT_No = 0;
			a.AF_RefFourniss = "";
			a.DL_PUDevise = 0;
			a.DL_NoRef = 1;
			a.DO_DateLivr = new DateTime(1900, 01, 01);
			a.DL_DateAvancement = new DateTime(1900, 01, 01);
			a.PF_Num = "";
			a.DL_Taxe2 = 0;
			a.DL_Taxe3 = 0;
			a.DL_NonLivre = 0;
			a.DL_NoLink = 0;
			a.DL_QteRessource = 0;
			a.DL_Valorise = 1;
			a.DL_TTC = 0;
			a.DL_Frais = 0;
			a.DL_NonLivre = 0;
			a.AC_RefClient = "";
			a.DL_FactPoids = 0;
			a.DL_Escompte = 0;
			a.DL_NoColis = "";
			a.DL_DateBC = new DateTime(1900, 01, 01);
			a.DL_DateBL = new DateTime(1900, 01, 01);
			a.DL_DatePL = new DateTime(1900, 01, 01);
			a.DO_DateLivr = new DateTime(1900, 01, 01);
			a.DL_DateAvancement = new DateTime(1900, 01, 01);

			a.DL_Qte = 1;
			a.DL_QteBC = 0;
			a.DL_QtePL = 0;
			a.DL_QteBL = 0;
			a.EU_Qte = 0;
			a.DL_CMUP = 0;
			a.DL_PrixRU = 0;
			a.DL_PieceBC = "";
			a.DL_PieceBL = "";
			a.DL_PiecePL = "";
			a.DL_MontantHT = 0;
			a.DL_MontantTTC = 0;
			a.DL_PrixRU = 0;
			a.DL_PrixUnitaire = 0;
			a.DL_Taxe1 = 0;
			a.DL_Taxe2 = 0;
			a.DL_Taxe3 = 0;
			a.DL_PUTTC = 0;
			a.DL_TypePL = 0;





		}
		public void FillDO(F_DOCENTETE a)
		{
			a.DO_Date = DateTime.Today;
			a.CO_No = 0;
			a.DO_Period = 0;
			a.DO_Devise = 0;
			a.DO_Cours = 0;
			a.LI_No = 0;
			a.DO_Expedit = 0;
			a.DO_NbFacture = 0;
			a.DO_BLFact = 0;
			a.DO_TxEscompte = 0;
			a.DO_Reliquat = 0;
			a.DO_Imprim = 0;
			a.DO_Coord01 = "";
			a.DO_Coord02 = "";
			a.DO_Coord03 = "";
			a.DO_Coord04 = "";
			a.DO_ValFranco = 0;
			a.DO_TypeLigneFranco = 0;
			a.DO_Taxe1 = 0;
			a.DO_TypeTaux1 = 0;
			a.DO_TypeTaxe1 = 0;
			a.DO_Taxe2 = 0;
			a.DO_TypeTaux2 = 0;
			a.DO_TypeTaxe2 = 0;
			a.DO_Taxe3 = 0;
			a.DO_TypeTaux3 = 0;
			a.DO_TypeTaxe3 = 0;
			a.DO_MajCpta = 0;
			a.DO_Motif = "";
			a.DO_Contact = "";
			a.DO_FactureElec = 0;
			a.DO_TypeTransac = 0;
			a.DO_DateLivr = new DateTime(1900, 01, 01);
			a.DO_DateExpedition = new DateTime(1900, 01, 01);
			a.DO_FactureFrs = "";
			a.DO_PieceOrig = "";
			a.DO_DemandeRegul = 0;
			a.ET_No = 0;
			a.DO_Valide = 0;
			a.DO_Coffre = 0;
			a.DO_DebutAbo = new DateTime(1900, 01, 01);
			a.DO_FinAbo = new DateTime(1900, 01, 01);
			a.DO_DebutPeriod = new DateTime(1900, 01, 01);
			a.DO_FinPeriod = new DateTime(1900, 01, 01);
			a.DO_Statut = 0;
			a.CA_No = 0;
			a.CO_NoCaissier = 0;
			a.DO_Transfere = 0;
			a.DO_Cloture = 0;
			a.DO_NoWeb = "";
			a.DO_Attente = 0;
			a.DO_Provenance = 0;
			a.CA_NumIFRS = "";
			a.MR_No = 0;
			a.DO_TypeFrais = 0;
			a.DO_ValFrais = 0;
			a.DO_TypeLigneFrais = 0;
			a.DO_TypeFranco = 0;
			a.DO_Souche = 0;
			a.DO_DateLivr = new DateTime(1900, 01, 01);
			a.DO_Condition = 0;
			a.DO_Tarif = 0;
			a.DO_Colisage = 1;
			a.DO_Transaction = 0;
			a.DO_Langue = 0;
			a.DO_Ecart = 0;
			a.DO_Regime = 0;
			a.N_CatCompta = 0;
			a.DO_Ventile = 0;
			a.AB_No = 0;
			a.DO_TotalHT = 0;
			a.DO_TxEscompte = 0;

			a.DO_Expedit = 1;
			a.DO_NbFacture = 1;
			a.DO_Condition = 1;
			a.DO_Tarif = 1;
			a.DO_Colisage = 1;
			a.DO_TypeColis = 1;
			a.N_CatCompta = 1;
			a.DO_DateLivrRealisee = new DateTime(1900, 1, 1);
			a.DO_Period = 1;




		}
		public void FillCA(F_COMPTEA a)
		{
			a.N_Analytique = 1;
			a.CA_Type = 0;
			a.CA_Raccourci = "";
			a.CA_Report = 1;
			a.N_Analyse = 2;
			a.CA_Saut = 1;
			a.CA_Sommeil = 0;
			a.CA_Domaine = 0;
			a.CA_Achat = 0;
			a.CA_Vente = 0;
			a.CO_No = 0;
			a.CA_Statut = 0;
			a.CA_ModeFacturation = 0;
		}
		public void FillCG(F_COMPTEG a)
		{
			a.CG_Type = 0;
			a.CG_Report = 2;
			a.CG_Raccourci = "";
			a.CG_Saut = 1; ;
			a.CG_Regroup = 0;
			a.CG_Analytique = 0;
			a.CG_Echeance = 0;
			a.CG_Quantite = 0;
			a.CG_Lettrage = 1;
			a.CG_Tiers = 1;
			a.CG_Devise = 1;
			a.N_Devise = 0;
			a.CG_Sommeil = 0;
			a.CG_ReportAnal = 0;




		}
		public List<Items> VersionsSage()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 1, Name = "Sage 100C V8" });
			list.Add(new Items { Id = 2, Name = "Sage 100C V7" });
			list.Add(new Items { Id = 3, Name = "Sage 100C V6" });
			list.Add(new Items { Id = 0, Name = "Sage 100C V5" });
			list.Add(new Items { Id = 4, Name = "Sage 100C V4" });
			list.Add(new Items { Id = 5, Name = "Sage 100C V3" });
			list.Add(new Items { Id = 6, Name = "Sage 100C V2" });
			list.Add(new Items { Id = 7, Name = "Sage 100C V1" });
			list.Add(new Items { Id = 8, Name = "Sage 100 V8" });

			return list;
		}
		public List<Items> AV_StatutContrat()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Saisi" });
			list.Add(new Items { Id = 1, Name = "Validé" });
			list.Add(new Items { Id = 2, Name = "Expiré" });
			list.Add(new Items { Id = 3, Name = "Annulé" });

			return list;
		}
		public List<Items> AV_Regime()
		{
			List<Items> list = new List<Items>();

			return list;
		}
		public List<Items> AV_TypeOffre()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 5, Name = "Early Booking".ToUpper() });
			list.Add(new Items { Id = 1, Name = "Long Stay".ToUpper() });
			list.Add(new Items { Id = 2, Name = "6 = 7".ToUpper() });
			list.Add(new Items { Id = 3, Name = "Travel Agent".ToUpper() });
			list.Add(new Items { Id = 4, Name = "Diver".ToUpper() });


			return list;
		}
		public List<Items> AV_TypeArticle()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Chambre" });
			list.Add(new Items { Id = 1, Name = "Supplément" });
			list.Add(new Items { Id = 2, Name = "Réduction" });
			list.Add(new Items { Id = 3, Name = "Taxe" });
			return list;
		}
		public List<Items> SituationMarche()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "En arrét" });
			list.Add(new Items { Id = 1, Name = "En cours" });
			list.Add(new Items { Id = 2, Name = "Récéptionné" });
			list.Add(new Items { Id = 3, Name = "Résilié" });

			return list;
		}
		public List<Items> TypeMarche()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Non reconductible" });
			list.Add(new Items { Id = 1, Name = "Reconductible" });

			return list;
		}
		public List<Items> PhaseMarche()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Arrét" });
			list.Add(new Items { Id = 1, Name = "Phase de garantie" });
			list.Add(new Items { Id = 2, Name = "Phase d'entretien" });
			list.Add(new Items { Id = 3, Name = "Phase d'execution" });
			list.Add(new Items { Id = 4, Name = "Récéptionné definitivement" });
			return list;
		}
		public List<Items> AttributeTables()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 1, Name = "Articles" });
			list.Add(new Items { Id = 2, Name = "Entete document" });
			list.Add(new Items { Id = 3, Name = "Ligne document" });
			list.Add(new Items { Id = 4, Name = "Ecriture comptable" });
			list.Add(new Items { Id = 5, Name = "Client" });
			list.Add(new Items { Id = 11, Name = "Fournisseur" });
			list.Add(new Items { Id = 6, Name = "Compte général" });
			list.Add(new Items { Id = 7, Name = "Affaire" });
			list.Add(new Items { Id = 8, Name = "Projet" });
			list.Add(new Items { Id = 9, Name = "Entete caisse" });
			list.Add(new Items { Id = 10, Name = "Ligne caisse" });
			return list;
		}
		public List<Items> AttributeModels()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 1, Name = "Articles liste" });
			list.Add(new Items { Id = 2, Name = "Clients liste" });
			list.Add(new Items { Id = 3, Name = "Fournisseurs liste" });
			list.Add(new Items { Id = 4, Name = "Comptes généraux liste" });
			list.Add(new Items { Id = 5, Name = "Affaires liste" });
			list.Add(new Items { Id = 6, Name = "Projets liste" });
			list.Add(new Items { Id = 8, Name = "Sites liste" });
			list.Add(new Items { Id = 10, Name = "Personnels liste" });
			list.Add(new Items { Id = 11, Name = "Matériels liste" });
			return list;
		}
		public string getMontant(Decimal number)
		{
			DecimalToFrench dc = new DecimalToFrench();
			return dc.ConvertToFrench(number)?.ToUpper();
		}
		public string getMontant2(float chiffre)
		{
			float centaine, dizaine, unite, reste, y;
			bool dix = false;
			string lettre = "";
			//strcpy(lettre, "");

			reste = chiffre / 1;

			for (int i = 1000000000; i >= 1; i /= 1000)
			{
				y = reste / i;
				if (y != 0)
				{
					centaine = y / 100;
					dizaine = (y - centaine * 100) / 10;
					unite = y - (centaine * 100) - (dizaine * 10);
					switch (centaine)
					{
						case 0:
							break;
						case 1:
							lettre += "cent ";
							break;
						case 2:
							if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
							else lettre += "deux cent ";
							break;
						case 3:
							if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
							else lettre += "trois cent ";
							break;
						case 4:
							if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
							else lettre += "quatre cent ";
							break;
						case 5:
							if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
							else lettre += "cinq cent ";
							break;
						case 6:
							if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
							else lettre += "six cent ";
							break;
						case 7:
							if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
							else lettre += "sept cent ";
							break;
						case 8:
							if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
							else lettre += "huit cent ";
							break;
						case 9:
							if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
							else lettre += "neuf cent ";
							break;
					}// endSwitch(centaine)

					switch (dizaine)
					{
						case 0:
							break;
						case 1:
							dix = true;
							break;
						case 2:
							lettre += "vingt ";
							break;
						case 3:
							lettre += "trente ";
							break;
						case 4:
							lettre += "quarante ";
							break;
						case 5:
							lettre += "cinquante ";
							break;
						case 6:
							lettre += "soixante ";
							break;
						case 7:
							dix = true;
							lettre += "soixante ";
							break;
						case 8:
							lettre += "quatre-vingt ";
							break;
						case 9:
							dix = true;
							lettre += "quatre-vingt ";
							break;
					} // endSwitch(dizaine)

					switch (unite)
					{
						case 0:
							if (dix) lettre += "dix ";
							break;
						case 1:
							if (dix) lettre += "onze ";
							else lettre += "un ";
							break;
						case 2:
							if (dix) lettre += "douze ";
							else lettre += "deux ";
							break;
						case 3:
							if (dix) lettre += "treize ";
							else lettre += "trois ";
							break;
						case 4:
							if (dix) lettre += "quatorze ";
							else lettre += "quatre ";
							break;
						case 5:
							if (dix) lettre += "quinze ";
							else lettre += "cinq ";
							break;
						case 6:
							if (dix) lettre += "seize ";
							else lettre += "six ";
							break;
						case 7:
							if (dix) lettre += "dix-sept ";
							else lettre += "sept ";
							break;
						case 8:
							if (dix) lettre += "dix-huit ";
							else lettre += "huit ";
							break;
						case 9:
							if (dix) lettre += "dix-neuf ";
							else lettre += "neuf ";
							break;
					} // endSwitch(unite)

					switch (i)
					{
						case 1000000000:
							if (y > 1) lettre += "milliards ";
							else lettre += "milliard ";
							break;
						case 1000000:
							if (y > 1) lettre += "millions ";
							else lettre += "million ";
							break;
						case 1000:
							lettre += "mille ";
							break;
					}
				} // end if(y!=0)
				reste -= y * i;
				dix = false;
			} // end for
			if (lettre.Length == 0) lettre += "zero";

			return (lettre ?? "").ToUpper();
		}
		public List<Items> ListeCarburant()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Gasoil" });
			list.Add(new Items { Id = 1, Name = "Essence" });
			list.Add(new Items { Id = 2, Name = "Electricité" });
			list.Add(new Items { Id = 3, Name = "Autre" });

			return list;
		}
		public List<Items> TypeMateriel()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Voiture" });
			list.Add(new Items { Id = 1, Name = "Pick-UP" });
			list.Add(new Items { Id = 2, Name = "Materiel Lourd" });
			list.Add(new Items { Id = 3, Name = "Autre" });

			return list;
		}
		public List<Items> AchatMateriel()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Achat" });
			list.Add(new Items { Id = 1, Name = "Leasing" });
			list.Add(new Items { Id = 2, Name = "Location" });

			return list;
		}
		public List<Items> SalaireType()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Mensuel" });
			list.Add(new Items { Id = 1, Name = "Journalier" });
			list.Add(new Items { Id = 2, Name = "Horaire" });

			return list;
		}
		public List<Items> CaisseMV()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Recette" });
			list.Add(new Items { Id = 1, Name = "Dépense" });

			return list;
		}
		public List<Items> Consistance()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "F" });
			list.Add(new Items { Id = 1, Name = "M" });
			list.Add(new Items { Id = 2, Name = "F+M" });

			return list;
		}
		public List<Items> TypeAssurance()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Assurance AT" });
			list.Add(new Items { Id = 1, Name = "Assurance RC" });
			list.Add(new Items { Id = 2, Name = "Tour risque chantier" });
			list.Add(new Items { Id = 3, Name = "Assurance parc" });
			return list;
		}
		public List<Items> Receptions()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Prévisionnelle" });
			list.Add(new Items { Id = 1, Name = "Partielle" });
			list.Add(new Items { Id = 2, Name = "Définitive" });

			return list;
		}
		public List<Items> TypeEntretien()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Consommation Gasoil" });
			list.Add(new Items { Id = 1, Name = "Assurance" });
			list.Add(new Items { Id = 2, Name = "Vignette" });
			list.Add(new Items { Id = 3, Name = "Visite Technique" });
			list.Add(new Items { Id = 4, Name = "Changement Pneus" });
			list.Add(new Items { Id = 5, Name = "GPS" });
			list.Add(new Items { Id = 6, Name = "Vidange" });
			list.Add(new Items { Id = 7, Name = "Entretien et réparation" });
			list.Add(new Items { Id = 8, Name = "Autres" });
			list.Add(new Items { Id = 9, Name = "Achat" });
			list.Add(new Items { Id = 10, Name = "Carnet de circulation" });
			list.Add(new Items { Id = 11, Name = "Carnet du disque" });
			list.Add(new Items { Id = 12, Name = "Taxe sur tonnage" });
			list.Add(new Items { Id = 13, Name = "Maintenance des extincteurs" });

			return list;
		}
		public List<Items> GetDocumentStatut(int DO_Type)
		{
			List<Items> list = new List<Items>();

			if (DO_Type == 0)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 1)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 2)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 3)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 4)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 5)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 6)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 7)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 10)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 11)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 12)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 13)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 14)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 15)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 16)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 17)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 20)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 21)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type == 23)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}
			if (DO_Type >= 40)
			{
				list.Add(new Items { ShortId = 0, Name = "Saisi" });
				list.Add(new Items { ShortId = 1, Name = "Confirmé" });
				list.Add(new Items { ShortId = 2, Name = "Validé" });
			}



			return list;
		}
		public List<Items> GroupedBy()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 1, Name = "Par Pièce" });
			list.Add(new Items { Id = 2, Name = "Par Tiers" });
			list.Add(new Items { Id = 3, Name = "Par Article" });
			list.Add(new Items { Id = 4, Name = "Par Collaborateur" });
			list.Add(new Items { Id = 5, Name = "Par Affaire" });
			return list;
		}
		public List<Items> CertifStatut()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "En cours" });
			list.Add(new Items { Id = 1, Name = "Ok" });
			return list;
		}
		public List<Items> TypeSociete()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Société à Responsabilité Limitée (SARL)" });
			list.Add(new Items { Id = 1, Name = "Entreprise individuelle" });
			list.Add(new Items { Id = 2, Name = "Société en Nom Collectif" });
			list.Add(new Items { Id = 3, Name = "Société en commandite simple" });
			list.Add(new Items { Id = 4, Name = "Société anonyme simplifiée" });
			list.Add(new Items { Id = 5, Name = "Société en commandite par actions" });
			list.Add(new Items { Id = 6, Name = "Société anonyme" });
			list.Add(new Items { Id = 7, Name = "Société en participation" });
			list.Add(new Items { Id = 8, Name = "Groupement d’interêt économique" });
			list.Add(new Items { Id = 8, Name = "Succursale" });

			return list;
		}
		public List<Items> TypeCaution()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Provisoire" });
			list.Add(new Items { Id = 1, Name = "Définitive" });
			list.Add(new Items { Id = 2, Name = "Retenue de garantie" });

			return list;
		}
		public List<Items> ResultatMarche()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Raté" });
			list.Add(new Items { Id = 1, Name = "Gagné" });

			return list;
		}
		public List<Items> TypeAppelOffre()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Classique" });
			list.Add(new Items { Id = 1, Name = "Notation" });
			list.Add(new Items { Id = 2, Name = "Classification" });

			return list;
		}
		public List<Items> GetTypeIntitule(short? index)
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Devis" });
			list.Add(new Items { Id = 1, Name = "BC vente" });
			list.Add(new Items { Id = 2, Name = "PC vente" });
			list.Add(new Items { Id = 3, Name = "BL vente" });
			list.Add(new Items { Id = 4, Name = "BR vente" });
			list.Add(new Items { Id = 5, Name = "BA vente" });
			list.Add(new Items { Id = 6, Name = "FA vente" });
			list.Add(new Items { Id = 7, Name = "FC vente" });
			list.Add(new Items { Id = 10, Name = "DA Achat" });
			list.Add(new Items { Id = 11, Name = "PR achat" });
			list.Add(new Items { Id = 12, Name = "Bons de commande achat" });
			list.Add(new Items { Id = 13, Name = "Bons de livraison achat" });
			list.Add(new Items { Id = 14, Name = "Bons de retour achat" });
			list.Add(new Items { Id = 15, Name = "Bons de retour achat" });
			list.Add(new Items { Id = 16, Name = "Factures achat" });
			list.Add(new Items { Id = 17, Name = "Factures comptabilisées achat" });
			list.Add(new Items { Id = 20, Name = "Mouvements d'entrée" });
			list.Add(new Items { Id = 21, Name = "Mouvements de sortie" });
			list.Add(new Items { Id = 23, Name = "Mouvments de transfert" });

			return list;
		}
		public List<Items> getApps()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 1, Name = "Achats", Icon = "shopping_cart" });
			list.Add(new Items { Id = 2, Name = "Ventes", Icon = "monetization_on" });
			list.Add(new Items { Id = 3, Name = "Stock", Icon = "inventory" });
			list.Add(new Items { Id = 4, Name = "Projets", Icon = "home_repair_service" });
			list.Add(new Items { Id = 5, Name = "Matériels", Icon = "local_shipping" });
			list.Add(new Items { Id = 6, Name = "Personnels", Icon = "group" });
			list.Add(new Items { Id = 7, Name = "Caisse", Icon = "local_atm" });
			list.Add(new Items { Id = 8, Name = "Comptabilité", Icon = "analytics" });
			list.Add(new Items { Id = 9, Name = "Outils", Icon = "construction" });
			list.Add(new Items { Id = 10, Name = "Agence Voyage", Icon = "flight" });
			list.Add(new Items { Id = 11, Name = "Tableau de bord", Icon = "analytics" });
			list.Add(new Items { Id = 12, Name = "Caisse", Icon = "local_atm" });
			list.Add(new Items { Id = 13, Name = "Banque", Icon = "account_balance" });
			list.Add(new Items { Id = 14, Name = "Etats", Icon = "moving" });
			list.Add(new Items { Id = 15, Name = "Certification", Icon = "description" });
			list.Add(new Items { Id = 16, Name = "Transfert Données", Icon = "sync_alt" });
			list.Add(new Items { Id = 18, Name = "Utilisateurs", Icon = "people" });
			list.Add(new Items { Id = 19, Name = "Traçabilité", Icon = "update" });
            list.Add(new Items { Id = 20, Name = "Food Cost", Icon = "bakery_dining" });

            return list;
		}
		public List<Items> ModeSoumission()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Electronique" });
			list.Add(new Items { Id = 1, Name = "Physique" });
			list.Add(new Items { Id = 2, Name = "Consultation" });

			return list;
		}
		public List<Items> DemandePar()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Mail" });
			list.Add(new Items { Id = 1, Name = "Fax" });
			list.Add(new Items { Id = 2, Name = "Téléphone" });
			list.Add(new Items { Id = 3, Name = "Autre" });

			return list;
		}
		public List<Items> MethodeEtalonnage()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "AFROLAB" });
			list.Add(new Items { Id = 3, Name = "Specifique" });

			return list;
		}
		public List<Items> LieuPrestation()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Laboratoire" });
			list.Add(new Items { Id = 1, Name = "Sur site" });
			list.Add(new Items { Id = 2, Name = "Laboratoire et site" });
			list.Add(new Items { Id = 3, Name = "Autre" });

			return list;
		}
		public List<Items> PointsEtalonnage()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "AFROLAB" });
			list.Add(new Items { Id = 3, Name = "Spécifique" });

			return list;
		}
		public List<Items> NaturePrestation()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Etalonnage" });
			list.Add(new Items { Id = 1, Name = "Verification" });
			list.Add(new Items { Id = 2, Name = "Etalonnage & Verification" });

			return list;
		}
		public List<Items> Resultat()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "OK" });
			list.Add(new Items { Id = 3, Name = "Autre" });

			return list;
		}
		public List<Items> PertinenceMethode()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Pertinente" });
			list.Add(new Items { Id = 1, Name = "Inappropriee" });
			list.Add(new Items { Id = 2, Name = "Perimee" });
			list.Add(new Items { Id = 3, Name = "Autre" });
			list.Add(new Items { Id = 4, Name = "/" });

			return list;
		}
		public List<Items> OperationAttendues()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Ajustage" });
			list.Add(new Items { Id = 1, Name = "Calibration" });
			list.Add(new Items { Id = 3, Name = "Ajustage & Calibration" });

			return list;
		}
		public List<Items> PriseChargeTransport()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "AFROLAB" });
			list.Add(new Items { Id = 1, Name = "Client" });

			return list;
		}
		public List<Items> ErreurMaximalTolere()
		{
			List<Items> list = new List<Items>();
			list.Add(new Items { Id = 0, Name = "Normative" });
			list.Add(new Items { Id = 1, Name = "Utilisateur" });


			return list;
		}
		public List<Items> RegleDecision()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Avec incertitude" });
			list.Add(new Items { Id = 1, Name = "Sans incertitude" });

			return list;
		}
		public List<Items> JugementConformite()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 0, Name = "Oui" });
			list.Add(new Items { Id = 1, Name = "Non" });

			return list;
		}
		public List<Items> GetMois()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 1, Name = "Janvier" });
			list.Add(new Items { Id = 2, Name = "Février" });
			list.Add(new Items { Id = 3, Name = "Mars" });
			list.Add(new Items { Id = 4, Name = "Avril" });
			list.Add(new Items { Id = 5, Name = "Mai" });
			list.Add(new Items { Id = 6, Name = "Juin" });
			list.Add(new Items { Id = 7, Name = "Juillet" });
			list.Add(new Items { Id = 8, Name = "Aout" });
			list.Add(new Items { Id = 9, Name = "Septembre" });
			list.Add(new Items { Id = 10, Name = "Octobre" });
			list.Add(new Items { Id = 11, Name = "Novembre" });
			list.Add(new Items { Id = 12, Name = "Décembre" });

			return list;
		}
		public List<Items> GetAnnee()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 2023, Name = "2023" });
			list.Add(new Items { Id = 2024, Name = "2024" });
			list.Add(new Items { Id = 2025, Name = "2025" });

			return list;
		}
		public List<Items> GetInformation()
		{
			List<Items> list = new List<Items>();

			list.Add(new Items { Id = 1, Name = "Types contrat" });
			list.Add(new Items { Id = 2, Name = "Diplomes" });
			list.Add(new Items { Id = 3, Name = "Départements" });
			list.Add(new Items { Id = 4, Name = "Fonctions" });
			list.Add(new Items { Id = 5, Name = "Agences" });
			list.Add(new Items { Id = 6, Name = "Modes de paiement" });
			list.Add(new Items { Id = 7, Name = "Périodes paiement" });
			list.Add(new Items { Id = 8, Name = "Nationalités" });
			list.Add(new Items { Id = 9, Name = "Situations familiales" });
			list.Add(new Items { Id = 10, Name = "Marques véhicules" });
			list.Add(new Items { Id = 11, Name = "Offres specials" });

			return list;
		}
		public string key { get; set; } = "VQJa4ZASLhXmZMkQcyYhC6KF4x7LBLry";
		public string EncryptString(string inputString)
		{
			byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);

			using (Aes aes = Aes.Create())
			{
				aes.Key = keyBytes;
				aes.GenerateIV(); // Generate a new IV (Initialization Vector) for each encryption

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

				byte[] combinedBytes = new byte[aes.IV.Length + encryptedBytes.Length];
				Array.Copy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
				Array.Copy(encryptedBytes, 0, combinedBytes, aes.IV.Length, encryptedBytes.Length);

				return Convert.ToBase64String(combinedBytes);
			}
		}

		public string DecryptString(string encryptedString)
		{
			byte[] combinedBytes = Convert.FromBase64String(encryptedString);
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);

			using (Aes aes = Aes.Create())
			{
				aes.Key = keyBytes;

				byte[] iv = new byte[aes.IV.Length];
				byte[] encryptedBytes = new byte[combinedBytes.Length - aes.IV.Length];

				Array.Copy(combinedBytes, 0, iv, 0, iv.Length);
				Array.Copy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

				aes.IV = iv;

				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

				return Encoding.UTF8.GetString(decryptedBytes);
			}
		}
		public static string NextString(string input)
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
		private const string Base36Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private const int FixedLength = 12;

		// Shuffled characters for better mixing (customize this to your preference)
		private static readonly char[] ShuffledChars = "H7XK2P9WQ5R4J6Y3L8Z1M0GUVTSNODAEFBCI".ToCharArray();

		public string Encode(int id)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be non-negative");

			// Step 1: Convert to base36
			var base36 = new StringBuilder();
			int value = id;

			do
			{
				base36.Insert(0, Base36Chars[value % 36]);
				value /= 36;
			} while (value > 0);

			string base36String = base36.ToString();

			// Step 2: Pad to at least 8 characters (for better mixing)
			if (base36String.Length < 8)
			{
				base36String = base36String.PadLeft(8, '0');
			}

			// Step 3: Hash to get more randomness (optional)
			using var sha256 = SHA256.Create();
			byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(base36String));
			string hashedString = BitConverter.ToString(hash).Replace("-", "");

			// Step 4: Convert to our mixed character set
			var mixed = new StringBuilder();
			foreach (char c in hashedString)
			{
				if (mixed.Length >= FixedLength) break;
				int index = c % ShuffledChars.Length;
				mixed.Append(ShuffledChars[index]);
			}

			// Ensure exactly 12 characters
			if (mixed.Length < FixedLength)
			{
				mixed.Append(ShuffledChars[0], FixedLength - mixed.Length);
			}

			return mixed.ToString().Substring(0, FixedLength);
		}


	}


}
