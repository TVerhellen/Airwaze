using AirWaze.Entities;

namespace AirWaze.Models
{
    public class FlightListViewModel
    {
        public int FlightID { get; set; }
        public string FlightNr { get; set; }
        public TimeSpan FlightTime { get; set; }
        public DateTime Departure { get; set; }
        public string Destination { get; set; }
        public Gate CurrentGate { get; set; }
        public int Status { get; set; }
    }
}
