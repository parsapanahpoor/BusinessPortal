using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Data.Repository
{
    public class TariffRepostory : ITariffRepostory
    {
        #region Ctor 

        private readonly BusinessPortalDbContext _context;

        public TariffRepostory(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        //Show Tariffs In Home Page 
        public async Task<List<Tariff>> ShowTariffInHomePage()
        {
            return await _context.Tariffs.Where(p => !p.IsDelete).ToListAsync();
        }

        //Add Tariff
        public async Task CreateTariff(Tariff tariff)
        {
            await _context.Tariffs.AddAsync(tariff);
            await _context.SaveChangesAsync();
        }

        //Get Tariff By Tariff Id 
        public async Task<Tariff?> GetTariffById(ulong tarriffId)
        {
            return await _context.Tariffs.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == tarriffId);
        }

        //Edit Tariff Method 
        public async Task EditTariff(Tariff tariff)
        {
            _context.Tariffs.Update(tariff);
            await _context.SaveChangesAsync();
        }

        //Delete Tariff
        public async Task DeleteTariff(Tariff tariff)
        {
            _context.Tariffs.Remove(tariff);
            await _context.SaveChangesAsync();
        }

        //Filter Tariff In Admin Side 
        public async Task<FilterTariffViewModel> FilterTariff(FilterTariffViewModel filter)
        {
            var query = _context.Tariffs
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.TariffName))
            {
                query = query.Where(s => EF.Functions.Like(s.TariffName, $"%{filter.TariffName}%"));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        #endregion
    }
}
