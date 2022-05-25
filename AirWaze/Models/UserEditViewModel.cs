using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AirWaze.Models
{
    public class UserEditViewModel
    {
        [DisplayName("ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ID is verplicht!")]
        public string UserID { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname is required!")]
        public string LastName { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname is required!")]
        public string FirstName { get; set; }

        [Display(Name = "Emailaddress")]
        [Required(ErrorMessage = "The emailaddress is required")]
        [EmailAddress(ErrorMessage = "Invalid Emailaddress")]
        public string? Email { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Streetname is required!")]
        public string StreetName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Housenumber error")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Housenumber is required!")]
        public int HouseNumber { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Bus error")]
        public string? Bus { get; set; }

        [Required(ErrorMessage = "Zipcode is Required")]
        [DataType(DataType.PostalCode)]
        public string Zipcode { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required!")]
        public string City { get; set; }

        [MinLength(1, ErrorMessage = "Minimum 1 character!")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required!")]
        public string Country { get; set; }

        [Required(ErrorMessage = "You must provide a phonenumber")]
        [Display(Name = "Homephone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phonenumber")]
        public string? PhoneNumber { get; set; }
    }
}
