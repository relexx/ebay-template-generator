# relexx' Template Generator

Eine Blazor WebAssembly Anwendung zur Erstellung von professionellen, eBay-konformen HTML-Templates für Artikelbeschreibungen.

## Features

- **3-Phasen-Wizard**: Eingabe → Vorschau → HTML-Export
- **Markdown-Unterstützung** in der Beschreibung
- **Base64-Bildkonvertierung** für eingebettete Bilder
- **Farbschema-Anpassung** mit intuitiven Color Pickern
- **LocalStorage-Autosave** alle 10 Sekunden
- **Import/Export** als JSON-Datei
- **Demo-Daten** zum schnellen Testen
- **Mobile-optimiert** und eBay-konform

## Voraussetzungen

- .NET 8 SDK
- Visual Studio 2022 / VS Code / Rider

## Lokale Entwicklung

```bash
# Repository klonen
cd ebay-template-generator

# Abhängigkeiten wiederherstellen
dotnet restore

# Anwendung starten
dotnet run

# Oder mit Hot Reload
dotnet watch run
```

Die Anwendung ist dann unter `https://localhost:5001` erreichbar.

## Deployment auf Azure Static Web Apps

### Option 1: Via GitHub Actions (empfohlen)

1. Erstelle ein GitHub Repository und pushe den Code
2. Erstelle eine neue Azure Static Web App im Azure Portal
3. Wähle "GitHub" als Deployment-Quelle
4. Konfiguriere:
   - **App location**: `/`
   - **Api location**: (leer lassen)
   - **Output location**: `wwwroot`
5. Azure erstellt automatisch einen GitHub Action Workflow

### Option 2: Via Azure CLI

```bash
# Azure Static Web App CLI installieren
npm install -g @azure/static-web-apps-cli

# Build erstellen
dotnet publish -c Release -o publish

# Lokal testen
swa start publish/wwwroot

# Deployment
swa deploy publish/wwwroot --env production
```

### Option 3: Manueller Build

```bash
# Release Build
dotnet publish -c Release -o publish

# Der Output ist in: publish/wwwroot/
# Diesen Ordner als Static Web App deployen
```

## GitHub Actions Workflow

Erstelle `.github/workflows/azure-static-web-apps.yml`:

```yaml
name: Azure Static Web Apps CI/CD

on:
  push:
    branches:
      - main
  pull_request:
    types: [opened, synchronize, reopened, closed]
    branches:
      - main

jobs:
  build_and_deploy_job:
    if: github.event_name == 'push' || (github.event_name == 'pull_request' && github.event.action != 'closed')
    runs-on: ubuntu-latest
    name: Build and Deploy Job
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
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
          action: "upload"
          app_location: "publish/wwwroot"
          skip_app_build: true

  close_pull_request_job:
    if: github.event_name == 'pull_request' && github.event.action == 'closed'
    runs-on: ubuntu-latest
    name: Close Pull Request Job
    steps:
      - name: Close Pull Request
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          action: "close"
```

## Projektstruktur

```
ebay-template-generator/
├── Models/
│   └── TemplateData.cs          # Datenmodell für Template-Felder
├── Pages/
│   └── Index.razor              # Haupt-Seite mit 3-Phasen-Wizard
├── Services/
│   └── TemplateGeneratorService.cs  # HTML-Generierung
├── wwwroot/
│   ├── css/
│   │   └── app.css              # Styling
│   ├── index.html               # Einstiegspunkt
│   └── staticwebapp.config.json # Azure SWA Konfiguration
├── App.razor                    # Root-Komponente
├── _Imports.razor               # Globale Usings
├── Program.cs                   # Service-Registrierung
└── EbayTemplateGenerator.csproj # Projektdatei
```

## Eingabeformat

### Highlights
```
Titel | Beschreibung
Titel2 | Beschreibung2
```

### Technische Daten
```
Spezifikation | Wert
Spezifikation2 | Wert2
```

### Kompatibilität
```
Intel | LGA 1851 │ LGA 1700
AMD | AM5 │ AM4
```

### Lieferumfang
```
Artikel 1
Artikel 2
Artikel 3
```

## Lizenz

MIT License
