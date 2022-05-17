using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AdminController : Controller
    {
        public static Schedule? scheduleToApprove;
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
            return View();
        }

        [HttpGet]
        public IActionResult GenerateSchedule()
        {
            return View(new ScheduleGenerateViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult GenerateSchedule(ScheduleGenerateViewModel viewModel)
        {
            viewModel.IsValidated = false;
            viewModel.Flights = new List<Flight>();

            if(TryValidateModel(viewModel))
            {
                scheduleToApprove = Airport.GenerateSchedule(viewModel.Date);
                return RedirectToAction("Schedule");
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult ApproveSchedule()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ConfirmApproveSchedule()
        {
            scheduleToApprove.IsValidated = true;
            Airport.CurrentSchedule = scheduleToApprove;
            scheduleToApprove = null;

            return RedirectToAction("Schedule");
        }

        [HttpGet]
        public IActionResult DeleteSchedule()
        {
            scheduleToApprove = null;

            return RedirectToAction("Schedule");
        }

        [HttpGet]
        public IActionResult ListSchedule()
        {
            return View();
        }
    }
}
