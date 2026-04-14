using eHarfaApp.Services;
using eHarfaApp.Shared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace eHarfaApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddSingleton<SqliteDatabase>();
        builder.Services.AddSingleton<IPdfExportService, MauiPdfExportService>();
        builder.Services.AddSingleton<ISongService, SongService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IApiService, ApiService>();

        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
