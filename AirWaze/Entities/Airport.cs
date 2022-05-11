namespace AirWaze.Entities
{
    public static class Airport
    {
        public static string Name { get; set; }
        public static string Address { get; set; }
        public static List<Gate> Gates { get; set; }
        public static List<Runway> Runways { get; set; }
        public static Schedule CurrentSchedule { get; set; }

    }
}
