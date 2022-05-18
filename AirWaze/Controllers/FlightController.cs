using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class FlightController : Controller
    {
        //private readonly List<Flight> flights = new List<Flight>();
        public readonly IAirWazeDatabase _airwazeDatabase;
        public static List<Flight> flights = new List<Flight>();
        public static List<Plane> planes = new List<Plane>();
        public static List<FlightCreateViewModel> tempFlights = new List<FlightCreateViewModel>();
        Random _random = new Random();

        

        //testplane (and linked testAirline) is used for new FlightCreateViewModels, before going to FlightPicker
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
            SeatDiagram = new string[5, 40],
        };
        public FlightController(IAirWazeDatabase airwazeDatabase)
        {
            List<Flight> oldlist = flights.ToList();
            _airwazeDatabase = airwazeDatabase;
            flights = _airwazeDatabase.GetFlights();
            foreach (Flight x in flights)
            {
                foreach(Flight y in oldlist)
                {
                    if (x.FlightID == y.FlightID)
                    {
                        if (x.Status != y.Status)
                        {
                            x.Status = y.Status;
                            _airwazeDatabase.UpdateFlight(x);
                        }
                        
                    }
                }
            }
            planes = _airwazeDatabase.GetPlanes();
        }

        //Roles: Everyone
        [HttpGet]
        public IActionResult Index()
        {
            var flightViewModels = new List<FlightListViewModel>();

            //foreach (var flight in flights)
            //{
            //    flightViewModels.Add(new FlightListViewModel()
            //    {
            //        FlightNr = flight.FlightNr,
            //        FlightTime = flight.FlightTime,
            //        Departure = flight.Departure,
            //        Destination = flight.Destination,
            //        CurrentGate = flight.CurrentGate,
            //        Status = flight.Status
            //    });
            //}

            foreach (var flight in flights)
            {
                flightViewModels.Add(new FlightListViewModel()
                {
                    FlightID = flight.FlightID,
                    FlightNr = flight.FlightNr,
                    FlightTime = flight.FlightTime,
                    Departure = flight.Departure,
                    Destination = flight.Destination,
                    CurrentGate = flight.CurrentGate,
                    Status = flight.Status
                });
            }
            return View(flightViewModels);
        }

        //Roles: everyone
        [HttpGet]
        [Route("Flight/Detail/{id}")]
        public IActionResult Detail(string id)
        {
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            //var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            if (flightEntity != null)
            {
                var flightDetails = new FlightDetailViewModel()
                {
                    FlightID = flightEntity.FlightID,
                    FlightNr = flightEntity.FlightNr,
                    CurrentPlane = flightEntity.CurrentPlane,
                    FlightTime = flightEntity.FlightTime,
                    Departure = flightEntity.Departure,
                    Distance = flightEntity.Distance,
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
        public IActionResult Create(FlightCreateViewModel flightViewModel)
        {
            flightViewModel.FlightNr = CreateFlightNr(flightViewModel);
            flightViewModel.CurrentGate = new Gate();
            flightViewModel.CurrentPlane = testplane;
            flightViewModel.CurrentRunway = new Runway();
            flightViewModel.Status = 0;

            if (TryValidateModel(flightViewModel))
            {
                tempFlights.Add(flightViewModel);   
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
            var flightCreateViewModel = tempFlights.SingleOrDefault(x => x.FlightNr == flightNr);
            
            return View(flightCreateViewModel);      
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult PlanePicker(FlightCreateViewModel flightViewModel, string id)
        {
            flightViewModel = tempFlights.SingleOrDefault(x => x.FlightNr == id);
            flightViewModel.CurrentPlane = planes.FirstOrDefault(x => x.PlaneNr == Request.Form["selectedPlaneNr"]);

            if (TryValidateModel(flightViewModel))
            {
                var newEntity = new Flight
                {
                    FlightID = flightViewModel.FlightID,
                    FlightNr = flightViewModel.FlightNr,
                    CurrentPlane = flightViewModel.CurrentPlane,
                    FlightTime = flightViewModel.FlightTime,
                    Departure = flightViewModel.Departure,
                    Distance = flightViewModel.Distance,
                    Destination = flightViewModel.Destination,
                    CurrentGate = flightViewModel.CurrentGate,
                    CurrentRunway = flightViewModel.CurrentRunway,
                    Status = flightViewModel.Status
                };
                _airwazeDatabase.AddFlight(newEntity);
                flights.Add(newEntity);
                tempFlights.Remove(flightViewModel);
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
            //var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);


            //
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            var flightEdit = new FlightEditViewModel()
            {
                FlightID = flightEntity.FlightID,
                FlightNr = flightEntity.FlightNr,
                CurrentPlane = flightEntity.CurrentPlane,
                FlightTime = flightEntity.FlightTime,
                Departure = flightEntity.Departure,
                Distance = flightEntity.Distance,
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
        public IActionResult Edit(string id, FlightEditViewModel flightViewModel)
        {
            flightViewModel.CurrentPlane = planes.FirstOrDefault(x => x.PlaneNr == Request.Form["selectedPlaneNr"]);
            flightViewModel.Status = Convert.ToInt32(Request.Form["selectedStatus"]);
            flightViewModel.FlightNr = id;

            //if (!TryValidateModel(flightViewModel)) return View(flightViewModel);

            //var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            flights.Remove(flightEntity);

            //flightEntity.FlightID = flightViewModel.FlightID;
            flightEntity.FlightNr = flightViewModel.FlightNr;
            flightEntity.CurrentPlane = flightViewModel.CurrentPlane;
            flightEntity.FlightTime = flightViewModel.FlightTime;
            flightEntity.Departure = flightViewModel.Departure;
            flightEntity.Distance = flightViewModel.Distance;
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
            //var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);

            if (flightEntity == null) return new NotFoundResult();

            //TODO: review below check if Database is implemented
            //Flight can only be deleted if flight status is 0 - Generated and there are 0 booked tickets for this flight
            //Otherwise -> Cancel flight via Edit Action (need record of this flight)
            if (flightEntity.Status == 0 && _airwazeDatabase.GetTicketsByFlight(flightEntity.FlightNr).Count == 0)
            {
                var flightViewModel = new FlightDeleteViewModel
                {
                    FlightID = flightEntity.FlightID,
                    FlightNr = flightEntity.FlightNr,
                    Departure = flightEntity.Departure,
                    Destination = flightEntity.Destination
                };

                return View(flightViewModel);
            }
            else
            {
                //TODO: throw message "cannot delete existing flight with booked tickets, please change flight status to "Cancelled"
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        //Roles: Admin + Airport Staff
        [HttpGet]
        public IActionResult DeleteConfirm(string id)
        {
            var flightEntity = flights.FirstOrDefault(x => x.FlightNr == id);
            //var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

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
            string tempFlightNr1 = flightmodel.Destination.Substring(0, 2).ToUpper() + flightmodel.Departure.ToString("yy") + flightmodel.Departure.ToString("MM") + flightmodel.Departure.ToString("dd");
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

            //bool flightNrExists;

            //do
            //{
            //    flightNrExists = false;

            //    foreach(var flight in flights)
            //    {
            //        if(tempFlightNrFull == flight.FlightNr)
            //        {
            //            flightNrExists = true;
            //        }
            //    }

            //    if(flightNrExists == true)
            //    {
            //        if (tempFlightNr2 == 99)
            //        {
            //            tempFlightNr2 = 0;
            //        }
            //        else
            //        {
            //            tempFlightNr2++;
            //        }
            //        tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");
            //    }

            //} while (flightNrExists == true);

            return tempFlightNrFull;
        }
        public static void UpdateDB(Flight myflight)
        {
            
        }
    }
    
}
