using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9_080724_1237 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddBool("API_T_CaisseEntete", "Valide"));
            migrationBuilder.Sql(fn.AddDate("API_T_CaisseEntete", "ValideDate"));

            migrationBuilder.Sql(fn.AddVarchar("API_T_Config", "EmailSmtpServer", "100"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_Config", "EmailPort", "100"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_Config", "EmailSenderEmail", "100"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_Config", "EmailPassword", "100"));
            migrationBuilder.Sql(fn.AddVarchar("API_T_Config", "EmailReleveObjet", "255"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
