using DinoBlazorApp.Components;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;


var builder = WebApplication.CreateBuilder(args);

// In some locked-down Windows environments, the default EventLog logger and the default
// DataProtection key location (AppData) can fail with access-denied and break the app.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (OperatingSystem.IsWindows())
{
    builder.Services
        .AddDataProtection()
        .PersistKeysToFileSystem(new System.IO.DirectoryInfo(System.IO.Path.Combine(builder.Environment.ContentRootPath, ".appdata", "dpkeys")))
        .ProtectKeysWithDpapi()
        .SetApplicationName("DinoBlazorApp");
}
else
{
    throw new PlatformNotSupportedException("DinoBlazorApp requires Windows Data Protection or a custom encrypted key provider.");
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// IMPORTANT FOR RENDER
//builder.WebHost.UseUrls("http://0.0.0.0:10000");

//https://127.0.0.1:7249/
//var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


//  2. HttpClient Header ?? Add??? bug?: request.Headers.Add("x-goog-api-key", _apiKey);
//builder.Services.AddHttpClient<GeminiService>();
// 2.1 Add this line in service class too: var url = $"v1beta/models/{model}:generateContent";

builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
    client.DefaultRequestHeaders.Add("x-goog-api-key", builder.Configuration["Gemini:ApiKey"]);
    client.Timeout = TimeSpan.FromMinutes(3); // Set a reasonable timeout for API calls
    //client.Timeout = Timeout.InfiniteTimeSpan; // 🔥 建議直接關掉
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
