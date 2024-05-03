using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_021023_1207 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string q1 = @"IF OBJECT_ID('[API_V_CREGLEMENT]', 'V') IS NOT NULL
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
                                            jr.CG_Num,
											FORMAT(YEAR(a.RG_Date),'0000') Annee,
											'M'+FORMAT(MONTH(a.RG_Date),'00') Mois,
											FORMAT(YEAR(a.RG_Date),'0000')+'/'+FORMAT(MONTH(a.RG_Date),'00') MoisAnnee,
											a.RG_Cloture


                                            FROM F_CREGLEMENT a
                                            LEFT JOIN P_REGLEMENT mo ON a.N_Reglement = mo.cbIndice
                                            INNER JOIN F_COMPTET ct ON a.CT_NumPayeur = ct.CT_Num
                                            INNER JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
GO



                                            ";


            string q2 = @"IF OBJECT_ID('[API_V_ECRITUREC]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_ECRITUREC];
                                            END
                                            GO
CREATE VIEW API_V_ECRITUREC
AS
SELECT 
      a.[cbMarq]
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
	  ,ct.CT_Intitule
	  ,cg.CG_Intitule
	  ,jr.JO_Intitule
	  ,DATEADD(DAY,a.EC_Jour-1, a.JM_Date) MV_Date,
	  CASE WHEN a.EC_Sens = 0 THEN a.EC_Montant ELSE 0 END Debit,
	  CASE WHEN a.EC_Sens = 1 THEN a.EC_Montant ELSE 0 END Credit
  FROM [dbo].[F_ECRITUREC] a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN F_JOURNAUX jr ON a.JO_Num = jr.JO_Num
  LEFT JOIN F_COMPTEG cg ON a.CG_Num = cg.CG_Num



                                            ";



            migrationBuilder.Sql(q1);
            migrationBuilder.Sql(q2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
