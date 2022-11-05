using AngleSharp.Css;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class ServiceService : IServiceService
    {
        #region ctor

        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
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
    }
}
