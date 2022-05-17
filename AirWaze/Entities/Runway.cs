using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Runway
    {
        [Key]
        public int RunwayID { get; set; }
        public int Number { get; set; }
        [NotMapped]
        public bool IsAvailable { get; set; }
        [NotMapped]
        public Flight? CurrentFlight { get; set; }
    }
}
