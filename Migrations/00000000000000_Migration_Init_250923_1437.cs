using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_250923_1437 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_HistoriqueConnexion", "CB_Type", "10"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
