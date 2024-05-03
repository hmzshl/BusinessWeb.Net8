using BoldReports.Web;
using BusinessWeb;
using BusinessWeb.Data;
using BusinessWeb.Middleware;
using BusinessWeb.Models;
using JBSystem;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OData.ModelBuilder;
using Radzen;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default, Args = args });
builder.Host.UseWindowsService();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.WebHost.UseUrls("http://*:5000");
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BusinessWeb.BusinessWebDBService>();
builder.Services.AddScoped<BusinessWeb.Helpers>();
builder.Services.AddDbContext<BusinessWeb.Data.BusinessWebDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessWebDBConnection"));
}, ServiceLifetime.Transient);
builder.Services.AddDbContext<BusinessWeb.Data.DB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIDB"));
}, ServiceLifetime.Transient);
builder.Services.AddHttpClient("BusinessWeb").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddSingleton<BusinessWebService>();
builder.Services.AddScoped<BusinessWeb.SecurityService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessWebDBConnection"));
}, ServiceLifetime.Transient);
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddCors(opts => opts.AddDefaultPolicy(bld =>
{
    bld
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("*")
    ;
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(e => e.EnableEndpointRouting = false);
builder.Services.AddScoped<AuthenticationStateProvider, BusinessWeb.ApplicationAuthenticationStateProvider>();
builder.Services.AddDbContext<BusinessWeb.Data.BusinessWebDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessWebDBConnection"));
});

ReportConfig.DefaultSettings = new ReportSettings().RegisterExtensions(new List<string> { "BoldReports.Data.WebData", "BoldReports.Data.Csv" });
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true; // Optional: Enable compression for HTTPS
});
builder.Services.AddMvc(e => e.EnableEndpointRouting = false);
var app = builder.Build();
app.UseResponseCompression();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE1MDU5NkAzMjM0MmUzMDJlMzBtOFFFakhYb2F6QVU1dTdvTmFaQk8vNzVXckNSb2dDcmJPcWpRekE4MFhnPQ==");
Bold.Licensing.BoldLicenseProvider.RegisterLicense("zidV+mAe83DkDarVg0J39aLvzg6umhkhjiPNndeU1j4=");
app.UseRequestLocalization("fr");
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fr");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseHeaderPropagation();
app.UseAuthentication();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
});
app.MapControllers();
app.Run();