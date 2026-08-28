using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class HomePage: ComponentBase, IDisposable
{
    private const int SearchDebounceMilliseconds = 300;

    [Inject]
    public ISongService SongService { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    private string _search = string.Empty;
    private int _selectedCategory;
    private CancellationTokenSource? _searchDebounce;

    private string Search
    {
        get => _search;
        set
        {
            if (_search == value) return;
            _search = value;
            DebounceSearch();
        }
    }

    private int SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory == value) return;
            _selectedCategory = value;
            _ = LoadSelectedCategorySongsAsync();
        }
    }

    private List<Song> CategorySongs { get; set; } = [];
    private List<SongCategory> Categories { get; set; } = [];
    private string PageTitle => Categories.Count == 0 ? "Cântări" : Categories[SelectedCategory].Title;

    protected override async Task OnInitializedAsync()
    {
        await GetSongCategoriesAsync();
        await LoadSelectedCategorySongsAsync();
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

    private Task ShowPreviousCategoryAsync()
    {
        if (SelectedCategory > 0)
            SelectedCategory--;

        return Task.CompletedTask;
    }

    private Task ShowNextCategoryAsync()
    {
        if (SelectedCategory < Categories.Count - 1)
            SelectedCategory++;

        return Task.CompletedTask;
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

    private void DebounceSearch()
    {
        _searchDebounce?.Cancel();
        _searchDebounce?.Dispose();

        var cts = new CancellationTokenSource();
        _searchDebounce = cts;
        _ = DebouncedLoadAsync(cts.Token);
    }

    private async Task DebouncedLoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(SearchDebounceMilliseconds, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (cancellationToken.IsCancellationRequested) return;

        await LoadSelectedCategorySongsAsync().ConfigureAwait(false);
    }

    private async Task LoadSelectedCategorySongsAsync()
    {
        if (Categories.Count == 0)
        {
            CategorySongs = [];
            return;
        }

        try
        {
            var categoryId = Categories[SelectedCategory].Id;
            CategorySongs = await SongService.GetSongSummariesFromDatabaseAsync(categoryId, Search).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        await InvokeAsync(StateHasChanged);
    }

    private List<Song> GetSongsForPanel(string categoryId)
    {
        if (Categories.Count == 0)
        {
            return [];
        }

        return categoryId.Equals(Categories[SelectedCategory].Id, StringComparison.InvariantCultureIgnoreCase)
            ? CategorySongs
            : [];
    }

    private string GetSongDisplayText(string categoryId, Song song)
    {
        var songs = GetSongsForPanel(categoryId);
        return $"{songs.IndexOf(song)}. {song.Title}";
    }

    private void SelectSong(string? arg)
    {
        if (string.IsNullOrEmpty(arg))
            return;

        NavigationManager.NavigateTo(RoutingLinks.GetSongPageLink(arg));
    }

    public void Dispose()
    {
        _searchDebounce?.Cancel();
        _searchDebounce?.Dispose();
    }
}
