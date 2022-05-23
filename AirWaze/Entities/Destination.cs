using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Destination
    {
        [Key]
        public int DestinationID { get; set; }
        public int Name { get; set; }
        public int Distance { get; set; }
        public bool Region { get; set; }
    }
}
