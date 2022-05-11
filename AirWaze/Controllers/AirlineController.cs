using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AirlineController : Controller
    {
        private static List<Airline> airlineEntities = new List<Airline>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var airlineCreateViewModel = new AirlineCreateViewModel();
            
            return View(airlineCreateViewModel);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Create(AirlineCreateViewModel airlineViewModel)
        {
            var isValid = TryValidateModel(airlineViewModel);

            if (isValid)
            {
                var newEntity = new Airline
                {
                    AirlineID = Guid.NewGuid(),
                    Name = airlineViewModel.Name,
                    CompanyNumber = airlineViewModel.CompanyNumber,
                    CurrentPlanes = airlineViewModel.CurrentPlanes,
                    Adress = airlineViewModel.Adress,
                    Email = airlineViewModel.Email,
                    PhoneNumber = airlineViewModel.PhoneNumber,                   
                    AccountNumber = airlineViewModel.AccountNumber,
                    //ListInvoices = airlineViewModel.ListInvoices,
                    Logo = airlineViewModel.Logo,
                };

                airlineEntities.Add(newEntity);
                //_myDatabase.AddAirline(newEntity);  
                return RedirectToAction("Index");
            }          
            return View(airlineViewModel);
        }

        [HttpGet]
        public IActionResult Detail(Guid ID)
        {
            var thisAirline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            var airlineDetailViewModel = new AirlineDetailViewModel()
            {
                AirlineID = thisAirline.AirlineID,
                Name = thisAirline.Name,
                CompanyNumber = thisAirline.CompanyNumber,
                CurrentPlanes = thisAirline.CurrentPlanes,
                Adress = thisAirline.Adress,
                Email = thisAirline.Email,
                PhoneNumber = thisAirline.PhoneNumber,
                AccountNumber = thisAirline.AccountNumber,
                //ListInvoices = airlineViewModel.ListInvoices,
                Logo = thisAirline.Logo,
            };
            var isValid = TryValidateModel(airlineDetailViewModel);
            if (isValid)
            {
                return View(airlineDetailViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(Guid ID)
        {  
            AirlineEditViewModel airlineUpdateViewModel = new AirlineEditViewModel();
            foreach (var airline in airlineEntities)
            {
                if (airline.AirlineID == ID)
                {
                    airlineUpdateViewModel.Name = airline.Name;
                    airlineUpdateViewModel.CompanyNumber = airline.CompanyNumber;
                    airlineUpdateViewModel.CurrentPlanes = airline.CurrentPlanes;
                    airlineUpdateViewModel.Adress = airline.Adress;
                    airlineUpdateViewModel.Email = airline.Email;
                    airlineUpdateViewModel.PhoneNumber = airline.PhoneNumber;
                    airlineUpdateViewModel.AccountNumber = airline.AccountNumber;
                    airlineUpdateViewModel.Logo = airline.Logo;
                    break;
                }
            }
            return View(airlineUpdateViewModel);
        }      

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Update(AirlineEditViewModel airlineUpdateViewModel)
        {

            var isValid = TryValidateModel(airlineUpdateViewModel);
            if (isValid)
            {
                Airline myairline = airlineEntities.FirstOrDefault(x => x.AirlineID == airlineUpdateViewModel.AirlineID);
                airlineEntities.Remove(airlineEntities.FirstOrDefault(x => x.AirlineID == airlineUpdateViewModel.AirlineID));
                var newEntity = new Airline
                {

                    AirlineID = myairline.AirlineID,
                    Name = myairline.Name,
                    CompanyNumber = myairline.CompanyNumber,
                    CurrentPlanes = myairline.CurrentPlanes,
                    Adress = myairline.Adress,
                    Email = myairline.Email,
                    PhoneNumber = myairline.PhoneNumber,
                    AccountNumber = myairline.AccountNumber,
                    Logo = myairline.Logo,
                };
                airlineEntities.Add(newEntity);
                //_myDatabase.UpdateAirline(newEntity);
                return RedirectToAction("Index");
            }
            return View(airlineUpdateViewModel);
        }

    }
}
