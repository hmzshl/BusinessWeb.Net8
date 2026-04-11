using BusinessWeb.Data;
using BusinessWeb.Models.BusinessWebDB;
using LogicNP.CryptoLicensing;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace BusinessWeb.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private const string APIKEY = "key";
        private readonly Helpers _fn = new Helpers();

        // Cache the validated CryptoLicense so it is not rebuilt on every request.
        // Key = activation string, Value = (license, created-at).
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, (CryptoLicense License, DateTime CreatedAt)>
            _licenseCache = new();
        private static readonly TimeSpan _licenseCacheTtl = TimeSpan.FromMinutes(30);

        private static readonly string _macAddr =
            NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Select(n => n.GetPhysicalAddress().ToString())
                .FirstOrDefault() ?? "UNKNOWN";

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, BusinessWebDBContext dbContext)
        {
            if (!IsApiPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

            // 1. Load and cache license
            var license = await GetCachedLicenseAsync(dbContext);

            // 2. Validate API key header
            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("ACCESS TO THE API IS RESTRICTED AS THE REQUIRED API KEY HAS NOT BEEN PROVIDED.");
                return;
            }

            // 3. Validate license feature
            if (license == null || !license.IsFeaturePresentEx(17))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("YOUR LICENSE KEY DOES NOT PERMIT THE USE OF THIS API.");
                return;
            }

            // 4. Validate API key value
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var expectedKey  = appSettings.GetValue<string>(APIKEY);
            if (!expectedKey.Equals(extractedApiKey.ToString()))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("INCORRECT API KEY PROVIDED. PLEASE ENSURE THE KEY IS ACCURATE AND TRY AGAIN.");
                return;
            }

            await _next(context);
        }

        private static bool IsApiPath(PathString path)
        {
            if (!path.StartsWithSegments("/api")) return false;
            var excluded = new[]
            {
                "/api/Files", "/api/BoldReportsJSON", "/api/BoldReportsMAUI",
                "/api/BoldReportsSQL", "/api/swagger", "/api/BoldReportsAPI", "/api/BoldReportsWriter"
            };
            return !excluded.Any(e => path.StartsWithSegments(e));
        }

        private async Task<CryptoLicense?> GetCachedLicenseAsync(BusinessWebDBContext dbContext)
        {
            var row = await dbContext.TLicenses.OrderBy(a => a.id).FirstOrDefaultAsync();
            if (row == null || string.IsNullOrEmpty(row.Activation)) return null;

            if (_licenseCache.TryGetValue(row.Activation, out var cached)
                && DateTime.UtcNow - cached.CreatedAt < _licenseCacheTtl)
                return cached.License;

            try
            {
                var license = new CryptoLicense(row.Activation, _fn.validationKey);
                _licenseCache[row.Activation] = (license, DateTime.UtcNow);
                return license;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load CryptoLicense for activation {Activation}", row.Activation);
                return null;
            }
        }
    }
}
