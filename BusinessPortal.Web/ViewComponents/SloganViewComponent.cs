using BusinessPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.ViewComponents
{
    //Sale Categrories View Component
    public class SloganViewComponent : ViewComponent
    {
        #region Ctor

        public ISloganService _sloganService{ get; set; }

        public SloganViewComponent(ISloganService sloganService)
        {
            _sloganService = sloganService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Slogan", await _sloganService.GetSlogan());
        }
    }
}
