using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneEditViewModel
    {
       
        public string PlaneNr { get; set; }

        public Airline? CurrentAirline { get; set; }

        public int PassengerCapacity { get; set; }

        public decimal FuelCapacity { get; set; }

        public string Type { get; set; }

        public string Manufacturer { get; set; }

        public string FlightRegion { get; set; }

        public int FirstClassCapacity { get; set; }

        public decimal LoadCapacity { get; set; }

        public decimal FuelUsagePerKM { get; set; }

        public string[,]? SeatDiagram { get; set; }

        public string? SeatDiagramPic { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime ConstructionYear { get; set; }

        public int AirMiles { get; set; }

        public int FlightHours { get; set; }

        public int NextMainentance { get; set; }

    }
}
