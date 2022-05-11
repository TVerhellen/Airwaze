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
    }
}
