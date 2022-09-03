using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BusinessPortal.Web.Controllers
{
    public class TaariffController : SiteBaseController
    {
        #region Ctor

        private readonly ITariffService _tariffService;

        public IStringLocalizer<SharedLocalizer.SharedLocalizer> _sharedLocalizer;

        public TaariffController(ITariffService tariffService, IStringLocalizer<SharedLocalizer.SharedLocalizer> sharedLocalizer)
        {
            _tariffService = tariffService;
            _sharedLocalizer = sharedLocalizer;
        }

        #endregion

        #region Buy Tariff

        [Authorize]
        public async Task<IActionResult> BuyTariff(ulong tariffId)
        {
            #region Buy Tariff Method 

            var res = await _tariffService.BuyTariff(tariffId, User.GetUserId());

            switch (res)
            {
                case 1:
                    TempData[ErrorMessage] = "Your Wallet Balance Is Not Enough";
                    return RedirectToAction("Index", "Home");

                case 2:
                    TempData[ErrorMessage] = "You Have Tariff Right Now";
                    return RedirectToAction("Index", "Home");

                case 3:
                    TempData[ErrorMessage] = "The operation has failed";
                    return RedirectToAction("Index", "Home");

                case 4:
                    TempData[SuccessMessage] = "The operation has been successfully completed";
                    return RedirectToAction("Index", "Home" , new { area = "UserPanel" });
            }

            #endregion

            return View();
        }

        #endregion
    }
}
