using eHarfaApp.Shared.DAL;
using MudBlazor;

namespace eHarfaApp.Shared.Services;

public class ApiService: IApiService
{
    public async Task<List<Song>> GetSongsAsync()
    {
        var list = new List<Song>();
        for (int i = 0; i < Random.Shared.Next(10, 100); i++)
        {
            list.Add(new Song()
            {
                Id = (i + 1).ToString(),
                CategoryId = Random.Shared.Next(1,16).ToString(),
                Title = $"Song rand_{Random.Shared.Next()}",
                Content = $"Song content rand_{Random.Shared.Next()}",
                Scale = "Do minor"
            });
        }
        return await Task.FromResult(list);
    }

    public async Task<List<SongCategory>> GetCategoriesAsync()
    {
        var list = new List<SongCategory>();
        for (var i = 0; i < Random.Shared.Next(0, 16); i++)
        {
            list.Add(new SongCategory()
            {
                Id = (i + 1).ToString(),
                Title = $"Song cat_{Random.Shared.Next()}",
                Icon = Icons.Material.Filled.Category
            });
        }
        return await Task.FromResult(list);
    }
}