using System.Text;
using System.Web;
using EbayTemplateGenerator.Models;
using Markdig;

namespace EbayTemplateGenerator.Services;

public class SectionTitles
{
    public string Highlights { get; set; } = "Highlights";
    public string TechnicalData { get; set; } = "Technische Daten";
    public string Compatibility { get; set; } = "Kompatibilität";
    public string DeliveryScope { get; set; } = "Lieferumfang";
}

public class TemplateGeneratorService
{
    private readonly MarkdownPipeline _pipeline;

    public TemplateGeneratorService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseEmphasisExtras()
            .Build();
    }

    public string GenerateHtml(TemplateData data, SectionTitles? titles = null)
    {
        titles ??= new SectionTitles();
        var sb = new StringBuilder();
        
        // Header comment
        sb.AppendLine("<!-- eBay HTML-Template - Generiert mit relexx' Template Generator -->");
        sb.AppendLine();
        
        // Viewport meta tag
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.AppendLine();
        
        // Main container
        sb.AppendLine($@"<div style=""max-width: 700px; margin: 0 auto; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #1a1a1a; line-height: 1.6; font-size: 16px;"">");
        sb.AppendLine();
        
        // Mobile Summary (schema.org)
        if (!string.IsNullOrWhiteSpace(data.MobileSummary))
        {
            sb.AppendLine("  <!-- Mobile Summary (schema.org) -->");
            sb.AppendLine("  <div vocab=\"https://schema.org/\" typeof=\"Product\" style=\"margin-bottom: 20px;\">");
            sb.AppendLine($"    <span property=\"description\">{HttpUtility.HtmlEncode(data.MobileSummary)}</span>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
        }
        
        // Header
        sb.AppendLine(GenerateHeader(data));
        
        // Image
        if (!string.IsNullOrWhiteSpace(data.ImageUrl))
        {
            sb.AppendLine(GenerateImageSection(data));
        }
        
        // Description
        if (!string.IsNullOrWhiteSpace(data.Description))
        {
            sb.AppendLine(GenerateDescriptionSection(data));
        }
        
        // Highlights
        if (!string.IsNullOrWhiteSpace(data.Highlights))
        {
            sb.AppendLine(GenerateHighlightsSection(data, titles.Highlights));
        }
        
        // Technical Data
        if (!string.IsNullOrWhiteSpace(data.TechnicalData))
        {
            sb.AppendLine(GenerateTechnicalDataSection(data, titles.TechnicalData));
        }
        
        // Compatibility
        if (!string.IsNullOrWhiteSpace(data.Compatibility))
        {
            sb.AppendLine(GenerateCompatibilitySection(data, titles.Compatibility));
        }
        
        // Delivery Scope
        if (!string.IsNullOrWhiteSpace(data.DeliveryScope))
        {
            sb.AppendLine(GenerateDeliveryScopeSection(data, titles.DeliveryScope));
        }
        
        // Footer
        sb.AppendLine(GenerateFooter(data));
        
        // Close main container
        sb.AppendLine("</div>");
        
        return sb.ToString();
    }

    private string GenerateHeader(TemplateData data)
    {
        return $@"  <!-- Header -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {data.PrimaryColor} 0%, {LightenColor(data.PrimaryColor, 0.1)} 100%); border-radius: 8px 8px 0 0;"">
    <tr>
      <td style=""padding: 25px 30px; text-align: center;"">
        <div style=""margin: 0; color: {data.AccentColor}; font-size: 22px; font-weight: 600; letter-spacing: 0.5px;"">
          {HttpUtility.HtmlEncode(data.Title)}
        </div>
        <div style=""margin: 8px 0 0 0; color: #cccccc; font-size: 14px;"">
          {HttpUtility.HtmlEncode(data.Subtitle)}
        </div>
      </td>
    </tr>
  </table>
";
    }

    private string GenerateImageSection(TemplateData data)
    {
        return $@"  <!-- Produktbild -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 20px; text-align: center;"">
        <img src=""{HttpUtility.HtmlEncode(data.ImageUrl)}"" 
             alt=""{HttpUtility.HtmlEncode(data.Title)}"" 
             style=""max-width: 100%; width: 600px; height: auto; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
      </td>
    </tr>
  </table>
";
    }

    private string GenerateDescriptionSection(TemplateData data)
    {
        var htmlContent = Markdown.ToHtml(data.Description, _pipeline);
        // Remove outer <p> tags and clean up for inline use
        htmlContent = htmlContent.Replace("<p>", "<p style=\"margin: 0 0 15px 0; font-size: 15px; color: #333;\">");
        
        return $@"  <!-- Beschreibung -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {data.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {htmlContent}
      </td>
    </tr>
  </table>
";
    }

    private string GenerateHighlightsSection(TemplateData data, string sectionTitle)
    {
        var lines = data.Highlights.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        
        sb.AppendLine($@"  <!-- Highlights -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {data.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          ✦ {HttpUtility.HtmlEncode(sectionTitle)}
        </div>
        
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var parts = line.Split('|', 2);
            var title = parts[0].Trim();
            var description = parts.Length > 1 ? parts[1].Trim() : "";
            
            bool isEvenRow = i % 2 == 0;
            bool isLastRow = i == lines.Length - 1;
            bool isLeftColumn = i % 2 == 0;
            
            if (isLeftColumn)
            {
                sb.AppendLine("          <tr>");
            }
            
            var borderStyle = isLastRow && lines.Length % 2 == 1 ? "" : "border-bottom: 1px solid #eee;";
            var paddingStyle = isLeftColumn ? "padding: 12px 15px 12px 0;" : "padding: 12px 0 12px 15px;";
            
            sb.AppendLine($@"            <td width=""50%"" style=""{paddingStyle} vertical-align: top; {borderStyle}"">
              <span style=""color: {data.AccentColor}; font-weight: bold;"">▸</span>
              <strong>{HttpUtility.HtmlEncode(title)}</strong>");
            
            if (!string.IsNullOrWhiteSpace(description))
            {
                sb.AppendLine($@"              <br><span style=""font-size: 13px; color: #666;"">{HttpUtility.HtmlEncode(description)}</span>");
            }
            
            sb.AppendLine("            </td>");
            
            if (!isLeftColumn || isLastRow)
            {
                if (isLastRow && isLeftColumn)
                {
                    // Add empty cell for odd number of items
                    sb.AppendLine("            <td width=\"50%\"></td>");
                }
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

    private string GenerateTechnicalDataSection(TemplateData data, string sectionTitle)
    {
        var lines = data.TechnicalData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        
        sb.AppendLine($@"  <!-- Technische Daten -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {data.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {data.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          ⚙ {HttpUtility.HtmlEncode(sectionTitle)}
        </div>
        
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #fff; border-radius: 6px; overflow: hidden; border: 1px solid #e0e0e0;"">
          <tr style=""background: {data.PrimaryColor};"">
            <td style=""padding: 12px 15px; font-weight: 600; width: 45%; color: #ffffff;"">Spezifikation</td>
            <td style=""padding: 12px 15px; font-weight: 600; color: #ffffff;"">Wert</td>
          </tr>");

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var parts = line.Split('|', 2);
            var key = parts[0].Trim();
            var value = parts.Length > 1 ? parts[1].Trim() : "";
            
            var bgStyle = i % 2 == 1 ? "background: #fafafa;" : "";
            var borderStyle = i < lines.Length - 1 ? "border-bottom: 1px solid #eee;" : "";
            
            sb.AppendLine($@"          <tr style=""{bgStyle}"">
            <td style=""padding: 10px 15px; {borderStyle} color: #555;"">{HttpUtility.HtmlEncode(key)}</td>
            <td style=""padding: 10px 15px; {borderStyle}"">{HttpUtility.HtmlEncode(value)}</td>
          </tr>");
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        
        return sb.ToString();
    }

    private string GenerateCompatibilitySection(TemplateData data, string sectionTitle)
    {
        var lines = data.Compatibility.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        
        // Predefined colors for compatibility boxes
        var colors = new[] 
        {
            ("linear-gradient(135deg, #0071c5 0%, #00aeef 100%)", "#ffffff"), // Intel blue
            ("linear-gradient(135deg, #ed1c24 0%, #ff6b6b 100%)", "#ffffff"), // AMD red
            ("linear-gradient(135deg, #76b900 0%, #a4d233 100%)", "#ffffff"), // Green
            ("linear-gradient(135deg, #ff6600 0%, #ffaa00 100%)", "#ffffff"), // Orange
        };
        
        sb.AppendLine($@"  <!-- Kompatibilität -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: #ffffff; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {data.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          🔧 {HttpUtility.HtmlEncode(sectionTitle)}
        </div>
        
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0;"">
          <tr>");

        var widthPercent = 100 / Math.Max(lines.Length, 1);
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var parts = line.Split('|', 2);
            var platform = parts[0].Trim();
            var sockets = parts.Length > 1 ? parts[1].Trim() : "";
            
            var (bgColor, textColor) = colors[i % colors.Length];
            var paddingStyle = i == 0 ? "padding: 10px 10px 10px 0;" : (i == lines.Length - 1 ? "padding: 10px 0 10px 10px;" : "padding: 10px 5px;");
            
            sb.AppendLine($@"            <td width=""{widthPercent}%"" style=""{paddingStyle} vertical-align: top;"">
              <div style=""background: {bgColor}; color: {textColor}; padding: 18px; border-radius: 6px; text-align: center;"">
                <div style=""font-size: 13px; opacity: 0.9; margin-bottom: 4px;"">{HttpUtility.HtmlEncode(platform)}</div>
                <div style=""font-size: 15px; font-weight: 600;"">{HttpUtility.HtmlEncode(sockets)}</div>
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

    private string GenerateDeliveryScopeSection(TemplateData data, string sectionTitle)
    {
        var lines = data.DeliveryScope.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        
        sb.AppendLine($@"  <!-- Lieferumfang -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {data.BackgroundColor}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {data.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          📦 {HttpUtility.HtmlEncode(sectionTitle)}
        </div>
        
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");

        foreach (var line in lines)
        {
            var item = line.Trim();
            if (!string.IsNullOrWhiteSpace(item))
            {
                sb.AppendLine($@"          <tr><td style=""padding: 6px 0; color: #333;"">✓ {HttpUtility.HtmlEncode(item)}</td></tr>");
            }
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        
        return sb.ToString();
    }

    private string GenerateFooter(TemplateData data)
    {
        var lines = data.Footer?.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Take(4)
            .ToArray() ?? Array.Empty<string>();
        
        if (lines.Length == 0)
        {
            return $@"  <!-- Footer -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {data.PrimaryColor} 0%, {LightenColor(data.PrimaryColor, 0.1)} 100%); border-radius: 0 0 8px 8px;"">
    <tr>
      <td style=""padding: 18px 20px; text-align: center; color: #999; font-size: 13px;"">
        &nbsp;
      </td>
    </tr>
  </table>
";
        }
        
        var sb = new StringBuilder();
        var widthPercent = 100 / lines.Length;
        
        sb.AppendLine($@"  <!-- Footer -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: linear-gradient(135deg, {data.PrimaryColor} 0%, {LightenColor(data.PrimaryColor, 0.1)} 100%); border-radius: 0 0 8px 8px;"">
    <tr>");
        
        for (int i = 0; i < lines.Length; i++)
        {
            var alignment = lines.Length == 1 ? "center" : 
                            i == 0 ? "left" : 
                            i == lines.Length - 1 ? "right" : "center";
            
            sb.AppendLine($@"      <td width=""{widthPercent}%"" style=""padding: 18px 20px; text-align: {alignment}; color: #999; font-size: 13px;"">
        <strong style=""color: {data.AccentColor};"">{GetFooterLabel(lines[i])}</strong> {GetFooterValue(lines[i])}
      </td>");
        }
        
        sb.AppendLine(@"    </tr>
  </table>
");
        
        return sb.ToString();
    }

    private static string GetFooterLabel(string footer)
    {
        if (string.IsNullOrWhiteSpace(footer)) return "";
        var parts = footer.Split(':', 2);
        return parts.Length > 1 ? HttpUtility.HtmlEncode(parts[0].Trim()) + ":" : "";
    }

    private static string GetFooterValue(string footer)
    {
        if (string.IsNullOrWhiteSpace(footer)) return "";
        var parts = footer.Split(':', 2);
        return parts.Length > 1 ? HttpUtility.HtmlEncode(parts[1].Trim()) : HttpUtility.HtmlEncode(footer);
    }

    private static string LightenColor(string hexColor, double factor)
    {
        try
        {
            hexColor = hexColor.TrimStart('#');
            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);
            
            r = Math.Min(255, (int)(r + (255 - r) * factor));
            g = Math.Min(255, (int)(g + (255 - g) * factor));
            b = Math.Min(255, (int)(b + (255 - b) * factor));
            
            return $"#{r:X2}{g:X2}{b:X2}";
        }
        catch
        {
            return hexColor;
        }
    }
}
