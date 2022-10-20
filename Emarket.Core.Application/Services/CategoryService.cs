using Dapper;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Categories;
using Emarket.Core.Domain.Entities;
using Microsoft.Data.SqlClient;
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

        public async Task<SaveCategoryViewModel> Add(SaveCategoryViewModel vm)
        {
            Category category = new();
            category.Name = vm.Name;
            category.Description = vm.Description;

            category = await _categoryRepository.AddAsync(category);

            SaveCategoryViewModel categoryVm = new();

            categoryVm.Id = category.Id;
            categoryVm.Name = category.Name;
            categoryVm.Description = category.Description;

            return categoryVm;
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<List<CategoryViewModel>> GetAllViewModel()
        {
            var cs = "Server=.;Database=DBEmarket;Trusted_Connection=true;MultipleActiveResultSets=True";
            using var con = new SqlConnection(cs);
            con.Open();

            var query = @"SELECT Categories.Name,Categories.Description,Categories.Id, COUNT(U.Id) CountAnuncios,count(distinct UserId) as countUser
                            FROM Categories
                            left join Announcements on Categories.Id = Announcements.CategoryId
                            left join Users U       ON U.Id = Announcements.UserId
                            group by Categories.Name,Categories.Description,Categories.Id";

            var categories  = await con.QueryAsync<QueryCategory>(query);

            List<CategoryViewModel> CategoryViewModels = new List<CategoryViewModel>();

            foreach (var item in categories)
            {
                CategoryViewModels.Add(new CategoryViewModel {
                    Id = item.Id,
                    Name = item.Name, 
                    AnnouncementQuantity = item.CountAnuncios,
                    UserQuantity = item.countUser, 
                    Description =  item.Description });
            }

            return CategoryViewModels;
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
