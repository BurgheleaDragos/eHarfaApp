using eHarfaApp.Shared.DAL;
using MudBlazor;

namespace eHarfaApp.Shared.Services;

public class SongService(SqliteDatabase sqliteDatabase) : ISongService
{
    public Task<List<Song>> GetSongsAsync()
    {
        return Task.FromResult(SeedData.CreateSongs());
    }

    public Task<List<SongCategory>> GetCategoriesAsync()
    {
        return Task.FromResult(SeedData.CreateCategories());
    }

    public async Task<Song> GetSongByIdAsync(string id)
    {
        return await Task.FromResult(SeedData.CreateSongById(id));
    }

    public async Task<List<Song>> GetSongsFromDatabaseAsync()
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        var categories = await GetCategoriesFromDatabaseAsync().ConfigureAwait(false);
        var categoryLookup = categories.ToDictionary(category => category.Id, StringComparer.OrdinalIgnoreCase);
        var songs = new List<Song>();

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Title, Scale, Content, CategoryId
            FROM Songs
            ORDER BY CAST(Id AS INTEGER), Id;
            """;

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            var categoryId = reader.GetString(reader.GetOrdinal("CategoryId"));
            categoryLookup.TryGetValue(categoryId, out var category);

            songs.Add(new Song
            {
                Id = reader.GetString(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Scale = reader.GetString(reader.GetOrdinal("Scale")),
                Content = reader.IsDBNull(reader.GetOrdinal("Content"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Content")),
                CategoryId = categoryId,
                Category = category ?? new SongCategory { Id = categoryId, Title = string.Empty, Icon = Icons.Material.Filled.Category }
            });
        }

        return songs;
    }

    public async Task<List<SongCategory>> GetCategoriesFromDatabaseAsync()
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        var categories = new List<SongCategory>();

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Title, Icon
            FROM SongCategories
            ORDER BY CAST(Id AS INTEGER), Id;
            """;

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            categories.Add(new SongCategory
            {
                Id = reader.GetString(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Icon = reader.GetString(reader.GetOrdinal("Icon"))
            });
        }

        return categories;
    }

    public async Task<Song?> GetSongByIdFromDatabaseAsync(string id)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT s.Id, s.Title, s.Scale, s.Content, s.CategoryId, c.Title AS CategoryTitle, c.Icon
            FROM Songs s
            INNER JOIN SongCategories c ON c.Id = s.CategoryId
            WHERE s.Id = @Id
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        if (await reader.ReadAsync().ConfigureAwait(false) == false)
        {
            return null;
        }

        return new Song
        {
            Id = reader.GetString(reader.GetOrdinal("Id")),
            Title = reader.GetString(reader.GetOrdinal("Title")),
            Scale = reader.GetString(reader.GetOrdinal("Scale")),
            Content = reader.IsDBNull(reader.GetOrdinal("Content"))
                ? null
                : reader.GetString(reader.GetOrdinal("Content")),
            CategoryId = reader.GetString(reader.GetOrdinal("CategoryId")),
            Category = new SongCategory
            {
                Id = reader.GetString(reader.GetOrdinal("CategoryId")),
                Title = reader.GetString(reader.GetOrdinal("CategoryTitle")),
                Icon = reader.GetString(reader.GetOrdinal("Icon"))
            }
        };
    }

    public async Task SaveSongsToDatabaseAsync(IEnumerable<Song> songs)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await using var transaction = connection.BeginTransaction();

        foreach (var song in songs)
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

        await transaction.CommitAsync().ConfigureAwait(false);
    }

    public async Task SaveCategoriesToDatabaseAsync(IEnumerable<SongCategory> categories)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await using var transaction = connection.BeginTransaction();

        foreach (var category in categories)
        {
            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText =
                """
                INSERT OR REPLACE INTO SongCategories (Id, Title, Icon)
                VALUES (@Id, @Title, @Icon);
                """;
            command.Parameters.AddWithValue("@Id", category.Id);
            command.Parameters.AddWithValue("@Title", category.Title);
            command.Parameters.AddWithValue("@Icon", category.Icon);
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        await transaction.CommitAsync().ConfigureAwait(false);
    }

    public async Task UpdateSongInDatabaseAsync(Song song)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Songs
            SET Title = @Title,
                Scale = @Scale,
                Content = @Content,
                CategoryId = @CategoryId
            WHERE Id = @Id;
            """;
        command.Parameters.AddWithValue("@Id", song.Id);
        command.Parameters.AddWithValue("@Title", song.Title);
        command.Parameters.AddWithValue("@Scale", song.Scale);
        command.Parameters.AddWithValue("@Content", (object?)song.Content ?? DBNull.Value);
        command.Parameters.AddWithValue("@CategoryId", song.CategoryId);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateCategoryInDatabaseAsync(SongCategory category)
    {
        await sqliteDatabase.EnsureCreatedAsync().ConfigureAwait(false);

        await using var connection = sqliteDatabase.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE SongCategories
            SET Title = @Title,
                Icon = @Icon
            WHERE Id = @Id;
            """;
        command.Parameters.AddWithValue("@Id", category.Id);
        command.Parameters.AddWithValue("@Title", category.Title);
        command.Parameters.AddWithValue("@Icon", category.Icon);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
