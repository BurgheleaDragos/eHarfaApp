using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class SongPage : ComponentBase, IAsyncDisposable
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private ISongService SongService { get; set; } = null!;

    [Inject]
    private IPdfExportService PdfExportService { get; set; } = null!;

    [Inject]
    private IJSRuntime JS { get; set; } = null!;

    [Parameter]
    public string Id { get; set; } = string.Empty;

    private Song? Song { get; set; }
    private double _fontSize = 1.0;
    private string ContentStyle => $"font-size:{_fontSize.ToString(System.Globalization.CultureInfo.InvariantCulture)}rem;";
    private DotNetObjectReference<SongPage>? _dotNetRef;
    private MarkupString FormattedContent =>
        new(FormatSongContent(Song?.Content));

    protected override async Task OnParametersSetAsync()
    {
        Song = await GetSongByIdAsync(Id);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("eHarfa.initZoom", "song-content", _dotNetRef);
        }
    }

    [JSInvokable]
    public void OnFontSizeChanged(double fontSize)
    {
        _fontSize = fontSize;
        StateHasChanged();
    }

    private async Task<Song?> GetSongByIdAsync(string id)
    {
        try
        {
            return await SongService.GetSongByIdFromDatabaseAsync(id);
        }
        catch(OperationCanceledException) { }
        catch (Exception)
        {
            return null;
        }
        
        return null;
    }

    private Task AddToFavourites(MouseEventArgs arg)
    {
        Snackbar.Add("Added to favourites", Severity.Info);
        return Task.CompletedTask;
    }

    private async Task ExportPDF(MouseEventArgs arg)
    {
        if (Song == null)
        {
            Snackbar.Add("Cântarea nu a putut fi exportată.", Severity.Error);
            return;
        }

        try
        {
            await PdfExportService.ExportSongAsync(Song);
            Snackbar.Add("PDF-ul a fost generat.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Exportul PDF a eșuat: {ex.Message}", Severity.Error);
        }
    }

    private Task ShareSong(MouseEventArgs arg)
    {
        Snackbar.Add("Share Song", Severity.Info);
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        return ValueTask.CompletedTask;
    }

    private static string FormatSongContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        return content
            .Replace("\\r\\n", "\n")
            .Replace("\\n", "\n")
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\n", "<br />");
    }
}
