namespace AirWaze.Entities
{
    public class Flight
    {
        public int FlightID { get; set; }
        public string FlightNr { get; set; }
        public Plane CurrentPlane { get; set; }
        public TimeSpan FlightTime { get; set; }
        public DateTime Departure { get; set; }
        public List<Ticket> ListTickets { get; set; }
        public decimal Distance { get; set; }
        public string Destination { get; set; }
        public bool IsCancelled { get; set; }
        public Gate CurrentGate { get; set; }
        public Runway CurrentRunway { get; set; }
        public bool IsCompleted { get; set; }
    }
}
