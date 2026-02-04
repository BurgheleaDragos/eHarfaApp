namespace eHarfaApp.Shared;

public static class RoutingLinks
{
    private static string SongPageLink => "/song/";

    public static string GetSongPageLink(string id)
    {
        return $"{SongPageLink}{id}";
    }
}