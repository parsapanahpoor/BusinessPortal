using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    //Sale Categrories View Component
    public class BrowsCategoriesViewComponent : ViewComponent
    {
        #region Ctor

        public IProductService _categoriesService { get; set; }

        public BrowsCategoriesViewComponent(IProductService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BrowsCategories", await _categoriesService.GetListOfProductCategoryInfoForShowInSiteHeader());
        }
    }

    //Service Categories View Component
    public class BuyCategoriesViewComponent : ViewComponent
    {
        #region Ctor

        public IServiceService _categoriesService { get; set; }

        public BuyCategoriesViewComponent(IServiceService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BuyCategories", await _categoriesService.GetMaiCategoriesForShowInSiteHeader());
        }
    }
}
