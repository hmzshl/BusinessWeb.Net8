using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_250923_1127 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_ICE", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_Patente", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_IdF", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_RC", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_Telephone", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_Email", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_Web", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_Adresse", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_ICE", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_ICE", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_ICE", "100"));
            migrationBuilder.Sql(fn.AddVarchar("P_DOSSIER", "API_ICE", "100"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
