using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_140324_1249 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifGrilleDialogueModif ( 
	id                   int NOT NULL   IDENTITY ,
	Dialogue             int NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogueModif_Dialogue] DEFAULT 0   ,
	Date                 smalldatetime     ,
	Nature               varchar(100)     ,
	Responsable          varchar(100)     ,
	Libelle              text     ,
	CONSTRAINT Pk_API_T_CertifGrilleDialogueModif_id PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifGrilleDialogueModif ADD CONSTRAINT fk_api_t_certifgrilledialoguemodif FOREIGN KEY ( Dialogue ) REFERENCES dbo.API_T_CertifGrilleDialogue( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
");
			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifGrilleDialogueExigence ( 
	id                   int NOT NULL   IDENTITY ,
	Dialogue             int NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogueExigence_Dialogue] DEFAULT 0   ,
	AR_Ref               varchar(40)     ,
	JugementConformite   bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogueExigence_JugementConformite] DEFAULT 0   ,
	AutreSpecification   text     ,
	Incertitude          bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogueExigence_Incertitude] DEFAULT 0   ,
	Classe               varchar(40)     ,
	Normative            bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogueExigence_Normative] DEFAULT 0   ,
	Utilisateur          varchar(40)     ,
	CONSTRAINT Pk_API_T_CertifGrilleDialogueExigence_id PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifGrilleDialogueExigence ADD CONSTRAINT fk_api_t_certifgrilledialogueexigence FOREIGN KEY ( Dialogue ) REFERENCES dbo.API_T_CertifGrilleDialogue( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
