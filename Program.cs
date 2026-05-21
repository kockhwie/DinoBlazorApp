using DinoBlazorApp.Components;
using DinoBlazorApp.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

// IMPORTANT: This must stay before CreateBuilder.
// Render's Linux containers can hit low inotify limits if ASP.NET Core watches
// config files. Do not fix this by re-adding appsettings.json later; ASP.NET
// Core already loads appsettings, User Secrets, and environment variables in
// the correct order, and a late reload can override User Secrets with dummy
// placeholder values such as REPLACE_THIS_IN_USER_SECRETS.
if (!OperatingSystem.IsWindows())
{
    Environment.SetEnvironmentVariable("DOTNET_hostBuilder__reloadConfigOnChange", "false");
}

var builder = WebApplication.CreateBuilder(args);

// Keep runtime logs simple and avoid Windows Event Log dependencies.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IFeedbackService, FeedbackService>();
builder.Services.AddSingleton<IAiUsageLimiter, AiUsageLimiter>();

builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
    client.Timeout = TimeSpan.FromMinutes(3);
});

var dataProtection = builder.Services
    .AddDataProtection()
    .SetApplicationName("DinoBlazorApp");

var keyDirectory = builder.Configuration["DataProtection:KeyDirectory"];
var keyDirectoryInfo = string.IsNullOrWhiteSpace(keyDirectory)
    ? new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, ".appdata", "dpkeys"))
    : new DirectoryInfo(keyDirectory);

if (OperatingSystem.IsWindows())
{
    dataProtection
        .PersistKeysToFileSystem(keyDirectoryInfo)
        .ProtectKeysWithDpapi();
}
else
{
    var certificate = LoadDataProtectionCertificate(builder.Configuration);

    if (certificate is not null)
    {
        dataProtection
            .PersistKeysToFileSystem(keyDirectoryInfo)
            .ProtectKeysWithCertificate(certificate);
    }
    else
    {
        dataProtection.UseEphemeralDataProtectionProvider();
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.TryAdd("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
    context.Response.Headers.TryAdd(
        "Content-Security-Policy",
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' https://unpkg.com https://code.iconify.design; " +
        "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
        "font-src 'self' https://fonts.gstatic.com https://cdn.jsdelivr.net data:; " +
        "img-src 'self' data: https: blob:; " +
        "connect-src 'self' wss: ws: https:; " +
        "frame-ancestors 'self'; base-uri 'self'");
    await next();
});

if (!string.IsNullOrWhiteSpace(builder.Configuration["ASPNETCORE_HTTPS_PORT"]) ||
    !string.IsNullOrWhiteSpace(builder.Configuration["HTTPS_PORT"]))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static X509Certificate2? LoadDataProtectionCertificate(IConfiguration configuration)
{
    var base64 = configuration["DataProtection:CertificateBase64"];
    var path = configuration["DataProtection:CertificatePath"];
    var password = configuration["DataProtection:CertificatePassword"];

    if (!string.IsNullOrWhiteSpace(base64))
    {
        var bytes = Convert.FromBase64String(base64);
        return string.IsNullOrWhiteSpace(password)
            ? X509CertificateLoader.LoadCertificate(bytes)
            : X509CertificateLoader.LoadPkcs12(bytes, password, X509KeyStorageFlags.EphemeralKeySet, Pkcs12LoaderLimits.Defaults);
    }

    if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
    {
        return string.IsNullOrWhiteSpace(password)
            ? X509CertificateLoader.LoadCertificateFromFile(path)
            : X509CertificateLoader.LoadPkcs12FromFile(path, password, X509KeyStorageFlags.EphemeralKeySet, Pkcs12LoaderLimits.Defaults);
    }

    return null;
}

public partial class Program { }
