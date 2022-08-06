using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    //Sale Categrories View Component
    public class BrowsCategoriesViewComponent : ViewComponent
    {
        #region Ctor

        public ICategoriesService _categoriesService { get; set; }

        public BrowsCategoriesViewComponent(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BrowsCategories", await _categoriesService.GetCategoriesForShowInHeader());
        }
    }

    //Buy Categories View Component
    public class BuyCategoriesViewComponent : ViewComponent
    {
        #region Ctor

        public ICategoriesService _categoriesService { get; set; }

        public BuyCategoriesViewComponent(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BuyCategories", await _categoriesService.GetCategoriesForShowInHeader());
        }
    }
}
