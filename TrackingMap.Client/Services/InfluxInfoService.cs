using TrackingMap.Components.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace TrackingMap.Client.Services
{
    public interface IInfluxInfoService
    {
        public List<InfluxConnectionData> GetAllInfluxServices();
        public InfluxConnectionData GetCurrentInfluxService();
        public void SetCurrentInfluxService(string CompanyName);
        public Task<List<InfluxConnectionData>> GetInfluxServersList();

	}

    public class InfluxInfoService : IInfluxInfoService
    {
        private readonly List<InfluxConnectionData> _connectionList;
        private InfluxConnectionData? _currentConnection = null;
        private IConfiguration _configuration;

        public InfluxInfoService(IConfiguration configuration)
        {
            this._configuration = configuration;
            Console.WriteLine("InfluxInfoService Constructor");

            _connectionList = [];
        }

        public void initInfluxServicesList()
        {
            IConfigurationSection configSection = _configuration.GetSection("InfluxServers");
            foreach (var section in configSection.GetChildren())
            {
                InfluxConnectionData data = new();
                foreach (var subsection in section.GetChildren())
                {
                    string value = subsection.Value ?? String.Empty;
                    switch (subsection.Key)
                    {
                        case InfluxConnectionData.bucket: data.Bucket = value; break;
                        case InfluxConnectionData.token: data.Token = value; break;
                        case InfluxConnectionData.org: data.Organization = value; break;
                        case InfluxConnectionData.host: data.Host = value; break;
                    }
                }
                if (data.isCorrect())
                {
                    _connectionList.Add(data);
					Console.WriteLine("Influx Server = " + data.ToString());
				}

            }
        }


		public async Task<List<InfluxConnectionData>> GetInfluxServersList()
        {
            if (_connectionList.Count <= 0)
            {
                await Task.Run(() => initInfluxServicesList());
				if (_connectionList.Count > 0)
					_currentConnection = _connectionList[0];
			}
			return _connectionList;
        }

        public List<InfluxConnectionData> GetAllInfluxServices()
        {
            return _connectionList;
        }

        public InfluxConnectionData GetCurrentInfluxService()
        {
            return _currentConnection ?? new InfluxConnectionData();
        }

        public void SetCurrentInfluxService(string OrganizationName)
        {
            int num = 0;
            foreach (var service in _connectionList)
            {
                if (service.Organization.Equals(OrganizationName))
                {
                    _currentConnection = _connectionList[num];
                }
                ++num;
            }
        } 
    }
}
