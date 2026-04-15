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

    // Stream the response directly to the UI
    public async IAsyncEnumerable<string> AskStreamAsync(string prompt)
    {
        string[] models = { 
            "gemini-2.5-flash",
            "gemini-1.5-flash"
        };

        foreach (var model in models)
        {
            var url = $"v1beta/models/{model}:streamGenerateContent?alt=sse";
            var requestBody = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests || response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    continue; // Try next model
                
                yield return $"\n\n❌ **Error ({(int)response.StatusCode})**: {response.ReasonPhrase}";
                yield break;
            }

            // Real Server-Sent Events reading
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            bool successStreamed = false;

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("data: "))
                {
                    var data = line.Substring("data: ".Length).Trim();
                    if (data == "[DONE]") break; // Stream finished

                    string? chunkToYield = null;
                    try
                    {
                        using var doc = JsonDocument.Parse(data);
                        var root = doc.RootElement;
                        if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                        {
                            var contentParts = candidates[0].GetProperty("content").GetProperty("parts");
                            if (contentParts.GetArrayLength() > 0 && contentParts[0].TryGetProperty("text", out var textEl))
                            {
                                successStreamed = true;
                                chunkToYield = textEl.GetString();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore partial JSON blocks if parsing fails mid-stream
                    }

                    if (chunkToYield != null)
                    {
                        yield return chunkToYield;
                    }
                }
            }
            
            if (successStreamed)
                yield break; // We successfully streamed from this model, don't try the rest.
        }
        
        yield return "\n\n❌ All Gemini models are currently overwhelmed. Please try again later.";
    }

    public async Task<string> RefinePromptAsync(string userPrompt) 
    {
        var metaPrompt = $@"You are an elite Prompt Engineer. Rewrite the user's draft prompt into a professional, highly structured prompt.
Use advanced frameworks like CO-STAR or RACE (Context, Objective, Style, Tone, Audience, Response).
IMPORTANT: Output ONLY the raw refined prompt text. Do not explain what you did. Do not include markdown codeblocks around the text. Do not say 'Here is your prompt'.

Draft Prompt:
{userPrompt}";

        var sb = new StringBuilder();
        await foreach(var chunk in AskStreamAsync(metaPrompt))
        {
            sb.Append(chunk);
        }
        return sb.ToString().Trim();
    }

    public async Task<string> TranslatePromptAsync(string sourceText, string targetLanguage) 
    {
        var metaPrompt = $@"You are an elite bilingual translator. Translate the following text into strictly {targetLanguage}.
Retain all markdown formatting exactly as it is (like bolding, lists, and codeblocks).
IMPORTANT: Output ONLY the translated text. Do not explain what you did.

Text to Translate:
{sourceText}";

        var sb = new StringBuilder();
        await foreach(var chunk in AskStreamAsync(metaPrompt))
        {
            sb.Append(chunk);
        }
        return sb.ToString().Trim();
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