namespace eHarfaApp.Shared.DAL;

public class Song
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Scale { get; set; }
    public string? Content { get; set; }
    public string CategoryId { get; set; }
    public SongCategory Category { get; set; }
}