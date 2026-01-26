namespace EbayTemplateGenerator.Models;

/// <summary>
/// Ein Layout-Template definiert die Struktur und das Design
/// </summary>
public class LayoutTemplate
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Name { get; set; } = "Neues Layout";
    public bool IsDefault { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    
    // Farbschema
    public ColorScheme Colors { get; set; } = new();
    
    // Block-Definitionen
    public List<BlockDefinition> Blocks { get; set; } = new();
    
    /// <summary>
    /// Erstellt eine tiefe Kopie
    /// </summary>
    public LayoutTemplate Clone(bool newId = true)
    {
        return new LayoutTemplate
        {
            Id = newId ? Guid.NewGuid().ToString("N")[..8] : Id,
            Name = newId ? $"{Name} (Kopie)" : Name,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            Colors = Colors.Clone(),
            Blocks = Blocks.Select(b => b.Clone()).ToList()
        };
    }
    
    /// <summary>
    /// Prüft ob ein anderes Layout strukturell kompatibel ist
    /// </summary>
    public bool IsCompatibleWith(LayoutTemplate? other)
    {
        if (other == null) return false;
        if (Blocks.Count != other.Blocks.Count) return false;
        
        var myIds = Blocks.OrderBy(b => b.Id).Select(b => b.Id).ToList();
        var otherIds = other.Blocks.OrderBy(b => b.Id).Select(b => b.Id).ToList();
        
        return myIds.SequenceEqual(otherIds);
    }
    
    /// <summary>
    /// Ordnet die Blöcke nach Order-Wert neu
    /// </summary>
    public void ReorderBlocks()
    {
        var ordered = Blocks.OrderBy(b => b.Order).ToList();
        for (int i = 0; i < ordered.Count; i++)
        {
            ordered[i].Order = i;
        }
        Blocks = ordered;
    }
    
    /// <summary>
    /// Standard-Layout mit allen typischen eBay-Blöcken
    /// </summary>
    public static LayoutTemplate CreateStandard()
    {
        return new LayoutTemplate
        {
            Id = "standard",
            Name = "Standard",
            IsDefault = true,
            Colors = new ColorScheme
            {
                PrimaryColor = "#1a1a1a",
                AccentColor = "#f5c518",
                BackgroundColor = "#f8f9fa"
            },
            Blocks = new List<BlockDefinition>
            {
                new()
                {
                    Id = "img",
                    Type = BlockType.Image,
                    Icon = "🖼️",
                    Title = "Produktbild",
                    Order = 0,
                    Options = new BlockOptions { Alignment = "center", MaxWidth = 600 }
                },
                new()
                {
                    Id = "desc",
                    Type = BlockType.RichText,
                    Icon = "📝",
                    Title = "Beschreibung",
                    Order = 1
                },
                new()
                {
                    Id = "highlights",
                    Type = BlockType.KeyValueGrid,
                    Icon = "✦",
                    Title = "Highlights",
                    Order = 2,
                    Options = new BlockOptions { BulletChar = "▸", Columns = 2 }
                },
                new()
                {
                    Id = "specs",
                    Type = BlockType.DataTable,
                    Icon = "⚙",
                    Title = "Technische Daten",
                    Order = 3,
                    Options = new BlockOptions
                    {
                        Column1Header = "Spezifikation",
                        Column2Header = "Wert",
                        ShowColumnHeaders = true,
                        AlternatingBackground = true
                    }
                },
                new()
                {
                    Id = "compat",
                    Type = BlockType.FeatureCards,
                    Icon = "🔧",
                    Title = "Kompatibilität",
                    Order = 4
                },
                new()
                {
                    Id = "scope",
                    Type = BlockType.CheckList,
                    Icon = "📦",
                    Title = "Lieferumfang",
                    Order = 5,
                    Options = new BlockOptions { BulletChar = "✓" }
                }
            }
        };
    }
}
