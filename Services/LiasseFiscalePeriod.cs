namespace BusinessWeb.Services;

public static class LiasseFiscalePeriod
{
    public static string FormatToolbarLabel(
        string exoLabel,
        DateTime exoDebut,
        DateTime exoFin,
        string prevExoLabel,
        DateTime? prevExoDebut,
        DateTime? prevExoFin)
    {
        if (string.IsNullOrWhiteSpace(exoLabel))
            return "Période : —";

        var text = $"Exercice {exoLabel} | Du {exoDebut:dd/MM/yyyy} au {exoFin:dd/MM/yyyy}";

        if (!string.IsNullOrWhiteSpace(prevExoLabel) && prevExoDebut.HasValue && prevExoFin.HasValue)
            text += $" | N-1 : {prevExoLabel} (du {prevExoDebut.Value:dd/MM/yyyy} au {prevExoFin.Value:dd/MM/yyyy})";

        return text;
    }
}
