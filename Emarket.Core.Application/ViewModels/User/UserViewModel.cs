using Emarket.Core.Application.ViewModels.Announcements;
using Emarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
 
        public string Phone { get; set; }

        public ICollection<Announcement> Announcement { get; set; }
    }
}
