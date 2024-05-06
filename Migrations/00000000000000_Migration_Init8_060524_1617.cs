using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init8_060524_1617 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_COMPTET]
				AS
				SELECT 
				a.cbMarq
				,a.CT_Num
				,ISNULL(a.CT_Intitule,'') CT_Intitule
				,ISNULL(a.CT_Type,0) CT_Type
				,ISNULL(a.CG_NumPrinc,'') CG_NumPrinc
				,a.CT_Qualite
				,a.CT_Classement
				,a.CT_Contact
				,a.CT_Adresse
				,a.CT_Complement
				,a.CT_CodePostal
				,a.CT_Ville
				,a.CT_CodeRegion
				,a.CT_Pays
				,a.CT_Raccourci
				,a.BT_Num
				,a.N_Devise
				,a.CT_Ape
				,a.CT_Identifiant
				,a.CT_Siret
				,a.CT_Statistique01
				,a.CT_Statistique02
				,a.CT_Statistique03
				,a.CT_Statistique04
				,a.CT_Statistique05
				,a.CT_Statistique06
				,a.CT_Statistique07
				,a.CT_Statistique08
				,a.CT_Statistique09
				,a.CT_Statistique10
				,a.CT_Commentaire
				,a.CT_Encours
				,a.CT_Assurance
				,a.CT_NumPayeur
				,a.N_Risque
				,a.CO_No
				,a.N_CatTarif
				,a.CT_Taux01
				,a.CT_Taux02
				,a.CT_Taux03
				,a.CT_Taux04
				,a.N_CatCompta
				,a.N_Period
				,a.CT_Facture
				,a.CT_BLFact
				,a.CT_Langue
				,a.N_Expedition
				,a.N_Condition
				,a.CT_Saut
				,a.CT_Lettrage
				,a.CT_ValidEch
				,a.CT_Sommeil
				,a.DE_No
				,a.CT_ControlEnc
				,a.CT_NotRappel
				,a.N_Analytique
				,a.CA_Num
				,a.CT_Telephone
				,a.CT_Telecopie
				,a.CT_EMail
				,a.CT_Site
				,a.CT_Coface
				,a.CT_Surveillance
				,a.CT_SvFormeJuri
				,a.CT_SvEffectif
				,a.CT_SvCA
				,a.CT_SvResultat
				,a.CT_SvIncident
				,a.CT_SvPrivil
				,a.CT_SvRegul
				,a.CT_SvCotation
				,a.CT_SvObjetMaj
				,a.N_AnalytiqueIFRS
				,a.CA_NumIFRS
				,a.CT_PrioriteLivr
				,a.CT_LivrPartielle
				,a.MR_No
				,a.CT_NotPenal
				,a.EB_No
				,a.CT_NumCentrale
				,a.CT_FactureElec
				,a.CT_TypeNIF
				,a.CT_RepresentInt
				,a.CT_RepresentNIF
				,a.CT_EdiCodeType
				,a.CT_EdiCode
				,a.CT_EdiCodeSage
				,a.CT_ProfilSoc
				,a.CT_StatutContrat
				,a.CT_EchangeRappro
				,a.CT_EchangeCR
				,a.PI_NoEchange
				,a.CT_BonAPayer
				,a.CT_DelaiTransport
				,a.CT_DelaiAppro
				,a.CT_LangueISO2
				,b.DelaiSommeil
				,b.DL_DateBL
				,b.DL_MontantHT
				,b.DL_MontantTTC
				,rg.RG_Montant
				,ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0) SoldeCommercial
				,cp.EC_Montant SoldeComptable,
				CASE a.CT_ControlEnc WHEN 0 THEN 'Controle Automatique' WHEN 1 THEN 'Selon Code Risque' WHEN 2 THEn 'Compte Bloqué' END Controle,
				CASE a.CT_Sommeil WHEN 0 THEN 'Actif' ELSE 'En sommeil' END SommeilIntitule,
				CASE WHEN (ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0)) = 0 THEN 'Soldé' ELSE 'Non soldé' END EtatSolde,
				ec.RT_NbJour,
				ec.R_Intitule,
				dev.D_Intitule



				FROM F_COMPTET a



				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				SUM(a.DL_MontantHT) DL_MontantHT,
				SUM(a.DL_MontantTTC) DL_MontantTTC,
				MAX(a.DL_DateBL) DL_DateBL,
				COUNT(DISTINCT a.AR_Ref) NbrArticles,
				COUNT(DISTINCT a.DO_Piece) NbrDocuments,
				DATEDIFF(DAY,MAX(a.DL_DateBL),GETDATE()) DelaiSommeil,
				CASE WHEN b.DL_MontantTTC != 0 THEN SUM(a.DL_MontantTTC)/b.DL_MontantTTC ELSE 0 END Taux

				FROM F_DOCLIGNE a
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				INNER JOIN
				(
				SELECT 
				ct.CT_Type,
				SUM(a.DL_MontantHT) DL_MontantHT,
				SUM(a.DL_MontantTTC) DL_MontantTTC

				FROM F_DOCLIGNE a
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				WHERE a.DL_Valorise = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17)
				GROUP BY ct.CT_Type
				) b ON ct.CT_Type = b.CT_Type
				WHERE a.DL_Valorise = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17)
				GROUP BY a.CT_Num , b.DL_MontantTTC
				) b ON a.CT_Num = b.CT_Num





				LEFT JOIN 
				(
				SELECT 
				a.CT_NumPayeur,
				SUM(a.RG_Montant) RG_Montant
				FROM F_CREGLEMENT a
				GROUP BY a.CT_NumPayeur
				) rg ON rg.CT_NumPayeur = a.CT_Num





				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				SUM(CASE WHEN a.EC_Sens = 0 THEN a.EC_Montant ELSE -a.EC_Montant END) EC_Montant
				FROM F_ECRITUREC a
				GROUP BY a.CT_Num
				) cp ON a.CT_Num = cp.CT_Num

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
				) ec ON a.CT_Num = ec.CT_Num

				LEFT JOIN P_DEVISE dev ON a.N_Devise = dev.cbIndice");
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
	  ,CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
	  CASE a.DO_Type WHEN 0 THEN 'DE Client' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL Client' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
	  WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA Fournisseur' WHEN 11 THEN 'PR Fournisseur' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL Fournisseur'
	  WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
	  WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule
	  ,pj.Objet
	  ,pj.NumeroMarche
	  ,CASE WHEN cl.CO_No IS NULL THEN cl_ct.CO_No ELSE cl.CO_No END CO_No2
	  ,ec.RT_NbJour
	  ,DATEADD(DAY,ISNULL(ec.RT_NbJour,0),a.DO_Date) DateEcheance
	  ,CASE WHEN DATEADD(DAY,ISNULL(ec.RT_NbJour,0),a.DO_Date)<= GETDATE() THEN 'Echu' ELSE 'Non echu' END Echeance
	  



  FROM F_DOCENTETE a
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
				) ec ON ct.CT_Num = ec.CT_Num



");
			migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_RELEVE]
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
				DATEADD(DAY,ISNULL(mr.RT_NbJour,0),a.DO_Date) DateEcheance
				


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
				) mr ON a.CT_Num = mr.CT_Num


");

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
