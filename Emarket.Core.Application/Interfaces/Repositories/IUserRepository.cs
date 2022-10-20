using Emarket.Core.Application.ViewModels.User;
using Emarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepositoryAsync<User>
    {
        Task<User> LoginAsync(LoginViewModel loginVm);
    }
   
}
