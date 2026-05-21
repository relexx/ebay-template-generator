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
    
    public ArticleData Clone() => new()
    {
        Title = Title,
        Subtitle = Subtitle,
        MobileSummary = MobileSummary,
        Footer = Footer,
        BlockContents = new Dictionary<string, string>(BlockContents),
        Layout = Layout,
        CreatedAt = CreatedAt,
        ModifiedAt = ModifiedAt
    };

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
    
    [GeneratedRegex(@"[^\w\-äöüÄÖÜß]", RegexOptions.None, 100)]
    private static partial Regex SafeFileNameRegex();
    
    [GeneratedRegex(@"-+", RegexOptions.None, 100)]
    private static partial Regex MultiDashRegex();
}
