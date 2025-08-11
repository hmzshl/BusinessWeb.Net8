using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250811_1141 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE VIEW [dbo].[API_V_CERTIFPOINTAGE]
						AS
						SELECT 
						a.*,
						ct.CT_Intitule,
						ct.CT_Adresse,
						ct.CT_Ville,
						CASE a.Statut WHEN 0 THEN 'En cours' WHEN 1 THEN 'Ok' END StatutIntitule,
						re.Nom ResponsableEtalonnageIntitule,
						re2.Nom ResponsableEtalonnage2Intitule,
						re3.Nom ResponsableEtalonnage3Intitule,
						rs.Nom ResponsableSaisieIntitule,
						avi.Nom VerificationAvantImpressionIntitule,
						api.Nom VerificationApresImpressionIntitule,
						CASE Certificats WHEN 1 THEN 'Oui' ELSE 'Non' END CertificatsIntitule,
						CASE VerificationScan WHEN 1 THEN 'Oui' ELSE 'Non' END VerificationScanIntitule,
						CASE Facture WHEN 1 THEN 'Oui' ELSE 'Non' END FactureIntitule,
						CASE BonDeLivraison WHEN 1 THEN 'Oui' ELSE 'Non' END BonDeLivraisonIntitule,
						CASE AccuseReception WHEN 1 THEN 'Oui' ELSE 'Non' END AccuseReceptionIntitule

						FROM API_T_CertifPointage a
						LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
						LEFT JOIN API_T_Personnel re ON a.ResponsableEtalonnage = re.id
						LEFT JOIN API_T_Personnel re2 ON a.ResponsableEtalonnage2 = re2.id
						LEFT JOIN API_T_Personnel re3 ON a.ResponsableEtalonnage3 = re3.id
						LEFT JOIN API_T_Personnel rs ON a.ResponsableSaisie = rs.id
						LEFT JOIN API_T_Personnel avi ON a.VerificationAvantImpression = avi.id
						LEFT JOIN API_T_Personnel api ON a.VerificationApresImpression = api.id";
			migrationBuilder.Sql(q1);

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
