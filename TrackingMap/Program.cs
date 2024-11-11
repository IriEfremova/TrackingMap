using TrackingMap.Client.Pages;
using TrackingMap.Client.Services;
using TrackingMap.Components;
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

app.MapGet("/api/report/", (IInfluxService influxService, IInfluxInfoService influxInfoService) =>
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

