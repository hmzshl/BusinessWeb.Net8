using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init5_130224_1558 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifEntete", "NumeroDossier", "100"));
            migrationBuilder.Sql(fn.AddCol("API_T_CertifEntete", "Remarque", "TEXT"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
