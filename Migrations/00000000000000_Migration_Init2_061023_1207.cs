using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init2_061023_1207 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"IF OBJECT_ID('[API_V_ATTACHEMENT]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_ATTACHEMENT];
                                            END
                                            GO

CREATE VIEW [dbo].[API_V_ATTACHEMENT]
AS
SELECT a.id
      ,a.Projet
      ,a.Date
      ,a.Piece
      ,a.Libelle
      ,a.Responsable
      ,a.Montant
      ,a.MontantMarche
      ,a.MontantCumul
      ,a.MontantReste
      ,a.TauxRabais
      ,a.DateDebut
      ,a.DateFin
      ,a.Statut
      ,a.NumeroDecompte
      ,a.DateDecompte
      ,a.MontantTTC
	  ,b.CA_Num
	  ,b.Objet
	  ,b.NumeroAppelOffre
	  ,b.NumeroMarche
	  ,st.Intitule SiteIntitule
	  ,vi.Intitule VilleIntitule
	  

  FROM API_T_Attachement a
  LEFT JOIN API_T_Projet b ON a.Projet = b.id
  LEFT JOIN API_T_Site st ON b.Site = st.id
  LEFT JOIN API_T_Ville vi ON b.Ville = vi.id
GO");

            migrationBuilder.Sql(@"IF OBJECT_ID('[API_V_BORDEREAU]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_BORDEREAU];
                                            END
                                            GO

CREATE VIEW [dbo].[API_V_BORDEREAU]
AS
SELECT a.id
      ,a.Projet
      ,a.Date
      ,a.Piece
      ,a.Libelle
      ,a.Responsable
      ,a.Montant
      ,a.MontantMarche
      ,a.CoutTotale
      ,a.Marge
      ,a.MargeP
      ,a.Rabais
      ,a.MontantTTC
	  ,b.CA_Num
	  ,b.Objet
	  ,b.NumeroAppelOffre
	  ,b.NumeroMarche
	  	  ,st.Intitule SiteIntitule
	  ,vi.Intitule VilleIntitule
  FROM dbo.API_T_Bordereau a
    LEFT JOIN API_T_Projet b ON a.Projet = b.id
	  LEFT JOIN API_T_Site st ON b.Site = st.id
  LEFT JOIN API_T_Ville vi ON b.Ville = vi.id
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
