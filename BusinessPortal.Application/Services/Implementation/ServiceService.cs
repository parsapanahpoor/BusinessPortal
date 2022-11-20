using AngleSharp.Css;
using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using BusinessPortal.Domain.ViewModels.Site.Product;
using BusinessPortal.Domain.ViewModels.Site.Services;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.ProductService;
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
    public class ServiceService : IServiceService
    {
        #region ctor

        private readonly IServiceRepository _serviceRepository;
        private readonly BusinessPortalDbContext _context;

        public ServiceService(IServiceRepository serviceRepository, BusinessPortalDbContext context)
        {
            _serviceRepository = serviceRepository;
            _context = context;
        }

        #endregion

        #region Admin 

        #region Service Category

        public async Task<FilterServiceCategoryViewModel> FilterServiceCategory(FilterServiceCategoryViewModel filter)
        {
            return await _serviceRepository.FilterServiceCategory(filter);
        }

        //Get Service Category By Service Category Id 
        public async Task<ServicesCategory?> GetServiceCategoryById(ulong serviceCategoryId)
        {
            return await _serviceRepository.GetServiceCategoryById(serviceCategoryId);
        }

        //Get Main Categories For Show In Site Header
        public async Task<List<ServicesCategoryInfo>> GetMaiCategoriesForShowInSiteHeader()
        {
            return await _serviceRepository.GetMaiCategoriesForShowInSiteHeader();
        }

        //Create Service Category
        public async Task<CreateServicecCategoryResult> CreateServiceCategory(CreateServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage)
        {
            #region Is Exist Service Category By Unique Name

            if (await _serviceRepository.IsExistServiceCategoryByUniqueName(serviceCategory.UniqueName))
            {
                return CreateServicecCategoryResult.UniqNameIsExist;
            }

            #endregion

            #region Add Service Category

            var mainServiceCategory = new ServicesCategory()
            {
                UniqueName = serviceCategory.UniqueName.SanitizeText(),
                IsDelete = false,
                IsActive = true
            };

            #region Add Image 

            if (serviceCategoryImage != null && serviceCategoryImage.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(serviceCategoryImage.FileName);
                serviceCategoryImage.AddImageToServer(imageName, PathTools.ServiceCategoryimageServerOrigin, 400, 300, PathTools.ServiceCategoryImageServerThumb);
                mainServiceCategory.ServiceCategoryImage = imageName;
            }

            #endregion

            if (serviceCategory.ParentId != null && serviceCategory.ParentId != 0)
            {
                if (await _serviceRepository.IsExistServiceCategoryById(serviceCategory.ParentId.Value))
                {
                    mainServiceCategory.ParentId = serviceCategory.ParentId;
                }
                else
                {
                    return CreateServicecCategoryResult.Fail;
                }
            }

            var serviceCategoryId = await _serviceRepository.AddServiceCategory(mainServiceCategory);

            #endregion

            #region Add LocationInfo

            var serviceCategoryInfo = new List<ServicesCategoryInfo>();

            foreach (var culture in serviceCategory.ServiceCategoryInfos)
            {
                var serviceCategoryInfos = new ServicesCategoryInfo
                {
                    ServicesCategoryId = serviceCategoryId,
                    LanguageId = culture.Culture,
                    Title = culture.Title.SanitizeText(),
                    CreateDate = DateTime.Now,
                };

                serviceCategoryInfo.Add(serviceCategoryInfos);
            }

            await _serviceRepository.AddServiceCategoryInfo(serviceCategoryInfo);

            #endregion

            return CreateServicecCategoryResult.Success;
        }

        //Fill Edit Service Category Info
        public async Task<EditServiceCategoryViewModel?> FillServiceArticleCategoryViewModel(ulong serviceCategoryId)
        {
            return await _serviceRepository.FillServiceArticleCategoryViewModel(serviceCategoryId);
        }

        //Edit Service Group
        public async Task<EditServiceCategoryResult> EditService(EditServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage)
        {
            #region Get Service Category By Id

            var serviceCat = await _serviceRepository.GetServiceCategoryById(serviceCategory.Id);

            if (serviceCat == null) return EditServiceCategoryResult.Fail;

            #endregion

            #region Is Exist Service Category By Unique Name

            if (serviceCat.UniqueName != serviceCategory.UniqueName)
            {
                if (await _serviceRepository.IsExistServiceCategoryByUniqueName(serviceCategory.UniqueName))
                {
                    return EditServiceCategoryResult.UniqNameIsExist;
                }
            }

            #endregion

            #region Is Exist Service Category By Parent Id

            if (serviceCategory.ParentId != null && serviceCategory.ParentId != 0)
            {
                if (!await _serviceRepository.IsExistServiceCategoryById(serviceCategory.ParentId.Value))
                {
                    return EditServiceCategoryResult.Fail;
                }
            }

            #endregion

            #region Update Service Category

            serviceCat.UniqueName = serviceCategory.UniqueName.SanitizeText();
            serviceCat.IsActive = true;

            if (serviceCategoryImage != null && serviceCategoryImage.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(serviceCategoryImage.FileName);
                serviceCategoryImage.AddImageToServer(imageName, PathTools.ServiceCategoryimageServerOrigin, 400, 300, PathTools.ServiceCategoryImageServerThumb);

                if (!string.IsNullOrEmpty(serviceCat.ServiceCategoryImage))
                {
                    serviceCat.ServiceCategoryImage.DeleteImage(PathTools.ServiceCategoryimageServerOrigin, PathTools.ServiceCategoryImageServerThumb);
                }

                serviceCat.ServiceCategoryImage = imageName;
            }

            _serviceRepository.UpdateServiceCategory(serviceCat);

            #endregion

            #region Service Info 

            foreach (var serviceCategoryInfo in serviceCat.ServicesCategoryInfo)
            {
                var updatedInfo = serviceCategory.ServiceCategoryInfos.FirstOrDefault(p => p.Culture == serviceCategoryInfo.LanguageId);

                if (updatedInfo != null)
                {
                    serviceCategoryInfo.Title = updatedInfo.Title.SanitizeText();
                }

                _serviceRepository.UpdateServiceCategoryInfo(serviceCategoryInfo);
            }

            #endregion

            await _serviceRepository.Savechanges();

            return EditServiceCategoryResult.Success;
        }

        //Delete Service Category
        public async Task<bool> DeleteServiceCategory(ulong serviceCategoryId)
        {
            //Get Service Category By Id
            var serviceCategory = await _serviceRepository.GetServiceCategoryById(serviceCategoryId);

            if (serviceCategory == null) return false;

            //Delete Service And Service Category Info
            await _serviceRepository.DeleteServiceCategory(serviceCategory);

            return true;
        }

        #endregion

        #endregion

        #region Product

        #region User Panel

        public async Task<List<CreateProductServiceViewModel>> FillCreateProductServiceViewModel()
        {
            return await _serviceRepository.FillCreateProductServiceViewModel();
        }

        //Add Product Service From User Panel
        public async Task<CreateServiceFromUserPanelResult> AddProductServiceFromUserPanel(AddProductSeviceViewModel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            string lang = CultureInfo.CurrentCulture.Name;

            if (upload_imgs.Count > 10)
            {
                return CreateServiceFromUserPanelResult.ImageCountNotValid;
            }

            #region Advertisement Properties

            Domain.Entities.Services.ProductService ads = new Domain.Entities.Services.ProductService()
            {
                AddressId = model.AddressID,
                UserId = (ulong)model.UserId,
                ImageName = null,
                CreateDate = DateTime.Now,
            };

            #endregion

            #region Product Service Image

            if (upload_imgs != null && upload_imgs.Any())
            {
                foreach (var item in upload_imgs)
                {
                    if (item.IsImage())
                    {
                        var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                        item.AddImageToServer(imageName, PathTools.ProductServiceimageServerOrigin, 150, 150, PathTools.ProductServiceImageServerThumb);
                        ads.ImageName = imageName;

                    }

                    if (item != null && !item.IsImage())
                    {
                        return CreateServiceFromUserPanelResult.ImageIsNotExist;
                    }
                }
            }
            else
            {
                return CreateServiceFromUserPanelResult.ImageIsNotValid;
            }

            #endregion

            await _context.ProductService.AddAsync(ads);
            await _context.SaveChangesAsync();

            #region Add Product Service Category

            foreach (var item in SelectedCategory)
            {
                ProductServiceSelectedService Category = new ProductServiceSelectedService()
                {
                    ServiceId = item,
                    ProductServiceId = ads.Id
                };

                await _context.ProductServiceSelectedService.AddAsync(Category);
            }

            await _context.SaveChangesAsync();

            #endregion

            #region Advertisement Info Properties

            ProductServiceInfo adsInfo = new ProductServiceInfo()
            {
                ProductServiceId = ads.Id,
                Lang_Id = lang,
                Title = model.Title.SanitizeText(),
                Description = model.Description.ConvertNewLineToBr().SanitizeText(),
                CreateDate = DateTime.Now
            };

            await _context.ProductServiceInfo.AddAsync(adsInfo);
            await _context.SaveChangesAsync();

            #endregion

            return CreateServiceFromUserPanelResult.Success;
        }

        //Filter Product Service 
        public async Task<FilterProductServiceViewModel> FilterProductServiceUserSide(FilterProductServiceViewModel filter)
        {
            var query = _context.ProductService
                            .Where(p => p.UserId == filter.UserId && !p.IsDelete)
                            .Include(s => s.User)
                            .Include(s => s.State)
                            .Include(p => p.ProductServiceSelectedService)
                            .ThenInclude(p => p.ServicesCategory)
                            .Include(p => p.ProductServiceInfo)
                            .ThenInclude(p => p.Language)
                            .OrderByDescending(p => p.CreateDate)
                            .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        //Fill Edit Product Service View Model
        public async Task<EditProductServiceViewModel> FillEditProductServiceViewModel(ulong Id)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.ProductService
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            #region Fill View Model

            EditProductServiceViewModel model = new EditProductServiceViewModel()
            {
                AdvertisementID = Ads.Id,
                AddressID = Ads.AddressId,
                UserId = Ads.UserId,
                AdsImage = Ads.ImageName,
            };

            #endregion

            #region Product Service Info

            var adsInfo = await _context.ProductServiceInfo.FirstOrDefaultAsync(p => p.ProductServiceId == Ads.Id
                                                        && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo != null)
            {
                model.Title = adsInfo.Title;
                model.Description = adsInfo.Description;
            }

            #endregion

            return model;
        }

        //Get All PRoduct Service Categories
        public async Task<List<ulong>> GetAllPRoductServiceCategories(ulong Id)
        {
            return await _context.ProductServiceSelectedService
                .Where(p => p.ProductServiceId == Id)
                .Select(p => p.ServiceId)
                .ToListAsync();
        }

        //Get Address By Address Id
        public async Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(p => p.Id == AddressId);
        }

        //Get Product Service By Id 
        public async Task<Domain.Entities.Services.ProductService?> GetProductServiceById(ulong Id)
        {
            return await _context.ProductService.FirstOrDefaultAsync(p => p.Id == Id);
        }

        //Edit Product Service
        public async Task<EditRequestProductServiceFromUserPanelResualt> EditProductServiceFromUserPanel(EditProductServiceViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.ProductService
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .FirstOrDefaultAsync(p => p.Id == model.AdvertisementID && p.UserId == model.UserId && !p.IsDelete);

            if (Ads == null)
            {
                return EditRequestProductServiceFromUserPanelResualt.NotFound;
            }

            #region Properties


            #endregion

            #region Product Service Info 

            var adsInfo = await _context.ProductServiceInfo.FirstOrDefaultAsync(p => p.ProductServiceId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                ProductServiceInfo advertisementInfo = new ProductServiceInfo()
                {
                    ProductServiceId = Ads.Id,
                    Lang_Id = lang,
                    Description = model.Description.SanitizeText(),
                    Title = model.Title.SanitizeText(),
                    CreateDate = DateTime.Now
                };

                await _context.ProductServiceInfo.AddAsync(advertisementInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                adsInfo.Title = model.Title.SanitizeText();
                adsInfo.Description = model.Description.SanitizeText();

                _context.ProductServiceInfo.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Image Part

            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.ImageName != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.ProductServiceimageServerOrigin, 150, 150
                     , PathTools.ProductServiceimageServerOriginThumb, Ads.ImageName);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.ProductServiceimageServerOrigin, 150, 150
                     , PathTools.ProductServiceImageServerThumb);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }

            }

            if (ImageName != null && !ImageName.IsImage())
            {
                return EditRequestProductServiceFromUserPanelResualt.ImageIsNotValid;
            }

            if (ImageName == null && string.IsNullOrEmpty(model.AdsImage))
            {
                return EditRequestProductServiceFromUserPanelResualt.ImageIsNotFound;
            }

            #endregion

            #region Categories

            var selected = await _context.ProductServiceSelectedService.Where(p => p.ProductServiceId == model.AdvertisementID).ToListAsync();

            foreach (var item in selected)
            {
                _context.ProductServiceSelectedService.Remove(item);
            }

            foreach (var item in SelectedCategory)
            {
                ProductServiceSelectedService Category = new ProductServiceSelectedService()
                {
                    ServiceId = item,
                    ProductServiceId = Ads.Id
                };

                await _context.ProductServiceSelectedService.AddAsync(Category);
            }

            #endregion

            _context.ProductService.Update(Ads);
            await _context.SaveChangesAsync();

            return EditRequestProductServiceFromUserPanelResualt.Success;
        }

        //Delete Product Service 
        public async Task<bool> DeleteProductServiceFromUserPanel(ulong productSericeId, ulong userId)
        {
            var ads = await _context.ProductService.FirstOrDefaultAsync(p => p.Id == productSericeId && p.UserId == userId && !p.IsDelete);

            if (ads == null) return false;

            ads.IsDelete = true;

            var adsInfo = await _context.ProductServiceInfo.Where(p => p.ProductServiceId == productSericeId
                                        && !p.IsDelete).ToListAsync();

            if (adsInfo != null && adsInfo.Any())
            {
                foreach (var item in adsInfo)
                {
                    item.IsDelete = true;

                    _context.ProductServiceInfo.Update(item);
                }
            }

            _context.ProductService.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }


        #endregion

        #region Admin Side 

        //Filter Product Service From Admin Side 
        public async Task<FilterProductServiceAdminSideViewModel> FilterProductServiceAdminSide(FilterProductServiceAdminSideViewModel filter)
        {
            var query = _context.ProductService
                            .Include(s => s.User)
                            .Include(p => p.ProductServiceInfo)
                            .ThenInclude(p => p.Language)
                            .Where(p => !p.IsDelete)
                            .OrderByDescending(p => p.CreateDate)
                            .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        //Show Product Service Language
        public async Task<ProductServiceInfo?> ShowProductServiceLanguage(ulong adsId)
        {
            return await _context.ProductServiceInfo.FirstOrDefaultAsync(p => p.Id == adsId && !p.IsDelete);
        }

        //Delete Product Service
        public async Task<bool> DeleteProductService(ulong Id)
        {
            var ads = await _context.ProductService.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == Id);

            if (ads == null)
            {
                return false;
            }

            ads.IsDelete = true;

            _context.ProductService.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Site Side

        //List Of Services
        public async Task<List<ListOfServicesViewModel>> ListOfServicesViewModel(string culture, ulong? categoryId)
        {
            #region filter properties

            if (categoryId.HasValue)
            {
                var returnModel = await _context.ProductServiceSelectedService.Include(p => p.ProductService).ThenInclude(p => p.ProductServiceInfo)
                            .Include(p => p.ServicesCategory).Where(p => p.ServiceId == categoryId.Value && !p.IsDelete && !p.ProductService.IsDelete).ToListAsync();

                var returnModel1 = new List<ListOfServicesViewModel>();

                foreach (var item in returnModel)
                {
                    returnModel1.Add(new ListOfServicesViewModel
                    {
                        ServiceId = item.ProductService.Id,
                        ProductTitle = await _context.ProductServiceInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.ProductServiceId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                        CreateDate = item.ProductService.CreateDate,
                        Image = item.ProductService.ImageName,
                    });
                }

                return returnModel1;
            }

            #endregion

            #region Get Current Product

            var advertisement = await _context.ProductServiceInfo
                        .Include(p => p.ProductService)
                        .ThenInclude(p => p.ProductServiceSelectedService)
                        .ThenInclude(p => p.ServicesCategory)
                        .Where(p => !p.IsDelete && !p.ProductService.IsDelete && p.Lang_Id == culture)
                        .Select(p => p.ProductService).ToListAsync();

            #endregion

            #region model

            var model = new List<ListOfServicesViewModel>();

            foreach (var item in advertisement)
            {
                model.Add(new ListOfServicesViewModel
                {
                    ServiceId = item.Id,
                    ProductTitle = await _context.ProductServiceInfo.Where(p => !p.IsDelete && p.Lang_Id == culture && p.ProductServiceId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                    CreateDate = item.CreateDate,
                    Image = item.ImageName,
                });
            }

            return model;

            #endregion
        }

        #endregion

        #endregion
    }
}
