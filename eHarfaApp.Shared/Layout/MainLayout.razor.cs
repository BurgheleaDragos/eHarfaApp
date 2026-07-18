using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eHarfaApp.Shared.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    private ISettingsService SettingsService { get; set; } = null!;

    private bool _drawerOpen = true;
    private bool _disableBackBtn = false;
    private MudThemeProvider _mudThemeProvider = null!;
    private bool _isDarkMode;
    private bool _useSystemDarkMode = true;
    private Settings? _settings;
    private MudTheme _theme = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _settings = await SettingsService.ReadSettingsFromDatabaseAsync();
        }
        catch
        {
            _settings = SettingsService.GetDefaultSettings();
        }
        ApplySettings(_settings);
        SettingsService.SettingsChanged += OnSettingsChanged;
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemDarkModeChanged);
            StateHasChanged();
        }
    }

    private void ApplySettings(Settings settings)
    {
        _settings = settings;
        _useSystemDarkMode = settings.ApplicationColor == ApplicationColor.Automatic;
        if (!_useSystemDarkMode)
            _isDarkMode = settings.ApplicationColor == ApplicationColor.Dark;

        _theme = BuildTheme(settings.FontFamily);
    }

    private static MudTheme BuildTheme(string fontFamily)
    {
        var families = new[] { fontFamily, "sans-serif" };
        return new MudTheme
        {
            Typography = new Typography
            {
                Default = new DefaultTypography { FontFamily = families },
                H1 = new H1Typography { FontFamily = families },
                H2 = new H2Typography { FontFamily = families },
                H3 = new H3Typography { FontFamily = families },
                H4 = new H4Typography { FontFamily = families },
                H5 = new H5Typography { FontFamily = families },
                H6 = new H6Typography { FontFamily = families },
                Subtitle1 = new Subtitle1Typography { FontFamily = families },
                Subtitle2 = new Subtitle2Typography { FontFamily = families },
                Body1 = new Body1Typography { FontFamily = families },
                Body2 = new Body2Typography { FontFamily = families },
                Button = new ButtonTypography { FontFamily = families },
                Caption = new CaptionTypography { FontFamily = families },
                Overline = new OverlineTypography { FontFamily = families }
            }
        };
    }

    private void OnSettingsChanged(Settings settings)
    {
        ApplySettings(settings);
        InvokeAsync(StateHasChanged);
    }

    private Task OnSystemDarkModeChanged(bool newValue)
    {
        if (_useSystemDarkMode)
        {
            _isDarkMode = newValue;
            StateHasChanged();
        }
        return Task.CompletedTask;
    }

    private Task OnDarkModeChangedFromSettings(bool newValue)
    {
        _isDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        SettingsService.SettingsChanged -= OnSettingsChanged;
    }
}
