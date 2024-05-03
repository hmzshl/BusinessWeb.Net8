using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_250923_1041 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();

            migrationBuilder.Sql(fn.AddBool("T_Societe", "HistoriqueConnexion"));
            migrationBuilder.Sql(fn.AddInt("T_Societe", "VersionSage"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
