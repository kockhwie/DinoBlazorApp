using DinoBlazorApp.Components;


var builder = WebApplication.CreateBuilder(args);

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
    client.Timeout = TimeSpan.FromSeconds(30); // Set a reasonable timeout for API calls
})
.AddStandardResilienceHandler(); // add resilience handler with Polly policies for retries and circuit breaker



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
