namespace TrackingMap.Components.Models
{
    public class InfluxConnectionData
    {
        public const string bucket = "bucket";
        public const string org = "org";
        public const string host = "host";
        public const string token = "token";

        public string Bucket { get; set; } = String.Empty;
        public string Organization { get; set; } = String.Empty;
        public string Host { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;

        public bool isCorrect()
        {
            return Bucket.Length != 0 && Organization.Length != 0 && Host.Length != 0 && Token.Length != 0;
        }

        public Dictionary<string, string> getConnectionData()
        {
            Console.WriteLine("getConnectionData() = " + Bucket);
            if (Bucket.Length != 0 && Organization.Length != 0 && Host.Length != 0 && Token.Length != 0)
                return new Dictionary<string, string> { { bucket, Bucket }, { org, Organization }, { host, Host }, { token, Token } };
            else return new Dictionary<string, string> { };
        }

        public override string ToString()
        {
            return string.Format("Bucket: {0}  Organization: {1}  Host: {2}  Token: {3}", Bucket, Organization, Host, Token);
        }
    }
}
