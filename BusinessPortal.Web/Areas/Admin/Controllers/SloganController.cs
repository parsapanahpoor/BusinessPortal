using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Slogan;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class SloganController : AdminBaseController
    {
        #region Ctor

        private readonly ISloganService _sloganService;

        public SloganController(ISloganService sloganService)
        {
            _sloganService = sloganService;
        }

        #endregion

        #region Create Or Edit Slogan

        [HttpGet]
        public async Task<ActionResult> CreateOrEditSlogan()
        {
            return View(await _sloganService.FillCreateOrEditSloganViewModel());
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrEditSlogan(CreateOrEditSloganViewModel model)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View(model);
            }

            #endregion

            #region Create OR Edit 

            var res = await _sloganService.CreateOrEditSlogan(model);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(CreateOrEditSlogan));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion
    }
}
