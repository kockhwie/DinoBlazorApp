using System.Text;
using System.Text.Json;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DinoBlazorApp.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IAiUsageLimiter _usageLimiter;

    public GeminiService(HttpClient httpClient, IConfiguration config, IAiUsageLimiter usageLimiter)
    {
        _httpClient = httpClient;
        _usageLimiter = usageLimiter;
        _apiKey = config["Gemini:ApiKey"]
                  ?? throw new Exception("Gemini API Key is missing");
    }

    // Stream the response directly to the UI
    public async IAsyncEnumerable<string> AskStreamAsync(
        string prompt,
        string usageKey,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var usageLease = await _usageLimiter.TryAcquireAsync(usageKey, cancellationToken);
        if (usageLease is null || !usageLease.IsAccepted)
        {
            yield return $"⚠️ **Rate limit**: {usageLease?.RejectionReason ?? "Please try again later."}";
            yield break;
        }

        // DO NOT CHANGE THESE MODELS without my approval.
        // Models prioritized by modern performance (2026 stack)
        var modelQueue = new Queue<string>(new[] {            
            
            "gemini-2.5-flash", // User's preferred primary
            "gemini-3.1-flash-lite", // 2026 Flash with better grounding, but watch for trailing artifacts
            "gemma-3-4b-it", // 2026 Stable with best grounding, but watch for trailing artifacts
            "gemini-2.0-flash", // Primary 2026 Stable           
            "gemma-4-31b-it" // 2026 Heavy with best grounding, but watch for trailing artifacts
            
            //"gemini-1.5-flash", // Reliable Fallback
            //"gemini-1.5-pro"    // Heavy Logic Fallback
        });

        if (string.IsNullOrWhiteSpace(_apiKey) ||
            _apiKey.Length < 10 ||
            _apiKey.Equals("REPLACE_THIS_IN_USER_SECRETS", StringComparison.OrdinalIgnoreCase))
        {
            yield return "❌ **Configuration Error**: Gemini API Key appears to be invalid or missing. Set `Gemini:ApiKey` in User Secrets or an environment variable.";
            yield break;
        }

        string? lastError = null;

        while (modelQueue.Count > 0)
        {
            var model = modelQueue.Dequeue();
            // Passing the key in the URL is more reliable than the header in many environments
            var relativeUrl = $"v1beta/models/{model}:streamGenerateContent?alt=sse&key={_apiKey}";
            
            // Mask the key for the UI display
            var maskedKey = _apiKey.Length > 8 ? $"{_apiKey[..4]}...{_apiKey[^4..]}" : "****";
            var displayUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:streamGenerateContent?alt=sse&key={maskedKey}";
            
            yield return $"__DINO_STATUS__:Trying {model}...";

            // Guard against trailing artifacts and grounding citations by prepending to the prompt
            var systemInstruction = "System Note: Do not include trailing grounding metadata, source citations, or thinking markers like '*.. ..' at the end of your response. End your response cleanly.\n\n";
            var combinedPrompt = systemInstruction + prompt;

            var requestBody = new 
            { 
                contents = new[] { new { parts = new[] { new { text = combinedPrompt } } } }
            };
            
            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage? response = null;
            try 
            {
                response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                lastError = $"[{model}] Network/Local failure: {ex.Message} (URL: {displayUrl})";
                continue; 
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                lastError = $"[{model}] API Error {(int)response.StatusCode}: {response.ReasonPhrase}\nURL: {displayUrl}\nResponse: {errorBody}";
                response.Dispose();
                continue; 
            }

            lastError = null; 
            yield return $"__DINO_STATUS__:Connected to {model} - streaming...";

            // Real Server-Sent Events reading
            using var responseLease = response;
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            bool successStreamed = false;

            string? line;
            while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
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
                            var candidate = candidates[0];
                            if (candidate.TryGetProperty("content", out var content) && content.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                            {
                                if (parts[0].TryGetProperty("text", out var textEl))
                                {
                                    successStreamed = true;
                                    chunkToYield = textEl.GetString();
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore partial JSON blocks if parsing fails mid-stream
                    }

                    if (chunkToYield != null)
                    {
                        // Final safety check: if the chunk itself contains known stop artifacts
                        if (chunkToYield.Contains("*.. ..") || chunkToYield.Contains("[Citations]"))
                        {
                             continue;
                        }
                        
                        yield return chunkToYield;
                    }
                }
            }
            
            if (successStreamed)
                yield break; // We successfully streamed from this model, don't try the rest.
        }
        
        yield return $"\n\n❌ **All Gemini models failed**. Final error: {lastError ?? "Unknown failure"}. Please try again later.";
    }

    public async Task<string> RefinePromptAsync(
        string userPrompt,
        string usageKey,
        CancellationToken cancellationToken = default) 
    {
        var metaPrompt = $@"You are an elite Prompt Engineer. Rewrite the user's draft prompt into a professional, highly structured prompt.
Use advanced frameworks like CO-STAR or RACE (Context, Objective, Style, Tone, Audience, Response).
IMPORTANT: Output ONLY the raw refined prompt text. Do not explain what you did. Do not include markdown codeblocks around the text. Do not say 'Here is your prompt'.

Draft Prompt:
{userPrompt}";

        var sb = new StringBuilder();
        await foreach(var chunk in AskStreamAsync(metaPrompt, usageKey, cancellationToken))
        {
            sb.Append(chunk);
        }
        return sb.ToString().Trim();
    }

    public async Task<string> TranslatePromptAsync(
        string sourceText,
        string targetLanguage,
        string usageKey,
        CancellationToken cancellationToken = default) 
    {
        var metaPrompt = $@"You are an elite bilingual translator. Translate the following text into strictly {targetLanguage}.
Retain all markdown formatting exactly as it is (like bolding, lists, and codeblocks).
IMPORTANT: Output ONLY the translated text. Do not explain what you did.

Text to Translate:
{sourceText}";

        var sb = new StringBuilder();
        await foreach(var chunk in AskStreamAsync(metaPrompt, usageKey, cancellationToken))
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
