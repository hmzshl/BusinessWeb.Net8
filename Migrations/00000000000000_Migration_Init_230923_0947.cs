using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_230923_0947 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string q1 = @"IF OBJECT_ID('[API_V_AFFAIRE]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AFFAIRE];
                                            END
                                            GO

                                            CREATE VIEW [dbo].[API_V_AFFAIRE]
                                            AS
                                            SELECT 
                                            a.N_Analytique
                                                  ,a.CA_Num
                                                  ,a.CA_Intitule
                                                  ,a.CA_Type
                                                  ,a.CA_Classement
                                                  ,a.CA_Raccourci
                                                  ,a.CA_Report
                                                  ,a.N_Analyse
                                                  ,a.CA_Saut
                                                  ,a.CA_Sommeil
                                                  ,a.CA_Domaine
                                                  ,a.CA_Achat
                                                  ,a.CA_Vente
                                                  ,a.CO_No
                                                  ,a.CA_Statut
                                                  ,a.CA_DateCreationAffaire
                                                  ,a.CA_DateAcceptAffaire
                                                  ,a.CA_DateDebutAffaire
                                                  ,a.CA_DateFinAffaire
                                                  ,a.CA_ModeFacturation
                                                  ,a.cbMarq
	                                              ,b.CADE
	                                              ,b.CABL
	                                              ,b.ResteCA
	                                              ,b.EcartCA
	                                              ,CASE WHEN ISNULL(b.CADE,0) != 0 THEN ISNULL(b.CABL,0)/ISNULL(b.CADE,0) ELSE 0 END Avancement

                                              FROM F_COMPTEA a

                                              LEFT JOIN
                                              (
	                                              SELECT 
	                                            a.CA_Num,
	                                            a.CADE,
	                                            a.CABL,
	                                            CASE WHEN (a.CADE - a.CABL > 0) AND a.CABL > 0 THEN  a.CADE - a.CABL ELSE 0 END ResteCA,
	                                            a.CADE - a.CABL EcartCA
	                                            FROM
	                                            (
	                                            SELECT 
	                                            a.CA_Num,
	                                            sum(CASE WHEN a.DO_Type = 'DE' THEN a.DL_MontantTTC ELSE 0 END) CADE,
	                                            sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_MontantTTC ELSE 0 END) CABL
	                                            FROM 
	                                            (SELECT
	                                            dl.CA_Num,
	                                            dl.DL_MontantTTC,
	                                            dl.DL_MontantHT,
	                                            CASE dl.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC' WHEN '2' THEN 'PL' ELSE 'BL' END DO_Type 
	                                            FROM F_DOCLIGNE dl
	                                            WHERE dl.AR_Ref is not null AND dl.DO_Type in (0,1,2,3,4,5,6,7) AND dl.DL_Valorise = 1) a
	                                            GROUP BY 
	                                            a.CA_Num
	                                            ) a
                                              ) b ON a.CA_Num = b.CA_Num
                                            GO


                                            ";

            string q2 = @"IF OBJECT_ID('[API_V_COLLABORATEUR]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_COLLABORATEUR];
                                            END
                                            GO

                                            CREATE VIEW [dbo].[API_V_COLLABORATEUR]
                                            AS
                                            SELECT  a.CO_No
                                                  ,a.CO_Nom
                                                  ,a.CO_Prenom
                                                  ,a.CO_Fonction
                                                  ,a.cbCO_Fonction
                                                  ,a.CO_Adresse
                                                  ,a.CO_Complement
                                                  ,a.CO_CodePostal
                                                  ,a.CO_Ville
                                                  ,a.CO_CodeRegion
                                                  ,a.CO_Pays
                                                  ,a.CO_Service
                                                  ,a.CO_Vendeur
                                                  ,a.CO_Caissier
                                                  ,a.CO_Acheteur
                                                  ,a.CO_Telephone
                                                  ,a.CO_Telecopie
                                                  ,a.CO_EMail
                                                  ,a.CO_Receptionnaire
                                                  ,a.PROT_No
                                                  ,a.CO_TelPortable
                                                  ,a.CO_ChargeRecouvr
                                                  ,a.CO_Matricule
                                                  ,a.cbCO_Matricule
                                                  ,a.CO_Financier
                                                  ,a.CO_Transmission
                                                  ,a.cbMarq
	                                              ,ISNULL(ca.MontantTTC,0) MontantTTC
	                                              ,ISNULL(fr.MontantFrais,0) MontantFrais
	                                              ,a.CO_Nom + ISNULL((' '+a.CO_Prenom),'') Intitule

                                              FROM F_COLLABORATEUR a

                                              LEFT JOIN

                                              (
                                              SELECT 
                                            a.CO_No, 
                                            SUM(a.DL_MontantTTC) MontantTTC
                                            FROM F_DOCLIGNE a 
                                            WHERE a.DL_Valorise = 1 AND a.DO_Type in (3,4,5,6,7)
                                            GROUP BY a.CO_No
                                              ) ca ON a.CO_No = ca.CO_No


                                              LEFT JOIN

                                              (
                                              SELECT 
                                            a.CO_No, 
                                            SUM(a.Montant) MontantFrais
                                            FROM API_T_FraisEntete a 
                                            GROUP BY a.CO_No
                                              ) fr ON a.CO_No = fr.CO_No
                                            GO


                                            ";


            migrationBuilder.Sql(q1);
            migrationBuilder.Sql(q2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
