using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration3_2601216_2237 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string q1 = @"-- Add missing columns from DT_Societe to T_Societe table
ALTER TABLE dbo.T_Societe
ADD 
    -- Fields from DT_Societe
    IFU NVARCHAR(50) NULL,
    Responsable NVARCHAR(100) NULL,
    Statut NVARCHAR(50) NULL,
    Regime INT NULL,
    Periode INT NULL,
    SourceRapprochement INT NULL,
    SourceDetailsFacture INT NULL,
    JO_Banques NVARCHAR(MAX) NULL,
    JO_ANouveaux NVARCHAR(MAX) NULL,
    JO_Achats NVARCHAR(MAX) NULL,
    JO_Ventes NVARCHAR(MAX) NULL,
    Rapprochement BIT NULL,
    JO_Caisses NVARCHAR(MAX) NULL,
    SensEncaissement INT NULL,
    SensDecaissement INT NULL,
    NumFA_Achat INT NULL,
    NumFA_Vente INT NULL,
    CG_Inclures NVARCHAR(MAX) NULL,
    CG_Inclures_VE NVARCHAR(MAX) NULL,
    RapprochementSur BIT NULL;";

			migrationBuilder.Sql(q1);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
