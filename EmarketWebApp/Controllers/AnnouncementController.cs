using Microsoft.AspNetCore.Mvc;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Announcements;
using WebApp.Emarket.Middlewares;

namespace EmarketWebApp.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _annocementService;
        private readonly ICategoryService _categoryService;
        private readonly ValidateUserSession _validateUserSession;

        public AnnouncementController(IAnnouncementService announcementService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _annocementService = announcementService;
            _categoryService = categoryService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            return View(await _annocementService.GetAllViewModel());
        }

        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            SaveAnnouncementViewModel vm = new();
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveAnnouncement", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAnnouncementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            if (vm.Id == 0)
            {
                vm.ImageUrl = UploadFile(vm.File, vm.Id);

                SaveAnnouncementViewModel productVm = await _annocementService.Add(vm);
            }
            else
            {
                vm.ImageUrl = UploadFile(vm.File, vm.Id);

                await _annocementService.Update(vm);
            }

            return RedirectToRoute(new { controller = "Announcement", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAnnouncementViewModel vm = await _annocementService.GetByIdSaveViewModel(id);
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveAnnouncement", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveAnnouncementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAnnouncementViewModel productVm = await _annocementService.GetByIdSaveViewModel(vm.Id);

            vm.ImageUrl = UploadFile(vm.File, vm.Id, true, productVm.ImageUrl);

            productVm.Price = vm.Price;
            productVm.CategoryId = vm.CategoryId;
            productVm.Description = vm.Description;
            productVm.File = vm.File;
            productVm.ImageUrl = vm.ImageUrl;

            await _annocementService.Update(productVm);
            
            return RedirectToRoute(new { controller = "Announcement", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            return View(await _annocementService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            await _annocementService.Delete(id);

            string basePath = $"/Images/Announcements/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "Announcement", action = "Index" });
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Announcements/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }
    }
}
