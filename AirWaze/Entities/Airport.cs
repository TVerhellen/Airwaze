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

        public static Timer aTimer;

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

        public static List<Flight> Flights { get; set; }
        public static List<Runway> Runways { get; set; }

        public static List<Plane> Planes { get; set; }   
        public static Schedule CurrentSchedule { get; set; }

        public static Schedule ScheduleToApprove { get; set; }

        public static List<Schedule> ComingSchedules { get; set; }
        
        public static void StartAirport()
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

        public static void AddRunways()
        {
            Runway thisRunway = new Runway();
            thisRunway.RunwayID = Runways.Count +1;
            thisRunway.Number = Runways.Count +1;
            myDatabase.AddRunway(thisRunway);
            Runways.Add(thisRunway);
        }

        //Alle Shedule Crud -- Admin resticted
        public static Schedule GenerateSchedule()
        {                    
            Schedule myshedule = new Schedule();
            myshedule.Date = _currenttime;
            myshedule.ScheduleID = generator.Next(0, 10000);
            List<Flight> theseflights = new List<Flight>();
            Flights = (List<Flight>)Flights.OrderBy(flight => flight.Departure);

            for (int i = 0; i < 25; i++)
            {
                theseflights.Add(Flights[i]);
            }
            for (int i = 0; i < Gates.Count; i++)
            {
                if (Gates[i].IsAvailable == true)
                {
                    theseflights[i].CurrentGate = Gates[i];
                    Gates[i].IsAvailable = false;
                    Gates[i].CurrentFlight = theseflights[i];
                }               
            }
            for (int i = 0; i < Runways.Count; i++)
            {
                if (Runways[i].IsAvailable == true)
                {
                    theseflights[i].CurrentRunway = Runways[i];
                    Runways[i].IsAvailable = false;
                    Runways[i].CurrentFlight = theseflights[i];
                }
            }
            return myshedule;
        }

        public static void FlightDeparts()
        {

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
