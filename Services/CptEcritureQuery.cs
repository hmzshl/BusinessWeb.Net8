using BusinessWeb.Data;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Perso;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Services;

public static class CptEcritureQuery
{
    public const string CaisseAccountPrefix = "5161";

    public sealed class LabelLookups
    {
        public Dictionary<string, string> Journal { get; init; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, string> Compte { get; init; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, string> Tier { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public static LabelLookups LoadLabels(DB db) => new()
    {
        Journal = db.F_JOURNAUX.AsNoTracking()
            .Where(j => j.JO_Num != null)
            .GroupBy(j => j.JO_Num)
            .ToDictionary(g => g.Key, g => g.Select(x => x.JO_Intitule).FirstOrDefault() ?? "", StringComparer.OrdinalIgnoreCase),
        Compte = db.F_COMPTEG.AsNoTracking()
            .Where(g => g.CG_Num != null)
            .ToDictionary(g => g.CG_Num, g => g.CG_Intitule ?? "", StringComparer.OrdinalIgnoreCase),
        Tier = db.F_COMPTET.AsNoTracking()
            .Where(t => t.CT_Num != null)
            .ToDictionary(t => t.CT_Num, t => t.CT_Intitule ?? "", StringComparer.OrdinalIgnoreCase)
    };

    public static IQueryable<F_ECRITUREC> ByEcDate(DB db, DateTime debut, DateTime fin) =>
        db.F_ECRITUREC.Where(e =>
            e.EC_Date.HasValue
            && e.EC_Date.Value >= debut
            && e.EC_Date.Value <= fin);

    public static IQueryable<F_ECRITUREC> ByEcYear(DB db, int year)
    {
        var debut = new DateTime(year, 1, 1);
        var fin = new DateTime(year, 12, 31);
        return ByEcDate(db, debut, fin);
    }

    public static IQueryable<F_ECRITUREC> ExcludeAN(IQueryable<F_ECRITUREC> query) =>
        query.Where(e => (e.EC_ANType ?? 0) != 1);

    public static DateTime? MovementDate(F_ECRITUREC e) => e.EC_Date;

    public static decimal DebitAmount(F_ECRITUREC e) =>
        e.EC_Sens == 0 ? e.EC_Montant ?? 0m : 0m;

    public static decimal CreditAmount(F_ECRITUREC e) =>
        e.EC_Sens == 1 ? e.EC_Montant ?? 0m : 0m;

    public static CptEcritureItem ToItem(F_ECRITUREC e, LabelLookups labels)
    {
        labels.Journal.TryGetValue(e.JO_Num ?? "", out var joLib);
        labels.Compte.TryGetValue(e.CG_Num ?? "", out var cgLib);
        labels.Tier.TryGetValue(e.CT_Num ?? "", out var ctLib);

        return new CptEcritureItem
        {
            JO_Num = e.JO_Num,
            JO_Intitule = joLib,
            MV_Date = e.EC_Date,
            EC_Piece = e.EC_Piece,
            EC_Reference = e.EC_Reference,
            CG_Num = e.CG_Num,
            CG_Intitule = cgLib,
            CT_Num = e.CT_Num,
            CT_Intitule = ctLib,
            EC_Intitule = e.EC_Intitule,
            Debit = DebitAmount(e),
            Credit = CreditAmount(e),
            EC_Lettre = e.EC_Lettre,
            EC_Lettrage = e.EC_Lettrage,
            EC_Echeance = e.EC_Echeance,
            EC_DateRappro = e.EC_DateRappro,
            Rapprochement = e.EC_DateRappro.HasValue && e.EC_DateRappro.Value.Year > 1900 ? e.EC_DateRappro : null
        };
    }

    public static List<CptEcritureItem> ToItemList(IEnumerable<F_ECRITUREC> rows, LabelLookups labels) =>
        rows.Select(e => ToItem(e, labels)).ToList();
}
