using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.ViewComponents
{
    public class LastestOurOfferAdvertisementFromCustomersViewComponent : ViewComponent
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        public LastestOurOfferAdvertisementFromCustomersViewComponent(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            return View("LastestOurOfferAdvertisementFromCustomers", await _advertisementService.GetLastestOurOfferAdvertisementFromCustomers(culture));
        }
    }
}
