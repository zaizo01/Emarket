using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Domain.Entities;
using Emarket.Infrastucture.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Infrastucture.Persistence.Repositories
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AnnouncementRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
