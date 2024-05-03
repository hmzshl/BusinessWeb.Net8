using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init5_190224_1810 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_DOCENTETE'
                                                            AND upper(COLUMN_NAME) = 'CT_NumOld')
                                                    BEGIN
                                                        ALTER TABLE F_DOCENTETE ADD CT_NumOld VARCHAR(40);
                                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
