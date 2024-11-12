using System.Net.Http.Json;
using TrackingMap.Components.Models;
using NodaTime;
using System.Net.Http;
using System;

namespace TrackingMap.Client.Services
{
    public interface IInfluxService
    {
        Task<Dictionary<String, Dictionary<String, Double>>> GetDataForReport();
        Task<Dictionary<DateTime, PointModel>> GetDataForMap();

        Task SetConnectionData(InfluxConnectionData connectionData);
    }

    public class InfluxService(HttpClient httpClient) : IInfluxService
    {
        public async Task SetConnectionData(InfluxConnectionData connectionData)
        {
            Console.WriteLine("InfluxService SetConnectionData = " + connectionData.ToString());
            await httpClient.PostAsJsonAsync("/api/data", connectionData);
        }

        public async Task<Dictionary<String, Dictionary<String, Double>>> GetDataForReport()
        {
            return await httpClient.GetFromJsonAsync<Dictionary<String, Dictionary<String, Double>>>("/api/report") ?? [];
        }

        public async Task<Dictionary<DateTime, PointModel>> GetDataForMap()
        {
            Console.WriteLine("InfluxService GetDataForMap");
            return await httpClient.GetFromJsonAsync<Dictionary<DateTime, PointModel>>("/api/map") ?? [];
        }
    }
}
