using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Controllers
{
    public class ServiceProductController : SiteBaseController
    {
        #region Ctor

        private readonly IServiceService _serviceService;

        public ServiceProductController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        #endregion

        #region List Of Product Services

        public async Task<IActionResult> FilterServiceProduct(ulong? categoryId, int pageId = 1)
        {
            #region Get Model 

            var model = await _serviceService.ListOfServicesViewModel(CultureInfo.CurrentCulture.Name, categoryId);

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
