# relexx' Template Generator

Eine moderne Blazor WebAssembly Anwendung zur Erstellung professioneller, eBay-konformer HTML-Templates für Artikelbeschreibungen – mit modularem Block-System, Drag & Drop Editor und vollständig anpassbarer Oberfläche.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Blazor WASM](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)
![License](https://img.shields.io/badge/License-MIT-green)

---

## Features

### 4-Phasen-Workflow

| Phase            | Beschreibung                                                           |
| ---------------- | ---------------------------------------------------------------------- |
| **0 – Layout**   | Layout erstellen/bearbeiten, Blöcke konfigurieren, Farbschema anpassen |
| **1 – Eingabe**  | Artikeldaten erfassen, Bilder hochladen, Markdown-Texte schreiben      |
| **2 – Vorschau** | Isolierte Live-Vorschau (Desktop / Mobile) in einem iframe             |
| **3 – HTML**     | Fertigen HTML-Code mit Syntax-Highlighting kopieren oder herunterladen |

Phasen werden im Stepper als erledigt (`✓`) markiert; vorwärts ist nur nach Abschluss der vorherigen Phase möglich. Mit **← / →** kann per Tastatur navigiert werden (außer in Eingabefeldern).

---

### Modulares Block-System

**7 Block-Typen:**

| Typ                    | Format                            | Optionen                                        |
| ---------------------- | --------------------------------- | ----------------------------------------------- |
| **Bild**               | URL oder Base64                   | Ausrichtung, maximale Breite                    |
| **Fließtext**          | Markdown                          | Titel ein/aus                                   |
| **Stichwort-Karten**   | `Titel \| Beschreibung` pro Zeile | 1–3 Spalten, Aufzählungszeichen                 |
| **Datentabelle**       | `Eigenschaft \| Wert` pro Zeile   | Spaltenüberschriften, Zebra-Streifen            |
| **Feature-Karten**     | `Kategorie \| Option1, Option2`   | Farbige Karten nebeneinander                    |
| **Aufzählungsliste**   | Ein Eintrag pro Zeile             | Aufzählungszeichen (✓ · ▸ ★ …)                  |
| **Fester Text**        | Markdown, im Layout hinterlegt    | Pro Artikel editierbar, Zurücksetzen auf Vorlage |

- **Drag & Drop**: Blöcke per SortableJS frei anordnen
- **Block-ID**: Eindeutiger Kurzname je Block, sichtbar als Badge im Editor; doppelte IDs werden abgefangen
- **Icon-Picker**: Jedem Block ein Lucide-Icon aus 6 kategorisierten Gruppen (Versand, Qualität, Technik, Kommunikation, Allgemein, UI) zuweisen
- **Demo-Daten**: Ein Klick füllt alle Felder des aktiven Layouts mit Beispielinhalten

---

### Layout-Management

- **Standard-Layout**: Vorkonfiguriertes Template mit 6 typischen eBay-Blöcken
- **Eigene Layouts**: Erstellen, umbenennen, duplizieren, löschen
- **Sicherheitsabfragen**: Alle destruktiven Aktionen (Block löschen, Layout löschen, Artikel zurücksetzen) erfordern eine Bestätigung im Confirm-Dialog
- **Import / Export**: Layouts als JSON sichern und wiederherstellen
- **Farbschema**: Primär-, Akzent- und Hintergrundfarbe frei wählbar
- **Portabilität**: Das komplette Layout wird beim Artikel-Export eingebettet; beim Import mit unterschiedlichem Layout erscheint ein Konfliktdialog (Layout übernehmen oder Daten einpassen)

---

### Eingabe-Phase

- **Bild-Drag & Drop**: Bilddatei direkt auf das Bildfeld ziehen – wird automatisch als Base64 eingebettet (max. 5 MB)
- **Datei-Upload**: Bild-URL oder Datei-Auswahl über Button
- **Format-Hints**: Jedes Block-Feld zeigt den erwarteten Eingabeformat-Hinweis (`Titel | Beschreibung`, `**fett** · *kursiv*` …)
- **Monospace-URLs**: Bild-URL-Felder werden in Monospace-Schrift dargestellt
- **Markdown-Unterstützung**: Fett, kursiv und Links in Fließtext- und FixedText-Blöcken
- **Mobile Summary**: Freitextfeld für schema.org-Produktbeschreibung (Google Shopping, max. 500 Zeichen)
- **Footer**: Bis zu 4 Zeilen `Schlüssel: Wert` für Herstellerangaben, SKU, Garantieinfos

---

### Vorschau-Phase

- **Isolierter iframe**: Die Vorschau rendert in einem echten `<iframe srcdoc>` – kein CSS-Bleed aus der App-Shell
- **Desktop / Mobile Toggle**: Zwischen breiter Desktop-Ansicht und schmaler Mobile-Simulation (375 px) umschalten

---

### HTML-Phase

- **Syntax-Highlighting**: HTML-Ausgabe wird coloriert dargestellt (One-Dark-Palette: Tags, Attribute, Strings, Kommentare)
- **Datei-Statistiken**: Zeilenanzahl, Dateigröße (kB), Zeichensatz als Badges
- **Kopieren**: HTML-Quellcode in die Zwischenablage kopieren
- **Herunterladen**: Fertiges `.html` herunterladen

---

### Generiertes eBay-HTML

- **Reine Inline-Styles**, kein externes CSS – maximale eBay-Kompatibilität
- **Tabellen-basiertes Layout** für E-Mail-Client-ähnliche Renderer
- **Viewport-Meta-Tag** für mobile Darstellung
- **SVG-Icons** in Block-Titeln (kleine Lucide-Piktogramme, Emoji-Fallback für ältere Daten)
- **Alternating Sections**: Gerade/ungerade Blöcke erhalten wechselnde Hintergründe
- **schema.org Mobile Summary** als unsichtbarer `<div>` für Google Shopping

---

### Oberfläche & Theming

- **Dark / Light Theme**: Umschaltbar per Einstellungs-Flyout, wird in `localStorage` gespeichert
- **Dichte**: Komfortabel (Standard) oder Kompakt – skaliert Abstände, Schriftgrößen und Steuerelemente
- **Akzent-Farbe**: 5 Voreinstellungen (Gelb, Blau, Grün, Lila, Rot) basierend auf OKLCH-Farbraum; sofortige Vorschau per CSS Custom Properties
- **Geist-Schriften**: Geist (Fließtext) und Geist Mono (Code, URLs) von Google Fonts
- **Lucide SVG-Icons**: Vektorgrafik-Icons als `<svg>`-Strings, inline gerendert – kein Webfont-Ladeoverhead
- **Panel / Field-Komponenten**: Wiederverwendbare Blazor-Komponenten für konsistente Abschnitte mit Titel, Icon, Badge und Actions-Slot
- **„Über die App"-Dialog**: Zeigt Versionsinformationen, Features und verwendete Bibliotheken; erscheint automatisch beim ersten Start und ist jederzeit über die Einstellungen abrufbar
- **„Alle lokalen Daten entfernen"**: Löscht sämtliche gespeicherten Daten (Layouts, Artikel, Einstellungen) per `localStorage.clear()` mit Sicherheitsabfrage

---

### Persistenz & Datenformate

- **LocalStorage**: Layouts, Artikeldaten und Einstellungen werden automatisch im Browser gespeichert (Keys: `relexx-layouts`, `relexx-article`, `relexx-settings`, `relexx-first-run`)
- **Artikel-Import / Export**: Vollständige Artikeldaten inkl. Layout als JSON sichern
- **Layout-Import / Export**: Layouts separat portieren
- **JSON-Schema für KI-Tools**: Generiert ein beschriftetes Schema für ChatGPT, Claude & Co. zur automatischen Befüllung; enthält Hinweise zum Dateiformat für den Download

---

## Schnellstart

### Voraussetzungen

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- IDE: Visual Studio 2022, VS Code oder JetBrains Rider

### Installation & Start

```bash
cd ebay-template-generator
dotnet restore
dotnet run
# oder mit Hot Reload:
dotnet watch run
```

Die Anwendung ist unter `https://localhost:5001` oder `http://localhost:5000` erreichbar.

---

## Projektstruktur

```
ebay-template-generator/
├── Constants.cs                    # Storage-Keys, Limits, Icon-Namen
├── Helpers.cs                      # ID-Generierung, IconHelper (Lucide SVG)
├── Program.cs                      # Service-Registrierung & App-Konfiguration
├── App.razor
├── _Imports.razor
│
├── Components/
│   ├── Icon.razor                  # Lucide-SVG-Komponente
│   ├── Panel.razor                 # Abschnitts-Container mit Titel/Icon/Badge/Actions
│   └── Field.razor                 # Formular-Zeile mit Label und Hint
│
├── Models/
│   ├── AppSettings.cs              # Theme, Density, AccentPreset (Record)
│   ├── ArticleData.cs              # Artikeldaten mit eingebettetem Layout
│   ├── BlockDefinition.cs          # Block-Typen, Optionen & Extensions
│   ├── ColorScheme.cs              # Farbschema-Modell
│   └── LayoutTemplate.cs           # Layout mit Block-Sammlung
│
├── Pages/
│   ├── Index.razor                 # Haupt-UI mit 4-Phasen-Workflow
│   └── Index.razor.cs              # Code-behind: State, JS-Interop, Phasen-Navigation
│
├── Services/
│   ├── LocalStorageService.cs      # ILocalStorageService + Implementierung (JS-Interop)
│   └── TemplateGeneratorService.cs # HTML- und JSON-Schema-Generierung
│
├── docs/
│   ├── MOCKUP_FEATURES.md          # Mockup-Analyse und Feature-Entscheidungen (alle umgesetzt)
│   ├── REDESIGN_PLAN.md            # Meilenstein-Plan für das UI-Redesign (alle Meilensteine abgeschlossen)
│   └── IDEAS.md                    # Ideen-Backlog: neue Block-Typen, Animationen, weitere Features
│
└── wwwroot/
    ├── css/app.css                 # OKLCH-Theming, Komponenten-Styles, Syntax-Highlighting
    ├── index.html                  # HTML-Einstiegspunkt + JS-Interop (SortableJS, Drag&Drop, Theme)
    └── staticwebapp.config.json    # Azure Static Web Apps Konfiguration
```

---

## Technologie-Stack

| Komponente   | Technologie                         |
| ------------ | ----------------------------------- |
| Frontend     | Blazor WebAssembly (.NET 8)         |
| Styling      | Custom CSS, OKLCH Custom Properties |
| Schriften    | Geist / Geist Mono (Google Fonts)   |
| Icons        | Lucide SVG (inline, kein Webfont)   |
| Markdown     | Markdig                             |
| Storage      | Custom `LocalStorageService` (JS-Interop) |
| Drag & Drop  | SortableJS                          |
| Hosting      | Azure Static Web Apps               |

---

## Tastaturkürzel

| Taste | Aktion                              |
| ----- | ----------------------------------- |
| `→`   | Nächste Phase (wenn freigeschaltet) |
| `←`   | Vorherige Phase                     |

---

## Deployment auf Azure Static Web Apps

### Option 1: GitHub Actions (empfohlen)

1. Repository auf GitHub erstellen und Code pushen
2. Im Azure Portal eine neue **Static Web App** erstellen
3. GitHub als Deployment-Quelle wählen
4. Build-Konfiguration: **App location** `/`, **Output location** `wwwroot`

### Option 2: Azure CLI

```bash
npm install -g @azure/static-web-apps-cli
dotnet publish -c Release -o publish
swa start publish/wwwroot
swa deploy publish/wwwroot --env production
```

---

## Mitwirken

1. Fork erstellen
2. Feature-Branch anlegen (`git checkout -b feature/neues-feature`)
3. Änderungen committen
4. Branch pushen
5. Pull Request erstellen

---

## Lizenz

MIT License – siehe [LICENSE](LICENSE.md) für Details.

---

## Danksagungen

- [Markdig](https://github.com/xoofx/markdig) – Markdown-Parser (BSD-2-Clause)
- [SortableJS](https://sortablejs.github.io/Sortable/) – Drag & Drop Bibliothek (MIT)
- [Lucide](https://lucide.dev/) – SVG-Icon-Bibliothek (ISC)
- [Geist](https://vercel.com/font) – Schriftfamilie von Vercel (SIL OFL 1.1)
