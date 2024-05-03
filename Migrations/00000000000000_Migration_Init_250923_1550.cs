using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_250923_1550 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string q1 = @"IF OBJECT_ID('[API_V_AUDIT_F_COMPTET]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_COMPTET];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_COMPTET]
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
				,a.CT_AnnulationCR
				,CASE a.CT_ControlEnc WHEN 0 THEN 'Controle Automatique' WHEN 1 THEN 'Selon Code Risque' WHEN 2 THEn 'Compte Bloqué' END Controle,
				CASE a.CT_Sommeil WHEN 0 THEN 'Actif' ELSE 'En sommeil' END SommeilIntitule
				,a.Operation,
				a.Timestamp,
				a.Suser_Name,
				a.Host_Name,
				a.Sage_Name,
				a.id



				FROM API_T_Audit_F_COMPTET a

GO



                                            ";

            string q2 = @"IF OBJECT_ID('[API_V_AUDIT_F_CREGLEMENT]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_CREGLEMENT];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_CREGLEMENT]
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
                                            jr.CG_Num,
											FORMAT(YEAR(a.RG_Date),'0000') Annee,
											'M'+FORMAT(MONTH(a.RG_Date),'00') Mois,
											FORMAT(YEAR(a.RG_Date),'0000')+'/'+FORMAT(MONTH(a.RG_Date),'00') MoisAnnee,
											a.Operation,
											a.Timestamp,
											a.Suser_Name,
											a.Host_Name,
											a.Sage_Name,
											a.id


                                            FROM API_T_Audit_F_CREGLEMENT a
                                            LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
                                            INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
                                            INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
GO



                                            ";

            string q3 = @"IF OBJECT_ID('[API_V_AUDIT_F_DOCENTETE]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_DOCENTETE];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_DOCENTETE]				
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
					  ,a.DO_GUID
					  ,a.DO_EStatut
					  ,a.DO_DemandeRegul
					  ,a.ET_No
					  ,a.DO_Valide
					  ,a.DO_Coffre
					  ,a.DO_CodeTaxe1
					  ,a.DO_CodeTaxe2
					  ,a.DO_CodeTaxe3
					  ,a.DO_TotalHT
					  ,a.DO_StatutBAP
					  ,ct.CT_Intitule
					  ,ca.CA_Intitule
					  ,cl.CO_Nom
					  ,de.DE_Intitule
					  ,cg.CG_Intitule
					  ,tf.TF_Intitule

					  ,CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule
					  ,CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
					  CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
					  WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
					  WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
					  WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule,
					  a.Operation,
					  a.Timestamp,
					  a.Suser_Name,
					  a.Host_Name,
					  a.Sage_Name,
					  a.id



				  FROM API_T_Audit_F_DOCENTETE a

				  LEFT JOIN F_COMPTET ct ON a.DO_Tiers = ct.CT_Num
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				  LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No
				  LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num
				  LEFT JOIN F_TARIF tf ON a.DO_Tarif = tf.TF_No
GO



                                            ";

            string q4 = @"IF OBJECT_ID('[API_V_AUDIT_F_DOCLIGNE]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_DOCLIGNE];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_DOCLIGNE]
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
	                                              CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
	                                              CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
	                                              WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
	                                              WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
	                                              WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule,
	                                              ca.CA_Intitule,
	                                              cl.CO_Nom,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantTTC/a.DL_Qte ELSE 0 END PUTTC,
	                                              a.DL_MontantTTC-a.DL_MontantHT MontantTVA,
	                                              a.DL_Remise01REM_Valeur Remise,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantHT/a.DL_Qte ELSE 0 END PUNet,
	                                              CASE WHEN a.DL_MvtStock = 1 THEN a.DL_Qte WHEN a.DL_MvtStock = 3 THEN -a.DL_Qte ELSE 0 END QteMvt,
	                                              CAST(CASE WHEN a.DL_MvtStock = 1 THEN 1 WHEN a.DL_MvtStock = 3 THEN 1 ELSE 0 END AS BIT) IsStock,
												  		a.Operation,
													  a.Timestamp,
													  a.Suser_Name,
													  a.Host_Name,
													  a.Sage_Name,
													  a.id


                                              FROM API_T_Audit_F_DOCLIGNE a
                                              INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
                                              LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
                                              LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                              INNER JOIN F_DOCENTETE b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type
                                              LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                              LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No
GO



                                            ";

            string q5 = @"IF OBJECT_ID('[API_V_AUDIT_F_ECRITUREC]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_ECRITUREC];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_ECRITUREC]
AS

SELECT [id]
      ,[Operation]
      ,[Timestamp]
      ,[Suser_Name]
      ,[Host_Name]
      ,[Sage_Name]
      ,a.[cbMarq]
      ,a.[JO_Num]
      ,[EC_No]
      ,[EC_NoLink]
      ,[JM_Date]
      ,[EC_Jour]
      ,[EC_Date]
      ,[EC_Piece]
      ,[EC_RefPiece]
      ,[EC_TresoPiece]
      ,a.[CG_Num]
      ,[CG_NumCont]
      ,a.[CT_Num]
      ,[EC_Intitule]
      ,[N_Reglement]
      ,[EC_Echeance]
      ,[EC_Parite]
      ,[EC_Quantite]
      ,a.[N_Devise]
      ,[EC_Sens]
      ,[EC_Montant]
      ,[EC_Lettre]
      ,[EC_Lettrage]
      ,[EC_Point]
      ,[EC_Pointage]
      ,[EC_Impression]
      ,[EC_Cloture]
      ,[EC_CType]
      ,[EC_Rappel]
      ,[CT_NumCont]
      ,[EC_LettreQ]
      ,[EC_LettrageQ]
      ,[EC_ANType]
      ,[EC_RType]
      ,[EC_Devise]
      ,[EC_Remise]
      ,[EC_ExportExpert]
      ,a.[TA_Code]
      ,[EC_Norme]
      ,[TA_Provenance]
      ,[EC_PenalType]
      ,[EC_DatePenal]
      ,[EC_DateRelance]
      ,[EC_DateRappro]
      ,[EC_Reference]
      ,[EC_StatusRegle]
      ,[EC_MontantRegle]
      ,[EC_DateRegle]
      ,[EC_RIB]
      ,[EC_DateOp]
      ,[EC_NoCloture]
      ,[EC_ExportRappro]
      ,[EC_FactureGUID]
      ,[EC_DateCloture]
	  ,ct.CT_Intitule
	  ,cg.CG_Intitule
	  ,jr.JO_Intitule
	  ,DATEADD(DAY,a.EC_Jour-1, a.JM_Date) MV_Date
  FROM [API_T_Audit_F_ECRITUREC] a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num
GO



                                            ";
            string q6 = @"IF OBJECT_ID('[API_V_CBSESSION]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_CBSESSION];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_CBSESSION]
AS
SELECT 
a.cbSession,
a.CB_Type,
a.CB_Creator,
a.cbUserName,
CASE WHEN a.CB_Type = 'CIAL' THEN 'Gestion commercial'
WHEN a.CB_Type = 'CPTA' THEN 'Comptabilité' ELSE 'Autre' END App
FROM cbUserSession a WHERE a.cbUserName IS NOT NULL
GO



                                            ";
            string q7 = @"IF OBJECT_ID('[API_V_HISTORIQUECONNEXION]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_HISTORIQUECONNEXION];
                                            END
                                            GO
CREATE VIEW API_V_HISTORIQUECONNEXION
AS
SELECT 
a.Utilisateur,
a.CB_Type,
CASE WHEN a.CB_Type = 'CIAL' THEN 'Gestion commercial'
WHEN a.CB_Type = 'CPTA' THEN 'Comptabilité' ELSE 'Autre' END App,
a.AdressIP,
a.Computer,
a.SessionWindows,
a.DateOP,
a.TypeOP

FROM API_T_HistoriqueConnexion a
GO



                                            ";



            migrationBuilder.Sql(q1);
            migrationBuilder.Sql(q2);
            migrationBuilder.Sql(q3);
            migrationBuilder.Sql(q4);
            migrationBuilder.Sql(q5);
            migrationBuilder.Sql(q6);
            migrationBuilder.Sql(q7);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
