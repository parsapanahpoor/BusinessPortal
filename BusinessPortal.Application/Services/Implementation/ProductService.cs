using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Ads;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Ads;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using BusinessPortal.Domain.ViewModels.Site.Advertisement;
using BusinessPortal.Domain.ViewModels.Site.Product;
using BusinessPortal.Domain.ViewModels.UserPanel.Product;
using BusinessPortal.Domain.ViewModels.UserPanel.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly IProductRepository _productRepository;

        private readonly BusinessPortalDbContext _context;

        public ProductService(IProductRepository productRepository , BusinessPortalDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
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

        #region Product

        #region User Panel Side

        //Product Category
        public async Task<List<CreateProductViewModel>> FillCreateProductViewModel()
        {
            return await _context.ProductCategories.Include(p => p.ProductCategoryInfos)
                .Where(p => !p.IsDelete)
                .Select(p => new CreateProductViewModel()
                {
                    ProductCategoryName = p.UniqueName,
                    ParentId = p.ParentId,
                    ProductId = p.Id
                }).ToListAsync();
        }

        //Add Product From User Panel
        public async Task<CreateProductFromUserPanelResult> AddProductFromUserPanel(AddProductViewModel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            string lang = CultureInfo.CurrentCulture.Name;

            if (upload_imgs.Count > 10)
            {
                return CreateProductFromUserPanelResult.ImageCountNotValid;
            }

            #region Product Properties

            Domain.Entities.Product.Product ads = new Domain.Entities.Product.Product()
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
                        item.AddImageToServer(imageName, PathTools.ProductimageServerOrigin, 150, 150, PathTools.ProductImageServerThumb);
                        ads.ImageName = imageName;

                    }

                    if (item != null && !item.IsImage())
                    {
                        return CreateProductFromUserPanelResult.ImageIsNotExist;
                    }
                }
            }
            else
            {
                return CreateProductFromUserPanelResult.ImageIsNotValid;
            }

            #endregion

            await _context.Products.AddAsync(ads);
            await _context.SaveChangesAsync();

            #region Add Product Category

            foreach (var item in SelectedCategory)
            {
                ProductSelectedCategories Category = new ProductSelectedCategories()
                {
                    CategoryId = item,
                    ProductId = ads.Id
                };

                await _context.ProductSelectedCategories.AddAsync(Category);
            }

            await _context.SaveChangesAsync();

            #endregion

            #region Product Info Properties

            ProductInfo adsInfo = new ProductInfo()
            {
                ProductId = ads.Id,
                Lang_Id = lang,
                Title = model.Title.SanitizeText(),
                Description = model.Description.ConvertNewLineToBr().SanitizeText(),
                CreateDate = DateTime.Now
            };

            await _context.ProductInfos.AddAsync(adsInfo);
            await _context.SaveChangesAsync();

            #endregion

            return CreateProductFromUserPanelResult.Success;
        }

        //Filter Product  
        public async Task<FilterProductViewModel> FilterProductUserSide(FilterProductViewModel filter)
        {
            var query = _context.Products
                            .Where(p => p.UserId == filter.UserId && !p.IsDelete)
                            .Include(s => s.User)
                            .Include(s => s.State)
                            .Include(p => p.ProductSelectedCategories)
                            .ThenInclude(p => p.ProductCategory)
                            .Include(p => p.ProductInfo)
                            .ThenInclude(p => p.Language)
                            .OrderByDescending(p => p.CreateDate)
                            .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        //Fill Edit Product View Model
        public async Task<EditProductViewModel> FillEditProductViewModel(ulong Id)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Products
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (Ads == null) return null;

            #region Fill View Model

            EditProductViewModel model = new EditProductViewModel()
            {
                AdvertisementID = Ads.Id,
                AddressID = Ads.AddressId,
                UserId = Ads.UserId,
                AdsImage = Ads.ImageName,
            };

            #endregion

            #region Product Info

            var adsInfo = await _context.ProductInfos.FirstOrDefaultAsync(p => p.ProductId == Ads.Id
                                                        && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo != null)
            {
                model.Title = adsInfo.Title;
                model.Description = adsInfo.Description;
            }

            #endregion

            return model;
        }

        //Get All Product Categories
        public async Task<List<ulong>> GetAllPRoductCategories(ulong Id)
        {
            return await _context.ProductSelectedCategories
                .Where(p => p.ProductId == Id)
                .Select(p => p.CategoryId)
                .ToListAsync();
        }

        //Get Address By Address Id
        public async Task<Domain.Entities.Address.Address?> GetAddressByAddressId(ulong AddressId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(p => p.Id == AddressId);
        }

        //Get Product By Id 
        public async Task<Domain.Entities.Product.Product?> GetProductById(ulong Id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
        }

        //Edit Product 
        public async Task<EditRequestProductFromUserPanelResualt> EditProductFromUserPanel(EditProductViewModel model, IFormFile ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            var Ads = await _context.Products
                .Include(s => s.User)
                .Include(s => s.State)
                .ThenInclude(s => s.AddressesState)
                .FirstOrDefaultAsync(p => p.Id == model.AdvertisementID && p.UserId == model.UserId && !p.IsDelete);

            if (Ads == null)
            {
                return EditRequestProductFromUserPanelResualt.NotFound;
            }

            #region Properties


            #endregion

            #region Product Info 

            var adsInfo = await _context.ProductInfos.FirstOrDefaultAsync(p => p.ProductId == Ads.Id
                                                           && p.Lang_Id == lang && !p.IsDelete);

            if (adsInfo == null)
            {
                ProductInfo advertisementInfo = new ProductInfo()
                {
                    ProductId = Ads.Id,
                    Lang_Id = lang,
                    Description = model.Description.SanitizeText(),
                    Title = model.Title.SanitizeText(),
                    CreateDate = DateTime.Now
                };

                await _context.ProductInfos.AddAsync(advertisementInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                adsInfo.Title = model.Title.SanitizeText();
                adsInfo.Description = model.Description.SanitizeText();

                _context.ProductInfos.Update(adsInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Image Part

            if (ImageName != null && ImageName.IsImage())
            {
                if (Ads.ImageName != "Default.png")
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.ProductimageServerOrigin, 150, 150
                     , PathTools.ProductimageServerOriginThumb, Ads.ImageName);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }
                else
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(ImageName.FileName);

                    var res = ImageName.AddImageToServer(imageName, PathTools.ProductimageServerOrigin, 150, 150
                     , PathTools.ProductImageServerThumb);

                    if (res)
                    {
                        Ads.ImageName = imageName;
                    }
                }

            }

            if (ImageName != null && !ImageName.IsImage())
            {
                return EditRequestProductFromUserPanelResualt.ImageIsNotValid;
            }

            if (ImageName == null && string.IsNullOrEmpty(model.AdsImage))
            {
                return EditRequestProductFromUserPanelResualt.ImageIsNotFound;
            }

            #endregion

            #region Categories

            var selected = await _context.ProductSelectedCategories.Where(p => p.ProductId == model.AdvertisementID).ToListAsync();

            foreach (var item in selected)
            {
                _context.ProductSelectedCategories.Remove(item);
            }

            foreach (var item in SelectedCategory)
            {
                ProductSelectedCategories Category = new ProductSelectedCategories()
                {
                    CategoryId = item,
                    ProductId = Ads.Id
                };

                await _context.ProductSelectedCategories.AddAsync(Category);
            }

            #endregion

            _context.Products.Update(Ads);
            await _context.SaveChangesAsync();

            return EditRequestProductFromUserPanelResualt.Success;
        }

        //Delete Product 
        public async Task<bool> DeleteProductFromUserPanel(ulong productId, ulong userId)
        {
            var ads = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.UserId == userId && !p.IsDelete);

            if (ads == null) return false;

            ads.IsDelete = true;

            var adsInfo = await _context.ProductInfos.Where(p => p.ProductId == productId
                                        && !p.IsDelete).ToListAsync();

            if (adsInfo != null && adsInfo.Any())
            {
                foreach (var item in adsInfo)
                {
                    item.IsDelete = true;

                    _context.ProductInfos.Update(item);
                }
            }

            _context.Products.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Admin Side 

        //Filter Product From Admin Side 
        public async Task<FilterProductAdminSideViewModel> FilterProductAdminSide(FilterProductAdminSideViewModel filter)
        {
            var query = _context.Products
                            .Include(s => s.User)
                            .Include(p => p.ProductInfo)
                            .ThenInclude(p => p.Language)
                            .Where(p => !p.IsDelete)
                            .OrderByDescending(p => p.CreateDate)
                            .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        //Show Product Language
        public async Task<ProductInfo?> ShowProductLanguage(ulong adsId)
        {
            return await _context.ProductInfos.FirstOrDefaultAsync(p => p.Id == adsId && !p.IsDelete);
        }

        //Delete Product
        public async Task<bool> DeleteProduct(ulong Id)
        {
            var ads = await _context.Products.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == Id);

            if (ads == null)
            {
                return false;
            }

            ads.IsDelete = true;

            _context.Products.Update(ads);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Site Side 

        //List Of Products
        public async Task<List<ListOfProductsViewModel>> ListOfProductsViewModel(string culture, ulong? categoryId)
        {
            #region filter properties

            if (categoryId.HasValue)
            {
                var returnModel = await _context.ProductSelectedCategories.Include(p => p.Product).ThenInclude(p => p.ProductInfo)
                            .Include(p => p.ProductCategory).Where(p => p.CategoryId == categoryId.Value && !p.IsDelete && !p.Product.IsDelete).ToListAsync();

                var returnModel1 = new List<ListOfProductsViewModel>();

                foreach (var item in returnModel)
                {
                    returnModel1.Add(new ListOfProductsViewModel
                    {
                        PRoductId = item.Product.Id,
                        ProductTitle = await _context.ProductInfos.Where(p => !p.IsDelete && p.Lang_Id == culture && p.ProductId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
                        CreateDate = item.Product.CreateDate,
                        Image = item.Product.ImageName,
                    });
                }

                return returnModel1;
            }

            #endregion

            #region Get Current Product

            var advertisement = await _context.ProductInfos
                        .Include(p => p.Product)
                        .ThenInclude(p => p.ProductSelectedCategories)
                        .ThenInclude(p => p.ProductCategory)
                        .Where(p => !p.IsDelete && !p.Product.IsDelete && p.Lang_Id == culture)
                        .Select(p => p.Product).ToListAsync();

            #endregion

            #region model

            var model = new List<ListOfProductsViewModel>();

            foreach (var item in advertisement)
            {
                model.Add(new ListOfProductsViewModel
                {
                    PRoductId = item.Id,
                    ProductTitle = await _context.ProductInfos.Where(p => !p.IsDelete && p.Lang_Id == culture && p.ProductId == item.Id).Select(p => p.Title).FirstOrDefaultAsync(),
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
