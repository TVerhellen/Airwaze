using AirWaze.Entities;

namespace AirWaze.Models
{
    public class PlaneDeleteViewModel
    {
        public string PlaneNr { get; set; }

        public Airline? CurrentAirline { get; set; }
       
        public string Type { get; set; }

        public string Manufacturer { get; set; }

       
    }
}
