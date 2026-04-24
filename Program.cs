using DinoBlazorApp.Components;
using DinoBlazorApp.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Keep runtime logs simple and avoid Windows Event Log dependencies.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IFeedbackService, FeedbackService>();

builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
    client.DefaultRequestHeaders.Add("x-goog-api-key", builder.Configuration["Gemini:ApiKey"]);
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
