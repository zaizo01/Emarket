using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Announcements;
using Emarket.Core.Application.ViewModels.User;
using Emarket.Infrastucture.Persistence.Contexts;
using EmarketWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, IAnnouncementService announcementService, ICategoryService categoryService, ValidateUserSession validateUserSession, ApplicationDbContext dbContext)
        {
            _annocementService = announcementService;
            _categoryService = categoryService;
            _logger = logger;
            _validateUserSession = validateUserSession;
            this.dbContext = dbContext;
        }
        
        public async Task<IActionResult> Index(string? searchString, string[] categories)
        {
       
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            var records = await _annocementService.GetAllViewModelHome(searchString, categories);
            var categoties = await _categoryService.GetAllViewModel();
           
            return View(new MainAnnouncement
            {
                Announcements = records,
                Categories = categoties
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            AnnouncementeDetails vm = new();
            var announcement = await dbContext.Announcements
                                    .Include(x => x.User)
                                    .Include(x => x.Category)
                                    .FirstOrDefaultAsync(x => x.Id == id);
                             
            vm.Name = announcement.Name;
            vm.Description = announcement.Description;
            vm.Id = announcement.Id;
            vm.Price = announcement.Price;
            vm.ImageUrl = announcement.ImageUrl;
            vm.CategoryName = announcement.Category.Name;
            vm.CategoryId = announcement.Category.Id;
            vm.UserId = announcement.UserId;
            vm.AnnouncementName = announcement.User.Name;
            vm.AnnouncementEmail = announcement.User.Email;
            vm.AnnouncementPhone = announcement.User.Phone;
            vm.AnnouncementCreateDate = announcement.Created.Value;
            if (vm is null) return NotFound();
            return View(vm);
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