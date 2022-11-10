using BusinessPortal.Domain.Entities.Ads;
using BusinessPortal.Domain.ViewModels.Admin.Ads;
using BusinessPortal.Domain.ViewModels.Site.Ads;
using BusinessPortal.Domain.ViewModels.UserPanel.Ads;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IAdsService
    {
        #region User Panel Side

        //Create Ads
        Task<bool> AddAdsFromUserPanel(AdsViewModel model, List<IFormFile> upload_imgs, ulong userId);

        //Filter Ads On User Panel 
        Task<FilterAdsViewModel> FilterAdsFromUserPanel(FilterAdsViewModel filter);

        //Fill Edit Ads From User Panel
        Task<EditAdsUserPanelViewModel?> FillEditAdsFromUserPanel(ulong Id);

        //Get Ads Gallery
        Task<List<AdsGallery>> GetAdsGalleriesForUser(ulong advertisementId);

        //Get Ads Galley
        Task<List<AdsGallery>> GetAdsGalleriesForAdmin(ulong advertisementId);

        //Delete Ads Galley Image
        Task<bool> DeleteGalleryByUser(ulong galleryId);

        //Edit Ads From User
        Task<bool> EditOnSaleAdvertisementFromUserPanel(EditAdsUserPanelViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs);

        //Delete Ads
        Task<bool> DeleteAds(ulong Id);

        //List Of Ads Site Side
        Task<List<FilterAdsSiteSideViewModel>> FilterAdsSiteSide(string culture);

        #endregion

        #region Admin Side

        //Filter Ads From Admin Side 
        Task<FilterAdsAdminSideViewModel> FilterAdsAdminSide(FilterAdsAdminSideViewModel filter);

        Task<AdsInfo?> ShowAdsLanguage(ulong adsId);

        #endregion
    }
}
