using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
    public partial class Migration_Init9_040724_2244 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_COMPTET]
				AS
				SELECT 
				a.cbMarq
				,a.CT_Num
				,ISNULL(a.CT_Intitule,'') CT_Intitule
				,ISNULL(a.CT_Type,0) CT_Type
				,ISNULL(a.CG_NumPrinc,'') CG_NumPrinc
				,a.CT_Qualite
				,a.CT_Classement
				,a.CT_Contact
				,a.CT_Adresse
				,a.CT_Complement
				,a.CT_CodePostal
				,a.CT_Ville
				,a.CT_CodeRegion
				,a.CT_Pays
				,a.CT_Raccourci
				,a.BT_Num
				,a.N_Devise
				,a.CT_Ape
				,a.CT_Identifiant
				,a.CT_Siret
				,a.CT_Statistique01
				,a.CT_Statistique02
				,a.CT_Statistique03
				,a.CT_Statistique04
				,a.CT_Statistique05
				,a.CT_Statistique06
				,a.CT_Statistique07
				,a.CT_Statistique08
				,a.CT_Statistique09
				,a.CT_Statistique10
				,a.CT_Commentaire
				,a.CT_Encours
				,a.CT_Assurance
				,a.CT_NumPayeur
				,a.N_Risque
				,a.CO_No
				,a.N_CatTarif
				,a.CT_Taux01
				,a.CT_Taux02
				,a.CT_Taux03
				,a.CT_Taux04
				,a.N_CatCompta
				,a.N_Period
				,a.CT_Facture
				,a.CT_BLFact
				,a.CT_Langue
				,a.N_Expedition
				,a.N_Condition
				,a.CT_Saut
				,a.CT_Lettrage
				,a.CT_ValidEch
				,a.CT_Sommeil
				,a.DE_No
				,a.CT_ControlEnc
				,a.CT_NotRappel
				,a.N_Analytique
				,a.CA_Num
				,a.CT_Telephone
				,a.CT_Telecopie
				,a.CT_EMail
				,a.CT_Site
				,a.CT_Coface
				,a.CT_Surveillance
				,a.CT_SvFormeJuri
				,a.CT_SvEffectif
				,a.CT_SvCA
				,a.CT_SvResultat
				,a.CT_SvIncident
				,a.CT_SvPrivil
				,a.CT_SvRegul
				,a.CT_SvCotation
				,a.CT_SvObjetMaj
				,a.N_AnalytiqueIFRS
				,a.CA_NumIFRS
				,a.CT_PrioriteLivr
				,a.CT_LivrPartielle
				,a.MR_No
				,a.CT_NotPenal
				,a.EB_No
				,a.CT_NumCentrale
				,a.CT_FactureElec
				,a.CT_TypeNIF
				,a.CT_RepresentInt
				,a.CT_RepresentNIF
				,a.CT_EdiCodeType
				,a.CT_EdiCode
				,a.CT_EdiCodeSage
				,a.CT_ProfilSoc
				,a.CT_StatutContrat
				,a.CT_EchangeRappro
				,a.CT_EchangeCR
				,a.PI_NoEchange
				,a.CT_BonAPayer
				,a.CT_DelaiTransport
				,a.CT_DelaiAppro
				,a.CT_LangueISO2
				,b.DelaiSommeil
				,b.DL_DateBL
				,b.DL_MontantHT
				,b.DL_MontantTTC
				,rg.RG_Montant
				,ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0) SoldeCommercial
				,cp.EC_Montant SoldeComptable,
				CASE a.CT_ControlEnc WHEN 0 THEN 'Controle Automatique' WHEN 1 THEN 'Selon Code Risque' WHEN 2 THEn 'Compte Bloqué' END Controle,
				CASE a.CT_Sommeil WHEN 0 THEN 'Actif' ELSE 'En sommeil' END SommeilIntitule,
				CASE WHEN (ISNULL(b.DL_MontantTTC,0)-ISNULL(rg.RG_Montant,0)) = 0 THEN 'Soldé' ELSE 'Non soldé' END EtatSolde,
				ec.RT_NbJour,
				ec.R_Intitule,
				dev.D_Intitule,
				null PROT_User,
				a.cbCreation,
				a.cbModification



				FROM F_COMPTET a
				--LEFT JOIN F_PROTECTIONCIAL uti ON a.cbCreationUser = uti.PROT_Guid
				

				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				SUM(a.DL_MontantHT) DL_MontantHT,
				SUM(a.DL_MontantTTC) DL_MontantTTC,
				MAX(a.DL_DateBL) DL_DateBL,
				COUNT(DISTINCT a.AR_Ref) NbrArticles,
				COUNT(DISTINCT a.DO_Piece) NbrDocuments,
				DATEDIFF(DAY,MAX(a.DL_DateBL),GETDATE()) DelaiSommeil,
				CASE WHEN b.DL_MontantTTC != 0 THEN SUM(a.DL_MontantTTC)/b.DL_MontantTTC ELSE 0 END Taux

				FROM F_DOCLIGNE a
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				INNER JOIN
				(
				SELECT 
				ct.CT_Type,
				SUM(a.DL_MontantHT) DL_MontantHT,
				SUM(a.DL_MontantTTC) DL_MontantTTC

				FROM F_DOCLIGNE a
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				WHERE (con.BL_Releve = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17) ) OR (con.BL_Releve = 0 AND a.DO_Type IN (6,7,16,17))
				GROUP BY ct.CT_Type
				) b ON ct.CT_Type = b.CT_Type



				WHERE (con.BL_Releve = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17) ) OR (con.BL_Releve = 0 AND a.DO_Type IN (6,7,16,17))
				GROUP BY a.CT_Num , b.DL_MontantTTC
				) b ON a.CT_Num = b.CT_Num





				LEFT JOIN 
				(
				SELECT 
				a.CT_NumPayeur,
				SUM(a.RG_Montant) RG_Montant
				FROM F_CREGLEMENT a
				GROUP BY a.CT_NumPayeur
				) rg ON rg.CT_NumPayeur = a.CT_Num





				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				SUM(CASE WHEN a.EC_Sens = 0 THEN a.EC_Montant ELSE -a.EC_Montant END) EC_Montant
				FROM F_ECRITUREC a
				GROUP BY a.CT_Num
				) cp ON a.CT_Num = cp.CT_Num

				LEFT JOIN

				(
				SELECT 
				a.CT_Num,
				a.RT_NbJour,
				rg.R_Intitule
				FROM F_REGLEMENTT a
				INNER JOIN
				(SELECT 
				a.CT_Num,
				MAX(a.cbMarq) cbMarq
				FROM F_REGLEMENTT a
				GROUP BY a.CT_Num) b ON a.cbMarq = b.cbMarq
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) ec ON a.CT_Num = ec.CT_Num

				LEFT JOIN P_DEVISE dev ON a.N_Devise = dev.cbIndice");

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
