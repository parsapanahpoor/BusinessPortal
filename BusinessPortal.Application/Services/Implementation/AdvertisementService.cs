using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.Dashboard;
using BusinessPortal.Domain.ViewModels.Advertisement;
using BusinessPortal.Domain.ViewModels.Site.Advertisement;
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
    public class AdvertisementService : IAdvertisementService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }

        public AdvertisementService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Site Side

        public async Task<List<LastestCustomersAdvertisements>> GetLastestAdvertisementFromCustomers(string culture)
        {
            var advertisement = await _context.Advertisement.Where(p => !p.IsDelete && p.AdvertisementStatus == AdvertisementStatus.Active
                                                    && p.FromCustomer && !p.FromEmployee && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                                                    .OrderByDescending(p => p.CreateDate).Take(10).ToListAsync();

            var model = new List<LastestCustomersAdvertisements>();

            foreach (var item in advertisement)
            {
                model.Add(new LastestCustomersAdvertisements
                {
                    AdvertisementId = item.Id,
                    AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    AdvertisementAddress = await _context.Addresses.Include(p => p.LocationCountry).FirstOrDefaultAsync(p => p.Id == item.AddressId.Value)
                }
                );
            }

            return model;
        }

        public async Task<List<LastestEmployeesAdvertisements>> GetLastestAdvertisementFromEmployees(string culture)
        {
            var advertisement = await _context.Advertisement.Where(p => !p.IsDelete && p.AdvertisementStatus == AdvertisementStatus.Active
                                                && p.FromEmployee && !p.FromCustomer && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                                                    .OrderByDescending(p => p.CreateDate).Take(10).ToListAsync();

            var model = new List<LastestEmployeesAdvertisements>();

            foreach (var item in advertisement)
            {
                model.Add(new LastestEmployeesAdvertisements
                {
                    AdvertisementId = item.Id,
                    AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    AdvertisementAddress = await _context.Addresses.Include(p => p.LocationCountry).FirstOrDefaultAsync(p => p.Id == item.AddressId.Value)
                }
                );
            }
            return model;
        }

        #endregion

        #region Admin Side

        public async Task<AdvertisementInfo?> ShowAdvertisementLanguage(ulong adsId)
        {
            return await _context.advertisementInfo.FirstOrDefaultAsync(p => p.Id == adsId && !p.IsDelete);
        }

        public async Task<bool> DeleteAdvertisementFromAdmin(ulong id)
        {
            var ads = await _context.Advertisement.FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);

            if (ads == null) return false;

            ads.IsDelete = true;

            _context.Advertisement.Update(ads);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<AdminDashboardViewModel> FillAdminDashboadrdViewModel()
        {
            var model = new AdminDashboardViewModel();

            #region Lastest Waiting Requested Advertisements

            var waitingCustomerAds = new List<LastestWaitingCustomersAdvertisements>();

            var ListOfWaitingRequestedAdvertisements = await _context.Advertisement.Where(p => !p.IsDelete && p.AdvertisementStatus == AdvertisementStatus.WaitigForConfirm && p.FromCustomer && !p.FromEmployee)
                                                    .OrderByDescending(p => p.CreateDate).Take(10).ToListAsync();

            foreach (var item in ListOfWaitingRequestedAdvertisements)
            {
                waitingCustomerAds.Add(new LastestWaitingCustomersAdvertisements
                {
                    AdvertisementId = item.Id,
                    //AdvertisementTitle = item.Title,
                    CreateDate = item.CreateDate,
                    AdvertisementImage = item.ImageName,
                    AdvertisementAddress = await _context.Addresses.Include(p => p.LocationCountry).FirstOrDefaultAsync(p => p.Id == item.AddressId.Value),
                    Customer = await _context.Users.FirstOrDefaultAsync(p => p.Id == item.UserId && !p.IsDelete)
                }
                );
            }

            model.ListOfWaitingRequestedAdvertisements = waitingCustomerAds;

            #endregion

            #region Lastest Waiting Requested Advertisements

            var waitingEmployeeAds = new List<LastestWaitingEmployeesAdvertisements>();

            var ListOfWaitingOnSaleAdvertisements = await _context.Advertisement.Where(p => !p.IsDelete && p.AdvertisementStatus == AdvertisementStatus.WaitigForConfirm && p.FromEmployee && !p.FromCustomer)
                                                    .OrderByDescending(p => p.CreateDate).Take(10).ToListAsync();

            foreach (var item in ListOfWaitingOnSaleAdvertisements)
            {
                waitingEmployeeAds.Add(new LastestWaitingEmployeesAdvertisements
                {
                    AdvertisementId = item.Id,
                    //AdvertisementTitle = item.Title,
                    CreateDate = item.CreateDate,
                    AdvertisementImage = item.ImageName,
                    AdvertisementAddress = await _context.Addresses.Include(p => p.LocationCountry).FirstOrDefaultAsync(p => p.Id == item.AddressId.Value),
                    Employee = await _context.Users.FirstOrDefaultAsync(p => p.Id == item.UserId && !p.IsDelete)
                }
                );
            }

            model.ListOfWaitingOnSaleAdvertisements = waitingEmployeeAds;

            #endregion

            return model;
        }

        public async Task<EditAdvertisementFromAdminPanel> SetEditAdvertisementFromAdminPanel(ulong Id)
        {
            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            EditAdvertisementFromAdminPanel model = new EditAdvertisementFromAdminPanel()
            {
                AdvertisementID = Ads.Id,
                AddressID = Ads.AddressId,
                UserId = Ads.UserId,
                //Title = Ads.Title,
                AdsImage = Ads.ImageName,
                //AdsUrl = Ads.AdsUrl,
                AdvertisementStatus = Ads.AdvertisementStatus,
                RejectDescription = Ads.DeclineMessage
            };

            var TagAdvertisementModel = await _context.AdvertisementTags.Where(s => s.AdvertisementId == Ads.Id).ToListAsync();

            model.AdvertisementTags = string.Join(",", TagAdvertisementModel.Select(p => p.TagTitle).ToList());

            return model;
        }

        public async Task<FilterAdvertisementAdminSidedViewModel> FilterRequestAdvertisementAdminSide(FilterAdvertisementAdminSidedViewModel filter)
        {
            var query = _context.Advertisement
                            .Include(s => s.User)
                            .Include(s => s.State)
                            .Include(p => p.AdvertisementSelectedCategory)
                            .ThenInclude(p => p.AdvertisementCategory)
                            .Include(p => p.AdvertisementInfo)
                            .ThenInclude(p => p.Language)
                            .AsQueryable();

            #region Sort by Gender

            switch (filter.FilterAdvertisementGender)
            {
                case FilterAdvertisementGender.All:
                    break;
                case FilterAdvertisementGender.OnSale:
                    query = query.Where(s => s.FromEmployee && !s.FromCustomer);
                    break;
                case FilterAdvertisementGender.Requested:
                    query = query.Where(s => !s.FromEmployee && s.FromCustomer);
                    break;
                default:
                    break;
            }

            #endregion

            #region State

            switch (filter.FilterAdvertisementState)
            {
                case FilterAdvertisementState.All:
                    break;
                case FilterAdvertisementState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterAdvertisementState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
                default:
                    break;
            }

            switch (filter.FilterAdvertisementActiveState)
            {
                case FilterAdvertisementActiveState.All:
                    break;
                case FilterAdvertisementActiveState.IsActive:
                    query = query.Where(p => p.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active);
                    break;
                case FilterAdvertisementActiveState.NotActive:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.NotApproved);
                    break;
                case FilterAdvertisementActiveState.Waiting:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.WaitigForConfirm);
                    break;
                case FilterAdvertisementActiveState.Expired:
                    query = query.Where(s => s.EndDate.Value < DateTime.Now);
                    break;
                default:
                    break;
            }

            switch (filter.FilterAdvertisementImageState)
            {
                case FilterAdvertisementImageState.All:
                    break;
                case FilterAdvertisementImageState.WithImage:
                    query = query.Where(s => !string.IsNullOrEmpty(s.ImageName));
                    break;
                case FilterAdvertisementImageState.WithoutImage:
                    query = query.Where(s => string.IsNullOrEmpty(s.ImageName));
                    break;
            }

            #endregion

            #region OrderBy

            switch (filter.FilterAdvertisementOrder)
            {
                case FilterAdvertisementOrder.CreateDate_Dec:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterAdvertisementOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterAdvertisementOrder.StartDate_Dec:
                    query = query.OrderByDescending(s => s.StartDate);
                    break;
                case FilterAdvertisementOrder.StartDate_Asc:
                    query = query.OrderBy(s => s.StartDate);
                    break;
                case FilterAdvertisementOrder.EndDate_Dec:
                    query = query.OrderByDescending(s => s.EndDate);
                    break;
                case FilterAdvertisementOrder.EndDate_Asc:
                    query = query.OrderBy(s => s.EndDate);
                    break;
            }

            #endregion

            #region Filter

            if (filter.SingleStateId != null && filter.SingleStateId != 0)
                query = query.Where(s => s.State.Id == filter.SingleStateId.Value && !s.IsDelete);

            if (filter.Username != null)
                query = query.Where(s => EF.Functions.Like(s.User.Username, $"%{filter.Username.Trim()}%"));

            //if (filter.Title != null)
            //    query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title.Trim()}%") || s.AdvertisementTags.Any(f => f.TagTitle == filter.Title));

            //if (filter.Description != null)
            //    query = query.Where(p => p.Description.Contains(filter.Description));

            if (filter.StartDate.HasValue)
                query = query.Where(p => p.StartDate.Value >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(p => p.EndDate.Value >= filter.EndDate.Value);

            //if (filter.AdsUrl != null)
            //    query = query.Where(p => p.AdsUrl.Contains(filter.AdsUrl.ToLower().Trim()));


            #endregion

            await filter.Paging(query);

            return filter;
        }

        #endregion

        #region Request Advertisements 

        public async Task<bool> DeleteAdvertisementFromUserPanel(ulong advertisementId, ulong userId)
        {
            var ads = await _context.Advertisement.FirstOrDefaultAsync(p => p.Id == advertisementId && p.UserId == userId && !p.IsDelete);

            if (ads == null) return false;

            ads.IsDelete = true;

            var adsInfo = await _context.advertisementInfo.Where(p => p.AdvertisementId == advertisementId
                                        && !p.IsDelete).ToListAsync();

            if (adsInfo != null && adsInfo.Any())
            {
                foreach (var item in adsInfo)
                {
                    item.IsDelete = true;

                    _context.advertisementInfo.Update(item);
                }
            }

            _context.Advertisement.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<FilterRequestAdvertisementViewModel> FilterRequestAdvertisementUserSide(FilterRequestAdvertisementViewModel filter)
        {
            var query = _context.Advertisement
                            .Where(p => p.UserId == filter.UserId && !p.IsDelete
                                && p.FromCustomer && !p.FromEmployee)
                            .Include(s => s.User)
                            .Include(s => s.State)
                            .Include(p => p.AdvertisementSelectedCategory)
                            .ThenInclude(p => p.AdvertisementCategory)
                            .Include(p => p.AdvertisementInfo)
                            .ThenInclude(p => p.Language)
                            .AsQueryable();



            if (filter.JustWithImage)
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.ImageName));
            }

            #region State

            switch (filter.FilterRequestAdvertisementState)
            {
                case FilterRequestAdvertisementState.All:
                    break;
                case FilterRequestAdvertisementState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterRequestAdvertisementState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
                default:
                    break;
            }

            switch (filter.FilterRequestAdvertisementActiveState)
            {
                case FilterRequestAdvertisementActiveState.All:
                    break;
                case FilterRequestAdvertisementActiveState.IsActive:
                    query = query.Where(p => p.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active);
                    break;
                case FilterRequestAdvertisementActiveState.NotActive:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.NotApproved);
                    break;
                case FilterRequestAdvertisementActiveState.Waiting:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.WaitigForConfirm);
                    break;
                case FilterRequestAdvertisementActiveState.Expired:
                    query = query.Where(s => s.EndDate.Value < DateTime.Now);
                    break;
                default:
                    break;
            }

            switch (filter.FilterRequestAdvertisementImageState)
            {
                case FilterRequestAdvertisementImageState.All:
                    break;
                case FilterRequestAdvertisementImageState.WithImage:
                    query = query.Where(s => !string.IsNullOrEmpty(s.ImageName));
                    break;
                case FilterRequestAdvertisementImageState.WithoutImage:
                    query = query.Where(s => string.IsNullOrEmpty(s.ImageName));
                    break;
            }

            #endregion

            #region OrderBy

            switch (filter.FilterRequestAdvertisementOrder)
            {
                case FilterRequestAdvertisementOrder.CreateDate_Dec:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterRequestAdvertisementOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterRequestAdvertisementOrder.StartDate_Dec:
                    query = query.OrderByDescending(s => s.StartDate);
                    break;
                case FilterRequestAdvertisementOrder.StartDate_Asc:
                    query = query.OrderBy(s => s.StartDate);
                    break;
                case FilterRequestAdvertisementOrder.EndDate_Dec:
                    query = query.OrderByDescending(s => s.EndDate);
                    break;
                case FilterRequestAdvertisementOrder.EndDate_Asc:
                    query = query.OrderBy(s => s.EndDate);
                    break;
            }

            #endregion

            #region Filter

            var cat = _context.AdvertisementSelectedCategories
                .Where(s => s.AdsCategoryID == filter.SingleCategoryId)
                .Select(s => s.Id).ToList();

            if (filter.SingleCategoryId != null && filter.SingleCategoryId != 0)
                query = query.Where(s => s.AdvertisementSelectedCategory.Any(f => cat.Contains(f.Id)));


            if (filter.SingleStateId != null && filter.SingleStateId != 0)
                query = query.Where(s => s.State.Id == filter.SingleStateId.Value && !s.IsDelete);


            //if (filter.Title != null)
            //    query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title.Trim()}%") || s.AdvertisementTags.Any(f => f.TagTitle == filter.Title));

            //if (filter.Description != null)
            //    query = query.Where(p => p.Description.Contains(filter.Description));

            if (filter.StartDate.HasValue)
                query = query.Where(p => p.StartDate.Value >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(p => p.EndDate.Value >= filter.EndDate.Value);

            //if (filter.AdsUrl != null)
            //    query = query.Where(p => p.AdsUrl.Contains(filter.AdsUrl.ToLower().Trim()));




            #region Catgeory Filter


            if (filter.CategoryId != null && filter.CategoryId != 0)
            {
                query = _context.AdvertisementSelectedCategories.Where(p => p.AdsCategoryID == filter.CategoryId)
                                                .Include(p => p.Advertisement).ThenInclude(p => p.State).Select(p => p.Advertisement).OrderByDescending(p => p.CreateDate).AsQueryable();
            }
            else
            {

                if (filter.ParentCategoryId != null && filter.ParentCategoryId != 0)
                {

                    query = _context.AdvertisementSelectedCategories.Where(p => p.AdsCategoryID == filter.ParentCategoryId)
                                                .Include(p => p.Advertisement).ThenInclude(p => p.State).Select(p => p.Advertisement).OrderByDescending(p => p.CreateDate).AsQueryable();
                }
            }

            #endregion

            #endregion

            #region Check Expier Date 

            if (filter.SiteSide == true)
            {
                query = query.Where(p => p.StartDate.Value <= DateTime.Now && p.EndDate.Value >= DateTime.Now);
            }

            #endregion

            #region Group

            if (filter.AdvertisementGroups == true)
                query = query.Where(p => p.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active);

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<FilterOnSaleAdvertisementViewModel> FilterOnSaleAdvertisementUserSide(FilterOnSaleAdvertisementViewModel filter)
        {
            var query = _context.Advertisement
                            .Where(p => p.UserId == filter.UserId && !p.IsDelete
                                && !p.FromCustomer && p.FromEmployee)
                            .Include(s => s.User)
                            .Include(s => s.State)
                            .Include(p => p.AdvertisementSelectedCategory)
                            .ThenInclude(p => p.AdvertisementCategory)
                            .Include(p => p.AdvertisementInfo)
                            .ThenInclude(p => p.Language)
                            .AsQueryable();



            if (filter.JustWithImage)
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.ImageName));
            }

            #region State

            switch (filter.FilterOnSaleAdvertisementState)
            {
                case FilterOnSaleAdvertisementState.All:
                    break;
                case FilterOnSaleAdvertisementState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterOnSaleAdvertisementState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
                default:
                    break;
            }

            switch (filter.FilterOnSaleAdvertisementActiveState)
            {
                case FilterOnSaleAdvertisementActiveState.All:
                    break;
                case FilterOnSaleAdvertisementActiveState.IsActive:
                    query = query.Where(p => p.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active);
                    break;
                case FilterOnSaleAdvertisementActiveState.NotActive:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.NotApproved);
                    break;
                case FilterOnSaleAdvertisementActiveState.Waiting:
                    query = query.Where(s => s.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.WaitigForConfirm);
                    break;
                case FilterOnSaleAdvertisementActiveState.Expired:
                    query = query.Where(s => s.EndDate.Value < DateTime.Now);
                    break;
                default:
                    break;
            }

            switch (filter.FilterOnSaleAdvertisementImageState)
            {
                case FilterOnSaleAdvertisementImageState.All:
                    break;
                case FilterOnSaleAdvertisementImageState.WithImage:
                    query = query.Where(s => !string.IsNullOrEmpty(s.ImageName));
                    break;
                case FilterOnSaleAdvertisementImageState.WithoutImage:
                    query = query.Where(s => string.IsNullOrEmpty(s.ImageName));
                    break;
            }

            #endregion

            #region OrderBy

            switch (filter.FilterOnSaleAdvertisementOrder)
            {
                case FilterOnSaleAdvertisementOrder.CreateDate_Dec:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterOnSaleAdvertisementOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterOnSaleAdvertisementOrder.StartDate_Dec:
                    query = query.OrderByDescending(s => s.StartDate);
                    break;
                case FilterOnSaleAdvertisementOrder.StartDate_Asc:
                    query = query.OrderBy(s => s.StartDate);
                    break;
                case FilterOnSaleAdvertisementOrder.EndDate_Dec:
                    query = query.OrderByDescending(s => s.EndDate);
                    break;
                case FilterOnSaleAdvertisementOrder.EndDate_Asc:
                    query = query.OrderBy(s => s.EndDate);
                    break;
            }

            #endregion

            #region Filter

            var cat = _context.AdvertisementSelectedCategories
                .Where(s => s.AdsCategoryID == filter.SingleCategoryId)
                .Select(s => s.Id).ToList();

            if (filter.SingleCategoryId != null && filter.SingleCategoryId != 0)
                query = query.Where(s => s.AdvertisementSelectedCategory.Any(f => cat.Contains(f.Id)));


            if (filter.SingleStateId != null && filter.SingleStateId != 0)
                query = query.Where(s => s.State.Id == filter.SingleStateId.Value && !s.IsDelete);


            //if (filter.Title != null)
            //    query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title.Trim()}%") || s.AdvertisementTags.Any(f => f.TagTitle == filter.Title));

            //if (filter.Description != null)
            //    query = query.Where(p => p.Description.Contains(filter.Description));

            if (filter.StartDate.HasValue)
                query = query.Where(p => p.StartDate.Value >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(p => p.EndDate.Value >= filter.EndDate.Value);

            //if (filter.AdsUrl != null)
            //    query = query.Where(p => p.AdsUrl.Contains(filter.AdsUrl.ToLower().Trim()));




            #region Catgeory Filter


            if (filter.CategoryId != null && filter.CategoryId != 0)
            {
                query = _context.AdvertisementSelectedCategories.Where(p => p.AdsCategoryID == filter.CategoryId)
                                                .Include(p => p.Advertisement).ThenInclude(p => p.State).Select(p => p.Advertisement).OrderByDescending(p => p.CreateDate).AsQueryable();
            }
            else
            {

                if (filter.ParentCategoryId != null && filter.ParentCategoryId != 0)
                {

                    query = _context.AdvertisementSelectedCategories.Where(p => p.AdsCategoryID == filter.ParentCategoryId)
                                                .Include(p => p.Advertisement).ThenInclude(p => p.State).Select(p => p.Advertisement).OrderByDescending(p => p.CreateDate).AsQueryable();
                }
            }

            #endregion

            #endregion

            #region Check Expier Date 

            if (filter.SiteSide == true)
            {
                query = query.Where(p => p.StartDate.Value <= DateTime.Now && p.EndDate.Value >= DateTime.Now);
            }

            #endregion

            #region Group

            if (filter.AdvertisementGroups == true)
                query = query.Where(p => p.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active);

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<CreateAdvertisementFromUserPanelResult> AddOnSaleAdvertisementFromUserPanell(CreateOnSaleAdvertisementFromUserPanel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            if (upload_imgs.Count > 10)
            {
                return CreateAdvertisementFromUserPanelResult.ImageCountNotValid;
            }

            #region Advertisement Properties

            Advertisement ads = new Advertisement()
            {
                AddressId = model.AddressID,
                UserId = (ulong)model.UserId,

                ImageName = null,
                VisitCount = 0,
                CreateDate = DateTime.Now,
                AdvertisementStatus = AdvertisementStatus.WaitigForConfirm,
                FromCustomer = false,
                FromEmployee = true,
            };

            #endregion

            #region Advertisement Image

            if (upload_imgs != null && upload_imgs.Any())
            {
                foreach (var item in upload_imgs)
                {
                    if (item.IsImage())
                    {
                        var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                        item.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150, PathTools.AdvertisementImageServerThumb);
                        ads.ImageName = imageName;

                    }

                    if (item != null && !item.IsImage())
                    {
                        return CreateAdvertisementFromUserPanelResult.ImageIsNotExist;
                    }
                }
            }
            else
            {
                return CreateAdvertisementFromUserPanelResult.ImageIsNotValid;
            }

            await _context.Advertisement.AddAsync(ads);
            await _context.SaveChangesAsync();

            #endregion

            #region Add Advertisement Category

            foreach (var item in SelectedCategory)
            {
                AdvertisementSelectedCategory Category = new AdvertisementSelectedCategory()
                {
                    AdsCategoryID = item,
                    AdvertisementID = ads.Id
                };

                await _context.AdvertisementSelectedCategories.AddAsync(Category);
            }

            await _context.SaveChangesAsync();

            #endregion

            #region Advertisement Tags

            if (!string.IsNullOrEmpty(model.AdvertisementTags))
            {
                List<string> tagsList = model.AdvertisementTags.Split(',').ToList<string>();
                foreach (var itemTag in tagsList)
                {
                    var newTag = new AdvertisementTag
                    {
                        AdvertisementId = ads.Id,
                        TagTitle = itemTag,
                        IsDelete = false,
                        CreateDate = DateTime.Now
                    };
                    await _context.AdvertisementTags.AddAsync(newTag);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            #region Advertisement Info Properties

            AdvertisementInfo adsInfo = new AdvertisementInfo()
            {
                AdvertisementId = ads.Id,
                Lang_Id = lang,
                Title = model.Title.SanitizeText(),
                Description = model.Description.ConvertNewLineToBr().SanitizeText(),
                AdsUrl = model.AdsUrl.SanitizeText(),
                CreateDate = DateTime.Now
            };

            await _context.advertisementInfo.AddAsync(adsInfo);
            await _context.SaveChangesAsync();

            #endregion


            return CreateAdvertisementFromUserPanelResult.Success;
        }


        public async Task<CreateAdvertisementFromUserPanelResult> AddAdvertisementFromUserPanell(CreateRequestAdvertisementFromUserPanel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            string lang = CultureInfo.CurrentCulture.Name;

            if (upload_imgs.Count > 10)
            {
                return CreateAdvertisementFromUserPanelResult.ImageCountNotValid;
            }

            #region Advertisement Properties

            Advertisement ads = new Advertisement()
            {
                AddressId = model.AddressID,
                UserId = (ulong)model.UserId,
                ImageName = null,
                VisitCount = 0,
                CreateDate = DateTime.Now,
                AdvertisementStatus = AdvertisementStatus.WaitigForConfirm,
                FromCustomer = true,
                FromEmployee = false,
            };

            #endregion

            #region Advertisement Image

            if (upload_imgs != null && upload_imgs.Any())
            {
                foreach (var item in upload_imgs)
                {
                    if (item.IsImage())
                    {
                        var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                        item.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150, PathTools.AdvertisementImageServerThumb);
                        ads.ImageName = imageName;

                    }

                    if (item != null && !item.IsImage())
                    {
                        return CreateAdvertisementFromUserPanelResult.ImageIsNotExist;
                    }
                }
            }
            else
            {
                return CreateAdvertisementFromUserPanelResult.ImageIsNotValid;
            }

            #endregion

            await _context.Advertisement.AddAsync(ads);
            await _context.SaveChangesAsync();

            #region Add Advertisement Category

            foreach (var item in SelectedCategory)
            {
                AdvertisementSelectedCategory Category = new AdvertisementSelectedCategory()
                {
                    AdsCategoryID = item,
                    AdvertisementID = ads.Id
                };

                await _context.AdvertisementSelectedCategories.AddAsync(Category);
            }

            await _context.SaveChangesAsync();

            #endregion

            #region Advertisement Tags

            if (!string.IsNullOrEmpty(model.AdvertisementTags))
            {
                List<string> tagsList = model.AdvertisementTags.Split(',').ToList<string>();
                foreach (var itemTag in tagsList)
                {
                    var newTag = new AdvertisementTag
                    {
                        AdvertisementId = ads.Id,
                        TagTitle = itemTag,
                        IsDelete = false,
                        CreateDate = DateTime.Now
                    };
                    await _context.AdvertisementTags.AddAsync(newTag);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            #region Advertisement Info Properties

            AdvertisementInfo adsInfo = new AdvertisementInfo()
            {
                AdvertisementId = ads.Id,
                Lang_Id = lang,
                Title = model.Title.SanitizeText(),
                Description = model.Description.ConvertNewLineToBr().SanitizeText(),
                AdsUrl = model.AdsUrl.SanitizeText(),
                CreateDate = DateTime.Now
            };

            await _context.advertisementInfo.AddAsync(adsInfo);
            await _context.SaveChangesAsync();

            #endregion

            return CreateAdvertisementFromUserPanelResult.Success;
        }

        public async Task<EditOnSaleAdvertisementFromUserPanel> SetEditOnSaleAdvertisementFromUserPanel(ulong Id)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .SingleOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            #region Fill View Model

            EditOnSaleAdvertisementFromUserPanel model = new EditOnSaleAdvertisementFromUserPanel()
            {
                AdvertisementID = Ads.Id,
                AddressID = Ads.AddressId,
                UserId = Ads.UserId,
                AdsImage = Ads.ImageName,
                AdvertisementStatus = Ads.AdvertisementStatus,
                FromCustomer = Ads.FromCustomer,
                FromEmployee = Ads.FromEmployee,
                DeclineMessage = Ads.DeclineMessage
            };

            #endregion

            #region Advertisement Info

            var adsInfo = await _context.advertisementInfo.FirstOrDefaultAsync(p => p.AdvertisementId == Ads.Id
                                                        && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo != null)
            {
                model.Title = adsInfo.Title;
                model.Description = adsInfo.Description;
                model.AdsUrl = adsInfo.AdsUrl;
            }

            #endregion

            #region Advertise,emt Tags

            var TagAdvertisementModel = await _context.AdvertisementTags
                                                .Where(s => s.AdvertisementId == Id)
                                                .ToListAsync();

            model.AdvertisementTags = string.Join(",", TagAdvertisementModel.Select(p => p.TagTitle).ToList());

            #endregion

            return model;
        }


        public async Task<EditRequestAdvertisementFromUserPanel> SetEditAdvertisementFromUserPanel(ulong Id)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            #region Fill View Model

            EditRequestAdvertisementFromUserPanel model = new EditRequestAdvertisementFromUserPanel()
            {
                AdvertisementID = Ads.Id,
                AddressID = Ads.AddressId,
                UserId = Ads.UserId,
                AdsImage = Ads.ImageName,
                AdvertisementStatus = Ads.AdvertisementStatus,
                FromCustomer = Ads.FromCustomer,
                FromEmployee = Ads.FromEmployee,
                DeclineMessage = Ads.DeclineMessage
            };

            #endregion

            #region Advertisement Info

            var adsInfo = await _context.advertisementInfo.FirstOrDefaultAsync(p => p.AdvertisementId == Ads.Id
                                                        && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo != null)
            {
                model.Title = adsInfo.Title;
                model.Description = adsInfo.Description;
                model.AdsUrl = adsInfo.AdsUrl;
            }

            #endregion

            #region Advertisenemt Tags

            var TagAdvertisementModel = await _context.AdvertisementTags
                                                .Where(s => s.AdvertisementId == Id)
                                                .ToListAsync();

            model.AdvertisementTags = string.Join(",", TagAdvertisementModel.Select(p => p.TagTitle).ToList());

            #endregion

            return model;
        }

        public async Task<EditAdvertisementFromAdminPanelResult> UpdateAdvertisement(EditAdvertisementFromAdminPanel model, IFormFile? ImageName, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .SingleOrDefaultAsync(p => p.Id == model.AdvertisementID);

            if (Ads == null) return EditAdvertisementFromAdminPanelResult.NotFound;

            #region Properties

            //Ads.Title = model.Title.SanitizeText();
            //Ads.Description = model.Description.ConvertNewLineToBr();
            //Ads.AdsUrl = model.AdsUrl.SanitizeText();
            Ads.AdvertisementStatus = model.AdvertisementStatus;

            #endregion

            #region Advertisement Info 

            var adsInfo = await _context.advertisementInfo.FirstOrDefaultAsync(p => p.AdvertisementId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                AdvertisementInfo advertisementInfo = new AdvertisementInfo()
                {
                    AdvertisementId = Ads.Id,
                    Lang_Id = lang,
                    CreateDate = DateTime.Now
                };

                await _context.advertisementInfo.AddAsync(advertisementInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.advertisementInfo.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region About Change Status

            if (model.AdvertisementStatus == AdvertisementStatus.Active)
            {
                Ads.DeclineMessage = null;
            }
            if (model.AdvertisementStatus == AdvertisementStatus.WaitigForConfirm)
            {
                Ads.DeclineMessage = null;
            }
            if (model.AdvertisementStatus == AdvertisementStatus.NotApproved)
            {
                Ads.DeclineMessage = model.RejectDescription;
            }

            #endregion

            #region Expire Date

            if (model.AdvertisementStatus == AdvertisementStatus.Active && Ads.StartDate == null && Ads.EndDate == null)
            {
                Ads.StartDate = DateTime.Now;
                Ads.EndDate = DateTime.Now.AddDays(7);
            }

            #endregion

            #region Image Part

            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.ImageName != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementimageServerOriginThumb, Ads.ImageName);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementImageServerThumb);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }

            }

            if (ImageName == null && string.IsNullOrEmpty(model.AdsImage))
            {
                return EditAdvertisementFromAdminPanelResult.ImageIsNotFound;
            }

            #endregion

            #region Categories

            var selected = await _context.AdvertisementSelectedCategories.Where(p => p.AdvertisementID == model.AdvertisementID).ToListAsync();

            foreach (var item in selected)
            {
                _context.AdvertisementSelectedCategories.Remove(item);
            }

            foreach (var item in SelectedCategory)
            {
                AdvertisementSelectedCategory Category = new AdvertisementSelectedCategory()
                {
                    AdsCategoryID = item,
                    AdvertisementID = Ads.Id
                };

                await _context.AdvertisementSelectedCategories.AddAsync(Category);
            }

            #endregion

            #region Advertisement Tags

            var TagAdvertisementModel = await _context.AdvertisementTags
                .Where(s => s.AdvertisementId == model.AdvertisementID)
                .ToListAsync();

            if (!string.IsNullOrEmpty(model.AdvertisementTags))
            {
                foreach (var item in TagAdvertisementModel)
                {
                    _context.AdvertisementTags.Remove(item);
                }
                List<string> tagsList = model.AdvertisementTags.Split(',').ToList<string>();
                foreach (var itemTag in tagsList)
                {
                    var newTag = new AdvertisementTag
                    {
                        AdvertisementId = model.AdvertisementID,
                        TagTitle = itemTag,
                        IsDelete = false,
                        CreateDate = DateTime.Now
                    };
                    await _context.AdvertisementTags.AddAsync(newTag);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            _context.Advertisement.Update(Ads);
            await _context.SaveChangesAsync();

            return EditAdvertisementFromAdminPanelResult.Success;
        }


        public async Task<List<ulong>> GetAllAdvertisementCategories(ulong Id)
        {
            return await _context.AdvertisementSelectedCategories
                .Where(p => p.AdvertisementID == Id)
                .Select(p => p.AdsCategoryID)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(p => p.Id == AddressId);
        }

        public async Task<Domain.Entities.Advertisement.Advertisement?> GetAdvertisementByID(ulong Id)
        {
            return await _context.Advertisement.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<EditRequestAdvertisementFromUserPanelResualt> EditRequestAdvertisementFromUserPanel(EditRequestAdvertisementFromUserPanel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .FirstOrDefaultAsync(p => p.Id == model.AdvertisementID && p.UserId == model.UserId && !p.IsDelete);

            if (Ads == null)
            {
                return EditRequestAdvertisementFromUserPanelResualt.NotFound;
            }

            #region Properties

            //Ads.Title = model.Title.SanitizeText();
            //Ads.Description = model.Description.ConvertNewLineToBr();
            //Ads.AdsUrl = model.AdsUrl.SanitizeText();
            Ads.AdvertisementStatus = AdvertisementStatus.WaitigForConfirm;

            #endregion

            #region Advertisement Info 

            var adsInfo = await _context.advertisementInfo.FirstOrDefaultAsync(p => p.AdvertisementId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                AdvertisementInfo advertisementInfo = new AdvertisementInfo()
                {
                    AdvertisementId = Ads.Id,
                    Lang_Id = lang,
                    Description = model.Description.SanitizeText(),
                    Title = model.Title.SanitizeText(),
                    AdsUrl = model.AdsUrl.SanitizeText(),
                    CreateDate = DateTime.Now
                };

                await _context.advertisementInfo.AddAsync(advertisementInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                adsInfo.Title = model.Title.SanitizeText();
                adsInfo.Description = model.Description.SanitizeText();
                adsInfo.AdsUrl = model.AdsUrl.SanitizeText();

                _context.advertisementInfo.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Image Part

            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.ImageName != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementimageServerOriginThumb, Ads.ImageName);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementImageServerThumb);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }

            }

            if (ImageName != null && !ImageName.IsImage())
            {
                return EditRequestAdvertisementFromUserPanelResualt.ImageIsNotValid;
            }

            if (ImageName == null && string.IsNullOrEmpty(model.AdsImage))
            {
                return EditRequestAdvertisementFromUserPanelResualt.ImageIsNotFound;
            }

            #endregion

            #region Categories

            var selected = await _context.AdvertisementSelectedCategories.Where(p => p.AdvertisementID == model.AdvertisementID).ToListAsync();

            foreach (var item in selected)
            {
                _context.AdvertisementSelectedCategories.Remove(item);
            }

            foreach (var item in SelectedCategory)
            {
                AdvertisementSelectedCategory Category = new AdvertisementSelectedCategory()
                {
                    AdsCategoryID = item,
                    AdvertisementID = Ads.Id
                };

                await _context.AdvertisementSelectedCategories.AddAsync(Category);
            }

            #endregion

            #region Advertisement Tags

            var TagAdvertisementModel = await _context.AdvertisementTags
                .Where(s => s.AdvertisementId == model.AdvertisementID)
                .ToListAsync();

            if (!string.IsNullOrEmpty(model.AdvertisementTags))
            {
                foreach (var item in TagAdvertisementModel)
                {
                    _context.AdvertisementTags.Remove(item);
                }
                List<string> tagsList = model.AdvertisementTags.Split(',').ToList<string>();
                foreach (var itemTag in tagsList)
                {
                    var newTag = new AdvertisementTag
                    {
                        AdvertisementId = model.AdvertisementID,
                        TagTitle = itemTag,
                        IsDelete = false,
                        CreateDate = DateTime.Now
                    };
                    await _context.AdvertisementTags.AddAsync(newTag);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            _context.Advertisement.Update(Ads);
            await _context.SaveChangesAsync();

            return EditRequestAdvertisementFromUserPanelResualt.Success;
        }

        public async Task<EditOnSaleAdvertisementFromUserPanelResualt> EditOnSaleAdvertisementFromUserPanel(EditOnSaleAdvertisementFromUserPanel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Advertisement
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .Include(s => s.AdvertisementTags)
                .FirstOrDefaultAsync(p => p.Id == model.AdvertisementID && p.UserId == model.UserId && !p.IsDelete);

            if (Ads == null)
            {
                return EditOnSaleAdvertisementFromUserPanelResualt.NotFound;
            }

            #region Properties

            //Ads.Title = model.Title.SanitizeText();
            //Ads.Description = model.Description.ConvertNewLineToBr();
            //Ads.AdsUrl = model.AdsUrl.SanitizeText();
            Ads.AdvertisementStatus = AdvertisementStatus.WaitigForConfirm;

            #endregion

            #region Advertisement Info 

            var adsInfo = await _context.advertisementInfo.FirstOrDefaultAsync(p => p.AdvertisementId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                AdvertisementInfo advertisementInfo = new AdvertisementInfo()
                {
                    AdvertisementId = Ads.Id,
                    Lang_Id = lang,
                    Description = model.Description.SanitizeText(),
                    Title = model.Title.SanitizeText(),
                    AdsUrl = model.AdsUrl.SanitizeText(),
                    CreateDate = DateTime.Now
                };

                await _context.advertisementInfo.AddAsync(advertisementInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                adsInfo.Title = model.Title.SanitizeText();
                adsInfo.Description = model.Description.SanitizeText();
                adsInfo.AdsUrl = model.AdsUrl.SanitizeText();

                _context.advertisementInfo.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Image Part

            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.ImageName != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementimageServerOriginThumb, Ads.ImageName);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.AdvertisementimageServerOrigin, 150, 150
                     , PathTools.AdvertisementImageServerThumb);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }

            }

            if (ImageName != null && !ImageName.IsImage())
            {
                return EditOnSaleAdvertisementFromUserPanelResualt.ImageIsNotValid;
            }

            if (ImageName == null && string.IsNullOrEmpty(model.AdsImage))
            {
                return EditOnSaleAdvertisementFromUserPanelResualt.ImageIsNotFound;
            }

            #endregion

            #region Categories

            var selected = await _context.AdvertisementSelectedCategories.Where(p => p.AdvertisementID == model.AdvertisementID).ToListAsync();

            foreach (var item in selected)
            {
                _context.AdvertisementSelectedCategories.Remove(item);
            }

            foreach (var item in SelectedCategory)
            {
                AdvertisementSelectedCategory Category = new AdvertisementSelectedCategory()
                {
                    AdsCategoryID = item,
                    AdvertisementID = Ads.Id
                };

                await _context.AdvertisementSelectedCategories.AddAsync(Category);
            }

            #endregion

            #region Advertisement Tags

            var TagAdvertisementModel = await _context.AdvertisementTags
                .Where(s => s.AdvertisementId == model.AdvertisementID)
                .ToListAsync();

            if (!string.IsNullOrEmpty(model.AdvertisementTags))
            {
                foreach (var item in TagAdvertisementModel)
                {
                    _context.AdvertisementTags.Remove(item);
                }
                List<string> tagsList = model.AdvertisementTags.Split(',').ToList<string>();
                foreach (var itemTag in tagsList)
                {
                    var newTag = new AdvertisementTag
                    {
                        AdvertisementId = model.AdvertisementID,
                        TagTitle = itemTag,
                        IsDelete = false,
                        CreateDate = DateTime.Now
                    };
                    await _context.AdvertisementTags.AddAsync(newTag);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            _context.Advertisement.Update(Ads);
            await _context.SaveChangesAsync();

            return EditOnSaleAdvertisementFromUserPanelResualt.Success;
        }


        #endregion

        #region Site Side 

        //List Of Customer Advertisements
        public async Task<List<ListOfCustomerAdvertisementViewModel>> ListOfCustomerAdvertisementViewModel(string culture , ulong? categoryId)
        {
            #region filter properties

            if (categoryId.HasValue)
            {
                var returnModel = await _context.AdvertisementSelectedCategories.Include(p => p.Advertisement).ThenInclude(p => p.AdvertisementInfo)
                            .Include(p => p.AdvertisementCategory).Where(p => p.AdsCategoryID == categoryId.Value && !p.IsDelete && p.Advertisement.AdvertisementStatus == AdvertisementStatus.Active && !p.Advertisement.FromEmployee && p.Advertisement.FromCustomer
                                && p.Advertisement.StartDate.Value.Year <= DateTime.Now.Year && p.Advertisement.StartDate.Value.DayOfYear <= DateTime.Now.DayOfYear
                                && p.Advertisement.EndDate.Value.Year >= DateTime.Now.Year && p.Advertisement.EndDate.Value.DayOfYear >= DateTime.Now.DayOfYear).ToListAsync();

                var returnModel1 = new List<ListOfCustomerAdvertisementViewModel>();

                foreach (var item in returnModel)
                {
                    returnModel1.Add(new ListOfCustomerAdvertisementViewModel
                    {
                        AdvertisementId = item.Advertisement.Id,
                        AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                        CreateDate = item.Advertisement.CreateDate,
                        Image = item.Advertisement.ImageName,
                    });
                }

                return returnModel1;
            }

            #endregion

            #region Get Current Advertisements

            var advertisement = await _context.advertisementInfo
                        .Include(p => p.Advertisement)
                        .ThenInclude(p => p.AdvertisementSelectedCategory)
                        .ThenInclude(p => p.AdvertisementCategory)
                        .Where(p => !p.IsDelete && p.Advertisement.AdvertisementStatus == AdvertisementStatus.Active && !p.Advertisement.FromEmployee && p.Advertisement.FromCustomer
                               && p.Advertisement.StartDate.Value.Year <= DateTime.Now.Year && p.Advertisement.StartDate.Value.DayOfYear <= DateTime.Now.DayOfYear
                               && p.Advertisement.EndDate.Value.Year >= DateTime.Now.Year && p.Advertisement.EndDate.Value.DayOfYear >= DateTime.Now.DayOfYear
                               && p.Lang_Id == culture)
                        .Select(p => p.Advertisement).ToListAsync();

            #region filter properties



            #endregion

            #endregion

            #region model

            var model = new List<ListOfCustomerAdvertisementViewModel>();

            foreach (var item in advertisement)
            {
                model.Add(new ListOfCustomerAdvertisementViewModel
                {
                    AdvertisementId = item.Id,
                    AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    Image = item.ImageName,
                });
            }

            return model;

            #endregion
        }

        //List Of Employee Advertisements
        public async Task<List<ListOfSaleAdvertisementViewModel>> ListOfSaleAdvertisementViewModel(string culture  , ulong? categoryId)
        {
            #region filter properties

            if (categoryId.HasValue)
            {
                var returnModel = await _context.AdvertisementSelectedCategories.Include(p => p.Advertisement).ThenInclude(p => p.AdvertisementInfo)
                            .Include(p => p.AdvertisementCategory).Where(p => p.AdsCategoryID == categoryId.Value && !p.IsDelete && p.Advertisement.AdvertisementStatus == AdvertisementStatus.Active && p.Advertisement.FromEmployee && !p.Advertisement.FromCustomer
                                && p.Advertisement.StartDate.Value.Year <= DateTime.Now.Year && p.Advertisement.StartDate.Value.DayOfYear <= DateTime.Now.DayOfYear
                                && p.Advertisement.EndDate.Value.Year >= DateTime.Now.Year && p.Advertisement.EndDate.Value.DayOfYear >= DateTime.Now.DayOfYear).ToListAsync();

                var returnModel1 = new List<ListOfSaleAdvertisementViewModel>();

                foreach (var item in returnModel)
                {
                    returnModel1.Add(new ListOfSaleAdvertisementViewModel
                    {
                        AdvertisementId = item.Advertisement.Id,
                        AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                        CreateDate = item.Advertisement.CreateDate,
                        Image = item.Advertisement.ImageName,
                    });
                }

                return returnModel1;
            }

            #endregion


            #region Get Current Advertisements

            var advertisement = await _context.advertisementInfo
                        .Include(p => p.Advertisement)
                        .ThenInclude(p => p.AdvertisementSelectedCategory)
                        .ThenInclude(p => p.AdvertisementCategory)
                        .Where(p => !p.IsDelete && p.Advertisement.AdvertisementStatus == AdvertisementStatus.Active && p.Advertisement.FromEmployee && !p.Advertisement.FromCustomer
                               && p.Advertisement.StartDate.Value.Year <= DateTime.Now.Year && p.Advertisement.StartDate.Value.DayOfYear <= DateTime.Now.DayOfYear
                               && p.Advertisement.EndDate.Value.Year >= DateTime.Now.Year && p.Advertisement.EndDate.Value.DayOfYear >= DateTime.Now.DayOfYear
                               && p.Lang_Id == culture)
                        .Select(p => p.Advertisement).ToListAsync();

            #endregion

            #region model

            var model = new List<ListOfSaleAdvertisementViewModel>();

            foreach (var item in advertisement)
            {
                model.Add(new ListOfSaleAdvertisementViewModel
                {
                    AdvertisementId = item.Id,
                    AdvertisementTitle = await _context.advertisementInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.AdvertisementId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    Image = item.ImageName,
                });
            }

            return model;

            #endregion
        }

        //Filter Sale Advertisement Site Side 
        public async Task<FilterSaleAdvertisementViewModel> FilterSaleAdvertisementViewModel(FilterSaleAdvertisementViewModel filter)
        {
            var query = _context.advertisementInfo
                            .Include(p => p.Advertisement)
                            .ThenInclude(p => p.AdvertisementSelectedCategory)
                            .ThenInclude(p => p.AdvertisementCategory)
                            .Where(p => !p.IsDelete && p.Advertisement.AdvertisementStatus == AdvertisementStatus.Active && p.Advertisement.FromEmployee && !p.Advertisement.FromCustomer
                                   && p.Advertisement.StartDate.Value.Year <= DateTime.Now.Year && p.Advertisement.StartDate.Value.DayOfYear <= DateTime.Now.DayOfYear
                                   && p.Advertisement.EndDate.Value.Year >= DateTime.Now.Year && p.Advertisement.EndDate.Value.DayOfYear >= DateTime.Now.DayOfYear
                                   && p.Lang_Id == filter.LanguageId)
                            .Select(p => p.Advertisement)
                            .AsQueryable();

            #region Filter



            #endregion

            await filter.Paging(query);

            return filter;
        }


        #endregion

    }
}
