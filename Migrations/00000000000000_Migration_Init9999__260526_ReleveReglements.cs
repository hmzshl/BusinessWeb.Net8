using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init9999__260526_ReleveReglements : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Keep replicated Sage subscriber databases selectable: règlement details are loaded in the page.
			if (System.DateTime.UtcNow < System.DateTime.MinValue)
			{
				migrationBuilder.Sql(@"BEGIN TRANSACTION;

DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql = @sql +
	'ALTER TABLE [' + OBJECT_NAME(parent_object_id) + '] DROP CONSTRAINT [' + name + '];' + CHAR(13)
FROM sys.foreign_keys
WHERE OBJECT_NAME(parent_object_id) LIKE 'API_T%'
	AND OBJECT_NAME(referenced_object_id) LIKE 'F_%';

PRINT @sql;
EXEC sp_executesql @sql;

			COMMIT;");

			migrationBuilder.Sql(@"IF OBJECT_ID('[dbo].[F_COMPTEA]', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[F_COMPTEA] (
		[CA_Num] varchar(13) NOT NULL,
		[CA_Intitule] varchar(35) NULL
	);
END");

			migrationBuilder.Sql(@"ALTER VIEW [dbo].[API_V_RELEVE]
AS
				SELECT 
				CASE WHEN ct.CT_Type = 0 THEN 'Clients'
				WHEN ct.CT_Type = 1 THEN 'Fournisseurs'
				ELSE 'Autres' END TypeTiers,
				a.*,
				ISNULL(a.Debit,0)-ISNULL(a.Credit,0) Solde,
				CASE WHEN ISNULL(a.Repre,'') = '' OR ISNULL(a.Repre,'') = ' ' THEN ISNULL(co.CO_Nom,'') + ' '+ISNULL(co.CO_Prenom,'') ELSE a.Repre END CO_Nom,
				ct.CT_Intitule,
				ct.CT_Ville,
				de.DE_Intitule,
				ca.CA_Intitule,
				CASE WHEN ISNULL(a.Repre,'') = '' OR ISNULL(a.Repre,'') = ' ' THEN ISNULL(co.CO_No,0) ELSE a.CO_No END CO_No2,
				ct.CT_Adresse,
				ct.CT_Telephone,
				ct.CT_Telecopie,
				ct.CT_EMail,
				mr.R_Intitule,
				mr.RT_NbJour,
				bl.BL,
				ech.RG_Montant,
				ct.CT_Encours,
				DATEADD(DAY,ISNULL(mr.RT_NbJour,0),a.DO_Date) DateEcheance,
				ct.CT_CodeRegion

				FROM
				(SELECT 
				'Document' Groupe,
				do.DO_Tiers CT_Num,
				do.DO_Date,
				do.DO_Piece,
				do.DO_Type,
				do.DO_Domaine,
				do.DO_Ref,
				'Document N° '+do.DO_Piece DO_Libelle,
				CASE WHEN a.MontantTTC > 0 THEN a.MontantTTC END Debit,
				CASE WHEN a.MontantTTC < 0 THEN -a.MontantTTC END Credit,
				a.MontantTTC,
				a.MontantRegle,
				a.Reste,
				null DO_Echeance,
				CASE WHEN do.DO_Type in (3,13) THEN 'Bon de livraison' 
				WHEN do.DO_Type in (4,5,14,15) THEN 'Bon de retour'
				WHEN do.DO_Type in (0) THEN 'Devis'
				WHEN do.DO_Type in (10) THEN 'Demande d''achat'
				WHEN do.DO_Type in (11) THEN 'Préparation de commande'
				WHEN do.DO_Type in (1,12) THEN 'Bon de commande'
				WHEN do.DO_Type in (2) THEN 'Préparation de livraison'
				WHEN do.DO_Type in (6,7,16,17) THEN CASE WHEN do.DO_Provenance = 0 THEN 'Facture' ELSE 'Facture Avoir' END
				END TypeIntitule,
				CASE WHEN a.Reste between -1 AND 1 THEN 'Réglé' ELSE CASE WHEN a.MontantRegle != 0 THEN 'Regl. Parciel' ELSE 'Non réglé' END END EtatReglement,
				CASE WHEN a.Reste = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
				DATEDIFF(DAY,do.DO_Date,GETDATE()) Jours,
				do.CO_No,
				do.DE_No,
				do.CA_Num,
				ISNULL(co.CO_Nom,'') + ' '+ISNULL(co.CO_Prenom,'') Repre,
				0 cbMarq,
				0 RG_No,
				rgdet.ReglePar,
				rgdet.DateReglement,
				rgdet.JoursReglement,
				rgdet.JournalReglement

				FROM
				(SELECT
				a.DO_Piece,
				a.DO_Type,
				a.DL_MontantTTC MontantTTC,
				ISNULL(b.MontantRegle,0) MontantRegle,
				a.DL_MontantTTC - ISNULL(b.MontantRegle,0) Reste
				FROM
				(SELECT
				a.DO_Type,
				a.DO_Piece,
				sum(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.DL_MontantTTC) ELSE a.DL_MontantTTC END) DL_MontantTTC
				FROM F_DOCLIGNE a
				WHERE a.DL_Valorise = 1
				AND a.DO_Domaine in (0,1) AND a.DO_Type in (3,4,5,6,7,13,14,15,16,17)
				GROUP BY
				a.DO_Type,
				a.DO_Piece) a
				LEFT JOIN 
				(
				SELECT 
				a.DO_Type,
				a.DO_Piece,
				SUM(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.RC_Montant) ELSE a.RC_Montant END) MontantRegle
				FROM F_REGLECH a
				GROUP BY
				a.DO_Type,
				a.DO_Piece) b ON a.DO_Piece = b.DO_Piece AND a.DO_Type = b.DO_Type) a
				LEFT JOIN F_DOCENTETE do ON a.DO_Piece = do.DO_Piece AND a.DO_Type = do.DO_Type
				LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
				LEFT JOIN F_COLLABORATEUR co ON do.CO_No = co.CO_No
				LEFT JOIN
				(
					SELECT
						src.DO_Type,
						src.DO_Piece,
						STUFF((
							SELECT DISTINCT ', ' + ISNULL(pr.R_Intitule,'Autre')
							FROM F_REGLECH fr
							INNER JOIN F_CREGLEMENT cr ON fr.RG_No = cr.RG_No
							LEFT JOIN P_REGLEMENT pr ON cr.N_Reglement = pr.cbIndice
							WHERE fr.DO_Type = src.DO_Type AND fr.DO_Piece = src.DO_Piece
							FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') ReglePar,
						STUFF((
							SELECT DISTINCT ', ' + CONVERT(varchar(10), cr.RG_Date, 103)
							FROM F_REGLECH fr
							INNER JOIN F_CREGLEMENT cr ON fr.RG_No = cr.RG_No
							WHERE fr.DO_Type = src.DO_Type AND fr.DO_Piece = src.DO_Piece
							FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') DateReglement,
						STUFF((
							SELECT DISTINCT ', ' + CONVERT(varchar(10), DATEDIFF(DAY, de2.DO_Date, cr.RG_Date))
							FROM F_REGLECH fr
							INNER JOIN F_CREGLEMENT cr ON fr.RG_No = cr.RG_No
							LEFT JOIN F_DOCENTETE de2 ON fr.DO_Piece = de2.DO_Piece AND fr.DO_Type = de2.DO_Type
							WHERE fr.DO_Type = src.DO_Type AND fr.DO_Piece = src.DO_Piece
							FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') JoursReglement,
						STUFF((
							SELECT DISTINCT ', ' + ISNULL(jr.JO_Intitule, cr.JO_Num)
							FROM F_REGLECH fr
							INNER JOIN F_CREGLEMENT cr ON fr.RG_No = cr.RG_No
							LEFT JOIN F_JOURNAUX jr ON cr.JO_Num = jr.JO_Num
							WHERE fr.DO_Type = src.DO_Type AND fr.DO_Piece = src.DO_Piece
							FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') JournalReglement
					FROM F_REGLECH src
					GROUP BY src.DO_Type, src.DO_Piece
				) rgdet ON a.DO_Piece = rgdet.DO_Piece AND a.DO_Type = rgdet.DO_Type

				WHERE (con.BL_Releve = 1 AND a.DO_Type IN (3,4,5,6,7,13,14,15,16,17) ) OR (con.BL_Releve = 0 AND a.DO_Type IN (6,7,16,17))

				UNION

				SELECT 
				'Réglement' Groupe,
				a.CT_NumPayeur CT_Num,
				a.RG_Date DO_Date,
				a.RG_Piece DO_Piece,
				null DO_Type,
				null DO_Domaine,
				a.RG_Reference DO_Ref,
				a.RG_Libelle DO_Libelle,
				CASE WHEN (a.RG_Montant) < 0 THEN -a.RG_Montant END Debit,
				CASE WHEN (a.RG_Montant) > 0 THEN a.RG_Montant END Credit,
				a.RG_Montant MontantTTC,
				ISNULL(rc.RC_Montant,0) MontantRegle,
				a.RG_Montant - ISNULL(rc.RC_Montant,0) Reste,
				CASE WHEN YEAR(a.RG_DateEchCont)>2000 AND ISNULL(rg.R_Intitule,'Autre') not like 'CH%' THEN a.RG_DateEchCont END DO_Echeance,
				ISNULL(rg.R_Intitule,'Autre') TypeIntitule,
				CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Affécté' ELSE CASE WHEN ISNULL(rc.RC_Montant,0) != 0 THEN 'Aff. Parciel' ELSE 'Non Affecté' END END EtatReglement,
				CASE WHEN a.RG_Montant - ISNULL(rc.RC_Montant,0) = 0 THEN 'Lettré' ELSE 'Non Lettré' END EtatLettrage,
				DATEDIFF(DAY,a.RG_Date,GETDATE()) Jours,
				0 CO_No,
				0 DE_No,
				'' CA_Num,
				null CO_Nom,
				a.cbMarq,
				a.RG_No,
				null ReglePar,
				null DateReglement,
				null JoursReglement,
				null JournalReglement

				FROM F_CREGLEMENT a
				LEFT JOIN
				(
				SELECT 
				a.RG_No,
				SUM(CASE WHEN a.DO_Type in (4,5,14,15) THEN -ABS(a.RC_Montant) ELSE a.RC_Montant END) RC_Montant
				FROM F_REGLECH a
				GROUP BY
				a.RG_No
				) rc ON a.RG_No = rc.RG_No
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) a
				INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
				LEFT JOIN F_COLLABORATEUR co ON ct.CO_No = co.CO_No
				LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
				LEFT JOIN F_COMPTEA ca ON a.CA_Num = ca.CA_Num
				LEFT JOIN (SELECT a.CT_Num, SUM(a.DL_MontantTTC) BL FROM F_DOCLIGNE a WHERE a.DO_Type IN (13,14,15,3,4,5) AND a.DL_Valorise = 1 GROUP BY CT_Num) bl ON a.CT_Num = bl.CT_Num
				LEFT JOIN (SELECT a.CT_NumPayeur, SUM(a.RG_Montant) RG_Montant FROM F_CREGLEMENT a WHERE a.RG_DateEchCont > GETDATE() GROUP BY CT_NumPayeur) ech ON a.CT_Num = ech.CT_NumPayeur
				LEFT JOIN
				(
				SELECT 
				a.CT_Num,
				a.RT_NbJour,
				rg.R_Intitule
				FROM F_REGLEMENTT a
				INNER JOIN (SELECT a.CT_Num,MIN(a.cbMarq) cbMarq FROM F_REGLEMENTT a GROUP BY a.CT_Num) b ON a.cbMarq = b.cbMarq
				LEFT JOIN P_REGLEMENT rg ON a.N_Reglement = rg.cbIndice
				) mr ON a.CT_Num = mr.CT_Num");
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
		}
	}
}
