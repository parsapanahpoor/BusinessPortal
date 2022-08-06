using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.ViewModels.Admin.State;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class TariffController : AdminBaseController
    {
        #region Ctor

        private readonly ITariffService _tariffService;

        public TariffController(ITariffService tariffService)
        {
            _tariffService = tariffService;
        }

        #endregion

        #region List Of Tariffs

        public async Task<IActionResult> FilterTariff(FilterTariffViewModel filter)
        {
            return View(await _tariffService.FilterTariff(filter));            
        }

        #endregion

        #region Create Tariff

        [HttpGet]
        public async Task<IActionResult> CreateTariff()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTariff(CreateTariffViewModel model)
        {
            #region Model State 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                return View(model);
            }

            #endregion

            #region Add Tariff 

            var res = await _tariffService.CreateTariff(model);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterTariff));
            }

            #endregion

            TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
            return View(model);
        }

        #endregion

        #region Edit Tariff 

        [HttpGet]
        public async Task<IActionResult> EditTariff(ulong tariffId)
        {
            #region Get Tariff By Id 

            var tariff = await _tariffService.GetTariffById(tariffId);
            if(tariff == null) return NotFound();

            #endregion

            return View(tariff);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTariff(Tariff tariff)
        {
            #region Edit Tariff 

            var res = await _tariffService.EditTariff(tariff);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterTariff));
            }

            #endregion

            TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
            return View(tariff);
        }

        #endregion

        #region Delete Tariff

        public async Task<IActionResult> DeleteTariff(ulong tariffId)
        {
            var result = await _tariffService.DeleteTariff(tariffId);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
