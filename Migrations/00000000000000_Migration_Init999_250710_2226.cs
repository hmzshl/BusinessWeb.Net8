using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250710_2226 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			string q1 = @"CREATE TABLE dbo.API_T_CertifReception ( 
	id                   int NOT NULL   IDENTITY ,
	NumeroEnregistrement varchar(50) NOT NULL    ,
	DateReception        date NOT NULL    ,
	CT_Num               varchar(100)     ,
	ClientNom            varchar(100)     ,
	NumeroDossier        varchar(50)     ,
	Priorite             varchar(50)     ,
	Planification        varchar(50)     ,
	Livreur              varchar(100)     ,
	Recepteur            varchar(100)     ,
	DateEntreeLabo       smalldatetime     ,
	DateSortieLabo       smalldatetime     ,
	HeureEntreeLabo      time(7)     ,
	NaturePrestation     varchar(100)     ,
	CreatedDate          smalldatetime  CONSTRAINT [defo_API_T_CertifReception_CreatedDate] DEFAULT getdate()   ,
	ModifiedDate         smalldatetime  CONSTRAINT [defo_API_T_CertifReception_ModifiedDate] DEFAULT getdate()   ,
	CONSTRAINT PK__API_T_Ce__3213E83FA134D9B3 PRIMARY KEY  ( id ) 
 );
";
			string q2 = @"CREATE TABLE dbo.API_T_CertifDecharge ( 
	id                   int NOT NULL   IDENTITY ,
	NumeroFiche          varchar(50) NOT NULL    ,
	DateFiche            smalldatetime NOT NULL    ,
	NumeroDossier        varchar(50)     ,
	NumeroFicheReceptionCorrespondante varchar(50)     ,
	SoussigneNom         varchar(100)     ,
	SocieteNom           varchar(100)     ,
	ReceptionID          int     ,
	CreatedDate          smalldatetime  CONSTRAINT [defo_API_T_CertifDecharge_CreatedDate] DEFAULT getdate()   ,
	ModifiedDate         smalldatetime  CONSTRAINT [defo_API_T_CertifDecharge_ModifiedDate] DEFAULT getdate()   ,
	CONSTRAINT PK__API_T_Ce__3213E83FBBA965AD PRIMARY KEY  ( id ) 
 );
";
			string q3 = @"CREATE TABLE dbo.API_T_CertifReceptionLigne ( 
	LigneID              int NOT NULL   IDENTITY ,
	EnregistrementID     int NOT NULL    ,
	Identification       varchar(50)     ,
	HorsService          bit     ,
	EnService            bit     ,
	Accessoires          varchar(100)     ,
	Observations         varchar(500)     ,
	Instrument           int NOT NULL CONSTRAINT [defo_API_T_CertifReceptionLigne_Instrument] DEFAULT 0   ,
	CONSTRAINT PK__API_T_Ce__3B499C53DA61377D PRIMARY KEY  ( LigneID ) 
 );

";
			string q4 = @"CREATE TABLE dbo.API_T_CertifDechargeLigne ( 
	id                   int NOT NULL   IDENTITY ,
	FicheReceptionID     int NOT NULL    ,
	Identification       varchar(50)     ,
	Accessoires          varchar(100)     ,
	Instrument           int NOT NULL CONSTRAINT [defo_API_T_CertifDechargeLigne_Instrument] DEFAULT 0   ,
	CONSTRAINT PK__API_T_Ce__3213E83F1A3EBABD PRIMARY KEY  ( id ) 
 );

";

			migrationBuilder.Sql(q1);
			migrationBuilder.Sql(q2);
			migrationBuilder.Sql(q3);
			migrationBuilder.Sql(q4);








		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
