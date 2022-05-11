using AirWaze.Entities;

namespace AirWaze.Models
{
    public class TicketDeleteViewModel
    {
        public string TicketNr { get; set; }
        public Flight CurrentFlight { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
