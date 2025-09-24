using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration3_250924_1250 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE TABLE dbo.T_Depot ( 
	                                id                   int NOT NULL   IDENTITY ,
	                                UserName             varchar(100)     ,
	                                Societe              int NOT NULL CONSTRAINT defo_T_Depot_Societe DEFAULT 0   ,
	                                DE_No                int NOT NULL CONSTRAINT defo_T_Depot_DE_No DEFAULT 0   ,
	                                Visible              bit NOT NULL CONSTRAINT defo_T_Depot_Visible DEFAULT 1   ,
	                                CONSTRAINT Pk_T_Collaborateur_id_0 PRIMARY KEY  ( id )
                                 );

                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
