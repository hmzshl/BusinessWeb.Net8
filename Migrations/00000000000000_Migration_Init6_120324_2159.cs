using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_120324_2159 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "LieuPrestationInt"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "OperationsAttenduesInt"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "PreuvesRaccordRes"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "ListePersonnelQualifieRes"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "AuditAuLaboratoireRes"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "PMQRes"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "ProcedureTraitementReclamationRes"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
