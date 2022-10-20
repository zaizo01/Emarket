using Microsoft.AspNetCore.Mvc;
using Emarket.Core.Application.ViewModels.User;
using System.Threading.Tasks;
using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Services;
using WebApp.Emarket.Middlewares;

namespace WebApp.Emarket.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUserService userService,ValidateUserSession validateUserSession)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
               return View(vm);
            }
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            UserViewModel userVm = await _userService.Login(vm);
            if (userVm != null)
            {
                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userValidation", "Datos de acceso incorrecto");
            }

            return View(vm);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var users = await _userService.GetAllViewModel();

            var userExist = users.Find(x => x.Username == vm.Username);
            
            if (userExist is null)
            {
                if (_validateUserSession.HasUser())
                {
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }

                await _userService.Add(vm);
                return RedirectToRoute(new { controller = "User", action = "Index" });
            } else
            {

                ModelState.AddModelError("userValidation", "Este nombre de usuario esta en uso");
            }
            return View(vm);

        }
    }
}
