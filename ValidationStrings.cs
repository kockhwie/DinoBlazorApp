using System.Globalization;

namespace DinoBlazorApp_v2;

public static class ValidationStrings
{
    public static string Required => IsChinese
        ? "這個欄位為必填。"
        : "The {0} field is required.";

    public static string StringLength => IsChinese
        ? "這個欄位的長度必須介於 {2} 到 {1} 個字元之間。"
        : "The {0} must be at least {2} and at max {1} characters long.";

    public static string PasswordMismatch => IsChinese
        ? "密碼與確認密碼不相符。"
        : "The password and confirmation password do not match.";

    public static string InvalidEmail => IsChinese
        ? "這個欄位不是有效的電子郵件地址。"
        : "The {0} field is not a valid e-mail address.";

    private static bool IsChinese =>
        CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase);
}
