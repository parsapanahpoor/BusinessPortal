using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.ViewComponents
{
    public class LastestAdvertisementFromCustomersViewComponent : ViewComponent
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        public LastestAdvertisementFromCustomersViewComponent(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            return View("LastestAdvertisementFromCustomers", await _advertisementService.GetLastestAdvertisementFromCustomers(culture));
        }
    }

}
