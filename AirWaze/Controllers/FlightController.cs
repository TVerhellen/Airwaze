using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightDatabase _flightDatabase;
        Random _random = new Random();

        public FlightController(IFlightDatabase flightDatabase)
        {
            _flightDatabase = flightDatabase;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var flightViewModels = new List<FlightListViewModel>();

            foreach (var flight in _flightDatabase.GetFlights())
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
            return View(flightViewModels);
        }

        [HttpGet]
        public IActionResult Detail(string flightnr)
        {
            var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            if (flightEntity != null)
            {
                var flightDetails = new FlightDetailViewModel()
                {
                    FlightNr = flightEntity.FlightNr,
                    CurrentPlane = flightEntity.CurrentPlane,
                    FlightTime = flightEntity.FlightTime,
                    Departure = flightEntity.Departure,
                    ListTickets = flightEntity.ListTickets,
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
                    ListTickets = flightViewModel.ListTickets,
                    Distance = flightViewModel.Distance,
                    Destination = flightViewModel.Destination,
                    IsCancelled = flightViewModel.IsCancelled,
                    CurrentGate = flightViewModel.CurrentGate,
                    CurrentRunway = flightViewModel.CurrentRunway,
                    IsCompleted = flightViewModel.IsCompleted
                };
                _flightDatabase.AddFlight(newEntity);
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
            var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            if (flightEntity == null) return new NotFoundResult();

            var flightEdit = new FlightEditViewModel()
            {
                FlightNr = flightEntity.FlightNr,
                CurrentPlane = flightEntity.CurrentPlane,
                FlightTime = flightEntity.FlightTime,
                Departure = flightEntity.Departure,
                ListTickets = flightEntity.ListTickets,
                Distance = flightEntity.Distance,
                Destination = flightEntity.Destination,
                IsCancelled = flightEntity.IsCancelled,
                CurrentGate = flightEntity.CurrentGate,
                CurrentRunway = flightEntity.CurrentRunway,
                IsCompleted = flightEntity.IsCompleted
            };

            return View(flightEdit);
        }

        [HttpPost]
        public IActionResult Edit(string flightnr, FlightEditViewModel flightViewModel)
        {
            if (!TryValidateModel(flightViewModel)) return View(flightViewModel);

            var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            if (flightEntity == null) return new NotFoundResult();

            flightEntity.FlightNr = flightViewModel.FlightNr;
            flightEntity.CurrentPlane = flightViewModel.CurrentPlane;
            flightEntity.FlightTime = flightViewModel.FlightTime;
            flightEntity.Departure = flightViewModel.Departure;
            flightEntity.ListTickets = flightViewModel.ListTickets;
            flightEntity.Distance = flightViewModel.Distance;
            flightEntity.Destination = flightViewModel.Destination;
            flightEntity.IsCancelled = flightViewModel.IsCancelled;
            flightEntity.CurrentGate = flightViewModel.CurrentGate;
            flightEntity.CurrentRunway = flightViewModel.CurrentRunway;
            flightEntity.IsCompleted = flightViewModel.IsCompleted;

            _flightDatabase.UpdateFlight(flightEntity);
            return RedirectToAction("Detail", new { flightnr = flightnr });
        }

        [HttpGet]
        public IActionResult Delete(string flightnr)
        {
            var flightEntity = _flightDatabase.GetFlightByNr(flightnr);

            if (flightEntity == null) return new NotFoundResult();

            var flightViewModel = new FlightDeleteViewModel
            {
                FlightNr = flightEntity.FlightNr,
                Departure = flightEntity.Departure,
                Destination = flightEntity.Destination
            };

            return View(flightViewModel);
        }

        public IActionResult DeleteConfirm()
        {
            return RedirectToAction("Index");
        }







        //TODO: fill in method logic
        private string CreateFlightNr(FlightCreateViewModel flightmodel)
        {
            //Unique number(check database): 1-3 letters + 1-4 numbers
            //e.g.BA4432
            //1st 2 letters of destination + random nr(if already exists +1) ??

            return "";
        }
    }
}
