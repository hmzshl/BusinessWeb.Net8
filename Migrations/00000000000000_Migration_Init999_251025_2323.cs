using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_251025_2323 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddDecimal("API_T_OrdreFabricationDetail", "QteStock"));
            migrationBuilder.Sql(fn.AddDecimal("API_T_OrdreFabricationDetail", "QtePreparation"));
            migrationBuilder.Sql(fn.AddBool("API_T_OrdreFabrication", "StockCreated"));
        }

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
