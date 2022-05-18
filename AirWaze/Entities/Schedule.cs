using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public List<Flight> Flights { get; set; }
        public bool IsValidated { get; set; }
    }
}
