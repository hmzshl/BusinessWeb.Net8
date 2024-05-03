using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init_210923_1003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Helpers fn = new Helpers();
            migrationBuilder.Sql(fn.AddVarchar("API_T_CaisseEntete", "Remarque","100")) ;
            migrationBuilder.Sql(fn.AddVarchar("API_T_CaisseEntete", "Reference", "100"));

            string q1 = @"IF OBJECT_ID('[API_V_CAISSEENTETE]', 'V') IS NOT NULL
                                            BEGIN
	                                            DROP VIEW [API_V_CAISSEENTETE];
                                            END
                                            GO

                                            CREATE VIEW [dbo].[API_V_CAISSEENTETE]
                                            AS
                                            SELECT a.id
                                                  ,a.Date
                                                  ,a.Numero
                                                  ,a.Libelle
                                                  ,a.CT_Num CT_NumProjet
                                                  ,a.CA_Num
	                                              ,ca.CA_Intitule
                                                  ,a.Personnel
                                                  ,a.Materiel
                                                  ,a.MontantLettre
                                                  ,a.Type
                                                  ,a.Caisse
                                                  ,a.Montant
                                                  ,a.Site
                                                  ,a.Projet
                                                  ,a.Sense
	                                              ,ct.CT_Intitule
	                                              ,pj.Objet
	                                              ,pj.CT_Num
	                                              ,pj.NumeroMarche
	                                              ,pr.Nom PersonnelIntitule
	                                              ,pr.CIN
	                                              ,pr.Telephone
	                                              ,pr.DateNaissance
	                                              ,pr.Matricule
	                                              ,mt.Intitule MaterielIntitule
	                                              ,mt.Immatricule
	                                              ,af.Intitule AffectationIntitule
	                                              ,a.Remarque
	                                              ,a.Reference


                                              FROM dbo.API_T_CaisseEntete a

                                              LEFT JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
                                              LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
                                                LEFT JOIN API_T_Projet pj ON a.Projet = pj.id
                                              LEFT JOIN API_T_Personnel pr ON a.Personnel = pr.id
                                              LEFT JOIN API_T_Materiel mt ON a.Materiel = mt.id
                                              LEFT JOIN API_T_Affectation af ON a.Affectation = af.id
                                            GO


                                            ";

            migrationBuilder.Sql(q1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
