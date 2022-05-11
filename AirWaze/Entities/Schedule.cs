namespace AirWaze.Entities
{
    public class Schedule
    {
        public DateTime Date { get; set; }
        public List<Flight> Flights { get; set; }
        public bool IsValidated { get; set; }
    }
}
