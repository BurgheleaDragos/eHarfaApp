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

    public async Task<ShareResult> ShareSongAsync(Song song)
    {
        var pdfBytes = SongPdfDocument.Generate(CreatePrintableSong(song));
        var fileName = $"{ToSafeFileName(song.Title)}.pdf";
        var base64 = Convert.ToBase64String(pdfBytes);

        var result = await jsRuntime.InvokeAsync<string>("eHarfa.shareSongPdf", fileName, base64, song.Title);

        return result switch
        {
            "shared" => ShareResult.Shared,
            "cancelled" => ShareResult.Cancelled,
            _ => ShareResult.Downloaded
        };
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
        // Windows and modern browsers accept Unicode file names.  Only remove
        // characters that are actually invalid in a file name.
        var sanitized = Regex.Replace(title, @"[\x00-\x1F<>:""/\\|?*]", "");
        sanitized = Regex.Replace(sanitized, "\\s+", " ").Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? "song-export" : sanitized;
    }
}
