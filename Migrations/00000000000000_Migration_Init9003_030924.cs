using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9003_030924 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMission", "Type", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMission", "NumeroSerie", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMission", "EtendueResolution", "100"));

			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMissionLigne", "Type", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMissionLigne", "NumeroSerie", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifRapportMissionLigne", "EtendueResolution", "100"));

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
