using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    public class OurTagetCountriesViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IAdvertisementService _advertisementService;

        public OurTagetCountriesViewComponent(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("OurTagetCountries", await _advertisementService.ListOFOurTargetCountries());
        }
    }
}
