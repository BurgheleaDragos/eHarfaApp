using eHarfaApp.Shared.DAL;

namespace eHarfaApp.Shared.Services;

public interface IApiService
{
    Task<List<Song>> GetSongsAsync();
    Task<List<SongCategory>> GetCategoriesAsync();
}