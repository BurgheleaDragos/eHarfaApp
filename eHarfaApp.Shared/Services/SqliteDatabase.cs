using System.Data;
using eHarfaApp.Shared.DAL;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace eHarfaApp.Shared.Services;

public class SqliteDatabase(IConfiguration configuration)
{
    private const string DatabaseFileName = "eHarfaApp.db";
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _isInitialized;

    public string DatabasePath =>
        configuration["Database:Path"]
        ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            DatabaseFileName);

    public async Task EnsureCreatedAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        await _initializationLock.WaitAsync().ConfigureAwait(false);

        try
        {
            if (_isInitialized)
            {
                return;
            }

            var directoryPath = Path.GetDirectoryName(DatabasePath);
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await using var connection = CreateConnection();
            await connection.OpenAsync().ConfigureAwait(false);

            await using var command = connection.CreateCommand();
            command.CommandText =
                """
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS SongCategories (
                    Id TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Icon TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Songs (
                    Id TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Scale TEXT NOT NULL,
                    Content TEXT,
                    CategoryId TEXT NOT NULL,
                    FOREIGN KEY(CategoryId) REFERENCES SongCategories(Id) ON DELETE RESTRICT
                );

                CREATE TABLE IF NOT EXISTS Settings (
                    Id INTEGER PRIMARY KEY CHECK (Id = 1),
                    FontSize INTEGER NOT NULL,
                    FontFamily TEXT NOT NULL,
                    ApplicationColor INTEGER NOT NULL,
                    Contact TEXT NOT NULL,
                    LastSynchronized TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS FontFamilies (
                    Name TEXT PRIMARY KEY
                );
                """;

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            await SeedAsync(connection).ConfigureAwait(false);

            _isInitialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DatabasePath}");
    }

    private async Task SeedAsync(SqliteConnection connection)
    {
        if (await TableHasRowsAsync(connection, "SongCategories").ConfigureAwait(false) == false)
        {
            foreach (var category in SeedData.CreateCategories())
            {
                await using var insertCategory = connection.CreateCommand();
                insertCategory.CommandText =
                    """
                    INSERT INTO SongCategories (Id, Title, Icon)
                    VALUES (@Id, @Title, @Icon);
                    """;
                insertCategory.Parameters.AddWithValue("@Id", category.Id);
                insertCategory.Parameters.AddWithValue("@Title", category.Title);
                insertCategory.Parameters.AddWithValue("@Icon", category.Icon);
                await insertCategory.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        if (await TableHasRowsAsync(connection, "Songs").ConfigureAwait(false) == false)
        {
            await using var transaction = connection.BeginTransaction();

            foreach (var song in SeedData.ImportInitialSongs())
            {
                await InsertOrReplaceSongAsync(connection, song, transaction).ConfigureAwait(false);
            }

            await transaction.CommitAsync().ConfigureAwait(false);
        }

        if (await TableHasRowsAsync(connection, "Settings").ConfigureAwait(false) == false)
        {
            var settings = SeedData.CreateDefaultSettings(GetContact());

            await using var insertSettings = connection.CreateCommand();
            insertSettings.CommandText =
                """
                INSERT INTO Settings (Id, FontSize, FontFamily, ApplicationColor, Contact, LastSynchronized)
                VALUES (1, @FontSize, @FontFamily, @ApplicationColor, @Contact, @LastSynchronized);
                """;
            insertSettings.Parameters.AddWithValue("@FontSize", settings.FontSize);
            insertSettings.Parameters.AddWithValue("@FontFamily", settings.FontFamily);
            insertSettings.Parameters.AddWithValue("@ApplicationColor", (int)settings.ApplicationColor);
            insertSettings.Parameters.AddWithValue("@Contact", settings.Contact);
            insertSettings.Parameters.AddWithValue("@LastSynchronized", settings.LastSynchronized.ToString("O"));
            await insertSettings.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        foreach (var fontFamily in SeedData.CreateFontFamilies())
        {
            await using var insertFont = connection.CreateCommand();
            insertFont.CommandText =
                """
                INSERT OR IGNORE INTO FontFamilies (Name)
                VALUES (@Name);
                """;
            insertFont.Parameters.AddWithValue("@Name", fontFamily);
            await insertFont.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }

    private string GetContact()
    {
        return configuration.GetSection("EmailContact")["EmailContact"]
               ?? configuration["EmailContact"]
               ?? "test@ccc.com";
    }

    private static async Task<bool> TableHasRowsAsync(SqliteConnection connection, string tableName)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT 1 FROM {tableName} LIMIT 1);";

        var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
        return result is long count && count == 1;
    }

    private static async Task InsertOrReplaceSongAsync(
        SqliteConnection connection, Song song, SqliteTransaction? transaction = null)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT OR REPLACE INTO Songs (Id, Title, Scale, Content, CategoryId)
            VALUES (@Id, @Title, @Scale, @Content, @CategoryId);
            """;
        command.Parameters.AddWithValue("@Id", song.Id);
        command.Parameters.AddWithValue("@Title", song.Title);
        command.Parameters.AddWithValue("@Scale", song.Scale);
        command.Parameters.AddWithValue("@Content", (object?)song.Content ?? DBNull.Value);
        command.Parameters.AddWithValue("@CategoryId", song.CategoryId);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
