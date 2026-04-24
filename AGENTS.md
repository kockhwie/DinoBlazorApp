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
