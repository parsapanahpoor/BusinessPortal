using BusinessPortal.Domain.ViewModels.Admin.Slider;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface ISliderServicee 
    {
        #region Sldier

        //Filter Sliders In Admin Side 
        Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filter);

        //Create Slider
        Task<bool> CreateSlider(CreateSliderViewModel slider, IFormFile sldierImage);

        //Delete Slider
        Task<bool> DeleteSlider(ulong sliderId);

        #endregion

        #region Banners

        //Create Or Edit Banner
        Task<CreateOrEditBannerViewModel> FillCreateOrEditBannerViewModel();

        //Create OR Edit Banner 
        Task<bool> CreateOrEdirBanner(CreateOrEditBannerViewModel model, IFormFile? top, IFormFile? button);

        #endregion
    }
}
