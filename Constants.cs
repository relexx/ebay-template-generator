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
    
    /// <summary>Verfügbare Icons für Blöcke</summary>
    public static readonly string[] AvailableIcons = 
    { 
        "🖼️", "📝", "✦", "⚙", "🔧", "📦", "💡", "⭐", 
        "🎯", "📊", "🔍", "⚡", "🛠️", "📐", "🎨", "📋" 
    };
    
    /// <summary>Verfügbare Aufzählungszeichen</summary>
    public static readonly string[] AvailableBullets = 
    { 
        "✓", "•", "▸", "★", "►", "◆", "→", "▪" 
    };
}
