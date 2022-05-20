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

        [Route("/test")]
        public IActionResult Blazortest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("/map")]
        public IActionResult Map()
        {
            return View();
        }


    }
}