using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_140324_1013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "MethodeEtalonnange"));
            migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "AvisPertinenceMethode"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "Commentaire","255"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "MethodeEtalonnageAutre","100"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
