using AirWaze.Entities;

namespace AirWaze.Models
{
    public class TicketFlightPickerViewModel
    {
        public List<Flight> flightList { get; set; }

        public string TicketNr { get; set; }
    }
}
