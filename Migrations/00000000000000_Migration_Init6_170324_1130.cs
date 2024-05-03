using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_170324_1130 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifDocument", "Reference", "100"));
			migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "Statut"));
			migrationBuilder.Sql(fn.AddInt("API_T_CertifOuvertureDossier", "Statut"));
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
