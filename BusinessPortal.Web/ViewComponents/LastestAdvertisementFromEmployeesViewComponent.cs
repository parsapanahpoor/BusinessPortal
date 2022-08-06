using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.ViewComponents
{
    public class LastestAdvertisementFromEmployeesViewComponent : ViewComponent
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        public LastestAdvertisementFromEmployeesViewComponent(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            return View("LastestAdvertisementFromEmployees", await _advertisementService.GetLastestAdvertisementFromEmployees(culture));
        }
    }
}
