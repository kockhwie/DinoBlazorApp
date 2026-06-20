using DinoAI.Resources;
using Microsoft.Extensions.Localization;
 

namespace DinoAI.Utilities
{
    public static class Loc
    {
        private static IStringLocalizer<SharedResource>? _localizer;

        // Called once at startup
        public static void Configure(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        // Safe accessor
        public static string Get(string key)
        {
            return _localizer?[key] ?? key;
        }
    }
}
