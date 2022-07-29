using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        public HomeController(IAdvertisementService advertisementService)
        {
           _advertisementService = advertisementService;
        }

        #endregion

        #region Admin Dashboard

        public async Task<IActionResult> Index()
        {
            return View(await _advertisementService.FillAdminDashboadrdViewModel());
        }

        #endregion

    }
}
