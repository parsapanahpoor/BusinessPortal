using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Ads;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class AdsController : AdminBaseController
    {
        #region Ctor

        private readonly IAdsService _adsService;

        public AdsController(IAdsService adsService)
        {
            _adsService = adsService;
        }

        #endregion

        #region Filter Ads 

        public async Task<IActionResult> FilterAdsAdminSide(FilterAdsAdminSideViewModel filter)
        {
            return View(await _adsService.FilterAdsAdminSide(filter));
        }

        #endregion

        #region Show Ads Language

        public async Task<IActionResult> ShowAdsLanguage(ulong adsId)
        {
            var ads = await _adsService.ShowAdsLanguage(adsId);

            return PartialView("_ShowAdsLanguage", ads);
        }

        #endregion

        #region Delete Ads

        public async Task<IActionResult> DeleteAds(ulong Id)
        {
            var res = await _adsService.DeleteAds(Id);

            if (res == true)
            {
                if (CultureInfo.CurrentCulture.Name == "fa-IR")
                {
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                }
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    TempData[SuccessMessage] = "Please select at least one image.";
                }
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    TempData[SuccessMessage] = "Операция завершена успешно.";
                }
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    TempData[SuccessMessage] = "تمت العملية بنجاح.";
                }
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    TempData[SuccessMessage] = "A operação foi concluída com sucesso.";
                }
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    TempData[SuccessMessage] = "İşlem başarıyla tamamlandı.";
                }

                return RedirectToAction(nameof(FilterAdsAdminSide));
            }
            else
            {
                return RedirectToAction(nameof(FilterAdsAdminSide));
            }

        }

        #endregion
    }
}
