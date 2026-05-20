# Redesign-Umsetzungsplan

Basis: `docs/MOCKUP_FEATURES.md` + Entscheidungen vom 2026-05-20  
Ziel: Pixelgenaue Übernahme des Mockup-Designs in die Blazor WASM App.

---

## Entscheidungslog

| Thema | Entscheidung |
|---|---|
| `FixedText`-Block, `CheckList`-Block | Bleiben erhalten — waren nur Mockup-Einschränkung |
| Fußzeile (Footer-Sektion) | Bleibt erhalten — war nur Mockup-Einschränkung |
| Tweaks-Panel | Wird **nicht** als floating Panel übernommen; stattdessen Zahnrad-Icon (`⚙`) oben rechts im Topbar, das ein Flyout öffnet (Theme, Density, Akzentfarbe) |
| Theme Toggle (Sonne/Mond) | Geht im Settings-Flyout auf |

---

## Technische Architektur-Notizen

### Icon-System
Die Icons aus `icons.jsx` / `ICON_PATHS` werden als C#-Dictionary mit SVG-Path-Strings abgebildet (`Helpers.Icons`). Eine `IconHelper.Svg(name, size, color)` Methode gibt fertigen SVG-HTML-String zurück, der per `@((MarkupString)...)` oder als eigene `<Icon>`-Komponente (`Components/Icon.razor`) gerendert wird. Das gleiche Dictionary wird auch im `TemplateGeneratorService` für die Inline-SVGs im generierten eBay-HTML verwendet.

### CSS Theming
Aktuell sind alle Farben im Dark Theme fest verdrahtet. Umstellung auf semantische Tokens per `data-theme`-Attribut auf `<html>`:
```css
[data-theme="light"] {
  --color-bg: #f8f9fa;
  --color-text: #1a1a1a;
  /* ... */
}
```
Akzentfarben werden als OKLCH-Variablen umgestellt (`--accent-h`, `--accent-c`, `--accent-l`), sodass alle Ableitungen automatisch korrekt sind. Fünf Presets (Emerald, Indigo, Orange, Rot, Gelb) — Gelb `#f5c518` ist der bisherige Default.

### Settings-Flyout
Neuer State in `Index.razor.cs`: `_showSettings`, `_theme` (dark/light), `_density` (comfortable/compact), `_accentPreset`. Werte werden in `localStorage` persistiert (eigener Key `relexx-settings`). Das Flyout setzt `data-theme` und `data-density` per JS auf `document.documentElement`.

### Panel & Field als Blazor-Komponenten
Neue Dateien `Components/Panel.razor` und `Components/Field.razor` (keine Code-Behind nötig). Nutzung via `<Panel Title="..." Icon="blocks" Badge="5">...</Panel>`. Dadurch werden alle Stage-Render-Fragments kürzer und konsistenter.

### Preview mit iframe
`<iframe @ref="_previewFrame">` + `OnAfterRenderAsync` setzt `srcdoc` via JS-Interop (`window.setIframeSrcDoc(element, html)`). Kein URL-Encoding-Problem, kein CSS-Bleed aus der App-Shell.

### Syntax-Highlighting
Eigene C#-Methode `SyntaxHighlightHtml(string html)` in `TemplateGeneratorService` (oder Hilfsmethode in Index), gibt `MarkupString` mit `<span>`-Tags zurück. Klassen: `.code-tag`, `.code-attr`, `.code-string`, `.code-comment`.

---

## Meilensteine

> **Status: Alle Meilensteine abgeschlossen** (Stand 2026-05-21)

### M1 — Design-System-Fundament ✓
*Alle nachfolgenden Milestones hängen davon ab.*

**Scope:**
- Geist + Geist Mono in `index.html` einbinden (Google Fonts)
- CSS komplett auf OKLCH-Akzent-Variablen umstellen (`--accent-h/c/l`)
- `data-theme`-Strukturen für Dark und Light anlegen (alle bestehenden Farb-Variablen als Dark-Default)
- `data-density`-Variablen für Spacing/Padding (`--gap`, `--pad`, etc.)
- `.mono`-Klasse für Code-Inputs
- `Icon.razor`-Komponente + `IconHelper.cs` (alle 20 Icons aus `ICON_PATHS` + `SVG_RAW`)

**Dateien:** `wwwroot/index.html`, `wwwroot/css/app.css`, `Components/Icon.razor`, `Helpers.cs` (neuer Abschnitt)  
**Aufwand:** M  
**Risiko:** CSS-Variablen-Umbau kann bestehende Komponenten-Farben verschieben → nach Commit visuell prüfen.

---

### M2 — App-Shell ✓
*Benötigt: M1*

**Scope:**
- Custom SVG-Wordmark (`relexx_tmpl` + animierter Cursor) ersetzt `<h1>`
- Settings-Flyout: Zahnrad-Button im Topbar (ersetzt bisherigen Platzhalter), öffnet Flyout mit:
  - Theme-Umschalter (Dark / Light) → schreibt `data-theme` auf `<html>` via JS
  - Density-Umschalter (Comfortable / Compact) → schreibt `data-density` auf `<html>` via JS
  - 5 Akzentfarben-Swatches → setzt `--accent-h/c/l` via JS
  - Persistenz in `localStorage` (Key `relexx-settings`)
- Stepper: `maxReached`-Tracking → bereits besuchte Steps als `is-done` markiert; vorwärts-Navigation nur bis `maxReached + 1`
- Keyboard-Navigation: `←`/`→` wechseln Stage (JS `keydown`-Listener, ignoriert Inputs)

**Dateien:** `Pages/Index.razor`, `Pages/Index.razor.cs`, `wwwroot/css/app.css`, `wwwroot/index.html` (JS-Helper für Theme/Density)  
**Aufwand:** M  
**Abhängig von:** M1 (braucht Icon-Komponente für Gear + Sun/Moon, OKLCH-Variablen für Akzent-Swatches)

---

### M3 — Panel & Field Komponenten ✓
*Benötigt: M1*

**Scope:**
- `Components/Panel.razor`: Parameter `Title`, `Icon` (string → Icon-Komponente), `Badge` (string?), `Actions` (RenderFragment?), `ChildContent` (RenderFragment), `Tight` (bool)
- `Components/Field.razor`: Parameter `Label` (string?), `Hint` (string?), `ChildContent` (RenderFragment)
- CSS für `.panel`, `.panel-head`, `.panel-body`, `.field`, `.field-label`, `.hint`, `.badge`
- Bestehende `sidebar-section`-Blöcke im Layout-Editor testweise auf Panel umstellen

**Dateien:** `Components/Panel.razor`, `Components/Field.razor`, `wwwroot/css/app.css`  
**Aufwand:** S  
**Abhängig von:** M1

---

### M4 — Stage-Rahmen: page-head + btn-stack ✓
*Benötigt: M1, M2, M3*

**Scope:**
- Jede Stage erhält `<div class="page-head">` mit `<h1>`, `.page-sub` und `.page-head-actions`
- Sekundäre Toolbar-Buttons (Neu, Duplizieren, Export, Import) wandern als `btn-stack` in `page-head-actions`; Toolbar-Leiste enthält danach nur noch: `[select] [divider] [meta-info] [flex-spacer] [delete]`
- `divider`-CSS-Klasse
- CSS-Layout für `.page-head`, `.page-head-actions`, `.btn-stack`, `.stage-foot`
- `stage-foot`-Bereich (Zurück + Weiter) konsistent für alle Stages

**Dateien:** `Pages/Index.razor`, `wwwroot/css/app.css`  
**Aufwand:** M  
**Abhängig von:** M1–M3  
**Risiko:** Strukturänderung an allen 4 Stages — gründlich testen, Flyout-Buttons auf Funktion prüfen.

---

### M5 — Layout-Phase verfeinern ✓
*Benötigt: M4*

**Scope:**
- Toolbar-Metadaten: Anzahl Blöcke + relatives Änderungsdatum (`currentLayout.ModifiedAt`) — Formatierung: „5 Blöcke · vor 3 Min."
- Delete-Button aus Block-Editor in Toolbar (rechts) verschieben
- Block-ID als Badge im `<Panel>`-Header des Block-Editors
- Gesamten Layout-Editor auf Panel/Field-Komponenten umstellen
- Icon-Picker und Bullet-Picker mit neuer Icon-Komponente statt Emoji

**Dateien:** `Pages/Index.razor`, `Pages/Index.razor.cs`, `wwwroot/css/app.css`  
**Aufwand:** S

---

### M6 — Eingabe-Phase verfeinern ✓
*Benötigt: M4*

**Scope:**
- `content-grid`: 2-spaltiges CSS-Grid für die Eingabe-Karten (auf breiten Screens), 1-spaltig auf Mobile
- Alle Eingabe-Sektionen auf `<Panel>` + `<Field>` umstellen (inkl. Badges: „Pflicht", „Markdown", live Zeichenzahl)
- Image-Block: Drag-and-Drop-Dropzone (`ondragover`/`ondrop` in Blazor + JS-Fallback) mit Bild-Vorschau
- Textarea-Hints: Formathinweise im `<Field hint="...">` (z. B. „**fett** · *kursiv*" für RichText, „Eigenschaft | Wert" für Datentabelle)
- `.mono`-Klasse auf URL- und ID-Inputs anwenden

**Dateien:** `Pages/Index.razor`, `Pages/Index.razor.cs`, `wwwroot/css/app.css`, `wwwroot/index.html` (Dropzone-JS)  
**Aufwand:** M

---

### M7 — Vorschau-Phase ✓
*Benötigt: M4*

**Scope:**
- Desktop/Mobile-Toggle als `btn-stack` in `page-head-actions`; State `_previewDevice` (desktop/mobile) in Index
- `<iframe>` mit `@ref` statt `@(MarkupString)generatedHtml`
- JS-Helper `window.setIframeSrcDoc(element, html)` in `index.html`
- `OnAfterRenderAsync` ruft Helper auf wenn Phase 2 aktiv
- CSS: `.preview-shell`, `.preview-frame` mit `max-width`-Transition (380 px ↔ 760 px)

**Dateien:** `Pages/Index.razor`, `Pages/Index.razor.cs`, `wwwroot/index.html`, `wwwroot/css/app.css`  
**Aufwand:** S

---

### M8 — HTML-Phase ✓
*Benötigt: M4*

**Scope:**
- Datei-Stats: `generatedHtml.Split('\n').Length` (Zeilen) + `Encoding.UTF8.GetByteCount(generatedHtml) / 1024.0` (kB) als `.tag.mono`-Chips in `page-head-actions`
- Syntax-Highlighting: C#-Methode `SyntaxHighlightHtml(string html)` → `MarkupString` mit Klassen `.code-tag`, `.code-attr`, `.code-string`, `.code-comment`
- CSS für Code-Block Farben (an Dark/Light Theme angepasst)
- „UTF-8 · text/html" Metainfo in der Code-Toolbar
- `.tag` / `.tag.accent` / `.tag.mono` CSS-Klassen

**Dateien:** `Pages/Index.razor`, `Pages/Index.razor.cs`, `Services/TemplateGeneratorService.cs`, `wwwroot/css/app.css`  
**Aufwand:** S

---

### M9 — Generiertes eBay-HTML ✓
*Benötigt: M1 (IconHelper für SVG-Pfade)*

**Scope:**
- `IconHelper.SvgForHtml(iconName, accentColor, size)` — gibt SVG-String für Inline-Einbettung zurück
- In `TemplateGeneratorService`: alle Abschnitts-Titel verwenden SVG statt Emoji
- Geist-Font-Stack: `font-family: 'Geist', -apple-system, system-ui, sans-serif` im äußeren Container
- Äußeren Container auf `<div style="max-width: 760px; margin: 0 auto;">` umstellen (statt reiner Table-Wrapper-Logik)
- Mobile-Meta-Tag `<meta name="viewport">` in Output einbauen (Mockup macht das)

**Dateien:** `Services/TemplateGeneratorService.cs`, `Helpers.cs`  
**Aufwand:** S  
**Hinweis:** Ändert das Format des generierten HTML — bestehende gespeicherte Artikel sind davon nicht betroffen (Content bleibt gleich), nur das Rendering-Output ändert sich.

---

## Reihenfolge & Abhängigkeiten

```
M1 (Design-System-Fundament)
├── M2 (App-Shell) ──────────────────────┐
├── M3 (Panel & Field)                   │
│   └── M4 (Stage-Rahmen) ←─────────────┘  (braucht M1 + M2 + M3)
│       ├── M5 (Layout-Phase)
│       ├── M6 (Eingabe-Phase)
│       ├── M7 (Vorschau-Phase)
│       └── M8 (HTML-Phase)
└── M9 (Generiertes HTML)  ← kann parallel zu M4–M8 laufen
```

M9 ist von M4 unabhängig und kann jederzeit nach M1 angegangen werden.

---

## Aufwandsschätzung

| Meilenstein | Größe | Hauptrisiko |
|---|---|---|
| M1 Design-System | M (4–6h) | CSS-Umbau kann bestehende Stile brechen |
| M2 App-Shell | M (3–5h) | Theme-Persistence + JS-Interop |
| M3 Panel/Field | S (2–3h) | Komponentenschnittstelle richtig definieren |
| M4 Stage-Rahmen | M (3–4h) | Alle 4 Stages anfassen |
| M5 Layout-Phase | S (2–3h) | gering |
| M6 Eingabe-Phase | M (3–4h) | Dropzone-JS |
| M7 Vorschau | S (1–2h) | iframe srcDoc Interop |
| M8 HTML-Phase | S (2–3h) | Regex-basiertes Highlighting |
| M9 Generiertes HTML | S (2–3h) | SVG-Inline korrekt escapen |
| **Gesamt** | **~25–35h** | |
