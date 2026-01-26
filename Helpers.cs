using System.Text.Json;

namespace EbayTemplateGenerator;

/// <summary>
/// Gemeinsame Hilfsmethoden
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Generiert eine kurze eindeutige ID
    /// </summary>
    public static string GenerateShortId() 
        => Guid.NewGuid().ToString("N")[..Constants.Limits.IdLength];
    
    /// <summary>
    /// Zentrale JsonSerializerOptions für konsistente Serialisierung
    /// </summary>
    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = null // PascalCase beibehalten
    };
}
