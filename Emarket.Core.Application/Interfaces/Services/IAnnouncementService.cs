using Emarket.Core.Application.ViewModels.Announcements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Services
{
    internal interface IAnnouncementService: IGenericService<SaveAnnouncementViewModel, AnnouncementViewModel>
    {
    }
}
