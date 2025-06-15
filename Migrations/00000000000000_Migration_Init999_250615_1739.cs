using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250615_1739 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddDate("API_T_OrdreFabrication", "DateLivraison"));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
