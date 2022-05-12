using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class FlightController : Controller
    {
        List<Flight> flights = new List<Flight>();
        //private readonly IFlightDatabase _flightDatabase;
        Random _random = new Random();

        //public FlightController(IFlightDatabase flightDatabase)
        //{
        //    _flightDatabase = flightDatabase;
        //}

        [HttpGet]
        public IActionResult Index()
        {
            var flightViewModels = new List<FlightListViewModel>();

            foreach (var flight in flights)
            {
                flightViewModels.Add(new FlightListViewModel()
                {
                    FlightID = flight.FlightID,
                    FlightNr = flight.FlightNr,
                    FlightTime = flight.FlightTime,
                    Departure = flight.Departure,
                    Destination = flight.Destination,
                    IsCancelled = flight.IsCancelled,
                    CurrentGate = flight.CurrentGate,
                    IsCompleted = flight.IsCompleted
                });
            }

            //foreach (var flight in _flightDatabase.GetFlights())
            //{
            //    flightViewModels.Add(new FlightListViewModel()
            //    {
            //        FlightID = flight.FlightID,
            //        FlightNr = flight.FlightNr,
            //        FlightTime = flight.FlightTime,
            //        Departure = flight.Departure,
            //        Destination = flight.Destination,
            //        IsCancelled = flight.IsCancelled,
            //        CurrentGate = flight.CurrentGate,
            //        IsCompleted = flight.IsCompleted
            //    });
            //}
            return View(flightViewModels);
        }

        [HttpGet]
        public IActionResult Detail(string flightnr)
        {
            Flight flightEntity = null;

            foreach(var flight in flights)
            {
                if(flight.FlightNr == flightnr)
                {
                    flightEntity = flight;
                }
            }

            //var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            if (flightEntity != null)
            {
                var flightDetails = new FlightDetailViewModel()
                {
                    FlightNr = flightEntity.FlightNr,
                    CurrentPlane = flightEntity.CurrentPlane,
                    FlightTime = flightEntity.FlightTime,
                    Departure = flightEntity.Departure,
                    Distance = flightEntity.Distance,
                    Destination = flightEntity.Destination,
                    IsCancelled = flightEntity.IsCancelled,
                    CurrentGate = flightEntity.CurrentGate,
                    CurrentRunway = flightEntity.CurrentRunway,
                    IsCompleted = flightEntity.IsCompleted
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
            var isModelValid = TryValidateModel(flightViewModel);

            if (isModelValid)
            {
                var newEntity = new Flight
                {
                    FlightNr = CreateFlightNr(flightViewModel),
                    CurrentPlane = flightViewModel.CurrentPlane,
                    FlightTime = flightViewModel.FlightTime,
                    Departure = flightViewModel.Departure,
                    Distance = flightViewModel.Distance,
                    Destination = flightViewModel.Destination,
                    IsCancelled = flightViewModel.IsCancelled,
                    CurrentGate = flightViewModel.CurrentGate,
                    CurrentRunway = flightViewModel.CurrentRunway,
                    IsCompleted = flightViewModel.IsCompleted
                };
                //_flightDatabase.AddFlight(newEntity);
                flights.Add(newEntity);
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
            //var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            Flight flightEntity = null;

            foreach (var flight in flights)
            {
                if (flight.FlightNr == flightnr)
                {
                    flightEntity = flight;
                }
            }


            if (flightEntity == null) return new NotFoundResult();

            var flightEdit = new FlightEditViewModel()
            {
                FlightNr = flightEntity.FlightNr,
                CurrentPlane = flightEntity.CurrentPlane,
                FlightTime = flightEntity.FlightTime,
                Departure = flightEntity.Departure,
                Distance = flightEntity.Distance,
                Destination = flightEntity.Destination,
                IsCancelled = flightEntity.IsCancelled,
                CurrentGate = flightEntity.CurrentGate,
                CurrentRunway = flightEntity.CurrentRunway,
                IsCompleted = flightEntity.IsCompleted
            };

            return View(flightEdit);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(string flightnr, FlightEditViewModel flightViewModel)
        {
            if (!TryValidateModel(flightViewModel)) return View(flightViewModel);

            //var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            Flight flightEntity = null;

            foreach (var flight in flights)
            {
                if (flight.FlightNr == flightnr)
                {
                    flightEntity = flight;
                }
            }

            if (flightEntity == null) return new NotFoundResult();

            flights.Remove(flightEntity);

            flightEntity.FlightNr = flightViewModel.FlightNr;
            flightEntity.CurrentPlane = flightViewModel.CurrentPlane;
            flightEntity.FlightTime = flightViewModel.FlightTime;
            flightEntity.Departure = flightViewModel.Departure;
            flightEntity.Distance = flightViewModel.Distance;
            flightEntity.Destination = flightViewModel.Destination;
            flightEntity.IsCancelled = flightViewModel.IsCancelled;
            flightEntity.CurrentGate = flightViewModel.CurrentGate;
            flightEntity.CurrentRunway = flightViewModel.CurrentRunway;
            flightEntity.IsCompleted = flightViewModel.IsCompleted;

            flights.Add(flightEntity);
            //_flightDatabase.UpdateFlight(flightEntity);
            return RedirectToAction("Detail", new { flightnr = flightnr });
        }

        [HttpGet]
        public IActionResult Delete(string flightnr)
        {
            //var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            Flight flightEntity = null;

            foreach (var flight in flights)
            {
                if (flight.FlightNr == flightnr)
                {
                    flightEntity = flight;
                }
            }

            if (flightEntity == null) return new NotFoundResult();

            var flightViewModel = new FlightDeleteViewModel
            {
                FlightNr = flightEntity.FlightNr,
                Departure = flightEntity.Departure,
                Destination = flightEntity.Destination
            };

            return View(flightViewModel);
        }

        [HttpGet]
        public IActionResult DeleteConfirm(string flightnr)
        {
            Flight flightEntity = null;

            foreach (var flight in flights)
            {
                if (flight.FlightNr == flightnr)
                {
                    flightEntity = flight;
                }
            }

            if(flightEntity != null)
            {
                flights.Remove(flightEntity);
            }

            return RedirectToAction("Index");
        }







        
        private string CreateFlightNr(FlightCreateViewModel flightmodel)
        {
            //1st 2 letters of destination + date
            string tempFlightNr1 = flightmodel.Destination.Substring(0, 2) + flightmodel.Departure.ToString("yy") + flightmodel.Departure.ToString("MM") + flightmodel.Departure.ToString("dd");
            //random number of length 2
            int tempFlightNr2 = _random.Next(0, 100);
            //combine both temp nrs
            string tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");

            //keep checking database for flight with same flightnr, if present -> increment random (max 99) and recheck
            //while (_flightDatabase.GetFlightByNr(tempFlightNrFull) != null)
            //{
            //    if (tempFlightNr2 == 99)
            //    {
            //        tempFlightNr2 = 0;
            //    }
            //    else
            //    {
            //        tempFlightNr2++;
            //    }
            //    tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");
            //}
            bool flightnrexists;

            do
            {
                flightnrexists = false;

                foreach(var flight in flights)
                {
                    if(tempFlightNrFull == flight.FlightNr)
                    {
                        flightnrexists = true;
                    }
                }

                if(flightnrexists == true)
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

            } while (flightnrexists == true);


            //while (_flightDatabase.GetFlightByNr(tempFlightNrFull) != null)
            //{
            //    if (tempFlightNr2 == 99)
            //    {
            //        tempFlightNr2 = 0;
            //    }
            //    else
            //    {
            //        tempFlightNr2++;
            //    }
            //    tempFlightNrFull = tempFlightNr1 + tempFlightNr2.ToString("D2");
            //}

            return tempFlightNrFull;
        }
    }
}
