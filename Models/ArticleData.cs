using System.Text.RegularExpressions;

namespace EbayTemplateGenerator.Models;

/// <summary>
/// Artikeldaten mit eingebettetem Layout für maximale Portabilität
/// </summary>
public class ArticleData
{
    // === Header-Felder (immer gleich strukturiert) ===
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string MobileSummary { get; set; } = string.Empty;
    
    // === Footer (max 4 Zeilen, dynamisch) ===
    public string Footer { get; set; } = string.Empty;
    
    // === Block-Inhalte (Key = BlockDefinition.Id, Value = Inhalt) ===
    public Dictionary<string, string> BlockContents { get; set; } = new();
    
    // === Eingebettetes Layout für Portabilität ===
    public LayoutTemplate Layout { get; set; } = LayoutTemplate.CreateStandard();
    
    // === Metadaten ===
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Holt den Inhalt eines Blocks oder leeren String
    /// </summary>
    public string GetBlockContent(string blockId)
    {
        return BlockContents.TryGetValue(blockId, out var content) ? content : string.Empty;
    }
    
    /// <summary>
    /// Setzt den Inhalt eines Blocks
    /// </summary>
    public void SetBlockContent(string blockId, string content)
    {
        BlockContents[blockId] = content;
        ModifiedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Löscht den Inhalt eines Blocks
    /// </summary>
    public void ClearBlockContent(string blockId)
    {
        BlockContents.Remove(blockId);
        ModifiedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Generiert einen sicheren Dateinamen aus Titel und Zeitstempel
    /// </summary>
    public string GenerateFileName(string extension = "json")
    {
        var safeName = string.IsNullOrWhiteSpace(Title) 
            ? "artikel" 
            : Regex.Replace(Title, @"[^\w\-äöüÄÖÜß]", "-", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        
        // Mehrfache Bindestriche entfernen und kürzen
        safeName = Regex.Replace(safeName, @"-+", "-", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Trim('-');
        if (safeName.Length > 50) safeName = safeName[..50].TrimEnd('-');
        
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
        return $"{safeName}_{timestamp}.{extension}";
    }
    
    /// <summary>
    /// Generiert ein dynamisches JSON-Schema basierend auf dem Layout (für KI-Befüllung)
    /// </summary>
    public static string GenerateJsonSchema(LayoutTemplate layout)
    {
        var blockSchemas = layout.Blocks
            .OrderBy(b => b.Order)
            .Select(b => $@"    ""{b.Id}"": ""{b.Type.GetInputPlaceholder().Replace("\n", "\\n").Replace("\"", "\\\"")}""")
            .ToList();
        
        return $@"{{
  ""_schema"": ""Artikeldaten für relexx' Template Generator"",
  ""_layout"": ""{layout.Name}"",
  ""_hint"": ""Fülle alle Felder aus. Das Ergebnis kann direkt importiert werden."",
  
  ""Title"": ""Produktname (Hauptüberschrift)"",
  ""Subtitle"": ""Kurzbeschreibung │ Artikelnummer"",
  ""MobileSummary"": ""Max. 800 Zeichen für mobile Vorschau und Google Shopping"",
  ""Footer"": ""SKU: ABC123\\nFarbe: Schwarz\\nGarantie: 2 Jahre (max. 4 Zeilen)"",
  
  ""BlockContents"": {{
{string.Join(",\n", blockSchemas)}
  }}
}}";
    }
    
    /// <summary>
    /// Demo-Daten für das Standard-Layout
    /// </summary>
    public static ArticleData CreateDemo()
    {
        var layout = LayoutTemplate.CreateStandard();
        var data = new ArticleData
        {
            Title = "CORSAIR iCUE LINK TITAN 360 RX RGB",
            Subtitle = "AIO-Wasserkühlung │ 360mm Radiator │ CW-9061018-WW",
            MobileSummary = "Corsair iCUE LINK TITAN 360 RX RGB - Premium AIO-Wasserkühlung mit 360mm Radiator. FlowDrive-Pumpe mit leisem Dreiphasenmotor, 3x RX120 RGB-Lüfter mit 44 RGB-LEDs. iCUE LINK für einfaches Kabelmanagement. Kompatibel mit Intel LGA 1851/1700 und AMD AM5/AM4.",
            Footer = "SKU: CW-9061018-WW\nFarbe: Schwarz\nGarantie: 6 Jahre",
            Layout = layout
        };
        
        data.BlockContents["img"] = "https://assets.corsair.com/image/upload/c_pad,q_85,h_1100,w_1100,f_auto/products/Liquid-Cooling/titan-rx-rgb/Gallery/CW-9061018-WW/CW-9061018-WW_01.webp";
        
        data.BlockContents["desc"] = @"Der **Corsair iCUE LINK TITAN 360 RX RGB** ist eine leistungsstarke All-in-One-Wasserkühlung für CPUs, die extreme Kühlleistung mit flüsterleisem Betrieb und spektakulärer RGB-Beleuchtung vereint.

Die hauseigene **FlowDrive-Kühl-Engine** mit Dreiphasenmotor sorgt für eine hohe Durchflussrate bei minimalem Geräuschpegel. Drei RX120 RGB-Lüfter mit magnetischer Schwebetechnik liefern optimierten Luftdurchsatz für erstklassige Kühlung.";
        
        data.BlockContents["highlights"] = @"FlowDrive-Pumpe | Leiser Dreiphasenmotor für optimale Wärmeeffizienz
3× RX120 RGB-Lüfter | 120mm Lüfter mit hohem Luftdurchsatz
44 RGB-LEDs | 20 an der Pumpe + 8 pro Lüfter
iCUE LINK | Ein Kabel, ein Anschluss – vereinfachtes Kabelmanagement
XTM70-Wärmeleitpaste | Hochleistungs-Paste vorinstalliert
6 Jahre Garantie | Erstklassiger Support von Corsair";
        
        data.BlockContents["specs"] = @"Radiatorgröße | 360mm (396 × 120 × 27 mm)
Lüfter | 3× 120mm RX120 RGB
Lüfterdrehzahl | 300 – 2.100 U/min (±10%)
Luftdurchsatz | 10,4 – 73,5 CFM
Geräuschpegel | 10 – 36 dBA
Schlauchlänge | 450 mm
Gewicht | 2.515 g";
        
        data.BlockContents["compat"] = @"Intel | LGA 1851 │ LGA 1700
AMD | AM5 │ AM4";
        
        data.BlockContents["scope"] = @"iCUE LINK TITAN 360 RX RGB Wasserkühlung
3× iCUE LINK RX120 RGB Lüfter (vorinstalliert)
iCUE LINK System Hub
Montagematerial für Intel & AMD
XTM70-Wärmeleitpaste (voraufgetragen)
Dokumentation";
        
        return data;
    }
}
