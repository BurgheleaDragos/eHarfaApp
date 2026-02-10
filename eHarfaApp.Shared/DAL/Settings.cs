namespace eHarfaApp.Shared.DAL;

public class Settings
{
    public required int FontSize { get; set; }
    public required string FontFamily { get; set; }
    public ApplicationColor ApplicationColor { get; set; }
    public required string Contact { get; set; }
    
    public DateTime LastSynchronized { get; set; }
}