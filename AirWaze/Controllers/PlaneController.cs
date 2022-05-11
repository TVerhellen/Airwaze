using AirWaze.Entities;
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

        public async Task<IActionResult> Index()
        {
            List<PlaneListViewModel> thislist = new List<PlaneListViewModel>();
            foreach (var plane in planeEntities)
            {
                thislist.Add(new PlaneListViewModel()
                {
                    PlaneNr = plane.PlaneNr
                    //TO DO ADD PROPS
                });
            }
            return View(thislist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var planeCreateViewModel = new PlaneCreateViewModel();

            return View(planeCreateViewModel);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PlaneCreateViewModel planeViewModel)
        {
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
                    //TO DO ADD PROPS                
                };
                planeEntities.Add(newEntity);
                //await _myDatabase.AddPlane(newEntity);  
                return RedirectToAction("Index");
            }
            return View(planeViewModel);
        }

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
        public async Task<IActionResult> DeleteConfirm(string ID)
        {
            var plane = planeEntities.FirstOrDefault(x => x.PlaneNr == ID);
            planeEntities.Remove(plane);
            //await _myDatabase.RemovePlane(plane);
            return RedirectToAction("Index");
        }
    }
}
