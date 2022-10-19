using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<ICategoryService, CategoryService>();
            
        }
    }
}
