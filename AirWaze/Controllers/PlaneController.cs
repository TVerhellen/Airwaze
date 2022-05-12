﻿using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AirWaze.Controllers
{
    public class PlaneController : Controller
    {       
        
        private static List<Plane> planeEntities = new List<Plane>();

        private static List<string> regionList = new List<string>
        {
            "EUR","NA","SA","ASI","ME","AF","OC"
        };


        //Gets All Entities of Planes - Will do For all uses!
        public PlaneController()
        {
            if (planeEntities.Count == 0)
            {
                Airline testAirline = new Airline
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
                Plane testplane = new Plane
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
                planeEntities.Add(testplane);
            }
            
        }

        // AIRLINE ROLE
        public async Task<IActionResult> Index(Guid ID)
        {
            planeEntities[0].CurrentAirline.AirlineID = ID;
            
            List<Plane> planelistAirline = new List<Plane>();
            foreach (Plane x in planeEntities)
            {
                if (x.CurrentAirline.AirlineID == ID)
                {
                    planelistAirline.Add(x);
                }
            }                 
            List<PlaneListViewModel> thislist = new List<PlaneListViewModel>();
            foreach (var plane in planelistAirline)
            {
                thislist.Add(new PlaneListViewModel()
                {
                    PlaneNr = plane.PlaneNr,                    
                    CurrentAirline = plane.CurrentAirline,
                    PassengerCapacity = plane.PassengerCapacity,                    
                    FlightRegion = plane.FlightRegion,                    
                    IsAvailable = plane.IsAvailable, 
                    Manufacturer = plane.Manufacturer,
                    Type = plane.Type,
                });
            }
            return View(thislist);
        }

        //ADMIN ROLE
        public async Task<IActionResult> List()
        {
            List<PlaneListViewModel> thislist = new List<PlaneListViewModel>();
            foreach (var plane in planeEntities)
            {
                thislist.Add(new PlaneListViewModel()
                {
                    PlaneNr = plane.PlaneNr,
                    CurrentAirline = plane.CurrentAirline,
                    PassengerCapacity = plane.PassengerCapacity,
                    FlightRegion = plane.FlightRegion,
                    IsAvailable = plane.IsAvailable,
                    Manufacturer = plane.Manufacturer,
                    Type = plane.Type,
                });
            }
            return View(thislist);
        }

        //ADMIN
        [HttpGet]
        public IActionResult Create()
        {
            var planeCreateViewModel = new PlaneCreateViewModel();

            return View(planeCreateViewModel);
        }
        //Airline
        [HttpGet]
        public IActionResult AddPlane(Airline ID)
        {
            var planeCreateViewModel = new PlaneCreateViewModel();

            planeCreateViewModel.CurrentAirline = ID;
            return View(planeCreateViewModel);
        }
        //AIRLINE + ADMIN
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PlaneCreateViewModel planeViewModel)
        {
            planeViewModel.CurrentAirline = planeEntities[0].CurrentAirline;
            var isValid = TryValidateModel(planeViewModel);
            Random generator = new Random();
            if (isValid)
            {
                //UniqueCOmbo with Meaning

                string tempPlaneNr = planeViewModel.CurrentAirline.NameTag + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + generator.Next(0, 100).ToString() + planeViewModel.FlightRegion;
                var newEntity = new Plane
                {
                    
                    PlaneNr = tempPlaneNr,
                    //FROM Cmb box
                    FlightRegion = planeViewModel.FlightRegion,      
                    CurrentAirline = planeViewModel.CurrentAirline,
                    PassengerCapacity = planeViewModel.PassengerCapacity,
                    FuelUsagePerKM = planeViewModel.FuelUsagePerKM,
                    FirstClassCapacity = planeViewModel.FirstClassCapacity,                   
                    FuelCapacity = planeViewModel.FuelCapacity,
                    IsAvailable = planeViewModel.IsAvailable,
                    LoadCapacity = planeViewModel.LoadCapacity,
                    Manufacturer = planeViewModel.Manufacturer,
                    Type = planeViewModel.Type                   
                };
                planeEntities.Add(newEntity);
                //await _myDatabase.AddPlane(newEntity);  
                return RedirectToAction("Index");
            }
            return View(planeViewModel);
        }

        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Detail(string ID)
        {
            var thisPlane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            var planeDetailViewModel = new PlaneDetailViewModel()
            {
                PlaneNr = thisPlane.PlaneNr,
                //ADD PROPS
            };
            var isValid = TryValidateModel(planeDetailViewModel);
            if (isValid)
            {
                return View(planeDetailViewModel);
            }
            return RedirectToAction("Index");
        }

        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Update(string ID)
        {
            PlaneEditViewModel planeUpdateViewModel = new PlaneEditViewModel();
            foreach (var plane in planeEntities)
            {
                if (plane.PlaneNr == ID)
                {
                    //ADD PROPS
                    break;
                }
            }
            return View(planeUpdateViewModel);
        }

        //AIRLINE + ADMIN
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(PlaneEditViewModel planeUpdateViewModel)
        {

            var isValid = TryValidateModel(planeUpdateViewModel);
            if (isValid)
            {
                Plane myplane = planeEntities.FirstOrDefault(x => x.PlaneNr ==planeUpdateViewModel.PlaneNr);
                planeEntities.Remove(planeEntities.FirstOrDefault(x => x.PlaneNr == planeUpdateViewModel.PlaneNr));
                var newEntity = new Plane
                {

                    PlaneNr = myplane.PlaneNr,
                    //ADD PROPS
                    
                };
                planeEntities.Add(newEntity);
                //await _myDatabase.UpdatePlanes(newEntity);
                return RedirectToAction("Index");
            }
            return View(planeUpdateViewModel);
        }

        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Delete(string ID)
        {
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            PlaneDeleteViewModel planeDeleteViewModel = new PlaneDeleteViewModel
            {
                PlaneNr = plane.PlaneNr,
                //ADD PROPS
            };
            return View(planeDeleteViewModel);
        }

        //AIRLINE + ADMIN
        public async Task<IActionResult> DeleteConfirm(string ID)
        {
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            planeEntities.Remove(plane);
            //await _myDatabase.RemovePlane(plane);
            return RedirectToAction("Index");
        }
    }
}
