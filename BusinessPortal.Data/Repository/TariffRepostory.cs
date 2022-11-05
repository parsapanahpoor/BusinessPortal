using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Advertisement;
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
            return await _context.Tariffs.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == tarriffId);
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

        #region Site Side 

        //Has User Any Tariff Right Now 
        public async Task<bool> HasUserAnyActiveTariffRightNow(ulong userId)
        {
            return await _context.UserSelectedTariff.AnyAsync(p => !p.IsDelete && p.UserId == userId &&
                                                              p.Startdate.Year == DateTime.Now.Year && p.Startdate.DayOfYear <= DateTime.Now.DayOfYear &&
                                                              p.EndDate.Year == DateTime.Now.Year && p.EndDate.DayOfYear >= DateTime.Now.DayOfYear);
        }

        //Add User Selected Tariff
        public async Task CreateUserSelectedTariff(UserSelectedTariff user)
        {
            await _context.UserSelectedTariff.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        //Get Count Of User Seen Ads Today 
        public async Task<int> GetCountOfUserSeenAdsToday(ulong userId)
        {
            return await _context.UserSeenAdvertisementLogs.CountAsync(p => !p.IsDelete && p.UserId == userId
                                                                       && p.CreateDate.Year == DateTime.Now.Year
                                                                       && p.CreateDate.Month == DateTime.Now.Month
                                                                       && p.CreateDate.Day == DateTime.Now.Day);
        }

        //Get Count Of User Seen Ads  
        public async Task<int> GetCountOfUserSeenAds(ulong userId)
        {
            return await _context.UserSeenAdvertisementLogs.CountAsync(p => !p.IsDelete && p.UserId == userId);
        }

        //Add User Seen Advertisement Log To Data Base 
        public async Task CreateUserAdvertisementLog(UserSeenAdvertisementLog log)
        {
            await _context.UserSeenAdvertisementLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        //Get User Selected Tariff By User Id 
        public async Task<Tariff?> GetUserSelectedTariffByUserId(ulong userId)
        {
            var userSelectedTariff = await _context.UserSelectedTariff.Include(p => p.Tariff)
                                      .Where(p => !p.IsDelete
                                          && ((p.Startdate.Year <= DateTime.Now.Year && p.EndDate.Year > DateTime.Now.Year)
                                          || (p.Startdate.Year == DateTime.Now.Year
                                               && p.Startdate.DayOfYear <= DateTime.Now.DayOfYear
                                               && p.EndDate.Year == DateTime.Now.Year
                                               && p.EndDate.DayOfYear >= DateTime.Now.DayOfYear))).ToListAsync();

            var returnModel = userSelectedTariff.FirstOrDefault(p => p.UserId == userId);

            if (returnModel == null) return null;

            return returnModel.Tariff;
        }

        //Get User Selected Tariff By User Id 
        public async Task<UserSelectedTariff?> GetJustUserSelectedTariffByUserId(ulong userId)
        {
            var userSelectedTariff = await _context.UserSelectedTariff.Include(p => p.Tariff)
                                      .Where(p => !p.IsDelete
                                           && ((p.Startdate.Year <= DateTime.Now.Year && p.EndDate.Year > DateTime.Now.Year)
                                          || (p.Startdate.Year == DateTime.Now.Year
                                               && p.Startdate.DayOfYear <= DateTime.Now.DayOfYear
                                               && p.EndDate.Year == DateTime.Now.Year
                                               && p.EndDate.DayOfYear >= DateTime.Now.DayOfYear))).ToListAsync();

            return userSelectedTariff.FirstOrDefault(p => p.UserId == userId);

        }

        //Get User Active Tariff 
        public async Task<Tariff?> GetUserActiveTariff(ulong userId)
        {
            var userSelectedTariff = await _context.UserSelectedTariff.Include(p => p.Tariff)
                                      .Where(p => !p.IsDelete
                                          && ((p.Startdate.Year <= DateTime.Now.Year && p.EndDate.Year > DateTime.Now.Year)
                                          || (p.Startdate.Year == DateTime.Now.Year
                                               && p.Startdate.DayOfYear <= DateTime.Now.DayOfYear
                                               && p.EndDate.Year == DateTime.Now.Year
                                               && p.EndDate.DayOfYear >= DateTime.Now.DayOfYear))).ToListAsync();

            var returnModel = userSelectedTariff.FirstOrDefault(p=> p.UserId == userId);

            if (returnModel == null) return null;

            return returnModel.Tariff;
        }

        //Get Count Of Create Customer Ads Today 
        public async Task<int> GetCountOfCreateCustomerAdsToday(ulong userId)
        {
            return await _context.UserCreateAdvertisementLogs
                .CountAsync(p => !p.IsDelete && p.UserId == userId && p.FromCustomer && !p.FromEmployee
                                                                       && p.CreateDate.Year == DateTime.Now.Year
                                                                       && p.CreateDate.Month == DateTime.Now.Month
                                                                       && p.CreateDate.Day == DateTime.Now.Day);
        }

        //Ads User Create Customer Ads Log Today 
        public async Task UserCreateCustomerAdsLog(UserCreateAdvertisementLog log)
        {
            await _context.UserCreateAdvertisementLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        //Get Count Of Create Sale Ads Today 
        public async Task<int> GetCountOfCreateSaleAdsToday(ulong userId)
        {
            return await _context.UserCreateAdvertisementLogs
                .CountAsync(p => !p.IsDelete && p.UserId == userId && !p.FromCustomer && p.FromEmployee
                                                                       && p.CreateDate.Year == DateTime.Now.Year
                                                                       && p.CreateDate.Month == DateTime.Now.Month
                                                                       && p.CreateDate.Day == DateTime.Now.Day);
        }


        #endregion
    }
}
