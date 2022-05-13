using System.ComponentModel.DataAnnotations;

namespace AirWaze.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public List<Flight> Flights { get; set; }
        public bool IsValidated { get; set; }
    }
}
