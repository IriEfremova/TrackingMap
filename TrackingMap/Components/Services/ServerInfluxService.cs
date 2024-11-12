using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using NodaTime;
using TrackingMap.Client.Services;
using TrackingMap.Components.Models;

namespace TrackingMap.Components.Services
{
    public class ServerInfluxService : IInfluxService
    {
        private InfluxConnectionData _connectionData;
        public async Task SetConnectionData(InfluxConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<Dictionary<DateTime, PointModel>> GetDataForMap()
        {
            Dictionary<DateTime, PointModel> coordDict = [];

            if (_connectionData == null)
            {
                return [];
            }


            Console.WriteLine("ServerInfluxService GetDataForMap = " + _connectionData.ToString());

            using var client = new InfluxDBClient(_connectionData.Host, token: _connectionData.Token);
            try
            {
                var flux = "from(bucket: \"{0:C0}\") |> range(start: 2024-05-29T04:00:00Z, stop: 2024-05-29T04:30:00Z) " +
                   "|> filter(fn: (r) => r[\"_measurement\"] == \"77777\")" +
                   " |> filter(fn: (r) => r[\"type\"] == \"gps\") |> filter(fn: (r) => r[\"_field\"] == \"lat\"" +
                   " or r[\"_field\"] == \"lon\")" +
                   " |> sort(columns: [\"time\"])";
                String fluxQuery = String.Format(flux, _connectionData.Bucket);
                Console.WriteLine("fluxQuery = " + fluxQuery);

                var tables = await client.GetQueryApi().QueryAsync<InfluxDBRecord>(fluxQuery, _connectionData.Organization);
                Console.WriteLine("List = " + tables.Count);
                client.Dispose();

                foreach (var record in tables)
                {
                   // Console.WriteLine("record = " + record.time + "  " + record.name + "  " + record.value);
                    if (coordDict.ContainsKey(record.time.ToDateTimeUtc()))
                    {
                        if (record.name == "lon")
                        {
                            coordDict[record.time.ToDateTimeUtc()].Longitude = record.value;
                        }
                        if (record.name == "lat")
                        {
                            coordDict[record.time.ToDateTimeUtc()].Latitude = record.value;
                        }
                    }
                    else
                    {
                        PointModel pointModel = new( 0, 0 );
                        if (record.name == "lon")
                        {
                            pointModel.Longitude = record.value;
                        }
                        if (record.name == "lat")
                        {
                            pointModel.Latitude = record.value;
                        }
                        //Console.WriteLine("PointModel = " + pointModel);
                        coordDict.Add(record.time.ToDateTimeUtc(), pointModel);
                    }
                    //Console.WriteLine("Param = " + p.name + "  " + p.value);
                }
                Console.WriteLine("coordDict = " + coordDict.Count);
                /*
                foreach (var p in coordDict)
                {
                    Console.WriteLine($"key: {p.Key}  value: {p.Value}");
                }
                */
                return coordDict;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXEPTION = " + ex.Message);
                client.Dispose();
                return [];
            }
        }

        public async Task<Dictionary<String, Dictionary<String, Double>>> GetDataForReport()
        {
            //InfluxConnectionData connectionData = new InfluxConnectionData();
            if (_connectionData == null)
                return [];

            Console.WriteLine("ServerInfluxService CreateReport = " + _connectionData.ToString());

            using var client = new InfluxDBClient(_connectionData.Host, token: _connectionData.Token);
            try
            {
                 var flux = "from(bucket: \"{0:C0}\") |> range(start: 2024-05-25T04:00:00Z, stop: 2024-05-29T04:10:00Z) " +
                    "|> filter(fn: (r) => r[\"_measurement\"] == \"77777\")" +
                    " |> filter(fn: (r) => r[\"type\"] == \"tech_data\") |> filter(fn: (r) => r[\"_field\"] == \"accelerator_2\"" +
                    " or r[\"_field\"] == \"accelerator_1\" or r[\"_field\"] == \"accelerator_0\")";
                String fluxQuery = String.Format(flux, _connectionData.Bucket);
                Console.WriteLine("fluxQuery = " + fluxQuery);

                var tables = await client.GetQueryApi().QueryAsync<InfluxDBRecord>(fluxQuery, _connectionData.Organization);
                Console.WriteLine("List = " + tables.Count);
                client.Dispose();
                return [];
                /*
                foreach (var p in tables)
                {
                    Console.WriteLine("Param = " + p.name + "  " + p.value);
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXEPTION = " + ex.Message);
                client.Dispose();
            }
            return [];
        } 
    }
}
