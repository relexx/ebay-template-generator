namespace EbayTemplateGenerator.Models;

/// <summary>
/// Farbschema für ein Layout
/// </summary>
public class ColorScheme
{
    /// <summary>Primärfarbe (Header, Footer, Tabellenkopf)</summary>
    public string PrimaryColor { get; set; } = "#1a1a1a";
    
    /// <summary>Akzentfarbe (Highlights, Überschriften)</summary>
    public string AccentColor { get; set; } = "#f5c518";
    
    /// <summary>Hintergrundfarbe (Alternating Sections)</summary>
    public string BackgroundColor { get; set; } = "#f8f9fa";
    
    /// <summary>
    /// Erstellt eine Kopie des Farbschemas
    /// </summary>
    public ColorScheme Clone()
    {
        return new ColorScheme
        {
            PrimaryColor = PrimaryColor,
            AccentColor = AccentColor,
            BackgroundColor = BackgroundColor
        };
    }
}
