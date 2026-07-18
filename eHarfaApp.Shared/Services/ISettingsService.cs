using eHarfaApp.Shared.DAL;

namespace eHarfaApp.Shared.Services;

public interface ISettingsService
{
    event Action<Settings>? SettingsChanged;

    Task<Settings> ReadSettingsAsync();
    Task SaveSettingsAsync(Settings settings);
    Settings GetDefaultSettings();
    Task<List<string>> GetFontFamiliesAsync();
    Task<Settings> ReadSettingsFromDatabaseAsync();
    Task SaveSettingsToDatabaseAsync(Settings settings);
    Task UpdateSettingsInDatabaseAsync(Settings settings);
    Task<List<string>> GetFontFamiliesFromDatabaseAsync();
    Task SaveFontFamiliesToDatabaseAsync(IEnumerable<string> fontFamilies);
    Task UpdateFontFamilyInDatabaseAsync(string currentName, string newName);
}
