using System.Text.RegularExpressions;
using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Services;

namespace eHarfaApp.Services;

public class MauiPdfExportService : IPdfExportService
{
    public async Task ExportSongAsync(Song song)
    {
        var printableSong = new Song
        {
            Id = song.Id,
            Title = song.Title,
            Scale = song.Scale,
            CategoryId = song.CategoryId,
            Category = song.Category,
            Content = NormalizeContent(song.Content)
        };

        var pdfBytes = SongPdfDocument.Generate(printableSong);
        var fileName = $"{ToSafeFileName(song.Title)}.pdf";
        var filePath = Path.Combine(FileSystem.Current.CacheDirectory, fileName);

        await File.WriteAllBytesAsync(filePath, pdfBytes);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = song.Title,
            File = new ShareFile(filePath)
        });
    }

    private static string NormalizeContent(string? content)
    {
        return (content ?? string.Empty)
            .Replace("\\r\\n", "\n")
            .Replace("\\n", "\n")
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");
    }

    private static string ToSafeFileName(string title)
    {
        var sanitized = Regex.Replace(title, "[^a-zA-Z0-9 _.-]", "");
        sanitized = Regex.Replace(sanitized, "\\s+", " ").Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? "song-export" : sanitized;
    }
}
