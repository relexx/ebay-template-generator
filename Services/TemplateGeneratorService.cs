using System.Text;
using System.Web;
using EbayTemplateGenerator.Models;
using Markdig;

namespace EbayTemplateGenerator.Services;

/// <summary>
/// Generiert eBay-konformes HTML aus Artikeldaten und dynamischen Blöcken
/// </summary>
public class TemplateGeneratorService
{
    private readonly MarkdownPipeline _pipeline;

    public TemplateGeneratorService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseEmphasisExtras()
            .Build();
    }

    /// <summary>
    /// Generiert das komplette HTML aus Artikeldaten
    /// </summary>
    public string GenerateHtml(ArticleData article)
    {
        var layout = article.Layout;
        var colors = layout.Colors;
        var sb = new StringBuilder();
        
        // Header comment
        sb.AppendLine("<!-- eBay HTML-Template - Generiert mit relexx' Template Generator -->");
        sb.AppendLine($"<!-- Layout: {layout.Name} -->");
        sb.AppendLine();
        
        // Viewport meta tag
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.AppendLine();
        
        // Main container
        sb.AppendLine($@"<div style=""max-width: 700px; margin: 0 auto; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #1a1a1a; line-height: 1.6; font-size: 16px;"">");
        sb.AppendLine();
        
        // Mobile Summary (schema.org)
        if (!string.IsNullOrWhiteSpace(article.MobileSummary))
        {
            sb.AppendLine("  <!-- Mobile Summary (schema.org) -->");
            sb.AppendLine("  <div vocab=\"https://schema.org/\" typeof=\"Product\" style=\"margin-bottom: 20px;\">");
            sb.AppendLine($"    <span property=\"description\">{Encode(article.MobileSummary)}</span>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
        }
        
        // Header
        sb.AppendLine(GenerateHeader(article, colors));
        
        // Dynamic Blocks
        foreach (var block in layout.Blocks.OrderBy(b => b.Order))
        {
            var content = article.GetBlockContent(block.Id);
            if (string.IsNullOrWhiteSpace(content)) continue;
            
            var html = block.Type switch
            {
                BlockType.Image => GenerateImageBlock(block, content, colors),
                BlockType.RichText => GenerateRichTextBlock(block, content, colors),
                BlockType.KeyValueGrid => GenerateKeyValueGridBlock(block, content, colors),
                BlockType.DataTable => GenerateDataTableBlock(block, content, colors),
                BlockType.FeatureCards => GenerateFeatureCardsBlock(block, content, colors),
                BlockType.CheckList => GenerateCheckListBlock(block, content, colors),
                _ => string.Empty
            };
            
            if (!string.IsNullOrEmpty(html))
            {
                sb.AppendLine(html);
            }
        }
        
        // Footer
        sb.AppendLine(GenerateFooter(article, colors));
        
        // Close main container
        sb.AppendLine("</div>");
        
        return sb.ToString();
    }

    private string GenerateHeader(ArticleData article, ColorScheme colors)
    {
        return $@"  <!-- Header -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {colors.PrimaryColor} 0%, {LightenColor(colors.PrimaryColor, 0.1)} 100%); border-radius: 8px 8px 0 0;"">
    <tr>
      <td style=""padding: 25px 30px; text-align: center;"">
        <div style=""margin: 0; color: {colors.AccentColor}; font-size: 22px; font-weight: 600; letter-spacing: 0.5px;"">
          {Encode(article.Title)}
        </div>
        <div style=""margin: 8px 0 0 0; color: #cccccc; font-size: 14px;"">
          {Encode(article.Subtitle)}
        </div>
      </td>
    </tr>
  </table>
";
    }

    private string GenerateImageBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var alignment = block.Options.Alignment;
        var maxWidth = block.Options.MaxWidth;
        
        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 20px; text-align: {alignment};"">
        <img src=""{Encode(content)}"" alt=""Produktbild"" style=""max-width: 100%; width: {maxWidth}px; height: auto; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
      </td>
    </tr>
  </table>
";
    }

    private string GenerateRichTextBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var html = Markdown.ToHtml(content, _pipeline);
        html = html.Replace("<p>", "<p style=\"margin: 0 0 15px 0; font-size: 15px; color: #333;\">");
        html = html.Replace("<strong>", $"<strong style=\"color: {colors.PrimaryColor};\">");
        
        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {colors.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {html}
      </td>
    </tr>
  </table>
";
    }

    private string GenerateKeyValueGridBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var bullet = block.Options.BulletChar;
        var columns = Math.Max(1, Math.Min(3, block.Options.Columns));
        var widthPercent = 100 / columns;
        
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {colors.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          {block.Icon} {Encode(block.Title)}
        </div>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");

        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', 2);
            var title = parts[0].Trim();
            var desc = parts.Length > 1 ? parts[1].Trim() : "";
            
            bool rowStart = i % columns == 0;
            bool rowEnd = (i + 1) % columns == 0 || i == lines.Length - 1;
            
            if (rowStart) sb.AppendLine("          <tr>");
            
            sb.AppendLine($@"            <td width=""{widthPercent}%"" style=""padding: 12px 10px; vertical-align: top; border-bottom: 1px solid #eee;"">
              <span style=""color: {colors.AccentColor}; font-weight: bold;"">{bullet}</span>
              <strong>{Encode(title)}</strong>
              {(string.IsNullOrEmpty(desc) ? "" : $"<br><span style=\"font-size: 13px; color: #666;\">{Encode(desc)}</span>")}
            </td>");
            
            if (rowEnd)
            {
                // Leere Zellen auffüllen
                var remaining = columns - (i % columns) - 1;
                for (int j = 0; j < remaining; j++)
                    sb.AppendLine($"            <td width=\"{widthPercent}%\"></td>");
                sb.AppendLine("          </tr>");
            }
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateDataTableBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var col1 = block.Options.Column1Header;
        var col2 = block.Options.Column2Header;
        var showHeaders = block.Options.ShowColumnHeaders;
        var zebra = block.Options.AlternatingBackground;
        
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {colors.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {colors.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          {block.Icon} {Encode(block.Title)}
        </div>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #fff; border-radius: 6px; overflow: hidden; border: 1px solid #e0e0e0;"">");

        if (showHeaders)
        {
            sb.AppendLine($@"          <tr style=""background: {colors.PrimaryColor};"">
            <td style=""padding: 12px 15px; font-weight: 600; width: 45%; color: #ffffff;"">{Encode(col1)}</td>
            <td style=""padding: 12px 15px; font-weight: 600; color: #ffffff;"">{Encode(col2)}</td>
          </tr>");
        }

        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', 2);
            var key = parts[0].Trim();
            var value = parts.Length > 1 ? parts[1].Trim() : "";
            
            var bg = zebra && i % 2 == 1 ? "background: #fafafa;" : "";
            var border = i < lines.Length - 1 ? "border-bottom: 1px solid #eee;" : "";
            
            sb.AppendLine($@"          <tr style=""{bg}"">
            <td style=""padding: 10px 15px; {border} color: #555;"">{Encode(key)}</td>
            <td style=""padding: 10px 15px; {border}"">{Encode(value)}</td>
          </tr>");
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateFeatureCardsBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var cardGradients = new[]
        {
            ("linear-gradient(135deg, #0071c5 0%, #00aeef 100%)", "#ffffff"),
            ("linear-gradient(135deg, #ed1c24 0%, #ff6b6b 100%)", "#ffffff"),
            ("linear-gradient(135deg, #76b900 0%, #a4d233 100%)", "#ffffff"),
            ("linear-gradient(135deg, #ff6600 0%, #ffaa00 100%)", "#ffffff"),
            ("linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%)", "#ffffff")
        };
        
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {colors.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          {block.Icon} {Encode(block.Title)}
        </div>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
          <tr>");

        var widthPercent = 100 / Math.Max(lines.Length, 1);
        
        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', 2);
            var platform = parts[0].Trim();
            var variants = parts.Length > 1 ? parts[1].Trim() : "";
            
            var (bg, text) = cardGradients[i % cardGradients.Length];
            var pad = i == 0 ? "padding: 10px 10px 10px 0;" : (i == lines.Length - 1 ? "padding: 10px 0 10px 10px;" : "padding: 10px 5px;");
            
            sb.AppendLine($@"            <td width=""{widthPercent}%"" style=""{pad} vertical-align: top;"">
              <div style=""background: {bg}; color: {text}; padding: 18px; border-radius: 6px; text-align: center;"">
                <div style=""font-size: 13px; opacity: 0.9; margin-bottom: 4px;"">{Encode(platform)}</div>
                <div style=""font-size: 15px; font-weight: 600;"">{Encode(variants)}</div>
              </div>
            </td>");
        }

        sb.AppendLine(@"          </tr>
        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateCheckListBlock(BlockDefinition block, string content, ColorScheme colors)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var bullet = block.Options.BulletChar;
        
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {colors.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {colors.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          {block.Icon} {Encode(block.Title)}
        </div>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");

        foreach (var line in lines)
        {
            var item = line.Trim();
            if (!string.IsNullOrEmpty(item))
            {
                sb.AppendLine($@"          <tr>
            <td style=""padding: 6px 0; color: #333;"">
              <span style=""color: {colors.AccentColor}; margin-right: 8px;"">{bullet}</span>{Encode(item)}
            </td>
          </tr>");
            }
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateFooter(ArticleData article, ColorScheme colors)
    {
        var lines = article.Footer?.Split('\n', StringSplitOptions.RemoveEmptyEntries).Take(4).ToArray() ?? [];
        
        if (lines.Length == 0)
        {
            return $@"  <!-- Footer -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {colors.PrimaryColor} 0%, {LightenColor(colors.PrimaryColor, 0.1)} 100%); border-radius: 0 0 8px 8px;"">
    <tr>
      <td style=""padding: 18px 20px; text-align: center; color: #999; font-size: 13px;"">&nbsp;</td>
    </tr>
  </table>
";
        }
        
        var sb = new StringBuilder();
        var widthPercent = 100 / lines.Length;
        
        sb.AppendLine($@"  <!-- Footer -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {colors.PrimaryColor} 0%, {LightenColor(colors.PrimaryColor, 0.1)} 100%); border-radius: 0 0 8px 8px;"">
    <tr>");
        
        for (int i = 0; i < lines.Length; i++)
        {
            var align = lines.Length == 1 ? "center" : i == 0 ? "left" : i == lines.Length - 1 ? "right" : "center";
            var parts = lines[i].Split(':', 2);
            var label = parts.Length > 1 ? Encode(parts[0].Trim()) + ":" : "";
            var value = parts.Length > 1 ? Encode(parts[1].Trim()) : Encode(lines[i]);
            
            sb.AppendLine($@"      <td width=""{widthPercent}%"" style=""padding: 18px 20px; text-align: {align}; color: #999; font-size: 13px;"">
        <strong style=""color: {colors.AccentColor};"">{label}</strong> {value}
      </td>");
        }
        
        sb.AppendLine(@"    </tr>
  </table>
");
        return sb.ToString();
    }

    private static string Encode(string? text) => HttpUtility.HtmlEncode(text ?? "");

    private static string LightenColor(string hex, double factor)
    {
        try
        {
            hex = hex.TrimStart('#');
            int r = Convert.ToInt32(hex[..2], 16);
            int g = Convert.ToInt32(hex[2..4], 16);
            int b = Convert.ToInt32(hex[4..6], 16);
            
            r = Math.Min(255, (int)(r + (255 - r) * factor));
            g = Math.Min(255, (int)(g + (255 - g) * factor));
            b = Math.Min(255, (int)(b + (255 - b) * factor));
            
            return $"#{r:X2}{g:X2}{b:X2}";
        }
        catch { return hex; }
    }
}
