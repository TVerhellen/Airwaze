using AirWaze.Entities;

namespace AirWaze.Models
{
    public class TicketListViewModel
    {
        public string TicketNr { get; set; }
        public Flight CurrentFlight { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Status { get; set; }
    }
}
