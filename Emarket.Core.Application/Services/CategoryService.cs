using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Categories;
using Emarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Add(SaveCategoryViewModel vm)
        {
            Category category = new();
            category.Name = vm.Name;
            category.Description = vm.Description;

            await _categoryRepository.AddAsync(category);
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<List<CategoryViewModel>> GetAllViewModel()
        {
            var categoryList = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Products" });

            return categoryList.Select(category => new CategoryViewModel
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                AnnouncementQuantity = category.Announcements.Count()
            }).ToList();
        }

        public async Task<SaveCategoryViewModel> GetByIdSaveViewModel(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            SaveCategoryViewModel vm = new();
            vm.Id = category.Id;
            vm.Name = category.Name;
            vm.Description = category.Description;

            return vm;
        }

        public async Task Update(SaveCategoryViewModel vm)
        {
            Category category = new();
            category.Id = vm.Id;
            category.Name = vm.Name;
            category.Description = vm.Description;

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
