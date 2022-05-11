using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneListViewModel
    {       
        public string PlaneNr { get; set; }
        public Airline CurrentAirline { get; set; }
        public int PassengerCapacity { get; set; }
        public string FlightRegion { get; set; }
        public bool IsAvailable { get; set; }

    }
}
