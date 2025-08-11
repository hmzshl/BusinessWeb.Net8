using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250801_1604 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifPointage", "Intermediaire", "255"));

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
