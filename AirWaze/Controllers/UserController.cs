using AirWaze.Entities;
using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class UserController : Controller
    {
        private static readonly List<User> userEntities = new List<User>()
        {
            new User
            {
                    UserID = Guid.NewGuid(),
                    FirstName = "Toinon",
                    LastName = "Naesen",
                    Email = "toinon.naesen@gmail.com",
                    StreetName = "Zeelaan",
                    HouseNumber = "2",
                    Zipcode = "8450",
                    City = "Bredene",
                    Country = "Belgium",
                    PhoneNumber = "0472/43.95.43"
             }
        };

        [HttpGet]
        public IActionResult Index()
        {
            List<UserListViewModel> userList = new List<UserListViewModel>();
            foreach (var user in userEntities)
            {
                userList.Add(new UserListViewModel()
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                });
            }
            return View(userList);
        }

        [Route("User/Detail/{UserID:Guid}")]
        [HttpGet]
        public IActionResult Detail(Guid UserID)
        {
            UserDetailViewModel newUser = new UserDetailViewModel();
            foreach (User user in userEntities)
            {
                if (user.UserID == UserID)
                {
                    newUser.FirstName = user.FirstName;
                    newUser.LastName = user.LastName;
                    newUser.Email = user.Email;
                    newUser.StreetName = user.StreetName;
                    newUser.HouseNumber = user.HouseNumber;
                    newUser.Zipcode = user.Zipcode;
                    newUser.Bus = user.Bus;
                    newUser.City = user.City;
                    newUser.Country = user.Country;
                    newUser.PhoneNumber = user.PhoneNumber;
                    break;
                }
            }
            return View(newUser);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userCreateViewModel = new UserCreateViewModel();
            return View(userCreateViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(UserCreateViewModel newUser)
        {
            var isModelValid = TryValidateModel(newUser);


            if (isModelValid)
            {
                userEntities.Add(new User()
                {
                    UserID = Guid.NewGuid(),
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    StreetName = newUser.StreetName,
                    HouseNumber = newUser.HouseNumber,
                    Bus = newUser.Bus,
                    Zipcode = newUser.Zipcode,
                    Country = newUser.Country,
                    City = newUser.City,
                    PhoneNumber  = newUser.PhoneNumber,
                });
                return RedirectToAction("Index");
            }
            return View(newUser);
        }
    }
}
