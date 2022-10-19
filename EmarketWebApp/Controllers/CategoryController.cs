using Emarket.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Categories;

namespace EmarketWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
          

            return View(await _categoryService.GetAllViewModel());
        }

        public IActionResult Create()
        {
           

            return View("SaveCategory", new SaveCategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCategoryViewModel vm)
        {
          

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            await _categoryService.Add(vm);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
          

            return View("SaveCategory", await _categoryService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCategoryViewModel vm)
        {
           

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            await _categoryService.Update(vm);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
           

            return View(await _categoryService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
           

            await _categoryService.Delete(id);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }
    }
}

