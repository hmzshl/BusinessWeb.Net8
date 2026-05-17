using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9999__260517 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_MaterielEntretien", "NumOF", "100"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
