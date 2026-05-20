namespace EbayTemplateGenerator.Models;

/// <summary>
/// Verfügbare Block-Typen für das Layout
/// </summary>
public enum BlockType
{
    /// <summary>Einzelbild (zentriert)</summary>
    Image,
    
    /// <summary>Markdown-Fließtext</summary>
    RichText,
    
    /// <summary>Stichwort-Karten im Grid: Titel | Beschreibung</summary>
    KeyValueGrid,
    
    /// <summary>Datentabelle: Key | Value mit Spaltenüberschriften</summary>
    DataTable,
    
    /// <summary>Feature-Karten: Farbige Boxen nebeneinander</summary>
    FeatureCards,
    
    /// <summary>Aufzählungsliste mit Häkchen/Punkten</summary>
    CheckList,

    /// <summary>Fester Text aus dem Layout (Markdown, mit Standardwert)</summary>
    FixedText
}

/// <summary>
/// Definition eines einzelnen Blocks im Layout
/// </summary>
public class BlockDefinition
{
    public string Id { get; set; } = Helpers.GenerateShortId();
    public BlockType Type { get; set; }
    public string Icon { get; set; } = "📄";
    public string Title { get; set; } = "Neuer Block";
    public int Order { get; set; }
    public BlockOptions Options { get; set; } = new();
    
    /// <summary>
    /// Erstellt eine Kopie mit neuer ID
    /// </summary>
    public BlockDefinition Clone() => new()
    {
        Id = Helpers.GenerateShortId(),
        Type = Type,
        Icon = Icon,
        Title = Title,
        Order = Order,
        Options = Options.Clone()
    };
    
    /// <summary>
    /// Erstellt einen Standard-Block für einen Typ
    /// </summary>
    public static BlockDefinition CreateDefault(BlockType type, int order = 0) => new()
    {
        Type = type,
        Icon = type.GetDefaultIcon(),
        Title = type.GetDefaultTitle(),
        Order = order,
        Options = BlockOptions.CreateDefault(type)
    };
}

/// <summary>
/// Typ-spezifische Optionen für Blöcke
/// </summary>
public class BlockOptions
{
    // === Gemeinsame Optionen ===
    public bool ShowTitle { get; set; } = true;
    public string BulletChar { get; set; } = "✓";
    public bool AlternatingBackground { get; set; } = true;
    
    // === DataTable Optionen ===
    public string Column1Header { get; set; } = "Eigenschaft";
    public string Column2Header { get; set; } = "Wert";
    public bool ShowColumnHeaders { get; set; } = true;
    
    // === KeyValueGrid Optionen ===
    public int Columns { get; set; } = 2;
    
    // === Image Optionen ===
    public string Alignment { get; set; } = "center";
    public int MaxWidth { get; set; } = 600;

    // === FixedText Optionen ===
    public string FixedContent { get; set; } = string.Empty;
    
    public BlockOptions Clone() => new()
    {
        ShowTitle = ShowTitle,
        BulletChar = BulletChar,
        AlternatingBackground = AlternatingBackground,
        Column1Header = Column1Header,
        Column2Header = Column2Header,
        ShowColumnHeaders = ShowColumnHeaders,
        Columns = Columns,
        Alignment = Alignment,
        MaxWidth = MaxWidth,
        FixedContent = FixedContent
    };
    
    public static BlockOptions CreateDefault(BlockType type) => type switch
    {
        BlockType.Image => new() { Alignment = "center", MaxWidth = 600 },
        BlockType.RichText => new(),
        BlockType.KeyValueGrid => new() { BulletChar = "▸", Columns = 2 },
        BlockType.DataTable => new() 
        { 
            Column1Header = "Spezifikation", 
            Column2Header = "Wert",
            ShowColumnHeaders = true,
            AlternatingBackground = true
        },
        BlockType.FeatureCards => new(),
        BlockType.CheckList => new() { BulletChar = "✓" },
        BlockType.FixedText => new() { FixedContent = "**Hinweis:** Standardtext hier eingeben..." },
        _ => new()
    };
}

/// <summary>
/// Hilfsmethoden für BlockType — alle Metadaten zentral in einer Tabelle.
/// Neuen BlockType hinzufügen: Enum-Wert + eine Zeile in Meta.
/// </summary>
public static class BlockTypeExtensions
{
    private record BlockTypeMeta(
        string  Icon,
        string  DefaultTitle,
        string  DisplayName,
        string  InputLabel,
        string  InputPlaceholder,
        string? InputHint,
        int     TextareaRows,
        string  DemoContent
    );

    private static readonly Dictionary<BlockType, BlockTypeMeta> Meta = new()
    {
        [BlockType.Image] = new(
            Icon:             "image",
            DefaultTitle:     "Produktbild",
            DisplayName:      "Bild",
            InputLabel:       "Bild-URL oder Base64",
            InputPlaceholder: "https://beispiel.de/bild.jpg",
            InputHint:        null,
            TextareaRows:     4,
            DemoContent:      "https://placehold.co/600x400/1a1a1a/f5c518?text=Produktbild"
        ),
        [BlockType.RichText] = new(
            Icon:             "fileText",
            DefaultTitle:     "Beschreibung",
            DisplayName:      "Fließtext",
            InputLabel:       "Text (Markdown unterstützt)",
            InputPlaceholder: "**Fett** und *kursiv* werden unterstützt...",
            InputHint:        "**fett** · *kursiv* · [Link](url)",
            TextareaRows:     6,
            DemoContent:      "**Das Produkt** bietet herausragende Qualität und durchdachte Features.\n\nDie hochwertige Verarbeitung garantiert langlebige Zuverlässigkeit für den täglichen Einsatz."
        ),
        [BlockType.KeyValueGrid] = new(
            Icon:             "sparkles",
            DefaultTitle:     "Highlights",
            DisplayName:      "Stichwort-Karten",
            InputLabel:       "Pro Zeile: Titel | Beschreibung",
            InputPlaceholder: "Titel | Beschreibung\nTitel 2 | Beschreibung 2",
            InputHint:        "Titel | Beschreibung",
            TextareaRows:     7,
            DemoContent:      "Premium Qualität | Erstklassige Materialien und Verarbeitung\nInnovatives Design | Moderne Optik trifft Funktionalität\nEinfache Bedienung | Intuitive Handhabung für jeden\nLanglebigkeit | Robust und zuverlässig"
        ),
        [BlockType.DataTable] = new(
            Icon:             "barChart",
            DefaultTitle:     "Technische Daten",
            DisplayName:      "Datentabelle",
            InputLabel:       "Pro Zeile: Eigenschaft | Wert",
            InputPlaceholder: "Eigenschaft | Wert\nEigenschaft 2 | Wert 2",
            InputHint:        "Eigenschaft | Wert",
            TextareaRows:     8,
            DemoContent:      "Material | Aluminium / Kunststoff\nAbmessungen | 250 × 150 × 80 mm\nGewicht | 450 g\nFarbe | Schwarz\nAnschlüsse | USB-C, HDMI"
        ),
        [BlockType.FeatureCards] = new(
            Icon:             "target",
            DefaultTitle:     "Kompatibilität",
            DisplayName:      "Feature-Karten",
            InputLabel:       "Pro Zeile: Kategorie | Optionen",
            InputPlaceholder: "Kategorie | Option 1, Option 2\nKategorie 2 | Option 3",
            InputHint:        "Kategorie | Option 1, Option 2",
            TextareaRows:     3,
            DemoContent:      "Typ A | Standard, Premium, Pro\nTyp B | Basic, Advanced"
        ),
        [BlockType.CheckList] = new(
            Icon:             "box",
            DefaultTitle:     "Lieferumfang",
            DisplayName:      "Aufzählungsliste",
            InputLabel:       "Pro Zeile: Ein Eintrag",
            InputPlaceholder: "Artikel 1\nArtikel 2\nArtikel 3",
            InputHint:        null,
            TextareaRows:     5,
            DemoContent:      "1× Hauptgerät\n1× USB-C Kabel\n1× Kurzanleitung\n1× Garantiekarte"
        ),
        [BlockType.FixedText] = new(
            Icon:             "pin",
            DefaultTitle:     "Hinweis",
            DisplayName:      "Fester Text",
            InputLabel:       "Fester Text (Markdown, aus Layout vorbelegt)",
            InputPlaceholder: "Text aus Layout-Vorlage (editierbar)",
            InputHint:        null,
            TextareaRows:     4,
            DemoContent:      "**Hinweis:** Dies ist ein fester Textbaustein aus dem Layout."
        ),
    };

    public static string  GetDefaultIcon(this BlockType t)      => Meta[t].Icon;
    public static string  GetDefaultTitle(this BlockType t)     => Meta[t].DefaultTitle;
    public static string  GetDisplayName(this BlockType t)      => Meta[t].DisplayName;
    public static string  GetInputLabel(this BlockType t)       => Meta[t].InputLabel;
    public static string  GetInputPlaceholder(this BlockType t) => Meta[t].InputPlaceholder;
    public static string? GetInputHint(this BlockType t)        => Meta[t].InputHint;
    public static int     GetTextareaRows(this BlockType t)     => Meta[t].TextareaRows;
    public static string  GetDemoContent(this BlockType t)      => Meta[t].DemoContent;
}
