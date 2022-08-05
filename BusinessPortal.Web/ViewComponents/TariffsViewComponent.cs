using BusinessPortal.Domain.Entities.Tariff;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    public class TariffsViewComponent : ViewComponent
    {
        #region Ctor

        public Tariff _tariffService { get; set; }

        public TariffsViewComponent(Tariff tariffService)
        {
           _tariffService = tariffService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Tariffs");
        }
    }
}
