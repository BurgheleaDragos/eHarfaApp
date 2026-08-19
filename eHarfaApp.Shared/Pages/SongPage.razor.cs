using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class SongPage : ComponentBase, IAsyncDisposable
{
    private const double MinFontSize = 0.7;
    private const double MaxFontSize = 2.5;

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
            await JS.InvokeVoidAsync(
                "eHarfa.initSongGestures",
                "song-content",
                GetSongTextForClipboard(),
                _dotNetRef);
        }
    }

    [JSInvokable]
    public void OnFontSizeChanged(double fontSize)
    {
        _fontSize = Math.Clamp(fontSize, MinFontSize, MaxFontSize);
        StateHasChanged();
    }

    [JSInvokable]
    public void OnSongCopied(bool copied)
    {
        Snackbar.Add(
            copied ? "Cântarea a fost copiată în clipboard." : "Copierea nu este disponibilă pe acest dispozitiv.",
            copied ? Severity.Success : Severity.Error);
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

    private async Task ShareSong(MouseEventArgs arg)
    {
        if (Song == null)
        {
            Snackbar.Add("Cântarea nu a putut fi distribuită.", Severity.Error);
            return;
        }

        try
        {
            var result = await PdfExportService.ShareSongAsync(Song);
            switch (result)
            {
                case ShareResult.Shared:
                    Snackbar.Add("Cântarea a fost distribuită.", Severity.Success);
                    break;
                case ShareResult.Downloaded:
                    Snackbar.Add("Distribuirea directă nu este disponibilă în acest browser. PDF-ul a fost descărcat.", Severity.Info);
                    break;
                case ShareResult.Cancelled:
                    break;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Distribuirea a eșuat: {ex.Message}", Severity.Error);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await JS.InvokeVoidAsync("eHarfa.disposeSongGestures", "song-content");
        }
        catch (JSDisconnectedException)
        {
            // The WebView has already been disposed.
        }

        _dotNetRef?.Dispose();
    }

    private static string FormatSongContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        return NormalizeSongContent(content)
            .Replace("\n", "<br />");
    }

    private static string NormalizeSongContent(string? content)
    {
        return (content ?? string.Empty)
            .Replace("\\r\\n", "\n")
            .Replace("\\n", "\n")
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");
    }

    private string GetSongTextForClipboard()
    {
        return Song is null
            ? string.Empty
            : $"{Song.Title}\n\n{NormalizeSongContent(Song.Content)}";
    }
}
