using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.ViewModels.Admin.Categories;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface ICategoriesService
    {
        #region Main Category

        Task<ListOfCategoriesViewModel> FilterCategoryViewModel(ListOfCategoriesViewModel filter);
        Task<CreateCategoryResult> AddMainCategory(CreateCategoryViewModel cat);
        Task<bool> IsDuplicatedArticleCategory(string urlName);
        Task<EditCategoryViewModel> FillEditCategoryViewModel(Category model);
        Task<EditCategoryResult> EditCategoryResult(EditCategoryViewModel category, Category BrowsCategory);
        Task<Category> GetCategoryById(ulong Id);
        Task DeleteCategory(Category cat);
        List<SelectListItem> GetAdvertisementMainCategoryDrowpDown();
        Task<List<AdvertisementCategoryViewModel>> GetAllCorrectCategoryForShowInUserPanel();
        Task<List<Category>> GetMainCategoriesForShowInHomePage();

        #endregion

        #region Sub Catgeories

        Task<List<Category>> GetSubCategoriesOfMAinCategory(ulong MainCategoryId);

        #endregion

    }
}
