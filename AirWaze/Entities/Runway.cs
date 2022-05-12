namespace AirWaze.Entities
{
    public class Runway
    {
        public int Number { get; set; }
        public bool IsAvailable { get; set; }
        public Flight? CurrentFlight { get; set; }
    }
}
