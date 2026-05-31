using BusinessWeb.Models.BusinessWebDB;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Services;

public sealed class LiasseIdentificationInfo
{
    public string RaisonSociale { get; init; } = "—";
    public string IdentifiantFiscal { get; init; } = "—";
    public string Ice { get; init; } = "—";
    public string NumeroRc { get; init; } = "—";
    public string Cnss { get; init; } = "—";
    public string ActivitePrincipale { get; init; } = "—";
    public string Adresse { get; init; } = "—";
    public string Ville { get; init; } = "—";
    public string Telephone { get; init; } = "—";

    public static LiasseIdentificationInfo From(TSociete societe, P_DOSSIER dossier)
    {
        static string Pick(params string[] values)
        {
            foreach (var v in values)
            {
                if (!string.IsNullOrWhiteSpace(v))
                    return v.Trim();
            }
            return "—";
        }

        var adresse = Pick(
            societe?.Adresse,
            dossier?.D_Adresse,
            dossier?.API_Adresse);

        if (!string.IsNullOrWhiteSpace(dossier?.D_Complement) && adresse != "—")
            adresse = $"{adresse}, {dossier.D_Complement.Trim()}";

        return new LiasseIdentificationInfo
        {
            RaisonSociale = Pick(societe?.Intitule, dossier?.D_RaisonSoc),
            IdentifiantFiscal = Pick(societe?.IdF, societe?.IFU, dossier?.D_Identifiant, dossier?.API_IdF),
            Ice = Pick(societe?.ICE, dossier?.API_ICE),
            NumeroRc = Pick(societe?.RC, dossier?.API_RC),
            Cnss = Pick(societe?.CNSS),
            ActivitePrincipale = Pick(dossier?.D_Profession, dossier?.D_Ape),
            Adresse = adresse,
            Ville = Pick(societe?.Ville, dossier?.D_Ville),
            Telephone = Pick(societe?.Telephone, dossier?.D_Telephone, dossier?.API_Telephone)
        };
    }
}
