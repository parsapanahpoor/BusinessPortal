using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.Entities.Tariff;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    public class TariffsViewComponent : ViewComponent
    {
        #region Ctor

        public ITariffService _tariffService { get; set; }

        public TariffsViewComponent(ITariffService tariffService)
        {
           _tariffService = tariffService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Tariffs" , await _tariffService.ShowTariffInHomePage());
        }
    }
}
