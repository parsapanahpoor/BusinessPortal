using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Site.Product;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Ctor 

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Filter Product

        public async Task<IActionResult> FilterProducts(ulong? categoryId, int pageId = 1)
        {
            #region Get Model 

            var model = await _productService.ListOfProductsViewModel(CultureInfo.CurrentCulture.Name, categoryId);

            #endregion

            #region Proccess Paging

            ViewBag.pageId = pageId;
            ViewBag.categoryId = categoryId;

            int take = 28;

            int skip = (pageId - 1) * take;

            int pageCount = (model.Count() / take);

            if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = model.Skip(skip).Take(take).ToList();

            var viewModel = Tuple.Create(query, pageCount);

            #endregion

            return View(viewModel);
        }

        #endregion
    }
}
