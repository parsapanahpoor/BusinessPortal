using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class TariffService : ITariffService
    {
        #region ctor

        private readonly ITariffRepostory _tariff;

        public TariffService(ITariffRepostory tariff)
        {
            _tariff = tariff;
        }

        #endregion

        #region Admin Side 

        //Crete Tariff
        public async Task<bool> CreateTariff(CreateTariffViewModel model)
        {
            #region Fill Entity 

            Tariff tariff = new Tariff()
            {
                tariffDuration = model.TariffDuration,
                TariffName = model.TariffName,
                TariffPrice = model.TariffPrice,
                CountOfAddAdvertisement = model.CountOfAddAdvertisement,
                CountOfSeenAdvertisement = model.CountOfSeenAdvertisement,
            };

            #endregion

            #region Add Method

            await _tariff.CreateTariff(tariff);

            #endregion

            return true;
        }

        //Get Tariff By Tariff Id 
        public async Task<Tariff?> GetTariffById(ulong tarriffId)
        {
            return await _tariff.GetTariffById(tarriffId);
        }

        //Edit Tariff 
        public async Task<bool> EditTariff(Tariff model)
        {
            #region Get Tariff By Tariff Id 

            var tariff = await _tariff.GetTariffById(model.Id);
            if (tariff == null) return false;

            #endregion

            #region Edit Properties

            tariff.TariffPrice = model.TariffPrice;
            tariff.TariffName = model.TariffName;
            tariff.tariffDuration = model.tariffDuration;
            tariff.CountOfAddAdvertisement = model.CountOfAddAdvertisement;
            tariff.CountOfSeenAdvertisement = model.CountOfSeenAdvertisement;

            #endregion

            #region Edit Method 

            await _tariff.EditTariff(tariff);

            #endregion

            return true;
        }

        //Delete Tariff
        public async Task<bool> DeleteTariff(ulong tariffId)
        {
            #region Get Tariff By Tariff Id 

            var tariff = await _tariff.GetTariffById(tariffId);
            if (tariff == null) return false;

            #endregion

            #region Edit Properties

            tariff.IsDelete = tariff.IsDelete;

            #endregion

            #region Delete Method

            await _tariff.DeleteTariff(tariff);

            #endregion

            return true;
        }

        //Filter Tariff In Admin Side 
        public async Task<FilterTariffViewModel> FilterTariff(FilterTariffViewModel filter)
        {
            return await _tariff.FilterTariff(filter);
        }

        #endregion
    }
}
