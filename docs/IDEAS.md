# Ideas & Backlog

Gesammelte Ideen aus Brainstorming-Sessions. Keine Priorisierung, kein Versprechen — reine Ideenliste.

---

## Animationen

- ✅ **Phasenwechsel** — Slide-left/right zwischen den Phasen (CSS `translate` + `opacity`, je nach Navigationsrichtung)
- ✅ **Block hinzufügen/entfernen** — `slide-down` + `fade-in` beim Einfügen, `slide-up` + `fade-out` beim Löschen
- ✅ **Modal öffnen** — `scale(0.95) → scale(1)` + `opacity` statt sofortigem Erscheinen
- ✅ **Notification** — Slide-in von unten rechts, automatisches Slide-out nach Timeout
- ✅ **Step-Connector** — Breite animiert von 0 → 100 % beim Abschließen einer Phase
- ✅ **Drag & Drop** — Drop-Zone pulsiert beim Hovern über gültiger Zielposition
- ✅ **Farbwähler-Swatch** — Kurzer `pop`-Effekt beim Aktivieren
- ✅ **Preview-Iframe** — Fade-in nach dem Laden statt harter Erscheinung
- ✅ **Toolbar-Meta Counter** — `count-up`-Animation beim Ändern der Blockanzahl
- ✅ **Wortmarke-Animation** — Alle 10-20 Sekunden wird "relexx_tmpl" wie in einer Konsole neu geschrieben
- ✅ **Animationen ausschalten** — Toggle in den Einstellungen, um Animationen zu aktivieren/deaktivieren. Standard ist AN.

---

## Block-Varianten

> eBay-Kompatibilität recherchiert (Stand Mai 2026).
> Alles muss mit reinem HTML + Inline-CSS auskommen — kein JavaScript, keine `<iframe>`, keine `<form>`-Elemente.

### ✅ Voll umsetzbar

| Block | Beschreibung | Notizen |
|---|---|---|
| **Pros & Cons** | Zweispaltige Tabelle mit ✓ Vorteile / ✗ Nachteile | Akzentfarbe für Spalten-Header |
| **Warn-/Hinweisbox** | Farbige Callout-Box (Info, Warnung, Tipp) mit Icon-Präfix | Varianten: blau / gelb / rot / grün |
| **Badge-Streifen** | Horizontale Flex-Reihe aus Icon+Text-Badges (Garantie, Zertifikate…) | Inline-Flexbox, kein JS nötig |
| **Bewertungs-Snippet** | Statische Sternebewertung (★) + Kurztext | Rein dekorativ, keine Interaktivität |
| **Bildergalerie** | Statisches CSS-Grid aus `<img>`-Tags | Kein Lightbox/Zoom (braucht JS) |
| **Downloads / Links** | Klickbare Linkliste (Datenblatt, Anleitung…) | `target="_blank"` ist bei eBay Pflicht |
| **Banner / Hero** | Vollbreites Bild mit überlagertem Titeltext | Absolute Positionierung via Inline-CSS |

### ⚠️ Eingeschränkt umsetzbar

| Block | Einschränkung | Empfehlung |
|---|---|---|
| **Maße / Abmessungen** | Kein interaktives Diagramm | Tabellarisch oder inline-`<svg>` |
| **FAQ (statisch)** | Echtes Accordion braucht JS oder `<input>` (geblockt) | Alle Fragen/Antworten aufgeklappt darstellen |
| **Video** | Nur selbst gehostetes MP4 via `<video>`; YouTube/Vimeo per `<iframe>` geblockt | Thumbnail + ▶-Overlay als klickbares Bild mit Link |

### ❌ Nicht umsetzbar auf eBay

| Block | Grund |
|---|---|
| **Video-Embed (YouTube/Vimeo)** | `<iframe>` ist geblockt, kein Workaround |
| **Interaktive Tabs** | Benötigt JS; CSS-only-Trick via `:checked` funktioniert nicht, da `<input>` geblockt |
| **Countdown-Timer** | JavaScript |
| **Produktempfehlungen (dynamisch)** | JavaScript + externe API |

---

## Icons (Lucide-Erweiterung)

> ✅ Umgesetzt: Kategorisierter Icon-Picker mit 6 Gruppen in `Constants.IconCategories` (Stand 2026-05-21)

Ursprüngliche Vorschläge, die in `Constants.AvailableIcons` aufgenommen wurden:

**Versand & Logistik**
`truck` · `package2` · `mapPin` · `clock` · `calendar`

**Qualität & Service**
`shield` · `award` · `star` · `zap` · `checkCircle`

**Technik & Produkt**
`cpu` · `wifi` · `battery` · `camera` · `headphones` · `monitor` · `printer` · `wrench` · `scissors` · `toolbox` · `hammer` · `ruler` · `weight`

**Kommunikation**
`globe` · `phone` · `mail` · `link`

**Kategorisierung**
`tags` · `grid` · `list`

**Weitere UI**
`alertTriangle` · `info` · `eye` · `eyeOff` · `xCircle`

✅ **Kategorisierung im Icon-Picker** ist bereits implementiert (Abschnitte statt flacher Liste).

---

## Weitere Features (Priorisiert)

### Editing

- **Undo / Redo** (Ctrl+Z / Ctrl+Y) — für Layout-Änderungen und Eingabe-Phase
- **Schema pasten** — JSON aus Text in der Zwischenablage direkt laden (quasi wie Import, nur ohne Datei-Zwischenschritt)
- **Speichern von Artikeln** — Möglichkeit, auch Artikel im LocalStorage zu speichern, sodass man sie später z.B. als Vorlage für weitere Artikel wieder abrufen kann.

### Vorschau & Export

- **Bild-Optimierung** — Hinweis bei zu großen Base64-Bildern, Vorschlag zum Verkleinern
- **Batch-Export** — mehrere gespeicherte Artikel auf einmal als ZIP exportieren

### Workflow

- **Keyboard-Shortcut-Overlay** (? oder F1) — alle verfügbaren Shortcuts in einem Modal

### Qualität & UX

- **Auto-Validierung** — warnt vor leerem Pflichtfeld (Titel) bevor Phase 1 → 2 möglich ist
- **HTML-Größenindikator mit Grenze** — Warnung ab z.B. 500 kB bzw. 500000 Zeichen (eBay-Limit)
- **Spaltenbreite Sidebar anpassbar** — Drag-Resize des `.layout-split`-Grids

---

## Weitere Features (Backlog)

### Editing
- **Block duplizieren** — direkt im Block-Editor, inkl. Inhaltsübernahme
- **Clipboard Smart-Paste** — eingefügten Text automatisch als Markdown / Key-Value-Paare erkennen und formatieren
- **Vorlagen-Bibliothek** — vorgefertigte Layouts als Startpunkt (Electronics, Kleidung, Werkzeug, …)
- **Block-Suche / Filter** im Block-Editor bei vielen Blöcken

### Vorschau & Export

- **Mehrsprachig** — gleiche Inhalte in DE/EN/FR exportieren mit konfigurierbaren UI-Labels
- **PDF-Preview** — druckbares A4-PDF des Listings via `window.print()` + `@media print`
- **Dunkle/Helle Template-Vorschau** — Toggle für `prefers-color-scheme` des eBay-Käufers

### Workflow

- **Artikelversionierung** — letzte 5 Stände eines Artikels in LocalStorage sichern, wiederherstellbar
- **eBay-Kategorie-Presets** — schlägt passende Block-Kombination vor (z.B. Laptop → Image + Specs + Compat + Scope)

### Qualität & UX

- **Responsive Template-Preview-Breakpoints** — frei einstellbare Viewport-Breite statt nur Desktop/Mobile

---

## eBay HTML — Kurzreferenz

> Recherche-Ergebnis aus Mai 2026 für eigene Blöcke relevant.

| Was | Status |
|---|---|
| Inline-CSS (`style="..."`) | ✅ Zuverlässig |
| `<style>`-Block im Body | ✅ Funktioniert in den meisten Listern |
| Strukturelles HTML (`div`, `table`, `ul`, `p`…) | ✅ |
| `<img>` mit HTTPS-URL | ✅ |
| `<a href="..." target="_blank">` | ✅ `target="_blank"` Pflicht |
| CSS-Animationen / Transitions | ✅ |
| Flexbox / Grid via Inline-CSS | ✅ (moderne Browser) |
| Externe CSS-Dateien (`<link>`) | ⚠️ Unzuverlässig, lieber vermeiden |
| `<video>` (HTML5, selbst gehostet) | ⚠️ Erlaubt, aber nur eigener Server |
| JavaScript (alle Formen) | ❌ |
| `<iframe>`, `<embed>`, `<object>` | ❌ |
| `<form>`, `<input>`, `<button>`, `<select>` | ❌ |
