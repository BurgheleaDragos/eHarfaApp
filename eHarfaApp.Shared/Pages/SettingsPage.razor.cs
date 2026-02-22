using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace eHarfaApp.Shared.Pages;

public partial class SettingsPage: ComponentBase
{
    [Inject]
    private ISettingsService SettingsService { get; set; } = null!;

    [Inject]
    private ISongService SongService { get; set; } = null!;

    [Inject]
    private IApiService ApiService { get; set; } = null!;

    private Settings _settings = null!;
    private bool _nightMode;
    private List<string> _fontFamilies = [];

    [Parameter]
    [CascadingParameter(Name = "DarkMode")]
    public bool NightMode
    {
        get => _nightMode;
        set
        {
            if (_nightMode == value) return;
            _nightMode = value;
            IsDarkModeChanged.InvokeAsync(value);
        }
    }

    [CascadingParameter(Name = "DarkModeChanged")]
    public EventCallback<bool> IsDarkModeChanged { get; set; }   
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _settings = await GetSettingsAsync().ConfigureAwait(false);
        _fontFamilies = await GetFontFamiliesAsync().ConfigureAwait(false);
    }

    private async Task<List<string>> GetFontFamiliesAsync()
    {
        try
        {
            return await SettingsService.GetFontFamiliesAsync();
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
            return await SettingsService.ReadSettingsAsync().ConfigureAwait(false);
        }
        catch(OperationCanceledException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return SettingsService.GetDefaultSettings();
    }

    private Task SyncData(MouseEventArgs arg)
    {
        _settings.LastSynchronized = DateTime.Now;
        return Task.CompletedTask;
    }
}