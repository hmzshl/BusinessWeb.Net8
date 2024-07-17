using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9_170724_1548 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_ARTICLEMVT]
				AS
				SELECT 
				a.DO_Date,
				a.DO_Piece,
				a.DL_DateBL,
				a.DL_PieceBL,
				CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule,
				CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
				CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
				WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
				WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
				WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule,
				a.AR_Ref,
				ar.AR_Design,
				a.CT_Num,
				CASE WHEN a.DO_Domaine IN (0,1,4) THEN ct.CT_Intitule ELSE de2.DE_Intitule END TiersIntitule,
				a.DE_No,
				de.DE_Intitule,
				a.DL_Qte,
				CASE WHEN a.DL_MvtStock = 1 THEN ABS(a.DL_Qte) ELSE -ABS(a.DL_Qte) END Mvt,
				CASE WHEN a.DL_MvtStock = 1 THEN 'Entrée' ELSE 'Sortie' END Sense,
				fa.FA_CodeFamille,
				fa.FA_Intitule,
				a.DO_Type,
				a.DO_Domaine,
				un.U_Intitule



				FROM F_DOCLIGNE a
				INNER JOIN F_DEPOT de ON a.DE_No = de.DE_No
				INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
				INNER JOIN F_FAMILLE fa ON ar.FA_CodeFamille = fa.FA_CodeFamille
				LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				LEFT JOIN F_DEPOT de2 ON a.CT_Num = CAST(de2.DE_No AS VARCHAR(3))
				LEFT JOIN P_UNITE un ON ar.AR_UniteVen = un.cbIndice

				WHERE a.DL_MvtStock IN (1,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
