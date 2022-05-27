using AirWaze.Database.Design;
using AirWaze.Database;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class FlightController : Controller
    {
        public readonly IAirWazeDatabase _airwazeDatabase;
        public static List<Flight> flights = new List<Flight>();
        //public static List<Flight> tempFlights = new List<Flight>();
        public static List<FlightCreateViewModel> tempFlightModels = new List<FlightCreateViewModel>();
        Random _random = new Random();
        //public static List<Plane> planes = new List<Plane>();
        public static bool flightsChangedWithoutDatabaseUpdate = false;
        public static List<Destination> destinations = new List<Destination>();

        //testplane (and linked testAirline) is used for new FlightCreateViewModels, before going to PlanePicker
        static Airline testAirline = new Airline
        {
            Number = "00000",
            PhoneNumber = "XXXXXXXXXX",
            CurrentPlanes = new List<Plane>(),
            AccountNumber = "XXXXXXXX",
            Adress = "Teststraat",
            City = "Test",
            AirlineID = Guid.NewGuid(),
            CompanyNumber = "1111111",
            Email = "test@email.com",
            Name = "TestAirWays",
            NameTag = "TEST",
        };
        static Plane testplane = new Plane
        {
            PlaneNr = "TestPlane",
            CurrentAirline = testAirline,
            PassengerCapacity = 1,
            FuelUsagePerKM = 1,
            FirstClassCapacity = 1,
            FlightRegion = "EUR",
            FuelCapacity = 1,
            IsAvailable = true,
            LoadCapacity = 1,
            Manufacturer = "Tester",
            Type = "111",
        };
        public FlightController(IAirWazeDatabase airwazeDatabase)
        {
            _airwazeDatabase = airwazeDatabase;


            //TODO: REMOVE COMMENTS AFTER TESTING
            //if(flightsChangedWithoutDatabaseUpdate)
            //{
            //    //Save changes made to flights in Database

            //    List<Flight> databaseFlights = _airwazeDatabase.GetFlights();


            //    foreach (Flight databaseFlight in databaseFlights)
            //    {
            //        Flight flightToChange = flights.SingleOrDefault(x => x.FlightNr == databaseFlight.FlightNr);
            //        _airwazeDatabase.RemoveFlight(flightToChange);
            //        _airwazeDatabase.AddFlight(flightToChange);
            //    }
            //    flightsChangedWithoutDatabaseUpdate = false;
            //}






            //List<Flight> oldlist = flights.ToList();
            //if (flights.Count == 0)
            //{
            //    flights = _airwazeDatabase.GetFlights();
            //}
            //else
            //{
            //    foreach (Flight x in flights)
            //    {
            //        foreach (Flight y in oldlist)
            //        {
            //            if (x.FlightID == y.FlightID)
            //            {
            //                if (x.Status != y.Status)
            //                {
            //                    x.Status = y.Status;
            ////_airwazeDatabase.UpdateFlight(x);
            //            }
            //        }
            //    }

            //}
            ////planes = _airwazeDatabase.GetPlanes();
            //}
        }

        //Roles: Everyone
        [HttpGet]
        public IActionResult Index(string option, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var flightViewModels = new List<FlightListViewModel>();

            foreach (var flight in flights)
            {
                flightViewModels.Add(new FlightListViewModel()
                {
                    FlightID = flight.FlightID,
                    FlightNr = flight.FlightNr,
                    Departure = flight.Departure,
                    Destination = flight.Destination,
                    CurrentGate = flight.CurrentGate,
                    Status = flight.Status
                });
            }
            //searchfunction
            var myFlight = from s in flightViewModels
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (option == "Destination")
                {
                    myFlight = myFlight.Where(s => s.Destination.Name.Contains(searchString));
                }
                else if (option == "Date")
                {
                    myFlight = myFlight.Where(s => s.Departure.ToString("dd/MM/yyyy").Contains(searchString) || s.Departure.ToString("dd-MM-yyyy").Contains(searchString)).ToList();
                }
            }
            return View(myFlight.ToList());
        }


        //Roles: everyone
        [HttpGet]
        [Route("Flight/Detail/{id}")]
        public IActionResult Detail(string id)
        {
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity != null)
            {
                var flightDetails = new FlightDetailViewModel()
                {
                    FlightID = flightEntity.FlightID,
                    FlightNr = flightEntity.FlightNr,
                    CurrentPlane = flightEntity.CurrentPlane,
                    Departure = flightEntity.Departure,
                    Destination = flightEntity.Destination,
                    CurrentGate = flightEntity.CurrentGate,
                    CurrentRunway = flightEntity.CurrentRunway,
                    Status = flightEntity.Status
                };
                return View(flightDetails);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [HttpGet]
        public IActionResult Create()
        {
            var flightViewModel = new FlightCreateViewModel();
            return View(flightViewModel);
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(FlightCreateViewModel flightViewModel)
        {
            await Task.Delay(1500);
            flightViewModel.Destination = destinations.FirstOrDefault(x => x.Name == Request.Form["selectedDestination"]);
            flightViewModel.FlightNr = CreateFlightNr(flightViewModel);
            flightViewModel.CurrentGate = _airwazeDatabase.GetGateByNr(0);
            flightViewModel.CurrentPlane = testplane;
            flightViewModel.CurrentRunway = _airwazeDatabase.GetRunwayByNr(0);
            flightViewModel.Status = 0;

            if (TryValidateModel(flightViewModel))
            {
                tempFlightModels.Add(flightViewModel);   
                return RedirectToAction("PlanePicker", new { flightNr = flightViewModel.FlightNr });
            }
            else
            {
                return View(flightViewModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //[Route("Flight/PlanePicker/{flightNr}")]
        public IActionResult PlanePicker(string flightNr)
        {
            var flightCreateViewModel = tempFlightModels.SingleOrDefault(x => x.FlightNr == flightNr);
            
            return View(flightCreateViewModel);      
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult PlanePicker(FlightCreateViewModel flightViewModel, string id)
        {
            flightViewModel = tempFlightModels.SingleOrDefault(x => x.FlightNr == id);
            flightViewModel.CurrentPlane = PlaneController.planeEntities.FirstOrDefault(x => x.PlaneNr == Request.Form["selectedPlaneNr"]);

            if (TryValidateModel(flightViewModel))
            {
                var newEntity = new Flight
                {
                    FlightID = flightViewModel.FlightID,
                    FlightNr = flightViewModel.FlightNr,
                    CurrentPlane = flightViewModel.CurrentPlane,
                    Departure = flightViewModel.Departure,
                    Destination = flightViewModel.Destination,
                    CurrentGate = flightViewModel.CurrentGate,
                    CurrentRunway = flightViewModel.CurrentRunway,
                    Status = flightViewModel.Status
                };
                _airwazeDatabase.AddFlight(newEntity);
                flights.Add(newEntity);
                tempFlightModels.Remove(flightViewModel);
                return RedirectToAction("Index");
            }
            else
            {
                return View(flightViewModel);
            }
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            var flightEdit = new FlightEditViewModel()
            {
                FlightID = flightEntity.FlightID,
                FlightNr = flightEntity.FlightNr,
                CurrentPlane = flightEntity.CurrentPlane,
                Departure = flightEntity.Departure,
                Destination = flightEntity.Destination,
                CurrentGate = flightEntity.CurrentGate,
                CurrentRunway = flightEntity.CurrentRunway,
                Status = flightEntity.Status
            };

            return View(flightEdit);
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, FlightEditViewModel flightViewModel)
        {
            await Task.Delay(1500);
            flightViewModel.CurrentPlane = PlaneController.planeEntities.FirstOrDefault(x => x.PlaneNr == Request.Form["selectedPlaneNr"]);
            flightViewModel.Status = Convert.ToInt32(Request.Form["selectedStatus"]);
            flightViewModel.Destination = destinations.FirstOrDefault(x => x.Name == Request.Form["selectedDestination"]);
            flightViewModel.FlightNr = id;

            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            flights.Remove(flightEntity);

            //flightEntity.FlightID = flightViewModel.FlightID;
            flightEntity.FlightNr = flightViewModel.FlightNr;
            flightEntity.CurrentPlane = flightViewModel.CurrentPlane;
            flightEntity.Departure = flightViewModel.Departure;
            flightEntity.Destination = flightViewModel.Destination;
            flightEntity.CurrentGate = flightViewModel.CurrentGate;
            flightEntity.CurrentRunway = flightViewModel.CurrentRunway;
            flightEntity.Status = flightViewModel.Status;

            flights.Add(flightEntity);
            _airwazeDatabase.UpdateFlight(flightEntity);
            return RedirectToAction("Index", "Flight");
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            //TODO: review below check if Database is implemented
            //Flight can only be deleted if flight status is 0 - Generated and there are 0 booked tickets for this flight
            //Otherwise -> Cancel flight via Edit Action (need record of this flight)
            //if (flightEntity.Status == 0 && _airwazeDatabase.GetTicketsByFlight(flightEntity.FlightNr).Count == 0)
            //{
                var flightViewModel = new FlightDeleteViewModel
                {
                    FlightID = flightEntity.FlightID,
                    FlightNr = flightEntity.FlightNr,
                    Departure = flightEntity.Departure,
                    Destination = flightEntity.Destination
                };

                return View(flightViewModel);
            //}
            //else
            //{
            //    //TODO: throw message "cannot delete existing flight with booked tickets, please change flight status to "Cancelled"
            //    return RedirectToAction("Index");
            //}
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            await Task.Delay(1500);
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity != null)
            {
                flights.Remove(flightEntity);
                _airwazeDatabase.RemoveFlight(flightEntity);
            }

            return RedirectToAction("Index");
        }

        private string CreateFlightNr(FlightCreateViewModel flightmodel)
        {
            //1st 2 letters of destination + date
            string tempFlightNr1 = flightmodel.Destination.Name.Substring(0, 2).ToUpper() + flightmodel.Departure.ToString("yy") + flightmodel.Departure.ToString("MM") + flightmodel.Departure.ToString("dd");
            //random number of length 2
            int tempFlightNr2 = _random.Next(0, 100);
            //combine both temp nrs
            string tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");

            //keep checking database for flight with same flightnr, if present->increment random(max 99) and recheck
            while (flights.FirstOrDefault(x => x.FlightNr == flightmodel.FlightNr) != null)
            {
                if (tempFlightNr2 == 99)
                {
                    tempFlightNr2 = 0;
                }
                else
                {
                    tempFlightNr2++;
                }
                tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");
            }

            return tempFlightNrFull;
        }
    }
}
