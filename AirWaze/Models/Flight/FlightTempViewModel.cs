namespace AirWaze.Models
{
    public class FlightTempViewModel
    {
        public TimeSpan FlightTime { get; set; }
        public DateTime Departure { get; set; }
        public decimal Distance { get; set; }
        public string? Destination { get; set; }
    }
}
