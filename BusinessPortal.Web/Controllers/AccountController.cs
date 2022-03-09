﻿using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Account;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusinessPortal.Web.Controllers
{
    public class AccountController : SiteBaseController
    {

        #region Ctor

        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Register

        [HttpGet("Register")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر وارد شده معتبر نمی باشد . ";
                return View(register);
            }

            var result = await _userService.RegisterUser(register);

            switch (result)
            {
                case RegisterUserResult.EmailExists:
                    TempData[ErrorMessage] = "ایمیل وارد شده موجود است .";
                    break;

                case RegisterUserResult.MobileExist:
                    TempData[ErrorMessage] = "شماره موبایل وارد شده موجود است ";
                    break;

                case RegisterUserResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    TempData[InfoMessage] = $"ایمیل حاوی کد فعالسازی حساب کاربری به {register.Email} ارسال شد .";
                    return RedirectToAction("Login", "Account");
            }

            return View(register);
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Login(string ReturnUrl = "")
        {
            var result = new LoginUserViewModel();

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                result.ReturnUrl = ReturnUrl;
            }

            return View(result);
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر وارد شده معتبر نمی باشد .";
                return View(login);
            }

            var result = await _userService.CheckUserForLogin(login);

            switch (result)
            {
                case LoginResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربری با اطلاعات وارد شده یافت نشد .";
                    break;
                case LoginResult.UserIsBan:
                    TempData[WarningMessage] = "دسترسی شما به سایت محدود شده است .";
                    break;
                case LoginResult.EmailNotActivated:
                    TempData[WarningMessage] = "برای ورود ابتدا باید از طریق ایمیل ارسال شده حساب کاربری خود را فعال کنید .";
                    break;
                case LoginResult.Success:

                    #region Login User

                    var user = await _userService.GetUserByEmail(login.Email);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties { IsPersistent = login.RememberMe };

                    await HttpContext.SignInAsync(principal, properties);

                    #endregion

                    TempData[SuccessMessage] = $"خوش آمدید {user.Username}";

                    if (!string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }

                    return Redirect("/");
            }

            return View(login);

        }

        #endregion

        #region Email Activation

        [HttpGet("Activate-Account/{emailActivationCode}")]
        public async Task<IActionResult> AccountActivation(string emailActivationCode)
        {
            if (string.IsNullOrEmpty(emailActivationCode))
            {
                TempData[ErrorMessage] = "کد فعالسازی شما معتبر نمی باشد لطفا مجدد تلاش کنید .";
                return RedirectToAction("Login", "Account");
            }

            var result = await _userService.AccountActivation(emailActivationCode);

            if (result)
            {
                TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد .";
            }
            else
            {
                TempData[ErrorMessage] = "کد فعالسازی شما معتبر نمی باشد لطفا مجدد تلاش کنید .";
            }

            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Forgot Password

        [HttpGet("ForgotPassword")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر وارد شده معتبر نمی باشد .";
                return View(forgotPassword);
            }

            var result = await _userService.ForgotPasswordUser(forgotPassword);

            if (result)
            {
                TempData[InfoMessage] = $"ایمیل حاوی لینک بازیابی کلمه عبور به {forgotPassword.Email} ارسال شد .";
                return RedirectToAction("Login", "Account");
            }

            TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";

            return View(forgotPassword);
        }

        #endregion

        #region Reset Password

        [HttpGet("ResetPassword/{emailActivationCode}")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ResetPassword(string emailActivationCode)
        {
            var result = await _userService.GetResetPasswordViewModel(emailActivationCode);

            // check user exists by activation code
            if (result == null)
            {
                TempData[ErrorMessage] = "کد فعالسازی شما معتبر نمی باشد لطفا مجدد تلاش کنید .";
                return RedirectToAction("Login", "Account");
            }

            return View(result);
        }

        [HttpPost("ResetPassword/{emailActivationCode}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر وارد شده معتبر نمی باشد .";
                return View(resetPassword);
            }

            var result = await _userService.ResetPassword(resetPassword);

            if (!result)
            {
                TempData[ErrorMessage] = "کد فعالسازی معتبر نمی باشد لطفا مجدد تلاش کنید .";
            }
            else
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
            }

            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Logout

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است ";
            return Redirect("/");
        }

        #endregion

    }
}
