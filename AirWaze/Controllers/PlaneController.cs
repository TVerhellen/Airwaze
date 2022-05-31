using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AirWaze.Controllers
{
    public class PlaneController : Controller
    {

        private IAirWazeDatabase _myDatabase;

        public static List<PlaneCreateViewModel> planesToAdd = new List<PlaneCreateViewModel>();
              
        public static List<Plane> planeEntities = new List<Plane>();

        private static List<Airline> airlineEntities = new List<Airline>();

        private static List<string> regionList = new List<string>
        {
            "EUR","NA","SA","ASI","ME","AF","OC"
        };

        public static Airline LoggedInAirline;

        //Gets All Entities of Planes - Will do For all uses!
        public PlaneController(IAirWazeDatabase mydatabase)
        {
            List<Plane> oldlist = planeEntities.ToList();
            if (_myDatabase == null)
            {
                _myDatabase = mydatabase;               
            }
            planeEntities = _myDatabase.GetPlanes();
            foreach (Plane x in planeEntities)
            {
                foreach (Plane y in oldlist)
                {
                    if (x.PlaneID == y.PlaneID)
                    {
                        if (x.IsAvailable != y.IsAvailable)
                        {
                            x.IsAvailable = y.IsAvailable;
                            //_myDatabase.UpdatePlane(x);
                        }
                    }
                }
            }
            airlineEntities = _myDatabase.GetAirlines();
        }

        [Authorize(Roles = "Airline")]
        // AIRLINE ROLE
        [HttpGet]
        public async Task<IActionResult> Index(Guid ID, string searchString, string option)
        {
            ViewData["CurrentFilter"] = searchString;
            LoggedInAirline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            if (LoggedInAirline == null)
            {

                LoggedInAirline = new Airline
                {
                    Name = "RyanAir",
                };
                LoggedInAirline =  airlineEntities.FirstOrDefault(x => x.Name == LoggedInAirline.Name);
               
            }

            if (LoggedInAirline.CurrentPlanes == null)
            {
                LoggedInAirline.CurrentPlanes = new List<Plane>();
            }
            List<Plane> planelistAirline = new List<Plane>();
            foreach (Plane x in planeEntities)
            {
                if (x.CurrentAirline.AirlineID == LoggedInAirline.AirlineID) 
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
                    AirMiles = plane.AirMiles,
                    ConstructionYear = plane.ConstructionYear,
                    FlightHours = plane.FlightHours,
                    NextMainentance = plane.NextMainentance,
                }) ;
            }
            //searchfunction
            var myPlane = from s in thislist
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (option == "Name")
                {
                    myPlane = myPlane.Where(s => s.PlaneNr.Contains(searchString));
                }
                else if (option == "Type")
                {
                    myPlane = myPlane.Where(s => s.Type.Contains(searchString));
                }
                else if (option == "Capacity")
                {
                    myPlane = myPlane.Where(s => s.PassengerCapacity.ToString().Contains(searchString)).ToList();
                }
                else if (option == "Date")
                {
                    myPlane = myPlane.Where(s => s.ConstructionYear.ToString("dd/MM/yyyy").Contains(searchString) || s.ConstructionYear.ToString("dd-MM-yyyy").Contains(searchString)).ToList();
                }
            }
            return View(myPlane.ToList());

            //return View(thislist);
        }

        [Authorize(Roles = "Admin")]

        //ADMIN ROLE
        public async Task<IActionResult> List(string option, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
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
                    AirMiles = plane.AirMiles,
                    ConstructionYear = plane.ConstructionYear,
                    FlightHours = plane.FlightHours,
                    NextMainentance = plane.NextMainentance,
                });
            }
            //searchfunction
            var myPlane = from s in thislist
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (option == "Name")
                {
                    myPlane = myPlane.Where(s => s.PlaneNr.Contains(searchString));
                }
                else if (option == "Type")
                {
                    myPlane = myPlane.Where(s => s.Type.Contains(searchString));
                }
                else if (option == "Capacity")
                {
                    myPlane = myPlane.Where(s => s.PassengerCapacity.ToString().Contains(searchString)).ToList();
                }
                else if (option == "Date")
                {
                    myPlane = myPlane.Where(s => s.ConstructionYear.ToString("dd/MM/yyyy").Contains(searchString) || s.ConstructionYear.ToString("dd-MM-yyyy").Contains(searchString)).ToList();
                }
            }
            return View(myPlane.ToList());
        }

        [Authorize(Roles = "Admin")]
        //ADMIN
        [HttpGet]
        public IActionResult Create()
        {
            var planeCreateViewModel  = new PlaneCreateViewModel();
            if (planesToAdd.Count != 0)
            {
                planeCreateViewModel = planesToAdd[0];
            }
                      
            return View(planeCreateViewModel);
        }

        [Authorize(Roles = "Airline, Admin")]
        //Airline
        [HttpGet]
        public IActionResult AddPlane(Airline ID)
        {
            var planeCreateViewModel = planesToAdd[0];
            planeCreateViewModel.CurrentAirline = ID;
            return View(planeCreateViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PlaneCreateViewModel planeViewModel)
        {
            await Task.Delay(1500);
            if (LoggedInAirline == null)
            {

                LoggedInAirline = new Airline
                {
                    Name = "AirWazeAir",
                };
                LoggedInAirline = airlineEntities.FirstOrDefault(x => x.Name == LoggedInAirline.Name);
                if (LoggedInAirline == null)
                {
                    LoggedInAirline = new Airline
                    {
                        Name = "AirWazeAir",
                    };
                    LoggedInAirline.CurrentPlanes = new List<Plane>();
                }                
            }
            else if (LoggedInAirline.CurrentPlanes == null)
            {
                LoggedInAirline.CurrentPlanes = new List<Plane>();
            }
            planeViewModel.CurrentAirline = LoggedInAirline;
            var isValid = TryValidateModel(planeViewModel);
            Random generator = new Random();
            if (isValid)
            {
                //UniqueCOmbo with Meaning

                string tempPlaneNr = planeViewModel.Type + "-"  + planeViewModel.CurrentAirline.NameTag + "-" + planeViewModel.FlightRegion + generator.Next(0,1000).ToString();
                var newEntity = new Plane
                {
                    
                    PlaneNr = tempPlaneNr,
                    FlightRegion = planeViewModel.FlightRegion,      
                    CurrentAirline = planeViewModel.CurrentAirline,
                    PassengerCapacity = planeViewModel.PassengerCapacity,
                    FuelUsagePerKM = planeViewModel.FuelUsagePerKM,
                    FirstClassCapacity = planeViewModel.FirstClassCapacity,                   
                    FuelCapacity = planeViewModel.FuelCapacity,
                    IsAvailable = planeViewModel.IsAvailable,
                    LoadCapacity = planeViewModel.LoadCapacity,
                    Manufacturer = planeViewModel.Manufacturer,
                    Type = planeViewModel.Type  ,
                    SeatDiagramPic = planeViewModel.SeatDiagramPic,
                    AirMiles = planeViewModel.AirMiles,
                    ConstructionYear = planeViewModel.ConstructionYear,
                    FlightHours = planeViewModel.FlightHours,
                    NextMainentance = planeViewModel.NextMainentance,

                };
                planesToAdd.Clear();
                planeEntities.Add(newEntity);
                _myDatabase.AddPlane(newEntity);
                //planeEntities = _myDatabase.GetPlanes();
                //airlineEntities = _myDatabase.GetAirlines();
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Index", newEntity.CurrentAirline.AirlineID);
                }              
            }
            return View(planeViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Detail(string ID)
        {
            ID = ID.Replace("%2F", "/");
            var thisPlane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            var planeDetailViewModel = new PlaneDetailViewModel()
            {
                PlaneNr = thisPlane.PlaneNr,
                FlightRegion = thisPlane.FlightRegion,
                CurrentAirline = thisPlane.CurrentAirline,
                PassengerCapacity = thisPlane.PassengerCapacity,
                FuelUsagePerKM = thisPlane.FuelUsagePerKM,
                FirstClassCapacity = thisPlane.FirstClassCapacity,
                FuelCapacity = thisPlane.FuelCapacity,
                IsAvailable = thisPlane.IsAvailable,
                LoadCapacity = thisPlane.LoadCapacity,
                Manufacturer = thisPlane.Manufacturer,
                Type = thisPlane.Type,
                SeatDiagramPic = thisPlane.SeatDiagramPic,
                AirMiles = thisPlane.AirMiles,
                ConstructionYear = thisPlane.ConstructionYear,
                FlightHours = thisPlane.FlightHours,
                NextMainentance = thisPlane.NextMainentance,

            };
            var isValid = TryValidateModel(planeDetailViewModel);
            if (isValid)
            {
                return View(planeDetailViewModel);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Update(string ID)
        {
            ID = ID.Replace("%2F", "/");
            PlaneEditViewModel planeUpdateViewModel = new PlaneEditViewModel();
            foreach (var plane in planeEntities)
            {
                if (plane.PlaneNr == ID)
                {
                    planeUpdateViewModel.PlaneNr = plane.PlaneNr;
                    planeUpdateViewModel.FlightRegion = plane.FlightRegion;
                    planeUpdateViewModel.CurrentAirline = plane.CurrentAirline;
                    planeUpdateViewModel.PassengerCapacity = plane.PassengerCapacity;
                    planeUpdateViewModel.FuelUsagePerKM = plane.FuelUsagePerKM;
                    planeUpdateViewModel.FirstClassCapacity = plane.FirstClassCapacity;
                    planeUpdateViewModel.FuelCapacity = plane.FuelCapacity;
                    planeUpdateViewModel.IsAvailable = plane.IsAvailable;
                    planeUpdateViewModel.LoadCapacity = plane.LoadCapacity;
                    planeUpdateViewModel.Manufacturer = plane.Manufacturer;
                    planeUpdateViewModel.Type = plane.Type;
                    planeUpdateViewModel.AirMiles = plane.AirMiles;
                    planeUpdateViewModel.ConstructionYear = plane.ConstructionYear;
                    planeUpdateViewModel.FlightHours = plane.FlightHours;
                    planeUpdateViewModel.NextMainentance = plane.NextMainentance;
                    planeUpdateViewModel.SeatDiagramPic = plane.SeatDiagramPic;
                    break;
                }
            }
            return View(planeUpdateViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(PlaneEditViewModel planeUpdateViewModel)
        {
            await Task.Delay(1500);
            var isValid = TryValidateModel(planeUpdateViewModel);
            if (isValid)
            {
                
                Plane myplane = planeEntities.FirstOrDefault(x => x.PlaneNr ==planeUpdateViewModel.PlaneNr);
                planeUpdateViewModel.CurrentAirline = myplane.CurrentAirline;              
                var newEntity = new Plane
                {

                    PlaneID = myplane.PlaneID,
                    PlaneNr = planeUpdateViewModel.PlaneNr,
                    FlightRegion = planeUpdateViewModel.FlightRegion,
                    CurrentAirline = planeUpdateViewModel.CurrentAirline,
                    PassengerCapacity = planeUpdateViewModel.PassengerCapacity,
                    FuelUsagePerKM = planeUpdateViewModel.FuelUsagePerKM,
                    FirstClassCapacity = planeUpdateViewModel.FirstClassCapacity,
                    FuelCapacity = planeUpdateViewModel.FuelCapacity,
                    IsAvailable = planeUpdateViewModel.IsAvailable,
                    LoadCapacity = planeUpdateViewModel.LoadCapacity,
                    Manufacturer = planeUpdateViewModel.Manufacturer,
                    Type = planeUpdateViewModel.Type,
                    AirMiles = planeUpdateViewModel.AirMiles,
                    ConstructionYear = planeUpdateViewModel.ConstructionYear,
                    FlightHours = planeUpdateViewModel.FlightHours,
                    NextMainentance = planeUpdateViewModel.NextMainentance,
                    SeatDiagramPic = planeUpdateViewModel?.SeatDiagramPic
                };               
                _myDatabase.UpdatePlane(newEntity);
                planeEntities = _myDatabase.GetPlanes();
                airlineEntities = _myDatabase.GetAirlines();
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(planeUpdateViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        [HttpGet]
        public IActionResult Delete(string ID)
        {
            ID = ID.Replace("%2F", "/");
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            PlaneDeleteViewModel planeDeleteViewModel = new PlaneDeleteViewModel
            {
                PlaneNr = plane.PlaneNr,
                CurrentAirline = plane.CurrentAirline,
                Manufacturer = plane.Manufacturer,
                Type = plane.Type
            };
            return View(planeDeleteViewModel);
        }

        [Authorize(Roles = "Admin, Airline")]
        //AIRLINE + ADMIN
        public async Task<IActionResult> DeleteConfirm(string ID)
        {
            await Task.Delay(1500);
            ID = ID.Replace("%2F", "/");
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            planeEntities.Remove(plane);
            _myDatabase.RemovePlane(plane);
            return RedirectToAction("Index");
        }
       
        [Route("Type")]
        public IActionResult Type()
        {          
            return View();
        }

      
    }
}
