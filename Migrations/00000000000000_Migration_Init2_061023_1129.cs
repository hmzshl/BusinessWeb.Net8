using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_061023_1129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE API_T_Attachement ADD MontantTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_AttachementDetail ADD MontantTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_AttachementDetail ADD PUTTC DECIMAL(24,6) DEFAULT 0");
            migrationBuilder.Sql("ALTER TABLE API_T_AttachementDetail ADD Taxe DECIMAL(24,6) DEFAULT 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
