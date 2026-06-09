using DinoAI.Components;
using DinoAI.Components.Account;
using DinoAI.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ── RAZOR COMPONENTS ─────────────────────────────────────────────────────────
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── LOCALIZATION Part 1: ──────────────────────────────────────────────────────
// ResourcesPath tells IStringLocalizer where to find .resx files.
builder.Services.AddLocalization();

var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("zh-Hant") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    // Cookie first (set by CultureController), then browser Accept-Language, then default
    options.RequestCultureProviders =
    [
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    ];
});
 
// ── IDENTITY ──────────────────────────────────────────────────────────────────
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// LOCALIZATION Part 2: Add the localization middleware to the pipeline.
// Must be before UseAntiforgery so culture is resolved before any component renders
app.UseRequestLocalization();


app.UseAntiforgery();

app.MapStaticAssets();

// ── CULTURE ENDPOINT — minimal API, no MVC ────────────────────────────────────
// Reads the posted form, writes the ASP.NET Core culture cookie, redirects back.
// .DisableAntiforgery() is required because LangToggle is a plain HTML form
// with no Blazor antiforgery token — language switching carries no security risk.
app.MapPost("/culture/set", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var culture = form["culture"].ToString();
    var redirectUri = form["redirectUri"].ToString();

    string[] supported = ["en", "zh-Hant"];
    if (!supported.Contains(culture))
        return Results.BadRequest();

    context.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions
        {
            Path = "/",
            MaxAge = TimeSpan.FromDays(365),
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });

    if (!Uri.IsWellFormedUriString(redirectUri, UriKind.Relative))
        redirectUri = "/";

    return Results.LocalRedirect(redirectUri);
})
.DisableAntiforgery();
// ─────────────────────────────────────────────────────────────────────────────


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
