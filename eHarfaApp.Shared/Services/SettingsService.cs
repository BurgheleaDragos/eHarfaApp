using eHarfaApp.Shared.DAL;
using Microsoft.Extensions.Configuration;

namespace eHarfaApp.Shared.Services;

public class SettingsService(IConfiguration configuration) : ISettingsService
{
    private Settings? _settings = null;

    public async Task<Settings> ReadSettingsAsync()
    {
        var localSettings = _settings ?? GetDefaultSettings();
        localSettings.Contact = configuration.GetSection("EmailContact")["EmailContact"] ?? string.Empty;
        return await Task.FromResult(localSettings);
    }

    public Task SaveSettingsAsync(Settings settings)
    {
         _settings = settings;
         return Task.CompletedTask;
    }

    public Settings GetDefaultSettings()
    {
        return new Settings()
        {
            FontSize = 20,
            FontFamily = "INTER",
            ApplicationColor = ApplicationColor.Automatic,
            Contact = "test@ccc.com", //configuration["EmailContact"] ?? string.Empty,
            LastSynchronized = DateTime.Now,
        };
    }

    public Task<List<string>> GetFontFamiliesAsync()
    {
        var task = new List<string>();
        task.Add("INTER");
        task.Add("Arial");
        return Task.FromResult(task);
    }
}