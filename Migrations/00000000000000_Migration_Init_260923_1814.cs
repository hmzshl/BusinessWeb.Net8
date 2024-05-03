using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_260923_1814 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string q1 = @"IF OBJECT_ID('[API_V_AUDIT_F_DOCLIGNE]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_DOCLIGNE];
                                            END
                                            GO
                                            CREATE VIEW [dbo].[API_V_AUDIT_F_DOCLIGNE]
                                            AS
                                            SELECT  a.cbMarq
                                                  ,a.DO_Domaine
                                                  ,a.DO_Type
                                                  ,a.CT_Num
                                                  ,a.DO_Piece
                                                  ,a.DL_PieceBC
                                                  ,a.DL_PieceBL
                                                  ,a.DO_Date
                                                  ,a.DL_DateBC
                                                  ,a.DL_DateBL
                                                  ,a.DL_Ligne
                                                  ,a.DO_Ref
                                                  ,a.DL_TNomencl
                                                  ,a.DL_TRemPied
                                                  ,a.DL_TRemExep
                                                  ,a.AR_Ref
                                                  ,a.DL_Design
                                                  ,a.DL_Qte
                                                  ,a.DL_QteBC
                                                  ,a.DL_QteBL
                                                  ,a.DL_PoidsNet
                                                  ,a.DL_PoidsBrut
                                                  ,a.DL_Remise01REM_Valeur
                                                  ,a.DL_Remise01REM_Type
                                                  ,a.DL_Remise02REM_Valeur
                                                  ,a.DL_Remise02REM_Type
                                                  ,a.DL_Remise03REM_Valeur
                                                  ,a.DL_Remise03REM_Type
                                                  ,a.DL_PrixUnitaire
                                                  ,a.DL_PUBC
                                                  ,a.DL_Taxe1
                                                  ,a.DL_TypeTaux1
                                                  ,a.DL_TypeTaxe1
                                                  ,a.DL_Taxe2
                                                  ,a.DL_TypeTaux2
                                                  ,a.DL_TypeTaxe2
                                                  ,a.CO_No
                                                  ,a.AG_No1
                                                  ,a.AG_No2
                                                  ,a.DL_PrixRU
                                                  ,a.DL_CMUP
                                                  ,a.DL_MvtStock
                                                  ,a.DT_No
                                                  ,a.AF_RefFourniss
                                                  ,a.EU_Enumere
                                                  ,a.EU_Qte
                                                  ,a.DL_TTC
                                                  ,a.DE_No
                                                  ,a.DL_NoRef
                                                  ,a.DL_TypePL
                                                  ,a.DL_PUDevise
                                                  ,a.DL_PUTTC
                                                  ,a.DL_No
                                                  ,a.DO_DateLivr
                                                  ,a.CA_Num
                                                  ,a.DL_Taxe3
                                                  ,a.DL_TypeTaux3
                                                  ,a.DL_TypeTaxe3
                                                  ,a.DL_Frais
                                                  ,a.DL_Valorise
                                                  ,a.AR_RefCompose
                                                  ,a.DL_NonLivre
                                                  ,a.AC_RefClient
                                                  ,a.DL_MontantHT
                                                  ,a.DL_MontantTTC
                                                  ,a.DL_FactPoids
                                                  ,a.DL_Escompte
                                                  ,a.DL_PiecePL
                                                  ,a.DL_DatePL
                                                  ,a.DL_QtePL
                                                  ,a.DL_NoColis
                                                  ,a.DL_NoLink
                                                  ,a.RP_Code
                                                  ,a.DL_QteRessource
                                                  ,a.DL_DateAvancement
                                                  ,a.PF_Num
                                                  ,a.DL_CodeTaxe1
                                                  ,a.DL_CodeTaxe2
                                                  ,a.DL_CodeTaxe3
	                                              ,ct.CT_Intitule,
	                                              de.DE_Intitule,
	                                              CASE a.DO_Domaine WHEN  0 THEN 'Ventes' WHEN 1 THEN 'Achats' WHEN 2 THEN 'Stocks' WHEN 4 THEn 'Internes' ELSE 'Autres' END DomaineIntitule,
	                                              CASE WHEN a.DO_Domaine = 40 THEN 'Document interne' ELSE 
	                                              CASE a.DO_Type WHEN 0 THEN 'DE' WHEN 1 THEN 'BC Client' WHEN 2 THEN 'PL' WHEN 3 THEN 'BL Client' WHEN 4 THEN 'BR Client' WHEN 5 THEN 'BR Client'
	                                              WHEN 6 THEN 'FA Client' WHEN 7 THEN 'FC Client' WHEN 10 THEN 'DA' WHEN 11 THEN 'PR' WHEN 12 THEN 'BC Fournisseur' WHEN 13 THEn 'BL'
	                                              WHEN 14 THEN 'BR' WHEN 15 THEN 'BR' WHEN 16 THEN 'FA Fournisseur' WHEN 17 THEN 'FC Fournisseur'
	                                              WHEN 20 THEN 'ME' WHEN 21 THEN 'MS' WHEN 23 THEN 'MT' ELSE 'AD' END END TypeIntitule,
	                                              ca.CA_Intitule,
	                                              cl.CO_Nom,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantTTC/a.DL_Qte ELSE 0 END PUTTC,
	                                              a.DL_MontantTTC-a.DL_MontantHT MontantTVA,
	                                              a.DL_Remise01REM_Valeur Remise,
	                                              CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantHT/a.DL_Qte ELSE 0 END PUNet,
	                                              CASE WHEN a.DL_MvtStock = 1 THEN a.DL_Qte WHEN a.DL_MvtStock = 3 THEN -a.DL_Qte ELSE 0 END QteMvt,
	                                              CAST(CASE WHEN a.DL_MvtStock = 1 THEN 1 WHEN a.DL_MvtStock = 3 THEN 1 ELSE 0 END AS BIT) IsStock,
												  		a.Operation,
													  a.Timestamp,
													  a.Suser_Name,
													  a.Host_Name,
													  a.Sage_Name,
													  a.id


                                              FROM API_T_Audit_F_DOCLIGNE a
                                              LEFT JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
                                              LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
                                              LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                              LEFT JOIN F_DOCENTETE b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type
                                              LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                              LEFT JOIN F_COLLABORATEUR cl ON a.CO_No = cl.CO_No
GO



                                            ";
            migrationBuilder.Sql(q1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
