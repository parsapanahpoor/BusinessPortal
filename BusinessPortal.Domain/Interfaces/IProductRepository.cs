using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Interfaces
{
    public interface IProductRepository
    {
        #region Product Category

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

        //Add Product Categories
        Task<ulong> AddProductCategory(ProductCategory productCategory);

        //Add Product Category Info
        Task AddProductCategoryInfo(List<ProductCategoryInfo> productCategoryInfos);

        //Fill Edit Service Category Info
        Task<EditProductCategoryViewModel?> FillProductCategoryViewModel(ulong productCategoryId);

        //Update Product Category
        void UpdateProductCategory(ProductCategory productCategory);

        //Update Product Category Info
        void UpdateProductCategoryInfo(ProductCategoryInfo productCategoryInfo);

        //Save Changes
        Task Savechanges();

        //Delete Product Category Info
        Task DeleteProductCategoryInfo(ulong productCategoryId);

        //Get Childs Of Product Category By Parent ID
        Task<List<ProductCategory>> GetChildProductCategoryByParentId(ulong parentId);

        //Delete Product Category And Prodcut Category Info
        Task DeleteProductCategory(ProductCategory productCategory);

        #endregion

        #endregion
    }
}
