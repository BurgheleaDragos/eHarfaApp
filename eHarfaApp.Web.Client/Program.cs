using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using eHarfaApp.Shared.Services;
using eHarfaApp.Web.Client.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

// Add device-specific services used by the eHarfaApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddSingleton<SqliteDatabase>();
builder.Services.AddScoped<IPdfExportService, WebPdfExportService>();
builder.Services.AddSingleton<ISongService, SongService>();
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<IApiService, ApiService>();

await builder.Build().RunAsync();
