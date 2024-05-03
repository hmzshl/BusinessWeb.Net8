using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_051023_1728 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE API_T_Bordereau ADD MontantTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_BordereauDetail ADD MontantTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_BordereauDetail ADD PUTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_BordereauDetail ADD Taxe DECIMAL(24,6) DEFAULT 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
