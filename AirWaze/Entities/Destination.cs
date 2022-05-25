using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Destination
    {
        [Key]
        public int DestinationID { get; set; }
        public string Name { get; set; }
        public decimal Distance { get; set; }
        public string Region { get; set; }
        public TimeSpan FlightTime { get; set; }
    }
}
