using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using BusinessPortal.Domain.ViewModels.UserPanel.ProductService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IServiceService
    {
        #region Admin 

        #region Service Category

        //Get Main Categories For Show In Site Header
        Task<List<ServicesCategoryInfo>> GetMaiCategoriesForShowInSiteHeader();

        Task<FilterServiceCategoryViewModel> FilterServiceCategory(FilterServiceCategoryViewModel filter);

        //Get Service Category By Service Category Id 
        Task<ServicesCategory?> GetServiceCategoryById(ulong serviceCategoryId);

        //Create Service Category
        Task<CreateServicecCategoryResult> CreateServiceCategory(CreateServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage);

        //Fill Edit Service Category Info
        Task<EditServiceCategoryViewModel?> FillServiceArticleCategoryViewModel(ulong serviceCategoryId);

        //Edit Service Group
        Task<EditServiceCategoryResult> EditService(EditServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage);

        //Delete Service Category
        Task<bool> DeleteServiceCategory(ulong serviceCategoryId);

        #endregion

        #endregion

        #region User Panel Side 

        Task<List<CreateProductServiceViewModel>> FillCreateProductServiceViewModel();

        //Add Product Service From User Panel
        Task<CreateServiceFromUserPanelResult> AddProductServiceFromUserPanel(AddProductSeviceViewModel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);

        //Filter Product Service 
        Task<FilterProductServiceViewModel> FilterProductServiceUserSide(FilterProductServiceViewModel filter);

        //Fill Edit Product Service View Model
        Task<EditProductServiceViewModel> FillEditProductServiceViewModel(ulong Id);

        //Get All PRoduct Service Categories
        Task<List<ulong>> GetAllPRoductServiceCategories(ulong Id);

        //Get Address By Address Id
        Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId);

        //Get Product Service By Id 
        Task<Domain.Entities.Services.ProductService?> GetProductServiceById(ulong Id);

        //Edit Product Service
        Task<EditRequestProductServiceFromUserPanelResualt> EditProductServiceFromUserPanel(EditProductServiceViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);

        //Delete Product Service 
        Task<bool> DeleteProductServiceFromUserPanel(ulong productSericeId, ulong userId);

        #endregion
    }
}
