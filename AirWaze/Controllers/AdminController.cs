using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AdminController : Controller
    {
        public static Schedule? scheduleToApprove = Airport.CurrentSchedule;
        public static Schedule? scheduleApproved = Airport.CurrentSchedule;
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

           ScheduleGenerateViewModel viewModel = new ScheduleGenerateViewModel
            {
                Date = scheduleToApprove.Date,
                Flights = scheduleToApprove.Flights,
                IsValidated = scheduleToApprove.IsValidated
            };

            //

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ApproveSchedule()
        {
            scheduleToApprove.IsValidated = true;
            Airport.CurrentSchedule = scheduleToApprove;
            scheduleToApprove = null;

            return RedirectToAction("Schedule");
        }
    }
}
