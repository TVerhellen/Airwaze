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

            List<Flight> listflights = new List<Flight>();
            foreach(var flight in scheduleToApprove.Flights)
            {
                listflights.Add(flight);
            }
            Airport.ApprovedSchedules.Add(new Schedule
            {
                ScheduleID = scheduleToApprove.ScheduleID,
                Date = scheduleToApprove.Date,
                Flights = listflights,
                IsValidated = scheduleToApprove.IsValidated
            });

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
        public IActionResult DetailSchedule(int id)
        {
            ScheduleGenerateViewModel viewModel = new ScheduleGenerateViewModel();

            if (id == -1)
            {
                viewModel.ScheduleID = scheduleToApprove.ScheduleID;
                viewModel.Date = scheduleToApprove.Date;
                viewModel.Flights = scheduleToApprove.Flights;
                viewModel.IsValidated = scheduleToApprove.IsValidated;
            }
            else
            {
                Schedule chosenSchedule = Airport.ApprovedSchedules.FirstOrDefault(x => x.ScheduleID == id);
                viewModel.ScheduleID = chosenSchedule.ScheduleID;
                viewModel.Date = chosenSchedule.Date;
                viewModel.Flights = new List<Flight>();
                foreach(var flight in chosenSchedule.Flights)
                {
                    viewModel.Flights.Add(flight);
                }
                viewModel.IsValidated = chosenSchedule.IsValidated;
            }
            return View(viewModel);
        }
    }
}
