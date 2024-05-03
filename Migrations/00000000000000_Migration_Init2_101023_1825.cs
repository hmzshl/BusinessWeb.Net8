using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_101023_1825 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"IF OBJECT_ID('[API_V_BALANCEAGEECLIENT]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_BALANCEAGEECLIENT];
                                            END
                                            GO

CREATE VIEW [dbo].[API_V_BALANCEAGEECLIENT]
				AS
				SELECT 
				a.CT_Num,
				a.CT_Intitule,
				ISNULL(a.CO_Nom,'') CO_Nom,
				ISNULL(a.CT_Ville,'') CT_Ville,
				sum(a.Reste) Encours,
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) > 150 THEN a.Reste ELSE 0 END) AN,
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 30  THEN a.Reste ELSE 0 END) '30J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 60 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 30 THEN a.Reste ELSE 0 END) '60J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 90 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 60 THEN a.Reste ELSE 0 END) '90J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 120 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 90 THEN a.Reste ELSE 0 END) '120J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 150 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 120 THEN a.Reste ELSE 0 END) '150J'

				FROM

				(SELECT 
				a.*,
				isnull(b.Reglement,0) Reglement,
				isnull(a.Montant,0) - isnull(b.Reglement,0) Reste,
				CASE WHEN (a.Montant - isnull(b.Reglement,0)) = 0 THEN 'Reglé' ELSE 'Non réglé' END Etat
				FROM
				(

				--Documents
				SELECT
				dl.CT_Num,
				ct.CT_Intitule,
				co.CO_Prenom + ' ' +co.CO_Nom CO_Nom,
				ct.CT_Ville,
				dl.DO_Date,
				dl.DO_Piece,
				dl.DO_Type,
				YEAR(dl.DO_Date) Annee,
				MONTH(dl.DO_Date) Mois,
				sum(CASE WHEN dl.DO_Type in (4,5) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END) - (sum(CASE WHEN dl.DO_Type in (4,5) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END))*do.DO_TxEscompte/100 Montant 


				FROM F_DOCLIGNE dl
				INNER JOIN F_DOCENTETE do ON dl.DO_Piece = do.DO_Piece AND dl.DO_Type = do.DO_Type
				INNER JOIN F_COMPTET ct ON dl.CT_Num = ct.CT_Num
				LEFT JOIN F_COLLABORATEUR co ON ct.CO_No = co.CO_No

				WHERE dl.DO_Type in (3,4,5,6,7) AND dl.DL_Valorise = 1

				GROUP BY 
				ct.CT_Intitule,
				co.CO_Prenom + ' ' +co.CO_Nom,
				ct.CT_Ville,
				dl.DO_Date,
				dl.CT_Num,
				dl.DO_Piece,
				do.DO_TxEscompte,
				YEAR(dl.DO_Date),
				MONTH(dl.DO_Date),
				dl.DO_Type

				) a

				--Reglements
				LEFT JOIN 
				(
				SELECT
				ec.DO_Piece,
				ec.DO_Type,
				sum(ec.RC_Montant) Reglement

				FROM F_REGLECH ec

				GROUP BY ec.DO_Piece,
				ec.DO_Type
				) b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type

				) a

				WHERE (a.Reste > 1 or a.Reste <-1)


				GROUP BY a.CT_Num,a.CT_Intitule,a.CO_Nom,a.CT_Ville;
GO");

            migrationBuilder.Sql(@"IF OBJECT_ID('[API_V_BALANCEAGEEFOURNISSEUR]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_BALANCEAGEEFOURNISSEUR];
                                            END
                                            GO

CREATE VIEW [dbo].[API_V_BALANCEAGEEFOURNISSEUR]
				AS
				SELECT 
				a.CT_Num,
				a.CT_Intitule,
				ISNULL(a.CO_Nom,'') CO_Nom,
				ISNULL(a.CT_Ville,'') CT_Ville,
				sum(a.Reste) Encours,
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) > 150 THEN a.Reste ELSE 0 END) AN,
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 30  THEN a.Reste ELSE 0 END) '30J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 60 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 30 THEN a.Reste ELSE 0 END) '60J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 90 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 60 THEN a.Reste ELSE 0 END) '90J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 120 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 90 THEN a.Reste ELSE 0 END) '120J',
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 150 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 120 THEN a.Reste ELSE 0 END) '150J'

				FROM

				(SELECT 
				a.*,
				isnull(b.Reglement,0) Reglement,
				isnull(a.Montant,0) - isnull(b.Reglement,0) Reste,
				CASE WHEN (a.Montant - isnull(b.Reglement,0)) = 0 THEN 'Reglé' ELSE 'Non réglé' END Etat
				FROM
				(

				--Documents
				SELECT
				dl.CT_Num,
				ct.CT_Intitule,
				co.CO_Prenom + ' ' +co.CO_Nom CO_Nom,
				ct.CT_Ville,
				dl.DO_Date,
				dl.DO_Piece,
				dl.DO_Type,
				YEAR(dl.DO_Date) Annee,
				MONTH(dl.DO_Date) Mois,
				sum(CASE WHEN dl.DO_Type in (14,15) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END) - (sum(CASE WHEN dl.DO_Type in (14,15) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END))*do.DO_TxEscompte/100 Montant 


				FROM F_DOCLIGNE dl
				INNER JOIN F_DOCENTETE do ON dl.DO_Piece = do.DO_Piece AND dl.DO_Type = do.DO_Type
				INNER JOIN F_COMPTET ct ON dl.CT_Num = ct.CT_Num
				LEFT JOIN F_COLLABORATEUR co ON ct.CO_No = co.CO_No

				WHERE dl.DO_Type in (13,14,15,16,17) AND dl.DL_Valorise = 1

				GROUP BY 
				ct.CT_Intitule,
								co.CO_Prenom + ' ' +co.CO_Nom,
				ct.CT_Ville,
				dl.DO_Date,
				dl.CT_Num,
				dl.DO_Piece,
				do.DO_TxEscompte,
				YEAR(dl.DO_Date),
				MONTH(dl.DO_Date),
				dl.DO_Type

				) a

				--Reglements
				LEFT JOIN 
				(
				SELECT
				ec.DO_Piece,
				ec.DO_Type,
				sum(ec.RC_Montant) Reglement

				FROM F_REGLECH ec

				GROUP BY ec.DO_Piece,
				ec.DO_Type
				) b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type

				) a

				WHERE (a.Reste > 1 or a.Reste <-1)


				GROUP BY a.CT_Num,a.CT_Intitule,a.CO_Nom,a.CT_Ville;
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
