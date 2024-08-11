using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9001_110824 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifInstrument ( 
	id                   int NOT NULL   IDENTITY ,
	Intitule             varchar(100)     ,
	IdentificationInterne varchar(100)     ,
	Etat                 varchar(100)     ,
	Remarque             varchar(255)     ,
	CONSTRAINT Pk_API_T_CertifInstrument_id PRIMARY KEY  ( id ) 
 )");


			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifOrdreMission ( 
	id                   int NOT NULL   IDENTITY ,
	Date                 smalldatetime     ,
	Piece                varchar(100)     ,
	CT_Num               varchar(30)     ,
	NumeroDossier        varchar(100)     ,
	Lieu                 varchar(255)     ,
	DateDepart           smalldatetime     ,
	DateRetour           smalldatetime     ,
	Jours                int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_Jours] DEFAULT 0   ,
	CO_No                int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_CO_No] DEFAULT 0   ,
	CO_No1               int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_CO_No1] DEFAULT 0   ,
	CO_No2               int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_CO_No2] DEFAULT 0   ,
	CO_No3               int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_CO_No3] DEFAULT 0   ,
	CO_No4               int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMission_CO_No4] DEFAULT 0   ,
	MissionObjet         varchar(100)     ,
	MoyenTransport       varchar(100)     ,
	CONSTRAINT Pk_API_T_CertifOrdreMission_id PRIMARY KEY  ( id ) 
 )");


			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifOrdreMissionLigne ( 
	id                   int NOT NULL   IDENTITY ,
	Ordre                int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMissionLigne_Ordre] DEFAULT 0   ,
	Instrument           int NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMissionLigne_Instrument] DEFAULT 0   ,
	Libelle              varchar(255)     ,
	Statut               bit NOT NULL CONSTRAINT [defo_API_T_CertifOrdreMissionLigne_Statut] DEFAULT 0   ,
	CONSTRAINT Pk_API_T_CertifOrdreMissionLigne_id PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifOrdreMissionLigne ADD CONSTRAINT fk_api_t_certifordremissionligne FOREIGN KEY ( Ordre ) REFERENCES dbo.API_T_CertifOrdreMission( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifOrdreMissionLigne ADD CONSTRAINT fk_api_t_certifordremissionligne_2 FOREIGN KEY ( Instrument ) REFERENCES dbo.API_T_CertifInstrument( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
");


			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifRapportMission ( 
	id                   int NOT NULL   IDENTITY ,
	Date                 smalldatetime     ,
	Piece                varchar(100)     ,
	Lieu                 varchar(255)     ,
	CT_Num               varchar(30)     ,
	CO_No                int NOT NULL CONSTRAINT [defo_API_T_CertifRapportMission_CO_No] DEFAULT 0   ,
	NumeroDossier        varchar(100)     ,
	DateDebutTravaux     smalldatetime     ,
	DateFinTravaux       smalldatetime     ,
	CONSTRAINT Pk_API_T_CertifRapportMission_id PRIMARY KEY  ( id ) 
 );
");


			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifRapportMissionLigne ( 
	id                   int NOT NULL   IDENTITY ,
	Rapport              int NOT NULL CONSTRAINT [defo_API_T_CertifRapportMissionLigne_Rapport] DEFAULT 0   ,
	Instrument           int NOT NULL CONSTRAINT [defo_API_T_CertifRapportMissionLigne_Instrument] DEFAULT 0   ,
	Designation          varchar(255)     ,
	Constructeur         varchar(100)     ,
	Identifiant          varchar(100)     ,
	NumeroCertificat     varchar(100)     ,
	Emplacement          varchar(100)     ,
	CONSTRAINT Pk_API_T_CertifRapportMissionLigne_id PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifRapportMissionLigne ADD CONSTRAINT fk_api_t_certifrapportmissionligne FOREIGN KEY ( Rapport ) REFERENCES dbo.API_T_CertifRapportMission( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifRapportMissionLigne ADD CONSTRAINT fk_api_t_certifrapportmissionligne_2 FOREIGN KEY ( Instrument ) REFERENCES dbo.API_T_CertifInstrument( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
");




		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
