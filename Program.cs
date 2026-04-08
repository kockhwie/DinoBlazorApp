using DinoBlazorApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// IMPORTANT FOR RENDER
builder.WebHost.UseUrls("http://0.0.0.0:10000");

//https://127.0.0.1:7249/
//var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddHttpClient<GeminiService>();

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
