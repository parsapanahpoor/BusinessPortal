using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return View("LastestAdvertisementFromEmployees", await _advertisementService.GetLastestAdvertisementFromEmployees());
        }
    }
}
