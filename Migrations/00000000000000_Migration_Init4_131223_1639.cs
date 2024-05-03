using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init4_131223_1639 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW API_V_REGLEMENTT
AS
SELECT a.CT_Num
      ,a.N_Reglement
      ,a.RT_Condition
      ,a.RT_NbJour
      ,a.RT_JourTb01
      ,a.RT_JourTb02
      ,a.RT_JourTb03
      ,a.RT_JourTb04
      ,a.RT_JourTb05
      ,a.RT_JourTb06
      ,a.RT_TRepart
      ,a.RT_VRepart
      ,a.cbMarq
	  ,ct.CT_Intitule
	  ,rg.R_Intitule
  FROM F_REGLEMENTT a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
  LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice");
            migrationBuilder.Sql(@"CREATE VIEW API_V_ARTFOURNISS
AS
SELECT a.AR_Ref
      ,a.CT_Num
      ,a.AF_RefFourniss
      ,a.AF_PrixAch
      ,a.AF_Unite
      ,a.AF_Conversion
      ,a.AF_DelaiAppro
      ,a.AF_Garantie
      ,a.AF_Colisage
      ,a.AF_QteMini
      ,a.AF_QteMont
      ,a.EG_Champ
      ,a.AF_Principal
      ,a.AF_PrixDev
      ,a.AF_Devise
      ,a.AF_Remise
      ,a.AF_ConvDiv
      ,a.AF_TypeRem
      ,a.AF_CodeBarre
      ,a.AF_PrixAchNouv
      ,a.AF_PrixDevNouv
      ,a.AF_RemiseNouv
      ,a.AF_DateApplication
      ,a.cbMarq
	  ,ct.CT_Intitule
  FROM F_ARTFOURNISS a
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num");
            migrationBuilder.Sql(@"CREATE VIEW API_V_ARTCLIENT
AS
SELECT a.AR_Ref
      ,a.AC_Categorie
      ,a.AC_PrixVen
      ,a.AC_Coef
      ,a.AC_PrixTTC
      ,a.AC_Arrondi
      ,a.AC_QteMont
      ,a.EG_Champ
      ,a.AC_PrixDev
      ,a.AC_Devise
      ,a.CT_Num
      ,a.AC_Remise
      ,a.AC_Calcul
      ,a.AC_TypeRem
      ,a.AC_RefClient
      ,a.AC_CoefNouv
      ,a.AC_PrixVenNouv
      ,a.AC_PrixDevNouv
      ,a.AC_RemiseNouv
      ,a.cbMarq
	  ,tr.CT_Intitule CategorieTarif
	  ,ct.CT_Intitule
  FROM F_ARTCLIENT a
  LEFT JOIN P_CATTARIF tr ON a.AC_Categorie = tr.cbIndice
  LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
