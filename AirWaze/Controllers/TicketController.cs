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
            UserID = Guid.Parse("549C7D76-AEBB-4956-BC79-BFF28F67D9CA"),
            Name = "tverhel",
            Password = "password",
            LastName = "Verhellen",
            FirstName = "Tijs",
            Email = "tijsverhellen2@gmail.com",
            StreetName = "Savaanstraat",
            HouseNumber = "33",
            Zipcode = "9000",
            City = "Ghent",
            Country = "Belgium",
            PhoneNumber = "+32477777777",
            IsVerified = true
        };
        static Airline testAirline = new Airline
        {
            Number = "55",
            PhoneNumber = "777888999",
            CurrentPlanes = new List<Plane>(),
            AccountNumber = "111222333",
            Adress = "Koekoekstraat",
            City = "Melle",
            AirlineID = Guid.NewGuid(),
            CompanyNumber = "5555555",
            Email = "ikke@virgin.com",
            Name = "Harald Airways",
            NameTag = "HAR",
        };
        static Plane testplane = new Plane
        {
            PlaneNr = "6666",
            CurrentAirline = testAirline,
            PassengerCapacity = 200,
            FuelUsagePerKM = 500,
            FirstClassCapacity = 100,
            FlightRegion = "EUR",
            FuelCapacity = 5000,
            IsAvailable = true,
            LoadCapacity = 10000,
            Manufacturer = "Boeing",
            Type = "747",
            SeatDiagram = new string[5, 40],
        };
        public static readonly List<Flight> allFlights = new List<Flight>
        {
            new Flight()
            {
                FlightNr = "2",
                CurrentPlane = testplane,
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

        public static List<TicketCreateViewModel> ticketsToHandle = new List<TicketCreateViewModel>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            List<TicketListViewModel> list = new List<TicketListViewModel>();
            foreach (var ticket in loadedTickets)
            {
                if (ticket.CurrentUser == myUser)
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
                flightList = new List<Flight>(),
                TicketNr = ticketNr
            };

            foreach(Flight flight in allFlights) // TO BE REPLACED BY EITHER REFINED QUERY FUNCTION GetFlightsByDate(DateTime)
            {
                if(flight.Destination == toHandle.Destination)
                {
                    if((toHandle.Departure - flight.Departure).Duration() <= TimeSpan.FromDays(3))
                    {
                        // I should use FlightDetailViewModel here but I dont want to
                        flightPicker.flightList.Add(flight);
                    }
                }
            }
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
            return View();
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
        public IActionResult Edit(string ID, TicketEditViewModel editedTicket)
        {
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

        public IActionResult ConfirmedDelete(string ID)
        {
            database.RemoveTicket(loadedTickets.Single(x => x.TicketNr == ID));
            return RedirectToAction("List");
        }

        public IActionResult Payment(string ID, string ticket)
        {
            TicketCreateViewModel toHandle = ticketsToHandle.Single(x => x.TicketNr == ticket);
            //Flight chosenFlight = database.GetFlightByNr(ID);
            Flight chosenFlight = allFlights.Single(x => x.FlightNr == ID);
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
            database.AddTicket(newTicket);
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

        public void LoadTicketList(User user)
        {
            loadedTickets = database.GetTicketsByUser(user);
        }
    }
}
