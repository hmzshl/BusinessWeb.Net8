using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250414_1135 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"/*
								Run this script on:

										(local).BIJOU_COPIE    -  This database will be modified

								to synchronize it with:

										(local).BIJOU

								You are recommended to back up your database before running this script

								Script created by SQL Compare version 15.4.13.28096 from Red Gate Software Ltd at 14/04/2025 12:40:59

								*/
								SET NUMERIC_ROUNDABORT OFF
								GO
								SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
								GO
								SET XACT_ABORT ON
								GO
								SET TRANSACTION ISOLATION LEVEL Serializable
								GO
								BEGIN TRANSACTION
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating [dbo].[API_T_OrdreFabrication]'
								GO
								CREATE TABLE [dbo].[API_T_OrdreFabrication]
								(
								[id] [int] NOT NULL IDENTITY(1, 1),
								[Date] [smalldatetime] NULL,
								[Numero] [varchar] (50) COLLATE French_CI_AS NULL,
								[Reference] [varchar] (50) COLLATE French_CI_AS NULL,
								[CA_Num] [varchar] (13) COLLATE French_CI_AS NULL,
								[CT_Num] [varchar] (17) COLLATE French_CI_AS NULL,
								[Responsable] [int] NULL,
								[Statut] [int] NOT NULL CONSTRAINT [defo_API_T_OrdreFabrication_Statut] DEFAULT ((0))
								)
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating primary key [Pk_API_T_OrdreFabrication_id] on [dbo].[API_T_OrdreFabrication]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabrication] ADD CONSTRAINT [Pk_API_T_OrdreFabrication_id] PRIMARY KEY CLUSTERED ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating [dbo].[API_T_OrdreFabricationLigne]'
								GO
								CREATE TABLE [dbo].[API_T_OrdreFabricationLigne]
								(
								[id] [int] NOT NULL IDENTITY(1, 1),
								[Ordre] [int] NOT NULL CONSTRAINT [defo_API_T_OrderFabricationLigne_Ordre] DEFAULT ((0)),
								[AR_Ref] [varchar] (19) COLLATE French_CI_AS NULL,
								[Qte] [decimal] (24, 6) NOT NULL CONSTRAINT [defo_API_T_OrderFabricationLigne_Qte] DEFAULT ((0)),
								[FraisU] [decimal] (24, 6) NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationLigne_FraisU] DEFAULT ((0))
								)
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating primary key [Pk_API_T_OrderFabricationLigne_id] on [dbo].[API_T_OrdreFabricationLigne]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationLigne] ADD CONSTRAINT [Pk_API_T_OrderFabricationLigne_id] PRIMARY KEY CLUSTERED ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating [dbo].[API_T_OrdreFabricationDetail]'
								GO
								CREATE TABLE [dbo].[API_T_OrdreFabricationDetail]
								(
								[id] [int] NOT NULL IDENTITY(1, 1),
								[Ligne] [int] NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationDetail_Ligne] DEFAULT ((0)),
								[AR_Ref] [varchar] (19) COLLATE French_CI_AS NULL,
								[Qte] [decimal] (24, 6) NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationDetail_Qte] DEFAULT ((0)),
								[FraisU] [decimal] (24, 6) NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationDetail_FraisU] DEFAULT ((0))
								)
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating primary key [Pk_API_T_OrdreFabricationDetail_id] on [dbo].[API_T_OrdreFabricationDetail]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationDetail] ADD CONSTRAINT [Pk_API_T_OrdreFabricationDetail_id] PRIMARY KEY CLUSTERED ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating [dbo].[API_T_OrdreFabricationOperation]'
								GO
								CREATE TABLE [dbo].[API_T_OrdreFabricationOperation]
								(
								[id] [int] NOT NULL IDENTITY(1, 1),
								[Ligne] [int] NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationOperation_Ligne] DEFAULT ((0))
								)
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating primary key [Pk_API_T_OrdreFabricationOperation_id] on [dbo].[API_T_OrdreFabricationOperation]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationOperation] ADD CONSTRAINT [Pk_API_T_OrdreFabricationOperation_id] PRIMARY KEY CLUSTERED ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating [dbo].[API_T_OrdreFabricationPointage]'
								GO
								CREATE TABLE [dbo].[API_T_OrdreFabricationPointage]
								(
								[id] [int] NOT NULL IDENTITY(1, 1),
								[Ordre] [int] NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationPointage_Ordre] DEFAULT ((0)),
								[Presonnel] [int] NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationPointage_Presonnel] DEFAULT ((0)),
								[Date] [smalldatetime] NULL,
								[Debut] [time] NULL,
								[Fin] [time] NULL,
								[FraisJournalier] [decimal] (24, 6) NOT NULL CONSTRAINT [DF__API_T_Ord__Frais__5C2696C7] DEFAULT ((0)),
								[AutresFrais] [decimal] (24, 6) NOT NULL CONSTRAINT [defo_API_T_OrdreFabricationPointage_AutresFrais] DEFAULT ((0))
								)
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Creating primary key [Pk_API_T_OrdreFabricationPointage_id] on [dbo].[API_T_OrdreFabricationPointage]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationPointage] ADD CONSTRAINT [Pk_API_T_OrdreFabricationPointage_id] PRIMARY KEY CLUSTERED ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Adding foreign keys to [dbo].[API_T_OrdreFabricationDetail]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationDetail] ADD CONSTRAINT [fk_api_t_ordrefabricationdetail] FOREIGN KEY ([Ligne]) REFERENCES [dbo].[API_T_OrdreFabricationLigne] ([id])
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationDetail] ADD CONSTRAINT [fk_api_t_ordrefabricationdetail_2] FOREIGN KEY ([AR_Ref]) REFERENCES [dbo].[F_ARTICLE] ([AR_Ref]) ON DELETE CASCADE
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Adding foreign keys to [dbo].[API_T_OrdreFabricationLigne]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationLigne] ADD CONSTRAINT [fk_api_t_orderfabricationligne] FOREIGN KEY ([Ordre]) REFERENCES [dbo].[API_T_OrdreFabrication] ([id]) ON DELETE CASCADE
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationLigne] ADD CONSTRAINT [fk_api_t_ordrefabricationligne] FOREIGN KEY ([AR_Ref]) REFERENCES [dbo].[F_ARTICLE] ([AR_Ref]) ON DELETE CASCADE
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Adding foreign keys to [dbo].[API_T_OrdreFabricationOperation]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationOperation] ADD CONSTRAINT [fk_api_t_ordrefabricationoperation] FOREIGN KEY ([Ligne]) REFERENCES [dbo].[API_T_OrdreFabricationLigne] ([id]) ON DELETE CASCADE
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Adding foreign keys to [dbo].[API_T_OrdreFabricationPointage]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationPointage] ADD CONSTRAINT [fk_api_t_ordrefabricationpointage] FOREIGN KEY ([Ordre]) REFERENCES [dbo].[API_T_OrdreFabrication] ([id]) ON DELETE CASCADE
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabricationPointage] ADD CONSTRAINT [fk_api_t_ordrefabricationpointage_3] FOREIGN KEY ([Presonnel]) REFERENCES [dbo].[API_T_Personnel] ([id])
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								PRINT N'Adding foreign keys to [dbo].[API_T_OrdreFabrication]'
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabrication] ADD CONSTRAINT [fk_api_t_ordrefabrication_2] FOREIGN KEY ([CT_Num]) REFERENCES [dbo].[F_COMPTET] ([CT_Num]) ON DELETE CASCADE
								GO
								ALTER TABLE [dbo].[API_T_OrdreFabrication] ADD CONSTRAINT [fk_api_t_ordrefabrication_3] FOREIGN KEY ([Responsable]) REFERENCES [dbo].[API_T_Personnel] ([id]) ON DELETE CASCADE
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								COMMIT TRANSACTION
								GO
								IF @@ERROR <> 0 SET NOEXEC ON
								GO
								-- This statement writes to the SQL Server Log so SQL Monitor can show this deployment.
								IF HAS_PERMS_BY_NAME(N'sys.xp_logevent', N'OBJECT', N'EXECUTE') = 1
								BEGIN
									DECLARE @databaseName AS nvarchar(2048), @eventMessage AS nvarchar(2048)
									SET @databaseName = REPLACE(REPLACE(DB_NAME(), N'\', N'\\'), N'""', N'\""')
									SET @eventMessage = N'Redgate SQL Compare: { ""deployment"": { ""description"": ""Redgate SQL Compare deployed to ' + @databaseName + N'"", ""database"": ""' + @databaseName + N'"" }}'
									EXECUTE sys.xp_logevent 55000, @eventMessage
								END
								GO
								DECLARE @Success AS BIT
								SET @Success = 1
								SET NOEXEC OFF
								IF (@Success = 1) PRINT 'The database update succeeded'
								ELSE BEGIN
									IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
									PRINT 'The database update failed'
								END
								GO
								");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
