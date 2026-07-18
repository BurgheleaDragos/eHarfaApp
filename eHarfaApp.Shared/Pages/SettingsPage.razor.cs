using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class SettingsPage: ComponentBase
{
    [Inject]
    private ISettingsService SettingsService { get; set; } = null!;

    [Inject]
    private ISongService SongService { get; set; } = null!;

    [Inject]
    private IApiService ApiService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private Settings _settings = null!;
    private bool _nightMode;
    private List<string> _fontFamilies = [];

    [CascadingParameter(Name = "DarkMode")]
    public bool NightMode
    {
        get => _nightMode;
        set
        {
            if (_nightMode == value) return;
            _nightMode = value;
            IsDarkModeChanged.InvokeAsync(value);
            _ = PersistNightModeAsync(value);
        }
    }

    [CascadingParameter(Name = "DarkModeChanged")]
    public EventCallback<bool> IsDarkModeChanged { get; set; }

    private async Task PersistNightModeAsync(bool isDark)
    {
        _settings.ApplicationColor = isDark ? ApplicationColor.Dark : ApplicationColor.Light;
        await SaveSettingsAsync().ConfigureAwait(false);
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _settings = await GetSettingsAsync().ConfigureAwait(false);
        _fontFamilies = await GetFontFamiliesAsync().ConfigureAwait(false);
        _nightMode = _settings.ApplicationColor == ApplicationColor.Dark;
    }

    private async Task<List<string>> GetFontFamiliesAsync()
    {
        try
        {
            return await SettingsService.GetFontFamiliesFromDatabaseAsync().ConfigureAwait(false);
        }
        catch(OperationCanceledException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return [];
    }

    private async Task<Settings> GetSettingsAsync()
    {
        try
        {
            return await SettingsService.ReadSettingsFromDatabaseAsync().ConfigureAwait(false);
        }
        catch(OperationCanceledException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return SettingsService.GetDefaultSettings();
    }

    private async Task SaveSettingsAsync()
    {
        await SettingsService.UpdateSettingsInDatabaseAsync(_settings).ConfigureAwait(false);
        Snackbar.Add("Setările au fost salvate.", Severity.Success);
    }

    private async Task OnFontSizeChanged(int value)
    {
        _settings.FontSize = value;
        await SaveSettingsAsync().ConfigureAwait(false);
    }

    private async Task OnFontFamilyChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        _settings.FontFamily = value;
        await SaveSettingsAsync().ConfigureAwait(false);
    }

    private async Task SyncData(MouseEventArgs arg)
    {
        var categories = await ApiService.GetCategoriesAsync().ConfigureAwait(false);
        if (categories.Count == 0)
        {
            categories = await SongService.GetCategoriesAsync().ConfigureAwait(false);
        }

        await SongService.SaveCategoriesToDatabaseAsync(categories).ConfigureAwait(false);

        var songs = await ApiService.GetSongsAsync().ConfigureAwait(false);
        if (songs.Count == 0)
        {
            songs = await SongService.GetSongsAsync().ConfigureAwait(false);
        }

        await SongService.SaveSongsToDatabaseAsync(songs).ConfigureAwait(false);

        _settings.LastSynchronized = DateTime.Now;
        await SaveSettingsAsync().ConfigureAwait(false);
        Snackbar.Add("Datele au fost sincronizate din baza de date.", Severity.Success);
    }
}
