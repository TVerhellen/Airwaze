using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class TicketController : Controller
    {
        static User myUser = new User();
        public static readonly List<Ticket> allTickets = new List<Ticket>
        {
            new Ticket()
            {
                TicketNr = "1",
                CurrentFlight = new Flight(),
                CurrentUser = myUser,
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
                FlightID = 1,
                FlightNr = "2",
                CurrentPlane = new Plane(),
                FlightTime = TimeSpan.FromHours(5),
                Departure = DateTime.Now.AddDays(5),
                ListTickets = new List<Ticket>(),
                Distance = 1000,
                Destination = "Berlin",
                IsCancelled = false,
                CurrentGate = new Gate(),
                CurrentRunway = new Runway(),
                IsCompleted = false
            }
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            List<TicketListViewModel> list = new List<TicketListViewModel>();
            foreach (var ticket in allTickets)
            {
                if (ticket.CurrentUser == myUser)
                {
                    list.Add(new TicketListViewModel()
                    {
                        TicketNr = ticket.TicketNr,
                        CurrentFlight = ticket.CurrentFlight,
                        LastName = ticket.LastName,
                        FirstName = ticket.FirstName
                    });
                }

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
                Flight flight = allFlights.Single(x => x.FlightNr == newTicket.CurrentFlightNr);
                newTicket.Price = newTicket.FirstClass ? (newTicket.ExtraLuggage ? flight.Distance * (decimal)1.2 + 75 : flight.Distance * (decimal)1.2) : (newTicket.ExtraLuggage ? flight.Distance + 75 : flight.Distance);
                string ticketNr = GenerateTicketNumber(flight);
                string seat = GenerateSeatNumber(flight);
                allTickets.Add(new Ticket()
                {
                    TicketNr = ticketNr,
                    CurrentFlight = flight,
                    CurrentUser = myUser,
                    LastName = newTicket.LastName,
                    FirstName = newTicket.FirstName,
                    Price = newTicket.Price,
                    FirstClass = newTicket.FirstClass,
                    Seat = seat,
                    ExtraLuggage = newTicket.ExtraLuggage,
                    Status = 0
                });
                TicketEditViewModel editTicket = new TicketEditViewModel()
                {
                    TicketNr = ticketNr,
                    CurrentFlight = flight,
                    LastName = newTicket.LastName,
                    FirstName = newTicket.FirstName,
                    Price = newTicket.Price,
                    FirstClass = newTicket.FirstClass,
                    Seat = seat,
                    ExtraLuggage = newTicket.ExtraLuggage
                };
                if (TryValidateModel(editTicket))
                {
                    return RedirectToAction("Payment", editTicket);
                }

            }
            return View(newTicket);
        }

        public IActionResult Detail(string ID)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string ID)
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(string ID, TicketEditViewModel editedTicket)
        {
            return View();
        }

        public IActionResult Delete(string ID)
        {
            return View();
        }

        public IActionResult ConfirmedDelete(string ID)
        {
            return View();
        }

        public IActionResult Payment(TicketEditViewModel editTicket)
        {
            return View(editTicket);
        }

        public IActionResult ConfirmedPayment(string ID)
        {
            allTickets.Single(x => x.TicketNr == ID).Status = 1;
            return RedirectToAction("Index");
        }

        public IActionResult FailedPayment(TicketCreateViewModel newTicket)
        {
            return RedirectToAction("Create", newTicket);
        }

        public string GenerateTicketNumber(Flight flight)
        {
            return "ABCD";
        }

        public string GenerateSeatNumber(Flight flight)
        {
            return "15B";
        }
    }
}
