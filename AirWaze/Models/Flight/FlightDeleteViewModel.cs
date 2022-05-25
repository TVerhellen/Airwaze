using AirWaze.Entities;

namespace AirWaze.Models
{
    public class FlightDeleteViewModel
    {
        public int FlightID { get; set; }
        public string FlightNr { get; set; }
        public DateTime Departure { get; set; }
        public Destination Destination { get; set; }
        public int Status { get; set; }
    }
}
