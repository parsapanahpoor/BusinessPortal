using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.Ads;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using BusinessPortal.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BusinessPortal.Web.Areas.UserPanel.Controllers
{
    public class AdsController : UserBaseController
    {
        #region ctor

        private readonly IAdsService _adsService;

        public AdsController(IAdsService adsService)
        {
            _adsService = adsService;
        }

        #endregion

        #region List Of Ads

        public async Task<IActionResult> ListOfAds(FilterAdsViewModel filter)
        {
            #region Seed Data

            filter.UserId = User.GetUserId();
            filter.FilterAdsState = FilterAdsState.All;

            #endregion

            return View(await _adsService.FilterAdsFromUserPanel(filter));
        }

        #endregion

        #region Create Ads

        [HttpGet]
        public async Task<IActionResult> CreateAds()
        {
            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAds(AdsViewModel ads, List<IFormFile> upload_imgs)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                if (upload_imgs == null || upload_imgs.Count() >= 10)
                {
                    if (CultureInfo.CurrentCulture.Name == "fa-IR")
                    {
                        TempData[WarningMessage] = "لطفا حداقل یک تصویر را انتخاب کنید.";
                    }
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Please select at least one image.";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Выберите хотя бы одно изображение.";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "الرجاء تحديد صورة واحدة على الأقل.";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "Selecione pelo menos uma imagem.";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Lütfen en az bir resim seçin.";
                    }

                    return View(ads);
                }

                return View(ads);
            }

            #endregion

            #region Add Ads

            var res = await _adsService.AddAdsFromUserPanel(ads , upload_imgs , User.GetUserId());
            if (res)
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

                return RedirectToAction(nameof(ListOfAds));
            }

            #endregion

            return View();
        }

        #endregion

        #region Edit Ads

        [HttpGet]
        public async Task<IActionResult> EditAds(ulong Id)
        {
            #region Fill Model 

            var ads = await _adsService.FillEditAdsFromUserPanel(Id);
            if (ads == null) return NotFound();

            ads.Description = ads.Description.ConvertBrToNewLine();

            #endregion

            #region Model Data

            ViewBag.SelectedGalleries = await _adsService.GetAdsGalleriesForAdmin(Id);

            #endregion

            return View(ads);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAds(EditAdsUserPanelViewModel model, IFormFile? ImageName, List<IFormFile> upload_imgs)
        {
            #region Model Data

            ViewBag.SelectedGalleries = await _adsService.GetAdsGalleriesForAdmin(model.AdsId);

            #endregion

            if (model.UserId != User.GetUserId())
            {
                return NotFound();
            }

            var ads = await _adsService.FillEditAdsFromUserPanel(model.AdsId);
            if (ads == null) return NotFound();

            ads.Description = ads.Description.ConvertBrToNewLine();

            if (ModelState.IsValid)
            {
                var result = await _adsService.EditOnSaleAdvertisementFromUserPanel(model, ImageName, upload_imgs);
                if (result)
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

                    return RedirectToAction(nameof(ListOfAds));
                }

            }

            return View(model);
        }

        #endregion

        #region Delete gallery Images From Admin

        public async Task<IActionResult> DeleteImageGalleryByAdmin(ulong galleryId , ulong adsId)
        {
            var ads = adsId;

            var result = await _adsService.DeleteGalleryByUser(galleryId);
            if (result)
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

                return RedirectToAction(nameof(EditAds) , new { Id = adsId });
            }
            else
            {
                return RedirectToAction(nameof(EditAds), new { Id = adsId });
            }
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

                return RedirectToAction(nameof(ListOfAds));
            }
            else
            {
                return RedirectToAction(nameof(ListOfAds));
            }

        }

        #endregion

    }
}
