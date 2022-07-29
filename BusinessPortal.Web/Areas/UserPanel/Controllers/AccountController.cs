using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class AccountController : UserBaseController
    {
        #region Ctor

        public IUserService _userService { get; set; }

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Send Request To Be Seller

        public async Task<IActionResult> RequestToBeSeller()
        {
            var result = await _userService.SendRequestToBeSeller( User.GetUserId());

            if (result)
            {
                #region English Language

                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    return JsonResponseStatus.SendStatus(
                          JsonResponseStatusType.Success,
                          "Your application for sales has been successfully registered",
                          null
                          );
                }

                #endregion

                #region Arabic Language 

                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    return JsonResponseStatus.SendStatus(
                          JsonResponseStatusType.Success,
                          "تم تسجيل طلبك للمبيعات بنجاح",
                          null
                          );
                }

                #endregion

                #region Russian Language

                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    return JsonResponseStatus.SendStatus(
                          JsonResponseStatusType.Success,
                          "Ваша заявка на продажу успешно зарегистрирована",
                          null
                          );
                }

                #endregion

                #region Turkish Langauge

                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    return JsonResponseStatus.SendStatus(
                          JsonResponseStatusType.Success,
                          "Satış başvurunuz başarıyla kaydedildi",
                          null
                          );
                }

                #endregion

                #region Persian Language

                return JsonResponseStatus.SendStatus(
                        JsonResponseStatusType.Success,
                        "درخواست شما برای فروشندگی باموفقیت ثبت شده",
                        null
                        );

                #endregion

            }

            #region English Language

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return JsonResponseStatus.SendStatus(
               JsonResponseStatusType.Warning,
               "Registration of applications is problematic ",
               null
               );
            }

            #endregion

            #region Arabic Language

            if (CultureInfo.CurrentCulture.Name == "ar-SA")
            {
                return JsonResponseStatus.SendStatus(
               JsonResponseStatusType.Warning,
               "تسجيل الطلبات إشكالية",
               null
               );
            }

            #endregion

            #region Russian Language

            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                return JsonResponseStatus.SendStatus(
               JsonResponseStatusType.Warning,
               "Регистрация приложений проблематична",
               null
               );
            }

            #endregion

            #region Turkish Language

            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                return JsonResponseStatus.SendStatus(
               JsonResponseStatusType.Warning,
               "Başvuruların kaydı sorunlu",
               null
               );
            }

            #endregion

            #region Persian Language

            return JsonResponseStatus.SendStatus(
                  JsonResponseStatusType.Warning,
                  "ثبت درخوسات بامشکل مواجه شده است ",
                  null
                  );

            #endregion

        }

        #endregion
       
    }
}
