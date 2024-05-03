using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration1_091123_1216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.T_Authorize ( 
	id                   int NOT NULL   IDENTITY ,
	Societe              int NOT NULL CONSTRAINT defo_T_Authorize_Societe DEFAULT 0   ,
	SelectedAPP          int NOT NULL CONSTRAINT defo_T_Authorize_SelectedAPP DEFAULT 0   ,
	UserID               nvarchar(450)     ,
	Title                nvarchar(100)     ,
	Url                  nvarchar(100)     ,
	Description          nvarchar(100)     ,
	Visible              bit NOT NULL CONSTRAINT defo_T_Authorize_Visible DEFAULT 1   ,
	CONSTRAINT Pk_T_Authorize_id PRIMARY KEY  ( id )
 );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
