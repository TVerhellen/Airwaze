using AirWaze.Entities;
using System.ComponentModel.DataAnnotations;

namespace AirWaze.Models
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "A flight must be selected!")]
        public Flight CurrentFlight { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }
        public decimal Price { get; set; }
        public bool FirstClass { get; set; }
        public string Seat { get; set; }
        public bool ExtraLuggage { get; set; }
    }
}
