using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Pages;

namespace eHarfaApp.Shared.Services;

public interface ISongService
{
    Task<List<Song>> GetSongsAsync();
    Task<List<SongCategory>> GetCategoriesAsync();
}