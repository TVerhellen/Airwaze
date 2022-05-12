namespace AirWaze.Entities
{
    public class Gate
    {
        public int Number { get; set; }
        public int Queue { get; set; }
        public Flight? CurrentFlight { get; set; }
        public bool IsAvailable { get; set; }
    }
}
