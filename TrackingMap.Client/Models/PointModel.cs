namespace TrackingMap.Components.Models
{
    public class PointModel(double latitude, double longitude)
    {
        public double Latitude { get; set; } = latitude;
        public double Longitude { get; set; } = longitude;

        public override string ToString()
        {
            return "Lat = " + Latitude + " Lon = " + Longitude;
        }
    }
}
