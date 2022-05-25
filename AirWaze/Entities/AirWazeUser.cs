namespace AirWaze.Entities
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string? Bus { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
