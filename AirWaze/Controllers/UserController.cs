using AirWaze.Database.Design;
using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class UserController : Controller
    {
        private readonly IAirWazeDatabase _airWazeDatabase;
        public UserController(IAirWazeDatabase airWazeDatabase)
        {
            _airWazeDatabase = airWazeDatabase;
        }
        //private static readonly List<User> userEntities = new List<User>()
        //{
        //    new User
        //    {
        //            UserID = Guid.NewGuid(),
        //            FirstName = "Toinon",
        //            LastName = "Naesen",
        //            Email = "toinon.naesen@gmail.com",
        //            StreetName = "Zeelaan",
        //            HouseNumber = "2",
        //            Zipcode = "8450",
        //            City = "Bredene",
        //            Country = "Belgium",
        //            PhoneNumber = "0472/43.95.43"
        //     }
        //};

        //only user + admin
        [HttpGet]
        public IActionResult Index()
        {
            var userViewModels = new List<UserListViewModel>();
            foreach (var user in _airWazeDatabase.GetUsers())
            {
                userViewModels.Add(new UserListViewModel() { UserID = user.UserID, FirstName = user.FirstName, LastName = user.LastName });
            }
            return View(userViewModels);

            //List<UserListViewModel> userList = new List<UserListViewModel>();
            //foreach (var user in userEntities)
            //{
            //    userList.Add(new UserListViewModel()
            //    {
            //        UserID = user.UserID,
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //    });
            //}
            //return View(userList);
        }

        //only user + admin
        [Route("User/Detail/{UserID:Guid}")]
        [HttpGet]
        public IActionResult Detail(Guid UserID)
        {
            var users = _airWazeDatabase.FindUserByID(UserID);
            if (users != null)
            {
                var viewModel = new UserDetailViewModel
                {
                    UserID = users.UserID,
                    FirstName = users.FirstName,
                    LastName = users.LastName,
                    StreetName = users.StreetName,
                    HouseNumber = users.HouseNumber,
                    Bus = users.Bus,
                    City = users.City,
                    Zipcode = users.Zipcode,
                    Country = users.Country,
                    Email = users.Email,
                    PhoneNumber = users.PhoneNumber,
                };
                return View(viewModel);
            }
            return View();

            //UserDetailViewModel newUser = new UserDetailViewModel();
            //foreach (User user in userEntities)
            //{
            //    if (user.UserID == UserID)
            //    {
            //        newUser.FirstName = user.FirstName;
            //        newUser.LastName = user.LastName;
            //        newUser.Email = user.Email;
            //        newUser.StreetName = user.StreetName;
            //        newUser.HouseNumber = user.HouseNumber;
            //        newUser.Zipcode = user.Zipcode;
            //        newUser.Bus = user.Bus;
            //        newUser.City = user.City;
            //        newUser.Country = user.Country;
            //        newUser.PhoneNumber = user.PhoneNumber;
            //        break;
            //    }
            //}
            //return View(newUser);
        }

        //only user + admin
        [HttpGet]
        public IActionResult Create()
        {
            var userCreateViewModel = new UserCreateViewModel();
            return View(userCreateViewModel);
        }

        //only user + admin
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(UserCreateViewModel userViewModel)
        {
            var isModelValid = TryValidateModel(userViewModel);

            if (isModelValid)
            {
                var newEntity = new User
                {
                    UserID = userViewModel.UserID,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    StreetName = userViewModel.StreetName,
                    HouseNumber = userViewModel.HouseNumber,
                    Bus = userViewModel.Bus,
                    City = userViewModel.City,
                    Zipcode = userViewModel.Zipcode,
                    Country = userViewModel.Country,
                    Email = userViewModel.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                };
                _airWazeDatabase.AddUser(newEntity);
                return RedirectToAction("Index");
            }
            return View(userViewModel);

            //var isModelValid = TryValidateModel(newUser);


            //if (isModelValid)
            //{
            //    userEntities.Add(new User()
            //    {
            //        UserID = Guid.NewGuid(),
            //        FirstName = newUser.FirstName,
            //        LastName = newUser.LastName,
            //        StreetName = newUser.StreetName,
            //        HouseNumber = newUser.HouseNumber,
            //        Bus = newUser.Bus,
            //        Zipcode = newUser.Zipcode,
            //        Country = newUser.Country,
            //        City = newUser.City,
            //        PhoneNumber  = newUser.PhoneNumber,
            //        Email = newUser.Email,
            //    });
            //    return RedirectToAction("Index");
            //}
            //return View(newUser);
        }

        //only user + admin
        [Route("User/Edit/{UserID:Guid}")]
        [HttpGet]
        public ActionResult Edit(Guid UserID)
        {
            var existingUser = _airWazeDatabase.FindUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();

            var userEditViewModel = new UserEditViewModel
            {
                UserID = existingUser.UserID,
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

            //var user = userEntities.FirstOrDefault(x => x.UserID == UserID);
            //UserEditViewModel myUser = new UserEditViewModel()
            //{
            //    UserID = user.UserID,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    StreetName = user.StreetName,
            //    HouseNumber = user.HouseNumber,
            //    Bus = user.Bus,
            //    Zipcode = user.Zipcode,
            //    Country = user.Country,
            //    City = user.City,
            //    PhoneNumber = user.PhoneNumber,
            //    Email = user.Email,
            //};
            //return View(myUser);
        }
        //only user + admin
        [HttpPost]
        public ActionResult Edit(Guid UserID, [FromForm]UserEditViewModel newUser)
        {
            if (!TryValidateModel(newUser)) return View(newUser);

            var existingUser = _airWazeDatabase.FindUserByID(UserID);

            if (existingUser == null) return new NotFoundResult();

            existingUser.FirstName = newUser.FirstName;
            existingUser.LastName = newUser.LastName;
            existingUser.StreetName = newUser.StreetName;
            existingUser.HouseNumber = newUser.HouseNumber;
            existingUser.Bus = newUser.Bus;
            existingUser.City = newUser.City;
            existingUser.Country = newUser.Country;
            existingUser.Zipcode = newUser.Zipcode;
            existingUser.PhoneNumber = newUser.PhoneNumber;
            existingUser.Email = newUser.Email;

            _airWazeDatabase.UpdateUser(existingUser);

            return RedirectToAction("Index");

            //User myUser = new User()
            //{
            //    UserID = newUser.UserID,
            //    FirstName = newUser.FirstName,
            //    LastName = newUser.LastName,
            //    StreetName = newUser.StreetName,
            //    HouseNumber = newUser.HouseNumber,
            //    Bus = newUser.Bus,
            //    Zipcode = newUser.Zipcode,
            //    Country = newUser.Country,
            //    City = newUser.City,
            //    PhoneNumber = newUser.PhoneNumber,
            //    Email = newUser.Email,
            //};
            //var user = userEntities.FirstOrDefault(x => x.UserID == newUser.UserID);
            //var index = userEntities.IndexOf(user);
            //userEntities[index] = myUser;

            //return RedirectToAction("Index");
        }

        
        //only user + admin
        [Route("User/Delete/{UserID:Guid}")]
        [HttpGet]
        public ActionResult Delete(Guid UserID)
        {
            var existingUser = _airWazeDatabase.FindUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();
            var deleteViewModel = new UserDeleteViewModel
            {
                UserID = existingUser.UserID,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
            };
            return View(deleteViewModel);

            //var user = userEntities.FirstOrDefault(x => x.UserID == UserID);
            //UserDeleteViewModel userDeleteViewModel = new UserDeleteViewModel
            //{
            //    UserID = user.UserID,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //};
            //return View(userDeleteViewModel);
        }

        //only user + admin
        [Route("User/Delete/{UserID:Guid}")]
        [HttpPost]
        public ActionResult ConfirmDelete(Guid UserID)
        {
            var existingUser = _airWazeDatabase.FindUserByID(UserID);
            if (existingUser == null) return new NotFoundResult();
            _airWazeDatabase.RemoveUser(existingUser);
            return RedirectToAction("Index");

            //var user = userEntities.FirstOrDefault(x => x.UserID == UserID);
            //userEntities.Remove(user);
            ////await _myDatabase.RemoveUser(user);
            //return RedirectToAction("Index");
        }
    }
}
