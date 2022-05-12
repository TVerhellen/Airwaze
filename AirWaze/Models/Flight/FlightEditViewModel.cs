using AirWaze.Entities;

namespace AirWaze.Models
{
    public class FlightEditViewModel
    {
        public string FlightNr { get; set; }
        public Plane CurrentPlane { get; set; }
        public TimeSpan FlightTime { get; set; }
        public DateTime Departure { get; set; }
        public decimal Distance { get; set; }
        public string Destination { get; set; }
        public Gate CurrentGate { get; set; }
        public Runway CurrentRunway { get; set; }
        public int Status { get; set; }
    }
}
