using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_281225_1204 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE TABLE [dbo].[API_T_AREdit_Hist](
						[id] [int] IDENTITY(1,1) NOT NULL,
						[DateOP] [datetime] NULL,
						[OldDate] [datetime] NULL,
						[OldPiece] [varchar](40) NULL,
						[OldCT_Num] [varchar](40) NULL,
						[OldAR_Ref] [varchar](40) NULL,
						[OldDesign] [varchar](100) NULL,
						[OldQte] [decimal](24, 6) NULL,
						[OldPU] [decimal](24, 6) NULL,
						[OldTaxe] [decimal](24, 6) NULL,
						[OldRem] [decimal](24, 6) NULL,
						[OldHT] [decimal](24, 6) NULL,
						[OldTTC] [decimal](24, 6) NULL,
						[NewDate] [datetime] NULL,
						[NewPiece] [varchar](40) NULL,
						[NewCT_Num] [varchar](40) NULL,
						[NewAR_Ref] [varchar](40) NULL,
						[NewDesign] [varchar](100) NULL,
						[NewQte] [decimal](24, 6) NULL,
						[NewPU] [decimal](24, 6) NULL,
						[NewTaxe] [decimal](24, 6) NULL,
						[NewRem] [decimal](24, 6) NULL,
						[NewHT] [decimal](24, 6) NULL,
						[NewTTC] [decimal](24, 6) NULL,
						[OldCT_Intitule] [varchar](100) NULL,
						[NewCT_Intitule] [varchar](100) NULL,
						[OldcbMarq] [int] NULL,
						[NewcbMarq] [int] NULL,
					 CONSTRAINT [Pk_API_T_AREdit_Hist_id] PRIMARY KEY CLUSTERED 
					(
						[id] ASC
					)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
					) ON [PRIMARY]
					GO";
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
