using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init99_261026_1140 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_CertifSuiviDossier ( 
	OrdreMissionDate     smalldatetime     ,
	OrdreMissionDateExist bit     ,
	OrdreMissionRef      varchar(100)     ,
	OrdreMissionRemarque text     ,
	RapportMissionDate   smalldatetime     ,
	RapportMissionDateExist bit     ,
	RapportMissionRef    varchar(100)     ,
	RapportMissionRemarque text     ,
	BordereauEnvoiDate   smalldatetime     ,
	BordereauEnvoiDateExist bit     ,
	BordereauEnvoiRef    varchar(100)     ,
	BordereauEnvoiRemarque text     ,
	FicheReceptionDate   smalldatetime     ,
	FicheReceptionDateExist bit     ,
	FicheReceptionRef    varchar(100)     ,
	FicheReceptionRemarque text     ,
	DechargeMaterielDate smalldatetime     ,
	DechargeMaterielDateExist bit     ,
	DechargeMaterielRef  varchar(100)     ,
	DechargeMaterielRemarque text     ,
	EnqueteSatisfactionDate smalldatetime     ,
	EnqueteSatisfactionDateExist bit     ,
	EnqueteSatisfactionRef varchar(100)     ,
	EnqueteSatisfactionRemarque text     ,
	FeuilleMesureDate    smalldatetime     ,
	FeuilleMesureDateExist bit     ,
	FeuilleMesureRef     varchar(100)     ,
	FeuilleMesureRemarque text     ,
	CertificatEtalonnageDate smalldatetime     ,
	CertificatEtalonnageDateExist bit     ,
	CertificatEtalonnageRef varchar(100)     ,
	CertificatEtalonnageRemarque text     ,
	FeuilleCalculIncertitudeDate smalldatetime     ,
	FeuilleCalculIncertitudeDateExist bit     ,
	FeuilleCalculIncertitudeRef varchar(100)     ,
	FeuilleCalculIncertitudeRemarque text     ,
	ConstatsVerificationDate smalldatetime     ,
	ConstatsVerificationDateExist bit     ,
	ConstatsVerificationRef varchar(100)     ,
	ConstatsVerificationRemarque text     ,
	FicheAnomalieTechniqueDate smalldatetime     ,
	FicheAnomalieTechniqueDateExist bit     ,
	FicheAnomalieTechniqueRef varchar(100)     ,
	FicheAnomalieTechniqueRemarque text     ,
	AutresDate           smalldatetime     ,
	AutresDateExist      bit     ,
	AutresRef            varchar(100)     ,
	AutresRemarque       text     ,
	id                   int NOT NULL   IDENTITY ,
	Date                 smalldatetime     ,
	Piece                varchar(100)     ,
	CT_Num               varchar(100)     ,
	CONSTRAINT Pk_API_T_CertifSuiviDossier_id PRIMARY KEY  ( id ) 
 );

                                ");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
