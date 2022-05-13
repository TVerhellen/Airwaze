using System.ComponentModel.DataAnnotations;

namespace AirWaze.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public string TicketNr { get; set; }
        public Flight CurrentFlight { get; set; }
        public User CurrentUser { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal Price { get; set; }
        public bool FirstClass { get; set; }
        public string Seat { get; set; }
        public bool ExtraLuggage { get; set; }
        public int Status { get; set; }
        /* 
         * 0 = generated, not paid
         * 1 = generated, paid
         * 2 = checked in
         * 3 = boarded
         * 4 = refunded (customer request)
         * 5 = cancelled (airport request)
         */
    }
}
