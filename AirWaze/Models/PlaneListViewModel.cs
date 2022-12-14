using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneListViewModel
    {       
        public string PlaneNr { get; set; }
        public Airline? CurrentAirline { get; set; }

        public string Manufacturer { get; set; }

        public string Type { get; set; }
        public int PassengerCapacity { get; set; }
        public string FlightRegion { get; set; }
        public bool IsAvailable { get; set; }

        public DateTime ConstructionYear { get; set; }

        public int AirMiles { get; set; }

        public int FlightHours { get; set; }

        public int NextMainentance { get; set; }


    }
}
