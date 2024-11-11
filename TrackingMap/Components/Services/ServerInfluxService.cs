using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using NodaTime;
using TrackingMap.Client.Services;
using TrackingMap.Components.Models;

namespace TrackingMap.Components.Services
{
    public class ServerInfluxService : IInfluxService
    {
        public async Task<Dictionary<DateTime, PointModel>> GetDataForMap()
        {
            Dictionary<DateTime, PointModel> coordDict = [];

            Console.WriteLine("InfluxService GetDataForMap");
            InfluxConnectionData connectionData = new InfluxConnectionData();
            /*
            string host = connectionData.Host;
            string token = connectionData.Token;
            string organization = connectionData.Organization;
            string bucket = connectionData.Bucket;
            */
            const string host = "http://5.2.39.199:8086";
            const string token = "p2TJ3ZIErklEF87E80yM5bKkE4AyINPIRTY_rbz41xhLPjnoqADigr2Krv_4sr5vitRn_nelfhW5qHBJn3SSfw==";
            string organization = "haultrac";
            string bucket = "company-1";

            using var client = new InfluxDBClient(host, token: token);
            try
            {
                var flux = "from(bucket: \"{0:C0}\") |> range(start: 2024-05-29T04:00:00Z, stop: 2024-05-29T04:30:00Z) " +
                   "|> filter(fn: (r) => r[\"_measurement\"] == \"869101051668305\")" +
                   " |> filter(fn: (r) => r[\"type\"] == \"gps\") |> filter(fn: (r) => r[\"_field\"] == \"lat\"" +
                   " or r[\"_field\"] == \"lon\")" +
                   " |> sort(columns: [\"time\"])";
                String fluxQuery = String.Format(flux, bucket);
                Console.WriteLine("fluxQuery = " + fluxQuery);

                var tables = await client.GetQueryApi().QueryAsync<InfluxDBRecord>(fluxQuery, organization);
                Console.WriteLine("List = " + tables.Count);
                client.Dispose();

                //PointModel pointModel1 = new();
                //coordDict.Add(tables[0].time.ToDateTimeUtc(), pointModel1);
                //return coordDict;

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
            InfluxConnectionData connectionData = new InfluxConnectionData();
            Console.WriteLine("InfluxService CreateReport");
            /*
            string host = connectionData.Host;
            string token = connectionData.Token;
            string organization = connectionData.Organization;
            string bucket = connectionData.Bucket;
            */
            const string host = "http://5.2.39.199:8086";
            const string token = "p2TJ3ZIErklEF87E80yM5bKkE4AyINPIRTY_rbz41xhLPjnoqADigr2Krv_4sr5vitRn_nelfhW5qHBJn3SSfw==";
            string organization = "haultrac";
            string bucket = "company-1";

            using var client = new InfluxDBClient(host, token: token);
            try
            {
                 var flux = "from(bucket: \"{0:C0}\") |> range(start: 2024-05-25T04:00:00Z, stop: 2024-05-29T04:10:00Z) " +
                    "|> filter(fn: (r) => r[\"_measurement\"] == \"869101051668305\")" +
                    " |> filter(fn: (r) => r[\"type\"] == \"tech_data\") |> filter(fn: (r) => r[\"_field\"] == \"accelerator_2\"" +
                    " or r[\"_field\"] == \"accelerator_1\" or r[\"_field\"] == \"accelerator_0\")";
                String fluxQuery = String.Format(flux, bucket);
                Console.WriteLine("fluxQuery = " + fluxQuery);

                var tables = await client.GetQueryApi().QueryAsync<InfluxDBRecord>(fluxQuery, organization);
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
