using System.Text;
using eHarfaApp.Shared.DAL;
using SkiaSharp;

namespace eHarfaApp.Shared.Services;

public static class SongPdfDocument
{
    private const float PageWidth = 595f;
    private const float PageHeight = 842f;
    private const float Margin = 50f;
    private const float TitleFontSize = 21f;
    private const float ScaleFontSize = 11f;
    private const float LyricsFontSize = 12f;
    private const float LyricsLineHeight = 18f;
    private const float LyricsIndent = 18f;
    private const float ChorusIndent = 34f;
    private const float HeaderBaseHeight = 88f;
    private const float FooterHeight = 28f;
    private const float BlockSpacing = 6f;
    private const float SectionSpacing = 14f;
    private const string HeaderCaption = "eHarfaApp Songbook";

    public static byte[] Generate(Song song)
    {
        using var stream = new MemoryStream();
        using var document = SKDocument.CreatePdf(stream);

        var lines = BuildWrappedLines(NormalizeContent(song.Content).Split('\n'));
        using var titleMeasurePaint = CreatePaint(SKColors.Black);
        using var titleMeasureFont = CreateFont(TitleFontSize, true);
        var titleLines = WrapText(song.Title, titleMeasureFont, titleMeasurePaint, PageWidth - (Margin * 2));
        var headerHeight = CalculateHeaderHeight(titleLines.Count);
        var pages = PaginateLines(lines, headerHeight);
        var totalPages = Math.Max(1, pages.Count);

        for (var pageIndex = 0; pageIndex < totalPages; pageIndex++)
        {
            using var canvas = document.BeginPage(PageWidth, PageHeight);
            using var titlePaint = CreatePaint(SKColors.Black);
            using var scalePaint = CreatePaint(SKColors.DimGray);
            using var lyricsPaint = CreatePaint(SKColors.Black);
            using var chorusPaint = CreatePaint(new SKColor(55, 55, 55));
            using var verseNumberPaint = CreatePaint(SKColors.Black);
            using var dividerPaint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 1, IsAntialias = true };
            using var footerPaint = CreatePaint(SKColors.DimGray);
            using var captionPaint = CreatePaint(new SKColor(120, 120, 120));
            using var titleFont = CreateFont(TitleFontSize, true);
            using var scaleFont = CreateFont(ScaleFontSize, false);
            using var lyricsFont = CreateFont(LyricsFontSize, false);
            using var chorusFont = CreateFont(LyricsFontSize, false, true);
            using var verseNumberFont = CreateFont(LyricsFontSize, true);
            using var footerFont = CreateFont(9f, false);
            using var captionFont = CreateFont(9f, false);

            DrawHeader(canvas, titleLines, song.Scale, titlePaint, titleFont, scalePaint, scaleFont, captionPaint, captionFont, dividerPaint);
            DrawLyricsPage(canvas, pages[pageIndex], headerHeight, lyricsPaint, lyricsFont, chorusPaint, chorusFont, verseNumberPaint, verseNumberFont);
            DrawFooter(canvas, pageIndex + 1, totalPages, footerPaint, footerFont, captionPaint, captionFont);

            document.EndPage();
        }

        document.Close();
        return stream.ToArray();
    }

    private static SKPaint CreatePaint(SKColor color)
    {
        return new SKPaint
        {
            IsAntialias = true,
            Color = color
        };
    }

    private static SKFont CreateFont(float textSize, bool isBold, bool isItalic = false)
    {
        var weight = isBold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        var slant = isItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        return new SKFont(
            SKTypeface.FromFamilyName("Arial", new SKFontStyle(weight, SKFontStyleWidth.Normal, slant)),
            textSize);
    }

    private static void DrawHeader(
        SKCanvas canvas,
        IReadOnlyList<string> titleLines,
        string scale,
        SKPaint titlePaint,
        SKFont titleFont,
        SKPaint scalePaint,
        SKFont scaleFont,
        SKPaint captionPaint,
        SKFont captionFont,
        SKPaint dividerPaint)
    {
        DrawAlignedText(canvas, HeaderCaption, PageWidth / 2f, Margin - 14f, SKTextAlign.Center, captionFont, captionPaint);

        var centerX = PageWidth / 2f;
        var titleY = Margin + 12f;
        foreach (var titleLine in titleLines)
        {
            DrawAlignedText(canvas, titleLine, centerX, titleY, SKTextAlign.Center, titleFont, titlePaint);
            titleY += 24f;
        }

        var scaleY = titleY + 6f;
        DrawAlignedText(canvas, scale, PageWidth - Margin, scaleY, SKTextAlign.Right, scaleFont, scalePaint);
        canvas.DrawLine(Margin, scaleY + 20f, PageWidth - Margin, scaleY + 20f, dividerPaint);
    }

    private static void DrawLyricsPage(
        SKCanvas canvas,
        IReadOnlyList<PdfLine> lines,
        float headerHeight,
        SKPaint lyricsPaint,
        SKFont lyricsFont,
        SKPaint chorusPaint,
        SKFont chorusFont,
        SKPaint verseNumberPaint,
        SKFont verseNumberFont)
    {
        var y = Margin + headerHeight;

        foreach (var line in lines)
        {
            if (line.Kind == PdfLineKind.Empty)
            {
                y += line.SpacingAfter;
                continue;
            }

            var paint = line.Kind == PdfLineKind.Chorus ? chorusPaint : lyricsPaint;
            var font = line.Kind == PdfLineKind.Chorus ? chorusFont : lyricsFont;
            var indent = line.Kind == PdfLineKind.Chorus ? ChorusIndent : LyricsIndent;
            var x = Margin + indent;

            if (line.Kind == PdfLineKind.Lyric && TrySplitVersePrefix(line.Text, out var prefix, out var remainder))
            {
                DrawAlignedText(canvas, prefix, x, y, SKTextAlign.Left, verseNumberFont, verseNumberPaint);
                var prefixWidth = verseNumberFont.MeasureText(prefix, verseNumberPaint);
                DrawAlignedText(canvas, remainder, x + prefixWidth + 4f, y, SKTextAlign.Left, font, paint);
            }
            else
            {
                DrawAlignedText(canvas, line.Text, x, y, SKTextAlign.Left, font, paint);
            }

            y += line.Height + line.SpacingAfter;
        }
    }

    private static void DrawFooter(SKCanvas canvas, int currentPage, int totalPages, SKPaint footerPaint, SKFont footerFont, SKPaint captionPaint, SKFont captionFont)
    {
        var footerY = PageHeight - 18f;
        DrawAlignedText(canvas, HeaderCaption, Margin, footerY, SKTextAlign.Left, captionFont, captionPaint);
        DrawAlignedText(canvas, $"{currentPage} / {totalPages}", PageWidth - Margin, footerY, SKTextAlign.Right, footerFont, footerPaint);
    }

    private static float GetUsableHeight(float headerHeight)
    {
        return PageHeight - (Margin + headerHeight) - FooterHeight;
    }

    private static List<PdfLine> BuildWrappedLines(IEnumerable<string> paragraphs)
    {
        using var lyricsPaint = CreatePaint(SKColors.Black);
        using var lyricsFont = CreateFont(LyricsFontSize, false);
        using var chorusPaint = CreatePaint(new SKColor(35, 52, 88));
        using var chorusFont = CreateFont(LyricsFontSize, true);

        var maxWidth = PageWidth - (Margin * 2) - LyricsIndent;
        var maxChorusWidth = PageWidth - (Margin * 2) - ChorusIndent;
        var wrapped = new List<PdfLine>();
        var previousWasEmpty = true;

        var inChorusBlock = false;

        foreach (var paragraph in paragraphs)
        {
            if (string.IsNullOrWhiteSpace(paragraph))
            {
                if (previousWasEmpty == false)
                {
                    wrapped.Add(new PdfLine(string.Empty, PdfLineKind.Empty, 0f, SectionSpacing));
                }

                previousWasEmpty = true;
                inChorusBlock = false;
                continue;
            }

            var startsNewVerse = StartsNewVerse(paragraph);
            var startsChorus = IsChorusLine(paragraph);
            if (startsChorus)
            {
                inChorusBlock = true;
            }
            else if (startsNewVerse)
            {
                inChorusBlock = false;
            }

            var isChorus = inChorusBlock;
            var words = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var currentLine = new StringBuilder();
            var paint = isChorus ? chorusPaint : lyricsPaint;
            var font = isChorus ? chorusFont : lyricsFont;
            var lineWidth = isChorus ? maxChorusWidth : maxWidth;
            var lineKind = isChorus ? PdfLineKind.Chorus : PdfLineKind.Lyric;

            foreach (var word in words)
            {
                var candidate = currentLine.Length == 0 ? word : $"{currentLine} {word}";
                if (font.MeasureText(candidate, paint) <= lineWidth)
                {
                    currentLine.Clear();
                    currentLine.Append(candidate);
                }
                else
                {
                    if (currentLine.Length > 0)
                    {
                        wrapped.Add(new PdfLine(currentLine.ToString(), lineKind, LyricsLineHeight, BlockSpacing));
                    }

                    currentLine.Clear();
                    currentLine.Append(word);
                }
            }

            if (currentLine.Length > 0)
            {
                wrapped.Add(new PdfLine(currentLine.ToString(), lineKind, LyricsLineHeight, BlockSpacing));
            }

            previousWasEmpty = false;
        }

        TrimTrailingEmptyLines(wrapped);
        return wrapped;
    }

    private static List<List<PdfLine>> PaginateLines(IReadOnlyList<PdfLine> lines, float headerHeight)
    {
        var pages = new List<List<PdfLine>>();
        var currentPage = new List<PdfLine>();
        var currentHeight = 0f;
        var usableHeight = GetUsableHeight(headerHeight);

        foreach (var line in lines)
        {
            var lineHeight = line.Height + line.SpacingAfter;

            if (currentPage.Count > 0 && currentHeight + lineHeight > usableHeight)
            {
                TrimTrailingEmptyLines(currentPage);
                pages.Add(currentPage);
                currentPage = [];
                currentHeight = 0f;
            }

            if (currentPage.Count == 0 && line.Kind == PdfLineKind.Empty)
            {
                continue;
            }

            currentPage.Add(line);
            currentHeight += lineHeight;
        }

        if (currentPage.Count > 0)
        {
            TrimTrailingEmptyLines(currentPage);
            pages.Add(currentPage);
        }

        if (pages.Count == 0)
        {
            pages.Add([]);
        }

        return pages;
    }

    private static float CalculateHeaderHeight(int titleLineCount)
    {
        var extraTitleLines = Math.Max(0, titleLineCount - 1);
        return HeaderBaseHeight + (extraTitleLines * 24f);
    }

    private static void TrimTrailingEmptyLines(List<PdfLine> lines)
    {
        while (lines.Count > 0 && lines[^1].Kind == PdfLineKind.Empty)
        {
            lines.RemoveAt(lines.Count - 1);
        }
    }

    private static bool IsChorusLine(string text)
    {
        var value = text.TrimStart();
        return value.StartsWith("R:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("R1:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("R2:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("R3:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("R4:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("R5:", StringComparison.OrdinalIgnoreCase)
               || value.StartsWith("/:", StringComparison.OrdinalIgnoreCase);
    }

    private static bool StartsNewVerse(string text)
    {
        var value = text.TrimStart();
        var dotIndex = value.IndexOf(". ", StringComparison.Ordinal);
        if (dotIndex <= 0)
        {
            return false;
        }

        return value[..dotIndex].All(char.IsDigit);
    }

    private static bool TrySplitVersePrefix(string text, out string prefix, out string remainder)
    {
        prefix = string.Empty;
        remainder = text;

        var trimmed = text.TrimStart();
        var dotIndex = trimmed.IndexOf(". ", StringComparison.Ordinal);
        if (dotIndex <= 0)
        {
            return false;
        }

        var candidate = trimmed[..dotIndex];
        if (candidate.All(char.IsDigit) == false)
        {
            return false;
        }

        prefix = $"{candidate}.";
        remainder = trimmed[(dotIndex + 2)..];
        return true;
    }

    private static void DrawAlignedText(SKCanvas canvas, string text, float x, float y, SKTextAlign align, SKFont font, SKPaint paint)
    {
        var measuredWidth = font.MeasureText(text, paint);
        var drawX = align switch
        {
            SKTextAlign.Center => x - measuredWidth / 2f,
            SKTextAlign.Right => x - measuredWidth,
            _ => x
        };

        canvas.DrawText(text, new SKPoint(drawX, y), font, paint);
    }

    private static List<string> WrapText(string text, SKFont font, SKPaint paint, float maxWidth)
    {
        var wrapped = new List<string>();
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            var candidate = currentLine.Length == 0 ? word : $"{currentLine} {word}";
            if (font.MeasureText(candidate, paint) <= maxWidth)
            {
                currentLine.Clear();
                currentLine.Append(candidate);
            }
            else
            {
                if (currentLine.Length > 0)
                {
                    wrapped.Add(currentLine.ToString());
                }

                currentLine.Clear();
                currentLine.Append(word);
            }
        }

        if (currentLine.Length > 0)
        {
            wrapped.Add(currentLine.ToString());
        }

        return wrapped.Count == 0 ? [text] : wrapped;
    }

    private static string NormalizeContent(string? content)
    {
        return (content ?? string.Empty)
            .Replace("\\r\\n", "\n")
            .Replace("\\n", "\n")
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");
    }

    private enum PdfLineKind
    {
        Lyric,
        Chorus,
        Empty
    }

    private readonly record struct PdfLine(string Text, PdfLineKind Kind, float Height, float SpacingAfter);
}
