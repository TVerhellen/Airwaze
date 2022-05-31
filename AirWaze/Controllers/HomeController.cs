using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirWaze.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static IAirWazeDatabase _myDatabase;
      
        public HomeController(ILogger<HomeController> logger, IAirWazeDatabase mydatabase)
        {
            _logger = logger;
            if (_myDatabase == null)
            {
                _myDatabase = mydatabase;
            }
            if (!Airport.IsOnline)
            {
                Airport.StartAirport();
            }
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/AirportMap")]
        public IActionResult Blazortest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("/Seatpicker")]
        public IActionResult Map()
        {
            return View();
        }
        public IActionResult Departures()
        {
            List<FlightListViewModel> departures = new List<FlightListViewModel>();

            foreach (Flight flight in FlightController.flights)
            {
                if(flight.Departure >= DateTime.Now.AddMinutes(-15) && flight.Departure <= DateTime.Now.AddHours(24))
                {
                    if (flight.Status != 0 && flight.Status != 5)
                    {
                        if(flight.Status == 4)
                        {
                            if(flight.Departure >= DateTime.Now.AddMinutes(-15) && flight.Departure <= DateTime.Now.AddMinutes(15))
                            {
                                departures.Add(CreateNewFlightListViewModel(flight));
                            }
                        }
                        else
                        {
                            departures.Add(CreateNewFlightListViewModel(flight));
                        }
                    }
                }
            }

            departures = departures.OrderBy(flight => flight.Departure).ToList();

            return View(departures);
        }

        private FlightListViewModel CreateNewFlightListViewModel(Flight flight)
        {
            return new FlightListViewModel
            {
                FlightID = flight.FlightID,
                FlightNr = flight.FlightNr,
                Departure = flight.Departure,
                Destination = flight.Destination,
                CurrentGate = flight.CurrentGate,
                Status = flight.Status
            };
        }
    }
}