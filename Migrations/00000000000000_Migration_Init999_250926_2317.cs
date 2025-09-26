using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Migrations
{
	public partial class Migration_Init999_250926_2317 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string q1 = @"CREATE VIEW API_V_CAPREV
AS
SELECT 
FORMAT(YEAR(a.DO_Date),'0000') Annee,
'M'+FORMAT(MONTH(a.DO_Date),'00') Mois,
FORMAT(YEAR(a.DO_Date),'0000')+'/'+FORMAT(MONTH(a.DO_Date),'00') MoisAnnee,
a.DO_Piece,
a.DO_Date,
ct.CT_Num,
ct.CT_Intitule,
ar.AR_Ref,
ar.AR_Design,
a.DL_Qte,
a.DL_PrixUnitaire PU,
a.DL_Remise01REM_Valeur Remise1,
a.DL_Remise02REM_Valeur Remise2,
a.DL_Remise03REM_Valeur Remise3,
CASE WHEN a.DL_Qte != 0 THEN a.DL_MontantHT/a.DL_Qte ELSE 0 END PUNet,
a.DL_MontantHT,
a.DL_Taxe1,
a.DL_MontantTTC-a.DL_MontantHT DL_MontantTVA,
a.DL_MontantTTC,

pu.CMUP,
a.DL_Qte * pu.CMUP CMUPCoutTotal,
CASE WHEN pu.CMUP != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.CMUP) ELSE 0 END CMUPMarge,
CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.CMUP != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.CMUP) ELSE 0 END)/a.DL_MontantHT ELSE 0 END CMUPMargeP,


pu.DernierPUAchat,
a.DL_Qte * pu.DernierPUAchat DPCoutTotal,
CASE WHEN pu.DernierPUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.DernierPUAchat) ELSE 0 END DPMarge,
CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.DernierPUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.DernierPUAchat) ELSE 0 END)/a.DL_MontantHT ELSE 0 END DPMargeP,

pu.PUAchat,
a.DL_Qte * pu.PUAchat PACoutTotal,
CASE WHEN pu.PUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.PUAchat) ELSE 0 END PAMarge,
CASE WHEN a.DL_MontantHT != 0 THEN (CASE WHEN pu.PUAchat != 0 THEN a.DL_MontantHT - (a.DL_Qte * pu.PUAchat) ELSE 0 END)/a.DL_MontantHT ELSE 0 END PAMargeP,

CASE WHEN co_dl.CO_Nom IS NULL THEN CASE WHEN co_do.CO_Nom IS NULL THEN ISNULL(co_ct.CO_Nom,'') + ' ' + ISNULL(co_ct.CO_Prenom,'')
ELSE ISNULL(co_do.CO_Nom,'') + ' ' + ISNULL(co_do.CO_Prenom,'') END
ELSE ISNULL(co_dl.CO_Nom,'') + ' ' + ISNULL(co_dl.CO_Prenom,'')
END CO_Nom,
fm.FA_CodeFamille,
fm.FA_Intitule,
CASE WHEN co_dl.CO_No IS NULL THEN CASE WHEN co_do.CO_No IS NULL THEN ISNULL(co_ct.CO_No,'')
ELSE ISNULL(co_do.CO_No,'') END
ELSE ISNULL(co_dl.CO_No,'')
END CO_No,
ISNULL(de.DE_Intitule,de_e.DE_Intitule) DE_Intitule,
ISNULL(ca_dl.CA_Num,ca_do.CA_Num) CA_Num,
ISNULL(ca_dl.CA_Intitule,ca_do.CA_Intitule) CA_Intitule,
ct.CT_Ville,
ct.CT_CodeRegion,
CASE WHEN a.DO_Type = 0 THEN 'E1 - Devis'
WHEN a.DO_Type = 1 THEN 'E2 - Commandes'
WHEN a.DO_Type in (3,4,5) THEN 'E3 - Livraisons'
WHEN a.DO_Type in (6,7) THEN 'E4 - Factures'
ELSE 'E5 - Autres' END
EtapeCommande,
CASE WHEN a.DO_Type = 0 THEN a.DL_MontantTTC ELSE 0 END CA_DE,
CASE WHEN a.DO_Type = 1 THEN a.DL_MontantTTC ELSE 0 END CA_BC,
CASE WHEN a.DO_Type IN (3,4,5) THEN a.DL_MontantTTC ELSE 0 END CA_BL,
CASE WHEN a.DO_Type IN (6,7) THEN a.DL_MontantTTC ELSE 0 END CA_FA,
CASE WHEN a.DO_Type IN (0,1,2,3,4,5,6,7) THEN a.DL_MontantTTC ELSE 0 END CA_Prev,
CASE WHEN a.DO_Type IN (0,1) THEN a.DL_MontantTTC ELSE 0 END CA_DE_BC


FROM F_DOCLIGNE a
LEFT JOIN 
(
SELECT 
a.AR_Ref,
CASE WHEN a.AR_PrixAch = 0 THEN ISNULL(dp.PU,0) ELSE a.AR_PrixAch END PUAchat,
ISNULL(dp.PU,a.AR_PrixAch) DernierPUAchat,
ISNULL(cm.CMUP,a.AR_PrixAch) CMUP,
CASE 2 
WHEN 0 THEN CASE WHEN a.AR_PrixAch = 0 THEN ISNULL(dp.PU,0) ELSE a.AR_PrixAch END
WHEN 1 THEN ISNULL(dp.PU,a.AR_PrixAch)
WHEN 2 THEN ISNULL(cm.CMUP,a.AR_PrixAch) END PU
FROM F_ARTICLE a
LEFT JOIN 
(
SELECT
a.AR_Ref,
CASE WHEN sum(a.DL_Qte) != 0 THEN sum(a.DL_MontantHT)/sum(a.DL_Qte) ELSE 0 END CMUP
FROM F_DOCLIGNE a
INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
WHERE a.DO_Type in (20,13,16,17)
AND a.DL_MontantHT != 0
GROUP BY
a.AR_Ref
) cm ON a.AR_Ref = cm.AR_Ref
LEFT JOIN 
(
SELECT 
b.AR_Ref,
CASE WHEN b.DL_Qte != 0 THEN b.DL_MontantHT/b.DL_Qte ELSE 0 END PU
FROM
(SELECT 
a.AR_Ref,
MAX(a.cbMarq) cbMArq
FROM
(SELECT 
a.AR_Ref,
a.cbMarq,
MAX(CASE WHEN YEAR(a.DL_DateBL) > 2000 THEN a.DL_DateBL ELSE a.DO_Date END) DL_DateBL
FROM F_DOCLIGNE a
INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
WHERE (a.DO_Date in (13) OR (a.DO_Type in (16,17) AND b.DO_Provenance = 0)) AND a.DL_MontantHT != 0
GROUP BY 
a.AR_Ref,
a.cbMarq) a
GROUP BY
a.AR_Ref) a
INNER JOIN F_DOCLIGNE b ON a.cbMArq = b.cbMarq
) dp ON a.AR_Ref = dp.AR_Ref
) pu ON a.AR_Ref = pu.AR_Ref

INNER JOIN F_DOCENTETE b ON a.DO_Type = b.DO_Type AND a.DO_Piece = b.DO_Piece
INNER JOIN F_COMPTET ct ON a.CT_Num = ct.CT_Num
INNER JOIN F_ARTICLE ar ON a.AR_Ref = ar.AR_Ref
LEFT JOIN F_COLLABORATEUR co_dl ON a.CO_No = co_dl.CO_No
LEFT JOIN F_COLLABORATEUR co_do ON a.CO_No = co_do.CO_No
LEFT JOIN F_COLLABORATEUR co_ct ON a.CO_No = co_ct.CO_No
LEFT JOIN (SELECT TOP 1 * FROM API_T_Config) con ON 1 = 1
LEFT JOIN F_FAMILLE fm ON ar.FA_CodeFamille = fm.FA_CodeFamille
LEFT JOIN F_DEPOT de ON a.DE_No = de.DE_No
LEFT JOIN F_DEPOT de_e ON b.DE_No = de_e.DE_No
LEFT JOIN F_COMPTEA ca_dl ON a.CA_Num = ca_dl.CA_Num
LEFT JOIN F_COMPTEA ca_do ON b.CA_Num = ca_do.CA_Num
WHERE a.DL_Valorise = 1 
AND a.DO_Domaine = 0
GO


";
			migrationBuilder.Sql(q1);

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
