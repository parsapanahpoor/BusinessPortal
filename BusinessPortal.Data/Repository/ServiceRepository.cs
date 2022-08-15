using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Data.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public ServiceRepository(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        #region Service Category 

        //Get Main Categories For Show In Site Header
        public async Task<List<ServicesCategoryInfo>> GetMaiCategoriesForShowInSiteHeader()
        {
            return await _context.ServicesCategoryInfos.Include(p=> p.ServicesCategory)
                            .Where(p=> !p.IsDelete && p.ServicesCategory.ParentId == null)
                            .ToListAsync();
        }

        public async Task<FilterServiceCategoryViewModel> FilterServiceCategory(FilterServiceCategoryViewModel filter)
        {
            var query = _context.ServicesCategoryInfos
               .Include(a => a.ServicesCategory)
               .ThenInclude(p => p.Parent)
               .OrderByDescending(s => s.CreateDate)
               .AsQueryable();


            #region Filter

            if (!string.IsNullOrEmpty(filter.UniqueName))
            {
                query = query.Where(s => EF.Functions.Like(s.ServicesCategory.UniqueName, $"%{filter.UniqueName}%"));
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));
            }

            if (filter.ParentId != null)
            {
                query = query.Where(a => a.ServicesCategory.ParentId == filter.ParentId);
                filter.ParentServicesCategory = await _context.ServicesCategories.FirstOrDefaultAsync(a => a.Id == filter.ParentId);
            }
            else
            {
                query = query.Where(a => a.ServicesCategory.ParentId == null);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        //Get Service Category By Service Category Id 
        public async Task<ServicesCategory?> GetServiceCategoryById(ulong serviceCategoryId)
        {
            return await _context.ServicesCategories.Include(p => p.ServicesCategoryInfo)
                                    .FirstOrDefaultAsync(s => !s.IsDelete && s.Id == serviceCategoryId);
        }

        //Is Exist Service Category By Unique Name
        public async Task<bool> IsExistServiceCategoryByUniqueName(string uniqueName)
        {
            return await _context.ServicesCategories.AnyAsync(p => p.UniqueName == uniqueName && !p.IsDelete);
        }

        //Is Exist Any Service Category By Id 
        public async Task<bool> IsExistServiceCategoryById(ulong serviceCategoryId)
        {
            return await _context.ServicesCategories.AnyAsync(p => p.Id == serviceCategoryId && !p.IsDelete);
        }

        //Add Service Categories
        public async Task<ulong> AddServiceCategory(ServicesCategory serviceCategory)
        {
            #region Add Service

            await _context.ServicesCategories.AddAsync(serviceCategory);
            await _context.SaveChangesAsync();

            #endregion

            return serviceCategory.Id;
        }

        //Add Service Category Info
        public async Task AddServiceCategoryInfo(List<ServicesCategoryInfo> serviceCategoryInfos)
        {
            await _context.ServicesCategoryInfos.AddRangeAsync(serviceCategoryInfos);
            await _context.SaveChangesAsync();
        }

        //Fill Edit Service Category Info
        public async Task<EditServiceCategoryViewModel?> FillServiceArticleCategoryViewModel(ulong serviceCategoryId)
        {
            return await _context.ServicesCategories
                            .Include(p => p.ServicesCategoryInfo)
                            .Where(p => p.Id == serviceCategoryId && !p.IsDelete).Select(p => new EditServiceCategoryViewModel()
                            {
                                Id = p.Id,
                                UniqueName = p.UniqueName,
                                ParentId = p.ParentId,
                                CurrentInfos = p.ServicesCategoryInfo.AsQueryable().IgnoreQueryFilters().ToList()
                            }).FirstOrDefaultAsync();
        }

        //Update Service Category
        public void UpdateServiceCategory(ServicesCategory serviceCategory)
        {
            _context.ServicesCategories.Update(serviceCategory);
        }

        //Update Service Category Info
        public void UpdateServiceCategoryInfo(ServicesCategoryInfo serviceCategoryInfo)
        {
            _context.ServicesCategoryInfos.Update(serviceCategoryInfo);
        }

        //Save Changes
        public async Task Savechanges()
        {
             await _context.SaveChangesAsync();
        }

        //Delete Service Category Info
        public async Task DeleteServiceCategoryInfo(ulong serviceCategoryId)
        {
            var serviceCategoryInfo = await _context.ServicesCategoryInfos.Where(p => p.ServicesCategoryId == serviceCategoryId).IgnoreQueryFilters().ToListAsync();

            if (serviceCategoryInfo != null && serviceCategoryInfo.Any())
            {
                foreach (var item in serviceCategoryInfo)
                {
                    _context.ServicesCategoryInfos.Remove(item);
                }
            }
        }

        //Get Childs Of Service Category By PArent ID
        public async Task<List<ServicesCategory>> GetChildServiceCategoryByParentId(ulong parentId)
        {
            return await _context.ServicesCategories.Where(p => !p.IsDelete && p.ParentId == parentId).ToListAsync();
        }

        //Delete Service Category And Service Category Info
        public async Task DeleteServiceCategory(ServicesCategory serviceCategory)
        {
            //Delete First Part Of Categories
            serviceCategory.IsDelete = true;
            _context.ServicesCategories.Update(serviceCategory);

            //Delete First PartOf Category Info
            await DeleteServiceCategoryInfo(serviceCategory.Id);

            //Get Seconde Part Of Category Info
            var secondePartOfChild = await GetChildServiceCategoryByParentId(serviceCategory.Id);

            if (secondePartOfChild != null && secondePartOfChild.Any())
            {
                foreach (var item in secondePartOfChild)
                {
                    //Delete Seconde PartOf Category Info
                    item.IsDelete = true;
                    _context.ServicesCategories.Update(item);

                    //Delete Seconde PartOf Category Info
                    await DeleteServiceCategoryInfo(item.Id);

                }
            }

            await _context.SaveChangesAsync();

        }

        #endregion

        #endregion
    }
}
