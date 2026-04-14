using System.Text.RegularExpressions;
using eHarfaApp.Shared.DAL;
using Microsoft.JSInterop;

namespace eHarfaApp.Shared.Services;

public class WebPdfExportService(IJSRuntime jsRuntime) : IPdfExportService
{
    public async Task ExportSongAsync(Song song)
    {
        var pdfBytes = SongPdfDocument.Generate(CreatePrintableSong(song));
        var fileName = $"{ToSafeFileName(song.Title)}.pdf";
        var base64 = Convert.ToBase64String(pdfBytes);

        await jsRuntime.InvokeVoidAsync("eHarfa.downloadPdfFromBase64", fileName, base64);
    }

    private static Song CreatePrintableSong(Song song)
    {
        return new Song
        {
            Id = song.Id,
            Title = song.Title,
            Scale = song.Scale,
            CategoryId = song.CategoryId,
            Category = song.Category,
            Content = NormalizeContent(song.Content)
        };
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
