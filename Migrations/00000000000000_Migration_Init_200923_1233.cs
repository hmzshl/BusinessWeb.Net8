using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_200923_1233 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string q6 = @"IF NOT EXISTS (SELECT 1
                                                            FROM INFORMATION_SCHEMA.COLUMNS
                                                            WHERE upper(TABLE_NAME) = 'F_CREGLEMENT'
                                                            AND upper(COLUMN_NAME) = 'Remarque')
                                                    BEGIN
                                                        ALTER TABLE dbo.F_CREGLEMENT ADD Remarque VARCHAR(100);
                                                    END
                                                    ";
            string q8 = @"IF OBJECT_ID('[API_V_CREGLEMENT]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_CREGLEMENT];
                                            END
                                            GO

                                            CREATE VIEW [dbo].[API_V_CREGLEMENT]
                                            AS
                                            SELECT 
                                            a.cbMarq,
                                            a.RG_No,
                                            a.RG_Date,
                                            a.RG_Piece,
                                            a.RG_Reference,
                                            a.RG_Libelle,
                                            ct.CT_Num,
                                            ct.CT_Intitule,
                                            jr.JO_Num,
                                            jr.JO_Intitule,
                                            CASE WHEN YEAR(a.RG_DateEchCont) > 2000 THEN FORMAT(a.RG_DateEchCont,'dd/MM/yyyy') END RG_DateEcheance,
                                            CASE WHEN a.RG_DateEchCont>GETDATE() THEN 'Non echu' ELSE 'Echu' END Echeance,
                                            ISNULL(mo.R_Intitule,'Autre') R_Intitule,
                                            CASE WHEN ct.CT_Type = 0 THEN 'Client' ELSE 'Fournisseur' END CT_Type,
                                            a.RG_Montant,
                                            CASE WHEN ct.CT_Type = 0 THEN a.RG_Montant END Debit,
                                            CASE WHEN ct.CT_Type = 1 THEN a.RG_Montant END Credit,
                                            a.RG_Type,
                                            a.Encaiss,
                                            a.Impaye,
                                            a.Incorpore,
                                            a.Depose,
                                            a.Rappro,
                                            a.DateDepot,
                                            a.DatePaie,
                                            a.Remarque,
                                            jr.CG_Num


                                            FROM F_CREGLEMENT a
                                            LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
                                            INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
                                            INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
                                            GO


                                            ";

            migrationBuilder.Sql(q6);
            migrationBuilder.Sql(q8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
