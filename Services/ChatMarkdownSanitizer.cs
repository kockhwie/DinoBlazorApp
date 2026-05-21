using System.Text.RegularExpressions;

namespace DinoBlazorApp.Services;

/// <summary>
/// Post-processes Markdig HTML to block dangerous URI schemes in links and images.
/// </summary>
public static partial class ChatMarkdownSanitizer
{
    private static readonly Regex DangerousUriAttribute = DangerousUriAttributeRegex();

    public static string SanitizeHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return html;
        }

        return DangerousUriAttribute.Replace(html, match =>
        {
            var attr = match.Groups["attr"].Value;
            return $"{attr}=\"#\"";
        });
    }

    public static bool ContainsBlockedUri(string html)
        => DangerousUriAttribute.IsMatch(html);

    [GeneratedRegex(
        @"(?<attr>href|src)\s*=\s*""(?<url>(?:javascript|data|vbscript):[^""]*)""",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex DangerousUriAttributeRegex();
}
