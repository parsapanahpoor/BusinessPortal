using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Data.Repository;
using BusinessPortal.Domain.Interfaces;
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
            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IAdvertisementService, AdvertisementService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITariffService, TariffService>();
            services.AddScoped<ITicketService , TicketService>();

            #endregion

            #region Repository

            services.AddScoped<IWalletRepository , WalletRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITariffRepostory , TariffRepostory>();
            services.AddScoped<ITicketRepository , TicektRepository>();

            #endregion
        }
    }
}
