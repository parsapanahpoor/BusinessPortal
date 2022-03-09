using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.ViewModels.Account;
using BusinessPortal.Domain.ViewModels.Admin;
using BusinessPortal.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Authorize

        Task<User?> GetUserById(ulong userId);
        Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register);
        Task<bool> IsExistsUserByEmail(string email);
        Task<bool> IsExistUserByMobile(string mobile);
        Task<LoginResult> CheckUserForLogin(LoginUserViewModel login);
        Task<User?> GetUserByEmail(string email);
        Task<bool> AccountActivation(string emailActivationCode);
        Task<User> GetUserByEmailActivationCode(string emailActivationCode);
        Task<bool> ForgotPasswordUser(ForgotPasswordViewModel forgotPassword);
        Task<ResetPasswordViewModel> GetResetPasswordViewModel(string emailActivationCode);
        Task<bool> ResetPassword(ResetPasswordViewModel resetPassword);

        #endregion

        #region Admin

        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<bool> ChangePasswordInAdmin(ChangePasswordInAdminViewModel passwordViewModel);
        Task<AdminEditUserInfoViewModel> FillAdminEditUserInfoViewModel(ulong userId);
        Task<bool> IsValidMobileForUserEditByAdmin(string mobile, ulong userId);
        Task<bool> IsValidEmailForUserEditByAdmin(string email, ulong userId);
        Task<AdminEditUserInfoResult> EditUserInfo(AdminEditUserInfoViewModel edit);

        #endregion

    }
}
