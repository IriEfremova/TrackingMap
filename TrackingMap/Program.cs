using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using TrackingMap.Client.Pages;
using TrackingMap.Client.Services;
using TrackingMap.Components;
using TrackingMap.Components.Models;
using TrackingMap.Components.Pages;
using TrackingMap.Components.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IInfluxInfoService, InfluxInfoService>();
builder.Services.AddSingleton<IInfluxService, ServerInfluxService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapPost("/api/data/", (IInfluxService influxService, InfluxConnectionData data) =>
{
    Console.WriteLine("MapPost = " + data.ToString());
    influxService.SetConnectionData(data);
});

app.MapGet("/api/report/", (IInfluxService influxService) =>
{
    return influxService.GetDataForReport();
});

app.MapGet("/api/map/", (IInfluxService influxService, IInfluxInfoService influxInfoService) =>
{
    return influxService.GetDataForMap();
});


app.MapRazorComponents<App>()
     .AddInteractiveWebAssemblyRenderMode()
        .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(TrackingMap.Client._Imports).Assembly);
   // .AddAdditionalAssemblies(typeof(Report).Assembly);

app.Run();

