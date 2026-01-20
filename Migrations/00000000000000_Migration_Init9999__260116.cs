using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9999__260116 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE TABLE dbo.API_T_CategorieAchat ( 
	Id                   int NOT NULL   IDENTITY ,
	Nom                  nvarchar(255)     ,
	Description          nvarchar(max)     ,
	CONSTRAINT PK__API_T_Categ__3214EC077DF26EDF PRIMARY KEY  ( Id )
 );

CREATE TABLE dbo.API_T_Declaration ( 
	Id                   int NOT NULL   IDENTITY ,
	SocieteId            int NOT NULL    ,
	Periode              datetime     ,
	DateSoumission       datetime     ,
	Statut               nvarchar(50)     ,
	TVACollectee         decimal(18,2)     ,
	TVADeductible        decimal(18,2)     ,
	TVANette             decimal(18,2)     ,
	Observation          nvarchar(max)     ,
	DateDebut            datetime     ,
	DateFin              datetime     ,
	Intitule             nvarchar(255)     ,
	CONSTRAINT PK__API_T_Decla__3214EC0733269517 PRIMARY KEY  ( Id )
 );

CREATE TABLE dbo.API_T_HistoriqueModification ( 
	Id                   int NOT NULL   IDENTITY ,
	TableAffectee        nvarchar(100)     ,
	ID_Affecte           int NOT NULL    ,
	UtilisateurId        int NOT NULL    ,
	DateModification     datetime     ,
	AncienneValeur       nvarchar(max)     ,
	NouvelleValeur       nvarchar(max)     ,
	CONSTRAINT PK__API_T_Histo__3214EC07E6E4896A PRIMARY KEY  ( Id )
 );

CREATE TABLE dbo.API_T_License ( 
	Id                   int NOT NULL   IDENTITY ,
	SocieteId            int     ,
	TypeLicense          nvarchar(100)     ,
	DateDebut            datetime NOT NULL    ,
	DateFin              datetime NOT NULL    ,
	Statut               nvarchar(50)     ,
	CONSTRAINT PK__API_T_Licen__3214EC07D4A73B44 PRIMARY KEY  ( Id )
 );

CREATE TABLE dbo.API_T_Paiement ( 
	id                   int NOT NULL   IDENTITY ,
	Date                 datetime     ,
	RG_Date              datetime     ,
	RG_Piece             varchar(100)     ,
	RG_Libelle           varchar(100)     ,
	RG_Echeance          datetime     ,
	RG_Reference         varchar(100)     ,
	EC_Piece             varchar(100)     ,
	EC_Intitule          varchar(100)     ,
	EC_RefPiece          varchar(100)     ,
	EC_Lettrage          varchar(20)     ,
	EC_Montant           decimal(24,6)     ,
	EC_No                int     ,
	cbMarq               int     ,
	EC_Sens              int     ,
	CG_Num               varchar(100)     ,
	SrcRappro            varchar(30)     ,
	MoisRappro           varchar(40)     ,
	CT_Num               varchar(40)     ,
	EC_Date              datetime     ,
	JM_Date              datetime     ,
	EC_DateRappro        datetime     ,
	Mois                 int     ,
	Annee                int     ,
	ModePaiement         int     ,
	ModePaiementChar     varchar(100)     ,
	RG_Type              int NOT NULL CONSTRAINT defo_API_T_Paiement_RG_Type DEFAULT 0   ,
	Societe              int NOT NULL CONSTRAINT defo_API_T_Paiement_Societe DEFAULT 0   ,
	Declaration          int NOT NULL CONSTRAINT defo_API_T_Paiement_Declaration DEFAULT 0   ,
	JO_Num               varchar(50)     ,
	Src                  varchar(30)     ,
	CT_Intitule          varchar(100)     ,
	Ignored              bit NOT NULL CONSTRAINT defo_API_T_Paiement_Ignored DEFAULT 0   ,
	CONSTRAINT Pk_API_T_Paiement_id PRIMARY KEY  ( id )
 );

CREATE TABLE dbo.API_T_Taxe ( 
	id                   int NOT NULL   IDENTITY ,
	TA_Sens              int NOT NULL CONSTRAINT defo_API_T_Taxe_TA_Sens DEFAULT 0   ,
	CG_Num               varchar(30)     ,
	TA_Taux              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Taxe_TA_Taux DEFAULT 0   ,
	Societe              int NOT NULL CONSTRAINT defo_API_T_Taxe_Societe DEFAULT 0   ,
	Intitule             varchar(69)     ,
	CONSTRAINT Pk_API_T_Taxe_id PRIMARY KEY  ( id )
 );

CREATE TABLE dbo.API_T_Tiers ( 
	id                   int NOT NULL   IDENTITY ,
	Societe              int NOT NULL CONSTRAINT defo_API_T_Tiers_Societe DEFAULT 0   ,
	CT_Type              int NOT NULL CONSTRAINT defo_API_T_Tiers_CT_Type DEFAULT 0   ,
	CT_Num               varchar(30)     ,
	ICE                  varchar(30)     ,
	IdF                  varchar(30)     ,
	CT_Intitule          varchar(150)     ,
	NatureService        varchar(255)     ,
	CONSTRAINT Pk_API_T_Tiers_id PRIMARY KEY  ( id )
 );

CREATE TABLE dbo.API_T_Facture ( 
	Id                   int NOT NULL   IDENTITY ,
	DeclarationId        int     ,
	Numero               nvarchar(100)     ,
	DateFacture          datetime NOT NULL    ,
	ClientFournisseur    nvarchar(255)     ,
	ICE                  nvarchar(50)     ,
	MontantHT            decimal(18,2) NOT NULL    ,
	MontantTVA           decimal(18,2) NOT NULL    ,
	MontantTTC           decimal(18,2)     ,
	Nature               nvarchar(100)     ,
	CategorieAchatId     int     ,
	ModeReglement        nvarchar(50)     ,
	DateReglement        datetime     ,
	SourceFacture        nvarchar(100)     ,
	Regl                 int NOT NULL CONSTRAINT defo_API_T_Facture_Regl DEFAULT 0   ,
	Src                  varchar(30)     ,
	Ignored              bit NOT NULL CONSTRAINT defo_API_T_Facture_Ignored DEFAULT 0   ,
	IsEXO                bit NOT NULL CONSTRAINT defo_API_T_Facture_IsEXO DEFAULT 0   ,
	cbMarq               int NOT NULL CONSTRAINT defo_API_T_Facture_cbMarq DEFAULT 0   ,
	EC_Lettre            int NOT NULL CONSTRAINT defo_API_T_Facture_EC_Lettre DEFAULT 0   ,
	EC_Lettrage          varchar(30)     ,
	Taxe                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_Facture_Taxe DEFAULT 0   ,
	Ordre                int NOT NULL CONSTRAINT defo_API_T_Facture_Ordre DEFAULT 0   ,
	CONSTRAINT PK__API_T_Factu__3214EC076117F25A PRIMARY KEY  ( Id )
 );

CREATE TABLE dbo.API_T_FactureTaux ( 
	id                   int NOT NULL   IDENTITY ,
	Facture              int NOT NULL CONSTRAINT defo_API_T_FactureTaux_Facture DEFAULT 0   ,
	Taux                 decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FactureTaux_Taux DEFAULT 0   ,
	HT                   decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FactureTaux_HT DEFAULT 0   ,
	TVA                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FactureTaux_TVA DEFAULT 0   ,
	TTC                  decimal(24,6) NOT NULL CONSTRAINT defo_API_T_FactureTaux_TTC DEFAULT 0   ,
	CG_Num               varchar(30)     ,
	cbMarq               int NOT NULL CONSTRAINT defo_API_T_FactureTaux_cbMarq DEFAULT 0   ,
	EC_Lettre            int NOT NULL CONSTRAINT defo_API_T_FactureTaux_EC_Lettre DEFAULT 0   ,
	EC_Lettrage          varchar(30)     ,
	CONSTRAINT Pk_API_T_FactureTaux_id PRIMARY KEY  ( id )
 );

CREATE TABLE dbo.API_T_FichierJoint ( 
	Id                   int NOT NULL   IDENTITY ,
	FactureId            int     ,
	NomFichier           nvarchar(255)     ,
	URL                  nvarchar(500)     ,
	DateAjout            datetime     ,
	CONSTRAINT PK__API_T_Fichi__3214EC0770DEED1E PRIMARY KEY  ( Id )
 );

ALTER TABLE dbo.API_T_Facture ADD CONSTRAINT FK_API_T_Facture_API_T_CategorieAchat FOREIGN KEY ( CategorieAchatId ) REFERENCES dbo.API_T_CategorieAchat( Id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_Facture ADD CONSTRAINT FK_API_T_Facture_API_T_Declaration FOREIGN KEY ( DeclarationId ) REFERENCES dbo.API_T_Declaration( Id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_Facture ADD CONSTRAINT fk_API_T_Facture_API_T_paiement FOREIGN KEY ( Regl ) REFERENCES dbo.API_T_Paiement( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_FactureTaux ADD CONSTRAINT fk_API_T_Facturetaux_API_T_Facture FOREIGN KEY ( Facture ) REFERENCES dbo.API_T_Facture( Id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_FichierJoint ADD CONSTRAINT FK_API_T_FichierJoint_API_T_Facture FOREIGN KEY ( FactureId ) REFERENCES dbo.API_T_Facture( Id ) ON DELETE NO ACTION ON UPDATE NO ACTION;";
			migrationBuilder.Sql(q1);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
