using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Gate
    {
        [Key]
        public int GateID { get; set; }
        public int Number { get; set; }
        [NotMapped]
        public int Queue { get; set; }
        [NotMapped]
        public Flight? CurrentFlight { get; set; }
        [NotMapped]
        public bool IsAvailable { get; set; }        
        public double CoordsLat { get; set; }       
        public double CoordsLon { get; set; }
    }
}
