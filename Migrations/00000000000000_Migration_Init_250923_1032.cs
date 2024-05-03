using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_250923_1032 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string q1 = @"CREATE TABLE [dbo].[API_T_HistoriqueConnexion](
	                            [id] [int] IDENTITY(1,1) NOT NULL,
	                            [Utilisateur] [varchar](100) NULL,
	                            [AdressIP] [varchar](100) NULL,
	                            [Computer] [varchar](100) NULL,
	                            [SessionWindows] [varchar](100) NULL,
	                            [DateOP] [smalldatetime] NULL,
	                            [TypeOP] [varchar](100) NULL,
                             CONSTRAINT [Pk_T_HistoriqueConnexion_id] PRIMARY KEY CLUSTERED 
                            (
	                            [id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                            GO";

            migrationBuilder.Sql(q1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
