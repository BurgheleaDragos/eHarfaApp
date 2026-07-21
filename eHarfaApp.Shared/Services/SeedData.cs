using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using eHarfaApp.Shared.DAL;
using MudBlazor;

namespace eHarfaApp.Shared.Services;

internal static class SeedData
{
    private const string InitialSongsResourceName = "InitialSongsList_v1.0.1.json";

    public static List<Song> ImportInitialSongs()
    {
        var categories = CreateCategories();
        var entries = ReadInitialSongEntries();

        var songs = new List<Song>();
        SongCategory? currentCategory = null;
        var nextId = 1;

        foreach (var entry in entries)
        {
            if (!string.IsNullOrWhiteSpace(entry.Comentariu))
            {
                currentCategory = categories.First(category => category.Title == entry.Comentariu);
            }

            if (currentCategory == null)
            {
                throw new InvalidOperationException(
                    "Song entry found before any category header (comentariu) in the initial songs list.");
            }

            songs.Add(new Song
            {
                Id = nextId.ToString(),
                Title = entry.Titlul,
                Scale = entry.Gama,
                Content = entry.Versuri,
                CategoryId = currentCategory.Id,
                Category = currentCategory
            });

            nextId++;
        }

        return songs;
    }

    private static List<InitialSongEntry> ReadInitialSongEntries()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .First(name => name.EndsWith(InitialSongsResourceName, StringComparison.Ordinal));

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");

        var entries = JsonSerializer.Deserialize<List<InitialSongEntry>>(stream) ?? [];

        // The source file uses NFD-decomposed diacritics (e.g. "a" + combining circumflex),
        // while the rest of the app uses precomposed NFC text. Normalize so category-title
        // lookups and in-app search/filtering compare equal.
        foreach (var entry in entries)
        {
            entry.Titlul = Normalize(entry.Titlul);
            entry.Gama = Normalize(entry.Gama);
            entry.Versuri = entry.Versuri == null ? null : Normalize(entry.Versuri);
            entry.Comentariu = entry.Comentariu == null ? null : Normalize(entry.Comentariu);
        }

        return entries;
    }

    private static string Normalize(string value)
    {
        return value.Normalize(NormalizationForm.FormC);
    }

    private sealed class InitialSongEntry
    {
        [JsonPropertyName("titlul")]
        public string Titlul { get; set; } = string.Empty;

        [JsonPropertyName("gama")]
        public string Gama { get; set; } = string.Empty;

        [JsonPropertyName("versuri")]
        public string? Versuri { get; set; }

        [JsonPropertyName("comentariu")]
        public string? Comentariu { get; set; }
    }

    public static List<SongCategory> CreateCategories()
    {
        return
        [
            new(id: "1", title: "Cântări despre anul nou și trecerea timpului",
                icon: Icons.Material.Filled.Event),
            new(id: "2", title: "Cântări despre binecuvântarea copiilor și despre părinți",
                icon: Icons.Material.Filled.ChildCare),
            new(id: "3", title: "Cântări despre botezul în apă și venirea la pocăință",
                icon: Icons.Material.Filled.Water),
            new(id: "4", title: "Cântări despre căsătorie și dragoste",
                icon: Icons.Material.Filled.Favorite),
            new(id: "5", title: "Cântări despre Cina cea de taină și suferințele Domnului",
                icon: Icons.Material.Filled.Bloodtype),
            new(id: "6", title: "Cântări despre Duhul Sfânt",
                icon: Icons.Material.Filled.FlashOn),
            new(id: "7", title: "Cântări pentru evanghelizare",
                icon: Icons.Material.Filled.Campaign),
            new(id: "8", title: "Cântări pentru mângâiere și îmbărbătare",
                icon: Icons.Material.Filled.VolunteerActivism),
            new(id: "9", title: "Cântări despre îndemn la veghere și pocăință",
                icon: Icons.Material.Filled.Lightbulb),
            new(id: "10", title: "Cântări despre înmormântare",
                icon: Icons.Material.Filled.HeartBroken),
            new(id: "11", title: "Cântări despre învierea și înălțarea Domnului",
                icon: Icons.Material.Filled.ArrowUpward),
            new(id: "12", title: "Cântări de laudă, mulțumire și bucurie",
                icon: Icons.Material.Filled.SentimentVerySatisfied),
            new(id: "13", title: "Cântări despre nașterea Domnului Isus",
                icon: Icons.Material.Filled.StarPurple500),
            new(id: "14", title: "Cântări despre predarea în slujba lui Dumnezeu",
                icon: Icons.Material.Filled.Handshake),
            new(id: "15", title: "Cântări despre revenirea Domnului și Patria cerească",
                icon: Icons.Material.Filled.CloudSync),
            new(id: "16", title: "Cântări pentru timpul de rugăciune",
                icon: Icons.Material.Filled.SelfImprovement),
        ];
    }

    public static Settings CreateDefaultSettings(string contact)
    {
        return new Settings
        {
            FontSize = 20,
            FontFamily = "INTER",
            ApplicationColor = ApplicationColor.Automatic,
            Contact = contact,
            LastSynchronized = DateTime.UtcNow,
        };
    }

    public static List<string> CreateFontFamilies()
    {
        return ["INTER", "Arial", "Lora", "Crimson Text", "Libre Baskerville"];
    }
}
