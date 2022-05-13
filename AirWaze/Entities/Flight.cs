namespace AirWaze.Entities
{
    public class Flight
    {
        public string FlightNr { get; set; }
        public Plane? CurrentPlane { get; set; }
        public TimeSpan FlightTime { get; set; }
        public DateTime Departure { get; set; }
        public decimal Distance { get; set; }
        public string Destination { get; set; }
        public Gate? CurrentGate { get; set; }
        public Runway? CurrentRunway { get; set; }
        public int Status { get; set; }
        /* 
          0 = Generated
          1 = Delayed
          2 = Boarding
          3 = Departed
          4 = Arrived
          5 = Cancelled
         */


        public override string ToString()
        {
            return $"{Destination} {Departure}";
        }
    }
}
