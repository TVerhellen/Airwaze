using AirWaze.Entities;

namespace AirWaze.Models
{
    public class ScheduleGenerateViewModel
    {
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public List<Flight>? Flights { get; set; }
        public bool IsValidated { get; set; }
    }
}
