using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class TicketController : Controller
    {
        static User myUser = new User();
        List<Ticket> allTickets = new List<Ticket>
        {
            new Ticket()
            {
                TicketNr = "1",
                CurrentFlight = new Flight(),
                CurrentUser = new User(),
                LastName = "Verhellen",
                FirstName = "Tijs",
                Price = 50,
                FirstClass = false,
                Seat = "15C",
                ExtraLuggage = false
            }
        };
        public static readonly List<Flight> allFlights = new List<Flight>
        {
            new Flight()
            {
                FlightNr = "2",
                CurrentPlane = new Plane(),
                FlightTime = TimeSpan.FromHours(5),
                Departure = DateTime.Now.AddDays(5),
                //ListTickets = new List<Ticket>(),
                Distance = 1000,
                Destination = "Berlin",
                CurrentGate = new Gate(),
                CurrentRunway = new Runway(),
                Status = 0
            }
        };

        public IActionResult Index()
        {
            List<TicketListViewModel> list = new List<TicketListViewModel>();
            foreach(var ticket in allTickets)
            {
                list.Add(new TicketListViewModel()
                {
                    TicketNr = ticket.TicketNr,
                    CurrentFlight = ticket.CurrentFlight,
                    LastName = ticket.LastName,
                    FirstName = ticket.FirstName
                });
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TicketCreateViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(TicketCreateViewModel newTicket)
        {
            if (TryValidateModel(newTicket))
            {
                allTickets.Add(new Ticket()
                {
                    TicketNr = "ABCD",
                    CurrentFlight = newTicket.CurrentFlight,
                    CurrentUser = myUser,
                    LastName = newTicket.LastName,
                    FirstName = newTicket.FirstName,
                    Price = newTicket.Price,
                    FirstClass = newTicket.FirstClass,
                    Seat = "1A",
                    ExtraLuggage = newTicket.ExtraLuggage,
                    Status = 0
                });
                return RedirectToAction("Index");
            }
            return View(newTicket);
        }

        public IActionResult Detail(string TicketNr)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string TicketNr)
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(string TicketNr, TicketEditViewModel editedTicket)
        {
            return View();
        }

        public IActionResult Delete(string TicketNr)
        {
            return View();
        }

        public IActionResult ConfirmedDelete(string TicketNr)
        {
            return View();
        }

        public IActionResult Payment(TicketCreateViewModel newTicket)
        {
            return View(newTicket);
        }

        public IActionResult ConfirmedPayment(TicketCreateViewModel newTicket)
        {
            return RedirectToAction("Index");
        }

        public IActionResult FailedPayment(TicketCreateViewModel newTicket)
        {
            return RedirectToAction("Create", newTicket);
        }

    }
}
