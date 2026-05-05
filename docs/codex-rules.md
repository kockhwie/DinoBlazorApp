# Codex Rules

Keep this short and current. These are the rules that matter most for future runs in this repo.

## Work Style

- Read the smallest set of files that answers the task.
- Do not re-dump large files unless the file changed or the user asked for it.
- Prefer targeted searches and line reads over broad repo scans.
- Keep commentary brief while working; report only what changes the next step.

## Security And Production

- Treat the app as production-bound for Render unless told otherwise.
- Keep secrets out of source control.
- Use user secrets or environment variables for real API keys.
- Do not re-add `appsettings.json` manually after `WebApplication.CreateBuilder(args)`.
- Keep the Render/inotify mitigation before `CreateBuilder`.
- Keep Blazor Interactive Server enabled for the main shell.
- Prefer encrypted DataProtection keys on non-Windows hosts; use ephemeral keys only when no safe persistence is available.

## Repo Boundaries

- Do not use destructive directory commands unless contents were checked first.
- Treat `.appdata/` and `.dotnetcli/` as local-only runtime folders.
- Keep `DinoBlazorApp.Tests` outside the app folder.

## Review Mode

- Start with findings, not a summary.
- Prioritize security, correctness, and production risk.
- Call out file and line references for every finding.
- Keep the final answer short and actionable.
