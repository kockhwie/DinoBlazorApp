# Repo Guidance

- Keep Blazor interactive server rendering enabled for the main app shell.
- Persist DataProtection keys locally only when needed, and always encrypt them at rest.
- Do not reintroduce Windows Event Log logging in this workspace.
- Keep secrets out of source control; use user secrets or environment variables.
- Treat `.appdata/` and `.dotnetcli/` as local-only runtime folders.
- If `.appdata/` or `.dotnetcli/` are already tracked, remember `.gitignore` only affects future commits; ask before untracking or rewriting history.
