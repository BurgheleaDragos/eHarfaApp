using eHarfaApp.Shared.DAL;

namespace eHarfaApp.Shared.Services;

public interface IPdfExportService
{
    Task ExportSongAsync(Song song);
    Task<ShareResult> ShareSongAsync(Song song);
}
