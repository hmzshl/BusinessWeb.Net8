using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_251009_1025 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifDechargeLigne", "AR_Ref","100"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifReceptionLigne", "AR_Ref", "100"));
			migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_ACHAT]
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
				CASE WHEN co_dl.CO_Nom IS NULL THEN CASE WHEN co_do.CO_Nom IS NULL THEN ISNULL(co_ct.CO_Nom,'') + ' ' + ISNULL(co_ct.CO_Prenom,'')
				ELSE ISNULL(co_do.CO_Nom,'') + ' ' + ISNULL(co_do.CO_Prenom,'') END
				ELSE ISNULL(co_dl.CO_Nom,'') + ' ' + ISNULL(co_dl.CO_Prenom,'')
				END CO_Nom,
				fm.FA_CodeFamille,
				fm.FA_Intitule,
				CASE WHEN co_dl.CO_No IS NULL THEN CASE WHEN co_do.CO_No IS NULL THEN ISNULL(co_ct.CO_No,'')
				ELSE ISNULL(co_do.CO_No,'') END
				ELSE ISNULL(co_dl.CO_No,'')
				END CO_No,
				ISNULL(de.DE_Intitule,de_e.DE_Intitule) DE_Intitule,
				a.DL_MvtStock,
				ISNULL(ca_dl.CA_Num,ca_do.CA_Num) CA_Num,
				ISNULL(ca_dl.CA_Intitule,ca_do.CA_Intitule) CA_Intitule,
				    'SM-' + 
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 1-DATEPART(WEEKDAY, b.DO_Date), b.DO_Date), 12), 6) + '-' +
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 7-DATEPART(WEEKDAY, b.DO_Date), b.DO_Date), 12), 6) AS Semaine

				FROM F_DOCLIGNE a



				INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				LEFT JOIN F_COLLABORATEUR co_dl ON a.CO_No = co_dl.CO_No
				LEFT JOIN F_COLLABORATEUR co_do ON a.CO_No = co_do.CO_No
				LEFT JOIN F_COLLABORATEUR co_ct ON a.CO_No = co_ct.CO_No
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				LEFT JOIN F_FAMILLE fm ON ar.FA_CodeFamille = fm.FA_CodeFamille
				LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
								LEFT JOIN F_DEPOT de_e ON b.DE_No = de_e.DE_No
				LEFT JOIN F_COMPTEA ca_dl ON a.CA_Num = ca_dl.CA_Num
				LEFT JOIN F_COMPTEA ca_do ON b.CA_Num = ca_do.CA_Num

				WHERE a.DL_Valorise = 1 
				AND a.DO_Type IN (13,14,15,16,17)");
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
										a.cbCreation,
										a.cbModification,
										ct.CT_CodeRegion,
										ct.CT_Ville,
														    'SM-' + 
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 1-DATEPART(WEEKDAY, a.RG_Date), a.RG_Date), 12), 6) + '-' +
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 7-DATEPART(WEEKDAY, a.RG_Date), a.RG_Date), 12), 6) AS Semaine


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
												) rc ON a.RG_No = rc.RG_No");
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_DOCENTETE]
AS
SELECT 
		a.cbMarq
      ,a.DO_Domaine
      ,a.DO_Type
      ,a.DO_Piece
      ,a.DO_Date
      ,a.DO_Ref
      ,a.DO_Tiers
      ,a.CO_No
      ,a.DO_Period
      ,a.DO_Devise
      ,a.DO_Cours
      ,a.DE_No
      ,a.LI_No
      ,a.CT_NumPayeur
      ,a.DO_Expedit
      ,a.DO_NbFacture
      ,a.DO_BLFact
      ,a.DO_TxEscompte
      ,a.DO_Reliquat
      ,a.DO_Imprim
      ,a.CA_Num
      ,a.DO_Coord01
      ,a.DO_Coord02
      ,a.DO_Coord03
      ,a.DO_Coord04
      ,a.DO_Souche
      ,a.DO_DateLivr
      ,a.DO_Condition
      ,a.DO_Tarif
      ,a.DO_Colisage
      ,a.DO_TypeColis
      ,a.DO_Transaction
      ,a.DO_Langue
      ,a.DO_Ecart
      ,a.DO_Regime
      ,a.N_CatCompta
      ,a.DO_Ventile
      ,a.AB_No
      ,a.DO_DebutAbo
      ,a.DO_FinAbo
      ,a.DO_DebutPeriod
      ,a.DO_FinPeriod
      ,a.CG_Num
      ,a.DO_Statut
      ,a.DO_Heure
      ,a.CA_No
      ,a.CO_NoCaissier
      ,a.DO_Transfere
      ,a.DO_Cloture
      ,a.DO_NoWeb
      ,a.DO_Attente
      ,a.DO_Provenance
      ,a.CA_NumIFRS
      ,a.MR_No
      ,a.DO_TypeFrais
      ,a.DO_ValFrais
      ,a.DO_TypeLigneFrais
      ,a.DO_TypeFranco
      ,a.DO_ValFranco
      ,a.DO_TypeLigneFranco
      ,a.DO_Taxe1
      ,a.DO_TypeTaux1
      ,a.DO_TypeTaxe1
      ,a.DO_Taxe2
      ,a.DO_TypeTaux2
      ,a.DO_TypeTaxe2
      ,a.DO_Taxe3
      ,a.DO_TypeTaux3
      ,a.DO_TypeTaxe3
      ,a.DO_MajCpta
      ,a.DO_Motif
      ,a.CT_NumCentrale
      ,a.DO_Contact
      ,a.DO_FactureElec
      ,a.DO_TypeTransac
      ,a.DO_DateLivrRealisee
      ,a.DO_DateExpedition
      ,a.DO_FactureFrs
      ,a.DO_PieceOrig
      ,a.DO_DemandeRegul
      ,a.ET_No
      ,a.DO_Valide
      ,a.DO_Coffre
      ,a.DO_CodeTaxe1
      ,a.DO_CodeTaxe2
      ,a.DO_CodeTaxe3
      ,a.DO_TotalHT
	  ,ct.CT_Intitule
	  ,ca.CA_Intitule
	  ,cl.CO_Nom
	  ,de.DE_Intitule
	  ,cg.CG_Intitule
	  ,tf.TF_Intitule
	  ,ISNULL(dl.DL_MontantHT,0) DL_MontantHT
	  ,ISNULL(dl.DL_MontantTTC,0) DL_MontantTTC
	  ,ISNULL(dl.DL_MontantTTC,0)-ISNULL(dl.DL_MontantHT,0) DL_MontantTVA
	  ,ISNULL(rg.RC_Montant,0) RC_Montant
	  ,ISNULL(dl.DL_MontantTTC,0) - ISNULL(rg.RC_Montant,0) Reste
	  ,CASE WHEN dl.DL_MontantTTC - ISNULL(rg.RC_Montant,0) = 0 THEN 'Réglé' ELSE 'Non réglé' END EtatReglement
	  ,CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule
	  ,ISNULL(di.D_Intitule,CASE WHEN a.DO_Domaine = 40 THEN 'D.Interne' ELSE 
	  CASE a.DO_Type WHEN 0 THEN 'DE.Client' WHEN 1 THEN 'BC.Client' WHEN 2 THEN 'PL.Client' WHEN 3 THEN 'BL.Client' WHEN 4 THEN 'BR.Client' WHEN 5 THEN 'BR.Client'
	  WHEN 6 THEN 'FA.Client' WHEN 7 THEN 'FC.Client' WHEN 10 THEN 'DA.Fournisseur' WHEN 11 THEN 'PR.Fournisseur' WHEN 12 THEN 'BC.Fournisseur' WHEN 13 THEn 'BL.Fournisseur'
	  WHEN 14 THEN 'BR.Fournisseur' WHEN 15 THEN 'BR.Fournisseur' WHEN 16 THEN 'FA.Fournisseur' WHEN 17 THEN 'FC.Fournisseur'
	  WHEN 20 THEN 'M.Entrée' WHEN 21 THEN 'M.Sortie' WHEN 23 THEN 'M.Transfert' ELSE 'A.Document' END END) TypeIntitule
	  ,pj.Objet
	  ,pj.NumeroMarche
	  ,CASE WHEN cl.CO_No IS NULL THEN cl_ct.CO_No ELSE cl.CO_No END CO_No2
	  ,ec.RT_NbJour
	  ,DATEADD(DAY,ISNULL(ec.RT_NbJour,0),a.DO_Date) DateEcheance
	  ,CASE WHEN DATEADD(DAY,ISNULL(ec.RT_NbJour,0),a.DO_Date)<= GETDATE() THEN 'Echu' ELSE 'Non echu' END Echeance,
	  a.cbCreation,
		a.cbModification,
		a.DO_TotalTTC,
		a.DO_NetAPayer,
		a.DO_TotalHTNet,
						    'SM-' + 
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 1-DATEPART(WEEKDAY, a.DO_Date), a.DO_Date), 12), 6) + '-' +
    RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 7-DATEPART(WEEKDAY, a.DO_Date), a.DO_Date), 12), 6) AS Semaine
	



  FROM F_DOCENTETE a
  LEFT JOIN P_INTERNE di ON a.DO_Type = 39+di.cbIndice AND a.DO_Domaine = 4
  LEFT JOIN API_T_Projet pj ON a.CA_Num = pj.CA_Num AND ISNULL(a.CA_Num,'') != ''
  LEFT JOIN F_COMPTET ct ON a.DO_Tiers = ct.CT_Num
  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
  LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No
  				  LEFT JOIN F_COLLABORATEUR cl_ct ON ct.CO_No = cl_ct.CO_No
  LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num
  LEFT JOIN F_TARIF tf ON a.DO_Tarif = tf.TF_No
  LEFT JOIN (
  SELECT a.DO_Piece, a.DO_Type, sum(a.DL_MontantHT) DL_MontantHT, sum(a.DL_MontantTTC) DL_MontantTTC FROM F_DOCLIGNE a WHERE a.DL_Valorise = 1 GROUP BY a.DO_Piece, a.DO_Type
  ) dl ON a.DO_Piece = dl.DO_Piece AND a.DO_Type = dl.DO_Type
  LEFT JOIN (SELECT a.DO_Type, a.DO_Piece, sum(a.RC_Montant) RC_Montant FROM F_REGLECH a GROUP BY a.DO_Type,a.DO_Piece) rg ON a.DO_Piece = rg.DO_Piece AND a.DO_Type = rg.DO_Type

  				LEFT JOIN

				(
				SELECT 
				a.CT_Num,
				a.RT_NbJour,
				rg.R_Intitule
				FROM F_REGLEMENTT a
				INNER JOIN
				(SELECT 
				a.CT_Num,
				MAX(a.cbMarq) cbMarq
				FROM F_REGLEMENTT a
				GROUP BY a.CT_Num) b ON a.cbMarq = b.cbMarq
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) ec ON ct.CT_Num = ec.CT_Num");
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_DOCLIGNE]
                                            AS
                                            SELECT  a.cbMarq
                                                  ,a.DO_Domaine
                                                  ,a.DO_Type
                                                  ,a.CT_Num
                                                  ,a.DO_Piece
                                                  ,a.DL_PieceBC
                                                  ,a.DL_PieceBL
                                                  ,a.DO_Date
                                                  ,a.DL_DateBC
                                                  ,a.DL_DateBL
                                                  ,a.DL_Ligne
                                                  ,a.DO_Ref
                                                  ,a.DL_TNomencl
                                                  ,a.DL_TRemPied
                                                  ,a.DL_TRemExep
                                                  ,a.AR_Ref
                                                  ,a.DL_Design
                                                  ,a.DL_Qte
                                                  ,a.DL_QteBC
                                                  ,a.DL_QteBL
                                                  ,a.DL_PoidsNet
                                                  ,a.DL_PoidsBrut
                                                  ,a.DL_Remise01REM_Valeur
                                                  ,a.DL_Remise01REM_Type
                                                  ,a.DL_Remise02REM_Valeur
                                                  ,a.DL_Remise02REM_Type
                                                  ,a.DL_Remise03REM_Valeur
                                                  ,a.DL_Remise03REM_Type
                                                  ,a.DL_PrixUnitaire
                                                  ,a.DL_PUBC
                                                  ,a.DL_Taxe1
                                                  ,a.DL_TypeTaux1
                                                  ,a.DL_TypeTaxe1
                                                  ,a.DL_Taxe2
                                                  ,a.DL_TypeTaux2
                                                  ,a.DL_TypeTaxe2
                                                  ,a.CO_No
                                                  ,a.AG_No1
                                                  ,a.AG_No2
                                                  ,a.DL_PrixRU
                                                  ,a.DL_CMUP
                                                  ,a.DL_MvtStock
                                                  ,a.DT_No
                                                  ,a.AF_RefFourniss
                                                  ,a.EU_Enumere
                                                  ,a.EU_Qte
                                                  ,a.DL_TTC
                                                  ,a.DE_No
                                                  ,a.DL_NoRef
                                                  ,a.DL_TypePL
                                                  ,a.DL_PUDevise
                                                  ,a.DL_PUTTC
                                                  ,a.DL_No
                                                  ,a.DO_DateLivr
                                                  ,a.CA_Num
                                                  ,a.DL_Taxe3
                                                  ,a.DL_TypeTaux3
                                                  ,a.DL_TypeTaxe3
                                                  ,a.DL_Frais
                                                  ,a.DL_Valorise
                                                  ,a.AR_RefCompose
                                                  ,a.DL_NonLivre
                                                  ,a.AC_RefClient
                                                  ,a.DL_MontantHT
                                                  ,a.DL_MontantTTC
                                                  ,a.DL_FactPoids
                                                  ,a.DL_Escompte
                                                  ,a.DL_PiecePL
                                                  ,a.DL_DatePL
                                                  ,a.DL_QtePL
                                                  ,a.DL_NoColis
                                                  ,a.DL_NoLink
                                                  ,a.RP_Code
                                                  ,a.DL_QteRessource
                                                  ,a.DL_DateAvancement
                                                  ,a.PF_Num
                                                  ,a.DL_CodeTaxe1
                                                  ,a.DL_CodeTaxe2
                                                  ,a.DL_CodeTaxe3
	                                              ,ct.CT_Intitule,
	                                              de.DE_Intitule,
	                                              CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule,
	                                              ISNULL(di.D_Intitule,CASE WHEN a.DO_Domaine = 40 THEN 'D.Interne' ELSE 
												  CASE a.DO_Type WHEN 0 THEN 'DE.Client' WHEN 1 THEN 'BC.Client' WHEN 2 THEN 'PL.Client' WHEN 3 THEN 'BL.Client' WHEN 4 THEN 'BR.Client' WHEN 5 THEN 'BR.Client'
												  WHEN 6 THEN 'FA.Client' WHEN 7 THEN 'FC.Client' WHEN 10 THEN 'DA.Fournisseur' WHEN 11 THEN 'PR.Fournisseur' WHEN 12 THEN 'BC.Fournisseur' WHEN 13 THEn 'BL.Fournisseur'
												  WHEN 14 THEN 'BR.Fournisseur' WHEN 15 THEN 'BR.Fournisseur' WHEN 16 THEN 'FA.Fournisseur' WHEN 17 THEN 'FC.Fournisseur'
												  WHEN 20 THEN 'M.Entrée' WHEN 21 THEN 'M.Sortie' WHEN 23 THEN 'M.Transfert' ELSE 'A.Document' END END) TypeIntitule,
	                                              ca.CA_Intitule,
	                                              cl.CO_Nom,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantTTC/a.DL_Qte ELSE 0 END PUTTC,
	                                              a.DL_MontantTTC-a.DL_MontantHT MontantTVA,
	                                              a.DL_Remise01REM_Valeur Remise,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantHT/a.DL_Qte ELSE 0 END PUNet,
	                                              CASE WHEN a.DL_MvtStock = 1 THEN a.DL_Qte WHEN a.DL_MvtStock = 3 THEN -a.DL_Qte ELSE 0 END QteMvt,
	                                              CAST(CASE WHEN a.DL_MvtStock = 1 THEN 1 WHEN a.DL_MvtStock = 3 THEN 1 ELSE 0 END AS BIT) IsStock,
													a.cbCreation,
													a.cbModification,
																	    'SM-' + 
												RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 1-DATEPART(WEEKDAY, a.DO_Date), a.DO_Date), 12), 6) + '-' +
												RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 7-DATEPART(WEEKDAY, a.DO_Date), a.DO_Date), 12), 6) AS Semaine


                                              FROM F_DOCLIGNE a
											    LEFT JOIN P_INTERNE di ON a.DO_Type = 39+di.cbIndice AND a.DO_Domaine = 4
                                              LEFT JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
                                              LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
                                              LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                              INNER JOIN F_DOCENTETE b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type
                                              LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                              LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No");
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

				CASE WHEN co_dl.CO_Nom IS NULL THEN CASE WHEN co_do.CO_Nom IS NULL THEN ISNULL(co_ct.CO_Nom,'') + ' ' + ISNULL(co_ct.CO_Prenom,'')
				ELSE ISNULL(co_do.CO_Nom,'') + ' ' + ISNULL(co_do.CO_Prenom,'') END
				ELSE ISNULL(co_dl.CO_Nom,'') + ' ' + ISNULL(co_dl.CO_Prenom,'')
				END CO_Nom,
				fm.FA_CodeFamille,
				fm.FA_Intitule,
				CASE WHEN co_dl.CO_No IS NULL THEN CASE WHEN co_do.CO_No IS NULL THEN ISNULL(co_ct.CO_No,'')
				ELSE ISNULL(co_do.CO_No,'') END
				ELSE ISNULL(co_dl.CO_No,'')
				END CO_No,
				ISNULL(de.DE_Intitule,de_e.DE_Intitule) DE_Intitule,
				ISNULL(ca_dl.CA_Num,ca_do.CA_Num) CA_Num,
				ISNULL(ca_dl.CA_Intitule,ca_do.CA_Intitule) CA_Intitule,
				ct.CT_Ville,
				ct.CT_CodeRegion,
								    'SM-' + 
				RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 1-DATEPART(WEEKDAY, b.DO_Date), b.DO_Date), 12), 6) + '-' +
				RIGHT(CONVERT(VARCHAR, DATEADD(DAY, 7-DATEPART(WEEKDAY, b.DO_Date), b.DO_Date), 12), 6) AS Semaine


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
				LEFT JOIN F_COLLABORATEUR co_dl ON a.CO_No = co_dl.CO_No
				LEFT JOIN F_COLLABORATEUR co_do ON a.CO_No = co_do.CO_No
				LEFT JOIN F_COLLABORATEUR co_ct ON a.CO_No = co_ct.CO_No
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				LEFT JOIN F_FAMILLE fm ON ar.FA_CodeFamille = fm.FA_CodeFamille
				LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				LEFT JOIN F_DEPOT de_e ON b.DE_No = de_e.DE_No
				LEFT JOIN F_COMPTEA ca_dl ON a.CA_Num = ca_dl.CA_Num
				LEFT JOIN F_COMPTEA ca_do ON b.CA_Num = ca_do.CA_Num
				WHERE a.DL_Valorise = 1 
				AND (con.BL_Marge = 1 AND a.DO_Type IN (3,4,5,6,7) ) OR (con.BL_Marge = 0 AND a.DO_Type IN (6,7))");

        }

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
