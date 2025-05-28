using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250528_2318 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE TABLE dbo.API_T_CertifPointage ( 
	id                   int NOT NULL   IDENTITY ,
	Date                 smalldatetime     ,
	CT_Num               varchar(17)     ,
	Ville                nvarchar(100)     ,
	DateDebutEtalonnage  smalldatetime     ,
	DateFinEtalonnage    smalldatetime     ,
	ResponsableEtalonnage int     ,
	DateReceptionDossier smalldatetime     ,
	DateDebutSaisie      smalldatetime     ,
	DateFinSaisie        smalldatetime     ,
	NombreCertificats    int     ,
	ResponsableSaisie    int     ,
	ResponsableImpression int     ,
	VerificationAvantImpression int     ,
	VerificationApresImpression int     ,
	Certificats          bit     ,
	VerificationScan     bit     ,
	Facture              bit     ,
	BonDeLivraison       bit     ,
	NumeroFACBL          nvarchar(50)     ,
	DateLivraison        smalldatetime     ,
	Livreur              nvarchar(100)     ,
	AccuseReception      bit     ,
	Commentaire          nvarchar(max)     ,
	DateCreation         smalldatetime  CONSTRAINT [defo_API_T_CertifPointage_DateCreation] DEFAULT getdate()   ,
	DateModification     smalldatetime  CONSTRAINT [defo_API_T_CertifPointage_DateModification] DEFAULT getdate()   ,
	ResponsableEtalonnage2 int     ,
	ResponsableEtalonnage3 int     ,
	Statut               int     ,
	CONSTRAINT PK__API_T_Ce__3213E83FA8292B7A PRIMARY KEY  ( id ) 
 );

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage FOREIGN KEY ( CT_Num ) REFERENCES dbo.F_COMPTET( CT_Num ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage2 FOREIGN KEY ( ResponsableEtalonnage ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage3 FOREIGN KEY ( ResponsableSaisie ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage4 FOREIGN KEY ( ResponsableImpression ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage5 FOREIGN KEY ( ResponsableEtalonnage2 ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage6 FOREIGN KEY ( ResponsableEtalonnage3 ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage7 FOREIGN KEY ( VerificationAvantImpression ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.API_T_CertifPointage ADD CONSTRAINT fk_api_t_certifpointage8 FOREIGN KEY ( VerificationApresImpression ) REFERENCES dbo.API_T_Personnel( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
";

			migrationBuilder.Sql(q1);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
