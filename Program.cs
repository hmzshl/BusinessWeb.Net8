using BoldReports.Web;
using BusinessWeb;
using BusinessWeb.Data;
using BusinessWeb.Middleware;
using BusinessWeb.Models;
using BusinessWeb.Pages;
using JBSystem;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using Microsoft.OData.ModelBuilder;
using MudBlazor.Services;
using Radzen;
using Serilog;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
	ApplicationName = Assembly.GetExecutingAssembly().GetName().Name,
	ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
	Args = args,
	// Explicitly set ALL paths:
	WebRootPath = WindowsServiceHelpers.IsWindowsService() ? Path.Combine(AppContext.BaseDirectory, "wwwroot") : default,
	EnvironmentName = WindowsServiceHelpers.IsWindowsService() ? Environments.Production : Environments.Development
});

// Force the current directory to the app's installation path:
if (WindowsServiceHelpers.IsWindowsService())
{
	Directory.SetCurrentDirectory(AppContext.BaseDirectory);
}

builder.Services.AddWindowsService(); // Add this
builder.Host.UseWindowsService();
builder.WebHost.UseUrls("http://*:5000");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ListenAnyIP(5000); // Force listen on all interfaces
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddMudServices();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BusinessWeb.BusinessWebDBService>();
builder.Services.AddScoped<BusinessWeb.Helpers>();
builder.Services.AddScoped<SageOM>();

builder.Services.AddDbContext<BusinessWeb.Data.BusinessWebDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessWebDBConnection"), o => o.UseCompatibilityLevel(100));
	options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
}, ServiceLifetime.Scoped);
builder.Services.AddDbContext<BusinessWeb.Data.DB>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("APIDB"), o => o.UseCompatibilityLevel(100));
}, ServiceLifetime.Scoped);
builder.Services.AddHttpClient("BusinessWeb").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<ClientInfoService>();
builder.Services.AddSingleton<BusinessWebService>();
builder.Services.AddScoped<BusinessWeb.SecurityService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessWebDBConnection"), o => o.UseCompatibilityLevel(100));
	options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
}, ServiceLifetime.Scoped);
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
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
// Add data protection services
builder.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys"))
		.SetApplicationName("BusinessWeb");


builder.Services.AddCors(opts => opts.AddDefaultPolicy(bld =>
{
	bld
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader()
		.WithExposedHeaders("*")
	;
}));
builder.Services.AddCors(options =>
{
	options.AddPolicy("NewPolicy", builder =>
	builder.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());
});
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
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
var app = builder.Build();
app.UseResponseCompression();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZccXRcRGFeWUV0WkJWYEg=");
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
app.UseCors("NewPolicy");
app.UseAuthorization();
app.UseAntiforgery();
//app.UseMiddleware<ApiKeyMiddleware>();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});
app.UseCors();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.MigrateAsync();
app.MapControllers();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.Run();