using AirWaze.Entities;

namespace AirWaze.Models
{
    public class TicketPaymentViewModel
    {
        public string TicketNr { get; set; }
        public Flight CurrentFlight { get; set; }
        public bool FirstClass { get; set; }
        public bool ExtraLuggage { get; set; }
        public decimal Price { get; set; }
    }
}
