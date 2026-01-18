using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace eHarfaApp.Shared.Pages;

public partial class HomePage: ComponentBase
{
    [Inject]
    public ISongService SongService { get; set; }
    private string factor => FormFactor.GetFormFactor();
    private string platform => FormFactor.GetPlatform();
    private string Search { get; set; }

    private List<Song> Songs { get; set; }
    private List<Song> FilteredSongs => 
        Songs
            //.Where(e => e.CategoryId.Equals(Categories[SelectedCategory], StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    private List<SongCategory> Categories { get; set; } = null!;
    private int SelectedCategory { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetSongCategoriesAsync();
        await GetSongsAsync();
        await base.OnInitializedAsync();
    }

    private async Task GetSongsAsync()
    {
        try
        {
            Songs = await SongService.GetSongsAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task GetSongCategoriesAsync()
    {
        try
        {
            Categories = await SongService.GetCategoriesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void SelectSong(MouseEventArgs e)
    {
        Console.WriteLine(e);
    }
}