using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init4_211223_1141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_ARTICLESTOCK]
				AS
				SELECT 
				a.AR_Ref,
				ar.AR_Design,
				a.DE_No,
				de.DE_Intitule,
				a.AS_QteSto,
				a.AS_MontSto,
				a.AS_QteCom,
				a.AS_QtePrepa,
				a.AS_QteRes,
				a.AS_QteMini,
				a.AS_QteMaxi,
				fa.FA_CodeFamille,
				fa.FA_Intitule
				FROM F_ARTSTOCK a
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				INNER JOIN F_DEPOT de ON a.DE_No = de.DE_No
				LEFT JOIN F_FAMILLE fa ON ar.FA_CodeFamille = fa.FA_CodeFamille");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
