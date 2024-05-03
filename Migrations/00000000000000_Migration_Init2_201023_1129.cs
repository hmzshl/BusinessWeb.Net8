using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_201023_1129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE TABLE dbo.API_T_Config ( 
	id                   int NOT NULL   IDENTITY ,
	BL_Releve            bit  CONSTRAINT defo_API_T_Config_BL_Releve DEFAULT 1   ,
	BL_BalanceClient     bit  CONSTRAINT defo_API_T_Config_BL_BalanceClient DEFAULT 1   ,
	BL_BalanceFournisseur bit  CONSTRAINT defo_API_T_Config_BL_BalanceFournisseur DEFAULT 1   ,
	BL_Marge             bit  CONSTRAINT defo_API_T_Config_BL_Marge DEFAULT 1   ,
	FA_Releve            bit  CONSTRAINT defo_API_T_Config_FA_Releve DEFAULT 1   ,
	FA_BalanceClient     bit  CONSTRAINT defo_API_T_Config_FA_BalanceClient DEFAULT 1   ,
	FA_BalanceFournisseur bit  CONSTRAINT defo_API_T_Config_FA_BalanceFournisseur DEFAULT 1   ,
	FA_Marge             bit  CONSTRAINT defo_API_T_Config_FA_Marge DEFAULT 1   ,
	CONSTRAINT Pk_API_T_Config_id PRIMARY KEY  ( id )
 );");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
