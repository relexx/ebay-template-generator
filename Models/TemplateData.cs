namespace EbayTemplateGenerator.Models;

public class TemplateData
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string MobileSummary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Highlights { get; set; } = string.Empty;
    public string TechnicalData { get; set; } = string.Empty;
    public string Compatibility { get; set; } = string.Empty;
    public string DeliveryScope { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;
    
    // Color settings
    public string PrimaryColor { get; set; } = "#1a1a1a";
    public string AccentColor { get; set; } = "#f5c518";
    public string BackgroundColor { get; set; } = "#f8f9fa";
    
    public static TemplateData GetDemoData()
    {
        return new TemplateData
        {
            Title = "CORSAIR iCUE LINK TITAN 360 RX RGB",
            Subtitle = "AIO-Wasserkühlung │ 360mm Radiator │ CW-9061018-WW",
            ImageUrl = "https://assets.corsair.com/image/upload/c_pad,q_85,h_1100,w_1100,f_auto/products/Liquid-Cooling/titan-rx-rgb/Gallery/CW-9061018-WW/CW-9061018-WW_01.webp",
            MobileSummary = "Corsair iCUE LINK TITAN 360 RX RGB - Premium AIO-Wasserkühlung mit 360mm Radiator. FlowDrive-Pumpe mit leisem Dreiphasenmotor, 3x RX120 RGB-Lüfter mit 44 RGB-LEDs. iCUE LINK für einfaches Kabelmanagement. Kompatibel mit Intel LGA 1851/1700 und AMD AM5/AM4. Inkl. XTM70-Wärmeleitpaste und 6 Jahre Garantie.",
            Description = @"Der **Corsair iCUE LINK TITAN 360 RX RGB** ist eine leistungsstarke All-in-One-Wasserkühlung für CPUs, die extreme Kühlleistung mit flüsterleisem Betrieb und spektakulärer RGB-Beleuchtung vereint.

Die hauseigene **FlowDrive-Kühl-Engine** mit Dreiphasenmotor sorgt für eine hohe Durchflussrate bei minimalem Geräuschpegel. Die präzisionsgefertigte Kupfer-Kühlplatte garantiert maximalen Kontakt mit dem CPU-Heatspreader für optimale Wärmeableitung.",
            Highlights = @"FlowDrive-Pumpe | Leiser Dreiphasenmotor für optimale Wärmeeffizienz
3× RX120 RGB-Lüfter | 120mm Lüfter mit hohem Luftdurchsatz
44 RGB-LEDs | 20 an der Pumpe + 8 pro Lüfter
iCUE LINK | Ein Kabel, ein Anschluss – vereinfachtes Kabelmanagement
CapSwap-Design | Modulares System für optionale Erweiterungen
XTM70-Wärmeleitpaste | Hochleistungs-Paste vorinstalliert
6 Jahre Herstellergarantie | Erstklassiger Support von Corsair",
            TechnicalData = @"Radiatorgröße | 360mm (396 × 120 × 27 mm)
Lüfter | 3× 120mm RX120 RGB
Lüfterdrehzahl | 300 – 2.100 U/min (±10%)
Luftdurchsatz | 10,4 – 73,5 CFM
Statischer Druck | 0,12 – 4,33 mm-H₂O
Geräuschpegel | 10 – 36 dBA
Schlauchlänge | 450 mm (ummantelt)
Radiatormaterial | Aluminium
Kühlplattenmaterial | Kupfer
Gewicht | 2.515 g",
            Compatibility = @"Intel | LGA 1851 │ LGA 1700
AMD | AM5 │ AM4",
            DeliveryScope = @"iCUE LINK TITAN 360 RX RGB Wasserkühlung
3× iCUE LINK RX120 RGB Lüfter (vorinstalliert)
iCUE LINK System Hub
Montagematerial für Intel & AMD
XTM70-Wärmeleitpaste (voraufgetragen)
Dokumentation",
            Footer = @"SKU: CW-9061018-WW
Farbe: Schwarz
Garantie: 6 Jahre",
            PrimaryColor = "#1a1a1a",
            AccentColor = "#f5c518",
            BackgroundColor = "#f8f9fa"
        };
    }
}
