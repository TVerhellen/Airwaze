using AirWaze.Entities;

namespace AirWaze.Models
{
    public class TicketDetailViewModel
    {
        public string TicketNr { get; set; }
        public Flight CurrentFlight { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal Price { get; set; }
        public bool FirstClass { get; set; }
        public string Seat { get; set; }
        public bool ExtraLuggage { get; set; }
    }
}
