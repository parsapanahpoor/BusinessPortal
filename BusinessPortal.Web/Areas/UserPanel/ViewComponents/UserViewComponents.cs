﻿
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
        private readonly ITariffService _tiffService;

        public UserSideBarViewComponent(IUserService userService , ITariffService tariffService)
        {
            _userService = userService;
            _tiffService = tariffService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.HasSellerPermission = await _userService.HasUserPermissionForSeller(User.GetUserId());

            ViewBag.HasAnyRequest = await _userService.IsExistRequestForSellerByUserId(User.GetUserId());

            ViewBag.userSelectedTariffInfo = await _tiffService.UserPanelTariffViewModel(User.GetUserId());

            return View("UserSideBar");
        }
    }

    #endregion

    #region User Header ViewComponent

    public class UserHeaderViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IWalletService _walletService;

        private readonly IUserService _userService;

        public UserHeaderViewComponent(IWalletService walletService, IUserService userService)
        {
            _walletService = walletService;
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
          return View("UserHeader" , await _userService.GetUserById(User.GetUserId()));
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
