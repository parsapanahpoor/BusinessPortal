using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Slider;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class SliderController : AdminBaseController
    {
        #region Ctor

        private readonly ISliderServicee _sliderService;

        public SliderController(ISliderServicee sliderService)
        {
            _sliderService = sliderService;
        }

        #endregion

        #region Slider

        #region List Of Sliders

        public async Task<IActionResult> ListOfSldiers(FilterSliderViewModel filter)
        {
            return View(await _sliderService.FilterSlider(filter));
        }

        #endregion

        #region Create Slider

        [HttpGet]
        public async Task<IActionResult> CreateSlider()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(CreateSliderViewModel model, IFormFile serviceCategoryImage)
        {
            #region Model State Validation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View(model);
            }

            #endregion

            #region Create Slider

            var res = await _sliderService.CreateSlider(model, serviceCategoryImage);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است .";
                return RedirectToAction(nameof(ListOfSldiers));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        #region Delete Slider 

        public async Task<IActionResult> DeleteSlider(ulong sliderId)
        {
            var res = await _sliderService.DeleteSlider(sliderId);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است .";
                return RedirectToAction(nameof(ListOfSldiers));
            }

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(ListOfSldiers));
        }

        #endregion

        #endregion

        #region Banner

        #region Create OR Edit Banner

        [HttpGet]
        public async Task<IActionResult> CreateOrEditBanner()
        {
            return View(await _sliderService.FillCreateOrEditBannerViewModel());
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEditBanner(CreateOrEditBannerViewModel model , IFormFile? top , IFormFile? button)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction(nameof(CreateOrEditBanner));
            }

            #endregion

            #region Create OR Edit Banner

            var res = await _sliderService.CreateOrEdirBanner(model , top , button);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است .";
                return RedirectToAction(nameof(CreateOrEditBanner));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(CreateOrEditBanner));
        }

        #endregion

        #endregion
    }
}
