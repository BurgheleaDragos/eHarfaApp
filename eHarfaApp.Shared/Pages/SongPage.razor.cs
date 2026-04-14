using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class SongPage: ComponentBase
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private ISongService SongService { get; set; } = null!;

    [Inject]
    private IPdfExportService PdfExportService { get; set; } = null!;

    [Parameter]
    public string Id { get; set; } = string.Empty;

    private Song? Song { get; set; }
    private MarkupString FormattedContent =>
        new(FormatSongContent(Song?.Content));
    
    protected override async Task OnParametersSetAsync()
    {
        Song = await GetSongByIdAsync(Id);
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
