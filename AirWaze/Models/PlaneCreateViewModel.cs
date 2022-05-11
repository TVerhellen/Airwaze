using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneCreateViewModel
    {
        public int PlaneID { get; set; }

        public string PlaneNr { get; set; }

        public Airline CurrentAirline { get; set; }

        public int PassengerCapacity { get; set; }

        public decimal FuelCapacity { get; set; }

        public string Type { get; set; }

        public string Manufacturer { get; set; }

        public enum FlightRegion { }

        public int FirstClassCapacity { get; set; }

        public decimal LoadCapacity { get; set; }

        public decimal FuelUsagePerKM { get; set; }

        public string[,] SeatDiagram { get; set; }
       
    }
}
