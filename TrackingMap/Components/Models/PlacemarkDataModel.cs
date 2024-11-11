namespace TrackingMap.Components.Models
{
    public class PlacemarkDataModel
    {
        public DateTime Time { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string DisplayText => $"Car was at {Latitude} and {Longitude} ft. at {Time}.";
    }
}
