using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Interfaces
{
    public interface ITariffRepostory 
    {
        #region Admin Side 

        //Show Tariffs In Home Page 
        Task<List<Tariff>> ShowTariffInHomePage();

        //Add Tariff
        Task CreateTariff(Tariff tariff);

        //Get Tariff By Tariff Id 
        Task<Tariff?> GetTariffById(ulong tarriffId);

        //Edit Tariff Method 
        Task EditTariff(Tariff tariff);

        //Delete Tariff
        Task DeleteTariff(Tariff tariff);

        //Filter Tariff In Admin Side 
        Task<FilterTariffViewModel> FilterTariff(FilterTariffViewModel filter);

        //Has User Any Tariff Right Now 
        Task<bool> HasUserAnyActiveTariffRightNow( ulong userId);

        //Add User Selected Tariff
        Task CreateUserSelectedTariff(UserSelectedTariff user);

        //Get Count Of User Seen Ads Today 
        Task<int> GetCountOfUserSeenAdsToday(ulong userId);

        //Add User Seen Advertisement Log To Data Base 
        Task CreateUserAdvertisementLog(UserSeenAdvertisementLog log);

        //Get User Active Tariff 
        Task<Tariff?> GetUserActiveTariff(ulong userId);

        #endregion

        #region User Pnael

        //Get Count Of Create Customer Ads Today 
        Task<int> GetCountOfCreateCustomerAdsToday(ulong userId);

        //Ads User Create Customer Ads Log Today 
        Task UserCreateCustomerAdsLog(UserCreateAdvertisementLog log);

        //Get Count Of Create Sale Ads Today 
        Task<int> GetCountOfCreateSaleAdsToday(ulong userId);

        #endregion
    }
}
