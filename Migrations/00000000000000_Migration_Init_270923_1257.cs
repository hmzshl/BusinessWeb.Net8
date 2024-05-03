using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_270923_1257 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string q1 = @"IF OBJECT_ID('[API_V_AUDIT_F_ECRITUREC]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_AUDIT_F_ECRITUREC];
                                            END
                                            GO
CREATE VIEW [dbo].[API_V_AUDIT_F_ECRITUREC]
AS

SELECT [id]
      ,[Operation]
      ,[Timestamp]
      ,[Suser_Name]
      ,[Host_Name]
      ,[Sage_Name]
      ,a.[cbMarq]
      ,a.[JO_Num]
      ,[EC_No]
      ,[EC_NoLink]
      ,[JM_Date]
      ,[EC_Jour]
      ,[EC_Date]
      ,[EC_Piece]
      ,[EC_RefPiece]
      ,[EC_TresoPiece]
      ,a.[CG_Num]
      ,[CG_NumCont]
      ,a.[CT_Num]
      ,[EC_Intitule]
      ,[N_Reglement]
      ,[EC_Echeance]
      ,[EC_Parite]
      ,[EC_Quantite]
      ,a.[N_Devise]
      ,[EC_Sens]
      ,[EC_Montant]
      ,[EC_Lettre]
      ,[EC_Lettrage]
      ,[EC_Point]
      ,[EC_Pointage]
      ,[EC_Impression]
      ,[EC_Cloture]
      ,[EC_CType]
      ,[EC_Rappel]
      ,[CT_NumCont]
      ,[EC_LettreQ]
      ,[EC_LettrageQ]
      ,[EC_ANType]
      ,[EC_RType]
      ,[EC_Devise]
      ,[EC_Remise]
      ,[EC_ExportExpert]
      ,a.[TA_Code]
      ,[EC_Norme]
      ,[TA_Provenance]
      ,[EC_PenalType]
      ,[EC_DatePenal]
      ,[EC_DateRelance]
      ,[EC_DateRappro]
      ,[EC_Reference]
      ,[EC_StatusRegle]
      ,[EC_MontantRegle]
      ,[EC_DateRegle]
      ,[EC_RIB]
      ,[EC_DateOp]
      ,[EC_NoCloture]
      ,[EC_ExportRappro]
      ,[EC_FactureGUID]
      ,[EC_DateCloture]
	  ,ct.CT_Intitule
	  ,cg.CG_Intitule
	  ,jr.JO_Intitule
	  ,DATEADD(DAY,a.EC_Jour-1, a.JM_Date) MV_Date,
	  CASE WHEN a.EC_Sens = 0 THEN a.EC_Montant ELSE 0 END Debit,
	  CASE WHEN a.EC_Sens = 1 THEN a.EC_Montant ELSE 0 END Credit
  FROM [API_T_Audit_F_ECRITUREC] a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num
GO



                                            ";
            migrationBuilder.Sql(q1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
