using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Banners;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Entities.Slider;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using BusinessPortal.Domain.ViewModels.Admin.Slider;
using BusinessPortal.Domain.ViewModels.Admin.Tariff;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class SliderService : ISliderServicee
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public SliderService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Slider

        //Filter Sliders In Admin Side 
        public async Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filter)
        {
            var query = _context.Sliders
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter



            #endregion

            await filter.Paging(query);

            return filter;
        }

        //Create Slider
        public async Task<bool> CreateSlider(CreateSliderViewModel slider, IFormFile sldierImage)
        {
            #region Model Validation 

            if (sldierImage == null)
            {
                return false;
            }

            #endregion

            #region Create Slider

            var sliderEntity = new Slider()
            {
                URL = slider.URL.SanitizeText(),
                IsDelete = false,
            };

            #region Add Image 

            if (sldierImage != null && sldierImage.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(sldierImage.FileName);
                sldierImage.AddImageToServer(imageName, PathTools.SliderimageServerOrigin, 400, 300, PathTools.SliderImageServerThumb);
                sliderEntity.SldierImageName = imageName;
            }

            #endregion

            await _context.Sliders.AddAsync(sliderEntity);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        //Delete Slider
        public async Task<bool> DeleteSlider(ulong sliderId)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sliderId);
            if (slider == null) return false;

            //Delete Slider 
            slider.IsDelete = true;

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Banners

        //Fill Create Or Edit Banner View Model
        public async Task<CreateOrEditBannerViewModel> FillCreateOrEditBannerViewModel()
        {
            #region Get Banners

            var banner = await _context.Banners.FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region For First Time 

            if (banner == null)
            {
                return new CreateOrEditBannerViewModel()
                {
                    BanenrButton = null,
                    BannerTop = null
                };
            }

            #endregion

            #region For The Seconde Time 

            return new CreateOrEditBannerViewModel()
            {
                BanenrButton = banner.SlidersImageButton,
                BannerTop = banner.SliderImageTop,
                URLButton = banner.URLButton,
                URLImageTop = banner.URLImageTop
            };

            #endregion
        }

        //Create OR Edit Banner 
        public async Task<bool> CreateOrEdirBanner(CreateOrEditBannerViewModel model, IFormFile? top, IFormFile? button)
        {
            #region Get Banners

            var banner = await _context.Banners.FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region For The First Time 

            if (banner == null)
            {
                var firstTime = new Banners()
                {
                    URLImageTop = model.URLImageTop,
                    URLButton = model.URLButton,
                };

                #region Add Image 

                if (top != null && top.IsImage())
                {
                    var companylogo = Guid.NewGuid() + Path.GetExtension(top.FileName);
                    top.AddImageToServer(companylogo, PathTools.BannerimageServerOrigin, 400, 300, PathTools.BannerImageServerThumb);
                    firstTime.SliderImageTop = companylogo;
                }

                if (button != null && button.IsImage())
                {
                    var bannerButton = Guid.NewGuid() + Path.GetExtension(button.FileName);
                    button.AddImageToServer(bannerButton, PathTools.BannerimageServerOrigin, 400, 300, PathTools.BannerImageServerThumb);
                    firstTime.SlidersImageButton = bannerButton;
                }

                #endregion

                //Add Banner To The Data Base 
                await _context.Banners.AddAsync(firstTime);
                await _context.SaveChangesAsync();

                return true;
            }

            #endregion

            #region For The Seconde Time 

            banner.URLButton = model.URLButton;
            banner.URLImageTop = model.URLImageTop;

            #region Update Image 

            if (top != null && top.IsImage())
            {
                if (!string.IsNullOrEmpty(banner.SliderImageTop))
                {
                    banner.SliderImageTop.DeleteImage(PathTools.BannerimageServerOrigin, PathTools.BannerImageServerThumb);
                }

                var companylogo = Guid.NewGuid() + Path.GetExtension(top.FileName);
                top.AddImageToServer(companylogo, PathTools.BannerimageServerOrigin, 400, 300, PathTools.BannerImageServerThumb);
                banner.SliderImageTop = companylogo;
            }

            if (button != null && button.IsImage())
            {
                if (!string.IsNullOrEmpty(banner.SlidersImageButton))
                {
                    banner.SlidersImageButton.DeleteImage(PathTools.BannerimageServerOrigin, PathTools.BannerImageServerThumb);
                }

                var bannerButton = Guid.NewGuid() + Path.GetExtension(button.FileName);
                button.AddImageToServer(bannerButton, PathTools.BannerimageServerOrigin, 400, 300, PathTools.BannerImageServerThumb);
                banner.SlidersImageButton = bannerButton;
            }

            #endregion

            //Update Banner 

            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        #endregion
    }
}
