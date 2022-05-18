using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        public static Schedule? scheduleToApprove;
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
            if(Airport.ApprovedSchedules == null)
            {
                Airport.ApprovedSchedules = new List<Schedule>();
            }
            Airport.ApprovedSchedules.Add(scheduleToApprove);
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
        public IActionResult ListSchedules()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DetailSchedule(int index)
        {
            ScheduleGenerateViewModel viewModel = new ScheduleGenerateViewModel();

            if (index == -1)
            {
                viewModel.Date = scheduleToApprove.Date;
                viewModel.Flights = scheduleToApprove.Flights;
                viewModel.IsValidated = scheduleToApprove.IsValidated;
            }
            else
            {
                viewModel.Date = Airport.ApprovedSchedules[index].Date;
                viewModel.Flights = Airport.ApprovedSchedules[index].Flights;
                viewModel.IsValidated = Airport.ApprovedSchedules[index].IsValidated;
            }
            return View(viewModel);
        }
    }
}
