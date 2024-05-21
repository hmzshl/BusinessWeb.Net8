using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init8_210524_1600 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "NaturePrestationAutre", "100"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogueExigence", "Jugement"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogueExigence", "RegleDecision"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogueExigence", "ErreurMaximalTolere"));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
