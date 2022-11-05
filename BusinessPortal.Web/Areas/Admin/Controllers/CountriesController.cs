using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Countries;
using BusinessPortal.Domain.ViewModels.Admin.Countries;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class CountriesController : AdminBaseController
    {
        #region Ctor

        private readonly IAdvertisementService _advertisementService;

        public CountriesController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        #endregion

        #region List Of Countries

        public async Task<IActionResult> FilterCountries(FilterCountriesViewModel model)
        {
            return View(await _advertisementService.FilterCountries(model)) ;
        }

        #endregion

        #region Add Country 

        [HttpGet]
        public async Task<IActionResult> AddCountry()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountry(string uniqueName , IFormFile flag)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View();
            }

            #endregion

            #region Add Country Method 

            var res = await _advertisementService.CreateCountryAdminSide(uniqueName , flag);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(FilterCountries));
            }

            #endregion

            TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
            return View();
        }

        #endregion

        #region Edit Country 

        [HttpGet]
        public async Task<IActionResult> EditCountry(ulong countryId)
        {
            #region Get Model 

            var model = await _advertisementService.GetCountryById(countryId);
            if (model == null)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction(nameof(FilterCountries));
            }

            #endregion

            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCountry(ulong countryId , string uniqueName, IFormFile? flag)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View();
            }

            #endregion

            #region Edit Country Method 

            var res = await _advertisementService.EditCountry(countryId , uniqueName , flag);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است..";
                return RedirectToAction(nameof(FilterCountries));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View();
        }

        #endregion

        #region Delete Country

        public async Task<IActionResult> DeleteCountry(ulong countryId)
        {
            var res = await _advertisementService.DeleteCountry(countryId);
            if (res)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
