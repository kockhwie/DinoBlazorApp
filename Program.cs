using DinoBlazorApp.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Keep runtime logs simple and avoid Windows Event Log dependencies.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var dataProtection = builder.Services
    .AddDataProtection()
    .SetApplicationName("DinoBlazorApp");

if (OperatingSystem.IsWindows())
{
    dataProtection
        .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, ".appdata", "dpkeys")))
        .ProtectKeysWithDpapi();
}
else
{
    var certificate = LoadDataProtectionCertificate(builder.Configuration);

    if (certificate is not null)
    {
        dataProtection
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, ".appdata", "dpkeys")))
            .ProtectKeysWithCertificate(certificate);
    }
    else
    {
        dataProtection.UseEphemeralDataProtectionProvider();
        Console.WriteLine("Warning: No DataProtection certificate configured. Using ephemeral in-memory keys.");
    }
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
    client.DefaultRequestHeaders.Add("x-goog-api-key", builder.Configuration["Gemini:ApiKey"]);
    client.Timeout = TimeSpan.FromMinutes(3);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

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
