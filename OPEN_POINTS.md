# Open Points

Punkte, die absichtlich zurückgestellt wurden. Keine Priorisierung.

---

## Undo / Redo (Ctrl+Z / Ctrl+Y)

**Was:** Schrittweises Rückgängigmachen von Layout-Änderungen und Eingabe-Änderungen.

**Warum zurückgestellt:**
- Erfordert einen vollständigen Command-Stack oder State-Snapshot-Ansatz.
- In einer Blazor-Komponente, bei der State in Feldern lebt, muss nach jedem
  mutierbaren Aufruf ein Snapshot erzeugt und vorgehalten werden.
- Layoutänderungen (Drag & Drop, Block-Optionen, ID-Umbenennung) und Artikelinhalte
  haben unterschiedliche Granularitäten — ein naiver Ansatz würde zu
  inkonsistenten Undo-Grenzen führen.
- **Empfohlener Ansatz:** `IMemento`-Pattern mit separaten Stacks für Layout und
  Article; Serialisierung per `JsonSerializer`; Max-Tiefe ~20 Schritte.
  LocalStorage-Persistenz des Undo-Stacks ist nicht nötig (nur im RAM).

---

## Batch-Export (mehrere Artikel als ZIP)

**Was:** Alle gespeicherten Artikel auf einmal als ZIP-Archiv herunterladen.

**Warum zurückgestellt:**
- Blazor WASM hat keine native ZIP-Unterstützung im Browser.
- Optionen:
  a) **`System.IO.Compression.ZipArchive`** — Verfügbar in .NET WASM,
     erzeugt ein `byte[]`; per `downloadFile`-JS-Helper herunterladen.
     Größtes Risiko: sehr große Base64-Bilder können RAM-Limits treffen.
  b) **JSZip (CDN)** — flexibler, kein .NET-Code nötig, aber externe Abhängigkeit.
- **Empfohlener Ansatz:** `ZipArchive` in C# (kein extra NuGet nötig).
  Jeder gespeicherte Artikel → `[Name]_[Datum].json` im ZIP.

---

## Weitere Backlog-Items (aus IDEAS.md)

| Feature | Kurzbeschreibung |
|---|---|
| Clipboard Smart-Paste | Eingefügten Text als Markdown / Key-Value-Paare erkennen |
| Vorlagen-Bibliothek | Vorgefertigte Layouts für Elektronik, Kleidung, Werkzeug usw. |
| Mehrsprachig | DE/EN/FR mit konfigurierbaren UI-Labels exportieren |
| PDF-Preview | `window.print()` + `@media print` für A4-Druck |
| Artikelversionierung | Letzte 5 Stände pro Artikel in LocalStorage |
| eBay-Kategorie-Presets | Schlägt Block-Kombination vor (z. B. Laptop → Image + Specs + Compat) |
| Responsive Viewport-Preview | Frei einstellbare Breite statt nur Desktop/Mobile |
| Spaltenbreite Sidebar (mobile) | Derzeit nur auf Desktop verfügbar |
| Block duplizieren | Direktes Klonen eines Blocks inkl. Inhalten |
| Block-Suche / Filter | Suche im Block-Editor bei vielen Blöcken |
