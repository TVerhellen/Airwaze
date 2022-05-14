using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class FlightController : Controller
    {
        //private readonly List<Flight> flights = new List<Flight>();
        private readonly IAirWazeDatabase _airwazeDatabase;
        Random _random = new Random();

        public FlightController(IAirWazeDatabase airwazeDatabase)
        {
            _airwazeDatabase = airwazeDatabase;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
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

            foreach (var flight in _airwazeDatabase.GetFlights())
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

        [HttpGet]
        public IActionResult Detail(string flightnr)
        {
            //var flightEntity = flights.FirstOrDefault(x => x.FlightNr == flightnr);

            var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

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

        [HttpGet]
        public IActionResult Create()
        {
            var flightCreateViewModel = new FlightCreateViewModel();
            return View(flightCreateViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(FlightCreateViewModel flightViewModel)
        {
            flightViewModel.FlightNr = CreateFlightNr(flightViewModel);
            flightViewModel.CurrentGate = new Gate();
            flightViewModel.CurrentRunway = new Runway();
            flightViewModel.CurrentPlane = new Plane();
            flightViewModel.Status = 0;

            var isModelValid = TryValidateModel(flightViewModel);

            if (isModelValid)
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
                //flights.Add(newEntity);
                return RedirectToAction("Index");
            }
            else
            {
                return View(flightViewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(string flightnr)
        {
            var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            //var flightEntity = flights.FirstOrDefault(x => x.FlightNr == flightnr);

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(string flightnr, FlightEditViewModel flightViewModel)
        {
            if (!TryValidateModel(flightViewModel)) return View(flightViewModel);

            var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            //var flightEntity = flights.FirstOrDefault(x => x.FlightNr == flightnr);

            if (flightEntity == null) return new NotFoundResult();

            //flights.Remove(flightEntity);
            flightEntity.FlightID = flightViewModel.FlightID;
            flightEntity.FlightNr = flightViewModel.FlightNr;
            flightEntity.CurrentPlane = flightViewModel.CurrentPlane;
            flightEntity.FlightTime = flightViewModel.FlightTime;
            flightEntity.Departure = flightViewModel.Departure;
            flightEntity.Distance = flightViewModel.Distance;
            flightEntity.Destination = flightViewModel.Destination;
            flightEntity.CurrentGate = flightViewModel.CurrentGate;
            flightEntity.CurrentRunway = flightViewModel.CurrentRunway;
            flightEntity.Status = flightViewModel.Status;

            //flights.Add(flightEntity);
            _airwazeDatabase.UpdateFlight(flightEntity);
            return RedirectToAction("Detail", new { flightnr = flightnr });
        }

        [HttpGet]
        public IActionResult Delete(string flightnr)
        {
            var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            //var flightEntity = flights.FirstOrDefault(x => x.FlightNr == flightnr);

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

        [HttpGet]
        public IActionResult DeleteConfirm(string flightnr)
        {
            var flightEntity = _airwazeDatabase.GetFlightByNr(flightnr);

            if (flightEntity != null)
            {
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
            while (_airwazeDatabase.GetFlightByNr(tempFlightNrFull) != null)
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
    }
}
