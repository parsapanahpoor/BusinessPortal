using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin;
using BusinessPortal.Domain.ViewModels.Admin.Account;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        #region ctor

        private IUserService _userService;
        private IPermissionService _permissionService;

        public AccountController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
     
        }

        #endregion

        #region Manage User

        #region Users List

        [HttpGet]
        public async Task<IActionResult> FilterUsers(FilterUserViewModel filter)
        {
            var result = await _userService.FilterUsers(filter);

            ViewData["Roles"] = await _permissionService.GetSelectRolesList();

            return View(result);
        }

        #endregion

        #region User Detail

        public async Task<IActionResult> AccountDetail(ulong id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null) return NotFound();

            return View(user);
        }

        #endregion

        #region Change Password

        [HttpGet]
        public async Task<IActionResult> ChangePassword(ulong userId)
        {
            var model = new ChangePasswordInAdminViewModel()
            {
                UserId = userId
            };
            return PartialView("_ChangePasswordPartial", model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordInAdminViewModel passwordViewModel)
        {
            var result = await _userService.ChangePasswordInAdmin(passwordViewModel);

            if (!result)
            {
                return JsonResponseStatus.Error();
            }

            return JsonResponseStatus.Success();
        }

        #endregion

        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> EditUserInfo(ulong id)
        {
            var result = await _userService.FillAdminEditUserInfoViewModel(id);

            ViewData["Roles"] = await _permissionService.GetSelectRolesList();

            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserInfo(AdminEditUserInfoViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _permissionService.GetSelectRolesList();

                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد .";
                return View(edit);
            }

            var result = await _userService.EditUserInfo(edit);

            switch (result)
            {
                case AdminEditUserInfoResult.NotValidImage:
                    TempData[ErrorMessage] = "تصویر انتخاب شده معتبر نمی باشد .";
                    break;
                case AdminEditUserInfoResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد .";
                    return RedirectToAction("FilterUsers", "Account", new { area = "Admin" });
                case AdminEditUserInfoResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("AccountDetail", "Account", new { area = "Admin", id = edit.UserId });
                case AdminEditUserInfoResult.NotValidEmail:
                    TempData[ErrorMessage] = "ایمیل وارد شده از قبل در سایت موجود است";
                    break;
                case AdminEditUserInfoResult.NotValidMobile:
                    TempData[ErrorMessage] = "موبایل وارد شده از قبل در سایت موجود است";
                    break;
                case AdminEditUserInfoResult.NotValidBirthDate:
                    TempData[ErrorMessage] = "تاریخ وارد شده نمی تواند بزرگتر از الان باشد .";
                    break;
            }

            ViewData["Roles"] = await _permissionService.GetSelectRolesList();

            return View(edit);
        }

        #endregion

    }
}
