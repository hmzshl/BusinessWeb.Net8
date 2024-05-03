using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_211023_1455 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifEntete ( 
	id                   int NOT NULL   IDENTITY ,
	Type                 int NOT NULL CONSTRAINT defo_API_T_CertifEntete_Type DEFAULT 0   ,
	Piece                varchar(100)     ,
	Date                 smalldatetime     ,
	Libelle              text     ,
	Beneficiaire         int NOT NULL CONSTRAINT defo_API_T_CertifEntete_Beneficiaire DEFAULT 0   ,
	Materiel             int NOT NULL CONSTRAINT defo_API_T_CertifEntete_Materiel DEFAULT 0   ,
	Projet               int NOT NULL CONSTRAINT defo_API_T_CertifEntete_Projet DEFAULT 0   ,
	CO_No                int NOT NULL CONSTRAINT defo_API_T_CertifEntete_CO_No DEFAULT 0   ,
	CT_Num               varchar(100)     ,
	CA_Num               varchar(100)     ,
	Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CertifEntete_Montant DEFAULT 0   ,
	CONSTRAINT Pk_API_T_CertifEntete_id PRIMARY KEY  ( id )
 );
");

            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifLigne ( 
	id                   int NOT NULL   IDENTITY ,
	Entete               int NOT NULL CONSTRAINT defo_API_T_CertifLigne_Entete DEFAULT 0   ,
	Affectation          int NOT NULL CONSTRAINT defo_API_T_CertifLigne_Affectation DEFAULT 0   ,
	Libelle              text     ,
	Montant              decimal(24,6) NOT NULL CONSTRAINT defo_API_T_CertifLigne_Montant DEFAULT 0   ,
	CONSTRAINT Pk_API_T_CertifLigne_id PRIMARY KEY  ( id )
 );

ALTER TABLE dbo.API_T_CertifLigne ADD CONSTRAINT fk_api_t_Certifligne FOREIGN KEY ( Entete ) REFERENCES dbo.API_T_CertifEntete( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[API_V_CERTIFENTETE]
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

  FROM API_T_CertifEntete a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
  LEFT JOIN F_COLLABORATEUR co ON a.CO_No = co.CO_No
GO");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
