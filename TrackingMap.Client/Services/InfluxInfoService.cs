using TrackingMap.Components.Models;

namespace TrackingMap.Client.Services
{
    public interface IInfluxInfoService
    {
        public List<InfluxConnectionData> GetAllInfluxServices();
        public InfluxConnectionData GetCurrentInfluxService();
        public void SetCurrentInfluxService(string CompanyName);
    }

    public class InfluxInfoService : IInfluxInfoService
    {
        private readonly List<InfluxConnectionData> _connectionList;
        private InfluxConnectionData? _currentConnection = null;

        public InfluxInfoService(IConfiguration configuration)
        {
            Console.WriteLine("InfluxInfoService Constuctor");

            _connectionList = [];

            IConfigurationSection configSection = configuration.GetSection("InfluxServers");
            foreach (var section in configSection.GetChildren())
            {
                Console.WriteLine("InfluxInfoService Constuctor = " + section.Key);
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
                    _connectionList.Add(data);
            }

            if (_connectionList.Count > 0)
                _currentConnection = _connectionList[0];
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
