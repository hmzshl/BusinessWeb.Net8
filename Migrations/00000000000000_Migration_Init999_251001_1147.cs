using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_251001_1147 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"ALTER VIEW [dbo].[API_V_RELEVE]
AS
				SELECT 
				CASE WHEN ct.CT_Type = 0 THEN 'Clients'
				WHEN ct.CT_Type = 1 THEN 'Fournisseurs'
				ELSE 'Autres' END TypeTiers,
				a.*,
				ISNULL(a.Debit,0)-ISNULL(a.Credit,0) Solde,
				CASE WHEN ISNULL(a.Repre,'') = '' OR ISNULL(a.Repre,'') = ' ' THEN ISNULL(co.CO_Nom,'') + ' '+ISNULL(co.CO_Prenom,'') ELSE a.Repre END CO_Nom,
				ct.CT_Intitule,
				ct.CT_Ville,
				de.DE_Intitule,
				ca.CA_Intitule,
				CASE WHEN ISNULL(a.Repre,'') = '' OR ISNULL(a.Repre,'') = ' ' THEN ISNULL(co.CO_No,0) ELSE a.CO_No END CO_No2,
				ct.CT_Adresse,
				ct.CT_Telephone,
				ct.CT_Telecopie,
				ct.CT_EMail,
				mr.R_Intitule,
				mr.RT_NbJour,
				bl.BL,
				ech.RG_Montant,
				ct.CT_Encours,
				DATEADD(DAY,ISNULL(mr.RT_NbJour,0),a.DO_Date) DateEcheance,
				ct.CT_CodeRegion
				


				FROM
				(SELECT 
				'Document' Groupe,
				do.DO_Tiers CT_Num,
				do.DO_Date,
				do.DO_Piece,
				do.DO_Type,
				do.DO_Domaine,
				do.DO_Ref,
				'Document N° '+do.DO_Piece DO_Libelle,
				CASE WHEN a.MontantTTC > 0 THEN a.MontantTTC END Debit,
				CASE WHEN a.MontantTTC < 0 THEN -a.MontantTTC END Credit,
				a.MontantTTC,
				a.MontantRegle,
				a.Reste,
				null DO_Echeance,
				CASE WHEN do.DO_Type in (3,13) THEN 'Bon de livraison' 
				WHEN do.DO_Type in (4,5,14,15) THEN 'Bon de retour'
				WHEN do.DO_Type in (0) THEN 'Devis'
				WHEN do.DO_Type in (10) THEN 'Demande d''achat'
				WHEN do.DO_Type in (11) THEN 'Préparation de commande'
				WHEN do.DO_Type in (1,12) THEN 'Bon de commande'
				WHEN do.DO_Type in (2) THEN 'Préparation de livraison'
				WHEN do.DO_Type in (6,7,16,17) THEN CASE WHEN do.DO_Provenance = 0 THEN 'Facture' ELSE 'Facture Avoir' END
				END TypeIntitule,
				CASE WHEN a.Reste between -1 AND 1 THEN 'Réglé' ELSE CASE WHEN a.MontantRegle != 0 THEN 'Regl. Parciel' ELSE 'Non réglé' END END EtatReglement,
				CASE WHEN a.Reste = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
				DATEDIFF(DAY,do.DO_Date,GETDATE()) Jours,
				do.CO_No,
				do.DE_No,
				do.CA_Num,
				ISNULL(co.CO_Nom,'') + ' '+ISNULL(co.CO_Prenom,'') Repre,
				0 cbMarq,
				0 RG_No


				FROM
				(SELECT
				a.DO_Piece,
				a.DO_Type,
				a.DL_MontantTTC MontantTTC,
				ISNULL(b.MontantRegle,0) MontantRegle,
				a.DL_MontantTTC - ISNULL(b.MontantRegle,0) Reste
				FROM
				(SELECT
				a.DO_Type,
				a.DO_Piece,
				sum(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.DL_MontantTTC) ELSE a.DL_MontantTTC END) DL_MontantTTC
				FROM F_DOCLIGNE a
				WHERE a.DL_Valorise = 1
				AND a.DO_Domaine in (0,1) AND a.DO_Type in (3,4,5,6,7,13,14,15,16,17)
				GROUP BY
				a.DO_Type,
				a.DO_Piece) a
				LEFT JOIN 
				(
				SELECT 
				a.DO_Type,
				a.DO_Piece,
				SUM(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.RC_Montant) ELSE a.RC_Montant END) MontantRegle
				FROM F_REGLECH a
				GROUP BY
				a.DO_Type,
				a.DO_Piece) b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type) a
				LEFT JOIN F_DOCENTETE do ON a.DO_Piece = do.DO_Piece AND a.DO_Type = do.DO_Type
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				LEFT JOIN F_COLLABORATEUR co ON do.CO_No = co.CO_No

				WHERE (con.BL_Releve = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17) ) OR (con.BL_Releve = 0 AND a.DO_Type IN (6,7,16,17))

				UNION

				SELECT 
				'Réglement' Groupe,
				a.CT_NumPayeur CT_Num,
				a.RG_Date DO_Date,
				a.RG_Piece DO_Piece,
				null DO_Type,
				null DO_Domaine,
				a.RG_Reference DO_Ref,
				a.RG_Libelle DO_Libelle,
				CASE WHEN (a.RG_Montant) < 0 THEN -a.RG_Montant END  Debit,
				CASE WHEN (a.RG_Montant) > 0 THEN a.RG_Montant END Credit,
				a.RG_Montant MontantTTC,
				ISNULL(rc.RC_Montant,0) MontantRegle,
				a.RG_Montant - ISNULL(rc.RC_Montant,0) Reste,
				CASE WHEN YEAR(a.RG_DateEchCont)>2000 AND ISNULL(rg.R_Intitule,'Autre') not like 'CH%' THEN a.RG_DateEchCont END DO_Echeance,
				ISNULL(rg.R_Intitule,'Autre') TypeIntitule,
				CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Affécté' ELSE CASE WHEN ISNULL(rc.RC_Montant,0) != 0 THEN 'Aff. Parciel' ELSE 'Non Affecté' END END EtatReglement,
				CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
				DATEDIFF(DAY,a.RG_Date,GETDATE()) Jours,
				0 CO_No,
				0 DE_No,
				'' CA_Num,
				null CO_Nom,
				a.cbMarq,
				a.RG_No

				FROM F_CREGLEMENT a
				LEFT JOIN
				(
				SELECT 
				a.RG_No,
				SUM(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.RC_Montant) ELSE a.RC_Montant END) RC_Montant
				FROM F_REGLECH a
				GROUP BY
				a.RG_No
				) rc ON a.RG_No = rc.RG_No
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) a
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				LEFT JOIN F_COLLABORATEUR co ON ct.CO_No = co.CO_No
				LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				LEFT JOIN (SELECT a.CT_Num, SUM(a.DL_MontantTTC) BL FROM F_DOCLIGNE a WHERE a.DO_Type IN (13,14,15,3,4,5) AND a.DL_Valorise = 1 GROUP BY CT_Num) bl ON a.CT_Num = bl.CT_Num
				LEFT JOIN (SELECT a.CT_NumPayeur, SUM(a.RG_Montant) RG_Montant FROM F_CREGLEMENT a WHERE a.RG_DateEchCont > GETDATE() GROUP BY CT_NumPayeur) ech ON a.CT_Num = ech.CT_NumPayeur
				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				a.RT_NbJour,
				rg.R_Intitule
				FROM F_REGLEMENTT a
				INNER JOIN (SELECT a.CT_Num,MIN(a.cbMarq) cbMarq FROM F_REGLEMENTT a GROUP BY a.CT_Num) b ON a.cbMarq = b.cbMarq
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) mr ON a.CT_Num = mr.CT_Num";
			migrationBuilder.Sql(q1);


            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_BALANCEAGEECLIENT]
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
				sum(CASE WHEN DATEDIFF(DAY,a.DO_Date,GETDATE()) <= 150 AND DATEDIFF(DAY,a.DO_Date,GETDATE()) > 120 THEN a.Reste ELSE 0 END) '150J',
				ISNULL(a.CT_CodeRegion,'') Region

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
				ct.CT_CodeRegion,
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
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				WHERE dl.DL_Valorise = 1
				AND (con.BL_BalanceClient = 1 AND dl.DO_Type IN (3,4,5,6,7) ) OR (con.BL_BalanceClient = 0 AND dl.DO_Type IN (6,7))
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
				dl.DO_Type,
				ct.CT_CodeRegion

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


				GROUP BY a.CT_Num,a.CT_Intitule,a.CO_Nom,a.CT_Ville,a.CT_CodeRegion;");

            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_CREGLEMENT]
                                            AS
                                            SELECT 
                                            a.cbMarq,
                                            a.RG_No,
                                            a.RG_Date,
                                            a.RG_Piece,
                                            a.RG_Reference,
                                            a.RG_Libelle,
                                            ct.CT_Num,
                                            ct.CT_Intitule,
                                            jr.JO_Num,
                                            jr.JO_Intitule,
                                            CASE WHEN YEAR(a.RG_DateEchCont) > 2000 THEN FORMAT(a.RG_DateEchCont,'dd/MM/yyyy') END RG_DateEcheance,
                                            CASE WHEN a.RG_DateEchCont>GETDATE() THEN 'Non echu' ELSE 'Echu' END Echeance,
                                            ISNULL(mo.R_Intitule,'Autre') R_Intitule,
                                            CASE WHEN ct.CT_Type = 0 THEN 'Client' ELSE 'Fournisseur' END CT_Type,
                                            a.RG_Montant,
                                            CASE WHEN ct.CT_Type = 0 THEN a.RG_Montant END Debit,
                                            CASE WHEN ct.CT_Type = 1 THEN a.RG_Montant END Credit,
                                            a.RG_Type,
                                            a.Encaiss,
                                            a.Impaye,
                                            a.Incorpore,
                                            a.Depose,
                                            a.Rappro,
                                            a.DateDepot,
                                            a.DatePaie,
                                            a.Remarque,
                                            jr.CG_Num,
											FORMAT(YEAR(a.RG_Date),'0000') Annee,
											'M'+FORMAT(MONTH(a.RG_Date),'00') Mois,
											FORMAT(YEAR(a.RG_Date),'0000')+'/'+FORMAT(MONTH(a.RG_Date),'00') MoisAnnee,
											a.RG_Cloture,
															CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Affécté' ELSE CASE WHEN ISNULL(rc.RC_Montant,0) != 0 THEN 'Aff. Parciel' ELSE 'Non Affecté' END END EtatReglement,
										CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
										CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END DateEcheance,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 1 THEN a.RG_Montant ELSE 0.0000 END M01,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 2 THEN a.RG_Montant ELSE 0.0000 END M02,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 3 THEN a.RG_Montant ELSE 0.0000 END M03,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 4 THEN a.RG_Montant ELSE 0.0000 END M04,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 5 THEN a.RG_Montant ELSE 0.0000 END M05,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 6 THEN a.RG_Montant ELSE 0.0000 END M06,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 7 THEN a.RG_Montant ELSE 0.0000 END M07,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 8 THEN a.RG_Montant ELSE 0.0000 END M08,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 9 THEN a.RG_Montant ELSE 0.0000 END M09,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 10 THEN a.RG_Montant ELSE 0.0000 END M10,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 11 THEN a.RG_Montant ELSE 0.0000 END M11,
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 12 THEN a.RG_Montant ELSE 0.0000 END M12,
										null PROT_User,
										a.cbCreation,
										a.cbModification,
										ct.CT_CodeRegion,
										ct.CT_Ville


                                            FROM F_CREGLEMENT a
											--LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
                                            LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
                                            INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
                                            INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
															LEFT JOIN
												(
												SELECT 
												a.RG_No,
												SUM(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.RC_Montant) ELSE a.RC_Montant END) RC_Montant
												FROM F_REGLECH a
												GROUP BY
												a.RG_No
												) rc ON a.RG_No = rc.RG_No");

        }

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
