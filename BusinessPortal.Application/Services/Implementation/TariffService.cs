using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using BusinessPortal.Domain.ViewModels.Admin.Wallet;
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

        private IUserService _userService;

        private readonly IWalletService _walletService;

        public TariffService(ITariffRepostory tariff , IUserService userService , IWalletService walletService)
        {
            _tariff = tariff;
            _userService = userService;
            _walletService = walletService;
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

        //Show Tariffs In Home Page 
        public async Task<List<Tariff>> ShowTariffInHomePage()
        {
            return await _tariff.ShowTariffInHomePage();
        }

        #endregion

        #region Site Side 

        //Buy Tariff By User 
        public async Task<int> BuyTariff(ulong tariffId , ulong userId)
        {
            //1. Account Balance in not enough
            //2. You Have Tariff Right Now
            //3. false
            //4. Success

            #region Get Tariff By Id 

            var tariff = await GetTariffById(tariffId);
            if (tariff == null) return 3;

            #endregion

            #region Get User By User Id

            var user = await _userService.GetUserById(userId);
            if (user == null) return 3;

            #endregion

            #region Check User Has Any Tariff Right Now

            if (await _tariff.HasUserAnyActiveTariffRightNow(userId)) return 2;

            #endregion

            #region Get User Account Balance 

            var accountBalance = user.WalletBalance;

            #endregion

            #region Is User Account Balance Has Enough Money 

            if (accountBalance < tariff.TariffPrice) return 1;

            #endregion

            #region Buy Tariff 

            #region Add User Selected Tariff

            //Fill Model 
            UserSelectedTariff selectedTariff = new UserSelectedTariff()
            {
                CreateDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(tariff.tariffDuration),
                Startdate = DateTime.Now,
                IsDelete = false,
                TariffId = tariffId,
                UserId = userId
            };

            //Add Model Into Data Base 
            await _tariff.CreateUserSelectedTariff(selectedTariff);

            #endregion

            #region Update User Wallet Balance

            AdminCreateWalletViewModel wallet = new AdminCreateWalletViewModel()
            {
                Description = $"Buy {tariff.TariffName} Tariff",
                GatewayType = Domain.Entities.Wallet.GatewayType.System,
                PaymentType = Domain.Entities.Wallet.PaymentType.Buy,
                Price = tariff.TariffPrice,
                TransactionType = Domain.Entities.Wallet.TransactionType.Withdraw,
                UserId = userId,
            };

            await _walletService.CreateWalletAsync(wallet);

            #endregion

            #endregion

            return 4;
        }

        //Check to see ads based on tariffs
        public async Task<bool> CheckUserSeeAdsBaseOnTariff(ulong userId)
        {
            #region Get Count Of User Seen Advertisement Today 

            var seenLog = await _tariff.GetCountOfUserSeenAdsToday(userId);

            #endregion

            #region Add Log 

            if (seenLog <= 3)
            {
                //Fill Model
                UserSeenAdvertisementLog log = new UserSeenAdvertisementLog()
                {
                    CreateDate = DateTime.Now,
                    UserId = userId,
                };

                //Add Log To Data Base 
                await _tariff.CreateUserAdvertisementLog(log);

                return true;
            }

            #endregion

            #region check User Has Any Tariff 

            if (await _tariff.HasUserAnyActiveTariffRightNow(userId))
            {
                #region Get User Active Tariff

                var tariff = await _tariff.GetUserActiveTariff(userId);

                //If the user rejects the allowed number
                if (tariff.CountOfSeenAdvertisement > seenLog)
                {
                    //Fill Model
                    UserSeenAdvertisementLog log = new UserSeenAdvertisementLog()
                    {
                        CreateDate = DateTime.Now,
                        UserId = userId,
                    };

                    //Add Log To Data Base 
                    await _tariff.CreateUserAdvertisementLog(log);

                    return true;
                }

                #endregion
            }

            #endregion

            return false;
        }

        #endregion

        #region User Panel
    
        //Check User Create Custmer Ads Log 
        public async Task<bool> CheckCustomerAdsBaseOnTariff(ulong userId)
        {
            #region Get Count Of Create Customer Advertisement Today 

            var createLog = await _tariff.GetCountOfCreateCustomerAdsToday(userId);

            #endregion

            #region Add Log 

            if (createLog <= 3)
            {
                //Fill Model
                UserCreateAdvertisementLog log = new UserCreateAdvertisementLog()
                {
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    FromCustomer = true,
                    FromEmployee = false,
                };

                //Add Log To Data Base 
                await _tariff.UserCreateCustomerAdsLog(log);

                return true;
            }

            #endregion

            #region check User Has Any Tariff 

            if (await _tariff.HasUserAnyActiveTariffRightNow(userId))
            {
                #region Get User Active Tariff

                var tariff = await _tariff.GetUserActiveTariff(userId);

                //If the user rejects the allowed number
                if (tariff.CountOfAddAdvertisement > createLog)
                {
                    //Fill Model
                    UserCreateAdvertisementLog log = new UserCreateAdvertisementLog()
                    {
                        CreateDate = DateTime.Now,
                        UserId = userId,
                        FromCustomer = true,
                        FromEmployee = false,
                    };

                    //Add Log To Data Base 
                    await _tariff.UserCreateCustomerAdsLog(log);

                    return true;
                }

                #endregion
            }

            #endregion

            return false;
        }

        //Check User Create Sale Ads Log 
        public async Task<bool> CheckSaleAdsBaseOnTariff(ulong userId)
        {
            #region Get Count Of Create Sale Advertisement Today 

            var createLog = await _tariff.GetCountOfCreateSaleAdsToday(userId);

            #endregion

            #region Add Log 

            if (createLog <= 3)
            {
                //Fill Model
                UserCreateAdvertisementLog log = new UserCreateAdvertisementLog()
                {
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    FromCustomer = false,
                    FromEmployee = true,
                };

                //Add Log To Data Base 
                await _tariff.UserCreateCustomerAdsLog(log);

                return true;
            }

            #endregion

            #region check User Has Any Tariff 

            if (await _tariff.HasUserAnyActiveTariffRightNow(userId))
            {
                #region Get User Active Tariff

                var tariff = await _tariff.GetUserActiveTariff(userId);

                //If the user rejects the allowed number
                if (tariff.CountOfAddAdvertisement > createLog)
                {
                    //Fill Model
                    UserCreateAdvertisementLog log = new UserCreateAdvertisementLog()
                    {
                        CreateDate = DateTime.Now,
                        UserId = userId,
                        FromCustomer = false,
                        FromEmployee = true,
                    };

                    //Add Log To Data Base 
                    await _tariff.UserCreateCustomerAdsLog(log);

                    return true;
                }

                #endregion
            }

            #endregion

            return false;
        }

        #endregion
    }
}
