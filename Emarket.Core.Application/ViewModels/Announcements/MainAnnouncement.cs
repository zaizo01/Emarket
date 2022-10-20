using Emarket.Core.Application.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.Announcements
{
    public class MainAnnouncement
    {
        public List<AnnouncementViewModel> Announcements { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
