using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Slider;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BusinessPortal.Web.ViewComponents
{
    //Slider View Component
    public class ListOfSliderViewComponent : ViewComponent
    {
        #region Ctor

        public ISliderServicee _sliderService { get; set; }

        public ListOfSliderViewComponent(ISliderServicee sliderService)
        {
            _sliderService = sliderService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterSliderViewModel();

            return View("ListOfSlider", await _sliderService.FilterSlider(filter));
        }
    }
}
