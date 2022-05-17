using AirWaze.Database;
using AirWaze.Controllers;
using AirWaze.Database.Design;

namespace AirWaze.Entities
{
    public static class Airport
    {

        private static string _name = "Batman Airport";
        private static string _adress = "Bosdreef 6 Istanbul Turkye";
        private static DateTime _currenttime = DateTime.Now;
        public static IAirWazeDatabase myDatabase;

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
        public static List<Schedule> ComingSchedules { get; set; }

        public static void StartUpAirport()
        {
            myDatabase = AirlineController._myDatabase;
            Runways = myDatabase.GetRunways();
            Gates = myDatabase.GetGates();
        }
        public static void AddGate()
        {
            Gate thisgate = new Gate();
            thisgate.GateID = Guid.NewGuid();
            thisgate.
        }

        public static void AddRunway()
        {
            //ExtraRunwaysAanmaken
        }

        //Alle Shedule Crud -- Admin resticted
        public static void CreateShedule()
        {
            //Shedules maken
        }
        public static void UpdateShedule()
        {
            //Shedules maken
        }
        public static void DeleteShedule()
        {
            //Shedules maken
        }

        //Methods voor Passenger/Airliner hier en dan aanroepen in controller?
        public static void ViewShedulePassenger()
        {

        }
        public static void ViewSheduleAirliner()
        {

        }

        public static Schedule GenerateSchedule()
        {
            Schedule myshedule = new Schedule();
            return myshedule;
        }
        public static Schedule ConfirmSchedule(Schedule thisschedule)
        {
            return thisschedule;
        }
    }

}
