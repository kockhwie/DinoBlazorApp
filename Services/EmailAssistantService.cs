using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DinoAI.Services;

public interface IEmailAssistantService
{
    Task<EmailAssistantResult> GenerateEmailAsync(string prompt, CancellationToken cancellationToken = default);
    Task<EmailAssistantResult> RefineEmailAsync(string email, string instruction, CancellationToken cancellationToken = default);
    Task<EmailAssistantResult> UpgradeEmailAsync(string emailOrFramework, string tone, CancellationToken cancellationToken = default);
}

public sealed record EmailAssistantResult(string Content, bool UsedLiveModel, string ModelName, string? FallbackReason = null);

public sealed class EmailAssistantService(HttpClient httpClient, IConfiguration configuration, ILogger<EmailAssistantService> logger) : IEmailAssistantService
{
    private readonly string? apiKey = configuration["Gemini:ApiKey"];
    private readonly string[] modelQueue = BuildModelQueue(configuration);

    public Task<EmailAssistantResult> GenerateEmailAsync(string prompt, CancellationToken cancellationToken = default) =>
        CompleteAsync(
            "You help beginners draft practical emails. Return only the email content, including a subject line.",
            prompt,
            () => DemoGenerate(prompt),
            cancellationToken);

    public Task<EmailAssistantResult> RefineEmailAsync(string email, string instruction, CancellationToken cancellationToken = default) =>
        CompleteAsync(
            "You improve emails for clarity, tone, grammar, and structure. Keep the writer's meaning. Return only the improved email.",
            $"Instruction: {instruction}\n\nEmail:\n{email}",
            () => DemoRefine(email, instruction),
            cancellationToken);

    public Task<EmailAssistantResult> UpgradeEmailAsync(string emailOrFramework, string tone, CancellationToken cancellationToken = default) =>
        CompleteAsync(
            "You rewrite emails in a polished professional style. Return only the email content, including a subject line.",
            $"Tone: {tone}\n\nDraft or framework:\n{emailOrFramework}",
            () => DemoUpgrade(tone),
            cancellationToken);

    private async Task<EmailAssistantResult> CompleteAsync(
        string systemInstruction,
        string userPrompt,
        Func<string> fallback,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return new EmailAssistantResult(fallback(), false, "demo fallback", "No Gemini API key was found in configuration.");
        }

        var lastFailureReason = string.Empty;

        foreach (var model in modelQueue)
        {
            try
            {
                var request = new GeminiGenerateRequest(
                    [new GeminiContent("user", [new GeminiPart($"{systemInstruction}\n\n{userPrompt}")])],
                    new GeminiGenerationConfig(0.4, 2048));

                using var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"v1beta/models/{model}:generateContent")
                {
                    Content = JsonContent.Create(request)
                };

                httpRequest.Headers.TryAddWithoutValidation("x-goog-api-key", apiKey);

                var response = await httpClient.SendAsync(httpRequest, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    lastFailureReason = await ReadFailureReasonAsync(response, model, cancellationToken);
                    if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                    {
                        break;
                    }

                    continue;
                }

                var payload = await response.Content.ReadFromJsonAsync<GeminiGenerateResponse>(cancellationToken);
                var candidate = payload?.Candidates?.FirstOrDefault();
                var text = candidate?.Content?.Parts is { Count: > 0 } parts
                    ? string.Concat(parts.Select(part => part.Text))
                    : string.Empty;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (string.Equals(candidate?.FinishReason, "MAX_TOKENS", StringComparison.OrdinalIgnoreCase))
                    {
                        lastFailureReason = $"Gemini response from model '{model}' was incomplete because it reached the token limit.";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(candidate?.FinishReason)
                        && !string.Equals(candidate.FinishReason, "STOP", StringComparison.OrdinalIgnoreCase))
                    {
                        lastFailureReason = $"Gemini response from model '{model}' stopped with finish reason '{candidate.FinishReason}'.";
                        continue;
                    }

                    return new EmailAssistantResult(text.Trim(), true, model);
                }

                lastFailureReason = $"Gemini returned an empty response for model '{model}'.";
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                logger.LogWarning(ex, "Gemini request failed for model {Model}.", model);
                lastFailureReason = $"Gemini request failed for model '{model}'. Check the model name, API key permissions, quota, or network access.";
            }
        }

        return new EmailAssistantResult(fallback(), false, "demo fallback", string.IsNullOrWhiteSpace(lastFailureReason) ? "No usable Gemini model was available." : lastFailureReason);
    }

    private static string[] BuildModelQueue(IConfiguration configuration)
    {
        var configuredQueue = configuration.GetSection("Gemini:ModelQueue").Get<string[]>();
        if (configuredQueue is { Length: > 0 })
        {
            return configuredQueue.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        var singleModel = configuration["Gemini:Model"];
        if (!string.IsNullOrWhiteSpace(singleModel))
        {
            return [singleModel.Trim()];
        }

        return
        [
            "gemini-2.5-flash",
            "gemini-3.1-flash-lite",
            "gemma-3-4b-it",
            "gemini-2.0-flash",
            "gemma-4-31b-it"
        ];
    }

    private static async Task<string> ReadFailureReasonAsync(HttpResponseMessage response, string model, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var status = (int)response.StatusCode;
        return string.IsNullOrWhiteSpace(body)
            ? $"Gemini request failed for model '{model}' with HTTP {status}."
            : $"Gemini request failed for model '{model}' with HTTP {status}: {body}";
    }

    private static string DemoGenerate(string prompt)
    {
        var lowerPrompt = prompt.ToLowerInvariant();

        if (lowerPrompt.Contains("vacation") || lowerPrompt.Contains("leave") || lowerPrompt.Contains("holiday"))
        {
            return "Subject: Leave Request\n\nDear [Recipient Name],\n\nI hope you are well. I would like to request leave from [start date] to [end date].\n\nI will make sure my current work is updated before I am away, and I am happy to help with any handover needed.\n\nThank you for considering my request.\n\nBest regards,\n[Your Name]";
        }

        if (lowerPrompt.Contains("follow"))
        {
            return "Subject: Follow-up on Our Previous Discussion\n\nDear [Recipient Name],\n\nI hope you are well. I am following up on our recent discussion to check if there are any updates or next steps.\n\nPlease let me know if you need any additional information from me.\n\nThank you, and I look forward to hearing from you.\n\nBest regards,\n[Your Name]";
        }

        if (lowerPrompt.Contains("complaint"))
        {
            return "Subject: Request for Assistance Regarding Recent Issue\n\nDear [Recipient Name],\n\nI am writing to raise a concern about [briefly describe the issue]. I would appreciate your help in reviewing this matter and advising on the next steps.\n\nThank you for your attention. I look forward to your response.\n\nBest regards,\n[Your Name]";
        }

        return "Subject: Request for Deadline Extension\n\nDear [Recipient Name],\n\nI hope you are well. I am writing to request a short extension for the current deadline.\n\nI need a little more time to complete the work properly and make sure the final result is clear and complete. If possible, I would appreciate an extension until [new date].\n\nThank you for your understanding. Please let me know if this would be acceptable.\n\nBest regards,\n[Your Name]";
    }

    private static string DemoRefine(string email, string instruction) =>
        $"Subject: Refined Email\n\nDear [Recipient Name],\n\nI hope you are well. I wanted to share the following message in a clearer and more polished way:\n\n{email.Trim()}\n\n{instruction}\n\nThank you for your time. I look forward to your response.\n\nBest regards,\n[Your Name]";

    private static string DemoUpgrade(string tone) =>
        tone switch
        {
            "Polite" => "Subject: Request for Timeline Review\n\nDear [Recipient Name],\n\nI hope you are well. I would like to discuss the current timeline and explore whether a small adjustment may help us maintain the quality of the final work.\n\nI appreciate your understanding and would welcome your thoughts on the best way forward.\n\nWarm regards,\n[Your Name]",
            "Persuasive" => "Subject: Request to Support a Better Final Outcome\n\nDear [Recipient Name],\n\nI am writing to request a short timeline adjustment so I can complete the work to a stronger standard.\n\nWith a little additional time, I can provide a clearer, more complete result and reduce the need for follow-up revisions. I would appreciate your support for this request.\n\nBest regards,\n[Your Name]",
            _ => "Subject: Request for Deadline Extension\n\nDear [Recipient Name],\n\nI am writing to request a short extension for the current deadline. This additional time would allow me to complete the remaining work thoroughly and deliver a more polished final result.\n\nThank you for your consideration. I look forward to your response.\n\nBest regards,\n[Your Name]"
        };

    private sealed record GeminiGenerateRequest(
        [property: JsonPropertyName("contents")] IReadOnlyList<GeminiContent> Contents,
        [property: JsonPropertyName("generationConfig")] GeminiGenerationConfig GenerationConfig);

    private sealed record GeminiContent(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("parts")] IReadOnlyList<GeminiPart> Parts);

    private sealed record GeminiPart([property: JsonPropertyName("text")] string Text);

    private sealed record GeminiGenerationConfig(
        [property: JsonPropertyName("temperature")] double Temperature,
        [property: JsonPropertyName("maxOutputTokens")] int MaxOutputTokens);

    private sealed record GeminiGenerateResponse([property: JsonPropertyName("candidates")] IReadOnlyList<GeminiCandidate>? Candidates);

    private sealed record GeminiCandidate(
        [property: JsonPropertyName("content")] GeminiContent? Content,
        [property: JsonPropertyName("finishReason")] string? FinishReason);
}
