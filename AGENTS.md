# Repo Guidance

Keep the detailed working rules in [docs/codex-rules.md](docs/codex-rules.md). Use this file as the short repo contract.

## Stack

- Target `.NET 10` and modern C#.
- Keep the app on Blazor Interactive Server.
- Follow the repo’s existing clean-architecture shape.

## UI And UX

- Prefer DaisyUI components and Tailwind utilities for all UI work unless there is a strong reason to do otherwise.
- Keep layouts minimal, responsive, and visually consistent with DaisyUI patterns.
- Avoid custom CSS when a DaisyUI component or utility class can express the same result cleanly.

## Security And Config

- Keep secrets out of source control.
- Use user secrets or environment variables for real API keys.
- Do not re-add `appsettings.json` manually after `WebApplication.CreateBuilder(args)`.
- Keep the Render/inotify mitigation before `CreateBuilder`.
- Persist DataProtection keys only when needed, and encrypt them at rest on non-Windows hosts.
- Do not reintroduce Windows Event Log logging.

## Workspace Boundaries

- Treat `.appdata/` and `.dotnetcli/` as local-only runtime folders.
- Do not use destructive directory commands unless contents were checked first.
- Keep `DinoBlazorApp.Tests` outside the app folder.

# Coding Rules

## Architecture

- Use ASP.NET Core 10
- Razor Pages
- PostgreSQL

## Data Access

- Prefer EF Core

- Use Dapper for:
  - Complex joins
  - Reporting
  - Dashboard queries
  - Performance critical queries

## Avoid

- Repository Pattern
- Unit Of Work
- Generic Repository
- Generic Service
- AutoMapper

## Design

- Feature Folder Structure

## Principles

- Keep code simple
- Avoid unnecessary abstractions
- Minimize file count
- Prefer explicit code