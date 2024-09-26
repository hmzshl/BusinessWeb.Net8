using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init99_240926_1158 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			Helpers fn = new Helpers();
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifOuvertureDossier", "CT_Num", "30"));
			migrationBuilder.Sql(fn.AddBool("API_T_CertifGrilleDialogue", "OldCompte"));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
