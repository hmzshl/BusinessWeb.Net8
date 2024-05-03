using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init6_240324_1146 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "PreuvesRaccordChar", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "ListePersonnelQualifieChar", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "AuditAuLaboratoireChar", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "PMQChar", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "ProcedureTraitementReclamationChar", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "DemandeParAutre", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "PeriodiciteEtalonnageEtiquettesPeriode", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "PeriodiciteEtalonnageRapportsPeriode", "100"));
			migrationBuilder.Sql(fn.AddVarchar("API_T_CertifGrilleDialogue", "PointsEtalonnageAutre", "100"));



			migrationBuilder.Sql(fn.AddInt("API_T_CertifGrilleDialogue", "PriseChargeTransportInt"));
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
