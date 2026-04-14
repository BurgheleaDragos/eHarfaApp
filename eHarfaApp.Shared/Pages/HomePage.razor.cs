using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class HomePage: ComponentBase
{
    [Inject]
    public ISongService SongService { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    private string Search { get; set; } = string.Empty;

    private List<Song> Songs { get; set; } = [];
    private List<Song> FilteredSongs => 
        Songs
            .Where(e =>
                Categories.Count > 0 &&
                e.CategoryId.Equals(Categories[SelectedCategory].Id, StringComparison.InvariantCultureIgnoreCase) &&
                (string.IsNullOrWhiteSpace(Search) ||
                 e.Title.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ||
                 (e.Content?.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ?? false)))
            .ToList();
    private List<SongCategory> Categories { get; set; } = [];
    private int SelectedCategory { get; set; }
    private string PageTitle => Categories.Count == 0 ? "Cântări" : Categories[SelectedCategory].Title;

    protected override async Task OnInitializedAsync()
    {
        await GetSongCategoriesAsync();
        await GetSongsAsync();
        await base.OnInitializedAsync();
    }
    
    private void HandleSwipe(SwipeEventArgs e)
    {
        var maxTabs = Categories.Count;

        if (e.SwipeDirection == SwipeDirection.RightToLeft && SelectedCategory < maxTabs - 1)
        {
            SelectedCategory++;
        }
        else if (e.SwipeDirection == SwipeDirection.LeftToRight && SelectedCategory > 0)
        {
            SelectedCategory--;
        }
    }

    private async Task GetSongsAsync()
    {
        try
        {
            Songs = await SongService.GetSongsFromDatabaseAsync().ConfigureAwait(false);
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
            Categories = await SongService.GetCategoriesFromDatabaseAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        if (SelectedCategory >= Categories.Count)
        {
            SelectedCategory = 0;
        }
    }

    private void SelectSong(string? arg)
    {
        if (string.IsNullOrEmpty(arg))
            return;
        
        NavigationManager.NavigateTo(RoutingLinks.GetSongPageLink(arg));
    }
}
