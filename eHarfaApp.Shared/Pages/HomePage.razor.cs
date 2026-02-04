using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class HomePage: ComponentBase
{
    [Inject]
    public ISongService SongService { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    
    private string Search { get; set; }

    private List<Song> Songs { get; set; }
    private List<Song> FilteredSongs => 
        Songs
            .Where(e => e.CategoryId.Equals(Categories[SelectedCategory].Id, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    private List<SongCategory> Categories { get; set; } = null!;
    private int SelectedCategory { get; set; }
    private string PageTitle => Categories[SelectedCategory].Title;

    protected override async Task OnInitializedAsync()
    {
        await GetSongCategoriesAsync();
        await GetSongsAsync();
        await base.OnInitializedAsync();
    }
    
    private const int MaxTabs = 16; // Total number of tabs
    
    private void HandleSwipe(SwipeEventArgs e)
    {
        if (e.SwipeDirection == SwipeDirection.RightToLeft && SelectedCategory < MaxTabs - 1)
        {
            SelectedCategory++; // Swipe left to go to next tab
        }
        else if (e.SwipeDirection == SwipeDirection.LeftToRight && SelectedCategory > 0)
        {
            SelectedCategory--; // Swipe right to go to previous tab
        }
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

    private void SelectSong(string? arg)
    {
        if (string.IsNullOrEmpty(arg))
            return;
        
        NavigationManager.NavigateTo(RoutingLinks.GetSongPageLink(arg));
    }
}