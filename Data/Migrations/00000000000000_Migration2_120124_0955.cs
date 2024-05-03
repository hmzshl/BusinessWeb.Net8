using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration2_120124_0955 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE dbo.T_Collaborateur ( 
	                            id                   int NOT NULL   IDENTITY ,
	                            UserName             varchar(100)     ,
	                            Societe              int NOT NULL CONSTRAINT defo_T_Collaborateur_Societe DEFAULT 0   ,
	                            CO_No                int NOT NULL CONSTRAINT defo_T_Collaborateur_CO_No DEFAULT 0   ,
	                            CONSTRAINT Pk_T_Collaborateur_id PRIMARY KEY  ( id )
                             );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
