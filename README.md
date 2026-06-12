# Dino App v2.0
# 🦖 Dino AI Academy

> Don't just use AI. **Evolve with it.**

Dino AI Academy is a gamified, bilingual (English / 繁體中文) web app that teaches people how to write better emails with AI. Instead of dumping a blank chat box on you, it frames the AI as a companion you **level up** — Level 1 → 2 → 3 — each unlocking smarter assistance that drafts, refines, and inserts email copy straight into a composer.

**Live demo:** https://dinoai.codingdinos.asia

![Dino AI Academy hero](docs/screenshot-hero.png)

---

## ✨ Features

- **Leveling AI assistant** — a game-like progression (Level 1/2/3) that gradually unlocks richer email-writing help.
- **One-click prompt suggestions** — context chips like *Deadline extension*, *Follow-up*, and *Meeting request* to start fast.
- **Draft-to-composer flow** — generated drafts drop directly into the email composer with a clear confirmation.
- **Bilingual UI** — full English and Traditional Chinese (`zh-Hant`) localization via cookie + `Accept-Language` resolution.
- **Accounts** — ASP.NET Core Identity (registration, login, confirmed accounts, passkey support).
- **Resilient connection UX** — branded, localized Blazor reconnect modal with auto-retry on tab focus (see [Reconnect handling](#-reconnect-handling)).

---

## 🧱 Tech Stack

| Layer | Technology |
| --- | --- |
| Framework | **.NET 10 · Blazor Web App** (Interactive Server render mode) |
| Auth | ASP.NET Core Identity (cookies, passkeys, confirmed accounts) |
| Data | Entity Framework Core · SQL Server |
| AI | Google **Gemini** (`generativelanguage.googleapis.com`) via typed `HttpClient` |
| Styling | Tailwind CSS (browser) + DaisyUI 5 |
| Icons / Fonts | Tabler Icons, Iconify · Fraunces, DM Sans, Sora (Google Fonts) |
| Animation | dotLottie player |
| i18n | `IStringLocalizer` + `.resx`, cookie-based culture switching |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or a full instance)
- A Google **Gemini API key** — https://aistudio.google.com/apikey

### 1. Clone

```bash
git clone https://github.com/kockhwie/DinoBlazorApp.git
cd DinoBlazorApp
```

### 2. Configure secrets

Never commit secrets. Use [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=DinoAI;Trusted_Connection=True;MultipleActiveResultSets=true"
dotnet user-secrets set "Gemini:ApiKey" "YOUR_GEMINI_API_KEY"
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run

```bash
dotnet watch run
```

The app starts on `https://localhost:5001` (or the port shown in the console).

---

## ⚙️ Configuration

| Key | Description |
| --- | --- |
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Gemini:ApiKey` | Google Gemini API key used by `EmailAssistantService` |

The supported cultures are configured in `Program.cs` (`en`, `zh-Hant`). The default culture is English; users can switch via the language toggle, which posts to `/culture/set` and persists a culture cookie for one year.

---

## 🗂️ Project Structure

```
DinoBlazorApp/
├─ Components/
│  ├─ App.razor                  # Root document, render-mode wiring
│  ├─ Routes.razor
│  ├─ Account/                   # Identity UI (login, register, passkeys)
│  ├─ Layout/
│  │  └─ ReconnectModal.razor    # Branded, localized reconnect UI (+ .css/.js)
│  └─ Pages/                     # App pages
├─ Data/                         # EF Core DbContext, ApplicationUser, migrations
├─ Services/
│  └─ EmailAssistantService.cs   # Gemini integration
├─ Resources/                    # .resx localization files (en, zh-Hant)
├─ wwwroot/
└─ Program.cs                    # Startup, DI, localization, circuit options
```

---

## 🔌 Reconnect handling

Blazor Server keeps a live SignalR "circuit" between browser and server. If the socket drops (network blip, server idle/restart, laptop sleep), Blazor shows a reconnect modal. This app ships a **branded, localized** modal that:

- matches the Dino theme (dark surface, green accent, app fonts) instead of the default white/blue dialog,
- shows localized text for every state (rejoining / retrying / failed / paused),
- **auto-retries when the tab regains focus**, so returning from a backgrounded tab resumes silently,
- tunes the client retry cadence and server circuit retention so brief disconnects recover without a full page reload.

See `Components/Layout/ReconnectModal.razor` and the circuit options in `Program.cs`.

---

## 🤝 Contributing

Issues and PRs are welcome. Please keep UI strings localized (add to both `en` and `zh-Hant` `.resx` files) and avoid committing secrets.

## 📄 License

This project is currently unlicensed (all rights reserved). Add a `LICENSE` file to clarify reuse terms.

---

Built with .NET, Blazor, and a healthy respect for dinosaurs. 🦕
