using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9_020724_0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogueExigence", "Code", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogueExigence", "PointsEtalonnage", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogueExigence", "Classe", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogueExigence", "ErreurMaximalTolereUtilisateur", "100"));
			migrationBuilder.Sql(@"DECLARE @sql NVARCHAR(MAX) = N'';
                    DECLARE @tableName NVARCHAR(128);

                    -- Cursor to iterate over all tables that start with 'T_'
                    DECLARE table_cursor CURSOR FOR
                    SELECT TABLE_NAME
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_TYPE = 'BASE TABLE'
                    AND TABLE_NAME LIKE 'API_T%';

                    OPEN table_cursor;
                    FETCH NEXT FROM table_cursor INTO @tableName;

                    -- Loop over each table and construct the ALTER TABLE statement
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        SET @sql = N'';

                        -- Check for each column and add to the SQL statement if it doesn't exist
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'Creation')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD Creation DATETIME2 DEFAULT GETDATE() NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'Modification')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD Modification DATETIME2 NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'CreationIP')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD CreationIP NVARCHAR(45) NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'ModificationIP')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD ModificationIP NVARCHAR(45) NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'CreationHost')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD CreationHost NVARCHAR(255) NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'ModificationHost')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD ModificationHost NVARCHAR(255) NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'CreationUser')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD CreationUser NVARCHAR(255) NULL;';
                        END
                        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'ModificationUser')
                        BEGIN
                            SET @sql = @sql + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD ModificationUser NVARCHAR(255) NULL;';
                        END

                        -- Execute the generated SQL if not empty
                        IF @sql <> ''
                        BEGIN
                            EXEC sp_executesql @sql;
                        END

                        FETCH NEXT FROM table_cursor INTO @tableName;
                    END

                    CLOSE table_cursor;
                    DEALLOCATE table_cursor;
                    ");


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
				dev.D_Intitule,
				null PROT_User,
				a.cbCreation,
				a.cbModification



				FROM F_COMPTET a
				--LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid


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
										a.cbModification


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
	  ,CASE WHEN DATEADD(DAY,ISNULL(ec.RT_NbJour,0),a.DO_Date)<= GETDATE() THEN 'Echu' ELSE 'Non echu' END Echeance
	  ,null PROT_User,
	  a.cbCreation,
		a.cbModification,
		a.DO_TotalTTC,
		a.DO_NetAPayer,
		a.DO_TotalHTNet
	



  FROM F_DOCENTETE a
  --LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
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
												  null PROT_User,
													a.cbCreation,
													a.cbModification


                                              FROM F_DOCLIGNE a
											  --LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
											    LEFT JOIN P_INTERNE di ON a.DO_Type = 39+di.cbIndice AND a.DO_Domaine = 4
                                              LEFT JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
                                              LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
                                              LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                              INNER JOIN F_DOCENTETE b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type
                                              LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                              LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No");
			migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_ECRITUREC]
AS
SELECT 
      a.[cbMarq]
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
	  ,ct.CT_Intitule
	  ,cg.CG_Intitule
	  ,jr.JO_Intitule
	  ,DATEADD(DAY,a.EC_Jour-1, a.JM_Date) MV_Date,
	  CASE WHEN a.EC_Sens = 0 THEN a.EC_Montant ELSE 0 END Debit,
	  CASE WHEN a.EC_Sens = 1 THEN a.EC_Montant ELSE 0 END Credit,
	  				null PROT_User,
				a.cbCreation,
				a.cbModification


  FROM [dbo].[F_ECRITUREC] a
  --LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num");


			migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_AFFAIRE]
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
	                                              ,CASE WHEN ISNULL(b.CADE,0) != 0 THEN ISNULL(b.CABL,0)/ISNULL(b.CADE,0) ELSE 0 END Avancement,
												  uti.PROT_User,
												  a.cbCreation,
												  a.cbModification

                                              FROM F_COMPTEA a
											  LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
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
                                              ) b ON a.CA_Num = b.CA_Num");
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
					  CASE WHEN sto.AS_QteSto > sto.AS_QteMaxi THEN 'En stock' WHEN sto.AS_QteSto <= sto.AS_QteMini THEN 'En rupture' ELSE 'A surveiller' END EtatStockMin,
					  null PROT_User,
					  a.cbCreation,
					  a.cbModification



	  
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

				  --LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid");

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
