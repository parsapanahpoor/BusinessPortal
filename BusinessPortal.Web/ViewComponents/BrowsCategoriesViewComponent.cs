using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
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
            return View("BrowsCategories", await _categoriesService.GetMainCategoriesForShowInHomePage());
        }
    }
}
