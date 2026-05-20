using System.Text.Json;
using EbayTemplateGenerator.Models;
using EbayTemplateGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace EbayTemplateGenerator.Pages;

public partial class Index
{
    [Inject] private TemplateGeneratorService TemplateGenerator { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ═══════════════ STATE ═══════════════
    private int currentPhase;
    private List<LayoutTemplate> layouts = new();
    private string selectedLayoutId = LayoutTemplate.StandardLayoutId;
    private LayoutTemplate currentLayout = LayoutTemplate.CreateStandard();
    private string? selectedBlockId;
    private BlockDefinition? SelectedBlock => currentLayout.Blocks.FirstOrDefault(b => b.Id == selectedBlockId);

    private ArticleData article = new();
    private string generatedHtml = string.Empty;

    private bool showAddBlockDialog;
    private bool showImportConflict;
    private ArticleData? importedArticle;

    private string notification = string.Empty;
    private bool notificationSuccess;
    private CancellationTokenSource? notificationCts;

    private DotNetObjectReference<Index>? dotNetHelper;
    private bool _needsSortableInit;

    // ═══════════════ LIFECYCLE ═══════════════
    protected override async Task OnInitializedAsync()
    {
        await LoadLayouts();
        await LoadArticle();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (currentPhase == 0 && (firstRender || _needsSortableInit))
        {
            _needsSortableInit = false;
            await InitSortable();
        }
    }

    public async ValueTask DisposeAsync()
    {
        notificationCts?.Cancel();
        notificationCts?.Dispose();
        try { await JS.InvokeVoidAsync("destroySortable", "block-list"); }
        catch { }
        dotNetHelper?.Dispose();
    }

    // ═══════════════ SORTABLE ═══════════════
    private async Task InitSortable()
    {
        dotNetHelper ??= DotNetObjectReference.Create(this);
        try { await JS.InvokeVoidAsync("initSortable", "block-list", dotNetHelper); }
        catch { }
    }

    [JSInvokable]
    public async Task OnBlockReordered(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex) return;

        var orderedBlocks = currentLayout.Blocks.OrderBy(b => b.Order).ToList();
        var movedBlock = orderedBlocks[oldIndex];
        orderedBlocks.RemoveAt(oldIndex);
        orderedBlocks.Insert(newIndex, movedBlock);

        for (var i = 0; i < orderedBlocks.Count; i++)
            orderedBlocks[i].Order = i;

        currentLayout.Blocks = orderedBlocks;
        await SaveLayouts();
        StateHasChanged();
    }

    // ═══════════════ LAYOUT MANAGEMENT ═══════════════
    private async Task LoadLayouts()
    {
        try
        {
            var saved = await LocalStorage.GetItemAsync<List<LayoutTemplate>>(Constants.Storage.LayoutsKey);
            layouts = saved ?? new();

            if (!layouts.Exists(l => l.IsDefault))
                layouts.Insert(0, LayoutTemplate.CreateStandard());

            currentLayout = layouts.Find(l => l.Id == selectedLayoutId) ?? layouts[0];
            selectedLayoutId = currentLayout.Id;
        }
        catch
        {
            layouts = new() { LayoutTemplate.CreateStandard() };
            currentLayout = layouts[0];
        }
    }

    private async Task SaveLayouts()
    {
        currentLayout.ModifiedAt = DateTime.UtcNow;
        await LocalStorage.SetItemAsync(Constants.Storage.LayoutsKey, layouts);
    }

    private async Task OnLayoutSelected()
    {
        currentLayout = layouts.Find(l => l.Id == selectedLayoutId) ?? layouts[0];
        selectedBlockId = null;
        article.Layout = currentLayout;
        PrefillFixedTextBlocks();
        await InitSortable();
    }

    private async Task CreateNewLayout()
    {
        var newLayout = new LayoutTemplate
        {
            Name = "Neues Layout",
            Blocks = new()
            {
                BlockDefinition.CreateDefault(BlockType.Image),
                BlockDefinition.CreateDefault(BlockType.RichText, 1)
            }
        };
        layouts.Add(newLayout);
        selectedLayoutId = newLayout.Id;
        currentLayout = newLayout;
        selectedBlockId = null;
        await SaveLayouts();
    }

    private async Task DuplicateLayout()
    {
        var copy = currentLayout.Clone();
        layouts.Add(copy);
        selectedLayoutId = copy.Id;
        currentLayout = copy;
        selectedBlockId = null;
        await SaveLayouts();
    }

    private async Task DeleteLayout()
    {
        if (currentLayout.IsDefault || layouts.Count <= 1) return;
        layouts.Remove(currentLayout);
        currentLayout = layouts[0];
        selectedLayoutId = currentLayout.Id;
        selectedBlockId = null;
        await SaveLayouts();
    }

    private async Task ExportLayout()
    {
        var json = JsonSerializer.Serialize(currentLayout, Helpers.JsonOptions);
        var fileName = $"layout-{currentLayout.Name}_{DateTime.UtcNow:yyyy-MM-dd_HHmm}.json";
        await JS.InvokeVoidAsync("downloadFile", fileName, "application/json", json);
    }

    private async Task ImportLayout(InputFileChangeEventArgs e)
    {
        try
        {
            await using var stream = e.File.OpenReadStream();
            var imported = await JsonSerializer.DeserializeAsync<LayoutTemplate>(stream);

            if (imported is null) return;

            imported.Id = Helpers.GenerateShortId();
            imported.IsDefault = false;
            layouts.Add(imported);
            selectedLayoutId = imported.Id;
            currentLayout = imported;
            await SaveLayouts();
            await ShowNotification("✓ Layout importiert!", true);
        }
        catch (Exception ex)
        {
            await ShowNotification($"Fehler: {ex.Message}", false);
        }
    }

    // ═══════════════ BLOCK MANAGEMENT ═══════════════
    private void SelectBlock(BlockDefinition block) => selectedBlockId = block.Id;

    private async Task AddBlock(BlockType type)
    {
        var order = currentLayout.Blocks.Count > 0 ? currentLayout.Blocks.Max(b => b.Order) + 1 : 0;
        var block = BlockDefinition.CreateDefault(type, order);
        currentLayout.Blocks.Add(block);
        selectedBlockId = block.Id;
        showAddBlockDialog = false;

        if (type == BlockType.FixedText)
            article.SetBlockContent(block.Id, block.Options.FixedContent);

        await SaveLayouts();
        await InitSortable();
    }

    private async Task DeleteBlock()
    {
        if (SelectedBlock is null) return;
        currentLayout.Blocks.Remove(SelectedBlock);
        currentLayout.ReorderBlocks();
        selectedBlockId = null;
        await SaveLayouts();
        await InitSortable();
    }

    private async Task OnBlockTypeChanged()
    {
        if (SelectedBlock is null) return;
        SelectedBlock.Options = BlockOptions.CreateDefault(SelectedBlock.Type);
        SelectedBlock.Icon = SelectedBlock.Type.GetDefaultIcon();
        SelectedBlock.Title = SelectedBlock.Type.GetDefaultTitle();
        await SaveLayouts();
    }

    private async Task OnFixedContentChanged(BlockDefinition block)
    {
        article.SetBlockContent(block.Id, block.Options.FixedContent);
        await SaveLayouts();
    }

    private async Task SetBlockIcon(string icon)
    {
        if (SelectedBlock is not null)
        {
            SelectedBlock.Icon = icon;
            await SaveLayouts();
        }
    }

    private async Task SetBullet(string bullet)
    {
        if (SelectedBlock is not null)
        {
            SelectedBlock.Options.BulletChar = bullet;
            await SaveLayouts();
        }
    }

    // ═══════════════ ARTICLE MANAGEMENT ═══════════════
    private async Task LoadArticle()
    {
        try
        {
            var saved = await LocalStorage.GetItemAsync<ArticleData>(Constants.Storage.ArticleKey);
            if (saved is not null)
            {
                article = saved;
                if (article.Layout?.Id is not null && layouts.Exists(l => l.Id == article.Layout.Id))
                {
                    selectedLayoutId = article.Layout.Id;
                    currentLayout = layouts.First(l => l.Id == article.Layout.Id);
                }
            }
            else
            {
                article = new() { Layout = currentLayout };
            }
        }
        catch
        {
            article = new() { Layout = currentLayout };
        }
        PrefillFixedTextBlocks();
    }

    private async Task SaveArticle()
    {
        article.Layout = currentLayout;
        article.ModifiedAt = DateTime.UtcNow;
        await LocalStorage.SetItemAsync(Constants.Storage.ArticleKey, article);
    }

    private void LoadDemo()
    {
        article = new ArticleData
        {
            Title = "Premium Produkt XYZ-3000",
            Subtitle = "Hochwertige Qualität | Art.-Nr. XYZ-3000-BK",
            MobileSummary = "Das Premium Produkt XYZ-3000 überzeugt durch erstklassige Verarbeitung und innovative Features. Perfekt geeignet für anspruchsvolle Anwender, die Wert auf Qualität legen.",
            Footer = "SKU: XYZ-3000-BK\nFarbe: Schwarz\nGarantie: 2 Jahre",
            Layout = currentLayout
        };

        foreach (var block in currentLayout.Blocks)
        {
            var content = block.Type == BlockType.FixedText
                ? block.Options.FixedContent
                : block.Type.GetDemoContent();
            article.SetBlockContent(block.Id, content);
        }
    }

    private void ResetArticle()
    {
        article = new() { Layout = currentLayout };
        PrefillFixedTextBlocks();
    }

    private void SetContent(string blockId, ChangeEventArgs e)
        => article.SetBlockContent(blockId, e.Value?.ToString() ?? string.Empty);

    private void ClearBlock(string blockId)
    {
        var block = currentLayout.Blocks.FirstOrDefault(b => b.Id == blockId);
        if (block?.Type == BlockType.FixedText)
            article.SetBlockContent(blockId, block.Options.FixedContent);
        else
            article.ClearBlockContent(blockId);
    }

    private void PrefillFixedTextBlocks()
    {
        foreach (var block in currentLayout.Blocks.Where(b => b.Type == BlockType.FixedText))
        {
            if (string.IsNullOrEmpty(article.GetBlockContent(block.Id)))
                article.SetBlockContent(block.Id, block.Options.FixedContent);
        }
    }

    private async Task UploadImage(InputFileChangeEventArgs e, string blockId)
    {
        try
        {
            var file = e.File;
            if (file.Size > Constants.FileLimits.MaxImageSizeBytes)
            {
                await ShowNotification("Bild zu groß (max. 5 MB)", false);
                return;
            }
            await using var stream = file.OpenReadStream(Constants.FileLimits.MaxImageSizeBytes);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            article.SetBlockContent(blockId, $"data:{file.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await ShowNotification($"Upload fehlgeschlagen: {ex.Message}", false);
        }
    }

    private async Task ExportArticle()
    {
        article.Layout = currentLayout;
        var json = JsonSerializer.Serialize(article, Helpers.JsonOptions);
        await JS.InvokeVoidAsync("downloadFile", article.GenerateFileName(), "application/json", json);
    }

    private async Task ImportArticle(InputFileChangeEventArgs e)
    {
        try
        {
            await using var stream = e.File.OpenReadStream(Constants.FileLimits.MaxImportSizeBytes);
            importedArticle = await JsonSerializer.DeserializeAsync<ArticleData>(stream);

            if (importedArticle is null) return;

            var layoutIdMatches = importedArticle.Layout?.Id == currentLayout.Id;
            var isCompatible = currentLayout.IsCompatibleWith(importedArticle.Layout);

            if (layoutIdMatches || isCompatible)
            {
                article = importedArticle;
                article.Layout = currentLayout;
                PrefillFixedTextBlocks();
                await ShowNotification("✓ Artikel importiert!", true);
            }
            else
            {
                showImportConflict = true;
            }
        }
        catch (Exception ex)
        {
            await ShowNotification($"Import fehlgeschlagen: {ex.Message}", false);
        }
    }

    private async Task ImportWithNewLayout()
    {
        if (importedArticle?.Layout is null) return;

        if (!layouts.Exists(l => l.Id == importedArticle.Layout.Id))
        {
            importedArticle.Layout.IsDefault = false;
            layouts.Add(importedArticle.Layout);
            await SaveLayouts();
        }

        selectedLayoutId = importedArticle.Layout.Id;
        currentLayout = importedArticle.Layout;
        article = importedArticle;
        PrefillFixedTextBlocks();
        showImportConflict = false;
        await ShowNotification("✓ Artikel mit Layout importiert!", true);
    }

    private async Task ImportKeepLayout()
    {
        if (importedArticle is null) return;

        article.Title = importedArticle.Title;
        article.Subtitle = importedArticle.Subtitle;
        article.MobileSummary = importedArticle.MobileSummary;
        article.Footer = importedArticle.Footer;

        foreach (var block in currentLayout.Blocks)
        {
            if (importedArticle.BlockContents.TryGetValue(block.Id, out var content))
                article.SetBlockContent(block.Id, content);
        }

        PrefillFixedTextBlocks();
        showImportConflict = false;
        await ShowNotification("✓ Daten importiert", true);
    }

    private async Task CopyJsonSchema()
    {
        var schema = TemplateGenerator.GenerateJsonSchema(currentLayout);
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", schema);
        await ShowNotification("✓ JSON-Schema kopiert!", true);
    }

    // ═══════════════ NAVIGATION & HTML ═══════════════
    private async Task GoToPhase(int phase)
    {
        if (phase >= 2)
        {
            article.Layout = currentLayout;
            generatedHtml = TemplateGenerator.GenerateHtml(article);
        }

        currentPhase = phase;
        await SaveArticle();

        if (phase == 0)
            _needsSortableInit = true;
    }

    private async Task CopyHtml()
    {
        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", generatedHtml);
            await ShowNotification("✓ HTML kopiert!", true);
        }
        catch
        {
            await ShowNotification("Kopieren fehlgeschlagen", false);
        }
    }

    private async Task DownloadHtml()
        => await JS.InvokeVoidAsync("downloadFile", article.GenerateFileName("html"), "text/html", generatedHtml);

    // ═══════════════ HELPERS ═══════════════
    private string GetPhaseClass(int phase) => currentPhase == phase ? "active" : currentPhase > phase ? "completed" : "";

    private static string GetPhaseName(int phase) => phase switch
    {
        0 => "Layout",
        1 => "Eingabe",
        2 => "Vorschau",
        _ => "HTML"
    };

    private int GetFooterLines() => string.IsNullOrWhiteSpace(article.Footer)
        ? 0
        : article.Footer.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;

    private static int GetTextareaRows(BlockType type) => type switch
    {
        BlockType.RichText => 6,
        BlockType.KeyValueGrid => 7,
        BlockType.DataTable => 8,
        BlockType.FeatureCards => 3,
        BlockType.CheckList => 5,
        _ => 4
    };

    private async Task TriggerImagePicker(string blockId)
        => await JS.InvokeVoidAsync("triggerClick", $"img-{blockId}");

    private async Task ShowNotification(string msg, bool success)
    {
        notificationCts?.Cancel();
        notificationCts = new CancellationTokenSource();

        notification = msg;
        notificationSuccess = success;
        StateHasChanged();

        try
        {
            await Task.Delay(Constants.Timing.NotificationDurationMs, notificationCts.Token);
            notification = string.Empty;
            StateHasChanged();
        }
        catch (TaskCanceledException) { }
    }
}
