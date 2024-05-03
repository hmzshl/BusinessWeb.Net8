using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
				string q1 = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='API_T_Affectation' and xtype='U')
				BEGIN
				CREATE TABLE dbo.API_T_Affectation ( 
					id                   int NOT NULL   IDENTITY ,
					Intitule             varchar(100)     ,
					CONSTRAINT Pk_API_T_Affectation_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceArticle ( 
					id                   int NOT NULL   IDENTITY ,
					Designation          varchar(255)     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_AgenceArticle_Type DEFAULT 0   ,
					MinPax               int NOT NULL CONSTRAINT defo_API_T_AgenceArticle_MinPax DEFAULT 0   ,
					MaxPax               int NOT NULL CONSTRAINT defo_API_T_AgenceArticle_MaxPax DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceArticle_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceBooking ( 
					id                   int NOT NULL   IDENTITY ,
					Date                 smalldatetime     ,
					Piece                varchar(100)     ,
					SellingDateStart     smalldatetime     ,
					SellingDateEnd       smalldatetime     ,
					ArrivalDateStart     smalldatetime     ,
					ArrivalDateEnd       smalldatetime     ,
					Libelle              text     ,
					NbrPax               decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_NbrPax DEFAULT 0   ,
					NbrPaxAdult          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_NbrPaxAdult DEFAULT 0   ,
					NbrPaxChild          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_NbrPaxChild DEFAULT 0   ,
					Reference            varchar(100)     ,
					Prestation           int NOT NULL CONSTRAINT defo_API_T_AgenceBooking_Prestation DEFAULT 0   ,
					Client               int NOT NULL CONSTRAINT defo_API_T_AgenceBooking_Client DEFAULT 0   ,
					Fournisseur          int NOT NULL CONSTRAINT defo_API_T_AgenceBooking_Fournisseur DEFAULT 0   ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_PU DEFAULT 0   ,
					PUDevise             decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_PUDevise DEFAULT 0   ,
					Devise               int NOT NULL CONSTRAINT defo_API_T_AgenceBooking_Devise DEFAULT 0   ,
					Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_Taux DEFAULT 0   ,
					NbrPaxInfant         decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBooking_NbrPaxInfant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceBooking_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceBookingDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Article              int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_Article DEFAULT 0   ,
					Libelle              varchar(100)     ,
					LibelleFA            varchar(100)     ,
					NbrPax               int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_NbrPax DEFAULT 0   ,
					NbrInfant            int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_NbrInfant DEFAULT 0   ,
					NbrAdult             int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_NbrAdult DEFAULT 0   ,
					NbrChild             int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_NbrChild DEFAULT 0   ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_Entete DEFAULT 0   ,
					Devise               int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_Devise DEFAULT 0   ,
					Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_Taux DEFAULT 0   ,
					PU_FR                decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_PU_FR DEFAULT 0   ,
					PUDevise_FR          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_PUDevise_FR DEFAULT 0   ,
					PU_CL                decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_PU_CL DEFAULT 0   ,
					PUDevise_CL          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_PUDevise_CL DEFAULT 0   ,
					NbtNuit              int NOT NULL CONSTRAINT defo_API_T_AgenceBookingDetail_NbtNuit DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceBookingDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceBookingOffre ( 
					id                   int NOT NULL   IDENTITY ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_AgenceBookingOffre_Entete DEFAULT 0   ,
					Offre                int NOT NULL CONSTRAINT defo_API_T_AgenceBookingOffre_Offre DEFAULT 0   ,
					SellingDateStart     smalldatetime     ,
					SellingDateEnd       smalldatetime     ,
					ArrivalDateStart     smalldatetime     ,
					ArrivalDateEnd       smalldatetime     ,
					Remise               decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceBookingOffre_Remise DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceBookingOffre_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceBookingPax ( 
					id                   int NOT NULL   IDENTITY ,
					Nom                  varchar(100)     ,
					CIN                  varchar(100)     ,
					Telephone            varchar(100)     ,
					Email                varchar(100)     ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_AgenceBookingPax_Entete DEFAULT 0   ,
					Naissance            smalldatetime     ,
					Adresse              varchar(100)     ,
					Pays                 varchar(100)     ,
					Ville                varchar(100)     ,
					CONSTRAINT Pk_API_T_AgenceBookingPax_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceContrat ( 
					id                   int NOT NULL   IDENTITY ,
					Date                 datetime     ,
					DateDebut            datetime     ,
					DateFin              datetime     ,
					Tiers                int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Tiers DEFAULT 0   ,
					CT_Num               varchar(100)     ,
					Statut               int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Statut DEFAULT 0   ,
					Devise               int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Devise DEFAULT 0   ,
					Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Taux DEFAULT 0   ,
					Piece                varchar(100)     ,
					Libelle              text     ,
					Fichier              text     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Type DEFAULT 0   ,
					Reference            varchar(100)     ,
					KidAgeEnd            int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_KidAgeEnd DEFAULT 0   ,
					Regime               int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Regime DEFAULT 0   ,
					InfantStart          int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_InfantStart DEFAULT 0   ,
					InfantEnd            int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_InfantEnd DEFAULT 1   ,
					ChildStart           int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_ChildStart DEFAULT 2   ,
					ChildEnd             int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_ChildEnd DEFAULT 5   ,
					AdultStart           int NOT NULL CONSTRAINT defo_API_T_AgenceContrat_AdultStart DEFAULT 6   ,
					RemiseInfant         decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContrat_RemiseInfant DEFAULT 0   ,
					RemiseChild          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContrat_RemiseChild DEFAULT 0   ,
					Tax                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContrat_Tax DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceContrat_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceContratDate ( 
					id                   int NOT NULL   IDENTITY ,
					Contrat              int NOT NULL CONSTRAINT defo_API_T_AgenceContratDate_Contrat DEFAULT 0   ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					CONSTRAINT Pk_API_T_AgenceContratDate_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceContratLigne ( 
					id                   int NOT NULL   IDENTITY ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_Entete DEFAULT 0   ,
					Libelle              text     ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_PU DEFAULT 0   ,
					IsJourneeReserved    bit NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_IsJourneeReserved DEFAULT 0   ,
					Unite                int NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_Unite DEFAULT 0   ,
					LibelleFA            text     ,
					PUDevise             decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_PUDevise DEFAULT 0   ,
					Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_Taux DEFAULT 0   ,
					Devise               int NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_Devise DEFAULT 0   ,
					Article              int NOT NULL CONSTRAINT defo_API_T_AgenceContratLigne_Article DEFAULT 0   ,
					Reference            varchar(100)     ,
					CONSTRAINT Pk_API_T_AgenceContratLigne_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Assurance ( 
					id                   int NOT NULL   IDENTITY ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_Assurance_Type DEFAULT 0   ,
					Projet               int     ,
					Materiel             int     ,
					Personnel            int     ,
					Fournisseur          varchar(100)     ,
					Montant              decimal(24,6)     ,
					ModePaiement         int     ,
					Libelle              varchar(100)     ,
					DateDebut            smalldatetime     ,
					Fichier              text     ,
					DateFin              smalldatetime     ,
					Prolongement         decimal(24,6)     ,
					NouvelleDateFin      smalldatetime     ,
					CONSTRAINT Pk_API_T_Assurance_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Attribute ( 
					id                   int NOT NULL   IDENTITY ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_Attribute_Type DEFAULT 0   ,
					Intitule             varchar(100) NOT NULL    ,
					Model                int NOT NULL CONSTRAINT defo_API_T_Attribute_Model DEFAULT 0   ,
					TableIndex           int NOT NULL CONSTRAINT defo_API_T_Attribute_TableIndex DEFAULT 0   ,
					TableName            varchar(100)     ,
					ParentID             int NOT NULL CONSTRAINT defo_API_T_Attribute_ParentID DEFAULT 0   ,
					Imprimable           bit NOT NULL CONSTRAINT defo_API_T_Attribute_Imprimable DEFAULT 0   ,
					Visible              bit NOT NULL CONSTRAINT defo_API_T_Attribute_Visible DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Attribute_id_0 PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AttributeDetail ( 
					id                   int NOT NULL   IDENTITY ,
					ParentID             int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_ParentID DEFAULT 0   ,
					TextValue            nvarchar(max)     ,
					DateValue            smalldatetime     ,
					BoolValue            bit NOT NULL CONSTRAINT defo_API_T_AttributeDetail_BoolValue DEFAULT 0   ,
					DecimalValue         decimal(27,6) NOT NULL CONSTRAINT defo_API_T_AttributeDetail_DecimalValue DEFAULT 0   ,
					IntValue             int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_IntValue DEFAULT 0   ,
					ModelValue           int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_ModelValue DEFAULT 0   ,
					TableValue           int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_TableValue DEFAULT 0   ,
					ChildID              int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_ChildID DEFAULT 0   ,
					TableName            varchar(100)     ,
					TableIndex           int NOT NULL CONSTRAINT defo_API_T_AttributeDetail_TableIndex DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Attribute_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AttributeTable ( 
					id                   int NOT NULL   IDENTITY ,
					ParentID             int NOT NULL CONSTRAINT defo_API_T_AttributeTable_ParentID DEFAULT 0   ,
					TextValue            nvarchar(max) NOT NULL    ,
					CONSTRAINT Pk_API_T_AttributeTable_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AuditLog ( 
					id                   int NOT NULL   IDENTITY ,
					EntityName           varchar(100)     ,
					Action               varchar(100)     ,
					OldValues            text     ,
					NewValues            text     ,
					Timestamp            datetime     ,
					UserName             varchar(100)     ,
					CONSTRAINT Pk_API_T_AuditLog_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_COMPTEA ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					N_Analytique         smallint NOT NULL    ,
					CA_Num               varchar(13) NOT NULL    ,
					CA_Intitule          varchar(35)     ,
					CA_Type              smallint     ,
					CA_Classement        varchar(17)     ,
					CA_Raccourci         varchar(7)     ,
					CA_Report            smallint     ,
					N_Analyse            smallint     ,
					CA_Saut              smallint     ,
					CA_Sommeil           smallint     ,
					CA_Domaine           smallint     ,
					CA_Achat             numeric(24,6)     ,
					CA_Vente             numeric(24,6)     ,
					CO_No                int     ,
					CA_Statut            smallint     ,
					CA_ModeFacturation   smallint     ,
					CONSTRAINT Pk_API_T_Audit_F_COMPTEA_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_COMPTEG ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					CG_Num               varchar(13) NOT NULL    ,
					CG_Type              smallint     ,
					CG_Intitule          varchar(35)     ,
					CG_Classement        varchar(17)     ,
					N_Nature             smallint     ,
					CG_Report            smallint     ,
					CR_Num               varchar(13)     ,
					CG_Raccourci         varchar(7)     ,
					CG_Saut              smallint     ,
					CG_Regroup           smallint     ,
					CG_Analytique        smallint     ,
					CG_Echeance          smallint     ,
					CG_Quantite          smallint     ,
					CG_Lettrage          smallint     ,
					CG_Tiers             smallint     ,
					CG_Devise            smallint     ,
					N_Devise             smallint     ,
					TA_Code              varchar(5)     ,
					CG_Sommeil           smallint     ,
					CG_ReportAnal        smallint     ,
					CONSTRAINT Pk_API_T_Audit_F_COMPTEG_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_COMPTET ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					CT_Num               varchar(20) NOT NULL    ,
					CT_Intitule          varchar(100)     ,
					CT_Type              smallint     ,
					CG_NumPrinc          varchar(20)     ,
					CT_Qualite           varchar(20)     ,
					CT_Classement        varchar(20)     ,
					CT_Contact           varchar(69)     ,
					CT_Adresse           varchar(69)     ,
					CT_Complement        varchar(69)     ,
					CT_CodePostal        varchar(20)     ,
					CT_Ville             varchar(69)     ,
					CT_CodeRegion        varchar(30)     ,
					CT_Pays              varchar(69)     ,
					CT_Raccourci         varchar(20)     ,
					BT_Num               smallint     ,
					N_Devise             smallint     ,
					CT_Ape               varchar(20)     ,
					CT_Identifiant       varchar(30)     ,
					CT_Siret             varchar(20)     ,
					CT_Statistique01     varchar(21)     ,
					CT_Statistique02     varchar(21)     ,
					CT_Statistique03     varchar(21)     ,
					CT_Statistique04     varchar(21)     ,
					CT_Statistique05     varchar(21)     ,
					CT_Statistique06     varchar(21)     ,
					CT_Statistique07     varchar(21)     ,
					CT_Statistique08     varchar(21)     ,
					CT_Statistique09     varchar(21)     ,
					CT_Statistique10     varchar(21)     ,
					CT_Commentaire       varchar(100)     ,
					CT_Encours           numeric(24,6)     ,
					CT_Assurance         numeric(24,6)     ,
					CT_NumPayeur         varchar(20)     ,
					N_Risque             smallint     ,
					CO_No                int     ,
					N_CatTarif           smallint     ,
					CT_Taux01            numeric(24,6)     ,
					CT_Taux02            numeric(24,6)     ,
					CT_Taux03            numeric(24,6)     ,
					CT_Taux04            numeric(24,6)     ,
					N_CatCompta          smallint     ,
					N_Period             smallint     ,
					CT_Facture           smallint     ,
					CT_BLFact            smallint     ,
					CT_Langue            smallint     ,
					N_Expedition         smallint     ,
					N_Condition          smallint     ,
					CT_Saut              smallint     ,
					CT_Lettrage          smallint     ,
					CT_ValidEch          smallint     ,
					CT_Sommeil           smallint     ,
					DE_No                int     ,
					CT_ControlEnc        smallint     ,
					CT_NotRappel         smallint     ,
					N_Analytique         smallint     ,
					CA_Num               varchar(20)     ,
					CT_Telephone         varchar(21)     ,
					CT_Telecopie         varchar(21)     ,
					CT_EMail             varchar(69)     ,
					CT_Site              varchar(69)     ,
					CT_Coface            varchar(25)     ,
					CT_Surveillance      smallint     ,
					CT_SvFormeJuri       varchar(33)     ,
					CT_SvEffectif        varchar(20)     ,
					CT_SvCA              numeric(24,6)     ,
					CT_SvResultat        numeric(24,6)     ,
					CT_SvIncident        smallint     ,
					CT_SvPrivil          smallint     ,
					CT_SvRegul           varchar(3)     ,
					CT_SvCotation        varchar(5)     ,
					CT_SvObjetMaj        varchar(61)     ,
					N_AnalytiqueIFRS     smallint     ,
					CA_NumIFRS           varchar(20)     ,
					CT_PrioriteLivr      smallint     ,
					CT_LivrPartielle     smallint     ,
					MR_No                int     ,
					CT_NotPenal          smallint     ,
					EB_No                int     ,
					CT_NumCentrale       varchar(20)     ,
					CT_FactureElec       smallint     ,
					CT_TypeNIF           smallint     ,
					CT_RepresentInt      varchar(69)     ,
					CT_RepresentNIF      varchar(30)     ,
					CT_EdiCodeType       smallint     ,
					CT_EdiCode           varchar(30)     ,
					CT_EdiCodeSage       varchar(20)     ,
					CT_ProfilSoc         smallint     ,
					CT_StatutContrat     smallint     ,
					CT_EchangeRappro     smallint     ,
					CT_EchangeCR         smallint     ,
					PI_NoEchange         int     ,
					CT_BonAPayer         smallint     ,
					CT_DelaiTransport    smallint     ,
					CT_DelaiAppro        smallint     ,
					CT_LangueISO2        varchar(3)     ,
					CT_AnnulationCR      smallint     ,
					CONSTRAINT Pk_API_T_Audit_F_COMPTET_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_CREGLEMENT ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					RG_No                int     ,
					CT_NumPayeur         varchar(20)     ,
					RG_Date              datetime     ,
					RG_Reference         varchar(20)     ,
					RG_Libelle           varchar(100)     ,
					RG_Montant           numeric(24,6)     ,
					RG_MontantDev        numeric(24,6)     ,
					N_Reglement          smallint     ,
					RG_Impute            smallint     ,
					RG_Compta            smallint     ,
					EC_No                int     ,
					RG_Type              smallint     ,
					RG_Cours             numeric(24,6)     ,
					N_Devise             smallint     ,
					JO_Num               varchar(20) NOT NULL    ,
					CG_NumCont           varchar(20)     ,
					RG_Impaye            datetime     ,
					CG_Num               varchar(20)     ,
					RG_TypeReg           smallint     ,
					RG_Heure             char(9)     ,
					RG_Piece             varchar(20)     ,
					CA_No                int     ,
					CO_NoCaissier        int     ,
					RG_Banque            smallint     ,
					RG_Transfere         smallint     ,
					RG_Cloture           smallint NOT NULL    ,
					RG_Ticket            smallint     ,
					RG_Souche            smallint     ,
					CT_NumPayeurOrig     varchar(20)     ,
					RG_DateEchCont       datetime     ,
					CG_NumEcart          varchar(20)     ,
					JO_NumEcart          varchar(20)     ,
					RG_MontantEcart      numeric(24,6)     ,
					RG_NoBonAchat        int     ,
					CONSTRAINT Pk_API_T_Audit_F_CREGLEMENT_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_DOCENTETE ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					DO_Domaine           smallint     ,
					DO_Type              smallint     ,
					DO_Piece             varchar(20)     ,
					DO_Date              datetime     ,
					DO_Ref               varchar(20)     ,
					DO_Tiers             varchar(20)     ,
					CO_No                int     ,
					DO_Period            smallint     ,
					DO_Devise            smallint     ,
					DO_Cours             numeric(24,6)     ,
					DE_No                int     ,
					LI_No                int     ,
					CT_NumPayeur         varchar(20)     ,
					DO_Expedit           smallint     ,
					DO_NbFacture         smallint     ,
					DO_BLFact            smallint     ,
					DO_TxEscompte        numeric(24,6)     ,
					DO_Reliquat          smallint     ,
					DO_Imprim            smallint     ,
					CA_Num               varchar(13)     ,
					DO_Coord01           varchar(25)     ,
					DO_Coord02           varchar(25)     ,
					DO_Coord03           varchar(25)     ,
					DO_Coord04           varchar(25)     ,
					DO_Souche            smallint     ,
					DO_DateLivr          datetime     ,
					DO_Condition         smallint     ,
					DO_Tarif             smallint     ,
					DO_Colisage          smallint     ,
					DO_TypeColis         smallint     ,
					DO_Transaction       smallint     ,
					DO_Langue            smallint     ,
					DO_Ecart             numeric(24,6)     ,
					DO_Regime            smallint     ,
					N_CatCompta          smallint     ,
					DO_Ventile           smallint     ,
					AB_No                int     ,
					DO_DebutAbo          datetime     ,
					DO_FinAbo            datetime     ,
					DO_DebutPeriod       datetime     ,
					DO_FinPeriod         datetime     ,
					CG_Num               varchar(20)     ,
					DO_Statut            smallint     ,
					DO_Heure             char(9)     ,
					CA_No                int     ,
					CO_NoCaissier        int     ,
					DO_Transfere         smallint     ,
					DO_Cloture           smallint     ,
					DO_NoWeb             varchar(20)     ,
					DO_Attente           smallint     ,
					DO_Provenance        smallint     ,
					CA_NumIFRS           varchar(20)     ,
					MR_No                int     ,
					DO_TypeFrais         smallint     ,
					DO_ValFrais          numeric(24,6)     ,
					DO_TypeLigneFrais    smallint     ,
					DO_TypeFranco        smallint     ,
					DO_ValFranco         numeric(24,6)     ,
					DO_TypeLigneFranco   smallint     ,
					DO_Taxe1             numeric(24,6)     ,
					DO_TypeTaux1         smallint     ,
					DO_TypeTaxe1         smallint     ,
					DO_Taxe2             numeric(24,6)     ,
					DO_TypeTaux2         smallint     ,
					DO_TypeTaxe2         smallint     ,
					DO_Taxe3             numeric(24,6)     ,
					DO_TypeTaux3         smallint     ,
					DO_TypeTaxe3         smallint     ,
					DO_MajCpta           smallint     ,
					DO_Motif             varchar(69)     ,
					CT_NumCentrale       varchar(20)     ,
					DO_Contact           varchar(35)     ,
					DO_FactureElec       smallint     ,
					DO_TypeTransac       smallint     ,
					DO_DateLivrRealisee  datetime     ,
					DO_DateExpedition    datetime     ,
					DO_FactureFrs        varchar(69)     ,
					DO_PieceOrig         varchar(20)     ,
					DO_GUID              uniqueidentifier     ,
					DO_EStatut           smallint     ,
					DO_DemandeRegul      smallint     ,
					ET_No                int     ,
					DO_Valide            smallint     ,
					DO_Coffre            smallint     ,
					DO_CodeTaxe1         varchar(5)     ,
					DO_CodeTaxe2         varchar(5)     ,
					DO_CodeTaxe3         varchar(5)     ,
					DO_TotalHT           numeric(24,6)     ,
					DO_StatutBAP         smallint     ,
					CONSTRAINT Pk_API_T_Audit_F_DOCENTETE_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_DOCLIGNE ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					DO_Domaine           smallint     ,
					DO_Type              smallint NOT NULL    ,
					CT_Num               varchar(20)     ,
					DO_Piece             varchar(20) NOT NULL    ,
					DL_PieceBC           varchar(20)     ,
					DL_PieceBL           varchar(20)     ,
					DO_Date              datetime     ,
					DL_DateBC            datetime     ,
					DL_DateBL            datetime     ,
					DL_Ligne             int     ,
					DO_Ref               varchar(30)     ,
					DL_TNomencl          smallint     ,
					DL_TRemPied          smallint     ,
					DL_TRemExep          smallint     ,
					AR_Ref               varchar(30)     ,
					DL_Design            varchar(69)     ,
					DL_Qte               numeric(24,6)     ,
					DL_QteBC             numeric(24,6)     ,
					DL_QteBL             numeric(24,6)     ,
					DL_PoidsNet          numeric(24,6)     ,
					DL_PoidsBrut         numeric(24,6)     ,
					DL_Remise01REM_Valeur numeric(24,6)     ,
					DL_Remise01REM_Type  smallint     ,
					DL_Remise02REM_Valeur numeric(24,6)     ,
					DL_Remise02REM_Type  smallint     ,
					DL_Remise03REM_Valeur numeric(24,6)     ,
					DL_Remise03REM_Type  smallint     ,
					DL_PrixUnitaire      numeric(24,6)     ,
					DL_PUBC              numeric(24,6)     ,
					DL_Taxe1             numeric(24,6)     ,
					DL_TypeTaux1         smallint     ,
					DL_TypeTaxe1         smallint     ,
					DL_Taxe2             numeric(24,6)     ,
					DL_TypeTaux2         smallint     ,
					DL_TypeTaxe2         smallint     ,
					CO_No                int     ,
					AG_No1               int     ,
					AG_No2               int     ,
					DL_PrixRU            numeric(24,6)     ,
					DL_CMUP              numeric(24,6)     ,
					DL_MvtStock          smallint     ,
					DT_No                int     ,
					AF_RefFourniss       varchar(30)     ,
					EU_Enumere           varchar(21)     ,
					EU_Qte               numeric(24,6)     ,
					DL_TTC               smallint     ,
					DE_No                int     ,
					DL_NoRef             smallint     ,
					DL_TypePL            smallint     ,
					DL_PUDevise          numeric(24,6)     ,
					DL_PUTTC             numeric(24,6)     ,
					DL_No                int     ,
					DO_DateLivr          datetime     ,
					CA_Num               varchar(20)     ,
					DL_Taxe3             numeric(24,6)     ,
					DL_TypeTaux3         smallint     ,
					DL_TypeTaxe3         smallint     ,
					DL_Frais             numeric(24,6)     ,
					DL_Valorise          smallint     ,
					AR_RefCompose        varchar(30)     ,
					DL_NonLivre          smallint     ,
					AC_RefClient         varchar(30)     ,
					DL_MontantHT         numeric(24,6)     ,
					DL_MontantTTC        numeric(24,6)     ,
					DL_FactPoids         smallint     ,
					DL_Escompte          smallint     ,
					DL_PiecePL           varchar(20)     ,
					DL_DatePL            datetime     ,
					DL_QtePL             numeric(24,6)     ,
					DL_NoColis           varchar(30)     ,
					DL_NoLink            int     ,
					RP_Code              varchar(11)     ,
					DL_QteRessource      int     ,
					DL_DateAvancement    datetime     ,
					PF_Num               varchar(20) NOT NULL    ,
					DL_CodeTaxe1         varchar(5)     ,
					DL_CodeTaxe2         varchar(5)     ,
					DL_CodeTaxe3         varchar(5)     ,
					DL_PieceOFProd       int     ,
					DL_PieceDE           varchar(20)     ,
					DL_DateDE            datetime     ,
					DL_QteDE             numeric(24,6)     ,
					DL_Operation         varchar(20)     ,
					CONSTRAINT Pk_API_T_Audit_F_DOCLIGNE_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_ECRITUREC ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					JO_Num               varchar(10) NOT NULL    ,
					EC_No                int NOT NULL    ,
					EC_NoLink            int     ,
					JM_Date              datetime NOT NULL    ,
					EC_Jour              smallint     ,
					EC_Date              datetime     ,
					EC_Piece             varchar(20)     ,
					EC_RefPiece          varchar(20)     ,
					EC_TresoPiece        varchar(20)     ,
					CG_Num               varchar(20) NOT NULL    ,
					CG_NumCont           varchar(20)     ,
					CT_Num               varchar(20)     ,
					EC_Intitule          varchar(100)     ,
					N_Reglement          smallint     ,
					EC_Echeance          datetime     ,
					EC_Parite            numeric(24,6)     ,
					EC_Quantite          numeric(24,6)     ,
					N_Devise             smallint     ,
					EC_Sens              smallint     ,
					EC_Montant           numeric(24,6)     ,
					EC_Lettre            smallint     ,
					EC_Lettrage          varchar(3)     ,
					EC_Point             smallint     ,
					EC_Pointage          varchar(3)     ,
					EC_Impression        smallint     ,
					EC_Cloture           smallint     ,
					EC_CType             smallint     ,
					EC_Rappel            smallint     ,
					CT_NumCont           varchar(20)     ,
					EC_LettreQ           smallint     ,
					EC_LettrageQ         varchar(3)     ,
					EC_ANType            smallint     ,
					EC_RType             smallint     ,
					EC_Devise            numeric(24,6)     ,
					EC_Remise            smallint     ,
					EC_ExportExpert      smallint     ,
					TA_Code              varchar(5)     ,
					EC_Norme             smallint     ,
					TA_Provenance        smallint     ,
					EC_PenalType         smallint     ,
					EC_DatePenal         datetime     ,
					EC_DateRelance       datetime     ,
					EC_DateRappro        datetime     ,
					EC_Reference         varchar(20)     ,
					EC_StatusRegle       smallint     ,
					EC_MontantRegle      numeric(24,6)     ,
					EC_DateRegle         datetime     ,
					EC_RIB               int     ,
					EC_DateOp            datetime     ,
					EC_NoCloture         int     ,
					EC_ExportRappro      smallint     ,
					EC_FactureGUID       uniqueidentifier     ,
					EC_DateCloture       datetime     ,
					CONSTRAINT Pk_API_T_Audit_F_ECRITUREC_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Audit_F_REGLECH ( 
					id                   int NOT NULL   IDENTITY ,
					Operation            varchar(20)     ,
					Timestamp            datetime     ,
					Suser_Name           varchar(255)     ,
					Host_Name            varchar(255)     ,
					Sage_Name            varchar(255)     ,
					cbMarq               int     ,
					RG_No                int NOT NULL    ,
					DR_No                int NOT NULL    ,
					DO_Domaine           smallint     ,
					DO_Type              smallint     ,
					DO_Piece             varchar(20)     ,
					RC_Montant           numeric(24,6)     ,
					RG_TypeReg           smallint     ,
					CONSTRAINT Pk_API_T_Audit_F_REGLECH_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Caisse ( 
					id                   int NOT NULL   IDENTITY ,
					C_Nom                varchar(100) NOT NULL    ,
					C_DateCreation       datetime NOT NULL CONSTRAINT defo_API_T_Caisse_C_DateCreation DEFAULT sysdatetime()   ,
					C_DateInitial        datetime NOT NULL CONSTRAINT defo_API_T_Caisse_C_DateInitial DEFAULT sysdatetime()   ,
					C_SoldeInitial       decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Caisse_C_SoldeInitial DEFAULT 0   ,
					C_Solde              decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Caisse_C_Solde DEFAULT 0   ,
					C_Remarque           varchar(255)     ,
					JO_Num               varchar(40)     ,
					CG_Num               varchar(40)     ,
					C_Racine             varchar(10)     ,
					Agence               int NOT NULL CONSTRAINT defo_API_T_Caisse_Agence DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Caisse_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_CaisseEntete ( 
					id                   int NOT NULL   IDENTITY ,
					Date                 smalldatetime     ,
					Numero               varchar(100)     ,
					Libelle              text     ,
					CT_Num               varchar(100)     ,
					CA_Num               varchar(100)     ,
					Personnel            int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Personnel DEFAULT 0   ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Materiel DEFAULT 0   ,
					MontantLettre        varchar(100)     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Type DEFAULT 0   ,
					Caisse               int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Caisse DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Montant DEFAULT 0   ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Site DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Projet DEFAULT 0   ,
					Sense                int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Sense DEFAULT 0   ,
					Affectation          int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Affectation DEFAULT 0   ,
					Representant         int NOT NULL CONSTRAINT defo_API_T_CaisseEntete_Representant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_CaisseEntete_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_CaisseLigne ( 
					id                   int NOT NULL   IDENTITY ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_CaisseLigne_Entete DEFAULT 0   ,
					Libelle              varchar(100)     ,
					Qte                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CaisseLigne_Qte DEFAULT 0   ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CaisseLigne_PU DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CaisseLigne_Montant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_CaisseLigne_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Caisse_User ( 
					id                   int NOT NULL   IDENTITY ,
					Caisse               int NOT NULL CONSTRAINT defo_API_T_Caisse_User_Caisse DEFAULT 0   ,
					[User]               varchar(100)     ,
					CONSTRAINT Pk_API_T_Caisse_User_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_CentreCharge ( 
					id                   int NOT NULL   IDENTITY ,
					Intitule             varchar(100)     ,
					Type                 int     ,
					Periode              int NOT NULL CONSTRAINT defo_API_T_CentreCharge_Periode DEFAULT 0   ,
					Montant              decimal(27,6) NOT NULL CONSTRAINT defo_API_T_CentreCharge_Montant DEFAULT 0   ,
					Cible                int NOT NULL CONSTRAINT defo_API_T_CentreCharge_Cible DEFAULT 0   ,
					CONSTRAINT Pk_API_T_CentreCharge_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_CentreChargeDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Charge               int NOT NULL CONSTRAINT defo_API_T_CentreChargeDetail_Charge DEFAULT 0   ,
					Cible                int NOT NULL CONSTRAINT defo_API_T_CentreChargeDetail_Cible DEFAULT 0   ,
					ValueID              int NOT NULL CONSTRAINT defo_API_T_CentreChargeDetail_ValueID DEFAULT 0   ,
					Montant              decimal(27,6) NOT NULL CONSTRAINT defo_API_T_CentreChargeDetail_Montant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_CentreChargeDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Depot_User ( 
					id                   int NOT NULL   IDENTITY ,
					Depot                int NOT NULL CONSTRAINT defo_API_T_Depot_User_Depot DEFAULT 0   ,
					[User]               varchar(100)     ,
					CONSTRAINT Pk_API_T_Depot_User_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_DocumentEntete ( 
					id                   int NOT NULL   IDENTITY ,
					Domaine              int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_Domaine DEFAULT 0   ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_Type DEFAULT 0   ,
					Date                 smalldatetime     ,
					CAPI_T_Num           varchar(100)     ,
					Personnel            int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_Personnel DEFAULT 0   ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_Materiel DEFAULT 0   ,
					CA_Num               int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_CA_Num DEFAULT 0   ,
					DE_No                int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_DE_No DEFAULT 0   ,
					JO_Num               varchar(100)     ,
					CG_Num               varchar(100)     ,
					Libelle              varchar(100)     ,
					MontantHT            decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentEntete_MontantHT DEFAULT 0   ,
					MontantTTC           decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentEntete_MontantTTC DEFAULT 0   ,
					Sense                int NOT NULL CONSTRAINT defo_API_T_DocumentEntete_Sense DEFAULT 0   ,
					CONSTRAINT Pk_API_T_DocumentEntete_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_DocumentLigne ( 
					id                   int NOT NULL   IDENTITY ,
					ProjeAPI_T_ID        int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_ProjeAPI_T_ID DEFAULT 0   ,
					CAPI_T_Num           varchar(100)     ,
					DO_Piece             varchar(100)     ,
					DO_Date              smalldatetime     ,
					DO_Ref               varchar(100)     ,
					AR_Ref               varchar(100)     ,
					DL_Design            varchar(150)     ,
					DL_PrixUnitaire      decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_PrixUnitaire DEFAULT 0   ,
					DL_Taxe1             int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_Taxe1 DEFAULT 0   ,
					DL_CodeTaxe1         varchar(10)     ,
					DL_Qte               decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_Qte DEFAULT 0   ,
					DL_QteDE             decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_QteDE DEFAULT 0   ,
					DL_QteBC             decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_QteBC DEFAULT 0   ,
					EU_Enumere           varchar(20)     ,
					DL_MontantHT         decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MontantHT DEFAULT 0   ,
					DL_MontantTTC        decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MontantTTC DEFAULT 0   ,
					Version              int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_Version DEFAULT 0   ,
					DO_Type              int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DO_Type DEFAULT 0   ,
					Ordre                int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_Ordre DEFAULT 0   ,
					DL_PrixRevient       decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_PrixRevient DEFAULT 0   ,
					DL_MargeU            decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MargeU DEFAULT 0   ,
					DL_MargeTotal        decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MargeTotal DEFAULT 0   ,
					DL_MargePourcentage  decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MargePourcentage DEFAULT 0   ,
					DL_MontantHTBC       decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MontantHTBC DEFAULT 0   ,
					DL_MontantTTCBC      decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_MontantTTCBC DEFAULT 0   ,
					Superficie           decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_Superficie DEFAULT 0   ,
					DL_PUBC              decimal(18,0) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_PUBC DEFAULT 0   ,
					DL_PrixBC            decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_DL_PrixBC DEFAULT 0   ,
					MargeLibre           decimal(27,6) NOT NULL CONSTRAINT defo_API_T_DocumentLigne_MargeLibre DEFAULT 0   ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_DocumentLigne_Entete DEFAULT 0   ,
					CONSTRAINT Pk_API_T_DocumentLigne_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_FraisEntete ( 
					id                   int NOT NULL   IDENTITY ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_FraisEntete_Type DEFAULT 0   ,
					Piece                varchar(100)     ,
					Date                 smalldatetime     ,
					Libelle              text     ,
					Beneficiaire         int NOT NULL CONSTRAINT defo_API_T_FraisEntete_Beneficiaire DEFAULT 0   ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_FraisEntete_Materiel DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_FraisEntete_Projet DEFAULT 0   ,
					CO_No                int NOT NULL CONSTRAINT defo_API_T_FraisEntete_CO_No DEFAULT 0   ,
					CT_Num               varchar(100)     ,
					CA_Num               varchar(100)     ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FraisEntete_Montant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_FraisEntete_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_FraisLigne ( 
					id                   int NOT NULL   IDENTITY ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_FraisLigne_Entete DEFAULT 0   ,
					Affectation          int NOT NULL CONSTRAINT defo_API_T_FraisLigne_Affectation DEFAULT 0   ,
					Libelle              text     ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FraisLigne_Montant DEFAULT 0   ,
					CONSTRAINT Pk_API_T_FraisLigne_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_HistoriquePiece ( 
					id                   int NOT NULL   IDENTITY ,
					Piece                varchar(100)     ,
					Date                 smalldatetime     ,
					CreatedAt            smalldatetime     ,
					DO_Type              int NOT NULL CONSTRAINT defo_API_T_HistoriquePiece_DO_Type DEFAULT 0   ,
					DO_Domaine           int NOT NULL CONSTRAINT defo_API_T_HistoriquePiece_DO_Domaine DEFAULT 0   ,
					Tab                  varchar(100)     ,
					CONSTRAINT Pk_API_T_HistoriquePiece_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_ImportLog ( 
					id                   int NOT NULL   IDENTITY ,
					Date                 smalldatetime     ,
					Libelle              text     ,
					TableName            varchar(100)     ,
					TableID              int NOT NULL CONSTRAINT defo_API_T_ImportLog_TableID DEFAULT 0   ,
					CONSTRAINT Pk_API_T_ImportLog_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Information ( 
					id                   int NOT NULL   IDENTITY ,
					Tab                  int NOT NULL CONSTRAINT defo_API_T_Information_Tab DEFAULT 0   ,
					Valeur               varchar(100)     ,
					CONSTRAINT Pk_API_T_Information_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Materiel ( 
					id                   int NOT NULL   IDENTITY ,
					MarqueVH             varchar(100)     ,
					TypeVH               varchar(100)     ,
					Immatricule          varchar(100)     ,
					ImmatriculeWW        varchar(100)     ,
					DMC                  varchar(100)     ,
					TypeMoteur           varchar(100)     ,
					Puissance            varchar(100)     ,
					Usage                varchar(100)     ,
					Conducteur           int NOT NULL CONSTRAINT defo_API_T_Materiel_Conducteur DEFAULT 0   ,
					ValeurVenale         decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurVenale DEFAULT 0   ,
					ValeurNeuf           decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurNeuf DEFAULT 0   ,
					ValeurPTA            decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurPTA DEFAULT 0   ,
					ValeurGlage          decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurGlage DEFAULT 0   ,
					RC                   varchar(100)     ,
					DR                   varchar(100)     ,
					INC                  varchar(100)     ,
					VOL                  varchar(100)     ,
					BDG                  varchar(100)     ,
					TIERCE               varchar(100)     ,
					PTA                  varchar(100)     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_Materiel_Type DEFAULT 0   ,
					ValeurAchat          decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurAchat DEFAULT 0   ,
					DateAchat            smalldatetime     ,
					Fournisseur          varchar(100)     ,
					TypeAchat            int NOT NULL CONSTRAINT defo_API_T_Materiel_TypeAchat DEFAULT 0   ,
					NumeroContrat        varchar(100)     ,
					Modele               varchar(100)     ,
					Chassis              varchar(100)     ,
					Carburant            int NOT NULL CONSTRAINT defo_API_T_Materiel_Carburant DEFAULT 0   ,
					Intitule             varchar(100) NOT NULL    ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_Materiel_Site DEFAULT 0   ,
					Consommation         decimal(24,6)  CONSTRAINT defo_API_T_Materiel_Consommation DEFAULT 0   ,
					Marque               int     ,
					CarteGriseDebut      smalldatetime     ,
					CarteGriseFin        smalldatetime     ,
					CarteGriseType       int     ,
					ValeurLocation       decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurLocation DEFAULT 0   ,
					ValeurLeasing        decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Materiel_ValeurLeasing DEFAULT 0   ,
					DateLocation         smalldatetime     ,
					CONSTRAINT Pk_API_T_Materiel_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_MaterielEntretien ( 
					id                   int NOT NULL   IDENTITY ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Materiel DEFAULT 0   ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Type DEFAULT 0   ,
					Date                 smalldatetime     ,
					Piece                varchar(100)     ,
					Libelle              text     ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Responsable DEFAULT 0   ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					Kilometrage          decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Kilometrage DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Montant DEFAULT 0   ,
					Fournisseur          varchar(100)     ,
					CT_Num               varchar(100)     ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Projet DEFAULT 0   ,
					KilometrageSuivant   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_KilometrageSuivant DEFAULT 0   ,
					DateSuivante         smalldatetime     ,
					Conducteur           int NOT NULL CONSTRAINT defo_API_T_MaterielEntretien_Conducteur DEFAULT 0   ,
					Annee                int     ,
					DatePaiement         smalldatetime     ,
					DateValidite         smalldatetime     ,
					NumeroQuittance      varchar(100)     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_MaterielEntretien_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_MaterielEntretienDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Entretien            int NOT NULL CONSTRAINT defo_API_T_MaterielEntretienDetail_Entretien DEFAULT 0   ,
					Qte                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretienDetail_Qte DEFAULT 0   ,
					Unite                varchar(100)     ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretienDetail_PU DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_MaterielEntretienDetail_Montant DEFAULT 0   ,
					Libelle              varchar(100)     ,
					CONSTRAINT Pk_API_T_MaterielEntretienDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Personnel ( 
					id                   int NOT NULL   IDENTITY ,
					Nom                  varchar(100) NOT NULL    ,
					Prenom               varchar(100)     ,
					DateEmbouche         smalldatetime     ,
					Activite             varchar(100)     ,
					SalaireNet           decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Personnel_SalaireNet DEFAULT 0   ,
					CNSS                 decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Personnel_CNSS DEFAULT 0   ,
					AMO                  decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Personnel_AMO DEFAULT 0   ,
					IR                   decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Personnel_IR DEFAULT 0   ,
					CMR                  decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Personnel_CMR DEFAULT 0   ,
					Banque               varchar(100)     ,
					Paiement             int NOT NULL CONSTRAINT defo_API_T_Personnel_Paiement DEFAULT 0   ,
					DateNaissance        smalldatetime     ,
					CIN                  varchar(100)     ,
					Adresse              varchar(255)     ,
					Departement          int NOT NULL CONSTRAINT defo_API_T_Personnel_Departement DEFAULT 0   ,
					Telephone            varchar(100)     ,
					NumCNSS              varchar(100)     ,
					Agence               int NOT NULL CONSTRAINT defo_API_T_Personnel_Agence DEFAULT 0   ,
					Fonction             int NOT NULL CONSTRAINT defo_API_T_Personnel_Fonction DEFAULT 0   ,
					Email                varchar(100)     ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_Personnel_Site DEFAULT 0   ,
					Matricule            varchar(100)     ,
					LieuNaissance        varchar(255)     ,
					Nationalite          int     ,
					Diplome              varchar(100)     ,
					Qualification        int     ,
					Categorie            int     ,
					DateEmbauche         smalldatetime     ,
					Actif                bit     ,
					SalaireBase          decimal(27,6)     ,
					RIB                  varchar(100)     ,
					SituationFamiliale   int     ,
					CIMR                 varchar(100)     ,
					ModePaiement         int     ,
					NombreEnfant         int     ,
					PersonneCharge       int     ,
					UnitePaiement        int     ,
					TypeDeclaration      int     ,
					TypePaiement         int     ,
					ModelePaie           int     ,
					JourRepos            int     ,
					FonctionIntitule     varchar(100)     ,
					Code                 varchar(100)     ,
					Equipe               int     ,
					TauxAnc              decimal(27,6)     ,
					Anc                  decimal(27,6)     ,
					TauxRepos            decimal(27,6)     ,
					PrimeDiversCNSS      bit     ,
					PrimeReposCNSS       bit     ,
					Checked              bit     ,
					BanqueID             varchar(3)     ,
					GuichetID            varchar(3)     ,
					CompteID             varchar(16)     ,
					CleID                varchar(2)     ,
					Contrat              int     ,
					PermisNum            varchar(100)     ,
					PermisType           varchar(100)     ,
					PermisDateDebut      smalldatetime     ,
					PermisDateFin        smalldatetime     ,
					TelephonePerso       varchar(100)     ,
					TelephonePro         varchar(100)     ,
					TelephoneAutre       varchar(100)     ,
					SalaireBaseMensuel   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Personnel_SalaireBaseMensuel DEFAULT 0   ,
					SalaireBaseJournalier decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Personnel_SalaireBaseJournalier DEFAULT 0   ,
					SalaireBaseHoraire   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Personnel_SalaireBaseHoraire DEFAULT 0   ,
					SalaireType          int NOT NULL CONSTRAINT defo_API_T_Personnel_SalaireType DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_Personnel_Projet DEFAULT 0   ,
					ExpirationCIN        smalldatetime     ,
					RefContrat           varchar(100)     ,
					DateSortie           smalldatetime     ,
					CONSTRAINT Pk_API_T_Personnel_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PersonnelEquipe ( 
					id                   int NOT NULL   IDENTITY ,
					ParentID             int NOT NULL CONSTRAINT defo_API_T_PersonnelEquipe_ParentID DEFAULT 0   ,
					EquipeID             int NOT NULL    ,
					CONSTRAINT Pk_API_T_PersonnelEquipe_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PersonnelMateriel ( 
					id                   int NOT NULL   IDENTITY ,
					ParentID             int NOT NULL CONSTRAINT defo_API_T_PersonnelMateriel_ParentID DEFAULT 0   ,
					MaterielID           int NOT NULL CONSTRAINT defo_API_T_PersonnelMateriel_MaterielID DEFAULT 0   ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					CONSTRAINT Pk_API_T_PersonnelMateriel_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Pointage ( 
					id                   int NOT NULL   IDENTITY ,
					Annee                int  CONSTRAINT defo_API_T_Pointage_Annee DEFAULT 0   ,
					Mois                 int  CONSTRAINT defo_API_T_Pointage_Mois DEFAULT 0   ,
					Type                 int  CONSTRAINT defo_API_T_Pointage_Type DEFAULT 0   ,
					Cloture              bit  CONSTRAINT defo_API_T_Pointage_Cloture DEFAULT 0   ,
					Libelle              varchar(100)     ,
					CONSTRAINT Pk_API_T_Pointage_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageAnnee ( 
					id                   int NOT NULL   IDENTITY ,
					Annee                int NOT NULL CONSTRAINT defo_API_T_PointageAnnee_Annee DEFAULT 0   ,
					Cloture              bit NOT NULL CONSTRAINT defo_API_T_PointageAnnee_Cloture DEFAULT 0   ,
					CONSTRAINT Pk_API_T_PointageAnnee_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageLigne ( 
					id                   int NOT NULL   IDENTITY ,
					Jour                 int     ,
					Personnel            int     ,
					Materiel             int     ,
					Projet               int     ,
					CA_Num               varchar(30)     ,
					Pointage             int NOT NULL CONSTRAINT defo_API_T_PointageLigne_Pointage DEFAULT 0   ,
					Pointeur             int     ,
					NbrHeure             decimal(24,6)     ,
					CONSTRAINT Pk_API_T_PointageLigne_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageMois ( 
					id                   int NOT NULL   IDENTITY ,
					AnneeID              int NOT NULL CONSTRAINT defo_API_T_PointageMois_AnneeID DEFAULT 0   ,
					Mois                 int NOT NULL CONSTRAINT defo_API_T_PointageMois_Mois DEFAULT 0   ,
					Cloture              bit NOT NULL CONSTRAINT defo_API_T_PointageMois_Cloture DEFAULT 0   ,
					Intitule             varchar(100)     ,
					Description          varchar(100)     ,
					CONSTRAINT Pk_API_T_PointageMois_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Projet ( 
					id                   int NOT NULL   IDENTITY ,
					CA_Num               varchar(13) NOT NULL    ,
					CT_Num               varchar(100)     ,
					Objet                text     ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_Projet_Site DEFAULT 0   ,
					Ville                int NOT NULL CONSTRAINT defo_API_T_Projet_Ville DEFAULT 0   ,
					TypeMarche           int NOT NULL CONSTRAINT defo_API_T_Projet_TypeMarche DEFAULT 0   ,
					SituationMarche      int NOT NULL CONSTRAINT defo_API_T_Projet_SituationMarche DEFAULT 0   ,
					PhaseMarche          int NOT NULL CONSTRAINT defo_API_T_Projet_PhaseMarche DEFAULT 0   ,
					DateOuverturePils    smalldatetime     ,
					DateOrdreNotification smalldatetime     ,
					DateEnregistrement   smalldatetime     ,
					DateReceptionDefinitivePrevue smalldatetime     ,
					DateReceptionDefinitiveEffective smalldatetime     ,
					DateOrdreService     smalldatetime     ,
					DateExemplaireUnique smalldatetime     ,
					CoutMarchePrevisionnel decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_CoutMarchePrevisionnel DEFAULT 0   ,
					TotalMarcheTTC       decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_TotalMarcheTTC DEFAULT 0   ,
					TotalMarcheHT        decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_TotalMarcheHT DEFAULT 0   ,
					MontantRetenueGarantie decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_MontantRetenueGarantie DEFAULT 0   ,
					TauxRetenueGarantie  decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_TauxRetenueGarantie DEFAULT 0   ,
					TauxRetenueGarantieDecompte decimal(27,6) NOT NULL CONSTRAINT defo_API_T_Projet_TauxRetenueGarantieDecompte DEFAULT 0   ,
					NumeroMarche         varchar(100)     ,
					PeriodeExecutionResume varchar(100)     ,
					PeriodeExecutionDetail text     ,
					ObjetDetail          text     ,
					CA_Intitule          varchar(35) NOT NULL    ,
					DatePublication      smalldatetime     ,
					Resultat             bit  CONSTRAINT defo_API_T_Projet_Resultat DEFAULT 0   ,
					ModeSoumission       int     ,
					TypeAppelOffre       int     ,
					MontantAppelOffreEstime decimal(24,6)     ,
					IsAppelOffre         bit     ,
					Utilisateur          varchar(100)     ,
					NumeroAppelOffre     varchar(100)     ,
					DateEnregistrementCPS smalldatetime     ,
					DateEnregistrementExemplaire smalldatetime     ,
					ResultatMarchePV     text     ,
					ResultatMarche       int NOT NULL CONSTRAINT defo_API_T_Projet_ResultatMarche DEFAULT 0   ,
					CONSTRAINT Pk_API_T_ProjeAPI_T_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_ProjetAvenant ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_ProjetAvenant_Projet DEFAULT 0   ,
					DateAvenant          smalldatetime     ,
					MontantAvenant       decimal(27,6) NOT NULL CONSTRAINT defo_API_T_ProjetAvenant_MontantAvenant DEFAULT 0   ,
					Libelle              text     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_ProjetAvenanAPI_T_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_ProjetDate ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_ProjetDate_Projet DEFAULT 0   ,
					DateArret            smalldatetime     ,
					DateReprise          smalldatetime     ,
					Motif                text     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_ProjetDates_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_ProjetPlanningDecompte ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_ProjetPlanningDecompte_Projet DEFAULT 0   ,
					Description          text     ,
					DatePrevue           smalldatetime     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_PlanningDecompte_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_ProjetReception ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_ProjetReception_Projet DEFAULT 0   ,
					Nature               text     ,
					DateReception        smalldatetime     ,
					Type                 int     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_ProjetReception_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Remarque ( 
					id                   int NOT NULL   IDENTITY ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_Remarque_Type DEFAULT 0   ,
					Date                 smalldatetime     ,
					DateAlerte           smalldatetime     ,
					Remarque             varchar(255)     ,
					Utilisateur          varchar(100)     ,
					Realisation          int NOT NULL CONSTRAINT defo_API_T_Remarque_Realisation DEFAULT 0   ,
					Etape                varchar(20) NOT NULL    ,
					CAPI_T_Num           varchar(50)     ,
					Demande              int     ,
					CONSTRAINT Pk_T_Table_id_0 PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_RevisionPrix ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_RevisionPrix_Projet DEFAULT 0   ,
					CONSTRAINT Pk_API_T_RevisionPrix_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Site ( 
					id                   int NOT NULL   IDENTITY ,
					Intitule             varchar(100) NOT NULL    ,
					Ville                varchar(100)     ,
					Adresse              varchar(100)     ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_Site_Responsable DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Site_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Site_User ( 
					id                   int NOT NULL   IDENTITY ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_Site_User_Site DEFAULT 0   ,
					[User]               varchar(100)     ,
					CONSTRAINT Pk_API_T_Site_User_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_TaxeTonnage ( 
					id                   int NOT NULL   IDENTITY ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_TaxeTonnage_Materiel DEFAULT 0   ,
					DatePaiement         smalldatetime     ,
					DateValidite         smalldatetime     ,
					CONSTRAINT Pk_API_T_TaxeTonnage_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Ville ( 
					id                   int NOT NULL   IDENTITY ,
					Intitule             varchar(100) NOT NULL    ,
					Region               varchar(100)     ,
					CONSTRAINT Pk_API_T_Ville_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceContratDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Periode              int NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_Periode DEFAULT 0   ,
					Ligne                int NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_Ligne DEFAULT 0   ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_PU DEFAULT 0   ,
					PUDevise             decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_PUDevise DEFAULT 0   ,
					Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_Taux DEFAULT 0   ,
					FR_Num               varchar(100)     ,
					CL_Num               varchar(100)     ,
					Contrat              int NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_Contrat DEFAULT 0   ,
					Article              int NOT NULL CONSTRAINT defo_API_T_AgenceContratDetail_Article DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AgenceContratDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AgenceOffre ( 
					id                   int NOT NULL   IDENTITY ,
					Intitule             varchar(100)     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_AgenceOffre_Type DEFAULT 0   ,
					Entete               int NOT NULL CONSTRAINT defo_API_T_AgenceOffre_Entete DEFAULT 0   ,
					SellingDateStart     smalldatetime     ,
					SellingDateEnd       smalldatetime     ,
					ArrivalDateStart     smalldatetime     ,
					ArrivalDateEnd       smalldatetime     ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceOffre_PU DEFAULT 0   ,
					Remise               decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AgenceOffre_Remise DEFAULT 0   ,
					Offre                int NOT NULL CONSTRAINT defo_API_T_AgenceOffre_Offre DEFAULT 0   ,
					Cumulative           bit NOT NULL CONSTRAINT defo_API_T_AgenceOffre_Cumulative DEFAULT 1   ,
					CONSTRAINT Pk_API_T_AgenceOffre_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Attachement ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_Attachement_Projet DEFAULT 0   ,
					Date                 smalldatetime     ,
					Piece                varchar(100)     ,
					Libelle              text     ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_Attachement_Responsable DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Attachement_Montant DEFAULT 0   ,
					MontantMarche        decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Attachement_MontantMarche DEFAULT 0   ,
					MontantCumul         decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Attachement_MontantCumul DEFAULT 0   ,
					MontantReste         decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Attachement_MontantReste DEFAULT 0   ,
					TauxRabais           decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Attachement_TauxRabais DEFAULT 0   ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					Statut               int  CONSTRAINT defo_API_T_Attachement_Statut DEFAULT 0   ,
					NumeroDecompte       varchar(100)     ,
					DateDecompte         smalldatetime     ,
					CONSTRAINT Pk_API_T_Attachement_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AttachementHorsBD ( 
					id                   int NOT NULL   IDENTITY ,
					Attachement          int NOT NULL CONSTRAINT defo_API_T_AttachementHorsBD_Attachement DEFAULT 0   ,
					Article              varchar(100)     ,
					AR_Ref               varchar(100)     ,
					Qte                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AttachementHorsBD_Qte DEFAULT 0   ,
					Unite                varchar(100)     ,
					Consistance          varchar(100)     ,
					Libelle              varchar(100)     ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AttachementHorsBD_PU DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AttachementHorsBD_Montant DEFAULT 0   ,
					DateExecution        smalldatetime     ,
					CONSTRAINT Pk_API_T_AttachementHorsBD_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Bordereau ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_Bordereau_Projet DEFAULT 0   ,
					Date                 smalldatetime     ,
					Piece                varchar(100)     ,
					Libelle              varchar(100)     ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_Bordereau_Responsable DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_Montant DEFAULT 0   ,
					MontantMarche        decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_MontantMarche DEFAULT 0   ,
					CoutTotale           decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_CoutTotale DEFAULT 0   ,
					Marge                decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_Marge DEFAULT 0   ,
					MargeP               decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_MargeP DEFAULT 0   ,
					Rabais               decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Bordereau_Rabais DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Bordereau_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_BordereauDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Bordereau            int NOT NULL CONSTRAINT defo_API_T_BordereauDetail_Bordereau DEFAULT 0   ,
					Article              int     ,
					AR_Ref               varchar(100)     ,
					QteMarche            decimal(24,6)     ,
					PUMarche             decimal(24,6)     ,
					MontantMarche        decimal(24,6)     ,
					PU                   decimal(24,6)     ,
					CoutU                decimal(24,6)     ,
					MargeU               decimal(24,6)     ,
					Unite                varchar(100)     ,
					Consistance          varchar(100)     ,
					Libelle              text     ,
					Montant              decimal(24,6)     ,
					CoutTotal            decimal(24,6)     ,
					MargeTotale          decimal(24,6)     ,
					MargeP               decimal(24,6)     ,
					NumeroPrix           varchar(100)     ,
					Ordre                int NOT NULL CONSTRAINT defo_API_T_BordereauDetail_Ordre DEFAULT 0   ,
					QteCumul             decimal(24,6)     ,
					QteReste             decimal(24,6)     ,
					MontantCumul         decimal(24,6)     ,
					MontantReste         decimal(24,6)     ,
					CONSTRAINT Pk_API_T_BordereauDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_CarteDisque ( 
					id                   int NOT NULL   IDENTITY ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_CarteDisque_Materiel DEFAULT 0   ,
					Annee                int     ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					Montant              decimal(24,6)     ,
					CONSTRAINT Pk_API_T_CarteDisque_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Caution ( 
					id                   int NOT NULL   IDENTITY ,
					EB_No                int NOT NULL CONSTRAINT defo_API_T_Caution_EB_No DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_Caution_Projet DEFAULT 0   ,
					Type                 int     ,
					Date                 smalldatetime     ,
					Taux                 decimal(24,6)     ,
					MontantMarche        decimal(24,6)     ,
					Montant              decimal(24,6)     ,
					DateRetraitBanque    smalldatetime     ,
					DateRetourBanque     smalldatetime     ,
					DateDepotClient      smalldatetime     ,
					DateAnnulationPrevue smalldatetime     ,
					Reference            varchar(100)     ,
					Fichier              text     ,
					MontantEstime        decimal(24,6)     ,
					Banque               varchar(100)     ,
					Etape                int  CONSTRAINT defo_API_T_Caution_Etape DEFAULT 0   ,
					CONSTRAINT Pk_API_T_Caution_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_DroitConstate ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_DroitConstate_Projet DEFAULT 0   ,
					Date                 smalldatetime     ,
					DatePaiement         smalldatetime     ,
					EB_No                int     ,
					PaiementP            decimal(24,6)     ,
					Montant              decimal(24,6)     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_DroitConstate_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_Nantissement ( 
					id                   int NOT NULL   IDENTITY ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_Nantissement_Projet DEFAULT 0   ,
					EB_No                int     ,
					DateDebut            smalldatetime     ,
					DateFin              smalldatetime     ,
					Fichier              text     ,
					CONSTRAINT Pk_API_T_Nantissement_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageJournee ( 
					id                   int NOT NULL   IDENTITY ,
					AnneeID              int NOT NULL CONSTRAINT defo_API_T_PointageJournee_AnneeID DEFAULT 0   ,
					MoisID               int NOT NULL CONSTRAINT defo_API_T_PointageJournee_MoisID DEFAULT 0   ,
					Date                 smalldatetime NOT NULL    ,
					Cloture              bit NOT NULL CONSTRAINT defo_API_T_PointageJournee_Cloture DEFAULT 0   ,
					CONSTRAINT Pk_API_T_PointageJournee_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageProjet ( 
					id                   int NOT NULL   IDENTITY ,
					Journee              int NOT NULL CONSTRAINT defo_API_T_PointageProjet_Journee DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_PointageProjet_Projet DEFAULT 0   ,
					CA_Num               varchar(30)     ,
					Site                 int NOT NULL CONSTRAINT defo_API_T_PointageProjet_Site DEFAULT 0   ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_PointageProjet_Responsable DEFAULT 0   ,
					CT_Num               varchar(40)     ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_PointageProjet_Type DEFAULT 0   ,
					CONSTRAINT Pk_API_T_PointageProjet_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AttachementDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Attachement          int NOT NULL CONSTRAINT defo_API_T_AttachementDetail_Attachement DEFAULT 0   ,
					Article              int     ,
					AR_Ref               varchar(100)     ,
					Qte                  decimal(24,6)     ,
					QteMarche            decimal(24,6)     ,
					QteReste             decimal(24,6)     ,
					Libelle              varchar(100)     ,
					DateExecution        smalldatetime     ,
					PU                   decimal(24,6)     ,
					Montant              decimal(24,6)     ,
					Ligne                int NOT NULL CONSTRAINT defo_API_T_AttachementDetail_Ligne DEFAULT 0   ,
					QtePrec              decimal(24,6)     ,
					QteCumul             decimal(24,6)     ,
					Unite                varchar(100)     ,
					Consistance          varchar(100)     ,
					MontantMarche        decimal(24,6)     ,
					MontantCumul         decimal(24,6)     ,
					MontantReste         decimal(24,6)     ,
					CONSTRAINT Pk_API_T_AttachementDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AvancementDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Avancement           int NOT NULL CONSTRAINT defo_API_T_AvancementDetail_Avancement DEFAULT 0   ,
					Article              int     ,
					AR_Ref               varchar(100)     ,
					Qte                  decimal(24,6)     ,
					QteMarche            decimal(24,6)     ,
					QteReste             decimal(24,6)     ,
					Libelle              text     ,
					DateExecution        smalldatetime     ,
					PU                   decimal(24,6)     ,
					Montant              decimal(24,6)     ,
					Ligne                int NOT NULL CONSTRAINT defo_API_T_AvancementDetail_Ligne DEFAULT 0   ,
					QtePrec              decimal(24,6)     ,
					QteCumul             decimal(24,6)     ,
					Unite                varchar(100)     ,
					Consistance          varchar(100)     ,
					MontantMarche        decimal(24,6)     ,
					MontantCumul         decimal(24,6)     ,
					MontantReste         decimal(24,6)     ,
					Journee              int NOT NULL CONSTRAINT defo_API_T_AvancementDetail_Journee DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_AvancementDetail_Projet DEFAULT 0   ,
					NumeroPrix           varchar(50)     ,
					CONSTRAINT Pk_API_T_AvancementDetail_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_AvancementHorsBD ( 
					id                   int NOT NULL   IDENTITY ,
					Avancement           int NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_Avancement DEFAULT 0   ,
					Article              varchar(100)     ,
					AR_Ref               varchar(100)     ,
					Qte                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_Qte DEFAULT 0   ,
					Unite                varchar(100)     ,
					Consistance          varchar(100)     ,
					Libelle              varchar(100)     ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_PU DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_Montant DEFAULT 0   ,
					DateExecution        smalldatetime     ,
					Journee              int NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_Journee DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_AvancementHorsBD_Projet DEFAULT 0   ,
					CONSTRAINT Pk_API_T_AvancementHorsBD_id PRIMARY KEY  ( id )
				 );

				CREATE TABLE dbo.API_T_PointageDetail ( 
					id                   int NOT NULL   IDENTITY ,
					Personnel            int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Personnel DEFAULT 0   ,
					Materiel             int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Materiel DEFAULT 0   ,
					NbrHeure             decimal(24,6) NOT NULL CONSTRAINT defo_API_T_PointageDetail_NbrHeure DEFAULT 0   ,
					Projet               int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Projet DEFAULT 0   ,
					PU                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_PointageDetail_PU DEFAULT 0   ,
					Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_PointageDetail_Montant DEFAULT 0   ,
					Journee              int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Journee DEFAULT 0   ,
					Responsable          int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Responsable DEFAULT 0   ,
					Type                 int NOT NULL CONSTRAINT defo_API_T_PointageDetail_Type DEFAULT 0   ,
					CONSTRAINT Pk_API_T_PointageDetail_id PRIMARY KEY  ( id )
				 );
				 ALTER TABLE dbo.API_T_AgenceBookingDetail ADD CONSTRAINT fk_api_t_agencebookingdetail FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_AgenceBooking( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceBookingOffre ADD CONSTRAINT fk_api_t_agencebookingoffre FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_AgenceBooking( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceBookingPax ADD CONSTRAINT fk_api_t_agencebookingpax FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_AgenceBooking( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceContratDate ADD CONSTRAINT fk_api_t_agencecontratdate FOREIGN KEY ( Contrat ) REFERENCES dbo.API_T_AgenceContrat( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceContratDetail ADD CONSTRAINT fk_api_t_agencecontratdetail_periode FOREIGN KEY ( Periode ) REFERENCES dbo.API_T_AgenceContratDate( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceContratDetail ADD CONSTRAINT fk_api_t_agencecontratdetail FOREIGN KEY ( Ligne ) REFERENCES dbo.API_T_AgenceContratLigne( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceContratLigne ADD CONSTRAINT fk_api_t_agencecontratligne_article FOREIGN KEY ( Article ) REFERENCES dbo.API_T_AgenceArticle( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceContratLigne ADD CONSTRAINT fk_api_t_agencecontratligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_AgenceContrat( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceOffre ADD CONSTRAINT fk_api_t_agenceoffre FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_AgenceContrat( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AgenceOffre ADD CONSTRAINT fk_api_t_agenceoffre_info FOREIGN KEY ( Offre ) REFERENCES dbo.API_T_Information( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_Attachement ADD CONSTRAINT fk_api_t_attachement FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AttachementDetail ADD CONSTRAINT fk_api_t_attachementdetail FOREIGN KEY ( Attachement ) REFERENCES dbo.API_T_Attachement( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AttachementDetail ADD CONSTRAINT fk_api_t_attachementdetail_ligne FOREIGN KEY ( Ligne ) REFERENCES dbo.API_T_BordereauDetail( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AttachementHorsBD ADD CONSTRAINT fk_api_t_attachementhorsbd FOREIGN KEY ( Attachement ) REFERENCES dbo.API_T_Attachement( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AttributeDetail ADD CONSTRAINT fk_API_T_attributedetail FOREIGN KEY ( ParentID ) REFERENCES dbo.API_T_Attribute( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AttributeTable ADD CONSTRAINT fk_API_T_attributetable FOREIGN KEY ( ParentID ) REFERENCES dbo.API_T_AttributeDetail( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AvancementDetail ADD CONSTRAINT fk_api_t_avancementdetail FOREIGN KEY ( Journee ) REFERENCES dbo.API_T_PointageJournee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AvancementDetail ADD CONSTRAINT fk_api_t_avancementdetail_pr FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AvancementHorsBD ADD CONSTRAINT fk_api_t_avancementhorsbd FOREIGN KEY ( Journee ) REFERENCES dbo.API_T_PointageJournee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_AvancementHorsBD ADD CONSTRAINT fk_api_t_avancementhorsbd_pr FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_Bordereau ADD CONSTRAINT fk_api_t_bordereau FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_BordereauDetail ADD CONSTRAINT fk_api_t_bordereaudetail FOREIGN KEY ( Bordereau ) REFERENCES dbo.API_T_Bordereau( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_CaisseEntete ADD CONSTRAINT fk_api_t_caisseentete FOREIGN KEY ( Caisse ) REFERENCES dbo.API_T_Caisse( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_CaisseLigne ADD CONSTRAINT fk_api_t_caisseligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_CaisseEntete( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_CarteDisque ADD CONSTRAINT fk_api_t_cartedisque FOREIGN KEY ( Materiel ) REFERENCES dbo.API_T_Materiel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_Caution ADD CONSTRAINT fk_api_t_caution_api_t_projet FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_DocumentLigne ADD CONSTRAINT fk_API_T_documentligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_DocumentEntete( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_DroitConstate ADD CONSTRAINT fk_api_t_droitconstate FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_FraisLigne ADD CONSTRAINT fk_api_t_fraisligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_FraisEntete( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_MaterielEntretienDetail ADD CONSTRAINT fk_api_t_materielentretiendetail FOREIGN KEY ( Entretien ) REFERENCES dbo.API_T_MaterielEntretien( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_Nantissement ADD CONSTRAINT fk_api_t_nantissement FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageDetail ADD CONSTRAINT fk_api_t_pointagedetail_jr FOREIGN KEY ( Journee ) REFERENCES dbo.API_T_PointageJournee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageDetail ADD CONSTRAINT fk_api_t_pointagedetail FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageJournee ADD CONSTRAINT fk_api_t_pointagejournee FOREIGN KEY ( AnneeID ) REFERENCES dbo.API_T_PointageAnnee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageJournee ADD CONSTRAINT fk_api_t_pointagejournee_mois FOREIGN KEY ( MoisID ) REFERENCES dbo.API_T_PointageMois( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageLigne ADD CONSTRAINT fk_api_t_pointageligne FOREIGN KEY ( Pointage ) REFERENCES dbo.API_T_Pointage( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageMois ADD CONSTRAINT fk_api_t_pointagemois FOREIGN KEY ( AnneeID ) REFERENCES dbo.API_T_PointageAnnee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_PointageProjet ADD CONSTRAINT fk_api_t_pointageprojet FOREIGN KEY ( Journee ) REFERENCES dbo.API_T_PointageJournee( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_ProjetAvenant ADD CONSTRAINT fk_API_T_projetavenanAPI_T_API_T_projet FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_ProjetDate ADD CONSTRAINT fk_API_T_projetdates_API_T_projet FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_ProjetPlanningDecompte ADD CONSTRAINT fk_API_T_planningdecompte_API_T_projet FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_ProjetReception ADD CONSTRAINT fk_API_T_projetreception_API_T_projet FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE CASCADE ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_RevisionPrix ADD CONSTRAINT fk_api_t_revisionprix FOREIGN KEY ( Projet ) REFERENCES dbo.API_T_Projet( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

				ALTER TABLE dbo.API_T_TaxeTonnage ADD CONSTRAINT fk_api_t_taxetonnage FOREIGN KEY ( Materiel ) REFERENCES dbo.API_T_Materiel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
				END
				GO
				IF OBJECT_ID('[API_V_AFFAIRE]', 'V') IS NOT NULL
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
				  ) b ON a.CA_Num = b.CA_Num;
				GO
				IF OBJECT_ID('[API_V_AFFAIREAVANCEMENT]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_AFFAIREAVANCEMENT];
				END
				GO
				CREATE VIEW [dbo].[API_V_AFFAIREAVANCEMENT]
				AS
				SELECT 
				a.CT_Num,
				a.CT_Intitule,
				a.CA_Num,
				a.AR_Ref,
				a.AR_Design,
				a.Unite,
				a.DE,
				a.BC,
				a.PL,
				a.BL,
				a.CADE,
				a.CABL,
				CASE WHEN (a.DE - a.BL > 0) AND a.BL > 0 THEN  a.DE - a.BL ELSE 0 END ResteQte,
				CASE WHEN (a.CADE - a.CABL > 0) AND a.CABL > 0 THEN  a.CADE - a.CABL ELSE 0 END ResteCA,
				a.DE - a.BL EcartQte,
				a.CADE - a.CABL EcartCA,
				CASE WHEN ISNULL(a.DE,0) != 0 THEN ISNULL(a.BL,0)/ISNULL(a.DE,0) ELSE 0 END Avancement

				FROM


				(

				SELECT 
				a.CT_Num,
				a.CA_Num,
				a.AR_Ref, 
				ar.AR_Design,
				un.U_Intitule Unite,
				ct.CT_Intitule,
				sum(CASE WHEN a.DO_Type = 'DE' THEN a.DL_Qte ELSE 0 END) DE,
				sum(CASE WHEN a.DO_Type = 'BC' THEN a.DL_Qte ELSE 0 END)+sum(CASE WHEN a.DO_Type = 'PL' THEN a.DL_Qte ELSE 0 END)+sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_Qte ELSE 0 END) BC,
				sum(CASE WHEN a.DO_Type = 'PL' THEN a.DL_Qte ELSE 0 END) PL,
				sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_Qte ELSE 0 END) BL,
				CASE WHEN sum(CASE WHEN a.DO_Type = 'DE' THEN a.DL_Qte ELSE 0 END)-sum(CASE WHEN a.DO_Type = 'PL' THEN a.DL_Qte ELSE 0 END)-sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_Qte ELSE 0 END) > 0
				THEN sum(CASE WHEN a.DO_Type = 'DE' THEN a.DL_Qte ELSE 0 END)-sum(CASE WHEN a.DO_Type = 'PL' THEN a.DL_Qte ELSE 0 END)-sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_Qte ELSE 0 END) ELSE 0 END
				ResteDE,
				sum(CASE WHEN a.DO_Type = 'DE' THEN a.DL_MontantTTC ELSE 0 END) CADE,
				sum(CASE WHEN a.DO_Type = 'BL' THEN a.DL_MontantTTC ELSE 0 END) CABL

				FROM 
				(SELECT
				dl.CT_Num,
				dl.CA_Num,
				dl.AR_Ref,
				dl.DL_Qte, 
				dl.DL_MontantTTC,
				dl.DL_MontantHT,
				CASE dl.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC' WHEN '2' THEN 'PL' ELSE 'BL' END DO_Type 
				FROM F_DOCLIGNE dl
				WHERE dl.AR_Ref is not null AND dl.DO_Type in (0,1,2,3,4,5,6,7) AND dl.DL_Valorise = 1) a

				LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				LEFT JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				LEFT JOIN P_Unite un ON ar.AR_UniteVen = un.cbIndice



				GROUP BY 

				a.CT_Num,
				a.AR_Ref, 
				ar.AR_Design,
				un.U_Intitule,
				ct.CT_Intitule,
				a.CA_Num

				) a;
				GO
				IF OBJECT_ID('[API_V_AGENCEBOOKING]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_AGENCEBOOKING];
				END
				GO
				CREATE VIEW [dbo].[API_V_AGENCEBOOKING]
				AS
				SELECT a.id
					  ,a.Date
					  ,a.Piece
					  ,a.SellingDateStart
					  ,a.SellingDateEnd
					  ,a.ArrivalDateStart
					  ,a.ArrivalDateEnd
					  ,a.Libelle
					  ,a.NbrPax
					  ,a.NbrPaxAdult
					  ,a.NbrPaxChild
					  ,a.Reference
					  ,a.Prestation
					  ,a.Client
					  ,a.Fournisseur
					  ,a.PU
					  ,a.PUDevise
					  ,a.Devise
					  ,a.Taux
					  ,a.NbrPaxInfant
					  ,cl.CT_Intitule ClientIntitule
					  ,fr.CT_Intitule FournisseurIntitule
				  FROM API_T_AgenceBooking a
				  LEFT JOIN F_COMPTET cl ON a.Client = cl.cbMarq
				  LEFT JOIN F_COMPTET fr ON a.Fournisseur = fr.cbMarq;
				GO
				IF OBJECT_ID('[API_V_AGENCECONTRAT]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_AGENCECONTRAT];
				END
				GO
				CREATE VIEW [dbo].[API_V_AGENCECONTRAT]
				AS
				SELECT a.id
					  ,a.Date
					  ,a.DateDebut
					  ,a.DateFin
					  ,a.Tiers
					  ,a.CT_Num
					  ,a.Statut
					  ,a.Devise
					  ,a.Taux
					  ,a.Piece
					  ,a.Libelle
					  ,a.Fichier
					  ,a.Type
					  ,a.Reference
					  ,a.KidAgeEnd
					  ,a.Regime
					  ,a.InfantStart
					  ,a.InfantEnd
					  ,a.ChildStart
					  ,a.ChildEnd
					  ,a.AdultStart
					  ,a.RemiseInfant
					  ,a.RemiseChild
					  ,a.Tax
					  ,ct.CT_Intitule
					  ,de.D_Intitule
				  FROM API_T_AgenceContrat a
				  LEFT JOIN F_COMPTET ct ON a.Tiers = ct.cbMarq
				  LEFT JOIN P_DEVISE de ON a.Devise = de.cbMarq;
				GO
				IF OBJECT_ID('[API_V_ARTCOMPTA]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_ARTCOMPTA];
				END
				GO
				CREATE VIEW [dbo].[API_V_ARTCOMPTA]
				AS
				SELECT 
				a.*,
				CASE WHEN a.Type = 0 THEN 'Vente' WHEN a.Type = 1 THEN 'Achat' WHEN a.Type = 2 THEN 'Stock' END Domaine,
				tx1.TA_Taux Taux1,
				tx2.TA_Taux Taux2,
				tx3.TA_Taux Taux3
				FROM
				(SELECT 
				a.AR_Ref,
				ISNULL(ar.ACP_Type,fa.FCP_Type) Type,
				ISNULL(ar.ACP_Champ,fa.FCP_Champ) Champ,
				ISNULL(ar.ACP_ComptaCPT_CompteG,fa.FCP_ComptaCPT_CompteG) CompteG,
				ISNULL(ar.ACP_ComptaCPT_Taxe1,fa.FCP_ComptaCPT_Taxe1) CodeTaxe1,
				ISNULL(ar.ACP_ComptaCPT_Taxe2,fa.FCP_ComptaCPT_Taxe2) CodeTaxe2,
				ISNULL(ar.ACP_ComptaCPT_Taxe3,fa.FCP_ComptaCPT_Taxe3) CodeTaxe3
				FROM F_ARTICLE a
				LEFT JOIN
				(
				SELECT 
				a.FA_CodeFamille,
				a.FCP_Type,
				a.FCP_Champ,
				a.FCP_ComptaCPT_CompteG,
				a.FCP_ComptaCPT_Taxe1,
				a.FCP_ComptaCPT_Taxe2,
				a.FCP_ComptaCPT_Taxe3
				FROM F_FAMCOMPTA a
				) fa ON a.FA_CodeFamille = fa.FA_CodeFamille

				LEFT JOIN
				(
				SELECT 
				a.AR_Ref,
				a.ACP_Type,
				a.ACP_Champ,
				a.ACP_ComptaCPT_CompteG,
				a.ACP_ComptaCPT_Taxe1,
				a.ACP_ComptaCPT_Taxe2,
				a.ACP_ComptaCPT_Taxe3
				FROM F_ARTCOMPTA a
				) ar ON a.AR_Ref = ar.AR_Ref) a

				LEFT JOIN F_TAXE tx1 ON a.CodeTaxe1 = tx1.TA_Code
				LEFT JOIN F_TAXE tx2 ON a.CodeTaxe2 = tx2.TA_Code
				LEFT JOIN F_TAXE tx3 ON a.CodeTaxe3 = tx3.TA_Code;
				GO
				IF OBJECT_ID('[API_V_ARTICLE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_ARTICLE];
				END
				GO
				CREATE VIEW [dbo].[API_V_ARTICLE]
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
					  ,a.AR_Cycle
					  ,a.AR_Criticite
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
						SUM(a.AS_MontSto) AS_MontSto
						FROM F_ARTSTOCK a
						GROUP BY 
						a.AR_Ref
				  ) sto ON a.AR_Ref = sto.AR_Ref;;
				GO
				IF OBJECT_ID('[API_V_ARTICLEMVT]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW API_V_ARTICLEMVT;
				END
				GO
				CREATE VIEW API_V_ARTICLEMVT
				AS
				SELECT 
				a.DO_Date,
				a.DO_Piece,
				a.DL_DateBL,
				a.DL_PieceBL,
				CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule,
				CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
				CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
				WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
				WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
				WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule,
				a.AR_Ref,
				ar.AR_Design,
				a.CT_Num,
				CASE WHEN a.DO_Domaine IN (0,1,4) THEN ct.CT_Intitule ELSE de2.DE_Intitule END TiersIntitule,
				a.DE_No,
				de.DE_Intitule,
				a.DL_Qte,
				CASE WHEN a.DL_MvtStock = 1 THEN a.DL_Qte ELSE -a.DL_Qte END Mvt,
				CASE WHEN a.DL_MvtStock = 1 THEN 'Entrée' ELSE 'Sortie' END Sense
				FROM F_DOCLIGNE a
				INNER JOIN F_DEPOT de ON a.DE_No = de.DE_No
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				LEFT JOIN F_DEPOT de2 ON a.CT_Num = CAST(de2.DE_No AS VARCHAR(3))

				WHERE a.DL_MvtStock IN (1,3);
				GO
				IF OBJECT_ID('[API_V_ARTICLESTOCK]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW API_V_ARTICLESTOCK;
				END
				GO
				CREATE VIEW API_V_ARTICLESTOCK
				AS
				SELECT 
				a.AR_Ref,
				ar.AR_Design,
				a.DE_No,
				de.DE_Intitule,
				a.AS_QteSto,
				a.AS_MontSto,
				a.AS_QteCom,
				a.AS_QtePrepa,
				a.AS_QteRes,
				a.AS_QteMini,
				a.AS_QteMaxi
				FROM F_ARTSTOCK a
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				INNER JOIN F_DEPOT de ON a.DE_No = de.DE_No;
				GO
				IF OBJECT_ID('[API_V_BALANCEAGEECLIENT]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_BALANCEAGEECLIENT];
				END
				GO
				CREATE VIEW [dbo].[API_V_BALANCEAGEECLIENT]
				AS
				SELECT 
				a.CT_Num,
				a.CT_Intitule,
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
				dl.DO_Date,
				dl.DO_Piece,
				dl.DO_Type,
				YEAR(dl.DO_Date) Annee,
				MONTH(dl.DO_Date) Mois,
				sum(CASE WHEN dl.DO_Type in (4,5) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END) - (sum(CASE WHEN dl.DO_Type in (4,5) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END))*do.DO_TxEscompte/100 Montant 


				FROM F_DOCLIGNE dl
				INNER JOIN F_DOCENTETE do ON dl.DO_Piece = do.DO_Piece AND dl.DO_Type = do.DO_Type
				INNER JOIN F_COMPTET ct ON dl.CT_Num = ct.CT_Num

				WHERE dl.DO_Type in (3,4,5,6,7) AND dl.DL_Valorise = 1

				GROUP BY 
				ct.CT_Intitule,
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


				GROUP BY a.CT_Num,a.CT_Intitule;
				GO
				IF OBJECT_ID('[API_V_BALANCEAGEEFOURNISSEUR]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_BALANCEAGEEFOURNISSEUR];
				END
				GO
				CREATE VIEW [dbo].[API_V_BALANCEAGEEFOURNISSEUR]
				AS
				SELECT 
				a.CT_Num,
				a.CT_Intitule,
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
				dl.DO_Date,
				dl.DO_Piece,
				dl.DO_Type,
				YEAR(dl.DO_Date) Annee,
				MONTH(dl.DO_Date) Mois,
				sum(CASE WHEN dl.DO_Type in (14,15) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END) - (sum(CASE WHEN dl.DO_Type in (14,15) THEN -dl.DL_MontantTTC ELSE dl.DL_MontantTTC END))*do.DO_TxEscompte/100 Montant 


				FROM F_DOCLIGNE dl
				INNER JOIN F_DOCENTETE do ON dl.DO_Piece = do.DO_Piece AND dl.DO_Type = do.DO_Type
				INNER JOIN F_COMPTET ct ON dl.CT_Num = ct.CT_Num

				WHERE dl.DO_Type in (13,14,15,16,17) AND dl.DL_Valorise = 1

				GROUP BY 
				ct.CT_Intitule,
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


				GROUP BY a.CT_Num,a.CT_Intitule;
				GO
				IF OBJECT_ID('[API_V_CAISSEENTETE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_CAISSEENTETE];
				END
				GO
				CREATE VIEW [dbo].[API_V_CAISSEENTETE]
				AS
				SELECT a.id
					  ,a.Date
					  ,a.Numero
					  ,a.Libelle
					  ,a.CT_Num CT_NumProjet
					  ,a.CA_Num
					  ,ca.CA_Intitule
					  ,a.Personnel
					  ,a.Materiel
					  ,a.MontantLettre
					  ,a.Type
					  ,a.Caisse
					  ,a.Montant
					  ,a.Site
					  ,a.Projet
					  ,a.Sense
					  ,ct.CT_Intitule
					  ,pj.Objet
					  ,pj.CT_Num
					  ,pj.NumeroMarche
					  ,pr.Nom PersonnelIntitule
					  ,pr.CIN
					  ,pr.Telephone
					  ,pr.DateNaissance
					  ,pr.Matricule
					  ,mt.Intitule MaterielIntitule
					  ,mt.Immatricule
					  ,af.Intitule AffectationIntitule


				  FROM dbo.API_T_CaisseEntete a
				  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
					LEFT JOIN API_T_Projet pj ON a.Projet = pj.id
				  LEFT JOIN API_T_Personnel pr ON a.Personnel = pr.id
				  LEFT JOIN API_T_Materiel mt ON a.Materiel = mt.id
				  LEFT JOIN API_T_Affectation af ON a.Affectation = af.id;
				GO
				IF OBJECT_ID('[API_V_COLLABORATEUR]', 'V') IS NOT NULL
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
				  ) fr ON a.CO_No = fr.CO_No;
				GO
				IF OBJECT_ID('[API_V_COMPTET]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_COMPTET];
				END
				GO
				CREATE VIEW [dbo].[API_V_COMPTET]
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
				,b.DelaiSommeil
				,b.DL_DateBL
				,b.DL_MontantHT
				,b.DL_MontantTTC
				,rg.RG_Montant
				,ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0) SoldeCommercial
				,cp.EC_Montant SoldeComptable,
				CASE a.CT_ControlEnc WHEN 0 THEN 'Controle Automatique' WHEN 1 THEN 'Selon Code Risque' WHEN 2 THEn 'Compte Bloqué' END Controle,
				CASE a.CT_Sommeil WHEN 0 THEN 'Actif' ELSE 'En sommeil' END SommeilIntitule,
				CASE WHEN (ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0)) = 0 THEN 'Soldé' ELSE 'Non soldé' END EtatSolde


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
				) cp ON a.CT_Num = cp.CT_Num;;
				GO
				IF OBJECT_ID('[API_V_CREGLEMENT]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_CREGLEMENT];
				END
				GO
				CREATE VIEW [dbo].[API_V_CREGLEMENT]
				AS
				SELECT 
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
				a.RG_Type


				FROM F_CREGLEMENT a
				LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
				INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
				INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num;
				GO
				IF OBJECT_ID('[API_V_DOCENTETE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_DOCENTETE];
				END
				GO
				CREATE VIEW [dbo].[API_V_DOCENTETE]
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
					  ,ISNULL(dl.DL_MontantHT,0) DL_MontantHT
					  ,ISNULL(dl.DL_MontantTTC,0) DL_MontantTTC
					  ,ISNULL(dl.DL_MontantTTC,0)-ISNULL(dl.DL_MontantHT,0) DL_MontantTVA
					  ,ISNULL(rg.RC_Montant,0) RC_Montant
					  ,ISNULL(dl.DL_MontantTTC,0) - ISNULL(rg.RC_Montant,0) Reste
					  ,CASE WHEN dl.DL_MontantTTC - ISNULL(rg.RC_Montant,0) = 0 THEN 'Réglé' ELSE 'Non réglé' END EtatReglement
					  ,CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule
					  ,CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
					  CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
					  WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
					  WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
					  WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule
					  ,pj.Objet
					  ,pj.NumeroMarche
	  



				  FROM F_DOCENTETE a
				  LEFT JOIN API_T_Projet pj ON a.CA_Num = pj.CA_Num AND ISNULL(a.CA_Num,'') != ''
				  LEFT JOIN F_COMPTET ct ON a.DO_Tiers = ct.CT_Num
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				  LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No
				  LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num
				  LEFT JOIN F_TARIF tf ON a.DO_Tarif = tf.TF_No
				  LEFT JOIN (
				  SELECT a.DO_Piece, a.DO_Type, sum(a.DL_MontantHT) DL_MontantHT, sum(a.DL_MontantTTC) DL_MontantTTC FROM F_DOCLIGNE a WHERE a.DL_Valorise = 1 GROUP BY a.DO_Piece, a.DO_Type
				  ) dl ON a.DO_Piece = dl.DO_Piece AND a.DO_Type = dl.DO_Type
				  LEFT JOIN (SELECT a.DO_Type, a.DO_Piece, sum(a.RC_Montant) RC_Montant FROM F_REGLECH a GROUP BY a.DO_Type,a.DO_Piece) rg ON a.DO_Piece = rg.DO_Piece AND a.DO_Type = rg.DO_Type;;
				GO
				IF OBJECT_ID('[API_V_DOCLIGNE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_DOCLIGNE];
				END
				GO
				CREATE VIEW [dbo].[API_V_DOCLIGNE]
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
					  ,a.DL_PieceOFProd
					  ,a.DL_PieceDE
					  ,a.DL_DateDE
					  ,a.DL_QteDE
					  ,a.DL_Operation,
					  ct.CT_Intitule,
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
					  CAST(CASE WHEN a.DL_MvtStock = 1 THEN 1 WHEN a.DL_MvtStock = 3 THEN 1 ELSE 0 END AS BIT) IsStock


				  FROM F_DOCLIGNE a
				  INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				  LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				  INNER JOIN F_DOCENTETE b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				  LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No;;
				GO
				IF OBJECT_ID('[API_V_FRAISENTETE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_FRAISENTETE];
				END
				GO
				CREATE VIEW [dbo].[API_V_FRAISENTETE]
				AS
				SELECT 
						a.id
					  ,a.Type
					  ,a.Piece
					  ,a.Date
					  ,a.Libelle
					  ,a.Beneficiaire
					  ,a.Materiel
					  ,a.Projet
					  ,a.CO_No
					  ,a.CT_Num
					  ,a.CA_Num
					  ,a.Montant
					  ,ct.CT_Intitule
					  ,ca.CA_Intitule
					  ,co.CO_Nom + ' ' + co.CO_Prenom CO_Nom

				  FROM API_T_FraisEntete a
				  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				  LEFT JOIN F_COLLABORATEUR co ON a.CO_No = co.CO_No;
				GO
				IF OBJECT_ID('[API_V_MARGE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_MARGE];
				END
				GO
				CREATE VIEW [dbo].[API_V_MARGE]
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


				WHERE a.DL_Valorise = 1 
				AND a.DO_Type in (3,4,5,6,7);
				GO
				IF OBJECT_ID('[API_V_MATERIEL]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_MATERIEL];
				END
				GO
				CREATE VIEW [dbo].[API_V_MATERIEL]
				AS
				SELECT 

						a.id
					  ,a.MarqueVH
					  ,a.TypeVH
					  ,a.Immatricule
					  ,a.ImmatriculeWW
					  ,a.DMC
					  ,a.TypeMoteur
					  ,a.Puissance
					  ,a.Usage
					  ,a.Conducteur
					  ,a.ValeurVenale
					  ,a.ValeurNeuf
					  ,a.ValeurPTA
					  ,a.ValeurGlage
					  ,a.RC
					  ,a.DR
					  ,a.INC
					  ,a.VOL
					  ,a.BDG
					  ,a.TIERCE
					  ,a.PTA
					  ,a.Type
					  ,a.ValeurAchat
					  ,a.DateAchat
					  ,a.Fournisseur
					  ,a.TypeAchat
					  ,a.NumeroContrat
					  ,a.Modele
					  ,a.Chassis
					  ,a.Carburant
					  ,a.Intitule
					  ,a.Site
					  ,a.Consommation
					  ,a.Marque
					  ,a.CarteGriseDebut
					  ,a.CarteGriseFin
					  ,a.CarteGriseType
					  ,a.ValeurLocation
					  ,a.ValeurLeasing
					  ,a.DateLocation
				,CASE a.Carburant WHEN 0 THEN 'Gasoil' WHEN 1 THEN 'Essence' WHEN 2 THEN 'Electricité' WHEN 3 THEN 'Autre' END CarburantIntitule
				,CASE a.TypeAchat WHEN 0 THEN 'Leasing' ELSE 'Autre' END TypeAchatIntitule
				,CASE a.Type WHEN 0 THEN 'Voiture' WHEN 1 THEN 'Pick-UP' WHEN 2 THEN 'Materiel Lourd' WHEN 3 THEN 'Autre' END TypeIntitule,
				mr.Valeur MarqueIntitule,
				pr.PermisNum,
				pr.PermisDateDebut,
				pr.PermisDateFin
				,pr.PermisType
				,pr.Nom


				FROM API_T_Materiel a
				LEFT JOIN API_T_Information mr ON a.Marque = mr.id
				LEFT JOIN API_T_Personnel pr ON a.Conducteur = pr.id;
				GO
				IF OBJECT_ID('[API_V_MATERIELENTRETIEN]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_MATERIELENTRETIEN];
				END
				GO
				CREATE VIEW [dbo].[API_V_MATERIELENTRETIEN]
				AS
				SELECT a.id
					  ,a.Materiel
					  ,a.Type
					  ,a.Date
					  ,a.Piece
					  ,a.Libelle
					  ,a.Responsable
					  ,a.DateDebut
					  ,a.DateFin
					  ,a.Kilometrage
					  ,a.Montant
					  ,a.Fournisseur
					  ,a.CT_Num
					  ,a.Projet
					  ,a.KilometrageSuivant
					  ,a.DateSuivante
					  ,a.Conducteur
					  ,a.Annee
					  ,a.DatePaiement
					  ,a.DateValidite
					  ,a.NumeroQuittance
					  ,a.Fichier
					  ,pr.CA_Num
					  ,pr.NumeroMarche
					  ,pr.Objet
					  ,sl.Nom
					  ,mt.Intitule
					  ,mt.Immatricule
					  ,qt.Qte


				  FROM API_T_MaterielEntretien a
				  LEFT JOIN API_T_Projet pr ON a.Projet = pr.id
				  LEFT JOIN API_T_Personnel sl ON a.Conducteur = sl.id
				  LEFT JOIN API_T_Materiel mt ON a.Materiel = mt.id

				  LEFT JOIN (SELECT a.Entretien, SUM(a.Qte) Qte FROM API_T_MaterielEntretienDetail a GROUP BY a.Entretien) qt ON a.id = qt.Entretien;
				GO
				IF OBJECT_ID('[API_V_PERSONNEL]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_PERSONNEL];
				END
				GO
				CREATE VIEW [dbo].[API_V_PERSONNEL]
				AS
				SELECT 
				a.id
					  ,a.Nom
					  ,a.Prenom
					  ,a.DateEmbouche
					  ,a.Activite
					  ,a.SalaireNet
					  ,a.CNSS
					  ,a.AMO
					  ,a.IR
					  ,a.CMR
					  ,a.Banque
					  ,a.Paiement
					  ,a.DateNaissance
					  ,a.CIN
					  ,a.Adresse
					  ,a.Departement
					  ,a.Telephone
					  ,a.NumCNSS
					  ,a.Agence
					  ,a.Fonction
					  ,a.Email
					  ,a.Site
					  ,a.Matricule
					  ,a.LieuNaissance
					  ,a.Nationalite
					  ,a.Diplome
					  ,a.Qualification
					  ,a.Categorie
					  ,a.DateEmbauche
					  ,a.Actif
					  ,a.SalaireBase
					  ,a.RIB
					  ,a.SituationFamiliale
					  ,a.CIMR
					  ,a.ModePaiement
					  ,a.NombreEnfant
					  ,a.PersonneCharge
					  ,a.UnitePaiement
					  ,a.TypeDeclaration
					  ,a.TypePaiement
					  ,a.ModelePaie
					  ,a.JourRepos
					  ,a.Code
					  ,a.Equipe
					  ,a.TauxAnc
					  ,a.Anc
					  ,a.TauxRepos
					  ,a.PrimeDiversCNSS
					  ,a.PrimeReposCNSS
					  ,a.Checked
					  ,a.BanqueID
					  ,a.GuichetID
					  ,a.CompteID
					  ,a.CleID
					  ,a.Contrat
					  ,a.PermisNum
					  ,a.PermisType
					  ,a.PermisDateDebut
					  ,a.PermisDateFin
					  ,a.TelephonePerso
					  ,a.TelephonePro
					  ,a.TelephoneAutre
					  ,a.SalaireBaseMensuel
					  ,a.SalaireBaseJournalier
					  ,a.SalaireBaseHoraire
					  ,a.SalaireType
					  ,a.Projet
					  ,a.ExpirationCIN
					  ,a.RefContrat
					  ,a.DateSortie
				,a.Prenom + ' '+a.Nom Intitule
				,de.Valeur DepartementIntitule
				,ag.Valeur AgenceIntitule
				,fn.Valeur FonctionIntitule
				,nt.Valeur NationaliteIntitule
				,st.Valeur SituationFamilialeIntitule
				,cr.Valeur ContratIntitule
				,zn.Intitule ZoneIntitule
				,md.Valeur ModePaiementIntitule

				FROM API_T_Personnel a
				LEFT JOIN API_T_Information cr ON a.Contrat = cr.id
				LEFT JOIN API_T_Information de ON a.Departement = de.id
				LEFT JOIN API_T_Information nt ON a.Nationalite = nt.id
				LEFT JOIN API_T_Information fn ON a.Fonction = fn.id
				LEFT JOIN API_T_Information ag ON a.Agence = ag.id
				LEFT JOIN API_T_Information st ON a.SituationFamiliale = st.id
				LEFT JOIN API_T_Information md ON a.ModePaiement = md.id
				LEFT JOIN API_T_Site zn ON a.Site = zn.id;
				GO
				IF OBJECT_ID('[API_V_POINTAGEDETAIL]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_POINTAGEDETAIL];
				END
				GO
				CREATE VIEW [dbo].[API_V_POINTAGEDETAIL]
				AS
				SELECT  a.id
					  ,a.Personnel
					  ,a.Materiel
					  ,a.NbrHeure
					  ,a.Projet
					  ,a.PU
					  ,a.Montant
					  ,a.Journee
					  ,a.Responsable
					  ,a.Type
					  ,jr.Date
					  ,pj.Objet
					  ,pj.CT_Num
					  ,pj.CA_Num
					  ,pj.NumeroMarche
					  ,pr.Nom PersonnelIntitule
					  ,pr.CIN
					  ,pr.Telephone
					  ,pr.DateNaissance
					  ,pr.Matricule
					  ,mt.Intitule MaterielIntitule
					  ,mt.Immatricule
					  ,rs.Nom ResponsableIntitule

				  FROM dbo.API_T_PointageDetail a
				  LEFT JOIN API_T_Projet pj ON a.Projet = pj.id
				  LEFT JOIN API_T_Personnel pr ON a.Personnel = pr.id
				  LEFT JOIN API_T_Materiel mt ON a.Materiel = mt.id
				  LEFT JOIN API_T_Personnel rs ON a.Responsable = rs.id
				  LEFT JOIN API_T_PointageJournee jr ON a.Journee = jr.id;
				GO
				IF OBJECT_ID('[API_V_PROJET]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_PROJET];
				END
				GO
				CREATE VIEW [dbo].[API_V_PROJET]
				AS
				SELECT a.id
					  ,a.CA_Num
					  ,a.CT_Num
					  ,a.Objet
					  ,a.Site
					  ,a.Ville
					  ,a.TypeMarche
					  ,a.SituationMarche
					  ,a.PhaseMarche
					  ,a.DateOuverturePils
					  ,a.DateOrdreNotification
					  ,a.DateEnregistrement
					  ,a.DateReceptionDefinitivePrevue
					  ,a.DateReceptionDefinitiveEffective
					  ,a.DateOrdreService
					  ,a.DateExemplaireUnique
					  ,a.CoutMarchePrevisionnel
					  ,a.TotalMarcheTTC
					  ,a.TotalMarcheHT
					  ,a.MontantRetenueGarantie
					  ,a.TauxRetenueGarantie
					  ,a.TauxRetenueGarantieDecompte
					  ,a.NumeroMarche
					  ,a.PeriodeExecutionResume
					  ,a.PeriodeExecutionDetail
					  ,a.ObjetDetail
					  ,a.DatePublication
					  ,a.Resultat
					  ,a.ModeSoumission
					  ,a.TypeAppelOffre
					  ,a.MontantAppelOffreEstime
					  ,a.IsAppelOffre
					  ,a.Utilisateur
					  ,a.NumeroAppelOffre
					  ,a.DateEnregistrementCPS
					  ,a.DateEnregistrementExemplaire
					  ,a.ResultatMarchePV
					  ,a.ResultatMarche
					  ,ct.CT_Intitule
					  ,ca.CA_Intitule
					  ,si.Intitule ZoneIntitule
					  ,vi.Intitule VilleIntitule

					  ,CASE a.SituationMarche 
					  WHEN 0 THEN 'En arrét'
					  WHEN 1 THEN 'En cours'
					  WHEN 2 THEN 'Récéptionné'
					  WHEN 3 THEN 'Résilié'
					  END SituationMarcheIntitule

					  ,CASE TypeMarche 
					  WHEN 0 THEN 'Non reconductible'
					  WHEN 1 THEN 'Reconductible'
					  END TypeMarcheIntitule

	  					  ,CASE PhaseMarche 
					  WHEN 0 THEN 'Arrét'
	  					  WHEN 1 THEN 'Phase de garantie'
						 WHEN 2 THEN 'Phase d''entretien'
	  					  WHEN 3 THEN 'Phase d''execution'
	  					  WHEN 4 THEN 'Récéptionné definitivement'
					  END PhaseMarcheIntitule

	  					  ,CASE ModeSoumission 
					  WHEN 0 THEN 'Electronique'
					  WHEN 1 THEN 'Physique'
					  WHEN 2 THEN 'Consultation'
					  END ModeSoumissionIntitule

	  					  ,CASE TypeAppelOffre 
					  WHEN 0 THEN 'Classique'
					  WHEN 1 THEN 'Notation'
	  					  WHEN 2 THEN 'Classification'
					  END TypeAppelOffreIntitule

	  	  					  ,CASE ResultatMarche 
					  WHEN 0 THEN 'Raté'
					  WHEN 1 THEN 'Gagné'
					  END ResultatMarcheIntitule


				  FROM dbo.API_T_Projet a
				  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				  LEFT JOIN API_T_Site si ON a.Site = si.id
				  LEFT JOIN API_T_Ville vi ON a.Ville = vi.id;
				GO
				IF OBJECT_ID('[API_V_RELEVE]', 'V') IS NOT NULL
				BEGIN
					DROP VIEW [API_V_RELEVE];
				END
				GO
				CREATE VIEW [dbo].[API_V_RELEVE]
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
				GO




				";

            migrationBuilder.Sql(q1);


            string q0 = @"IF NOT EXISTS (SELECT 1
                                        FROM INFORMATION_SCHEMA.COLUMNS
                                        WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                        AND upper(COLUMN_NAME) = 'Encaiss')
                                BEGIN
                                    ALTER TABLE dbo.F_CREGLEMENT ADD Encaiss BIT DEFAULT 0
                                END
                                ";

            string q2 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'Rappro')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD Rappro BIT DEFAULT 0
                                                    END
                                                    ";

            string q3 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'Depose')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD Depose BIT DEFAULT 0;
                                                    END
                                                    ";

            string q4 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'DateDepot')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD DateDepot smalldatetime;
                                                    END
                                                    ";
            string q5 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'DatePaie')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD DatePaie smalldatetime;
                                                    END
                                                    ";
            string q6 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'Impaye')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD Impaye BIT DEFAULT 0;
                                                    END
                                                    ";

            string q7 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'Incorpore')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD Incorpore BIT DEFAULT 0;
                                                    END
                                                    ";

            string q8 = @"IF OBJECT_ID('[API_V_CREGLEMENT]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_CREGLEMENT];
                                            END
                                            GO

                                            CREATE VIEW [dbo].[API_V_CREGLEMENT]
                                            AS
                                            SELECT 
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
                                            CASE a.Depose WHEN 1 THEN 'Impayé à redéposer' END Remarque


                                            FROM F_CREGLEMENT a
                                            LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
                                            INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
                                            INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
                                            GO


                                            ";


            migrationBuilder.Sql(q0);
            migrationBuilder.Sql(q2);
            migrationBuilder.Sql(q3);
            migrationBuilder.Sql(q4);
            migrationBuilder.Sql(q5);
            migrationBuilder.Sql(q6);
            migrationBuilder.Sql(q7);
            migrationBuilder.Sql(q8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
