using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion

        #region Product Category 

        #region Admin Side 

        //Get Product Categories For Show In Site Header
        public async Task<List<ProductCategoryInfo>> GetListOfProductCategoryInfoForShowInSiteHeader()
        {
            return await _productRepository.GetListOfProductCategoryInfoForShowInSiteHeader();
        }

        //Filter Product Category
        public async Task<FilterProductCategoryViewModel> FilterProductCategory(FilterProductCategoryViewModel filter)
        {
            return await _productRepository.FilterProductCategory(filter);
        }

        //Get Product Category By Product Category Id 
        public async Task<ProductCategory?> GetProductCategoryById(ulong productCategoryId)
        {
            return await _productRepository.GetProductCategoryById(productCategoryId);
        }

        //Is Exist Product Category By Unique Name
        public async Task<bool> IsExistProductCategoryByUniqueName(string uniqueName)
        {
            return await _productRepository.IsExistProductCategoryByUniqueName(uniqueName);
        }

        //Is Exist Any Product Category By Id 
        public async Task<bool> IsExistProductCategoryById(ulong productCategoryId)
        {
            return await _productRepository.IsExistProductCategoryById(productCategoryId);
        }

        //Create Product Category
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel productCategory, IFormFile? productCategoryLogo)
        {
            #region Is Exist Product Category By Unique Name

            if (await _productRepository.IsExistProductCategoryByUniqueName(productCategory.UniqueName))
            {
                return CreateProductCategoryResult.UniqNameIsExist;
            }

            #endregion

            #region Add Product Category

            var mainProductCategory = new ProductCategory()
            {
                UniqueName = productCategory.UniqueName.SanitizeText(),
                IsDelete = false,
                IsActive = true
            };

            #region Add Image 

            if (productCategoryLogo != null && productCategoryLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(productCategoryLogo.FileName);
                productCategoryLogo.AddImageToServer(imageName, PathTools.ProductCategoryimageServerOrigin, 400, 300, PathTools.ProductCategoryImageServerThumb);
                mainProductCategory.ProductCategoryImage = imageName;
            }

            #endregion

            if (productCategory.ParentId != null && productCategory.ParentId != 0)
            {
                if (await _productRepository.IsExistProductCategoryById(productCategory.ParentId.Value))
                {
                    mainProductCategory.ParentId = productCategory.ParentId;
                }
                else
                {
                    return CreateProductCategoryResult.Fail;
                }
            }

            var productCategoryId = await _productRepository.AddProductCategory(mainProductCategory);

            #endregion

            #region Add Product Info

            var productCategoryInfo = new List<ProductCategoryInfo>();

            foreach (var culture in productCategory.ProductCategoryInfos)
            {
                var productCategoryInfos = new ProductCategoryInfo
                {
                    ProductCategoryId = productCategoryId,
                    LanguageId = culture.Culture,
                    Title = culture.Title.SanitizeText(),
                    CreateDate = DateTime.Now,
                };

                productCategoryInfo.Add(productCategoryInfos);
            }

            await _productRepository.AddProductCategoryInfo(productCategoryInfo);

            #endregion

            return CreateProductCategoryResult.Success;
        }

        //Fill Edit Service Category Info
        public async Task<EditProductCategoryViewModel?> FillProductCategoryViewModel(ulong productCategoryId)
        {
            return await _productRepository.FillProductCategoryViewModel(productCategoryId);
        }

        //Edit Product Group
        public async Task<EditProductCategoryResult> EditProduct(EditProductCategoryViewModel productCategory, IFormFile? productCategoryLogo)
        {
            #region Get Product Category By Id

            var productCat = await _productRepository.GetProductCategoryById(productCategory.Id);

            if (productCat == null) return EditProductCategoryResult.Fail;

            #endregion

            #region Is Exist Product Category By Unique Name

            if (productCat.UniqueName != productCategory.UniqueName)
            {
                if (await _productRepository.IsExistProductCategoryByUniqueName(productCategory.UniqueName))
                {
                    return EditProductCategoryResult.UniqNameIsExist;
                }
            }

            #endregion

            #region Is Exist Product Category By Parent Id

            if (productCategory.ParentId != null && productCategory.ParentId != 0)
            {
                if (!await _productRepository.IsExistProductCategoryById(productCategory.ParentId.Value))
                {
                    return EditProductCategoryResult.Fail;
                }
            }

            #endregion

            #region Update Product Category

            productCat.UniqueName = productCategory.UniqueName.SanitizeText();
            productCat.IsActive = true;

            if (productCategoryLogo != null && productCategoryLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(productCategoryLogo.FileName);
                productCategoryLogo.AddImageToServer(imageName, PathTools.ProductCategoryimageServerOrigin, 400, 300, PathTools.ProductCategoryImageServerThumb);

                if (!string.IsNullOrEmpty(productCat.ProductCategoryImage))
                {
                    productCat.ProductCategoryImage.DeleteImage(PathTools.ProductCategoryimageServerOrigin, PathTools.ProductCategoryImageServerThumb);
                }

                productCat.ProductCategoryImage = imageName;
            }

            _productRepository.UpdateProductCategory(productCat);

            #endregion

            #region Product Info 

            foreach (var productCategoryInfo in productCat.ProductCategoryInfos)
            {
                var updatedInfo = productCategory.ProductCategoryInfos.FirstOrDefault(p => p.Culture == productCategoryInfo.LanguageId);

                if (updatedInfo != null)
                {
                    productCategoryInfo.Title = updatedInfo.Title.SanitizeText();
                }

                _productRepository.UpdateProductCategoryInfo(productCategoryInfo);
            }

            #endregion

            await _productRepository.Savechanges();

            return EditProductCategoryResult.Success;
        }

        //Delete Product Category
        public async Task<bool> DeleteProductCategory(ulong productCategoryId)
        {
            //Get Product Category By Id
            var productCategory = await _productRepository.GetProductCategoryById(productCategoryId);

            if (productCategory == null) return false;

            //Delete Product And Service Category Info
            await _productRepository.DeleteProductCategory(productCategory);

            return true;
        }

        #endregion

        #endregion
    }
}
