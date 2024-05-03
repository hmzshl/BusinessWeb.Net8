using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_201023_1530 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_BALANCEAGEEFOURNISSEUR]
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
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1

				WHERE dl.DL_Valorise = 1
				AND (con.BL_BalanceFournisseur = 1 AND dl.DO_Type IN (13,14,15,16,17) ) OR (con.BL_BalanceFournisseur = 0 AND dl.DO_Type IN (16,17))

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
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_MARGE]
				AS
				SELECT 
				FORMAT(YEAR(a.DO_Date),'0000') Annee,
				'M'+FORMAT(MONTH(a.DO_Date),'00') Mois,
				FORMAT(YEAR(a.DO_Date),'0000')+'/'+FORMAT(MONTH(a.DO_Date),'00') MoisAnnee,
				a.DO_Piece,
				a.DO_Date,
				ct.CT_Num,
				ct.CT_Intitule,
				ar.AR_Ref,
				ar.AR_Design,
				a.DL_Qte,
				a.DL_PrixUnitaire PU,
				a.DL_Remise01REM_Valeur Remise1,
				a.DL_Remise02REM_Valeur Remise2,
				a.DL_Remise03REM_Valeur Remise3,
				CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantHT/a.DL_Qte ELSE 0 END PUNet,
				a.DL_MontantHT,
				a.DL_Taxe1,
				a.DL_MontantTTC-a.DL_MontantHT DL_MontantTVA,
				a.DL_MontantTTC,

				pu.CMUP,
				a.DL_Qte * pu.CMUP CMUPCoutTotal,
				CASE WHEN pu.CMUP != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.CMUP) ELSE 0 END CMUPMarge,
				CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.CMUP != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.CMUP) ELSE 0 END)/a.DL_MontantHT ELSE 0 END CMUPMargeP,


				pu.DernierPUAchat,
				a.DL_Qte * pu.DernierPUAchat DPCoutTotal,
				CASE WHEN pu.DernierPUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.DernierPUAchat) ELSE 0 END DPMarge,
				CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.DernierPUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.DernierPUAchat) ELSE 0 END)/a.DL_MontantHT ELSE 0 END DPMargeP,

				pu.PUAchat,
				a.DL_Qte * pu.PUAchat PACoutTotal,
				CASE WHEN pu.PUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.PUAchat) ELSE 0 END PAMarge,
				CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.PUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.PUAchat) ELSE 0 END)/a.DL_MontantHT ELSE 0 END PAMargeP,

				co.CO_Nom


				FROM F_DOCLIGNE a
				LEFT JOIN 
				(
				SELECT 
				a.AR_Ref,
				CASE WHEN a.AR_PrixAch = 0 THEN ISNULL(dp.PU,0) ELSE a.AR_PrixAch END PUAchat,
				ISNULL(dp.PU,a.AR_PrixAch) DernierPUAchat,
				ISNULL(cm.CMUP,a.AR_PrixAch) CMUP,
				CASE 2 
				WHEN 0 THEN CASE WHEN a.AR_PrixAch = 0 THEN ISNULL(dp.PU,0) ELSE a.AR_PrixAch END
				WHEN 1 THEN ISNULL(dp.PU,a.AR_PrixAch)
				WHEN 2 THEN ISNULL(cm.CMUP,a.AR_PrixAch) END PU
				FROM F_ARTICLE a
				LEFT JOIN 
				(
				SELECT
				a.AR_Ref,
				CASE WHEN sum(a.DL_Qte) != 0 THEN sum(a.DL_MontantHT)/sum(a.DL_Qte) ELSE 0 END CMUP
				FROM F_DOCLIGNE a
				INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
				WHERE a.DO_Type in (20,13,16,17)
				AND a.DL_MontantHT != 0
				GROUP BY
				a.AR_Ref
				) cm ON a.AR_Ref = cm.AR_Ref
				LEFT JOIN 
				(
				SELECT 
				b.AR_Ref,
				CASE WHEN b.DL_Qte != 0 THEN b.DL_MontantHT/b.DL_Qte ELSE 0 END PU
				FROM
				(SELECT 
				a.AR_Ref,
				MAX(a.cbMarq) cbMArq
				FROM
				(SELECT 
				a.AR_Ref,
				a.cbMarq,
				MAX(CASE WHEN YEAR(a.DL_DateBL) > 2000 THEN a.DL_DateBL ELSE a.DO_Date END) DL_DateBL
				FROM F_DOCLIGNE a
				INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
				WHERE (a.DO_Date in (13) OR (a.DO_Type in (16,17) AND b.DO_Provenance = 0)) AND a.DL_MontantHT != 0
				GROUP BY 
				a.AR_Ref,
				a.cbMarq) a
				GROUP BY
				a.AR_Ref) a
				INNER JOIN F_DOCLIGNE b ON a.cbMArq = b.cbMarq
				) dp ON a.AR_Ref = dp.AR_Ref
				) pu ON a.AR_Ref = pu.AR_Ref

				INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				LEFT JOIN F_COLLABORATEUR co ON a.CO_No = co.CO_No
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1

				WHERE a.DL_Valorise = 1 
				AND (con.BL_Marge = 1 AND a.DO_Type IN (3,4,5,6,7) ) OR (con.BL_Marge = 0 AND a.DO_Type IN (6,7))
GO");
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_RELEVE]
				AS
				SELECT 
				CASE WHEN ct.CT_Type = 0 THEN 'Clients'
				WHEN ct.CT_Type = 1 THEN 'Fournisseurs'
				ELSE 'Autres' END TypeTiers,
				a.*,
				ISNULL(a.Debit,0)-ISNULL(a.Credit,0) Solde
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
				CASE WHEN a.Reste = 0 THEN 'Réglé' ELSE CASE WHEN a.MontantRegle != 0 THEN 'Regl. Parciel' ELSE 'Non réglé' END END EtatReglement,
				CASE WHEN a.Reste = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
				DATEDIFF(DAY,do.DO_Date,GETDATE()) Jours


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
				DATEDIFF(DAY,a.RG_Date,GETDATE()) Jours

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
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num;
GO");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
