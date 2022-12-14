using AirWaze.Areas.Identity.Data;
using AirWaze.Data;
using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IAirWazeDatabase _airWazeDatabase;
        private static List<AirWazeUser> userEntities = new List<AirWazeUser>();
        public UserController(IAirWazeDatabase airWazeDatabase)
        {
            _airWazeDatabase = airWazeDatabase;
            userEntities = _airWazeDatabase.GetUsers();
        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [HttpGet]
        public IActionResult Index()
        {
            var userViewModels = new List<UserListViewModel>();
            foreach (var user in _airWazeDatabase.GetUsers())
            {
                userViewModels.Add(new UserListViewModel() { UserID = user.Id, FirstName = user.FirstName, LastName = user.LastName });
            }
            return View(userViewModels);
            
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet]
        public IActionResult CustomerHome()
        {

            return View();

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [Route("User/Detail/{UserID}")]
        [HttpGet]
        public IActionResult Detail(string UserID)
        {
            var users = _airWazeDatabase.GetUserByID(UserID);
            if (users != null)
            {
                var viewModel = new UserDetailViewModel
                {
                    UserID = users.Id,
                    FirstName = users.FirstName,
                    LastName = users.LastName,
                    //StreetName = users.StreetName,
                    //HouseNumber = users.HouseNumber,
                    //Bus = users.Bus,
                    //City = users.City,
                    //Zipcode = users.Zipcode,
                    //Country = users.Country,
                    Email = users.Email,
                    PhoneNumber = users.PhoneNumber,
                };
                return View(viewModel);
            }
            return View();

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [HttpGet]
        public IActionResult Create()
        {
            var userCreateViewModel = new UserCreateViewModel();
            return View(userCreateViewModel);
        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel userViewModel)
        {
            var isModelValid = TryValidateModel(userViewModel);

            if (isModelValid)
            {
                var pasinfo = db.Users.FirstOrDefault(d => d.Email == userViewModel.Email);
                if (pasinfo == null)
                {
                    //pasinfo = db.Users.Create();
                    pasinfo.Email = userViewModel.Email;
                    db.SaveChanges();
                }

                var newEntity = new AirWazeUser
                {
                    Id = userViewModel.UserID,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    StreetName = userViewModel.StreetName,
                    HouseNumber = userViewModel.HouseNumber,
                    Bus = userViewModel.Bus,
                    City = userViewModel.City,
                    Zipcode = userViewModel.Zipcode,
                    Country = userViewModel.Country,
                    Email = pasinfo.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                };
                //userEntities.Add(newEntity);
                //_airWazeDatabase.AddUser(newEntity);
                return RedirectToAction("Index");
            }
            return View(userViewModel);

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [Route("User/Edit/{UserID}")]
        [HttpGet]
        public ActionResult Edit(string UserID)
        {
            var existingUser = _airWazeDatabase.GetUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();

            var userEditViewModel = new UserEditViewModel
            {
                UserID = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                StreetName = existingUser.StreetName,
                HouseNumber = existingUser.HouseNumber,
                Bus = existingUser.Bus,
                City = existingUser.City,
                Zipcode = existingUser.Zipcode,
                Country = existingUser.Country,
                Email = existingUser.Email,
                PhoneNumber = existingUser.PhoneNumber,
            };
            return View(userEditViewModel);

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(string UserID, [FromForm]UserEditViewModel newUser)
        {
            if (!TryValidateModel(newUser)) return View(newUser);

            var existingUser = _airWazeDatabase.GetUserByID(UserID);

            if (existingUser == null) return new NotFoundResult();

            existingUser.FirstName = newUser.FirstName;
            existingUser.LastName = newUser.LastName;
            //existingUser.StreetName = newUser.StreetName;
            //existingUser.HouseNumber = newUser.HouseNumber;
            //existingUser.Bus = newUser.Bus;
            //existingUser.City = newUser.City;
            //existingUser.Country = newUser.Country;
            //existingUser.Zipcode = newUser.Zipcode;
            existingUser.PhoneNumber = newUser.PhoneNumber;
            existingUser.Email = newUser.Email;

            //_airWazeDatabase.UpdateUser(existingUser);

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [Route("User/Delete/{UserID}")]
        [HttpGet]
        public ActionResult Delete(string UserID)
        {
            var existingUser = _airWazeDatabase.GetUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();
            var deleteViewModel = new UserDeleteViewModel
            {
                UserID = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
            };
            return View(deleteViewModel);

        }

        [Authorize(Roles = "Admin, Customer")]
        //only user + admin
        [Route("User/Delete/{UserID}")]
        [HttpPost]
        public ActionResult ConfirmDelete(string UserID)
        {
            var existingUser = _airWazeDatabase.GetUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();
            //_airWazeDatabase.RemoveUser(existingUser);
            return RedirectToAction("Index");
        }
    }
}
