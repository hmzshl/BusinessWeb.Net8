using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration3_250922_1629 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.T_SocieteUser ( 
	                                id                   int NOT NULL   IDENTITY ,
	                                UserID               nvarchar(450)     ,
	                                Societe              int NOT NULL CONSTRAINT defo_T_SocieteUser_Societe DEFAULT 0   ,
	                                CONSTRAINT Pk_T_SocieteUser_id PRIMARY KEY  ( id )
                                 );");

            migrationBuilder.Sql(@"CREATE TABLE dbo.T_AuthorizeMobile ( 
	                                id                   int NOT NULL   IDENTITY ,
	                                UserID               nvarchar(450)     ,
	                                Societe              int NOT NULL CONSTRAINT defo_T_AuthorizeMobile_Societe DEFAULT 0   ,
	                                RoleID               nvarchar(450)     ,
	                                Url                  nvarchar(250)     ,
	                                Visible              bit NOT NULL CONSTRAINT defo_T_AuthorizeMobile_Visible DEFAULT 0   ,
	                                CONSTRAINT Pk_T_AuthorizeMobile_id PRIMARY KEY  ( id )
                                 );
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
