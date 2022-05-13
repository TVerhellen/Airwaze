using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
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
                airlineEntities = _myDatabase.GetAirlines();
            }                      
        }

        //Airline Role
        public IActionResult Index()
        {
            AirlineIndexViewModel mymodel = new AirlineIndexViewModel();          
            if (LoggedInAirline == null)
            {
                
                LoggedInAirline = new Airline
                {
                    Name = "Harald Airways",
                };
                LoggedInAirline = airlineEntities.FirstOrDefault(x => x.Name == LoggedInAirline.Name);
                
            }
            mymodel.Airline = LoggedInAirline;
            return View(mymodel);
        }

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

        //Guest Role + Admin
        [HttpGet]
        public IActionResult Create()
        {
            var airlineCreateViewModel = new AirlineCreateViewModel();
            
            return View(airlineCreateViewModel);
        }

        //Guest Role + Admin
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(AirlineCreateViewModel airlineViewModel)
        {
            var isValid = TryValidateModel(airlineViewModel);

            if (isValid)
            {
                var newEntity = new Airline
                {
                    AirlineID = Guid.NewGuid(),
                    Name = airlineViewModel.Name,
                    NameTag = airlineViewModel.NameTag,
                    CompanyNumber = airlineViewModel.CompanyNumber,
                    CurrentPlanes = new List<Plane>(),
                    Adress = airlineViewModel.Adress,
                    Number = airlineViewModel.Number,
                    City = airlineViewModel.City,
                    Email = airlineViewModel.Email,
                    PhoneNumber = airlineViewModel.PhoneNumber,                   
                    AccountNumber = airlineViewModel.AccountNumber,
                    //ListInvoices = airlineViewModel.ListInvoices,
                    //Logo = airlineViewModel.Logo,
                    
                };

                airlineEntities.Add(newEntity);
                _myDatabase.AddAirline(newEntity);  
                return RedirectToAction("Index");
            }          
            return View(airlineViewModel);
        }

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
            
            return View(airlineDetailViewModel);
           
        }

        //Airline?? Zeker Admin
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

        //Airline?? Zeker Admin
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(AirlineEditViewModel airlineUpdateViewModel)
        {

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
                City = airline.City
            };
            return View(airlineDeleteViewModel);
        }

        //ADMIN role
        public async Task<IActionResult> DeleteConfirm(Guid ID)
        {
            var airline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            airlineEntities.Remove(airline);
            _myDatabase.RemoveAirline(airline);
            return RedirectToAction("List");
        }
    }
}
