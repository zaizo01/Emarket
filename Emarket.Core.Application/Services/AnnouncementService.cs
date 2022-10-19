using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Announcements;
using Emarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Services
{
    public class AnnouncementService: IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task Add(SaveAnnouncementViewModel vm)
        {
            Announcement announcement = new();
            announcement.Name = vm.Name;
            announcement.Price = vm.Price;
            announcement.ImageUrl = vm.ImageUrl;
            announcement.Description = vm.Description;
            announcement.CategoryId = vm.CategoryId;

            await _announcementRepository.AddAsync(announcement);
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
                CategoryId = announcement.Category.Id
            }).ToList();
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
