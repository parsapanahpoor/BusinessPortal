using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.ViewModels.Admin.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class CategoriesService : ICategoriesService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }

        public CategoriesService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Main Category

        public async Task<ListOfCategoriesViewModel> FilterCategoryViewModel(ListOfCategoriesViewModel filter)
        {
            var query = _context.Categories
                .Where(a => a.IsDelete == false)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State 

            #region Base On State (IsActive , IsDelete , ... )

            switch (filter.filterCategoryAdminSideState)
            {
                case FilterCategoryAdminSideState.All:
                    break;
                case FilterCategoryAdminSideState.IsActive:
                    query = query.Where(s => s.IsActive == true);
                    break;
                case FilterCategoryAdminSideState.NotActive:
                    query = query.Where(s => s.IsActive == false);
                    break;
            }

            #endregion

            #region Base On Ordering CreateDate

            switch (filter.filterCatgeoryAdminSideOrder)
            {
                case FilterCatgeoryAdminSideOrder.CreateDate_Des:
                    break;
                case FilterCatgeoryAdminSideOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
            }

            #endregion

            #region Base On Ordering Priority

            switch (filter.FilterCategoryAdminSidePriority)
            {
                case FilterCategoryAdminSidePriority.Priority_Des:
                    break;
                case FilterCategoryAdminSidePriority.Priority_Asc:
                    query = query.OrderBy(s => s.Priority);
                    break;
            }

            #endregion

            #endregion

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.DisplayName, $"%{filter.Title}%"));

            if (!string.IsNullOrEmpty(filter.UniqueName))
                query = query.Where(s => EF.Functions.Like(s.UrlName, $"%{filter.UniqueName}%"));

            if (filter.ParentId != null)
            {
                query = query.Where(p => p.ParentId == filter.ParentId);
            }

            if (filter.ParentId == null)
            {
                query = query.Where(p => p.ParentId == null);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<CreateCategoryResult> AddMainCategory(CreateCategoryViewModel cat)
        {
            if (await IsDuplicatedArticleCategory(cat.URL))
            {
                return CreateCategoryResult.CategoryIsExist;
            }

            Category category = new Category()
            {
                DisplayName = cat.DsiplayNamne.SanitizeText(),
                UrlName = cat.URL.SanitizeText(),
                IsActive = cat.IsActive,
                ParentId = cat.ParentId,
                CreateDate = DateTime.Now,
            };

            if (cat.Priority == null)
            {
                category.Priority = 1;
            }
            else
            {
                category.Priority = cat.Priority.Value;
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreateCategoryResult.Success;
        }

        public async Task<bool> IsDuplicatedArticleCategory(string urlName)
        {
            return await _context.Categories.AnyAsync(p => p.UrlName == urlName && !p.IsDelete);
        }

        public async Task<EditCategoryViewModel> FillEditCategoryViewModel(Category model)
        {
            EditCategoryViewModel cat = new EditCategoryViewModel()
            {
                URL = model.UrlName,
                DsiplayNamne = model.DisplayName,
                ParentId = model.ParentId,
                CategoryId = model.Id,
                IsActive = model.IsActive,
                Priority = model.Priority
            };

            return cat;
        }

        public async Task<EditCategoryResult> EditCategoryResult(EditCategoryViewModel category, Category BrowsCategory)
        {
            if (category.URL != BrowsCategory.UrlName && await IsDuplicatedArticleCategory(category.URL))
            {
                return Domain.ViewModels.Admin.Categories.EditCategoryResult.CategoryIsExist;
            }

            BrowsCategory.DisplayName = category.DsiplayNamne.SanitizeText();
            BrowsCategory.UrlName = category.URL.SanitizeText();
            BrowsCategory.IsActive = category.IsActive;
            BrowsCategory.Priority = category.Priority;

            _context.Categories.Update(BrowsCategory);
            await _context.SaveChangesAsync();

            return Domain.ViewModels.Admin.Categories.EditCategoryResult.success;
        }

        public async Task<Category> GetCategoryById(ulong Id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == Id && !p.IsDelete);

            return category;
        }

        public async Task DeleteCategory(Category cat)
        {
            cat.IsDelete = true;

            #region This Part For Main Categpries

            if (cat.ParentId == null)
            {
                var subCategories = await GetSubCategoriesOfMAinCategory(cat.Id);
                if (subCategories.Any())
                {
                    foreach (var item in subCategories)
                    {
                        item.IsDelete = true;
                        _context.Categories.Update(item);
                    }
                }
            }

            #endregion

            _context.Categories.Update(cat);
            await _context.SaveChangesAsync();
        }


        #endregion

        #region Sub Category

        public async Task<List<Category>> GetSubCategoriesOfMAinCategory(ulong MainCategoryId)
        {
            return await _context.Categories.Where(p => !p.IsDelete && p.ParentId.HasValue && p.ParentId == MainCategoryId).ToListAsync();
        }

        #endregion

    }
}
