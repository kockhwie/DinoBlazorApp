# Repo Guidance

## 🚀 Critical Tech Stack
- **Target Framework:** .NET 10 (Strictly use C# 14/latest features).
- **Core Pattern:** Clean Architecture + Blazor Interactive Server.
- **Standards:** Refer to `docs/modern_csharp.md` for coding style.

## 🛠️ Environment Rules

- Keep Blazor interactive server rendering enabled for the main app shell.
- Persist DataProtection keys locally only when needed, and always encrypt them at rest.
- Do not reintroduce Windows Event Log logging in this workspace.
- Keep secrets out of source control; use user secrets or environment variables.
- Treat `.appdata/` and `.dotnetcli/` as local-only runtime folders.
- If `.appdata/` or `.dotnetcli/` are already tracked, remember `.gitignore` only affects future commits; ask before untracking or rewriting history.
- On non-Windows hosts, prefer `DataProtection:CertificateBase64` or `DataProtection:CertificatePath`; otherwise use ephemeral keys instead of plaintext storage.

## 🛑 Safety & Workspace Boundaries

- **NEVER use destructive commands** (`Remove-Item`, `rm -rf`, etc.) on entire directories without explicitly listing the contents first to verify nothing valuable is inside. Do not trust user assumptions that a folder is a "safe duplicate."
- **Test Project Location:** The XUnit test project (`DinoBlazorApp.Tests`) must permanently remain OUTSIDE the `DinoBlazorApp` application folder (e.g., side-by-side at the `repos/` level). Do not continuously attempt to generate a test folder inside the main web project, as it causes namespace and visual studio display conflicts.
