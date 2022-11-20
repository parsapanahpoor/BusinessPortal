using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using BusinessPortal.Domain.ViewModels.Site.Product;
using BusinessPortal.Domain.ViewModels.UserPanel.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Product Categories

        #region Admin Side

        //Get Product Categories For Show In Site Header
        Task<List<ProductCategoryInfo>> GetListOfProductCategoryInfoForShowInSiteHeader();

        //Filter Product Category
        Task<FilterProductCategoryViewModel> FilterProductCategory(FilterProductCategoryViewModel filter);

        //Get Product Category By Product Category Id 
        Task<ProductCategory?> GetProductCategoryById(ulong productCategoryId);

        //Is Exist Product Category By Unique Name
        Task<bool> IsExistProductCategoryByUniqueName(string uniqueName);

        //Is Exist Any Product Category By Id 
        Task<bool> IsExistProductCategoryById(ulong productCategoryId);

        //Create Product Category
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel productCategory , IFormFile? productCategoryLogo);

        //Fill Edit Service Category Info
        Task<EditProductCategoryViewModel?> FillProductCategoryViewModel(ulong productCategoryId);

        //Edit Product Group
        Task<EditProductCategoryResult> EditProduct(EditProductCategoryViewModel productCategory, IFormFile? productCategoryLogo);

        //Delete Product Category
        Task<bool> DeleteProductCategory(ulong productCategoryId);

        #endregion

        #endregion

        #region Product

        #region User Panel Side 

        //Product Category
        Task<List<CreateProductViewModel>> FillCreateProductViewModel();

        //Add Product From User Panel
        Task<CreateProductFromUserPanelResult> AddProductFromUserPanel(AddProductViewModel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);

        //Filter Product  
        Task<FilterProductViewModel> FilterProductUserSide(FilterProductViewModel filter);

        //Fill Edit Product View Model
        Task<EditProductViewModel> FillEditProductViewModel(ulong Id);

        //Get All Product Categories
        Task<List<ulong>> GetAllPRoductCategories(ulong Id);

        //Get Address By Address Id
        Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId);

        //Get Product By Id 
        Task<Domain.Entities.Product.Product?> GetProductById(ulong Id);

        //Edit Product 
        Task<EditRequestProductFromUserPanelResualt> EditProductFromUserPanel(EditProductViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory);

        //Delete Product 
        Task<bool> DeleteProductFromUserPanel(ulong productId, ulong userId);

        #endregion

        #region Admin Side 

        //Filter Product From Admin Side 
        Task<FilterProductAdminSideViewModel> FilterProductAdminSide(FilterProductAdminSideViewModel filter);

        //Show Product Language
        Task<ProductInfo?> ShowProductLanguage(ulong adsId);

        //Delete Product
        Task<bool> DeleteProduct(ulong Id);

        #endregion

        #region Site Side 

        //List Of Products
        Task<List<ListOfProductsViewModel>> ListOfProductsViewModel(string culture, ulong? categoryId);

        #endregion

        #endregion
    }
}
