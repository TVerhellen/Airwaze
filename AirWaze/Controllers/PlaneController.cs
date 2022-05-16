using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
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
            if (_myDatabase == null)
            {
                _myDatabase = mydatabase;
                planeEntities = _myDatabase.GetPlanes();
                airlineEntities = _myDatabase.GetAirlines();
            }                                             
        }
        
        // AIRLINE ROLE
        [HttpGet]
        public async Task<IActionResult> Index(Guid ID)
        {
            LoggedInAirline = airlineEntities.FirstOrDefault(x => x.AirlineID == ID);
            if (LoggedInAirline == null)
            {

                LoggedInAirline = new Airline
                {
                    Name = "Harald Airways",
                };
                LoggedInAirline = airlineEntities.FirstOrDefault(x => x.Name == LoggedInAirline.Name);
               

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
            var planeCreateViewModel = planesToAdd[0];

            return View(planeCreateViewModel);
        }
        //Airline
        [HttpGet]
        public IActionResult AddPlane(Airline ID)
        {
            var planeCreateViewModel = planesToAdd[0];

            planeCreateViewModel.CurrentAirline = ID;
            return View(planeCreateViewModel);
        }
        //AIRLINE + ADMIN
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PlaneCreateViewModel planeViewModel)
        {
            if (LoggedInAirline == null)
            {

                LoggedInAirline = new Airline
                {
                    Name = "Harald Airways",
                };
                LoggedInAirline = airlineEntities.FirstOrDefault(x => x.Name == LoggedInAirline.Name);


            }

            if (LoggedInAirline.CurrentPlanes == null)
            {
                LoggedInAirline.CurrentPlanes = new List<Plane>();
            }
            planeViewModel.CurrentAirline = LoggedInAirline;
            var isValid = TryValidateModel(planeViewModel);
            Random generator = new Random();
            if (isValid)
            {
                //UniqueCOmbo with Meaning

                string tempPlaneNr = planeViewModel.CurrentAirline.NameTag + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + generator.Next(0, 100).ToString() + planeViewModel.FlightRegion;
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
                    SeatDiagramPic = planeViewModel.SeatDiagramPic
                    
                };
                planeEntities.Add(newEntity);
                _myDatabase.AddPlane(newEntity);
                planeEntities = _myDatabase.GetPlanes();
                airlineEntities = _myDatabase.GetAirlines();
                return RedirectToAction("Index", newEntity.CurrentAirline.AirlineID);
            }
            return View(planeViewModel);
        }

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
                Type = thisPlane.Type

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
                    Type = planeUpdateViewModel.Type

                };               
                _myDatabase.UpdatePlane(newEntity);
                planeEntities = _myDatabase.GetPlanes();
                airlineEntities = _myDatabase.GetAirlines();
                return RedirectToAction("Index");
            }
            return View(planeUpdateViewModel);
        }

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

        //AIRLINE + ADMIN
        public async Task<IActionResult> DeleteConfirm(string ID)
        {
            ID = ID.Replace("%2F", "/");
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            planeEntities.Remove(plane);
            _myDatabase.RemovePlane(plane);
            return RedirectToAction("Index");
        }
       
        [Route("Kippen")]
        public IActionResult Type()
        {          
            return View();
        }

      
    }
}
