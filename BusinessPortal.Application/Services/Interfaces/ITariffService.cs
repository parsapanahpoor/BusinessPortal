using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface ITariffService 
    {
        #region Admin Side

        //Crete Tariff
        Task<bool> CreateTariff(CreateTariffViewModel model);

        //Get Tariff By Tariff Id 
        Task<Tariff?> GetTariffById(ulong tarriffId);

        //Edit Tariff 
        Task<bool> EditTariff(Tariff model);

        //Delete Tariff
        Task<bool> DeleteTariff(ulong tariffId);

        //Filter Tariff In Admin Side 
        Task<FilterTariffViewModel> FilterTariff(FilterTariffViewModel filter);

        //Show Tariffs In Home Page 
        Task<List<Tariff>> ShowTariffInHomePage();

        //Buy Tariff By User 
        Task<int> BuyTariff(ulong tariffId, ulong userId);

        #endregion

        #region Site Side 

        //Check to see ads based on tariffs
        Task<bool> CheckUserSeeAdsBaseOnTariff(ulong userId);

        #endregion

        #region User Panel 

        //Check User Create Custmer Ads Log 
        Task<bool> CheckCustomerAdsBaseOnTariff(ulong userId);

        //Check User Create Sale Ads Log 
        Task<bool> CheckSaleAdsBaseOnTariff(ulong userId);

        #endregion
    }
}
