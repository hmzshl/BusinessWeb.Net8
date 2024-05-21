using BusinessWeb.Data;
using BusinessWeb.Models.BusinessWebDB;
using LogicNP.CryptoLicensing;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;

namespace BusinessWeb.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BusinessWebDBContext _dbContext;
        private
        const string APIKEY = "key";
        public CryptoLicense license = new CryptoLicense();
        Helpers fn = new Helpers();
        string macAddr = (
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, BusinessWebDBContext dbContext)
        {

			if (context.Request.Path.StartsWithSegments("/api") 
                && (!context.Request.Path.StartsWithSegments("/api/Files")) 
                && (!context.Request.Path.StartsWithSegments("/api/BoldReportsJSON")) 
                && (!context.Request.Path.StartsWithSegments("/api/BoldReportsMAUI")) 
                && (!context.Request.Path.StartsWithSegments("/api/BoldReportsSQL"))
                && (!context.Request.Path.StartsWithSegments("/api/swagger"))
                && (!context.Request.Path.StartsWithSegments("/api/BoldReportsAPI"))
                )
            {
                TLicense row = new TLicense();
                var dt = dbContext.TLicenses.OrderBy(a => a.id).ToList();
                if (dt.Count() != 0)
                {
                    row = dt.First();
                }
                try
                {
                    license = new CryptoLicense(row.Activation, fn.validationKey);
                }
                catch (Exception ex)
                {

                }
                if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Access to the API is restricted as the required API key has not been provided.".ToUpper());
                    return;
                }
                if (!license.IsFeaturePresentEx(17))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Your license key does not permit the use of this API.".ToUpper());
                    return;
                }
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEY);
                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Incorrect API key provided. Please ensure the key is accurate and try again.".ToUpper());
                    return;
                }

                await _next(context);
            }
            else
            {
                await _next(context);
            }
		}
    }
}
