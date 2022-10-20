using Emarket.Core.Application.ViewModels.Announcements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Services
{
    public interface IAnnouncementService: IGenericService<SaveAnnouncementViewModel, AnnouncementViewModel>
    {
        Task<List<AnnouncementViewModel>> GetAllViewModelHome(string? searchString, string[] categories);
    }
}
