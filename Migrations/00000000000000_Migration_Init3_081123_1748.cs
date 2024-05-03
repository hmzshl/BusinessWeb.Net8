using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init3_081123_1748 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_Authorize ( 
	id                   int NOT NULL   IDENTITY ,
	UserID               nvarchar(450)     ,
	Url                  nvarchar(100)     ,
	SelectedAPP          int  CONSTRAINT defo_API_T_Authorize_SelectedAPP DEFAULT 0   ,
	CONSTRAINT Pk_API_T_Authorize_id PRIMARY KEY  ( id )
 );");

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
