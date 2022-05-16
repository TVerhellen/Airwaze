using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        //public string Name { get; set; }
        //public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string? Bus { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
        public List<Ticket>? ListCurrentTickets { get; set; }
        [NotMapped]
        public List<Ticket>? ListPastTickets { get; set; }
        [NotMapped]
        public List<bool> Milestones { get; set; }
        public string? AdminComments { get; set; }
    }
}
