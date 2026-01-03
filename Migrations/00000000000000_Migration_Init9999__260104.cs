using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9999__260104 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE TABLE dbo.API_T_NoteFraisEntete ( 
	id                   int NOT NULL   IDENTITY ,
	Date                 smalldatetime     ,
	Numero               varchar(100)     ,
	Libelle              text     ,
	CT_Num               varchar(100)     ,
	CA_Num               varchar(100)     ,
	Personnel            int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Personnel] DEFAULT 0   ,
	Materiel             int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Materiel] DEFAULT 0   ,
	MontantLettre        varchar(100)     ,
	Type                 int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Type] DEFAULT 0   ,
	NoteFrais            int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_NoteFrais] DEFAULT 0   ,
	Montant              decimal(24,6) NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Montant] DEFAULT 0   ,
	Site                 int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Site] DEFAULT 0   ,
	Projet               int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Projet] DEFAULT 0   ,
	Sense                int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Sense] DEFAULT 0   ,
	Affectation          int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Affectation] DEFAULT 0   ,
	Representant         int NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Representant] DEFAULT 0   ,
	Remarque             varchar(100)     ,
	Reference            varchar(100)     ,
	Creation             datetime2(7)  CONSTRAINT [defo_API_T_NoteFraisEntete_Creation] DEFAULT getdate()   ,
	Modification         datetime2(7)     ,
	CreationIP           nvarchar(45)     ,
	ModificationIP       nvarchar(45)     ,
	CreationHost         nvarchar(255)     ,
	ModificationHost     nvarchar(255)     ,
	CreationUser         nvarchar(255)     ,
	ModificationUser     nvarchar(255)     ,
	Valide               bit NOT NULL CONSTRAINT [defo_API_T_NoteFraisEntete_Valide] DEFAULT 0   ,
	ValideDate           smalldatetime     ,
	CONSTRAINT Pk_API_T_NoteFraisEntete_id PRIMARY KEY  ( id ) 
 );

CREATE TABLE dbo.API_T_NoteFraisLigne ( 
	id                   int NOT NULL   IDENTITY ,
	Entete               int NOT NULL CONSTRAINT [defo_API_T_NoteFraisLigne_Entete] DEFAULT 0   ,
	Libelle              varchar(100)     ,
	Qte                  decimal(24,6) NOT NULL CONSTRAINT [defo_API_T_NoteFraisLigne_Qte] DEFAULT 0   ,
	PU                   decimal(24,6) NOT NULL CONSTRAINT [defo_API_T_NoteFraisLigne_PU] DEFAULT 0   ,
	Montant              decimal(24,6) NOT NULL CONSTRAINT [defo_API_T_NoteFraisLigne_Montant] DEFAULT 0   ,
	Creation             datetime2(7)  CONSTRAINT [defo_API_T_NoteFraisLigne_Creation] DEFAULT getdate()   ,
	Modification         datetime2(7)     ,
	CreationIP           nvarchar(45)     ,
	ModificationIP       nvarchar(45)     ,
	CreationHost         nvarchar(255)     ,
	ModificationHost     nvarchar(255)     ,
	CreationUser         nvarchar(255)     ,
	ModificationUser     nvarchar(255)     ,
	Affectation          int NOT NULL CONSTRAINT [defo_API_T_NoteFraisLigne_Affectation] DEFAULT 0   ,
	CONSTRAINT Pk_API_T_NoteFraisLigne_id PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_NoteFraisLigne ADD CONSTRAINT fk_api_t_NoteFraisligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_NoteFraisEntete( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

						";
			migrationBuilder.Sql(q1);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
