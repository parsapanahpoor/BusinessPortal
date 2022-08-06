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
    }
}
