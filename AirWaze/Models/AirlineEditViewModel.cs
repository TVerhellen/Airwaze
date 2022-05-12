using AirWaze.Entities;
using System.ComponentModel.DataAnnotations;

namespace AirWaze.Models
{
    public class AirlineEditViewModel
    {
        public Guid? AirlineID { get; set; }


        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        public string NameTag { get; set; }


        [Required]
        [StringLength(30)]
        public string CompanyNumber { get; set; }

        public List<Plane>? CurrentPlanes { get; set; }


        [Required]
        [StringLength(30)]
        public string Adress { get; set; }

        [Required]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        //public List<Invoice> ListInvoices { get; set; }

        public string? Logo { get; set; }
    }
}
