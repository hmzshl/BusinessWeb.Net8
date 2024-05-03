using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_290324_1416 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_ARTICLE]
				AS
				SELECT a.AR_Ref
					  ,a.AR_Design
					  ,a.FA_CodeFamille
					  ,a.AR_Substitut
					  ,a.AR_Raccourci
					  ,a.AR_Garantie
					  ,a.AR_UnitePoids
					  ,a.AR_PoidsNet
					  ,a.AR_PoidsBrut
					  ,a.AR_UniteVen
					  ,a.AR_PrixAch
					  ,a.AR_Coef
					  ,a.AR_PrixVen
					  ,a.AR_PrixTTC
					  ,a.AR_Gamme1
					  ,a.AR_Gamme2
					  ,a.AR_SuiviStock
					  ,a.AR_Nomencl
					  ,a.AR_Stat01
					  ,a.AR_Stat02
					  ,a.AR_Stat03
					  ,a.AR_Stat04
					  ,a.AR_Stat05
					  ,a.AR_Escompte
					  ,a.AR_Delai
					  ,a.AR_HorsStat
					  ,a.AR_VteDebit
					  ,a.AR_NotImp
					  ,a.AR_Sommeil
					  ,a.AR_Langue1
					  ,a.AR_Langue2
					  ,a.AR_EdiCode
					  ,a.AR_CodeBarre
					  ,a.AR_CodeFiscal
					  ,a.AR_Pays
					  ,a.AR_Frais01FR_Denomination
					  ,a.AR_Frais01FR_Rem01REM_Valeur
					  ,a.AR_Frais01FR_Rem01REM_Type
					  ,a.AR_Frais01FR_Rem02REM_Valeur
					  ,a.AR_Frais01FR_Rem02REM_Type
					  ,a.AR_Frais01FR_Rem03REM_Valeur
					  ,a.AR_Frais01FR_Rem03REM_Type
					  ,a.AR_Frais02FR_Denomination
					  ,a.AR_Frais02FR_Rem01REM_Valeur
					  ,a.AR_Frais02FR_Rem01REM_Type
					  ,a.AR_Frais02FR_Rem02REM_Valeur
					  ,a.AR_Frais02FR_Rem02REM_Type
					  ,a.AR_Frais02FR_Rem03REM_Valeur
					  ,a.AR_Frais02FR_Rem03REM_Type
					  ,a.AR_Frais03FR_Denomination
					  ,a.AR_Frais03FR_Rem01REM_Valeur
					  ,a.AR_Frais03FR_Rem01REM_Type
					  ,a.AR_Frais03FR_Rem02REM_Valeur
					  ,a.AR_Frais03FR_Rem02REM_Type
					  ,a.AR_Frais03FR_Rem03REM_Valeur
					  ,a.AR_Frais03FR_Rem03REM_Type
					  ,a.AR_Condition
					  ,a.AR_PUNet
					  ,a.AR_Contremarque
					  ,a.AR_FactPoids
					  ,a.AR_FactForfait
					  ,a.AR_SaisieVar
					  ,a.AR_Transfere
					  ,a.AR_Publie
					  ,a.AR_Photo
					  ,a.AR_PrixAchNouv
					  ,a.AR_CoefNouv
					  ,a.AR_PrixVenNouv
					  ,a.AR_CoutStd
					  ,a.AR_QteComp
					  ,a.AR_QteOperatoire
					  ,a.CO_No
					  ,a.AR_Prevision
					  ,a.CL_No1
					  ,a.CL_No2
					  ,a.CL_No3
					  ,a.CL_No4
					  ,a.AR_Type
					  ,a.RP_CodeDefaut
					  ,a.AR_Nature
					  ,a.AR_DelaiFabrication
					  ,a.AR_NbColis
					  ,a.AR_DelaiPeremption
					  ,a.AR_DelaiSecurite
					  ,a.AR_Fictif
					  ,a.AR_SousTraitance
					  ,a.AR_TypeLancement
					  ,a.cbMarq
					  ,fa.FA_Intitule
					  ,un.U_Intitule
					  ,ve.DL_Qte_Vente
					  ,ve.DL_MontantHT_Vente
					  ,ve.DL_MontantTTC_Vente
					  ,ac.DL_Qte_Achat
					  ,ac.DL_MontantHT_Achat
					  ,ac.DL_MontantTTC_Achat
					  ,dve.CL_Intitule
					  ,dve.CL_Num
					  ,dve.DL_DateBL_Vente
					  ,dve.DL_Dernier_MontantHT_Vente
					  ,dve.DL_Dernier_MontantTTC_Vente
					  ,dve.DL_PU_Vente 
					  ,dve.DL_Dernier_Qte_Vente
					  ,dac.FR_Intitule
					  ,dac.FR_Num
					  ,dac.DL_DateBL_Achat
					  ,dac.DL_Dernier_MontantHT_Achat
					  ,dac.DL_Dernier_MontantTTC_Achat
					  ,dac.DL_PU_Achat 
					  ,dac.DL_Dernier_Qte_Achat 
					  ,CASE a.AR_SuiviStock WHEN 0 THEN 'Aucun' WHEN 1 THEN 'Série' WHEN 2 THEN 'CMUP' WHEN 3 THEN 'FIFO' WHEN 4 THEN 'LIFO' WHEN 5 THEN 'Lot' END SuiviStockIntitule
					  ,CASE WHEN a.AR_Sommeil = 0 THEN 'Actif' ELSE 'En sommeil' END SommeilIntitule
					  ,sto.AS_QteSto
					  ,sto.AS_MontSto
					  ,CASE WHEN ISNULL(sto.AS_QteSto,0) > 0 THEN 'En stock' ELSE 'Stock épuisé' END EtatStock
					  ,sto.AS_QteMini
					  ,sto.AS_QteMaxi
					  ,sto.AS_QteCom
					  ,sto.AS_QtePrepa
					  ,sto.AS_QteRes
					  ,cd.EC_Enumere
					  ,CASE WHEN cd.EC_Quantite != 0 THEN sto.AS_QteSto / cd.EC_Quantite ELSE 0 END QteCD,
					  CASE WHEN sto.AS_QteSto > sto.AS_QteMaxi THEN 'En stock' WHEN sto.AS_QteSto <= sto.AS_QteMini THEN 'En rupture' ELSE 'A surveiller' END EtatStockMin



	  
				  FROM F_ARTICLE a 
				  INNER JOIN F_FAMILLE fa ON a.FA_CodeFamille = fa.FA_CodeFamille
				  LEFT JOIN P_UNITE un ON a.AR_UniteVen = un.cbIndice
				  LEFT JOIN 
				  (
				  SELECT 
					a.AR_Ref,
					SUM(a.DL_Qte) DL_Qte_Vente,
					SUM(a.DL_MontantHT) DL_MontantHT_Vente,
					SUM(a.DL_MontantTTC) DL_MontantTTC_Vente
					FROM F_DOCLIGNE a 
					WHERE a.DL_Valorise = 1 AND a.DO_Type in (3,4,5,6,7)
					GROUP BY a.AR_Ref
				  ) ve ON a.AR_Ref = ve.AR_Ref

					LEFT JOIN 
				  (
				  SELECT 
					a.AR_Ref,
					SUM(a.DL_Qte) DL_Qte_Achat,
					SUM(a.DL_MontantHT) DL_MontantHT_Achat,
					SUM(a.DL_MontantTTC) DL_MontantTTC_Achat
					FROM F_DOCLIGNE a 
					WHERE a.DL_Valorise = 1 AND a.DO_Type in (13,14,15,16,17)
					GROUP BY a.AR_Ref
				  ) ac ON a.AR_Ref = ac.AR_Ref

				  LEFT JOIN

					  (
					  SELECT 
					a.AR_Ref,
					b.DL_DateBL DL_DateBL_Vente,
					ct.CT_Num CL_Num,
					ct.CT_Intitule CL_Intitule,
					a.DL_Dernier_Qte_Vente,
					a.DL_Dernier_MontantHT_Vente,
					a.DL_Dernier_MontantTTC_Vente,
					b.DL_PrixUnitaire DL_PU_Vente

					FROM
					(SELECT 
					a.AR_Ref,
					MAX(b.cbMarq) cbMarq,
					SUM(b.DL_Qte) DL_Dernier_Qte_Vente,
					SUM(b.DL_MontantHT) DL_Dernier_MontantHT_Vente,
					SUM(b.DL_MontantTTC) DL_Dernier_MontantTTC_Vente
					FROM
					(SELECT 
					a.AR_Ref,
					MAX(a.DL_DateBL) DL_DateBL
					FROM F_DOCLIGNE a 
					WHERE a.DL_Valorise = 1 AND a.DO_Type in (3,4,5,6,7)
					GROUP BY a.AR_Ref) a
					INNER JOIN F_DOCLIGNE b ON a.AR_Ref = b.AR_Ref AND a.DL_DateBL = b.DL_DateBL
					GROUP BY a.AR_Ref) a
					INNER JOIN F_DOCLIGNE b ON a.cbMarq = b.cbMarq
					INNER JOIN F_COMPTET ct ON b.CT_Num = ct.CT_Num
					 WHERE b.DL_MontantHT > 0
				  ) dve ON a.AR_Ref = dve.AR_Ref

					LEFT JOIN

					  (
					  SELECT 
					a.AR_Ref,
					b.DL_DateBL DL_DateBL_Achat,
					ct.CT_Num FR_Num,
					ct.CT_Intitule FR_Intitule,
					a.DL_Dernier_Qte_Achat,
					a.DL_Dernier_MontantHT_Achat,
					a.DL_Dernier_MontantTTC_Achat,
					b.DL_PrixUnitaire DL_PU_Achat

					FROM
					(SELECT 
					a.AR_Ref,
					MAX(b.cbMarq) cbMarq,
					SUM(b.DL_Qte) DL_Dernier_Qte_Achat,
					SUM(b.DL_MontantHT) DL_Dernier_MontantHT_achat,
					SUM(b.DL_MontantTTC) DL_Dernier_MontantTTC_Achat
					FROM
					(SELECT 
					a.AR_Ref,
					MAX(a.DL_DateBL) DL_DateBL
					FROM F_DOCLIGNE a 
					WHERE a.DL_Valorise = 1 AND a.DO_Type in (13,14,15,16,17)
					GROUP BY a.AR_Ref) a
					INNER JOIN F_DOCLIGNE b ON a.AR_Ref = b.AR_Ref AND a.DL_DateBL = b.DL_DateBL
					GROUP BY a.AR_Ref) a
					INNER JOIN F_DOCLIGNE b ON a.cbMarq = b.cbMarq
					INNER JOIN F_COMPTET ct ON b.CT_Num = ct.CT_Num
					 WHERE b.DL_MontantHT > 0
				  ) dac ON a.AR_Ref = dac.AR_Ref


				  LEFT JOIN

				  (
						SELECT  
						a.AR_Ref,
						SUM(a.AS_QteSto) AS_QteSto,
						SUM(a.AS_MontSto) AS_MontSto,
						SUM(a.AS_QteMini) AS_QteMini,
						SUM(a.AS_QteMaxi) AS_QteMaxi,
						SUM(a.AS_QteRes) AS_QteRes,
						SUM(a.AS_QteCom) AS_QteCom,
						SUM(a.AS_QtePrepa) AS_QtePrepa
						FROM F_ARTSTOCK a
						GROUP BY 
						a.AR_Ref
				  ) sto ON a.AR_Ref = sto.AR_Ref

				  LEFT JOIN 
				  (
				  SELECT 
					a.AR_Ref,
					a.EC_Enumere,
					a.EC_Quantite
					FROM F_CONDITION a
					INNER JOIN (SELECT a.AR_Ref, MAX(a.cbMarq) cbMarq FROM F_CONDITION a GROUP BY a.AR_Ref) b ON a.cbMarq = b.cbMarq

				  ) cd ON a.AR_Ref = cd.AR_Ref


");

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
										CASE WHEN MONTH(CASE WHEN YEAR(a.RG_DateEchCont)>2000 THEN a.RG_DateEchCont ELSE a.RG_Date END) = 12 THEN a.RG_Montant ELSE 0.0000 END M12


                                            FROM F_CREGLEMENT a
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
												) rc ON a.RG_No = rc.RG_No


");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
