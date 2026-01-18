namespace eHarfaApp.Shared.DAL;

public class SongCategory
{
    public SongCategory()
    {
    }

    public SongCategory(string id, string title, string icon)
    {
        Id = id;
        Title = title;
        Icon = icon;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
}