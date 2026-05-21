using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using EbayTemplateGenerator.Models;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace EbayTemplateGenerator.Services;

public class TemplateGeneratorService
{
    private readonly MarkdownPipeline _pipeline;

    public TemplateGeneratorService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseEmphasisExtras()
            .Build();
    }

    public string GenerateHtml(ArticleData article)
    {
        var layout = article.Layout;
        var colors = layout.Colors;
        var sb = new StringBuilder();
        
        sb.AppendLine("<!-- eBay HTML-Template - Generiert mit relexx' Template Generator -->");
        sb.AppendLine($"<!-- Layout: {layout.Name} -->");
        sb.AppendLine();
        
        // Viewport meta tag
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.AppendLine();
        
        // Main container
        sb.AppendLine($@"<div style=""max-width: 760px; margin: 0 auto; font-family: 'Geist', -apple-system, system-ui, sans-serif; color: #1a1a1a; line-height: 1.6; font-size: 16px;"">");
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
        var blockIndex = 0;
        foreach (var block in layout.Blocks.OrderBy(b => b.Order))
        {
            var content = article.GetBlockContent(block.Id);
            if (string.IsNullOrWhiteSpace(content)) continue;

            var html = block.Type switch
            {
                BlockType.Image         => GenerateImageBlock(block, content, colors, blockIndex),
                BlockType.RichText      => GenerateRichTextBlock(block, content, colors, blockIndex),
                BlockType.KeyValueGrid  => GenerateKeyValueGridBlock(block, content, colors, blockIndex),
                BlockType.DataTable     => GenerateDataTableBlock(block, content, colors, blockIndex),
                BlockType.FeatureCards  => GenerateFeatureCardsBlock(block, content, colors, blockIndex),
                BlockType.CheckList     => GenerateCheckListBlock(block, content, colors, blockIndex),
                BlockType.FixedText     => GenerateFixedTextBlock(block, content, colors, blockIndex),
                BlockType.ProsConsTable => GenerateProsConsTableBlock(block, content, colors, blockIndex),
                BlockType.CalloutBox    => GenerateCalloutBoxBlock(block, content, colors, blockIndex),
                BlockType.BadgeStrip    => GenerateBadgeStripBlock(block, content, colors, blockIndex),
                BlockType.RatingSnippet => GenerateRatingSnippetBlock(block, content, colors, blockIndex),
                BlockType.Gallery       => GenerateGalleryBlock(block, content, colors, blockIndex),
                BlockType.LinkList      => GenerateLinkListBlock(block, content, colors, blockIndex),
                BlockType.HeroBanner    => GenerateHeroBannerBlock(block, content, colors, blockIndex),
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(html))
            {
                sb.AppendLine(html);
                blockIndex++;
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

    private string GenerateImageBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var alignment = block.Options.Alignment;
        var maxWidth = block.Options.MaxWidth;
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 20px; text-align: {alignment};"">
        {titleHtml}
        <img src=""{Encode(content)}"" alt=""Produktbild"" style=""max-width: 100%; width: {maxWidth}px; height: auto; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
      </td>
    </tr>
  </table>
";
    }

    private string GenerateRichTextBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var html = Markdown.ToHtml(content, _pipeline);
        html = html.Replace("<p>", "<p style=\"margin: 0 0 15px 0; font-size: 15px; color: #333;\">");
        html = html.Replace("<strong>", $"<strong style=\"color: {colors.PrimaryColor};\">");
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        {html}
      </td>
    </tr>
  </table>
";
    }

    private string GenerateKeyValueGridBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var bullet = block.Options.BulletChar;
        var columns = Math.Max(1, Math.Min(3, block.Options.Columns));
        var widthPercent = 100 / columns;

        var titleHtmlKvg = BlockTitleHtml(block, colors);

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtmlKvg}
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

    private string GenerateDataTableBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var col1 = block.Options.Column1Header;
        var col2 = block.Options.Column2Header;
        var showHeaders = block.Options.ShowColumnHeaders;
        var zebra = block.Options.AlternatingBackground;

        var titleHtmlDt = BlockTitleHtml(block, colors);

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtmlDt}
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

    private string GenerateFeatureCardsBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
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

        var titleHtmlFc = BlockTitleHtml(block, colors);

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtmlFc}
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

    private string GenerateCheckListBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var bullet = block.Options.BulletChar;

        var titleHtmlCl = BlockTitleHtml(block, colors);

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtmlCl}
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

    private string GenerateFixedTextBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var html = Markdown.ToHtml(content, _pipeline);
        html = html.Replace("<p>", "<p style=\"margin: 0 0 15px 0; font-size: 15px; color: #333;\">");
        html = html.Replace("<strong>", $"<strong style=\"color: {colors.PrimaryColor};\">");
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} (Fester Text) -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        {html}
      </td>
    </tr>
  </table>
";
    }

    private string GenerateProsConsTableBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var pros = lines.Where(l => l.TrimStart().StartsWith("+")).Select(l => l.TrimStart().TrimStart('+').Trim()).ToList();
        var cons = lines.Where(l => l.TrimStart().StartsWith("-")).Select(l => l.TrimStart().TrimStart('-').Trim()).ToList();

        var titleHtml = BlockTitleHtml(block, colors);
        var maxRows = Math.Max(pros.Count, cons.Count);
        if (maxRows == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border-radius: 6px; overflow: hidden; border: 1px solid #e0e0e0;"">
          <tr>
            <td width=""50%"" style=""padding: 12px 16px; background: {colors.AccentColor}; color: #1a1a1a; font-weight: 600; font-size: 14px;"">✓ Vorteile</td>
            <td width=""50%"" style=""padding: 12px 16px; background: {colors.PrimaryColor}; color: #ffffff; font-weight: 600; font-size: 14px;"">✗ Nachteile</td>
          </tr>");

        for (int i = 0; i < maxRows; i++)
        {
            var pro = i < pros.Count ? pros[i] : "";
            var con = i < cons.Count ? cons[i] : "";
            var bg = i % 2 == 1 ? "background: #fafafa;" : "";
            var border = i < maxRows - 1 ? "border-bottom: 1px solid #eee;" : "";

            sb.AppendLine($@"          <tr style=""{bg}"">
            <td style=""padding: 10px 16px; {border} vertical-align: top;"">
              {(string.IsNullOrEmpty(pro) ? "&nbsp;" : $"<span style=\"color: #27ae60; font-weight: 600; margin-right: 6px;\">✓</span>{Encode(pro)}")}
            </td>
            <td style=""padding: 10px 16px; {border} vertical-align: top;"">
              {(string.IsNullOrEmpty(con) ? "&nbsp;" : $"<span style=\"color: #e74c3c; font-weight: 600; margin-right: 6px;\">✗</span>{Encode(con)}")}
            </td>
          </tr>");
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateCalloutBoxBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var html = Markdown.ToHtml(content, _pipeline);
        html = html.Replace("<p>", "<p style=\"margin: 0 0 10px 0; font-size: 15px; color: #333;\">");

        var (borderColor, bgColor, iconName, label) = block.Options.CalloutVariant switch
        {
            "warning" => ("#e8a000", "#fef9e7", "alertTriangle", "Hinweis"),
            "tip"     => ("#27ae60", "#eafaf1", "lightbulb",     "Tipp"),
            "error"   => ("#e74c3c", "#fdf2f2", "xCircle",       "Achtung"),
            _         => ("#0071c5", "#e8f4fd", "info",          "Info"),
        };

        var iconSvg = IconHelper.SvgForHtml(iconName, borderColor, 16);
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        <div style=""border-left: 4px solid {borderColor}; background: {bgColor}; padding: 16px 20px; border-radius: 0 6px 6px 0;"">
          <div style=""display: flex; align-items: center; gap: 8px; font-weight: 600; color: {borderColor}; margin-bottom: 10px; font-size: 14px;"">
            {iconSvg}&nbsp;{label}
          </div>
          <div>{html}</div>
        </div>
      </td>
    </tr>
  </table>
";
    }

    private string GenerateBadgeStripBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var items = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToList();
        if (items.Count == 0) return string.Empty;

        var titleHtml = BlockTitleHtml(block, colors);
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        <div style=""display: flex; flex-wrap: wrap; gap: 10px; align-items: center;"">");

        foreach (var item in items)
            sb.AppendLine($@"          <span style=""display: inline-block; background: {colors.PrimaryColor}; color: {colors.AccentColor}; padding: 8px 18px; border-radius: 30px; font-size: 13px; font-weight: 500; white-space: nowrap; letter-spacing: 0.2px;"">{Encode(item)}</span>");

        sb.AppendLine(@"        </div>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateRatingSnippetBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var scoreStr = lines.Length > 0 ? lines[0].Trim() : "5";
        var subtitle = lines.Length > 1 ? string.Join(" ", lines.Skip(1)).Trim() : "";

        double.TryParse(scoreStr.Replace(',', '.'),
            System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out double score);
        score = Math.Clamp(score, 0, 5);

        var stars = RenderStars(score, colors.AccentColor);
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px; text-align: center;"">
        {titleHtml}
        <div style=""font-size: 36px; letter-spacing: 6px; margin-bottom: 10px; line-height: 1;"">{stars}</div>
        <div style=""font-size: 42px; font-weight: 700; color: #1a1a1a; line-height: 1; margin-bottom: 4px;"">{Encode(scoreStr)}</div>
        <div style=""font-size: 14px; color: #888; margin-bottom: {(string.IsNullOrEmpty(subtitle) ? "0" : "6px")};"">von 5 Sternen</div>
        {(string.IsNullOrEmpty(subtitle) ? "" : $"<div style=\"font-size: 14px; color: #666;\">{Encode(subtitle)}</div>")}
      </td>
    </tr>
  </table>
";
    }

    private static string RenderStars(double score, string accentColor)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= 5; i++)
        {
            if (score >= i - 0.25)
                sb.Append($"<span style=\"color: {accentColor};\">★</span>");
            else if (score >= i - 0.75)
                sb.Append($"<span style=\"color: {accentColor}; opacity: 0.4;\">★</span>");
            else
                sb.Append("<span style=\"color: #ccc;\">★</span>");
        }
        return sb.ToString();
    }

    private string GenerateGalleryBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var urls = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToList();
        if (urls.Count == 0) return string.Empty;

        var columns = Math.Max(1, Math.Min(4, block.Options.Columns));
        var widthPercent = 100 / columns;
        var titleHtml = BlockTitleHtml(block, colors);

        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");

        for (int i = 0; i < urls.Count; i++)
        {
            bool rowStart = i % columns == 0;
            bool rowEnd = (i + 1) % columns == 0 || i == urls.Count - 1;

            if (rowStart) sb.AppendLine("          <tr>");

            sb.AppendLine($@"            <td width=""{widthPercent}%"" style=""padding: 4px; vertical-align: top;"">
              <img src=""{Encode(urls[i])}"" alt=""Galerie-Bild"" style=""width: 100%; height: auto; display: block; border-radius: 4px;"">
            </td>");

            if (rowEnd)
            {
                var remaining = columns - (i % columns) - 1;
                for (int j = 0; j < remaining; j++)
                    sb.AppendLine($"            <td width=\"{widthPercent}%\" style=\"padding: 4px;\"></td>");
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

    private string GenerateLinkListBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) return string.Empty;

        var titleHtml = BlockTitleHtml(block, colors);
        var sb = new StringBuilder();
        sb.AppendLine($@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 25px 30px;"">
        {titleHtml}
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border: 1px solid #e0e0e0; border-radius: 6px; overflow: hidden;"">");

        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', 2);
            var label = parts[0].Trim();
            var url = parts.Length > 1 ? parts[1].Trim() : "#";
            var borderStyle = i < lines.Length - 1 ? "border-bottom: 1px solid #eee;" : "";
            var bg = i % 2 == 1 ? " background: #fafafa;" : "";

            sb.AppendLine($@"          <tr>
            <td style=""padding: 13px 16px; {borderStyle}{bg}"">
              <a href=""{Encode(url)}"" target=""_blank"" rel=""noopener noreferrer""
                 style=""color: #0071c5; text-decoration: none; font-size: 15px; display: flex; align-items: center; gap: 10px;"">
                <span style=""color: {colors.AccentColor}; font-size: 18px; line-height: 1; flex-shrink: 0;"">→</span>
                {Encode(label)}
              </a>
            </td>
          </tr>");
        }

        sb.AppendLine(@"        </table>
      </td>
    </tr>
  </table>
");
        return sb.ToString();
    }

    private string GenerateHeroBannerBlock(BlockDefinition block, string content, ColorScheme colors, int blockIndex)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var imageUrl = lines.Length > 0 ? lines[0].Trim() : "";
        var title    = lines.Length > 1 ? lines[1].Trim() : "";
        var subtitle = lines.Length > 2 ? lines[2].Trim() : "";
        if (string.IsNullOrEmpty(imageUrl)) return string.Empty;

        var textAlign = block.Options.Alignment switch { "left" => "left", "right" => "right", _ => "center" };
        var opacity   = Math.Clamp(block.Options.OverlayOpacity, 0, 90) / 100.0;
        var overlayBg = $"rgba(0,0,0,{opacity:F2})";
        var titleHtml = BlockTitleHtml(block, colors);

        return $@"  <!-- {block.Title} -->
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background: {BlockBackground(blockIndex, colors)}; border-left: 1px solid #e0e0e0; border-right: 1px solid #e0e0e0;"">
    <tr>
      <td style=""padding: 0;"">
        {(block.Options.ShowTitle ? $"<div style=\"padding: 20px 30px 0;\">{titleHtml}</div>" : "")}
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
               style=""background-image: url('{Encode(imageUrl)}'); background-size: cover; background-position: center; min-height: 240px;"">
          <tr>
            <td style=""padding: 60px 40px; background: {overlayBg}; text-align: {textAlign};"">
              {(string.IsNullOrEmpty(title) ? "" : $"<div style=\"font-size: 30px; font-weight: 700; color: #ffffff; margin-bottom: 10px; text-shadow: 0 2px 6px rgba(0,0,0,0.5); line-height: 1.2;\">{Encode(title)}</div>")}
              {(string.IsNullOrEmpty(subtitle) ? "" : $"<div style=\"font-size: 16px; color: rgba(255,255,255,0.9); text-shadow: 0 1px 4px rgba(0,0,0,0.5);\">{Encode(subtitle)}</div>")}
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
";
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

    private static string BlockTitleHtml(BlockDefinition block, ColorScheme colors)
    {
        if (!block.Options.ShowTitle) return string.Empty;
        var iconHtml = IconHelper.Has(block.Icon)
            ? IconHelper.SvgForHtml(block.Icon, colors.AccentColor, 18)
            : $"<span style=\"margin-right:6px\">{Encode(block.Icon)}</span>";
        return $@"
        <div style=""margin: 0 0 20px 0; font-size: 18px; color: #1a1a1a; border-bottom: 3px solid {colors.AccentColor}; padding-bottom: 10px; display: inline-block; font-weight: 600;"">
          {iconHtml}{Encode(block.Title)}
        </div>";
    }

    public static string SyntaxHighlightHtml(string html)
    {
        var escaped = System.Web.HttpUtility.HtmlEncode(html);

        // Comments first (greedy match before tags)
        escaped = Regex.Replace(escaped, @"&lt;!--.*?--&gt;",
            m => $"<span class=\"code-comment\">{m.Value}</span>",
            RegexOptions.Singleline);

        // Tags: opening/closing/self-closing
        escaped = Regex.Replace(escaped,
            @"(&lt;/?)([a-zA-Z][a-zA-Z0-9]*)((?:[^&]|&(?!gt;|lt;))*?)(/?)(&gt;)",
            m =>
            {
                var slash    = m.Groups[1].Value;
                var tag      = m.Groups[2].Value;
                var attrs    = m.Groups[3].Value;
                var selfClose = m.Groups[4].Value;
                var close    = m.Groups[5].Value;

                // Highlight attribute names and quoted values inside attrs
                var attrHighlighted = Regex.Replace(attrs,
                    @"([a-zA-Z_:][a-zA-Z0-9_:\-]*)(\s*=\s*)(""[^""]*""|'[^']*')",
                    a => $"<span class=\"code-attr\">{a.Groups[1].Value}</span>{a.Groups[2].Value}<span class=\"code-string\">{a.Groups[3].Value}</span>");

                return $"{slash}<span class=\"code-tag\">{tag}</span>{attrHighlighted}{selfClose}{close}";
            });

        return escaped;
    }

    private static string BlockBackground(int blockIndex, ColorScheme colors)
        => blockIndex % 2 == 0 ? "#ffffff" : colors.BackgroundColor;

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

    public string GenerateJsonSchema(LayoutTemplate layout)
    {
        var blockSchemas = layout.Blocks
            .Where(b => b.Type != BlockType.FixedText)
            .OrderBy(b => b.Order)
            .Select(b =>
            {
                var placeholder = b.Type.GetInputPlaceholder().Replace("\n", "\\n").Replace("\"", "\\\"");
                var formatHint = b.Type.GetInputLabel();
                return $@"    ""{b.Id}"": ""[{b.Title}] {formatHint}: {placeholder}""";
            });

        return $$"""
            {
              "_schema": "Artikeldaten für relexx' Template Generator",
              "_layoutId": "{{layout.Id}}",
              "_layoutName": "{{layout.Name}}",
              "_hint": "Dieses JSON kann direkt in den Template Generator importiert werden. Ersetze alle Werte mit den echten Produktdaten. Behalte das Format (z.B. 'Key | Value' für Tabellen, eine Zeile pro Eintrag). Entferne die [Blocktitel]-Hinweise beim Ausfüllen - sie dienen nur zur Orientierung. Biete das Ergebnis als Datei zum Herunterladen an, mit dem Dateinamen [Produkt-Namen-mit-Bindestrichen]_[YYYY-mm-DD].json .",

              "Title": "Produktname (Hauptüberschrift)",
              "Subtitle": "Kurzbeschreibung | Artikelnummer",
              "MobileSummary": "Max. {{Constants.Limits.MaxMobileSummaryLength}} Zeichen für mobile Vorschau und Google Shopping",
              "Footer": "SKU: ABC123\nFarbe: Schwarz\nHerstellergarantie: 2 Jahre (max. {{Constants.Limits.MaxFooterLines}} Zeilen)",

              "Layout": {
                "Id": "{{layout.Id}}",
                "Name": "{{layout.Name}}"
              },

              "BlockContents": {
            {{string.Join(",\n", blockSchemas)}}
              }
            }
            """;
    }
}
