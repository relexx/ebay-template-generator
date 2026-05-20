# Mockup Feature-Liste

Basis: `docs/Template Generator Mockup/` (icons-Variante)  
Ziel: Pixelgenaue Übernahme in die Blazor-App.

---

## Design-System

- [x] **Lucide SVG-Icons** — Emoji-Icons durch konsistente Lucide-Linie-Icons ersetzen (Buttons, Panels, Block-Liste, Stage-Indikatoren, generiertes HTML)
- [x] **Geist-Schriftarten** — `DM Sans` → `Geist`, `JetBrains Mono` → `Geist Mono`
- [x] **`mono`-Klasse** — URL-Felder, ID-Feld und Code-Textareas in Monospace rendern
- [x] **Dark / Light Mode Toggle** — Sun/Moon-Umschalter im Settings-Flyout (kein eigenständiger Header-Button); CSS via `data-theme`-Attribut auf `<html>`
- [x] **Density-Modus** — `comfortable` / `compact` per `data-density`-Attribut umschaltbar; Umschalter im Settings-Flyout (Zahnrad-Button im Topbar)
- [x] **Akzentfarben-Presets** — 5 auswählbare Akzentfarben (Grün, Indigo, Orange, Rot, Gelb) als OKLCH-basierte CSS-Variablen

---

## Chrome / App-Shell

- [x] **Wordmark** — Custom SVG-Logo `relexx_tmpl` mit Akzent-Farbe und animiertem Cursor statt plain-text `<h1>`
- [x] **Stepper `is-done` State** — Besuchte Schritte erhalten einen "erledigt"-Stil; Navigation nur bis zum bisher maximal erreichten Schritt (`maxReached`)
- [x] **Keyboard-Navigation** — `←` / `→` navigiert zwischen Stages (wenn kein Input fokussiert)

---

## Stage-Layout (Rahmen, gilt für alle Phasen)

- [x] **`page-head`-Bereich** — Jede Stage bekommt einen eigenen Header-Bereich mit `<h1>`, Untertitel-Beschreibung (`page-sub`) und `page-head-actions` rechts
- [x] **Sekundäre Toolbar-Buttons nach `page-head-actions`** — Neu/Duplizieren/Export/Import wandern in den Stage-Header; die Toolbar-Leiste wird schlanker
- [x] **`Panel`-Komponente** — Inhaltsbereiche als Karten mit eigenem Panel-Header (Titel + Icon + Badge + optionale Actions) und Panel-Body
- [x] **`Field`-Komponente** — Formularfelder in Field-Container mit Label + optionalem Hint-Text darunter
- [x] **`Badge`-System** — Kleine Info-Chips an Panel-Titeln (z. B. Block-Anzahl, „Pflicht", „Markdown", Zeichenzahl)
- [x] **`divider`-Element** — Visuelle Trennlinie (`<div class="divider">`) zwischen Toolbar-Sektionen

---

## Phase 0 – Layout

- [x] **Toolbar-Metadaten** — Toolbar zeigt: Anzahl Blöcke + „zuletzt geändert vor X Min."
- [x] **Delete-Button in Toolbar** — Löschen-Button rechts in der Toolbar (nicht im Block-Editor)
- [x] **Block-ID-Badge im Panel-Header** — Aktuelle Block-ID als Badge neben „Block bearbeiten"
- [x] **Feldanordnung im Block-Editor** — Reihenfolge: ID → Typ → Icon → Überschrift → ShowTitle-Checkbox → typ-spezifische Optionen

---

## Phase 1 – Eingabe

- [x] **`content-grid` Layout** — Zweispaltiges CSS-Grid für die Eingabefelder (statt einspaltige `form-grid`)
- [x] **Image-Dropzone** — Bild-Block bekommt Drag-and-Drop-Zone (`<div class="dropzone">`) zusätzlich zu URL-Input + Kamera-Button
- [x] **Textarea-Hints** — Formathinweise direkt am Textarea-Label (z. B. „**fett** · *kursiv*", „Eigenschaft | Wert")

---

## Phase 2 – Vorschau

- [x] **Desktop/Mobile-Toggle** — Zwei Buttons zum Wechsel zwischen Desktop-Breite (760 px) und Mobile-Breite (380 px), mit CSS-Transition
- [x] **`<iframe srcDoc>`-Rendering** — Vorschau in isoliertem iframe statt direktem `@(MarkupString)`, sodass Stile des generierten HTML nicht in die App-Shell durchschlagen

---

## Phase 3 – HTML

- [x] **Datei-Stats im Header** — Zeilen-Anzahl + Dateigröße (kB) als Tag-Chips in `page-head-actions`
- [x] **Syntax-Highlighting** — HTML-Code-Ansicht mit farbiger Hervorhebung (Tags, Attribute, Strings, Kommentare)
- [x] **UTF-8 / MIME-Hinweis** — Metainfo „UTF-8 · text/html" in der Code-Toolbar

---

## Generiertes eBay-HTML

- [x] **SVG-Icons in Abschnitts-Titeln** — Emoji-Icons durch Inline-SVG (`<svg>`) ersetzen, Farbe = Akzentfarbe
- [x] **Geist-Font-Stack im Output** — Generiertes HTML referenziert `'Geist', -apple-system, system-ui, sans-serif`
- [x] **Wrapper-`<div>` statt Table-Root** — Äußerer Container als `<div style="max-width:760px; margin:0 auto;">` statt reiner Table-Struktur

---

## Entschiedene Abweichungen vom Mockup

- **`FixedText`-Block, `CheckList`-Block, Fußzeile** — bleiben erhalten; waren nur Einschränkungen des Mockup-Prototypen.
- **Tweaks-Panel** — wird nicht als eigenständiges floating Panel übernommen. Dichte, Theme und Akzentfarbe gehen im Settings-Flyout auf (Zahnrad-Icon `⚙` oben rechts im Topbar).
