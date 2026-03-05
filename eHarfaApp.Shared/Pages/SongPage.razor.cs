using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class SongPage: ComponentBase
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private ISongService SongService { get; set; }

    [Parameter]
    public string Id { get; set; }

    private Song? Song { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Song = await GetSongByIdAsync(Id);
        if (Song != null)
        {
            Song.Content = Song?.Content?.Replace("\\n", "<br />");
        }

        await base.OnInitializedAsync();
    }

    private async Task<Song?> GetSongByIdAsync(string id)
    {
        try
        {
            return await SongService.GetSongByIdAsync(id);
        }
        catch(OperationCanceledException) { }
        catch (Exception ex)
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

    private Task ExportPDF(MouseEventArgs arg)
    {
        Snackbar.Add("Exported to PDF", Severity.Info);
        return Task.CompletedTask;
    }

    private Task ShareSong(MouseEventArgs arg)
    {
        Snackbar.Add("Share Song", Severity.Info);
        return Task.CompletedTask;
    }
}