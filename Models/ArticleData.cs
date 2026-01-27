using System.Text.RegularExpressions;

namespace EbayTemplateGenerator.Models;

/// <summary>
/// Artikeldaten mit eingebettetem Layout für maximale Portabilität
/// </summary>
public partial class ArticleData
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string MobileSummary { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;
    
    public Dictionary<string, string> BlockContents { get; set; } = new();
    public LayoutTemplate Layout { get; set; } = LayoutTemplate.CreateStandard();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    
    public string GetBlockContent(string blockId) 
        => BlockContents.TryGetValue(blockId, out var content) ? content : string.Empty;
    
    public void SetBlockContent(string blockId, string content)
    {
        BlockContents[blockId] = content;
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void ClearBlockContent(string blockId)
    {
        BlockContents.Remove(blockId);
        ModifiedAt = DateTime.UtcNow;
    }
    
    public string GenerateFileName(string extension = "json")
    {
        var safeName = string.IsNullOrWhiteSpace(Title) 
            ? "artikel" 
            : SafeFileNameRegex().Replace(Title, "-");
        
        safeName = MultiDashRegex().Replace(safeName, "-").Trim('-');
        if (safeName.Length > Constants.Limits.MaxFileNameLength) 
            safeName = safeName[..Constants.Limits.MaxFileNameLength].TrimEnd('-');
        
        return $"{safeName}_{DateTime.UtcNow:yyyy-MM-dd_HHmm}.{extension}";
    }
    
    public static string GenerateJsonSchema(LayoutTemplate layout)
    {
        var blockSchemas = layout.Blocks
            .OrderBy(b => b.Order)
            .Select(b => $@"    ""{b.Id}"": ""{b.Type.GetInputPlaceholder().Replace("\n", "\\n").Replace("\"", "\\\"")}""");
        
        return $$"""
            {
              "_schema": "Artikeldaten für relexx' Template Generator",
              "_layout": "{{layout.Name}}",
              "_hint": "Fülle alle Felder aus. Das Ergebnis kann direkt importiert werden.",
              
              "Title": "Produktname (Hauptüberschrift)",
              "Subtitle": "Kurzbeschreibung | Artikelnummer",
              "MobileSummary": "Max. {{Constants.Limits.MaxMobileSummaryLength}} Zeichen für mobile Vorschau und Google Shopping",
              "Footer": "SKU: ABC123\nFarbe: Schwarz\nHerstellergarantie: 2 Jahre (max. {{Constants.Limits.MaxFooterLines}} Zeilen)",
              
              "BlockContents": {
            {{string.Join(",\n", blockSchemas)}}
              }
            }
            """;
    }
    
    [GeneratedRegex(@"[^\w\-äöüÄÖÜß]", RegexOptions.None, 100)]
    private static partial Regex SafeFileNameRegex();
    
    [GeneratedRegex(@"-+", RegexOptions.None, 100)]
    private static partial Regex MultiDashRegex();
}
