namespace AirWaze.Entities
{
    public static class Airport
    {

        private static string _name = "Batman Airport";
        private static string _adress = "Bosdreef 6 Istanbul Turkye";
        private static DateTime _currenttime = DateTime.Now;

        public static string Name
        {
            get { return _name; }           
        }      
        public static string Adress
        {
            get { return _adress; }            
        }
        public static DateTime CurrentTime 
        { 
            get { return _currenttime; }
        }       
        public static List<Gate> Gates { get; set; }
        public static List<Runway> Runways { get; set; }   
        public static Schedule CurrentSchedule { get; set; }
        public static List<Schedule> ApprovedSchedules { get; set; }

        public static void StartUpAirport()
        {
            
        }
        public static void AddGates()
        {
            //Extra Gates aanmaken 
        }

        public static void AddRunways()
        {
            //ExtraRunwaysAanmaken
        }

        //Alle Schedule Crud -- Admin resticted
        public static Schedule GenerateSchedule()
        {
            Schedule newSchedule = new Schedule
            {
                Date = DateTime.Now,
                Flights = new List<Flight>(),
                IsValidated = false                
            };

            foreach(var flight in )

            return newSchedule;
        }
        public static void UpdateSchedule()
        {
            //Shedules maken
        }
        public static void DeleteSchedule()
        {
            //Shedules maken
        }

        //Methods voor Passenger/Airliner hier en dan aanroepen in controller?
        public static void ViewSchedulePassenger()
        {

        }
        public static void ViewScheduleAirliner()
        {

        }
    }

}
