using AirWaze.Database.Design;
using AirWaze.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AdminController : Controller
    {
        Schedule currentSchedule = new Schedule();
        public static List<Flight> flights = new List<Flight>();
        
        private readonly IAirWazeDatabase _airwazeDatabase;

        public AdminController(IAirWazeDatabase airwazeDatabase)
        {
            _airwazeDatabase = airwazeDatabase;
            flights = _airwazeDatabase.GetFlights();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Schedule()
        {

            return View();
        }
    }
}
