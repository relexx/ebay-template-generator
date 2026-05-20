namespace EbayTemplateGenerator;

/// <summary>
/// Zentrale Konstanten für die Anwendung
/// </summary>
public static class Constants
{
    /// <summary>LocalStorage Keys</summary>
    public static class Storage
    {
        public const string LayoutsKey = "relexx-layouts";
        public const string ArticleKey = "relexx-article";
        public const string SettingsKey = "relexx-settings";
    }
    
    /// <summary>Dateigrößen-Limits</summary>
    public static class FileLimits
    {
        public const long MaxImageSizeBytes = 5 * 1024 * 1024;  // 5 MB
        public const long MaxImportSizeBytes = 10 * 1024 * 1024; // 10 MB
    }
    
    /// <summary>UI-Zeitkonstanten</summary>
    public static class Timing
    {
        public const int NotificationDurationMs = 3000;
    }
    
    /// <summary>Validierungsgrenzen</summary>
    public static class Limits
    {
        public const int MaxMobileSummaryLength = 800;
        public const int MaxFooterLines = 4;
        public const int MaxFileNameLength = 50;
        public const int IdLength = 8;
    }
    
    public record IconCategory(string Label, string[] Icons);

    /// <summary>Icon categories for the block icon picker (Lucide icon set)</summary>
    public static readonly IconCategory[] IconCategories =
    [
        new("Versand & Logistik",  ["truck", "package2", "box", "mapPin", "clock", "calendar"]),
        new("Qualität & Service",  ["shield", "award", "star", "zap", "checkCircle", "sparkles"]),
        new("Technik & Produkt",   ["cpu", "wifi", "battery", "camera", "monitor", "headphones", "printer", "wrench", "toolbox", "scissors", "hammer", "ruler", "weight"]),
        new("Kommunikation",       ["globe", "phone", "mail", "link"]),
        new("Allgemein",           ["image", "fileText", "lightbulb", "target", "barChart", "search", "palette", "pin", "settings"]),
        new("UI & Status",         ["tags", "grid", "list", "alertTriangle", "info", "eye", "eyeOff", "xCircle"]),
    ];

    /// <summary>Flat list of all available icon names (derived from IconCategories)</summary>
    public static IEnumerable<string> AvailableIcons => IconCategories.SelectMany(c => c.Icons);
    
    /// <summary>Verfügbare Aufzählungszeichen</summary>
    public static readonly string[] AvailableBullets = 
    { 
        "✓", "•", "▸", "★", "►", "◆", "→", "▪" 
    };
}
