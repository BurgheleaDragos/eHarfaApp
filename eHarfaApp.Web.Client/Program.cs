using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using eHarfaApp.Shared.Services;
using eHarfaApp.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the eHarfaApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();