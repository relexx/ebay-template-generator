# Mockup Feature-Liste

Basis: `Template Generator Mockup/` (icons-Variante)  
Ziel: Pixelgenaue Übernahme in die Blazor-App.

---

## Design-System

- [ ] **Lucide SVG-Icons** — Emoji-Icons durch konsistente Lucide-Linie-Icons ersetzen (Buttons, Panels, Block-Liste, Stage-Indikatoren, generiertes HTML)
- [ ] **Geist-Schriftarten** — `DM Sans` → `Geist`, `JetBrains Mono` → `Geist Mono`
- [ ] **`mono`-Klasse** — URL-Felder, ID-Feld und Code-Textareas in Monospace rendern
- [ ] **Dark / Light Mode Toggle** — Umschaltknopf im Header (Sun/Moon-Icon); CSS via `data-theme`-Attribut
- [ ] **Density-Modus** — `comfortable` / `compact` per `data-density`-Attribut umschaltbar (Tweaks-Panel oder Settings)
- [ ] **Akzentfarben-Presets** — 5 auswählbare Akzentfarben (Grün, Indigo, Orange, Rot, Gelb) als OKLCH-basierte CSS-Variablen

---

## Chrome / App-Shell

- [ ] **Wordmark** — Custom SVG-Logo `relexx_tmpl` mit Akzent-Farbe und animiertem Cursor statt plain-text `<h1>`
- [ ] **Stepper `is-done` State** — Besuchte Schritte erhalten einen "erledigt"-Stil; Navigation nur bis zum bisher maximal erreichten Schritt (`maxReached`)
- [ ] **Keyboard-Navigation** — `←` / `→` navigiert zwischen Stages (wenn kein Input fokussiert)

---

## Stage-Layout (Rahmen, gilt für alle Phasen)

- [ ] **`page-head`-Bereich** — Jede Stage bekommt einen eigenen Header-Bereich mit `<h1>`, Untertitel-Beschreibung (`page-sub`) und `page-head-actions` rechts
- [ ] **Sekundäre Toolbar-Buttons nach `page-head-actions`** — Neu/Duplizieren/Export/Import wandern in den Stage-Header; die Toolbar-Leiste wird schlanker
- [ ] **`Panel`-Komponente** — Inhaltsbereiche als Karten mit eigenem Panel-Header (Titel + Icon + Badge + optionale Actions) und Panel-Body
- [ ] **`Field`-Komponente** — Formularfelder in Field-Container mit Label + optionalem Hint-Text darunter
- [ ] **`Badge`-System** — Kleine Info-Chips an Panel-Titeln (z. B. Block-Anzahl, „Pflicht", „Markdown", Zeichenzahl)
- [ ] **`divider`-Element** — Visuelle Trennlinie (`<div class="divider">`) zwischen Toolbar-Sektionen

---

## Phase 0 – Layout

- [ ] **Toolbar-Metadaten** — Toolbar zeigt: Anzahl Blöcke + „zuletzt geändert vor X Min."
- [ ] **Delete-Button in Toolbar** — Löschen-Button rechts in der Toolbar (nicht im Block-Editor)
- [ ] **Block-ID-Badge im Panel-Header** — Aktuelle Block-ID als Badge neben „Block bearbeiten"
- [ ] **Feldanordnung im Block-Editor** — Reihenfolge: ID → Typ → Icon → Überschrift → ShowTitle-Checkbox → typ-spezifische Optionen

---

## Phase 1 – Eingabe

- [ ] **`content-grid` Layout** — Zweispaltiges CSS-Grid für die Eingabefelder (statt einspaltige `form-grid`)
- [ ] **Image-Dropzone** — Bild-Block bekommt Drag-and-Drop-Zone (`<div class="dropzone">`) zusätzlich zu URL-Input + Kamera-Button
- [ ] **Textarea-Hints** — Formathinweise direkt am Textarea-Label (z. B. „**fett** · *kursiv*", „Eigenschaft | Wert")

---

## Phase 2 – Vorschau

- [ ] **Desktop/Mobile-Toggle** — Zwei Buttons zum Wechsel zwischen Desktop-Breite (760 px) und Mobile-Breite (380 px), mit CSS-Transition
- [ ] **`<iframe srcDoc>`-Rendering** — Vorschau in isoliertem iframe statt direktem `@(MarkupString)`, sodass Stile des generierten HTML nicht in die App-Shell durchschlagen

---

## Phase 3 – HTML

- [ ] **Datei-Stats im Header** — Zeilen-Anzahl + Dateigröße (kB) als Tag-Chips in `page-head-actions`
- [ ] **Syntax-Highlighting** — HTML-Code-Ansicht mit farbiger Hervorhebung (Tags, Attribute, Strings, Kommentare)
- [ ] **UTF-8 / MIME-Hinweis** — Metainfo „UTF-8 · text/html" in der Code-Toolbar

---

## Generiertes eBay-HTML

- [ ] **SVG-Icons in Abschnitts-Titeln** — Emoji-Icons durch Inline-SVG (`<svg>`) ersetzen, Farbe = Akzentfarbe
- [ ] **Geist-Font-Stack im Output** — Generiertes HTML referenziert `'Geist', -apple-system, system-ui, sans-serif`
- [ ] **Wrapper-`<div>` statt Table-Root** — Äußerer Container als `<div style="max-width:760px; margin:0 auto;">` statt reiner Table-Struktur

---

## Notizen

- Der `FixedText`-Block-Typ und die Fußzeile (`Footer`-Sektion) sind im Mockup nicht abgebildet — Klären, ob diese entfernt oder nur ausgeblendet werden sollen.
- Das Tweaks-Panel ist ein Prototyping-Tool des Mockups und wird nicht 1:1 übernommen; Dichte und Akzentfarbe werden stattdessen in die App-Settings integriert.
