using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init3_151123_0952 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifEntete ADD Souche smallint DEFAULT 0");
            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifEntete ADD FactureID int DEFAULT 0");
            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifEntete ADD IsFacture bit DEFAULT 0");
            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifEntete ADD FactureNum varchar(100)");
            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifEntete ADD FactureDate smalldatetime");

            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_CERTIFENTETE]
                                    AS
                                    SELECT 
		                                    a.id
                                          ,a.Type
                                          ,a.Piece
                                          ,a.Date
                                          ,a.Libelle
                                          ,a.Beneficiaire
                                          ,a.Materiel
                                          ,a.Projet
                                          ,a.CO_No
                                          ,a.CT_Num
                                          ,a.CA_Num
                                          ,a.Montant
	                                      ,ct.CT_Intitule
	                                      ,ca.CA_Intitule
	                                      ,co.CO_Nom + ' ' + co.CO_Prenom CO_Nom
	                                      ,a.DateLivraison
	                                      ,a.NumeroRapportMission
	                                      ,a.IsFacture
	                                      ,a.FactureDate,
	                                      a.FactureNum,
	                                      a.FactureID,
	                                      a.Souche,
	                                      CASE a.IsFacture WHEN 1 THEN 'Facturé' ELSE 'Non Facturé' END Statut,
	                                      sc.S_Intitule

                                      FROM API_T_CertifEntete a
                                      LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                      LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                      LEFT JOIN F_COLLABORATEUR co ON a.CO_No = co.CO_No
                                      LEFT JOIN P_SOUCHEVENTE sc ON a.Souche = sc.cbIndice

                                    GO");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
