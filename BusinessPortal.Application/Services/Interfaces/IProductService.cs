using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.ViewModels.Admin.Product;
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
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel productCategory);

        //Fill Edit Service Category Info
        Task<EditProductCategoryViewModel?> FillProductCategoryViewModel(ulong productCategoryId);

        //Edit Product Group
        Task<EditProductCategoryResult> EditProduct(EditProductCategoryViewModel productCategory);

        //Delete Product Category
        Task<bool> DeleteProductCategory(ulong productCategoryId);

        #endregion

        #endregion
    }
}
