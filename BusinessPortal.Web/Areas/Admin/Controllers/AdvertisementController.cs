using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Advertisement;
using BusinessPortal.Domain.ViewModels.Admin.State;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementController : AdminBaseController
    {
        #region Ctor

        public IAdvertisementService _advertisementService { get; set; }

        public ICategoriesService _categoriesService { get; set; }

        public IStateService _stateService { get; set; }

        public AdvertisementController(IAdvertisementService advertisementService , ICategoriesService categoriesService , IStateService stateService)
        {
            _advertisementService = advertisementService;
            _categoriesService = categoriesService;
            _stateService = stateService;
        }

        #endregion

        #region List Of Advertisement

        public async Task<IActionResult> Index(FilterAdvertisementAdminSidedViewModel filter)
        {
            return View(await _advertisementService.FilterRequestAdvertisementAdminSide(filter));
        }

        #endregion

        #region Edit Advertisement From Admin Panel

        public async Task<IActionResult> EditAdvertisement(ulong Id)
        {
            var advertisement = await _advertisementService.SetEditAdvertisementFromAdminPanel(Id);

            if (advertisement == null) return NotFound();

            #region Page Data 

            ViewBag.Categories = await _categoriesService.GetAllCorrectCategoryForShowInUserPanel();

            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(Id);

            if (advertisement.AddressID.HasValue)
            {
                ViewBag.AdveritsementAddress = await _stateService.GetAddressByAddressId(advertisement.AddressID.Value);
            }

            #endregion

            return View(advertisement);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdvertisement(EditAdvertisementFromAdminPanel model, IFormFile? ImageNames, List<ulong> SelectedCategory)
        {
            var advertisement = await _advertisementService.GetAdvertisementByID(model.AdvertisementID);

            if (advertisement == null) return NotFound();

            #region Page Data 

            ViewBag.Categories = await _categoriesService.GetAllCorrectCategoryForShowInUserPanel();

            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(advertisement.Id);

            if (advertisement.AddressId.HasValue)
            {
                ViewBag.AdveritsementAddress = await _stateService.GetAddressByAddressId(advertisement.AddressId.Value);
            }

            #endregion

            if (ModelState.IsValid)
            {
                
                if (SelectedCategory == null)
                {
                    TempData[WarningMessage] = "دسته بندی های آگهی باید انتخاب شود  ";

                    return View(model);
                }

                var result = await _advertisementService.UpdateAdvertisement(model, ImageNames, SelectedCategory);
                switch (result)
                {
                    case EditAdvertisementFromAdminPanelResult.ImageCountNotValid:
                        TempData[WarningMessage] = "مجموع عکس های وارد شده نباید بیشتر از ده تا باشد!";
                        return View(model);
                    case EditAdvertisementFromAdminPanelResult.NotFound:
                        TempData[ErrorMessage] = "آگهی مورد نظر یافت شند! ";
                        return Redirect("/Admin/Advertisement/Index");
                    case EditAdvertisementFromAdminPanelResult.Success:
                        TempData[SuccessMessage] = "آگهی مورد نظر با موفقیت ویرایش شده است ";
                        return Redirect("/Admin/Advertisement/Index");
                    case EditAdvertisementFromAdminPanelResult.Faild:
                        TempData[ErrorMessage] = "ویرایش آگهی مورد نظر با مشکل روبرو گردیده است  ";
                        return View(model);
                    case EditAdvertisementFromAdminPanelResult.SiteSettingNotExist:
                        TempData[ErrorMessage] = "تعداد روز نمایش آگهی در تنظیمات سایت یافت نشده است  ";
                        return View(model);
                    case EditAdvertisementFromAdminPanelResult.FillRejectDescription:
                        TempData[ErrorMessage] = "علت عدم تایید آگهی را وارد کنید";
                        return View(model);
                    case EditAdvertisementFromAdminPanelResult.ImageIsNotValid:
                        TempData[ErrorMessage] = "لطفا فقط عکس با فرمت های معتبر انتخاب شود";
                        return View(model);
                }

            }

            return View(model);
        }

        #endregion

        #region Delete Advertisement

        public async Task<IActionResult> DeleteAdvertisementFromAdmin(ulong id)
        {
            var result = await _advertisementService.DeleteAdvertisementFromAdmin(id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Show Advertisement Language

        public async Task<IActionResult> ShowAdvertisementLanguage(ulong adsId)
        {
            var ads = await _advertisementService.ShowAdvertisementLanguage(adsId);

            return PartialView("_ShowAdvertisementLanguage", ads);
        }

        #endregion
    }
}
