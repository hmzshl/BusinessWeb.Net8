using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_100324_1414 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifGrilleDialogue ( 
									id                   int NOT NULL   IDENTITY ,
									Date                 smalldatetime     ,
									Numero               varchar(100)     ,
									CT_Num               varchar(100)     ,
									DemandePar           int NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_DemandePar] DEFAULT 0   ,
									RaisonSocial         varchar(100)     ,
									Telephone            varchar(100)     ,
									Fax                  varchar(100)     ,
									Adresse              varchar(100)     ,
									Interlocuteur        varchar(100)     ,
									FonctionInterlocuteur varchar(100)     ,
									PreuvesRaccord       bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PreuvesRaccord] DEFAULT 0   ,
									[ ListePersonnelQualifie] bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_ ListePersonnelQualifie] DEFAULT 0   ,
									[ AuditAuLaboratoire] bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_ AuditAuLaboratoire] DEFAULT 0   ,
									PMQ                  bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PMQ] DEFAULT 0   ,
									ProcedureTraitementReclamation bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_ProcedureTraitementReclamation] DEFAULT 0   ,
									LieuPrestation       varchar(100)     ,
									OperationsAttendues  text     ,
									PeriodiciteEtalonnageEtiquettes bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PeriodiciteEtalonnageEtiquettes] DEFAULT 0   ,
									PeriodiciteEtalonnageRapports bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PeriodiciteEtalonnageRapports] DEFAULT 0   ,
									RelanceEtalonnageAnnuel bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_RelanceEtalonnageAnnuel] DEFAULT 0   ,
									DateSouhaiteePrestation smalldatetime     ,
									DateTransmissionDocuments smalldatetime     ,
									AdresseLivraisonExpedition varchar(100)     ,
									ModeLivraisonExpedition varchar(100)     ,
									PriseChargeTransport bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PriseChargeTransport] DEFAULT 0   ,
									NaturePrestation     int NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_NaturePrestation] DEFAULT 0   ,
									PointsEtalonnage     int NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_PointsEtalonnage] DEFAULT 0   ,
									CapaciteDisponibiliteRessourcesNecessaires bit NOT NULL CONSTRAINT [defo_API_T_CertifGrilleDialogue_CapaciteDisponibiliteRessourcesNecessaires] DEFAULT 0   ,
									CONSTRAINT Pk_API_T_CertifGrilleDialogue_id PRIMARY KEY  ( id ) 
								 );
								");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
