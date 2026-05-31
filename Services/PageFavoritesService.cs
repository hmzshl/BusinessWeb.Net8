using System.Text.Json;
using BusinessWeb.Data;
using BusinessWeb.Models;
using BusinessWeb.Models.BusinessWebDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace BusinessWeb.Services;

public class PageFavoritesService
{
    private const string LoginProvider = "BusinessWeb";
    private const string FavoritesTokenName = "PageFavorites";
    private const string MenuOnlyTokenName = "ShowOnlyFavoritesMenu";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly BusinessWebDBContext _db;
    private readonly IJSRuntime _js;
    private readonly List<PageFavorite> _favorites = new();
    private string _userId = "";
    private bool _loaded;

    public event Action Changed;

    public bool ShowOnlyFavoritesMenu { get; private set; }

    public PageFavoritesService(BusinessWebDBContext db, IJSRuntime js)
    {
        _db = db;
        _js = js;
    }

    public IReadOnlyList<PageFavorite> GetAll() => _favorites.ToList();

    public bool IsFavorite(string url)
    {
        var key = NormalizeUrl(url);
        return _favorites.Any(f => UrlEquals(f.Url, key));
    }

    public void InvalidateCache() => _loaded = false;

    public async Task EnsureLoadedAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            _favorites.Clear();
            ShowOnlyFavoritesMenu = false;
            _userId = "";
            _loaded = false;
            return;
        }

        if (_loaded && _userId == userId)
            return;

        _userId = userId;
        _favorites.Clear();
        ShowOnlyFavoritesMenu = false;

        await LoadFromDatabaseAsync(userId);

        if (_favorites.Count == 0)
            await TryMigrateFromLocalStorageAsync(userId);

        _loaded = true;
    }

    public async Task SetShowOnlyFavoritesMenuAsync(string userId, bool value)
    {
        await EnsureLoadedAsync(userId);
        if (string.IsNullOrWhiteSpace(userId))
            return;

        ShowOnlyFavoritesMenu = value;
        await UpsertTokenAsync(userId, MenuOnlyTokenName, value ? "1" : "0");
        await TrySaveLocalStorageMenuOnlyAsync(userId, value);
        Changed?.Invoke();
    }

    public async Task<bool> ToggleAsync(string userId, string url, string title, int appHint = 0)
    {
        await EnsureLoadedAsync(userId);
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        var key = NormalizeUrl(url);
        var existing = _favorites.FirstOrDefault(f => UrlEquals(f.Url, key));
        if (existing != null)
        {
            _favorites.Remove(existing);
            await SaveFavoritesAsync(userId);
            Changed?.Invoke();
            return false;
        }

        _favorites.Add(new PageFavorite
        {
            Url = key,
            Title = string.IsNullOrWhiteSpace(title) ? key : title.Trim(),
            AppHint = appHint > 0 ? appHint : null
        });
        await SaveFavoritesAsync(userId);
        Changed?.Invoke();
        return true;
    }

    private async Task LoadFromDatabaseAsync(string userId)
    {
        var tokens = await _db.AspNetUserTokens
            .AsNoTracking()
            .Where(t => t.UserId == userId && t.LoginProvider == LoginProvider
                && (t.Name == FavoritesTokenName || t.Name == MenuOnlyTokenName))
            .ToListAsync();

        var favToken = tokens.FirstOrDefault(t => t.Name == FavoritesTokenName);
        if (!string.IsNullOrWhiteSpace(favToken?.Value))
        {
            try
            {
                var items = JsonSerializer.Deserialize<List<PageFavorite>>(favToken.Value, JsonOptions);
                if (items != null)
                    ImportFavorites(items);
            }
            catch
            {
                // ignore corrupt payload
            }
        }

        var menuToken = tokens.FirstOrDefault(t => t.Name == MenuOnlyTokenName);
        ShowOnlyFavoritesMenu = menuToken?.Value is "1" or "true";
    }

    private async Task SaveFavoritesAsync(string userId)
    {
        var json = JsonSerializer.Serialize(_favorites, JsonOptions);
        await UpsertTokenAsync(userId, FavoritesTokenName, json);
        await TrySaveLocalStorageFavoritesAsync(userId, json);
    }

    private async Task UpsertTokenAsync(string userId, string name, string value)
    {
        var token = await _db.AspNetUserTokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == LoginProvider && t.Name == name);

        if (token == null)
        {
            _db.AspNetUserTokens.Add(new AspNetUserToken
            {
                UserId = userId,
                LoginProvider = LoginProvider,
                Name = name,
                Value = value
            });
        }
        else
        {
            token.Value = value;
        }

        await _db.SaveChangesAsync();
    }

    private async Task TryMigrateFromLocalStorageAsync(string userId)
    {
        try
        {
            var json = await _js.InvokeAsync<string>("bwFavorites.get", StorageKey(userId));
            if (!string.IsNullOrWhiteSpace(json))
            {
                var items = JsonSerializer.Deserialize<List<PageFavorite>>(json, JsonOptions);
                if (items != null && items.Count > 0)
                {
                    ImportFavorites(items);
                    await SaveFavoritesAsync(userId);
                }
            }

            var menuOnly = await _js.InvokeAsync<string>("bwFavorites.get", MenuOnlyStorageKey(userId));
            if (menuOnly is "1" or "true")
            {
                ShowOnlyFavoritesMenu = true;
                await UpsertTokenAsync(userId, MenuOnlyTokenName, "1");
            }
        }
        catch
        {
            // JS not available yet
        }
    }

    private async Task TrySaveLocalStorageFavoritesAsync(string userId, string json)
    {
        try
        {
            await _js.InvokeVoidAsync("bwFavorites.set", StorageKey(userId), json);
        }
        catch
        {
            // optional cache
        }
    }

    private async Task TrySaveLocalStorageMenuOnlyAsync(string userId, bool value)
    {
        try
        {
            await _js.InvokeVoidAsync("bwFavorites.set", MenuOnlyStorageKey(userId), value ? "1" : "0");
        }
        catch
        {
            // optional cache
        }
    }

    private void ImportFavorites(IEnumerable<PageFavorite> items)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items)
        {
            var key = NormalizeUrl(item.Url);
            if (string.IsNullOrEmpty(key) || !seen.Add(key))
                continue;

            _favorites.Add(new PageFavorite
            {
                Url = key,
                Title = string.IsNullOrWhiteSpace(item.Title) ? key : item.Title.Trim(),
                AppHint = item.AppHint ?? (item.SelectedAPP > 0 ? item.SelectedAPP : null)
            });
        }
    }

    public static string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "";

        var path = url.Trim();
        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                path = new Uri(path).AbsolutePath;
            }
            catch
            {
                // keep as-is
            }
        }

        path = path.TrimStart('/');
        var q = path.IndexOf('?');
        if (q >= 0)
            path = path[..q];
        return path.TrimEnd('/');
    }

    private static bool UrlEquals(string a, string b) =>
        string.Equals(NormalizeUrl(a), NormalizeUrl(b), StringComparison.OrdinalIgnoreCase);

    private static string StorageKey(string userId) => $"bw_page_favorites_{userId}";

    private static string MenuOnlyStorageKey(string userId) => $"bw_menu_favorites_only_{userId}";
}
