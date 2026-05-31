namespace BusinessWeb.Models;

public class PageFavorite
{
    public string Url { get; set; }
    public string Title { get; set; }

    /// <summary>Application to activate when opening this favorite (optional).</summary>
    public int? AppHint { get; set; }

    /// <summary>Legacy field from per-app favorites; used only when importing old data.</summary>
    public int SelectedAPP { get; set; }
}
