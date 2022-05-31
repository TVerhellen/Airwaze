using AirWaze.Areas.Identity.Data;
using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirWaze.Controllers
{
    [Authorize(Roles = "Admin, Customer")]
    public class TicketController : Controller
    {
        private readonly UserManager<AirWazeUser> _userManager;
        private readonly IAirWazeDatabase database;
        private static List<Ticket> loadedTickets = new List<Ticket>();
        public static List<Destination> allDestinations = new List<Destination>();
        public static List<Flight> allFlights = new List<Flight>();
        public static List<TicketCreateViewModel> ticketsToHandle = new List<TicketCreateViewModel>();
        public static List<Ticket> TicketsFromSeatpicker = new List<Ticket>();
        public static List<Ticket> TicketsForSeatpicker = new List<Ticket>();
        static ApplicationUser myUser;
        private Random random = new Random();

        public TicketController(IAirWazeDatabase db, UserManager<AirWazeUser> userManager)
        {
            _userManager = userManager;
            database = db;
            if(allDestinations.Count == 0)
            {
                allDestinations = database.GetDestinations();
            }
            
            //myUser = HttpContext.User;
            //myUser = database.GetUserByID(HttpContext.Session.GetString("user_id"));
            
        }

        public async void GetUser()
        {
            string myUserId = _userManager.GetUserId(User);
            myUser = database.GetUserByID(myUserId);
            if (loadedTickets.Count == 0)
            {
                LoadTicketList(myUser);
            }
        }

       

        


        public IActionResult Index()
        {
            GetUser();
            return View();
        }

        public IActionResult List(string option, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            GetUser();
            List<TicketListViewModel> list = new List<TicketListViewModel>();
            foreach (var ticket in loadedTickets)
            {
                if (ticket.CurrentUser.Id == myUser.Id)
                {
                    list.Add(new TicketListViewModel()
                    {
                        TicketNr = ticket.TicketNr,
                        CurrentFlight = ticket.CurrentFlight,
                        LastName = ticket.LastName,
                        FirstName = ticket.FirstName,
                        Status = ticket.Status
                    });
                }
            }
            //searchfunction
            var myTicket = from s in list
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (option == "Destination")
                {
                    myTicket = myTicket.Where(s => s.CurrentFlight.Destination.Name.Contains(searchString));
                }
                else if (option == "Date")
                {
                    myTicket = myTicket.Where(s => s.CurrentFlight.Departure.ToString("dd/MM/yyyy").Contains(searchString) || s.CurrentFlight.Departure.ToString("dd-MM-yyyy").Contains(searchString)).ToList();
                }
                else if (option == "Name")
                {
                    myTicket = myTicket.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
                }
            }
            return View(myTicket.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TicketCreateViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(TicketCreateViewModel newTicket)
        {
            await Task.Delay(1500);
            if (TryValidateModel(newTicket))
            {
                newTicket.TicketNr = $"NOTASSIGNED{ticketsToHandle.Count}";
                ticketsToHandle.Add(newTicket);
                LoadTicketList(myUser);
                return Redirect($"FlightPicker/{newTicket.TicketNr}");
            }
            return View(newTicket);
        }

        [HttpGet]
        [Route("Ticket/FlightPicker/{ticketNr}")]
        public IActionResult FlightPicker(string ticketNr)
        {
            TicketCreateViewModel toHandle = ticketsToHandle.Single(x => x.TicketNr == ticketNr);
            TicketFlightPickerViewModel flightPicker = new TicketFlightPickerViewModel()
            {
                flightList = database.GetFlightsByParams(toHandle.Departure, 3, toHandle.Destination),
                TicketNr = ticketNr
            };

            if(flightPicker.flightList.Count > 0)
            {
                return View(flightPicker);
            }
            else
            {
                ticketsToHandle.Remove(toHandle);
                return RedirectToAction("Create", toHandle);
            }           
        }

        public IActionResult Detail(string ID)
        {
            Ticket loadedTicket = loadedTickets.Single(x => x.TicketNr == ID);
            TicketDetailViewModel detailTicket = new TicketDetailViewModel()
            {
                TicketNr = loadedTicket.TicketNr,
                CurrentFlight = loadedTicket.CurrentFlight,
                LastName = loadedTicket.LastName,
                FirstName = loadedTicket.FirstName,
                Price = loadedTicket.Price,
                FirstClass = loadedTicket.FirstClass,
                Seat = loadedTicket.Seat,
                ExtraLuggage = loadedTicket.ExtraLuggage
            };
            return View(detailTicket);
        }

        [HttpGet]
        public IActionResult Edit(string ID)
        {
            Ticket loadedTicket = loadedTickets.Single(x => x.TicketNr == ID);
            TicketEditViewModel editTicket = new TicketEditViewModel()
            {
                TicketNr = loadedTicket.TicketNr,
                CurrentFlight = loadedTicket.CurrentFlight,
                LastName = loadedTicket.LastName,
                FirstName = loadedTicket.FirstName,
                Price = loadedTicket.Price,
                FirstClass = loadedTicket.FirstClass,
                Seat = loadedTicket.Seat,
                ExtraLuggage = loadedTicket.ExtraLuggage,
            };
            return View(editTicket);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(string ID, TicketEditViewModel editedTicket)
        {
            await Task.Delay(1500);
            Ticket loadedTicket = loadedTickets.Single(x => x.TicketNr == ID);
            loadedTicket.LastName = editedTicket.LastName;
            loadedTicket.FirstName = editedTicket.FirstName;
            database.UpdateTicket(loadedTicket);
            return RedirectToAction("List");
        }

        public IActionResult Delete(string ID)
        {
            Ticket loadedTicket = loadedTickets.Single(x => x.TicketNr == ID);
            TicketDeleteViewModel deleteTicket = new TicketDeleteViewModel()
            {
                TicketNr = loadedTicket.TicketNr,
                CurrentFlight = loadedTicket.CurrentFlight,
                LastName = loadedTicket.LastName,
                FirstName = loadedTicket.FirstName
            };
            LoadTicketList(myUser);
            return View(deleteTicket);
        }

        public async Task<IActionResult> ConfirmedDelete(string ID)
        {
            await Task.Delay(1500);
            database.RemoveTicket(loadedTickets.Single(x => x.TicketNr == ID));
            LoadTicketList(myUser);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Payment(string ID, string ticket)
        {
            await Task.Delay(1500);
            TicketCreateViewModel toHandle = ticketsToHandle.Single(x => x.TicketNr == ticket);
            Flight chosenFlight = database.GetFlightByNr(ID);
            string ticketnr = GenerateTicketNumber(chosenFlight);
            string seat = GenerateSeatNumber(chosenFlight);
            toHandle.Price = toHandle.FirstClass ? (toHandle.ExtraLuggage ? chosenFlight.Destination.Distance * (decimal)0.07 + 75 : chosenFlight.Destination.Distance * (decimal)0.07) : (toHandle.ExtraLuggage ? chosenFlight.Destination.Distance * (decimal)0.05 + 75 : chosenFlight.Destination.Distance * (decimal)0.05);
            GetUser();
            loadedTickets.Add(new Ticket()
            {
                TicketNr = ticketnr,
                CurrentFlight = chosenFlight,
                CurrentUser = myUser,
                LastName = toHandle.LastName,
                FirstName = toHandle.FirstName,
                Price = toHandle.Price,
                FirstClass = toHandle.FirstClass,
                Seat = seat,
                ExtraLuggage = toHandle.ExtraLuggage,
                Status = 0
            });
            TicketPaymentViewModel payTicket = new TicketPaymentViewModel()
            {
                TicketNr = ticketnr,
                CurrentFlight = chosenFlight,
                Price = toHandle.Price,
                FirstClass = toHandle.FirstClass,
                ExtraLuggage = toHandle.ExtraLuggage
            };
            return View(payTicket);
        }

        public IActionResult ConfirmedPayment(string ID)
        {
            Ticket newTicket = loadedTickets.Single(x => x.TicketNr == ID);
            newTicket.Status = 1;
            if(database.AddTicket(newTicket) <= 0)
            {
                LoadTicketList(myUser);
            }
            
            return RedirectToAction("List");
        }

        [Route("SeatPicker")]
        public IActionResult SeatPicker(string ID)
        {
            Ticket newTicket = loadedTickets.Single(x => x.TicketNr == ID);
            TicketsForSeatpicker.Add(newTicket);
            //HttpContext.Session.SetString("TicketNr", newTicket.TicketNr);
            return View();
        }

        public IActionResult SeatPickerConfirmed()
        {
            Ticket ticket = TicketsFromSeatpicker.LastOrDefault(x => x.CurrentUser == myUser);
            TicketsFromSeatpicker.Remove(ticket);
            TicketsForSeatpicker.Remove(TicketsForSeatpicker.LastOrDefault(x => x.TicketNr == ticket.TicketNr));
            loadedTickets[loadedTickets.IndexOf(loadedTickets.LastOrDefault(x => x.TicketNr == ticket.TicketNr))] = ticket;
            database.UpdateTicket(ticket);
            return RedirectToAction("List");
        }

        public IActionResult FailedPayment(TicketCreateViewModel newTicket)
        {

            return RedirectToAction("Create", newTicket);
        }

        public string GenerateTicketNumber(Flight flight)
        {
            string ticketNumber;
            do
            {
                ticketNumber = "";
                ticketNumber += flight.Destination.Name.Substring(0, 4) + "-";
                ticketNumber += flight.Departure.Day + flight.Departure.Month + "-";
                ticketNumber += myUser.Id.Substring(0, 4) + "-";
                ticketNumber += random.Next(0, 1000).ToString();
            }
            while (loadedTickets.FirstOrDefault(x => x.TicketNr.Equals(ticketNumber)) != null);
            return ticketNumber;
        }

        public string GenerateSeatNumber(Flight flight)
        {
            return flight.FillSeatDiagram(1);
        }

        public void LoadTicketList(ApplicationUser user)
        {
            loadedTickets = database.GetTicketsByUser(user);
        }
    }
}
