using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrackingMap.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IInfluxInfoService, InfluxInfoService>();
builder.Services.AddSingleton<IInfluxService, InfluxService>();


await builder.Build().RunAsync();

