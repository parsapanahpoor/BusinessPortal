using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Controllers
{
    public class AdsController : SiteBaseController
    {
        #region ctor

        private readonly IAdsService _adsServicec;

        public AdsController(IAdsService adsServicec)
        {
            _adsServicec = adsServicec;
        }

        #endregion

        #region List Of Ads

        public async Task<IActionResult> FilterSellerAdvertisement(int pageId = 1)
        {
            #region Get Model 

            var model = await _adsServicec.FilterAdsSiteSide(CultureInfo.CurrentCulture.Name);

            #endregion

            #region Proccess Paging

            ViewBag.pageId = pageId;

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
