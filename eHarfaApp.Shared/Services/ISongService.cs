using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Pages;

namespace eHarfaApp.Shared.Services;

public interface ISongService
{
    Task<List<Song>> GetSongsAsync();
    Task<List<SongCategory>> GetCategoriesAsync();
    Task<Song> GetSongByIdAsync(string id);
    Task<List<Song>> GetSongsFromDatabaseAsync();
    Task<List<SongCategory>> GetCategoriesFromDatabaseAsync();
    Task<Song?> GetSongByIdFromDatabaseAsync(string id);
    Task SaveSongsToDatabaseAsync(IEnumerable<Song> songs);
    Task SaveCategoriesToDatabaseAsync(IEnumerable<SongCategory> categories);
    Task UpdateSongInDatabaseAsync(Song song);
    Task UpdateCategoryInDatabaseAsync(SongCategory category);
}
