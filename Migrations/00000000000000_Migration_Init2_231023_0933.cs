using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_231023_0933 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"ALTER TABLE API_T_CertifLigne ADD  AR_Ref varchar(100)
                                    ALTER TABLE API_T_CertifLigne ADD  AR_Design  varchar(100)     
                                    ALTER TABLE API_T_CertifLigne ADD  Unite varchar(100)     
                                    ALTER TABLE API_T_CertifLigne ADD  QL_Qte decimal(24,6)  DEFAULT 0   
                                    ALTER TABLE API_T_CertifLigne ADD  DL_PU decimal(24,6)  DEFAULT 0   
                                    ALTER TABLE API_T_CertifLigne ADD  DL_Taxe decimal(24,6)  DEFAULT 0   
                                    ALTER TABLE API_T_CertifLigne ADD  DL_PUTTC decimal(24,6)  DEFAULT 0   
                                    ALTER TABLE API_T_CertifLigne ADD  DL_MontantHT decimal(24,6)  DEFAULT 0  
                                    ALTER TABLE API_T_CertifLigne ADD  DL_MontantTTC decimal(24,6)  DEFAULT 0   
                                    ALTER TABLE API_T_CertifLigne ADD  DL_CodeTaxe varchar(100)  
                                    ALTER TABLE API_T_CertifLigne ADD  Reference varchar(100)

                                    ALTER TABLE API_T_CertifEntete ADD  NumeroRapportMission varchar(100)
                                    ALTER TABLE API_T_CertifEntete ADD  DateLivraison smalldatetime");


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

                                      FROM API_T_CertifEntete a
                                      LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                      LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                      LEFT JOIN F_COLLABORATEUR co ON a.CO_No = co.CO_No
                                    GO");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
