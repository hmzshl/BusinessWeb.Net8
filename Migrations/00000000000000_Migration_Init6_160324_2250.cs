using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_160324_2250 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifOuvertureDossier ( 
									id                   int NOT NULL   IDENTITY ,
									Numero               varchar(100)     ,
									Date                 smalldatetime     ,
									CO_No                int NOT NULL CONSTRAINT [defo_API_T_CertifOuvertureDossier_CO_No] DEFAULT 0   ,
									Intitule             varchar(100)     ,
									Interlocuteur        varchar(100)     ,
									Service              varchar(100)     ,
									Telephone            varchar(100)     ,
									Fax                  varchar(100)     ,
									Email                varchar(100)     ,
									Adresse              varchar(255)     ,
									Commentaire          varchar(255)     ,
									NumeroDossier        varchar(100)     ,
									DateOuverture        smalldatetime     ,
									CONSTRAINT Pk_API_T_CertifOuvertureDossier_id PRIMARY KEY  ( id ) 
								 );
								");

            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifDocument ( 
	                            id                   int NOT NULL   IDENTITY ,
	                            Type                 int NOT NULL CONSTRAINT [defo_API_T_CertifDocument_Type] DEFAULT 0   ,
	                            Date                 smalldatetime     ,
	                            Numero               varchar(50)     ,
	                            Responsable          varchar(100)     ,
	                            Dossier              int NOT NULL CONSTRAINT [defo_API_T_CertifDocument_Dossier] DEFAULT 0   ,
	                            CONSTRAINT Pk_API_T_CertifDocument_id PRIMARY KEY  ( id ) 
                             );

                            ALTER TABLE dbo.API_T_CertifDocument ADD CONSTRAINT fk_api_t_certifdocument FOREIGN KEY ( Dossier ) REFERENCES dbo.API_T_CertifOuvertureDossier( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
                            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
