using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Announcements;
using Emarket.Core.Application.ViewModels.User;
using EmarketWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Emarket.Middlewares;

namespace EmarketWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnnouncementService _annocementService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<HomeController> _logger;
        private readonly ValidateUserSession _validateUserSession;


        public HomeController(ILogger<HomeController> logger, IAnnouncementService announcementService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _annocementService = announcementService;
            _categoryService = categoryService;
            _logger = logger;
            _validateUserSession = validateUserSession;

        }

        public async Task<IActionResult> Index()
        {
       
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            UserViewModel userViewModel = HttpContext.Session.Get<UserViewModel>("user");
            var records = await _annocementService.GetAllViewModel();
            var result = records.Where(x => x.UserId != userViewModel.Id);

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {

            SaveAnnouncementViewModel vm = await _annocementService.GetByIdSaveViewModel(id);
            if(vm is null) return NotFound();
            return View("Details", vm);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}