using AirWaze.Entities;

namespace AirWaze.Models
{
    public class FlightEditViewModel
    {
        public int FlightID { get; set; }
        public string FlightNr { get; set; }
        public Plane CurrentPlane { get; set; }
        public DateTime Departure { get; set; }
        public Destination Destination { get; set; }
        public Gate? CurrentGate { get; set; }
        public Runway? CurrentRunway { get; set; }
        public int Status { get; set; }
    }
}
