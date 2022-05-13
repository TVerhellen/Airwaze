using System.ComponentModel.DataAnnotations;

namespace AirWaze.Entities
{
    public class Gate
    {
        [Key]
        public int GateID { get; set; }
        public int Number { get; set; }
        public int Queue { get; set; }
        public Flight? CurrentFlight { get; set; }
        public bool IsAvailable { get; set; }
    }
}
