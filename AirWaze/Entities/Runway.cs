using System.ComponentModel.DataAnnotations;

namespace AirWaze.Entities
{
    public class Runway
    {
        [Key]
        public int RunwayID { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }
        public Flight? CurrentFlight { get; set; }
    }
}
