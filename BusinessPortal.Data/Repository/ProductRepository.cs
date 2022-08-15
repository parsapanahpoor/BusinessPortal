using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public ProductRepository(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Product Category

        #region Admin Side 

        //Get Product Categories For Show In Site Header
        public async Task<List<ProductCategoryInfo>> GetListOfProductCategoryInfoForShowInSiteHeader()
        {
            return await _context.ProductCategoryInfos.Include(p=> p.ProductCategory)   
                    .Where(p=> !p.IsDelete)
                    .ToListAsync(); 
        }

        //Filter Product Category
        public async Task<FilterProductCategoryViewModel> FilterProductCategory(FilterProductCategoryViewModel filter)
        {
            var query = _context.ProductCategoryInfos
               .Include(a => a.ProductCategory)
               .ThenInclude(p => p.Parent)
               .OrderByDescending(s => s.CreateDate)
               .AsQueryable();


            #region Filter

            if (!string.IsNullOrEmpty(filter.UniqueName))
            {
                query = query.Where(s => EF.Functions.Like(s.ProductCategory.UniqueName, $"%{filter.UniqueName}%"));
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));
            }

            if (filter.ParentId != null)
            {
                query = query.Where(a => a.ProductCategory.ParentId == filter.ParentId);
                filter.ParentProductCategory = await _context.ProductCategories.FirstOrDefaultAsync(a => a.Id == filter.ParentId);
            }
            else
            {
                query = query.Where(a => a.ProductCategory.ParentId == null);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        //Get Product Category By Product Category Id 
        public async Task<ProductCategory?> GetProductCategoryById(ulong productCategoryId)
        {
            return await _context.ProductCategories.Include(p => p.ProductCategoryInfos)
                                    .FirstOrDefaultAsync(s => !s.IsDelete && s.Id == productCategoryId);
        }

        //Is Exist Product Category By Unique Name
        public async Task<bool> IsExistProductCategoryByUniqueName(string uniqueName)
        {
            return await _context.ProductCategories.AnyAsync(p => p.UniqueName == uniqueName && !p.IsDelete);
        }

        //Is Exist Any Product Category By Id 
        public async Task<bool> IsExistProductCategoryById(ulong productCategoryId)
        {
            return await _context.ProductCategories.AnyAsync(p => p.Id == productCategoryId && !p.IsDelete);
        }

        //Add Product Categories
        public async Task<ulong> AddProductCategory(ProductCategory productCategory)
        {
            #region Add Product

            await _context.ProductCategories.AddAsync(productCategory);
            await _context.SaveChangesAsync();

            #endregion

            return productCategory.Id;
        }

        //Add Product Category Info
        public async Task AddProductCategoryInfo(List<ProductCategoryInfo> productCategoryInfos)
        {
            await _context.ProductCategoryInfos.AddRangeAsync(productCategoryInfos);
            await _context.SaveChangesAsync();
        }

        //Fill Edit Service Category Info
        public async Task<EditProductCategoryViewModel?> FillProductCategoryViewModel(ulong productCategoryId)
        {
            return await _context.ProductCategories
                            .Include(p => p.ProductCategoryInfos)
                            .Where(p => p.Id == productCategoryId && !p.IsDelete).Select(p => new EditProductCategoryViewModel()
                            {
                                Id = p.Id,
                                UniqueName = p.UniqueName,
                                ParentId = p.ParentId,
                                CurrentInfos = p.ProductCategoryInfos.AsQueryable().IgnoreQueryFilters().ToList()
                            }).FirstOrDefaultAsync();
        }

        //Update Product Category
        public void UpdateProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Update(productCategory);
        }

        //Update Product Category Info
        public void UpdateProductCategoryInfo(ProductCategoryInfo productCategoryInfo)
        {
            _context.ProductCategoryInfos.Update(productCategoryInfo);
        }

        //Save Changes
        public async Task Savechanges()
        {
            await _context.SaveChangesAsync();
        }

        //Delete Product Category Info
        public async Task DeleteProductCategoryInfo(ulong productCategoryId)
        {
            var productCategoryInfo = await _context.ProductCategoryInfos.Where(p => p.ProductCategoryId == productCategoryId).IgnoreQueryFilters().ToListAsync();

            if (productCategoryInfo != null && productCategoryInfo.Any())
            {
                foreach (var item in productCategoryInfo)
                {
                    _context.ProductCategoryInfos.Remove(item);
                }
            }
        }

        //Get Childs Of Product Category By Parent ID
        public async Task<List<ProductCategory>> GetChildProductCategoryByParentId(ulong parentId)
        {
            return await _context.ProductCategories.Where(p => !p.IsDelete && p.ParentId == parentId).ToListAsync();
        }

        //Delete Product Category And Prodcut Category Info
        public async Task DeleteProductCategory(ProductCategory productCategory)
        {
            //Delete First Part Of Categories
            productCategory.IsDelete = true;
            _context.ProductCategories.Update(productCategory);

            //Delete First PartOf Category Info
            await DeleteProductCategoryInfo(productCategory.Id);

            //Get Seconde Part Of Category Info
            var secondePartOfChild = await GetChildProductCategoryByParentId(productCategory.Id);

            if (secondePartOfChild != null && secondePartOfChild.Any())
            {
                foreach (var item in secondePartOfChild)
                {
                    //Delete Seconde PartOf Category Info
                    item.IsDelete = true;
                    _context.ProductCategories.Update(item);

                    //Delete Seconde PartOf Category Info
                    await DeleteProductCategoryInfo(item.Id);

                }
            }

            await _context.SaveChangesAsync();

        }

        #endregion

        #endregion
    }
}
