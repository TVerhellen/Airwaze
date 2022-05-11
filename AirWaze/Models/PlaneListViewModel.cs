using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneListViewModel
    {
        public int PlaneID { get; set; }
        public Airline CurrentAirline { get; set; }
        public int PassengerCapacity { get; set; }
        public enum FlightRegion { }
        public bool IsAvailable { get; set; }

    }
}
