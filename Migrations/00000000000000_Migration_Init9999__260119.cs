using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9999__260119 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE VIEW API_V_NOTEFRAISENTETE
							AS
							SELECT 
							pr.Nom,
							mt.Intitule,
							co.CO_Nom,
							ca.CA_Intitule,
							ct.CT_Intitule,
							a.*
							FROM API_T_NoteFraisEntete a
							LEFT JOIN API_T_Personnel pr ON a.Personnel = pr.id
							LEFT JOIN API_T_Materiel mt ON a.Materiel = mt.id
							LEFT JOIN F_COLLABORATEUR co ON a.Representant = co.CO_No
							LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
							LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num";
			migrationBuilder.Sql(q1);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
