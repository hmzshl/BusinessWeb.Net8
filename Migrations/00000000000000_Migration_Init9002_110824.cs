using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9002_110824 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddInt("API_T_CertifRapportMissionLigne", "Ligne"));
			migrationBuilder.Sql(fn.AddInt("API_T_CertifOrdreMissionLigne", "Ligne"));

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
