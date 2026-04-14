using eHarfaApp.Shared.DAL;
using Microsoft.Extensions.Configuration;

namespace eHarfaApp.Shared.Services;

public class SettingsService(IConfiguration configuration, SqliteDatabase sqliteDatabase) : ISettingsService
{
    private Settings? _settings = null;

    public async Task<Settings> ReadSettingsAsync()
    {
        var localSettings = _settings ?? GetDefaultSettings();
        localSettings.Contact = GetContact();
        return await Task.FromResult(localSettings);
    }

    public Task SaveSettingsAsync(Settings settings)
    {
         _settings = settings;
         return Task.CompletedTask;
    }

    public Settings GetDefaultSettings()
    {
        return SeedData.CreateDefaultSettings(GetContact());
    }

    public Task<List<string>> GetFontFamiliesAsync()
    {
        return Task.FromResult(SeedData.CreateFontFamilies());
    }

    public async Task<Settings> ReadSettingsFromDatabaseAsync()
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT FontSize, FontFamily, ApplicationColor, Contact, LastSynchronized
            FROM Settings
            WHERE Id = 1
            LIMIT 1;
            """;

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        if (await reader.ReadAsync().ConfigureAwait(false) == false)
        {
            return GetDefaultSettings();
        }

        return new Settings
        {
            FontSize = reader.GetInt32(reader.GetOrdinal("FontSize")),
            FontFamily = reader.GetString(reader.GetOrdinal("FontFamily")),
            ApplicationColor = (ApplicationColor)reader.GetInt32(reader.GetOrdinal("ApplicationColor")),
            Contact = reader.GetString(reader.GetOrdinal("Contact")),
            LastSynchronized = DateTime.Parse(
                reader.GetString(reader.GetOrdinal("LastSynchronized")),
                null,
                System.Globalization.DateTimeStyles.RoundtripKind)
        };
    }

    public async Task SaveSettingsToDatabaseAsync(Settings settings)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT OR REPLACE INTO Settings (Id, FontSize, FontFamily, ApplicationColor, Contact, LastSynchronized)
            VALUES (1, @FontSize, @FontFamily, @ApplicationColor, @Contact, @LastSynchronized);
            """;
        command.Parameters.AddWithValue("@FontSize", settings.FontSize);
        command.Parameters.AddWithValue("@FontFamily", settings.FontFamily);
        command.Parameters.AddWithValue("@ApplicationColor", (int)settings.ApplicationColor);
        command.Parameters.AddWithValue("@Contact", settings.Contact);
        command.Parameters.AddWithValue("@LastSynchronized", settings.LastSynchronized.ToString("O"));
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public Task UpdateSettingsInDatabaseAsync(Settings settings)
    {
        return SaveSettingsToDatabaseAsync(settings);
    }

    public async Task<List<string>> GetFontFamiliesFromDatabaseAsync()
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        var fontFamilies = new List<string>();

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Name
            FROM FontFamilies
            ORDER BY Name;
            """;

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            fontFamilies.Add(reader.GetString(reader.GetOrdinal("Name")));
        }

        return fontFamilies;
    }

    public async Task SaveFontFamiliesToDatabaseAsync(IEnumerable<string> fontFamilies)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await using var transaction = connection.BeginTransaction();

        foreach (var fontFamily in fontFamilies.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText =
                """
                INSERT OR REPLACE INTO FontFamilies (Name)
                VALUES (@Name);
                """;
            command.Parameters.AddWithValue("@Name", fontFamily);
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        await transaction.CommitAsync().ConfigureAwait(false);
    }

    public async Task UpdateFontFamilyInDatabaseAsync(string currentName, string newName)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE FontFamilies
            SET Name = @NewName
            WHERE Name = @CurrentName;
            """;
        command.Parameters.AddWithValue("@CurrentName", currentName);
        command.Parameters.AddWithValue("@NewName", newName);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    private string GetContact()
    {
        return configuration.GetSection("EmailContact")["EmailContact"]
               ?? configuration["EmailContact"]
               ?? "test@ccc.com";
    }
}
