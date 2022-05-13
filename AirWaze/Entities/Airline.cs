using System.ComponentModel.DataAnnotations;

namespace AirWaze.Entities
{
    public class Airline
    {
        [Key]
        public Guid AirlineID { get; set; }

        public string Name { get; set; }

        public string NameTag { get; set; }

        public string CompanyNumber { get; set; }

        public List<Plane> CurrentPlanes { get; set; }

        public string Adress { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string PhoneNumber  { get; set; }

        public string? AdminComments { get; set; }

        public string AccountNumber { get; set; }

        //public List<Invoice> ListInvoices { get; set; }

        public string? Logo { get; set; }   
    }
}
