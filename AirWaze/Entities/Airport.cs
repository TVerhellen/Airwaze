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
        private static Random generator = new Random();
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

        public static List<Flight> Flights { get; set; }
        public static List<Runway> Runways { get; set; }

        public static List<Plane> Planes { get; set; }   
        public static Schedule CurrentSchedule { get; set; }

        public static Schedule ScheduleToApprove { get; set; }

        public static List<Schedule> ComingSchedules { get; set; }

        public static void UpdateAirport()
        {
            myDatabase = AirlineController._myDatabase;
            if (Runways == null)
            {
                Runways = myDatabase.GetRunways();
                foreach(Runway x in Runways)
                {
                    x.IsAvailable = true;
                }
            }
            if (Gates == null)
            {
                Gates = myDatabase.GetGates();
                foreach(Gate x in Gates)
                {
                    x.IsAvailable = true;
                }
            }           
            Flights = myDatabase.GetFlights();
            Planes = myDatabase.GetPlanes();          
        }
        public static void AddGate()
        {
            Gate thisgate = new Gate();
            thisgate.GateID = Gates.Count +1;
            thisgate.Number = Gates.Count +1;
            myDatabase.AddGate(thisgate);
            Gates.Add(thisgate); 
        }

        public static void AddRunway()
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
            theseflights = (List<Flight>)theseflights.OrderBy(flight => flight.Departure);
            for (int i = 0; i < 25; i++)
            {
                theseflights.Add(Flights[i]);
            }
            for (int i = 0; i < Gates.Count; i++)
            {
                theseflights[i].CurrentGate = Gates[i];
                Gates[i].IsAvailable = false;
                Gates[i].CurrentFlight = theseflights[i];
            }
            for (int i = 0; i < Runways.Count; i++)
            {
                theseflights[i].CurrentRunway = Runways[i];
                Runways[i].IsAvailable = false;
                Runways[i].CurrentFlight = theseflights[i];
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
        public static Schedule ConfirmSchedule(Schedule thisschedule)
        {
            return thisschedule;
        }
    }

}
