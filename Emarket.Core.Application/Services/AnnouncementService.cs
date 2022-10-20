using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Announcements;
using Emarket.Core.Application.ViewModels.User;
using Emarket.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;


namespace Emarket.Core.Application.Services
{
    public class AnnouncementService: IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        public AnnouncementService(IAnnouncementRepository announcementRepository, IHttpContextAccessor httpContextAccessor)
        {
            _announcementRepository = announcementRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<SaveAnnouncementViewModel> Add(SaveAnnouncementViewModel vm)
        {
           

            Announcement announcement = new();
            announcement.Name = vm.Name;
            announcement.Price = vm.Price;
            announcement.ImageUrl = vm.ImageUrl;
            announcement.Description = vm.Description;
            announcement.CategoryId = vm.CategoryId;
            announcement.UserId = userViewModel.Id;

            await _announcementRepository.AddAsync(announcement);

            SaveAnnouncementViewModel announcementVm = new();
            announcementVm.Id = announcement.Id;
            announcementVm.Name = announcement.Name;
            announcementVm.Price = announcement.Price;
            announcementVm.ImageUrl = announcement.ImageUrl;
            announcementVm.Description = announcement.Description;
            announcementVm.CategoryId = announcement.CategoryId;

            return announcementVm;
        }

        public async Task Delete(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            await _announcementRepository.DeleteAsync(announcement);
        }

        public async Task<List<AnnouncementViewModel>> GetAllViewModel()
        {
            var announcementList = await _announcementRepository.GetAllWithIncludeAsync(new List<string> { "Category" });
            
            return announcementList.Select(announcement => new AnnouncementViewModel
            {
                Name = announcement.Name,
                Description = announcement.Description,
                Id = announcement.Id,
                Price = announcement.Price,
                ImageUrl = announcement.ImageUrl,
                CategoryName = announcement.Category.Name,
                CategoryId = announcement.Category.Id,
                UserId = announcement.UserId
            }).Where(x => x.UserId == userViewModel.Id).ToList();
        }

        public async Task<List<AnnouncementViewModel>> GetAllViewModelHome(string searchString  = "", string[] categories = null)
        {
            searchString += "";
            searchString = searchString.ToLower();

            var announcementList = await _announcementRepository.GetAllWithIncludeAsync(new List<string> { "Category" });

            
            
            if (!string.IsNullOrEmpty(searchString))
            {
                var de = announcementList.Select(announcement => new AnnouncementViewModel
                {
                    Name = announcement.Name,
                    Description = announcement.Description,
                    Id = announcement.Id,
                    Price = announcement.Price,
                    ImageUrl = announcement.ImageUrl,
                    CategoryName = announcement.Category.Name,
                    CategoryId = announcement.Category.Id,
                    UserId = announcement.UserId
                }).Where(x => x.UserId != userViewModel.Id && x.Name.ToLower().Contains(searchString));

                if (categories.Length != 0)
                {
                    List<AnnouncementViewModel> list = new();

                    foreach (var catrgoryId in categories)
                    {
                        list.AddRange(announcementList.Select(announcement => new AnnouncementViewModel
                        {
                            Name = announcement.Name,
                            Description = announcement.Description,
                            Id = announcement.Id,
                            Price = announcement.Price,
                            ImageUrl = announcement.ImageUrl,
                            CategoryName = announcement.Category.Name,
                            CategoryId = announcement.Category.Id,
                            UserId = announcement.UserId
                        }).Where(x => x.UserId != userViewModel.Id && x.CategoryId.Equals(Convert.ToInt32(catrgoryId))).ToList());
                    }

                    return list;
                }

                return de.ToList();
            }

            return announcementList.Select(announcement => new AnnouncementViewModel
            {
                Name = announcement.Name,
                Description = announcement.Description,
                Id = announcement.Id,
                Price = announcement.Price,
                ImageUrl = announcement.ImageUrl,
                CategoryName = announcement.Category.Name,
                CategoryId = announcement.Category.Id,
                UserId = announcement.UserId
            }).Where(x => x.UserId != userViewModel.Id ).ToList();
        }

        public async Task<SaveAnnouncementViewModel> GetByIdSaveViewModel(int id)
        {
           
            var announcement = await _announcementRepository.GetByIdAsync(id);

            SaveAnnouncementViewModel vm = new();
            vm.Id = announcement.Id;
            vm.Name = announcement.Name;
            vm.Description = announcement.Description;
            vm.CategoryId = announcement.CategoryId;
            vm.Price = announcement.Price;
            vm.ImageUrl = announcement.ImageUrl;

            return vm;
        }

        public async Task Update(SaveAnnouncementViewModel vm)
        {
            Announcement announcement = new();
            announcement.Id = vm.Id;
            announcement.Name = vm.Name;
            announcement.Description = vm.Description;
            announcement.Price = vm.Price;
            announcement.ImageUrl = vm.ImageUrl;
            announcement.CategoryId = vm.CategoryId;
           
            await _announcementRepository.UpdateAsync(announcement);
        }
    }
}
