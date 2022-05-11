

using AirWaze.Entities;

namespace AirWaze.Models
{
    public class AirlineCreateViewModel
    {
        public Guid AirlineID { get; set; }

        public string Name { get; set; }

        public int CompanyNumber { get; set; }

        public List<Plane> CurrentPlanes { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }
       
        public string AccountNumber { get; set; }

        //public List<Invoice> ListInvoices { get; set; }

        public string? Logo { get; set; }
    }
}
