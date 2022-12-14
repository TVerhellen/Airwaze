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
        //public static Schedule CurrentSchedule { get; set; }
        //public static Schedule ScheduleToApprove { get; set; }
        public static List<Schedule> ApprovedSchedules { get
            { return _approvedschedules; } set
            {
                _approvedschedules = value;
            } }
        
        public static void StartAirport()
        {
            UpdateAirportListsFromDatabase();
            IsOnline = true;
            StartTimer(1);
        }

        private static void UpdateAirportListsFromDatabase()
        {
            myDatabase = CrazyMethod(HomeController._myDatabase);
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

            FlightController.flights = myDatabase.GetFlights().ToList();
            Flights = FlightController.flights.ToList();
            FlightController.destinations = myDatabase.GetDestinations().ToList();
            //Flights = Flights.FindAll(x => x.Status != 6 && x.Status != 7);
            //Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            PlaneController.planeEntities = myDatabase.GetPlanes().ToList();
            Planes = PlaneController.planeEntities;

        }
        public static void StartTimer(int dueTime)
        {
            aTimer = new Timer(new TimerCallback(TimerProc));
            aTimer.Change(dueTime, 300000);
        }
        private static void TimerProc(object state)
        {
            Timer t = (Timer)state;
            UpdateAirport();
        }
        public static void UpdateAirport()
        {
            //Fill in lists with data from Controller lists
            UpdateAirportListsFromControllerLists();

            //Do checks for flight logic and make changes to Lists in both Airport and Controller classes so both remain up to date
            CheckGateAvailability();
            CheckRunwayAvailability();
            CheckPlaneAvailability();
            CheckBoarding();
            CheckDepartures();
            CheckArrived();
            CheckCompleted();
   
            myDatabase = CrazyMethod(HomeController._myDatabase);
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

            myshedule.Flights = theseflights;
            return myshedule;
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
        public static void CheckPlaneAvailability()
        {
            //make list of flights leaving in less than 2 hours with status approved or delayed and CurrentPlaneConfirmed false, all already have a CurrentPlane
            //Sort list by status descending (priority to status 2 - delayed) and then by departure date (priority to flights leaving earlier)


            List<Flight> nextFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if ((flight.Status == 1 || flight.Status == 2)
                    && DateTime.Now >= flight.Departure.AddHours(-2)
                    && flight.CurrentPlaneConfirmed == false)
                {
                    nextFlights.Add(flight);
                }
            }
            nextFlights = nextFlights.OrderByDescending(flight => flight.Status).ThenBy(flight => flight.Departure).ToList();

            //Check for each flight if the CurrentPlane is available,   if yes -> Plane.IsAvailable = false & update
            //                                                          if not -> Check for Available planes (remove planes with prop IsAvailable false from list!) and assign first free one, update Plane & Flight
            //                                                                                       if none available -> Delay Flight (2 hours), update Flight

            foreach (Flight flight in nextFlights)
            {
                if (flight.CurrentPlane.IsAvailable == false)
                {
                    List<Plane> availablePlanes = GetAvailablePlanesForFlight(flight);
                    availablePlanes = availablePlanes.FindAll(plane => plane.IsAvailable == true);

                    if (availablePlanes.Count > 0)
                    {
                        flight.CurrentPlane = availablePlanes[0];
                        flight.CurrentPlaneConfirmed = true;
                        UpdateFlightInAllLists(flight);
                        myDatabase.UpdateFlight(flight);

                        availablePlanes[0].IsAvailable = false;
                        UpdatePlaneInAllLists(availablePlanes[0]);
                        myDatabase.UpdatePlane(availablePlanes[0]);
                    }
                    else
                    {
                        //if none available -> Delay Flight (2 hours)
                        DelayFlight(flight, 120);
                    }
                }
                else
                {
                    //the currentplane for this flight is available -> plane availability on false and update
                    flight.CurrentPlane.IsAvailable = false;
                    UpdatePlaneInAllLists(flight.CurrentPlane);

                    //the currentplane for this flight is available -> flight planeconfirmed on true and update
                    flight.CurrentPlaneConfirmed = true;
                    UpdateFlightInAllLists(flight);
                }
            }
        }
        public static void CheckGateAvailability()
        {
            //make list of flights leaving in less than 2 hours with status approved or delayed and that have no gate yet
            //Sort list by status descending (priority to status 2 - delayed) and then by departure date (priority to flights leaving earlier)

            List <Flight> nextFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if((flight.Status == 1 || flight.Status == 2) 
                    && DateTime.Now >= flight.Departure.AddHours(-2) 
                    && flight.CurrentGate.Number == 0)
                {
                    nextFlights.Add(flight);
                }
            }
            nextFlights = nextFlights.OrderByDescending(flight => flight.Status).ThenBy(flight => flight.Departure).ToList();

            //Make list of available gates
            List<Gate> availableGates = new List<Gate>();
            availableGates = Gates.FindAll(gate => gate.IsAvailable == true);

            //Check for every flight if there is at least 1 gate available -> assign first gate to flight and flight to gate, update availability and remove gate from list availablegates
            for(int i = 0; i<nextFlights.Count; i++)
            {
                if (availableGates.Count > 0)
                {
                    availableGates[0].CurrentFlight = nextFlights[i];
                    availableGates[0].IsAvailable = false;
                    UpdateGateInGatesList(availableGates[0]);

                    nextFlights[i].CurrentGate = availableGates[0];
                    UpdateFlightInAllLists(nextFlights[i]);

                    availableGates.RemoveAt(0);
                }
                else
                {
                    //If no gate available for a flight -> check if it needs to be delayed
                    // Less than 30 mins to departure and still no free gate -> Delay (1 hour)
                    // Else do nothing -> Will be checked again with next UpdateAirport
                    if (DateTime.Now >= nextFlights[i].Departure.AddMinutes(-30))
                    {
                        DelayFlight(nextFlights[i], 60);
                    }
                }
            }
        }
        public static void CheckRunwayAvailability()
        {
            //make list of flights over next 30 mins with CurrentRunway == 0 and status 1&2
            //order by departure
            //available runway -> assign to next flight, priority status 2, then 1
            //Runway.IsAvailable = false

            //make list of flights leaving in less than 30 mins with status approved or delayed and that have no runway yet
            //Sort list by status descending (priority to status 2 - delayed) and then by departure date (priority to flights leaving earlier)
            List<Flight> nextFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if ((flight.Status == 1 || flight.Status == 2)
                    && DateTime.Now >= flight.Departure.AddMinutes(-30)
                    && flight.CurrentRunway.Number == 0)
                {
                    nextFlights.Add(flight);
                }
            }
            nextFlights = nextFlights.OrderByDescending(flight => flight.Status).ThenBy(flight => flight.Departure).ToList();


            //Make list of available gates
            List<Runway> availableRunways = new List<Runway>();
            availableRunways = Runways.FindAll(runway => runway.IsAvailable == true);

            //Check for every flight if there is at least 1 gate available -> assign first gate to flight and flight to gate, update availability and remove gate from list availablegates
            foreach (Flight nextFlight in nextFlights)
            {
                if (availableRunways.Count > 0)
                {
                    availableRunways[0].CurrentFlight = nextFlight;
                    availableRunways[0].IsAvailable = false;
                    UpdateRunwayInRunwaysList(availableRunways[0]);

                    nextFlight.CurrentRunway = availableRunways[0];
                    UpdateFlightInAllLists(nextFlight);

                    availableRunways.RemoveAt(0);
                }
                else
                {
                    //If no runway available for a flight -> check if it needs to be delayed
                    // Less than 10 mins to departure and still no free runway -> Delay (30 mins)
                    // Else do nothing -> Will be checked again with next UpdateAirport
                    if (DateTime.Now >= nextFlight.Departure.AddMinutes(-30))
                    {
                        DelayFlight(nextFlight, 30);
                    }
                }
            }
        }
        public static void CheckBoarding()
        {
            //make list of flights with status 1/2  &  departure in 30 mins or less & CurrentGate/CurrentRunway/CurrentPlane != 0
            //change status to Boarding

            List<Flight> boardingFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if ((flight.Status == 1 || flight.Status == 2)
                    && DateTime.Now >= flight.Departure.AddMinutes(-30)
                    && flight.CurrentRunway.Number != 0
                    && flight.CurrentGate.Number != 0
                    && flight.CurrentPlaneConfirmed == true)
                {
                    boardingFlights.Add(flight);
                }
            }

            foreach (Flight flight in boardingFlights)
            {
                flight.Status = 3;
                UpdateFlightInAllLists(flight);
            }
        }
        public static void CheckDepartures()
        {
            //make list of flights with status 3 & departure time passed
            //change status to Departed
            //Change CurrentGate & CurrentRunway to 0
            //Change Runway.IsAvailable & Gate.IsAvailable to True

            List<Flight> departingFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if (flight.Status == 3
                    && DateTime.Now >= flight.Departure)
                {
                    departingFlights.Add(flight);
                }
            }

            foreach (Flight flight in departingFlights)
            {
                flight.CurrentGate.IsAvailable = true;
                UpdateGateInGatesList(flight.CurrentGate);

                flight.CurrentRunway.IsAvailable = true;
                UpdateRunwayInRunwaysList(flight.CurrentRunway);

                flight.Status = 4;
                flight.CurrentGate = Gates.SingleOrDefault(x => x.Number == 0);
                flight.CurrentRunway = Runways.SingleOrDefault(x => x.Number == 0);
                UpdateFlightInAllLists(flight);
            }
        }
        public static void CheckArrived()
        {
            //List of flights with status 4 departed
            //If DateTime.Now >= Departure + FlightTime -> status = Arrived
            List<Flight> arrivedFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                if (flight.Status == 4
                    && DateTime.Now >= flight.Departure.Add(flight.Destination.FlightTime))
                {
                    arrivedFlights.Add(flight);
                }
            }

            foreach (Flight arrivedflight in arrivedFlights)
            {
                arrivedflight.Status = 5;
                UpdateFlightInAllLists(arrivedflight);
            }
        }
        public static void CheckCompleted()
        {
            //List of flights with status 5 arrived
            //If DateTime.Now >= Departure + 2xFlightTime + 1 week -> status = completed
            //Plane with CurrentPlane.PlaneID -> Available

            List<Flight> completedFlights = new List<Flight>();
            foreach (Flight flight in Flights)
            {
                DateTime flyingTime = flight.Departure.Add(2 * flight.Destination.FlightTime);
                DateTime totalReturnTime = flyingTime.AddDays(7);

                if (flight.Status == 5
                    && DateTime.Now >= totalReturnTime)
                {
                    completedFlights.Add(flight);
                }
            }

            foreach (Flight completedflight in completedFlights)
            {
                completedflight.Status = 7;
                UpdateFlightInAllLists(completedflight);

                completedflight.CurrentPlane.IsAvailable = true;
                UpdatePlaneInAllLists(completedflight.CurrentPlane);
            }
        }
        public static List<Plane> GetAvailablePlanesForFlight(Flight originalFlight)
        {
            //Both IsAvailable=true and IsAvailable=false need to be in this list, as for PlanePicker in FlightCreator, also nonavailable planes need to be able to be selected (flight is later)
            //For check PlaneAvailability in Airport (for departures), only IsAvailable=true is checked in that method

            List<Plane> availablePlanes = new List<Plane>();

            //Make timeslot for this flight to compare plane timeslots with
            DateTime originalFlightPeriodStart = originalFlight.Departure.AddHours(-2);
            DateTime originalFlightPeriodEnd = originalFlight.Departure.Add((2 * originalFlight.Destination.FlightTime) + TimeSpan.FromDays(7));

            foreach(Plane plane in Planes)
            {
                //only check planes that fly to same region as destination
                if (plane.FlightRegion == originalFlight.Destination.Region)
                {
                    //Make list of all flights with currentPlane = this plane and with status 0 - 5, without the flight which it is currently checking availability for
                    List<Flight> flightsPerPlane = new List<Flight>();

                    foreach (Flight flight in Flights)
                    {
                        if(flight.Status != 6 && flight.Status != 7 && flight.CurrentPlane.PlaneNr == plane.PlaneNr)
                        {
                            flightsPerPlane.Add(flight);
                        }
                    }

                    //If there are flights with this plane, check for each flight if timeslots do not overlap with original flight
                    if(flightsPerPlane.Count > 0)
                    {
                        bool planeAvailable = true;

                        foreach(Flight flightPP in flightsPerPlane)
                        {
                            //Make timeslot for the flights with this plane
                            DateTime newFlightPeriodStart = flightPP.Departure.AddHours(-2);
                            DateTime newFlightPeriodEnd = flightPP.Departure.Add((2 * flightPP.Destination.FlightTime) + TimeSpan.FromDays(7));

                            //Compare overlap on both timeslots, if none -> plane is available
                            bool overlap = newFlightPeriodStart < originalFlightPeriodEnd && originalFlightPeriodStart < newFlightPeriodEnd;

                            if (overlap && flightPP.FlightNr != originalFlight.FlightNr)
                            {
                                planeAvailable = false;
                            }
                        }

                        if(planeAvailable)
                        {
                            availablePlanes.Add(plane);
                        }
                    }
                    else
                    {
                        availablePlanes.Add(plane);
                    }
                }
            }
            return availablePlanes;
        }
        private static void DelayFlight(Flight flight, int minutesDelay)
        {
            flight.Departure = flight.Departure.AddMinutes(minutesDelay);
            flight.Status = 2;
            UpdateFlightInAllLists(flight);
        }
        private static void UpdateAirportListsFromControllerLists()
        {
            //Flights = myDatabase.GetFlights();
            Flights = FlightController.flights.ToList();
            //Flights = Flights.FindAll(x => x.Status != 6 && x.Status != 7);
            //Flights = Flights.OrderBy(flight => flight.Departure).ToList();
            Planes = PlaneController.planeEntities.ToList();
        }
        public static void UpdateFlightInAllLists(Flight updatedFlight)
        {
            //look for correct flight in Flights List and update only properties that can be modified in Airport Class automatically
            foreach(Flight flight in Flights)
            {
                if(flight.FlightNr == updatedFlight.FlightNr)
                {
                    flight.CurrentPlane = updatedFlight.CurrentPlane;
                    flight.CurrentPlaneConfirmed = updatedFlight.CurrentPlaneConfirmed;
                    flight.Departure = updatedFlight.Departure;
                    flight.CurrentGate = updatedFlight.CurrentGate;
                    flight.CurrentRunway = updatedFlight.CurrentRunway;
                    flight.Status = updatedFlight.Status;
                    break;
                }
            }

            //Also update flight list in FlightController
            foreach (Flight flight in FlightController.flights)
            {
                if (flight.FlightNr == updatedFlight.FlightNr)
                {
                    flight.CurrentPlane = updatedFlight.CurrentPlane;
                    flight.CurrentPlaneConfirmed = updatedFlight.CurrentPlaneConfirmed;
                    flight.Departure = updatedFlight.Departure;
                    flight.CurrentGate = updatedFlight.CurrentGate;
                    flight.CurrentRunway = updatedFlight.CurrentRunway;
                    flight.Status = updatedFlight.Status;
                    break;
                }
            }
        }
        public static void UpdatePlaneInAllLists(Plane updatedPlane)
        {
            //look for correct plane in Planes List and update only properties that can be modified in Airport Class automatically
            foreach (Plane plane in Planes)
            {
                if (plane.PlaneNr == updatedPlane.PlaneNr)
                {
                    plane.IsAvailable = updatedPlane.IsAvailable;
                    break;
                }
            }

            //Also update planes list in PlaneController
            foreach (Plane plane in PlaneController.planeEntities)
            {
                if (plane.PlaneNr == updatedPlane.PlaneNr)
                {
                    plane.IsAvailable = updatedPlane.IsAvailable;
                    break;
                }
            }
        }
        public static void UpdateRunwayInRunwaysList(Runway updatedRunway)
        {
            //look for correct plane in Planes List and update only properties that can be modified in Airport Class automatically
            foreach (Runway runway in Runways)
            {
                if (runway.Number == updatedRunway.Number)
                {
                    runway.IsAvailable = updatedRunway.IsAvailable;
                    break;
                }
            }
        }
        public static void UpdateGateInGatesList(Gate updatedGate)
        {
            //look for correct plane in Planes List and update only properties that can be modified in Airport Class automatically
            foreach (Gate gate in Gates)
            {
                if (gate.Number == updatedGate.Number)
                {
                    gate.IsAvailable = updatedGate.IsAvailable;
                    break;
                }
            }
        }
    }
}