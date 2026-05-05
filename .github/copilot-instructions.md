# DinoBlazorApp AI Coding Instructions

## Configuration and Secrets

- Never use or depend on API keys in `appsettings.json`; placeholder values such as `REPLACE_THIS_IN_USER_SECRETS` are dummy only.
- Real API keys must come from Visual Studio User Secrets or environment variables.
- Do not manually call `builder.Configuration.AddJsonFile("appsettings.json", ...)` after `WebApplication.CreateBuilder(args)`. ASP.NET Core already loads appsettings, environment-specific appsettings, User Secrets, and environment variables in the correct priority order. Re-adding appsettings later can override User Secrets with dummy placeholder values.
- Keep the Render/inotify mitigation before `WebApplication.CreateBuilder(args)`: set `DOTNET_hostBuilder__reloadConfigOnChange=false` on non-Windows hosts before builder creation. Do not move it below `CreateBuilder`.

