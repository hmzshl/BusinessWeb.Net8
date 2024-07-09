using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9_090724_2112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifAutoclaves ( 
	id                   int NOT NULL   IDENTITY ,
	Creation             datetime2(7)  CONSTRAINT [defo_API_T_CertifAutoclaves_Creation] DEFAULT getdate()   ,
	Modification         datetime2(7)     ,
	CreationIP           nvarchar(45)     ,
	ModificationIP       nvarchar(45)     ,
	CreationHost         nvarchar(255)     ,
	ModificationHost     nvarchar(255)     ,
	CreationUser         nvarchar(255)     ,
	ModificationUser     nvarchar(255)     ,
	AR_Ref               varchar(100)     ,
	Code                 varchar(100)     ,
	Essai                varchar(255)     ,
	EssaiJuge            int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_EssaiJuge] DEFAULT 0   ,
	TemperatureSterilisation varchar(255)     ,
	TemperatureSterilisationJuge int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_TemperatureSterilisationJuge] DEFAULT 0   ,
	Tolerance            varchar(255)     ,
	ToleranceJuge        int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_ToleranceJuge] DEFAULT 0   ,
	TempsSterilisation   varchar(255)     ,
	TempsSterilisationJuge int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_TempsSterilisationJuge] DEFAULT 0   ,
	TempsEquilibrage     varchar(255)     ,
	TempsEquilibrageJuge int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_TempsEquilibrageJuge] DEFAULT 0   ,
	Stabilite            varchar(255)     ,
	StabiliteJuge        int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_StabiliteJuge] DEFAULT 0   ,
	Homogeneite          varchar(255)     ,
	HomogeneiteJuge      int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_HomogeneiteJuge] DEFAULT 0   ,
	Pression             varchar(255)     ,
	PressionJuge         int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_PressionJuge] DEFAULT 0   ,
	ValeurSterilisatrice varchar(255)     ,
	ValeurSterilisatriceJuge int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_ValeurSterilisatriceJuge] DEFAULT 0   ,
	EntreeSondes         varchar(255)     ,
	EntreeSondesJuge     int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_EntreeSondesJuge] DEFAULT 0   ,
	Dialogue             int NOT NULL CONSTRAINT [defo_API_T_CertifAutoclaves_Dialogue] DEFAULT 0   ,
	CONSTRAINT Pk_API_T_CertifFiche_id_0 PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifAutoclaves ADD CONSTRAINT fk_api_t_certifautoclaves FOREIGN KEY ( Dialogue ) REFERENCES dbo.API_T_CertifGrilleDialogue( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
