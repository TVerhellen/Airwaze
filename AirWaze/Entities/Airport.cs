using AirWaze.Database;
using AirWaze.Controllers;
using AirWaze.Database.Design;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Entities
{
    public static class Airport
    {

        private static string _name = "Batman Airport";
        private static string _adress = "Bosdreef 6 Istanbul Turkye";
        private static DateTime _currenttime = DateTime.Now;
        private static Random generator = new Random();
        public static IAirWazeDatabase myDatabase = HomeController._myDatabase;
        public static bool IsOnline = false;

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

        public static List<Schedule>? ApprovedSchedules { get; set; }
        
        public static void StartAirport()
        {
            Runways = myDatabase.GetRunways();
            foreach (Runway x in Runways)
            {
                x.IsAvailable = true;
            }
            Gates = myDatabase.GetGates();
            foreach (Gate x in Gates)
            {
                x.IsAvailable = true;
            }
            Flights = myDatabase.GetFlights();
            Planes = myDatabase.GetPlanes();
            Flights = Flights.FindAll(x => x.Status != 3 || x.Status != 5);
            Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            IsOnline = true;
            
            StartTimer(10000);

        }

        public static void StartTimer(int dueTime)
        {
            aTimer = new Timer(new TimerCallback(TimerProc));
            aTimer.Change(dueTime, 60000);
        }
        private static void TimerProc(object state)
        {
            Timer t = (Timer)state;
            
            //t.Dispose();
            UpdateAirport();
        }
        public static void UpdateAirport()
        {
            //foreach (Flight x in Flights)
            //{
            //    if (x.Departure.To <= _currenttime.Minute)
            //    {
            //        x.Status = 3;
            //        myDatabase.UpdateFlight(x);
            //        CurrentSchedule.Flights.Remove(x);
            //    }
            //    else if (x.Departure.Minute)
            //}
            Flights = FlightController.flights.ToList();
            Planes = PlaneController.planeEntities.ToList();
            Flights = Flights.FindAll(x => x.Status != 3 || x.Status != 5);
            Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            //if (CurrentSchedule.Flights.Count != 25)
            //{
            //    CurrentSchedule = GenerateSchedule();
            //}
        }
        public static void AddGate()
        {
            Gate thisgate = new Gate();
            thisgate.GateID = Gates.Count + 1;
            thisgate.Number = Gates.Count + 1;
            myDatabase.AddGate(thisgate);
            Gates.Add(thisgate);
        }

        public static void AddRunway()
        {
            Runway thisRunway = new Runway();
            thisRunway.RunwayID = Runways.Count + 1;
            thisRunway.Number = Runways.Count + 1;
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
            //Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            Flights = FlightController.flights.OrderBy(flight => flight.Departure).ToList();

            for (int i = 0; i < Flights.Count; i++)
            {
                theseflights.Add(Flights[i]);
            }
            for (int i = 0; i < Flights.Count; i++)
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
        public static Schedule GenerateSchedule(DateTime chosenDate)
        {
            Schedule myshedule = new Schedule();
            myshedule.Date = chosenDate;
            myshedule.ScheduleID = generator.Next(0, 10000);
            List<Flight> theseflights = new List<Flight>();
            //Flights = (List<Flight>)Flights.OrderBy(flight => flight.Departure);
            Flights = FlightController.flights.OrderBy(flight => flight.Departure).ToList();


            for (int i = 0; i < Flights.Count; i++)
            {
                foreach(var flight in Flights)
                {
                    if(flight.Departure > chosenDate)
                    {
                        theseflights.Add(Flights[i]);
                    }
                }
                
            }
            for (int i = 0; i < Flights.Count; i++)
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

            myshedule.Flights = theseflights;

            return myshedule;
        }

        public static void FlightDeparts()
        {

        }
        public static void UpdateShedule()
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
        public static Schedule ConfirmSchedule(Schedule thisschedule)
        {
            return thisschedule;
        }
    }

}