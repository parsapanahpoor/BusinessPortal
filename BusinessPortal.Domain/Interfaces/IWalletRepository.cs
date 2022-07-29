﻿using BusinessPortal.Domain.Entities.Wallet;
using BusinessPortal.Domain.ViewModels.Admin.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Interfaces
{
    public interface IWalletRepository
    {
        #region Wallet

        Task<FilterWalletViewModel> FilterWalletsAsync(FilterWalletViewModel filter);

        Task<Wallet?> GetWalletByWalletIdAsync(ulong walletId);

        Task<int> GetSumUserWalletAsync(ulong userId);

        Task<AdminEditWalletViewModel?> GetWalletForEditAsync(ulong walletId);

        Task CreateWalletAsync(Wallet wallet);

        Task EditWalletAsync(Wallet wallet);

        Task ConfirmPayment(ulong payId, string authority, string refId);

        Task<Wallet> GetWalletById(ulong id);

        Task<ulong> CreateWallet(Wallet charge);

        Task<int> GetUserTotalDepositTransactions(ulong userId);

        Task<int> GetUserTotalWithdrawTransactions(ulong userId);

        Task<int> GetUserWalletBalance(ulong userId);

        #endregion

        #region Save Changes

        Task SaveChangesAsync();

        #endregion
    }
}
