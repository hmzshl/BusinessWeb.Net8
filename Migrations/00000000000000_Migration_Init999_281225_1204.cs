using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_281225_1204 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE TABLE dbo.API_T_AREdit_Hist ( 
							id                   int NOT NULL   IDENTITY ,
							DateOP               datetime     ,
							OldDate              datetime     ,
							OldPiece             varchar(40)     ,
							OldCT_Num            varchar(40)     ,
							OldAR_Ref            varchar(40)     ,
							OldDesign            varchar(100)     ,
							OldQte               decimal(24,6)     ,
							OldPU                decimal(24,6)     ,
							OldTaxe              decimal(24,6)     ,
							OldRem               decimal(24,6)     ,
							OldHT                decimal(24,6)     ,
							OldTTC               decimal(24,6)     ,
							NewDate              datetime     ,
							NewPiece             varchar(40)     ,
							NewCT_Num            varchar(40)     ,
							NewAR_Ref            varchar(40)     ,
							NewDesign            varchar(100)     ,
							NewQte               decimal(24,6)     ,
							NewPU                decimal(24,6)     ,
							NewTaxe              decimal(24,6)     ,
							NewRem               decimal(24,6)     ,
							NewHT                decimal(24,6)     ,
							NewTTC               decimal(24,6)     ,
							OldCT_Intitule       varchar(100)     ,
							NewCT_Intitule       varchar(100)     ,
							OldcbMarq            int     ,
							NewcbMarq            int     ,
							CONSTRAINT Pk_API_T_AREdit_Hist_id PRIMARY KEY  ( id ) 
						 );
						";
			string q2 = @"CREATE VIEW [dbo].[API_V_ORDREFABRICATION]
						AS
						SELECT 
							a.*,
							ct.CT_Intitule,
							ca.CA_Intitule,
							STUFF((
								SELECT CHAR(9) + FORMAT(ma2.Qte,'0') + ' ' +ar.AR_Design+'; '
								FROM API_T_OrdreFabricationLigne ma2
								LEFT JOIN F_ARTICLE ar ON ma2.AR_Ref = ar.AR_Ref
								WHERE ma2.Ordre = a.id
								FOR XML PATH(''), TYPE
							).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS AR_Refs
						FROM API_T_OrdreFabrication a
						LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
						LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
						GO";
			migrationBuilder.Sql(q1);
			migrationBuilder.Sql(q2);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
