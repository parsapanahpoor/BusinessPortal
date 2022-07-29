using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Generators;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Application.Utils;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.ViewModels.Account;
using BusinessPortal.Domain.ViewModels.Admin;
using BusinessPortal.Domain.ViewModels.Admin.Account;
using BusinessPortal.Domain.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }
        public ISiteSettingService _siteSettingService { get; set; }
        private IViewRenderService _viewRenderService;
        private IEmailSender _emailSender;

        public UserService(BusinessPortalDbContext context , ISiteSettingService siteSettingService , IViewRenderService viewRenderService, IEmailSender emailSender)
        {
            _context = context;
            _siteSettingService = siteSettingService;
            _viewRenderService = viewRenderService;
            _emailSender = emailSender;
        }

        #endregion

        #region Authorize

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(s =>
                s.Email == email.Trim().ToLower() && !s.IsDelete);
        }

        public async Task<LoginResult> CheckUserForLogin(LoginUserViewModel login)
        {
            // get email and password
            var password = PasswordHasher.EncodePasswordMd5(login.Password.SanitizeText());
            var email = login.Email.Trim().ToLower().SanitizeText();

            // get user by email
            var user = await GetUserByEmail(email);

            // check user exists
            if (user == null) return LoginResult.UserNotFound;

            // check user password
            if (!user.Password.Equals(password)) return LoginResult.UserNotFound;

            // check user ban status
            if (user.IsBan) return LoginResult.UserIsBan;

            // check user activation
            if (!user.IsEmailConfirm) return LoginResult.EmailNotActivated;

            return LoginResult.Success;
        }

        public async Task<bool> IsExistsUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(s => s.Email == email.Trim().ToLower() && !s.IsDelete);
        }

        public async Task<bool> IsExistUserByMobile(string mobile)
        {
            return await _context.Users.AnyAsync(p => p.Mobile == mobile && !p.IsDelete);
        }

        public async Task<User?> GetUserById(ulong userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(s => s.Id == userId && !s.IsDelete);
        }

        public async Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register)
        {
            //Fix Email Format
            var email = register.Email.Trim().ToLower().SanitizeText();

            //Check Email Address
            if (await IsExistsUserByEmail(register.Email))
            {
                return RegisterUserResult.EmailExists;
            }

            //Check Mobile Number
            if (await IsExistUserByMobile(register.Mobile))
            {
                return RegisterUserResult.MobileExist;
            }

            //Hash Password
            var password = PasswordHasher.EncodePasswordMd5(register.Password.SanitizeText());

            //Create User
            var User = new BusinessPortal.Domain.Entities.Account.User()
            {
                Email = email,
                Password = password,
                Username = email,
                Mobile = register.Mobile.SanitizeText(),
                EmailActivationCode = CodeGenerator.GenerateUniqCode(),
                MobileActivationCode = CodeGenerator.GenerateUniqCode(),
            };

            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();

            #region Send Email


            var emailViewModel = new EmailViewModel
            {
                EmailActivationCode = User.EmailActivationCode,
                ButtonName = "فعالسازی حساب کاربری",
                FullName = User.Username,
                Description = $"{User.Username} عزیز لطفا جهت فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید .",
                ButtonLink = $"{PathTools.SiteAddress}/Activate-Account/{User.EmailActivationCode}",
                EmailBanner = string.Empty,
            };

            string body = _viewRenderService.RenderToStringAsync("_Email", emailViewModel);

            await _emailSender.SendEmail(email, "فعالسازی حساب کاربری", body);

            //var result = SendEmail.Send(User.Email, "فعالسازی حساب کاربری", body);


            #endregion


            return RegisterUserResult.Success;

        }

        public async Task<bool> AccountActivation(string emailActivationCode)
        {
            // get user by email activation code
            var user = await GetUserByEmailActivationCode(emailActivationCode);

            // check user exists
            if (user == null) return false;

            // update user
            user.IsEmailConfirm = true;
            user.EmailActivationCode = CodeGenerator.GenerateUniqCode();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserByEmailActivationCode(string emailActivationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(s =>
                s.EmailActivationCode == emailActivationCode.SanitizeText());
        }

        public async Task<bool> ForgotPasswordUser(ForgotPasswordViewModel forgotPassword)
        {
            // get user by email
            var user = await GetUserByEmail(forgotPassword.Email.SanitizeText());

            // check user exists
            if (user == null) return false;

            #region Send Email

            var emailViewModel = new EmailViewModel
            {
                EmailActivationCode = user.EmailActivationCode,
                ButtonName = "فعالسازی حساب کاربری",
                FullName = user.Username,
                Description = $"{user.Username} عزیز لطفا جهت بازیابی کلمه عبور روی لینک زیر کلیک کنید .",
                ButtonLink = $"{PathTools.SiteAddress}/ResetPassword/{user.EmailActivationCode}",
                EmailBanner = string.Empty,
            };

            string body = _viewRenderService.RenderToStringAsync("_Email", emailViewModel);

            await _emailSender.SendEmail(forgotPassword.Email.SanitizeText(), "بازیابی کلمه عبور", body);

            #endregion

            return true;
        }

        public async Task<ResetPasswordViewModel> GetResetPasswordViewModel(string emailActivationCode)
        {
            // get user by activation code
            var user = await GetUserByEmailActivationCode(emailActivationCode.SanitizeText());

            // check user exists
            if (user == null) return null;

            return new ResetPasswordViewModel()
            {
                EmailActivationCode = user.EmailActivationCode
            };
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            // get user by activation code
            var user = await GetUserByEmailActivationCode(resetPassword.EmailActivationCode);

            // check user exists
            if (user == null) return false;

            // hash password
            var password = PasswordHasher.EncodePasswordMd5(resetPassword.Password.SanitizeText());

            // update user
            user.Password = password;
            user.IsEmailConfirm = true;
            user.EmailActivationCode = CodeGenerator.GenerateUniqCode();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Admin

        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            var query = _context.Users
                .Where(s => !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(s => EF.Functions.Like(s.Email, $"%{filter.Email}%"));
            }

            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                query = query.Where(s => s.Mobile != null && EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));
            }
      
            if (filter.RoleId != 0)
            {
                query = query.Include(s => s.UserRoles).Where(s => s.UserRoles.Select(s => s.RoleId).Contains(filter.RoleId));
            }

            if (!string.IsNullOrEmpty(filter.FromDate))
            {
                var fromDate = filter.FromDate.ToMiladiDateTime();
                query = query.Where(s => s.CreateDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(filter.username))
            {
                query = query.Where(s => s.Username.Contains(filter.username));
            }

            if (!string.IsNullOrEmpty(filter.ToDate))
            {
                var toDate = filter.ToDate.ToMiladiDateTime();
                query = query.Where(s => s.CreateDate <= toDate);
            }

            if (filter.TodayRegister == true)
            {
                query = query.Where(p => !p.IsDelete && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.DayOfYear == DateTime.Now.DayOfYear);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<bool> ChangePasswordInAdmin(ChangePasswordInAdminViewModel passwordViewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == passwordViewModel.UserId);

            if (user == null)
            {
                return false;
            }

            user.Password = PasswordHasher.EncodePasswordMd5(passwordViewModel.Password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AdminEditUserInfoViewModel> FillAdminEditUserInfoViewModel(ulong userId)
        {
            var user = await GetUserById(userId);

            if (user == null) return null;

            var userRoleIds = await _context.UserRoles
                .Where(s => !s.IsDelete && s.UserId == userId)
                .Select(s => s.RoleId)
                .ToListAsync();

            return new AdminEditUserInfoViewModel()
            {
                Mobile = user.Mobile,
                Email = user.Email,
                BanForComment = user.BanForComment,
                BanForTicket = user.BanForTicket,
                IsAdmin = user.IsAdmin,
                IsBan = user.IsBan,
                IsEmailConfirm = user.IsEmailConfirm,
                IsMobileConfirm = user.IsMobileConfirm,
                username = user.Username,
                UserRoles = userRoleIds,
                UserId = user.Id,
                AvatarName = user.Avatar
            };
        }

        public async Task<AdminEditUserInfoResult> EditUserInfo(AdminEditUserInfoViewModel edit)
        {
            var user = await GetUserById(edit.UserId);

            if (!await IsValidEmailForUserEditByAdmin(edit.Email, user.Id))
            {
                return AdminEditUserInfoResult.NotValidEmail;
            }

            if (!string.IsNullOrEmpty(edit.Mobile) && !await IsValidMobileForUserEditByAdmin(edit.Mobile, user.Id))
            {
                return AdminEditUserInfoResult.NotValidMobile;
            }

            user.Username = edit.username;
            user.Email = edit.Email;
            user.Mobile = edit.Mobile;
            user.IsMobileConfirm = edit.IsMobileConfirm;
            user.IsEmailConfirm = edit.IsEmailConfirm;
            user.IsAdmin = edit.IsAdmin;
            user.IsBan = edit.IsBan;
            user.BanForComment = edit.BanForComment;
            user.BanForTicket = edit.BanForTicket;

            _context.Update(user);
            await _context.SaveChangesAsync();

            #region Delete User Rols

            var roles = await _context.UserRoles.Where(s => !s.IsDelete && s.UserId == user.Id).ToListAsync();

            _context.RemoveRange(roles);

            #endregion

            if (edit.UserRoles != null && edit.UserRoles.Any())
            {
                foreach (var roleId in edit.UserRoles)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = roleId,
                        UserId = user.Id
                    };
                    await _context.AddAsync(userRole);
                }

                await _context.SaveChangesAsync();
            }

            return AdminEditUserInfoResult.Success;
        }

        public async Task<bool> IsValidMobileForUserEditByAdmin(string mobile, ulong userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => !s.IsDelete && s.Mobile == mobile.Trim());

            if (user == null) return true;
            if (user.Id == userId) return true;

            return false;
        }
        public async Task<bool> IsValidEmailForUserEditByAdmin(string email, ulong userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => !s.IsDelete && s.Email == email.Trim().ToLower());

            if (user == null) return true;

            if (user.Id == userId) return true;

            return false;
        }

        #endregion

        #region User Panel

        public async Task<bool> SendRequestToBeSeller(ulong userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return false;

            var anyRequest = await _context.requestForSellers.AnyAsync(p => !p.IsDelete && p.UserId == userId);
            if (anyRequest == true) return false;

            #region Request For Seller

            RequestForSeller request = new RequestForSeller()
            {
                UserId = userId,
                CreateDate = DateTime.Now,
                RequestForSellerStatus = Domain.Enums.RequestForSellerStatus.Accept
            };

            await _context.requestForSellers.AddAsync(request);
            await _context.SaveChangesAsync();

            #endregion

            #region Add Seller To This Id

            Seller seller = new Seller()
            {
                UserId = userId,
                CreateDate = DateTime.Now
            };

            await _context.Seller.AddAsync(seller);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<bool> HasUserPermissionForSeller(ulong userId)
        {
            return await _context.Seller.AnyAsync(p=>p.UserId == userId);
        }

        public async Task<bool> IsExistRequestForSellerByUserId(ulong userId)
        {
            var request = await _context.requestForSellers.AnyAsync(p => p.UserId == userId);

            var seller = await _context.Seller.AnyAsync(p => p.UserId == userId);

            if (request == false && seller == false) return true;

            return false;
        }

        #endregion
    }
}
