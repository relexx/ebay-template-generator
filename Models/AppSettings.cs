namespace EbayTemplateGenerator.Models;

public record AppSettings(
    string Theme = "dark",
    string Density = "comfortable",
    string AccentPreset = "Gelb"
);
