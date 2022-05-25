using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AirWaze.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AirWazeUser class
public class AirWazeUser : IdentityUser
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string StreetName { get; set; }
    public int HouseNumber { get; set; }
    public string? Bus { get; set; }
    public string Zipcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

