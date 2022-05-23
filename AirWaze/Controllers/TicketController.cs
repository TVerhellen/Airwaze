using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class TicketController : Controller
    {
        private readonly IAirWazeDatabase database;
        private static List<Ticket> loadedTickets = new List<Ticket>();

        public TicketController(IAirWazeDatabase db)
        {
            database = db;
            if(loadedTickets.Count == 0)
            {
                LoadTicketList(myUser);
            }
            
        }


        static User myUser = new User()
        {
            UserID = Guid.Parse("7B1A9A6C-658A-4391-8149-1184FAC528BE"),
            //Name = "tverhel",
            //Password = "password",
            LastName = "Verhellen",
            FirstName = "Tijs",
            Email = "tijs@milehighclub.com",
            StreetName = "Koperstraat",
            HouseNumber = "894",
            Bus = "4",
            Zipcode = "1000",
            City = "Brussel",
            Country = "Belgium",
            PhoneNumber = "0456789456",
            //IsVerified = true
        };

        public static List<Flight> allFlights = new List<Flight>();

        public static List<TicketCreateViewModel> ticketsToHandle = new List<TicketCreateViewModel>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(string option, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            List<TicketListViewModel> list = new List<TicketListViewModel>();
            foreach (var ticket in loadedTickets)
            {
                if (ticket.CurrentUser.UserID == myUser.UserID)
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
                    myTicket = myTicket.Where(s => s.CurrentFlight.Destination.Contains(searchString));
                }
                else if (option == "Date")
                {
                    myTicket = myTicket.Where(s => s.CurrentFlight.Departure.ToString("dd/MM/yyyy").Contains(searchString)).ToList();
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
                flightList = database.GetFlightsByDate(toHandle.Departure, 3),
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

        public IActionResult Payment(string ID, string ticket)
        {
            TicketCreateViewModel toHandle = ticketsToHandle.Single(x => x.TicketNr == ticket);
            Flight chosenFlight = database.GetFlightByNr(ID);
            string ticketnr = GenerateTicketNumber(chosenFlight);
            string seat = GenerateSeatNumber(chosenFlight);
            toHandle.Price = toHandle.FirstClass ? (toHandle.ExtraLuggage ? chosenFlight.Distance * (decimal)1.2 + 75 : chosenFlight.Distance * (decimal)1.2) : (toHandle.ExtraLuggage ? chosenFlight.Distance + 75 : chosenFlight.Distance);

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
            if(database.AddTicket(newTicket) > 0)
            { }
            else
            {
                LoadTicketList(myUser);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult FailedPayment(TicketCreateViewModel newTicket)
        {

            return RedirectToAction("Create", newTicket);
        }

        public string GenerateTicketNumber(Flight flight)
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateSeatNumber(Flight flight)
        {
            return "15B";
        }

        public void LoadTicketList(User user)
        {
            loadedTickets = database.GetTicketsByUser(user);
        }
    }
}
