using Microsoft.EntityFrameworkCore;
using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.ViewModels.User;
using Emarket.Core.Domain.Entities;
using System.Threading.Tasks;
using Emarket.Infrastucture.Persistence.Contexts;
using Emarket.Infrastucture.Persistence.Repositories;

namespace Emarket.Infrastructure.Persistence.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<User> AddAsync(User entity)
        {
            entity.Password = PasswordEncryptation.ComputeSha256Hash(entity.Password);
            await base.AddAsync(entity);
            return entity;
        }

        public async Task<User> LoginAsync(LoginViewModel loginVm)
        {
            string passwordEncrypt = PasswordEncryptation.ComputeSha256Hash(loginVm.Password);
            User user = await _dbContext.Set<User>().FirstOrDefaultAsync(user=> user.Username == loginVm.Username && user.Password == passwordEncrypt);
            return user;
        }

    }
}
