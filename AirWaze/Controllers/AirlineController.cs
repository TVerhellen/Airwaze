using AirWaze.Areas.Identity.Data;
using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AirlineController : Controller
    {

        private IAirWazeDatabase _myDatabase;
        
        public static Airline LoggedInAirline;

        private static List<Airline> airlineEntities = new List<Airline>();      
        //Gets All Entities of Airlines Later on - Will do For all uses!
        public AirlineController(IAirWazeDatabase mydatabase)
        {
           
            if (_myDatabase == null)
            {
                _myDatabase = mydatabase;               
            }
            airlineEntities = _myDatabase.GetAirlines();
        }

        [Authorize(Roles = "Airline")]
        //Airline Role
        public IActionResult Index()
        {
            AirlineIndexViewModel mymodel = new AirlineIndexViewModel();          
            if (LoggedInAirline == null)
            {
              
                LoggedInAirline = new Airline
                {
                    Email = User.Identity.Name,
                };
                LoggedInAirline = airlineEntities.FirstOrDefault(x => x.Email == LoggedInAirline.Email);
                
            }
            mymodel.Airline = LoggedInAirline;
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        //Only ADMINS
        public async Task<IActionResult> List()
        {
            List<AirlineListViewModel> thislist = new List<AirlineListViewModel>();
            foreach (var airline in airlineEntities)
            {
                thislist.Add(new AirlineListViewModel()
                {
                    AirlineID = airline.AirlineID,
                    NameTag = airline.NameTag,
                    Name = airline.Name,                    
                    Logo = airline.Logo,
                });              
            }
            return View(thislist);
        }

        [Authorize(Roles = "Admin")]
        //Guest Role + Admin
        [HttpGet]
        public IActionResult Create()
        {
            var airlineCreateViewModel = new AirlineCreateViewModel();
            
            return View(airlineCreateViewModel);
        }

        [Authorize(Roles = "Admin")]
        //Guest Role + Admin
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(AirlineCreateViewModel airlineViewModel)
        {
            await Task.Delay(1500);
            if (airlineViewModel.AirlineID == null)
            {
                airlineViewModel.AirlineID = Guid.NewGuid();
            }
            if (airlineViewModel.CurrentPlanes == null)
            {
                airlineViewModel.CurrentPlanes = new List<Plane>();
            }        
            var isValid = TryValidateModel(airlineViewModel);

            if (isValid)
            {
                var newEntity = new Airline
                {
                    
                    Name = airlineViewModel.Name,
                    NameTag = airlineViewModel.NameTag,
                    CompanyNumber = airlineViewModel.CompanyNumber,
                    CurrentPlanes  = airlineViewModel.CurrentPlanes,
                    AirlineID = (Guid)airlineViewModel.AirlineID,
                    Adress = airlineViewModel.Adress,
                    Number = airlineViewModel.Number,
                    City = airlineViewModel.City,
                    Email = airlineViewModel.Email,
                    PhoneNumber = airlineViewModel.PhoneNumber,                   
                    AccountNumber = airlineViewModel.AccountNumber,
                    //ListInvoices = airlineViewModel.ListInvoices,
                    Logo = airlineViewModel.Logo,
                    
                };

                airlineEntities.Add(newEntity);
                _myDatabase.AddAirline(newEntity);  
                return RedirectToAction("List");
            }          
            return View(airlineViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //Airline Role + Admin
        [HttpGet]
        public IActionResult Detail(Guid ID)
        {
            var thisAirline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            var airlineDetailViewModel = new AirlineDetailViewModel()
            {
                AirlineID = thisAirline.AirlineID,
                Name = thisAirline.Name,
                NameTag = thisAirline.NameTag,
                CompanyNumber = thisAirline.CompanyNumber,
                CurrentPlanes = thisAirline.CurrentPlanes,
                Adress = thisAirline.Adress,
                Number = thisAirline.Number,
                City = thisAirline.City,
                Email = thisAirline.Email,
                PhoneNumber = thisAirline.PhoneNumber,
                AccountNumber = thisAirline.AccountNumber,
                //ListInvoices = airlineViewModel.ListInvoices,
                Logo = thisAirline.Logo,
            };
            List<Plane> planeEntities = _myDatabase.GetPlanes();
            airlineDetailViewModel.CurrentPlanes = planeEntities.FindAll(x => x.CurrentAirline.AirlineID == ID);
            return View(airlineDetailViewModel);
           
        }

        [Authorize(Roles = "Admin, Airline")]
        //Airline?? Zeker Admin   //evt minder laten editen indien airline - Tijs
        [HttpGet]
        public IActionResult Update(Guid ID)
        {  
            AirlineEditViewModel airlineUpdateViewModel = new AirlineEditViewModel();
            foreach (var airline in airlineEntities)
            {
                if (airline.AirlineID == ID)
                {
                    airlineUpdateViewModel.AirlineID = airline.AirlineID;
                    airlineUpdateViewModel.Name = airline.Name;
                    airlineUpdateViewModel.NameTag = airline.NameTag;
                    airlineUpdateViewModel.CompanyNumber = airline.CompanyNumber;
                    airlineUpdateViewModel.CurrentPlanes = airline.CurrentPlanes;
                    airlineUpdateViewModel.Adress = airline.Adress;
                    airlineUpdateViewModel.Number = airline.Number;
                    airlineUpdateViewModel.City = airline.City;
                    airlineUpdateViewModel.Email = airline.Email;
                    airlineUpdateViewModel.PhoneNumber = airline.PhoneNumber;
                    airlineUpdateViewModel.AccountNumber = airline.AccountNumber;
                    airlineUpdateViewModel.Logo = airline.Logo;
                    break;
                }
            }
            return View(airlineUpdateViewModel);
        }

        [Authorize(Roles = "Admin")]
        //Airline?? Zeker Admin   //evt minder laten editen indien airline - Tijs
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(AirlineEditViewModel airlineUpdateViewModel)
        {

            await Task.Delay(1500);
            var isValid = TryValidateModel(airlineUpdateViewModel);
            if (isValid)
            {
                Airline myairline = airlineEntities.FirstOrDefault(x => x.AirlineID == airlineUpdateViewModel.AirlineID);
                
                var newEntity = new Airline
                {

                    AirlineID = (Guid)airlineUpdateViewModel.AirlineID,
                    Name = airlineUpdateViewModel.Name,
                    NameTag = airlineUpdateViewModel.NameTag,
                    CompanyNumber = airlineUpdateViewModel.CompanyNumber,
                    CurrentPlanes = airlineUpdateViewModel.CurrentPlanes,
                    Adress = airlineUpdateViewModel.Adress,
                    Number = airlineUpdateViewModel.Number,
                    City = airlineUpdateViewModel.City,
                    Email = airlineUpdateViewModel.Email,
                    PhoneNumber = airlineUpdateViewModel.PhoneNumber,
                    AccountNumber = airlineUpdateViewModel.AccountNumber,
                    Logo = airlineUpdateViewModel.Logo,
                };
                airlineEntities.Remove(myairline);
                airlineEntities.Add(newEntity);                
                _myDatabase.UpdateAirline(newEntity);
                return RedirectToAction("List");
            }
            return View(airlineUpdateViewModel);
        }

        [Authorize(Roles = "Admin")]
        //Admin Role
        [HttpGet]
        public IActionResult Delete(Guid ID)
        {
            var airline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            AirlineDeleteViewModel airlineDeleteViewModel = new AirlineDeleteViewModel
            {
                AirlineID = airline.AirlineID,
                Name = airline.Name,
                CompanyNumber = airline.CompanyNumber,
                Adress = airline.Adress,
                Number = airline.Number,
                City = airline.City,
                Logo = airline.Logo,
            };
            return View(airlineDeleteViewModel);
        }

        [Authorize(Roles = "Admin")]
        //ADMIN role
        public async Task<IActionResult> DeleteConfirm(Guid ID)
        {
            await Task.Delay(1500);
            var airline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            airlineEntities.Remove(airline);
            _myDatabase.RemoveAirline(airline);
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin, Airliner")]
        //Airliner + Admine Role
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateImg(IFormFile uploadFile)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);

                var filePath = Path.Combine(@"wwwroot\Images\", fileName);
                string[] subbie = filePath.Split('\\');
                AirlineCreateViewModel mymodel = new AirlineCreateViewModel();
                mymodel.Logo = "~/" + subbie[1] + "/" + subbie[2]; 
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileStream);

                }
                return View(mymodel);
            }
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> FLightList(Guid ID)
        {
            List<Flight> flightstodisplay= new List<Flight>();
            List<Flight> myflightlist = _myDatabase.GetFlights();
            flightstodisplay = myflightlist.FindAll(x => x.CurrentPlane.CurrentAirline.AirlineID == ID);
            List<FlightListViewModel> mymodel = new List<FlightListViewModel>();
            foreach (Flight flight in flightstodisplay)
            {
                FlightListViewModel x = new FlightListViewModel
                {
                    FlightID = flight.FlightID,
                    FlightNr = flight.FlightNr,
                    Departure = flight.Departure,
                    Destination = flight.Destination,
                    Status = flight.Status,                
               };
               mymodel.Add(x);
            }           
            return View(mymodel);
        }
    }
}
