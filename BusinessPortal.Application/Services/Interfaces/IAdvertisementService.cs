using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Dashboard;
using BusinessPortal.Domain.ViewModels.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IAdvertisementService
    {
        #region Site Side

        Task<List<LastestCustomersAdvertisements>> GetLastestAdvertisementFromCustomers();
        Task<List<LastestEmployeesAdvertisements>> GetLastestAdvertisementFromEmployees();

        #endregion

        #region Admin Side 

        Task<AdvertisementInfo?> ShowAdvertisementLanguage(ulong adsId);
        Task<FilterAdvertisementAdminSidedViewModel> FilterRequestAdvertisementAdminSide(FilterAdvertisementAdminSidedViewModel filter);
        Task<AdminDashboardViewModel> FillAdminDashboadrdViewModel();
        Task<EditAdvertisementFromAdminPanel> SetEditAdvertisementFromAdminPanel(ulong Id);
        Task<EditAdvertisementFromAdminPanelResult> UpdateAdvertisement(EditAdvertisementFromAdminPanel model, IFormFile? ImageName, List<ulong> SelectedCategory);
        Task<bool> DeleteAdvertisementFromAdmin(ulong id);

        #endregion

        #region Request Advertisement

        Task<FilterRequestAdvertisementViewModel> FilterRequestAdvertisementUserSide(FilterRequestAdvertisementViewModel filter);
        Task<FilterOnSaleAdvertisementViewModel> FilterOnSaleAdvertisementUserSide(FilterOnSaleAdvertisementViewModel filter);
        Task<CreateAdvertisementFromUserPanelResult> AddOnSaleAdvertisementFromUserPanell(CreateOnSaleAdvertisementFromUserPanel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);
        Task<CreateAdvertisementFromUserPanelResult> AddAdvertisementFromUserPanell(CreateRequestAdvertisementFromUserPanel model, List<IFormFile> upload_imgs , List<ulong> SelectedCategory);
        Task<EditRequestAdvertisementFromUserPanel> SetEditAdvertisementFromUserPanel(ulong Id);
        Task<EditOnSaleAdvertisementFromUserPanel> SetEditOnSaleAdvertisementFromUserPanel(ulong Id);
        Task<EditOnSaleAdvertisementFromUserPanelResualt> EditOnSaleAdvertisementFromUserPanel(EditOnSaleAdvertisementFromUserPanel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);
        Task<List<ulong>> GetAllAdvertisementCategories(ulong Id);
        Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId);
        Task<Domain.Entities.Advertisement.Advertisement?> GetAdvertisementByID(ulong Id);
        Task<EditRequestAdvertisementFromUserPanelResualt> EditRequestAdvertisementFromUserPanel(EditRequestAdvertisementFromUserPanel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);
        Task<bool> DeleteAdvertisementFromUserPanel(ulong advertisement , ulong userId);

        #endregion

    }
}
