using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250416_1737 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddVarchar("API_T_OrdreFabrication", "NumCommande", "40"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_OrdreFabrication", "NumPreparationCommande", "40"));
			migrationBuilder.Sql(fn.AddBool("API_T_OrdreFabrication", "PreparationCreated"));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
