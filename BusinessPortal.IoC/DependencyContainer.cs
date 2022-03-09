using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService , UserService >();
            services.AddScoped<ISiteSettingService , SiteSettingService >();
            services.AddScoped<IViewRenderService , ViewRenderService >();
            services.AddScoped<IEmailSender , EmailSender >();
            services.AddScoped<IPermissionService , PermissionService >();
            services.AddScoped<ICategoriesService , CategoriesService >();
        }
    }
}
