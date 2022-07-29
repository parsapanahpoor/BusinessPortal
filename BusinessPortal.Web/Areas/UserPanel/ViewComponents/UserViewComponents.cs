
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessPortal.Web.Areas.UserPanel.ViewComponents
{

    #region User SideBar ViewComponent

    public class UserSideBarViewComponent : ViewComponent
    {
        #region Ctor

        public IUserService _userService { get; set; }

        public UserSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.HasSellerPermission = await _userService.HasUserPermissionForSeller(User.GetUserId());

            ViewBag.HasAnyRequest = await _userService.IsExistRequestForSellerByUserId(User.GetUserId());

            return View("UserSideBar");
        }
    }

    #endregion

    #region User Header ViewComponent

    public class UserHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserHeader");
        }
    }

    #endregion

    #region User Chatbox ViewComponent

    public class UserChatboxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserChatbox");
        }
    }

    #endregion

    #region User Footer ViewComponent

    public class UserFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserFooter");
        }
    }

    #endregion

}
