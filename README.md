# TaikoProject

A **Taiko no Tatsujin**-inspired rhythm game built with C# and WPF (.NET), developed as a course project for IERG3080 at CUHK.

> Hit red and blue drum notes in time with the music. Perfect your timing, build your combo, and see your final score on the results screen.

---

## Features

- 🥁 **Red & Blue note gameplay** — press the correct key when each note reaches the hit line
- ⚖️ **Three-tier judgement system** — Perfect / Good / Bad / Miss with separate timing windows
- 🎵 **BGM + hit sound effects** — background music plays throughout; each drum hit has audio feedback
- 🎯 **Combo & score tracking** — real-time combo counter and score display during gameplay
- 📊 **Results screen** — full breakdown of Perfect / Good / Bad / Miss counts and accuracy
- 🎶 **Song selection** — choose a song and difficulty before each game
- 🏆 **Achievement system** — global game controller tracks player achievements (Singleton pattern)

---

## Architecture

The project is split into two namespaces with a clean **Model–View** separation:

```
TaikoProject/
├── GameLogic/               # Core logic (TaikoProject.Core) — no WPF dependency
│   ├── GameManager.cs       # Timing, note lifecycle, judgement (Façade over game subsystem)
│   ├── ScoreManager.cs      # Score, combo, Perfect/Good/Bad/Miss counts
│   └── Note.cs              # Note data model + NoteColor / NoteJudgement enums
│
├── MainWindow.xaml/.cs      # Title screen
├── SongSelectWindow.xaml/.cs# Song and difficulty selection
├── GameWindow.xaml/.cs      # In-game screen (renders notes, forwards input to GameManager)
├── ResultWindow.xaml/.cs    # Post-game results display
├── Resource/                # Audio and chart assets
└── TaikoProject.sln
```

### Design Patterns Used

| Pattern | Where |
|---|---|
| **Model–View (MVC-style)** | `GameLogic/` is pure model; XAML windows are views + controllers |
| **Façade** | `GameManager` exposes `Initialize`, `StartGame`, `Update`, `ProcessHit` — UI never touches internals |
| **Observer** | C# events (`Click`, `KeyDown`, `Tick`) with `+=` subscriptions throughout |
| **Singleton** | `GameController.Instance` — single global access point for save data and achievements |
| **Composite** | WPF control tree; `GameManager` holds a `List<Note>` treated uniformly by logic and renderer |

---

## Controls

| Key | Action |
|---|---|
| `D` / `F` | Left drum (Red — Don) |
| `J` / `K` | Right drum (Red — Don) |
| `S` | Left rim (Blue — Ka) |
| `L` | Right rim (Blue — Ka) |
| `Enter` | Confirm / Start |
| `Escape` | Back |

> Key bindings may vary — check `GameWindow.xaml.cs` → `GameWindow_KeyDown` for the exact mapping.

---

## Getting Started

### Requirements

- Windows 10 / 11
- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download) (or Visual Studio 2022 with .NET desktop workload)

### Run in Visual Studio

1. Clone the repository:
   ```bash
   git clone https://github.com/Scyyyy4/TaikoProject.git
   ```
2. Open `TaikoProject.sln` in Visual Studio 2022.
3. Set `TaikoProject` as the startup project.
4. Press **F5** to build and run.

### Run from command line

```bash
cd TaikoProject
dotnet run
```

---

## Judgement Windows

| Judgement | Timing window | Score |
|---|---|---|
| **Perfect** | ±43 ms | Full |
| **Good** | ±106 ms | Partial |
| **Bad** | ±200 ms | 0 |
| **Miss** | Note passes without hit | 0, resets combo |

*(Values are approximate — see `GameManager.cs` for exact thresholds.)*

---

## Project Report

The full course report (system design, test plan, and reflections) is available in [`docs/Report.pdf`](docs/Report.pdf).

---

## Acknowledgements

Inspired by [Taiko no Tatsujin](https://en.wikipedia.org/wiki/Taiko_no_Tatsujin) and [Taiko Web](https://cjdgrevival.com/).  
Built for IERG3080 Fall 2025, CUHK — Group 5.
