using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.SiteServices;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Ads;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Ads;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.Site.Ads;
using BusinessPortal.Domain.ViewModels.Site.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.Ads;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class AdsService : IAdsService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }

        public AdsService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region User Panel Side 

        //Create Ads
        public async Task<bool> AddAdsFromUserPanel(AdsViewModel model, List<IFormFile> upload_imgs , ulong userId)
        {
            #region Validation

            string lang = CultureInfo.CurrentCulture.Name;

            if (upload_imgs.Count > 10)
            {
                return false;
            }

            #endregion

            #region ads Properties

            Ads ads = new Ads()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                UserId = userId
            };

            #endregion

            #region Gallery

            if (upload_imgs != null && upload_imgs.Any())
            {
                if (upload_imgs.Count == 1)
                {
                    foreach (var item in upload_imgs)
                    {
                        if (item != null && item.IsImage())
                        {
                            var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                            item.AddImageToServer(imageName, PathTools.AdsimageServerOrigin, 150, 150, PathTools.AdsImageServerThumb);
                            ads.AdsImage = imageName;

                            //Add Ads
                            await _context.Ads.AddAsync(ads);
                            await _context.SaveChangesAsync();
                        }

                        if (item != null && !item.IsImage())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    var firstImage = upload_imgs.First();
                    var imageName = Guid.NewGuid() + Path.GetExtension(firstImage.FileName);
                    firstImage.AddImageToServer(imageName, PathTools.AdsimageServerOrigin, 150, 150, PathTools.AdsImageServerThumb);
                    ads.AdsImage = imageName;

                    //Add Ads
                    await _context.Ads.AddAsync(ads);
                    await _context.SaveChangesAsync();

                    var skippedListGallery = upload_imgs.Skip(1).ToList();
                    foreach (var galleyItem in skippedListGallery)
                    {
                        if (galleyItem.IsImage())
                        {
                            var newGallery = new AdsGallery();

                            newGallery.IsDelete = false;
                            newGallery.CreateDate = DateTime.Now;
                            newGallery.AdsId = ads.Id;

                            var itemImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleyItem.FileName);
                            galleyItem.AddImageToServer(itemImageName, PathTools.AdsimageServerOrigin, 150, 150, PathTools.AdsImageServerThumb);
                            newGallery.ImageName = itemImageName;

                            //Add Ads Gallery
                            await _context.AdsGalleries.AddAsync(newGallery);
                            await _context.SaveChangesAsync();
                        }

                        if (galleyItem != null && !galleyItem.IsImage())
                        {
                            return false;
                        }
                    }
                }
            }

            if (upload_imgs == null && !upload_imgs.Any())
            {
                return false;
            }

            #endregion

            #region Ads Info Properties

            AdsInfo adsInfo = new AdsInfo()
            {
                ShortDescription = model.ShortDescription,
                AdsName = model.AdsName,
                LongDescription = model.LongDescription,
                AdsId = ads.Id,
                Lang_Id = lang,
            };

            //Add AdsInfo
            await _context.AdsInfo.AddAsync(adsInfo);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        //Filter Ads On User Panel 
        public async Task<FilterAdsViewModel> FilterAdsFromUserPanel(FilterAdsViewModel filter)
        {
            var query = _context.Ads
                            .Where(p => p.UserId == filter.UserId && !p.IsDelete)
                            .Include(s => s.User)
                            .Include(p => p.AdsInfos)
                            .ThenInclude(p => p.Language)
                            .AsQueryable();

            #region State

            switch (filter.FilterAdsState)
            {
                case FilterAdsState.All:
                    break;
                case FilterAdsState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterAdsState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
                default:
                    break;
            }

            #endregion

            #region Filter


            //if (filter.Title != null)
            //    query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title.Trim()}%") || s.AdvertisementTags.Any(f => f.TagTitle == filter.Title));

            #endregion

            await filter.Paging(query);

            return filter;
        }

        //Fill Edit Ads From User Panel
        public async Task<EditAdsUserPanelViewModel?> FillEditAdsFromUserPanel(ulong Id)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Ads
                .Include(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            #region Fill View Model

            EditAdsUserPanelViewModel model = new EditAdsUserPanelViewModel()
            {
                UserId = Ads.UserId,
                AdsImage = Ads.AdsImage,
                AdsId = Ads.Id
            };

            #endregion

            #region Advertisement Info

            var adsInfo = await _context.AdsInfo.FirstOrDefaultAsync(p => p.AdsId == Ads.Id
                                                        && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo != null)
            {
                model.Title = adsInfo.AdsName;
                model.Description = adsInfo.LongDescription;
                model.ShortDescription = adsInfo.ShortDescription;
            }

            #endregion

            return model;
        }

        //Get Ads Gallery
        public async Task<List<AdsGallery>> GetAdsGalleriesForUser(ulong advertisementId)
        {
            return await _context.AdsGalleries
                .Where(s => s.AdsId == advertisementId)
                .ToListAsync();
        }

        //Get Ads Galley
        public async Task<List<AdsGallery>> GetAdsGalleriesForAdmin(ulong advertisementId)
        {
            var galleries = await GetAdsGalleriesForUser(advertisementId);
            if (galleries == null && !galleries.Any()) return null;
            return galleries;
        }

        //Delete Ads Galley Image
        public async Task<bool> DeleteGalleryByUser(ulong galleryId)
        {
            //Get Gallery Image
            var gallery = await _context.AdsGalleries.FindAsync(galleryId);
            if (gallery == null) return false;

            //Remove Galley Image
            _context.AdsGalleries.Remove(gallery);
            await _context.SaveChangesAsync();

            string deleteOriginPath = PathTools.AdsimageServerOrigin + gallery.ImageName;
            string deleteThumbPath = PathTools.AdsimageServerOriginThumb + gallery.ImageName;

            if (File.Exists(deleteOriginPath))
            {
                File.Delete(deleteOriginPath);
            }
            if (File.Exists(deleteThumbPath))
            {
                File.Delete(deleteThumbPath);
            }

            return true;
        }

        //Edit Ads From User
        public async Task<bool> EditOnSaleAdvertisementFromUserPanel(EditAdsUserPanelViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs)
        {
            #region Get Ads By Ads Id

            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Ads
                .Include(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == model.AdsId && p.UserId == model.UserId && !p.IsDelete);

            if (Ads == null)
            {
                return false;
            }

            #endregion

            #region Advertisement Info 

            var adsInfo = await _context.AdsInfo.FirstOrDefaultAsync(p => p.AdsId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                AdsInfo adsInfoNew = new AdsInfo()
                {
                    AdsId = Ads.Id,
                    Lang_Id = lang,
                    LongDescription = model.Description.SanitizeText(),
                    AdsName = model.Title.SanitizeText(),
                    ShortDescription = model.ShortDescription.SanitizeText(),
                    CreateDate = DateTime.Now
                };

                await _context.AdsInfo.AddAsync(adsInfoNew);
                await _context.SaveChangesAsync();
            }
            else
            {
                adsInfo.LongDescription = model.Description.SanitizeText();
                adsInfo.AdsName = model.Title.SanitizeText();
                adsInfo.ShortDescription = model.ShortDescription.SanitizeText();

                _context.AdsInfo.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Image Part


            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.AdsImage != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdsimageServerOrigin, 150, 150
                     , PathTools.AdsimageServerOriginThumb, Ads.AdsImage);

                    if (res)
                    {
                        Ads.AdsImage = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdsimageServerOrigin, 150, 150
                     , PathTools.AdsimageServerOriginThumb);

                    if (res)
                    {
                        Ads.AdsImage = imageName;
                    }
                }

            }

            if (ImageName != null && !ImageName.IsImage())
            {
                return false;
            }

            #endregion

            #region Upload Multiple Images

            if (upload_imgs != null)
            {
                foreach (var item in upload_imgs)
                {
                    if (item.IsImage())
                    {
                        //New Instance Of Gallery
                        var newGallery = new AdsGallery();

                        newGallery.IsDelete = false;
                        newGallery.CreateDate = DateTime.Now;
                        newGallery.AdsId = Ads.Id;

                        var itemImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(item.FileName);
                        item.AddImageToServer(itemImageName, PathTools.AdsimageServerOrigin, 120, 120, PathTools.AdsimageServerOriginThumb);

                        newGallery.ImageName = itemImageName;

                        //Add Image Gallery
                        await _context.AdsGalleries.AddRangeAsync(newGallery);
                        await _context.SaveChangesAsync();
                    }

                    if (item != null && !item.IsImage())
                    {
                        return false;
                    }
                }
            }

            #endregion

            //Update Ads
            _context.Ads.Update(Ads);
            await _context.SaveChangesAsync();

            return true;
        }

        //Delete Ads
        public async Task<bool> DeleteAds(ulong Id)
        {
            var ads = await _context.Ads.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == Id);

            if (ads == null)
            {
                return false;
            }

            ads.IsDelete = true;

            _context.Ads.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Site Side

        //List Of Ads Site Side
        public async Task<List<FilterAdsSiteSideViewModel>> FilterAdsSiteSide(string culture)
        {
            #region Get Current Advertisements

            var ads = await _context.AdsInfo
                        .Include(p => p.Ads)
                        .Where(p => !p.IsDelete && p.Lang_Id == culture)
                        .Select(p => p.Ads).ToListAsync();

            #endregion

            #region model

            var model = new List<FilterAdsSiteSideViewModel>();

            foreach (var item in ads)
            {
                model.Add(new FilterAdsSiteSideViewModel
                {
                    AdsId = item.Id,
                    AdsTitle = await _context.AdsInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdsId == item.Id).Select(p => p.AdsName).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    Image = item.AdsImage,
                });
            }

            return model;

            #endregion
        }

        #endregion

        #region Admin Side 

        //Filter Ads From Admin Side 
        public async Task<FilterAdsAdminSideViewModel> FilterAdsAdminSide(FilterAdsAdminSideViewModel filter)
        {
            var query = _context.Ads
                            .Include(s => s.User)
                            .Include(p => p.AdsInfos)
                            .ThenInclude(p => p.Language)
                            .Where(p=> !p.IsDelete)
                            .OrderByDescending(p=> p.CreateDate)
                            .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        public async Task<AdsInfo?> ShowAdsLanguage(ulong adsId)
        {
            return await _context.AdsInfo.FirstOrDefaultAsync(p => p.Id == adsId && !p.IsDelete);
        }

        #endregion
    }
}
