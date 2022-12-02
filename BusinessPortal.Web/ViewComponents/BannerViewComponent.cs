using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    //Banner Component
    public class BannerViewComponent : ViewComponent
    {
        #region Ctor

        public ISliderServicee _sliderService { get; set; }

        public BannerViewComponent(ISliderServicee sliderService)
        {
            _sliderService = sliderService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Banner", await _sliderService.FillCreateOrEditBannerViewModel());
        }
    }
}
