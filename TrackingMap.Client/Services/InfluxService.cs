using System.Net.Http.Json;
using TrackingMap.Components.Models;
using NodaTime;

namespace TrackingMap.Client.Services
{
    public interface IInfluxService
    {
        Task<Dictionary<String, Dictionary<String, Double>>> GetDataForReport();
        Task<Dictionary<DateTime, PointModel>> GetDataForMap();
    }

    public class InfluxService(HttpClient httpClient) : IInfluxService
    {
        public async Task<Dictionary<String, Dictionary<String, Double>>> GetDataForReport()
        {
            Console.WriteLine("InfluxService CreateReport");
            return await httpClient.GetFromJsonAsync<Dictionary<String, Dictionary<String, Double>>>("/api/report") ?? [];
        }

        public async Task<Dictionary<DateTime, PointModel>> GetDataForMap()
        {
            Console.WriteLine("InfluxService GetDataForMap");
            return await httpClient.GetFromJsonAsync<Dictionary<DateTime, PointModel>>("/api/map") ?? [];
        }
    }
}
