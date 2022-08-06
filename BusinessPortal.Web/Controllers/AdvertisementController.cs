using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Site.Advertisement;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BusinessPortal.Web.Controllers
{
    public class AdvertisementController : SiteBaseController
    {
        #region Ctor 

        private readonly IAdvertisementService _advertisementService;

        private readonly ITariffService _tariffService;

        public AdvertisementController(IAdvertisementService advertisementService, ITariffService tariffService)
        {
            _advertisementService = advertisementService;
            _tariffService = tariffService;
        }

        #endregion

        #region Seller Advertisements

        #region Filter Seller Advertisement 

        public async Task<IActionResult> FilterSellerAdvertisement(ulong? categoryId, int pageId = 1)
        {
            #region Get Model 

            var model = await _advertisementService.ListOfSaleAdvertisementViewModel(CultureInfo.CurrentCulture.Name, categoryId);

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

        #endregion

        #region Buy Advertisements

        #region Filter Customer Advertisement 

        public async Task<IActionResult> FilterCustomerAdvertisement(ulong? categoryId, int pageId = 1)
        {
            #region Get Model 

            var model = await _advertisementService.ListOfCustomerAdvertisementViewModel(CultureInfo.CurrentCulture.Name, categoryId);

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

        #endregion

        #region Advertisement Detail

        public async Task<IActionResult> AdvertisementDetail(ulong advertisementId)
        {
            #region Check User Tariff

            if (!await _tariffService.CheckUserSeeAdsBaseOnTariff(User.GetUserId()))
            {
                return NotFound();
            }

            #endregion

            return RedirectToAction("Index" , "Home");
        }

        #endregion
    }
}
