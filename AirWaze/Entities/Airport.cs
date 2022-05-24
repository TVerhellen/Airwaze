using AirWaze.Database;
using AirWaze.Controllers;
using AirWaze.Database.Design;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Entities
{
    public static class Airport 
    {

        private static string _name = "Airwaze";
        private static string _adress = "Bosdreef 6 Istanbul Turkye";
        private static DateTime _currenttime = DateTime.Now;
        private static Random generator = new Random();
        public static  IAirWazeDatabase myDatabase;      
        public static bool IsOnline = false;
        public static List<Schedule> _approvedschedules = new List<Schedule>();
        public static List<Destination> Destinations = new List<Destination>();
        
        public static Timer aTimer;

        static Airport()
        {
            myDatabase = CrazyMethod(HomeController._myDatabase);
        }
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

        public static List<Schedule> ApprovedSchedules { get
            { return _approvedschedules; } set
            {
                _approvedschedules = value;
            } }
        
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
            Flights = Flights.FindAll(x => x.Status != 4 && x.Status != 6);
            Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            IsOnline = true;
            StartTimer(1);
        }

        public static void StartTimer(int dueTime)
        {
            aTimer = new Timer(new TimerCallback(TimerProc));
            aTimer.Change(dueTime, 30000);
        }
        private static void TimerProc(object state)
        {
            Timer t = (Timer)state;
            UpdateAirport();
        }
        public static void UpdateAirport()
        {
            foreach (Flight x in Flights)
            {
                TimeSpan myspan = x.Departure - _currenttime;
                if (myspan.TotalMinutes < 120 && x.CurrentPlane.IsAvailable == true)
                {
                    if (x.CurrentPlane != null)
                    {
                        x.CurrentPlane.IsAvailable = false;
                        foreach (Plane y in Planes)
                        {
                            if(y.PlaneID == x.CurrentPlane.PlaneID)
                            {
                                y.IsAvailable = false;
                                PlaneController.planeEntities = Planes.ToList();                               
                            }
                        }                       
                    }                    
                }              
                if (myspan.TotalMinutes < 0)
                {                
                    x.Status = 4;
                    myDatabase = CrazyMethod(HomeController._myDatabase);                                      
                    FlightController.flights = Flights.ToList();

                    if (CurrentSchedule != null)
                    {
                        CurrentSchedule.Flights.Remove(x);
                    }
                }               
            }
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


        //generate schedule standard no longer needed?
        public static Schedule GenerateSchedule()
        {
            Schedule myshedule = new Schedule();
            myshedule.Date = _currenttime;
            myshedule.ScheduleID = generator.Next(0, 10000);
            List<Flight> theseflights = new List<Flight>();
            Flights = Flights.OrderBy(flight => flight.Departure).ToList();

            for (int i = 0; i < Flights.Count; i++)
            {
                theseflights.Add(Flights[i]);
            }

            //put this in correct methods
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
            Flights = Flights.OrderBy(flight => flight.Departure).ToList();

            for (int i = 0; i < Flights.Count; i++)
            {
                if(Flights[i].Departure >= chosenDate && Flights[i].Departure <= chosenDate.AddHours(24) && Flights[i].Status == 0)
                {
                    theseflights.Add(Flights[i]);
                }
            }

            //put this in correct methods
            if(theseflights.Count == 4)
            {
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
        public static IAirWazeDatabase CrazyMethod(IAirWazeDatabase myDatabase)
        {
            List<IAirWazeDatabase> myList = new List<IAirWazeDatabase>();
            myList.Add(myDatabase);
            var goocheltruc = myList.ToList();
            return goocheltruc[0];
        }

        public static void CheckGateAvailability()
        {

            //make list of flights over next 2 hrs with CurrentGate == 0 and status 1&2
            //order by departure
            //available gate -> assign to next flight, priority status 2, then 1 
            //Gate.IsAvailable = false
            //Plane with CurrentPlane.PlaneID -> Not Available
        }
        public static void CheckRunwayAvailability()
        {
            //make list of flights over next 30 mins with CurrentRunway == 0 and status 1&2
            //order by departure
            //available runway -> assign to next flight, priority status 2, then 1
            //Runway.IsAvailable = false
        }
        public static void CheckBoarding()
        {
            //make list of flights with status 1/2  &  departure in 30 mins or less & CurrentGate/CurrentRunway != 0
            //change status to Boarding
        }
        public static void CheckDepartures()
        {
            //make list of flights with status 3 & departure time passed
            //change status to Departed
            //Change CurrentGate & CurrentRunway to 0
            //Change Runway.IsAvailable & Gate.IsAvailable to True
        }
        public static void CheckArrived()
        {
            //List of flights with status 4 departed
            //If DateTime.Now >= Departure + FlightTime -> status = Arrived
        }
        public static void CheckCompleted()
        {
            //List of flights with status 5 arrived
            //If DateTime.Now >= Departure + 2xFlightTime + 1 week -> status = completed
            //Plane with CurrentPlane.PlaneID -> Available
        }

        //Check available planes for created flight in PlanePicker View
        public static List<Plane> AvailablePlanesForFlight(Destination dest, DateTime time)
        {
            //get full planes list and filter down
            //timeslot flight = Departure + 2xFlightTime + 1 week  -> timeslot this flight cannot overlap with timeslot other flights with that plane
            //only show planes that fly region of destination


            return new List<Plane>();
        }
    }
}