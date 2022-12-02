using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Countries;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Countries;
using BusinessPortal.Domain.ViewModels.Admin.Dashboard;
using BusinessPortal.Domain.ViewModels.Advertisement;
using BusinessPortal.Domain.ViewModels.Site.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        //List Of Customer Advertisements
        Task<List<ListOfCustomerAdvertisementViewModel>> ListOfCustomerAdvertisementViewModel(string culture , ulong? categoryId);

        Task<List<LastestCustomersAdvertisements>> GetLastestAdvertisementFromCustomers(string culture);

        Task<List<LastestEmployeesAdvertisements>> GetLastestAdvertisementFromEmployees(string culture);

        //Filter Sale Advertisement Site Side 
        Task<FilterSaleAdvertisementViewModel> FilterSaleAdvertisementViewModel(FilterSaleAdvertisementViewModel filter);

        //List Of Employee Advertisements
        Task<List<ListOfSaleAdvertisementViewModel>> ListOfSaleAdvertisementViewModel(string culture, ulong? categoryId);

        #endregion

        #region Admin Side 

        Task<AdvertisementInfo?> ShowAdvertisementLanguage(ulong adsId);

        Task<FilterAdvertisementAdminSidedViewModel> FilterRequestAdvertisementAdminSide(FilterAdvertisementAdminSidedViewModel filter);

        Task<AdminDashboardViewModel> FillAdminDashboadrdViewModel();

        Task<EditAdvertisementFromAdminPanel> SetEditAdvertisementFromAdminPanel(ulong Id);

        Task<EditAdvertisementFromAdminPanelResult> UpdateAdvertisement(EditAdvertisementFromAdminPanel model, IFormFile? ImageName, List<ulong> SelectedCategory);

        Task<bool> DeleteAdvertisementFromAdmin(ulong id);

        //Active Our Offer 
        Task<bool> ActiveOurOffer(ulong id);

        //Diss Active Our Offer 
        Task<bool> DisActiveOurOffer(ulong id);

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

        #region Countries

        //Filter Countries ViewModel Admin Side 
        Task<FilterCountriesViewModel> FilterCountries(FilterCountriesViewModel filter);

        //Create Country Admin Side 
        Task<bool> CreateCountryAdminSide(string uniqueName, IFormFile flag);

        //Get Country 
        Task<Countries?> GetCountryById(ulong countryId);

        //Edit Country
        Task<bool> EditCountry(ulong countryId, string uniqueName, IFormFile? flag);

        //Delete Country By Id 
        Task<bool> DeleteCountry(ulong countryId);

        //List Of Countries For Drowp Down
        List<SelectListItem> ListOfCountriesForDrowpDown();

        #endregion
    }
}
