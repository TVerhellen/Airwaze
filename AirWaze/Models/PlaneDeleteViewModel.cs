using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneDeleteViewModel
    {
        public int PlaneID { get; set; }
       
        public Airline CurrentAirline { get; set; }
       
        public string Type { get; set; }

        public string Manufacturer { get; set; }

       
    }
}
