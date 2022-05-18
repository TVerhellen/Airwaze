using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Plane
    {
        [Key]
        public int PlaneID { get; set; }
        public string PlaneNr { get; set; }

        public Airline CurrentAirline { get; set; }

        public int PassengerCapacity { get; set; }

        public decimal FuelCapacity { get; set; }

        public string Type { get; set; }

        public string Manufacturer { get; set; }

        public string FlightRegion { get; set; }

        public int FirstClassCapacity { get; set; }

        public decimal LoadCapacity { get; set; }

        public decimal FuelUsagePerKM { get; set; }
        [NotMapped]
        public string[,]? SeatDiagram { get; set; }
        public bool IsAvailable { get; set; }
        [NotMapped]
        public string? SeatDiagramPic { get; set; }
        [NotMapped]
        public DateTime ConstructionYear { get; set; }
        [NotMapped]
        public int AirMiles { get; set; }
        [NotMapped]
        public int FlightHours { get; set; }
        [NotMapped]
        public int NextMainentance { get; set; }
    }
}
