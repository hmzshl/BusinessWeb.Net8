using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9999__260113 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"ALTER VIEW [dbo].[API_V_DOCENTETE]
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
	  ,ISNULL(a.DO_TotalHT,0) DL_MontantHT
	  ,ISNULL(a.DO_TotalTTC,0) DL_MontantTTC
	  ,ISNULL(a.DO_TotalTTC,0)-ISNULL(a.DO_TotalHT,0) DL_MontantTVA
	  ,ISNULL(a.DO_MontantRegle,0) RC_Montant
	  ,ISNULL(a.DO_TotalTTC,0) - ISNULL(a.DO_MontantRegle,0) Reste
	  ,CASE WHEN a.DO_TotalTTC - ISNULL(a.DO_MontantRegle,0) = 0 THEN 'Réglé' ELSE 'Non réglé' END EtatReglement
	  ,CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule
	  ,ISNULL(di.D_Intitule,CASE WHEN a.DO_Domaine = 40 THEN 'D.Interne' ELSE 
	  CASE a.DO_Type WHEN 0 THEN 'DE.Client' WHEN 1 THEN 'BC.Client' WHEN 2 THEN 'PL.Client' WHEN 3 THEN 'BL.Client' WHEN 4 THEN 'BR.Client' WHEN 5 THEN 'BR.Client'
	  WHEN 6 THEN 'FA.Client' WHEN 7 THEN 'FC.Client' WHEN 10 THEN 'DA.Fournisseur' WHEN 11 THEN 'PR.Fournisseur' WHEN 12 THEN 'BC.Fournisseur' WHEN 13 THEn 'BL.Fournisseur'
	  WHEN 14 THEN 'BR.Fournisseur' WHEN 15 THEN 'BR.Fournisseur' WHEN 16 THEN 'FA.Fournisseur' WHEN 17 THEN 'FC.Fournisseur'
	  WHEN 20 THEN 'M.Entrée' WHEN 21 THEN 'M.Sortie' WHEN 23 THEN 'M.Transfert' ELSE 'A.Document' END END) TypeIntitule
	  ,pj.Objet
	  ,pj.NumeroMarche
	  ,CASE WHEN cl.CO_No IS NULL THEN cl_ct.CO_No ELSE cl.CO_No END CO_No2
	  ,CAST(0 AS smallint) RT_NbJour
	  ,DATEADD(DAY,ISNULL(0,0),a.DO_Date) DateEcheance
	  ,CASE WHEN DATEADD(DAY,ISNULL(0,0),a.DO_Date)<= GETDATE() THEN 'Echu' ELSE 'Non echu' END Echeance,
	  a.cbCreation,
		a.cbModification,
		a.DO_TotalTTC,
		a.DO_NetAPayer,
		a.DO_TotalHTNet,
						    'SM-' Semaine
	



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
						";
			migrationBuilder.Sql(q1);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
