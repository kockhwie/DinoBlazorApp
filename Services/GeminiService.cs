using System.Text;
using System.Text.Json;
using System.Net;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Gemini:ApiKey"]
                  ?? throw new Exception("Gemini API Key is missing");
    }

    public async Task<string> AskAsync(string prompt, IProgress<string>? progress = null)
    {
        // List of models to try in order of preference
        string[] models = { 
            
            /* "gemini-2.5-flash",
            "gemini-2.5-pro", 
            "gemini-2.0-flash-001", 
            "gemini-2.0-flash-lite-001", 
            "gemini-2.0-flash-lite", 
            "gemini-flash-latest", 
            "gemini-flash-lite-latest", 
            "gemini-pro-latest", 
            "gemini-2.5-flash-lite", 
            "gemini-3-flash-preview", 
            "gemini-3-pro-preview", 
            "gemini-3.1-pro-preview", 
            "gemini-3.1-flash-lite-preview", 
            "gemini-3.1-flash-live-preview", */
            "gemma-4-31b-it",  
            "gemma-4-26b-a4b-it",
            "gemma-3-27b-it",
            "gemma-3-12b-it", 
            "lyria-3-clip-preview" 
        };

        foreach (var model in models)
        {
            try
            {
                progress?.Report($"Connecting to {model} ...");
                var result = await ExecuteRequestAsync(model, prompt);
                progress?.Report($"Connecting to {model} ... Succeed");
                return result;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("high demand") || (int?)ex.StatusCode == 503)
            {
                // Log that the current model is busy, then loop to the next one
                Console.WriteLine($"Model {model} is busy. Trying fallback...");
                progress?.Report($"Connecting to {model} ... failed ({ex.Message})");
                continue;
            }
            catch (HttpRequestException ex)
            {
                // Other request-level failures (e.g. network, retries exhausted)
                Console.WriteLine($"Model {model} failed: {ex.Message}");
                progress?.Report($"Connecting to {model} ... failed ({ex.Message})");
                continue;
            }
        }

        //throw new Exception("All Gemini models are currently overwhelmed. Please try again later.");
        // change to use InvalidOperationException, because it's not a transient HTTP issue but rather a state of the service,
        // without it will be retried by the Polly policy and cause unnecessary retries and delays.
        throw new InvalidOperationException("All Gemini models are currently overwhelmed. Please try again later.");
        
    }

    private async Task<string> ExecuteRequestAsync(string model, string prompt)
    {

        // 2. dun use request.Headers.Add("x-goog-api-key", _apiKey);
        var url = $"v1beta/models/{model}:generateContent";

        //var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        // Alternative: Using Header instead of URL Query string
        // _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
        // var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        // var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        // request.Headers.Add("x-goog-api-key", _apiKey);

        using var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // If the HTTP status is not successful, try to extract a useful error message
        if (!response.IsSuccessStatusCode)
        {
            string serverMessage = json;
            try
            {
                using var errDoc = JsonDocument.Parse(json);
                var errRoot = errDoc.RootElement;
                if (errRoot.TryGetProperty("error", out var error))
                {
                    if (error.TryGetProperty("message", out var m))
                        serverMessage = m.GetString() ?? serverMessage;
                }
            }
            catch
            {
                // If parsing fails, fall back to raw body
            }

            // For transient errors, throw so caller can retry fallback models
            if (response.StatusCode == HttpStatusCode.TooManyRequests || response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new HttpRequestException(serverMessage, null, response.StatusCode);
            }

            // For authentication issues surface a clear message
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return $"❌ Authentication error ({(int)response.StatusCode}): {serverMessage}";
            }

            return $"❌ HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {serverMessage}";
        }

        // Parse success response
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Check for structured API-level error
            if (root.TryGetProperty("error", out var apiError))
            {
                var message = apiError.GetProperty("message").GetString();
                return $"❌ API Error: {message}";
            }

            if (root.TryGetProperty("candidates", out var candidates))
            {
                var text = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // add this check to avoid exceptions if the response format is different than expected
                if (candidates.GetArrayLength() > 0)
                {
                    var first = candidates[0];

                    if (first.TryGetProperty("content", out var contentEl) &&
                        contentEl.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        return parts[0].GetProperty("text").GetString()
                               ?? "No response";
                    }
                }

                return text ?? "No response from AI.";
            }

            return "⚠️ Unexpected response format.";
        }
        catch (JsonException)
        {
            return $"❌ Failed to parse response (HTTP {(int)response.StatusCode}). Raw response: {json}";
        }
    }

    /* this is working version.
    public async Task<string> AskAsync(string prompt)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var result = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return result ?? "No response from AI.";
    }
    */
}