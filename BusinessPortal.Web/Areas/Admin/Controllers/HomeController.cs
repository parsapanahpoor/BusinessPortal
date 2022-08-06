using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        private readonly IUserService _userService;

        public HomeController(IAdvertisementService advertisementService , IUserService userService)
        {
           _advertisementService = advertisementService;
            _userService = userService;
        }

        #endregion

        #region Admin Dashboard

        public async Task<IActionResult> Index()
        {
            return View(await _advertisementService.FillAdminDashboadrdViewModel());
        }

        #endregion

        #region SearchUserModal

        public async Task<IActionResult> SearchUserModal(FilterUserViewModel filter, string baseName)
        {
            filter.TakeEntity = 5;

            var result = await _userService.FilterUsers(filter);

            ViewBag.BaseName = baseName;

            return PartialView("_FilterUsersModalPartial", result);
        }

        #endregion
    }
}
