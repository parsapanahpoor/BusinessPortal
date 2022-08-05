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

        #endregion
    }
}
