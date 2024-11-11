using InfluxDB.Client.Core;
using NodaTime;

namespace TrackingMap.Components.Models
{
    [Measurement("haultrac")]
    public class InfluxDBRecord
    {
        [Column("_table")]
        public String table { get; set; }

        [Column("_volume")]
        public String volume { get; set; }

        [Column("_timeBegin")]
        public Instant timeBegin { get; set; }

        [Column("_timeEnd")]
        public Instant timeEnd { get; set; }

        [Column("_time")]
        public Instant time { get; set; }

        [Column("_value")]
        public Double value { get; set; }

        [Column("_field")]
        public String name { get; set; }

        [Column("_device")]
        public String imei { get; set; }

        [Column("type")]
        public String type { get; set; }
    }
}
