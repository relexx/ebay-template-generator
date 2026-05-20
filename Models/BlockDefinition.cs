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
/// Hilfsmethoden für BlockType
/// </summary>
public static class BlockTypeExtensions
{
    public static string GetDefaultIcon(this BlockType type) => type switch
    {
        BlockType.Image => "image",
        BlockType.RichText => "fileText",
        BlockType.KeyValueGrid => "sparkles",
        BlockType.DataTable => "barChart",
        BlockType.FeatureCards => "target",
        BlockType.CheckList => "box",
        BlockType.FixedText => "pin",
        _ => "blocks"
    };
    
    public static string GetDefaultTitle(this BlockType type) => type switch
    {
        BlockType.Image => "Produktbild",
        BlockType.RichText => "Beschreibung",
        BlockType.KeyValueGrid => "Highlights",
        BlockType.DataTable => "Technische Daten",
        BlockType.FeatureCards => "Kompatibilität",
        BlockType.CheckList => "Lieferumfang",
        BlockType.FixedText => "Hinweis",
        _ => "Block"
    };
    
    public static string GetDisplayName(this BlockType type) => type switch
    {
        BlockType.Image => "Bild",
        BlockType.RichText => "Fließtext",
        BlockType.KeyValueGrid => "Stichwort-Karten",
        BlockType.DataTable => "Datentabelle",
        BlockType.FeatureCards => "Feature-Karten",
        BlockType.CheckList => "Aufzählungsliste",
        BlockType.FixedText => "Fester Text",
        _ => "Block"
    };
    
    public static string GetInputPlaceholder(this BlockType type) => type switch
    {
        BlockType.Image => "https://beispiel.de/bild.jpg",
        BlockType.RichText => "**Fett** und *kursiv* werden unterstützt...",
        BlockType.KeyValueGrid => "Titel | Beschreibung\nTitel 2 | Beschreibung 2",
        BlockType.DataTable => "Eigenschaft | Wert\nEigenschaft 2 | Wert 2",
        BlockType.FeatureCards => "Kategorie | Option 1, Option 2\nKategorie 2 | Option 3",
        BlockType.CheckList => "Artikel 1\nArtikel 2\nArtikel 3",
        BlockType.FixedText => "Text aus Layout-Vorlage (editierbar)",
        _ => ""
    };
    
    public static string? GetInputHint(this BlockType type) => type switch
    {
        BlockType.RichText     => "**fett** · *kursiv* · [Link](url)",
        BlockType.KeyValueGrid => "Titel | Beschreibung",
        BlockType.DataTable    => "Eigenschaft | Wert",
        BlockType.FeatureCards => "Kategorie | Option 1, Option 2",
        _ => null
    };

    public static string GetInputLabel(this BlockType type) => type switch
    {
        BlockType.Image => "Bild-URL oder Base64",
        BlockType.RichText => "Text (Markdown unterstützt)",
        BlockType.KeyValueGrid => "Pro Zeile: Titel | Beschreibung",
        BlockType.DataTable => "Pro Zeile: Eigenschaft | Wert",
        BlockType.FeatureCards => "Pro Zeile: Kategorie | Optionen",
        BlockType.CheckList => "Pro Zeile: Ein Eintrag",
        BlockType.FixedText => "Fester Text (Markdown, aus Layout vorbelegt)",
        _ => "Inhalt"
    };
    
    public static string GetDemoContent(this BlockType type) => type switch
    {
        BlockType.Image => "https://placehold.co/600x400/1a1a1a/f5c518?text=Produktbild",
        BlockType.RichText => $"**Das Produkt** bietet herausragende Qualität und durchdachte Features.\n\nDie hochwertige Verarbeitung garantiert langlebige Zuverlässigkeit für den täglichen Einsatz.",
        BlockType.KeyValueGrid => "Premium Qualität | Erstklassige Materialien und Verarbeitung\nInnovatives Design | Moderne Optik trifft Funktionalität\nEinfache Bedienung | Intuitive Handhabung für jeden\nLanglebigkeit | Robust und zuverlässig",
        BlockType.DataTable => "Material | Aluminium / Kunststoff\nAbmessungen | 250 × 150 × 80 mm\nGewicht | 450 g\nFarbe | Schwarz\nAnschlüsse | USB-C, HDMI",
        BlockType.FeatureCards => "Typ A | Standard, Premium, Pro\nTyp B | Basic, Advanced",
        BlockType.CheckList => "1× Hauptgerät\n1× USB-C Kabel\n1× Kurzanleitung\n1× Garantiekarte",
        BlockType.FixedText => "**Hinweis:** Dies ist ein fester Textbaustein aus dem Layout.",
        _ => ""
    };
}
