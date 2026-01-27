# relexx' Template Generator

Eine moderne Blazor WebAssembly Anwendung zur Erstellung professioneller, eBay-konformer HTML-Templates für Artikelbeschreibungen – mit modularem Block-System und Drag & Drop Editor.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Blazor WASM](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)
![License](https://img.shields.io/badge/License-MIT-green)

---

## ✨ Features

### Modulares Block-System
- **6 Block-Typen**: Bild, Fließtext (Markdown), Stichwort-Karten, Datentabelle, Feature-Karten, Aufzählungsliste
- **Drag & Drop**: Blöcke per SortableJS frei anordnen
- **Typ-spezifische Optionen**: Spaltenanzahl, Aufzählungszeichen, Tabellenheader, Bildausrichtung u.v.m.
- **Dynamische Layouts**: Beliebig viele Blöcke hinzufügen, bearbeiten oder entfernen

### 4-Phasen-Workflow
| Phase            | Beschreibung                                                           |
| ---------------- | ---------------------------------------------------------------------- |
| **0 – Layout**   | Layout erstellen/bearbeiten, Blöcke konfigurieren, Farbschema anpassen |
| **1 – Eingabe**  | Artikeldaten erfassen, Bilder hochladen, Markdown-Texte schreiben      |
| **2 – Vorschau** | Live-Vorschau des generierten HTML-Templates                           |
| **3 – HTML**     | Fertigen HTML-Code kopieren oder herunterladen                         |

### Layout-Management
- **Standard-Layout**: Vorkonfiguriertes Template mit 6 typischen eBay-Blöcken
- **Eigene Layouts**: Erstellen, duplizieren, importieren, exportieren
- **Farbschema**: Primär-, Akzent- und Hintergrundfarbe anpassbar
- **Portabilität**: Layouts werden in exportierte Artikel eingebettet

### Weitere Features
- **Markdown-Support**: Fett, kursiv und weitere Formatierungen in Fließtext-Blöcken
- **Base64-Bilder**: Lokale Bilder werden automatisch eingebettet (max. 5 MB)
- **LocalStorage**: Automatische Speicherung aller Daten im Browser
- **JSON Import/Export**: Artikel und Layouts als JSON-Dateien sichern
- **JSON-Schema**: Generiertes Schema für KI-gestützte Artikelerstellung
- **Demo-Daten**: Ein Klick füllt alle Felder mit Beispieldaten
- **Mobile-optimiert**: Responsive Design, schema.org Mobile Summary
- **Konfliktauflösung**: Dialog bei Layout-Unterschieden beim Import

---

## 🚀 Schnellstart

### Voraussetzungen
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- IDE: Visual Studio 2022, VS Code oder JetBrains Rider

### Installation & Start

```bash
# In das Projektverzeichnis wechseln
cd ebay-template-generator

# Abhängigkeiten wiederherstellen
dotnet restore

# Anwendung starten
dotnet run

# Oder mit Hot Reload für Entwicklung
dotnet watch run
```

Die Anwendung ist unter `https://localhost:5001` oder `http://localhost:5000` erreichbar.

---

## 📁 Projektstruktur

```
ebay-template-generator/
├── Constants.cs                    # Zentrale Konstanten (Storage Keys, Limits, Icons)
├── Helpers.cs                      # Gemeinsame Hilfsmethoden (ID-Generierung, JSON)
├── Program.cs                      # Service-Registrierung & App-Konfiguration
├── App.razor                       # Root-Komponente
├── _Imports.razor                  # Globale Using-Direktiven
│
├── Models/
│   ├── ArticleData.cs              # Artikeldaten mit eingebettetem Layout
│   ├── BlockDefinition.cs          # Block-Typen, Optionen & Extensions
│   ├── ColorScheme.cs              # Farbschema-Modell
│   └── LayoutTemplate.cs           # Layout mit Block-Sammlung
│
├── Pages/
│   └── Index.razor                 # Haupt-UI mit 4-Phasen-Workflow
│
├── Services/
│   └── TemplateGeneratorService.cs # HTML-Generierung aus Artikeldaten
│
└── wwwroot/
    ├── css/app.css                 # Dark Theme Styling
    ├── index.html                  # HTML-Einstiegspunkt + JS-Interop
    └── staticwebapp.config.json    # Azure Static Web Apps Konfiguration
```

---

## 📝 Block-Typen im Detail

### 🖼️ Bild (Image)
- URL oder Base64-kodiertes Bild
- Optionen: Ausrichtung (links/zentriert/rechts), maximale Breite

### 📝 Fließtext (RichText)
- Markdown-Unterstützung (`**fett**`, `*kursiv*`)
- Mehrzeilige Absätze

### ✦ Stichwort-Karten (KeyValueGrid)
- Format: `Titel | Beschreibung` pro Zeile
- Optionen: 1–3 Spalten, Aufzählungszeichen

### ⚙ Datentabelle (DataTable)
- Format: `Eigenschaft | Wert` pro Zeile
- Optionen: Spaltenüberschriften, Zebra-Streifen

### 🔧 Feature-Karten (FeatureCards)
- Format: `Kategorie | Option1, Option2` pro Zeile
- Farbige Karten nebeneinander

### 📦 Aufzählungsliste (CheckList)
- Ein Eintrag pro Zeile
- Optionen: Aufzählungszeichen (✓, •, ▸, ★, etc.)

---

## 🎨 Farbschema

| Farbe       | Standard  | Verwendung                         |
| ----------- | --------- | ---------------------------------- |
| Primär      | `#1a1a1a` | Header, Footer, Tabellenkopf       |
| Akzent      | `#f5c518` | Überschriften, Highlights, Bullets |
| Hintergrund | `#f8f9fa` | Alternating Sections               |

---

## 💾 Datenformate

### Artikel-Export (JSON)
```json
{
  "Title": "Produktname",
  "Subtitle": "Kurzbeschreibung │ Art.-Nr.",
  "MobileSummary": "Zusammenfassung für Mobile...",
  "Footer": "SKU: ABC123\nFarbe: Schwarz",
  "BlockContents": {
    "img": "https://...",
    "desc": "**Markdown** Text...",
    "highlights": "Feature | Beschreibung\n..."
  },
  "Layout": { /* Eingebettetes Layout */ }
}
```

### Layout-Export (JSON)
```json
{
  "Id": "abc12345",
  "Name": "Mein Layout",
  "IsDefault": false,
  "Colors": {
    "PrimaryColor": "#1a1a1a",
    "AccentColor": "#f5c518",
    "BackgroundColor": "#f8f9fa"
  },
  "Blocks": [
    { "Id": "img", "Type": "Image", "Title": "Produktbild", "Order": 0, ... }
  ]
}
```

---

## ☁️ Deployment auf Azure Static Web Apps

### Option 1: GitHub Actions (empfohlen)

1. Repository auf GitHub erstellen und Code pushen
2. Im Azure Portal eine neue **Static Web App** erstellen
3. GitHub als Deployment-Quelle wählen
4. Build-Konfiguration:
   - **App location**: `/`
   - **Output location**: `wwwroot`

### Option 2: Azure CLI

```bash
# Static Web Apps CLI installieren
npm install -g @azure/static-web-apps-cli

# Release Build erstellen
dotnet publish -c Release -o publish

# Lokal testen
swa start publish/wwwroot

# Deployment
swa deploy publish/wwwroot --env production
```

### GitHub Actions Workflow

Erstelle `.github/workflows/azure-swa.yml`:

```yaml
name: Deploy to Azure Static Web Apps

on:
  push:
    branches: [main]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET 8
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
          
      - name: Publish
        run: dotnet publish -c Release -o publish
        
      - name: Deploy
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: upload
          app_location: publish/wwwroot
          skip_app_build: true
```

---

## 🛠️ Technologie-Stack

| Komponente  | Technologie                 |
| ----------- | --------------------------- |
| Frontend    | Blazor WebAssembly (.NET 8) |
| Styling     | Custom CSS (Dark Theme)     |
| Markdown    | Markdig                     |
| Storage     | Blazored.LocalStorage       |
| Drag & Drop | SortableJS                  |
| Hosting     | Azure Static Web Apps       |

---

## 📋 Tastenkürzel & Tipps

- **Demo laden**: Füllt alle Felder mit Beispieldaten passend zum aktuellen Layout
- **JSON-Schema**: Generiert ein Schema für KI-Tools (ChatGPT, Claude) zur automatischen Artikelerstellung
- **Import mit Konflikt**: Bei unterschiedlichen Layouts kannst du wählen:
  - *Layout übernehmen*: Importiertes Layout wird hinzugefügt
  - *Daten einpassen*: Nur kompatible Felder werden übernommen

---

## 🤝 Mitwirken

1. Fork erstellen
2. Feature-Branch anlegen (`git checkout -b feature/neues-feature`)
3. Änderungen committen (`git commit -m 'Neues Feature hinzugefügt'`)
4. Branch pushen (`git push origin feature/neues-feature`)
5. Pull Request erstellen

---

## 📄 Lizenz

MIT License – siehe [LICENSE](LICENSE.md) für Details.

---

## 🙏 Danksagungen

- [Markdig](https://github.com/xoofx/markdig) – Markdown-Parser
- [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage) – LocalStorage-Abstraktion
- [SortableJS](https://sortablejs.github.io/Sortable/) – Drag & Drop Bibliothek