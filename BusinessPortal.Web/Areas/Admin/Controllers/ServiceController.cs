using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class ServiceController : AdminBaseController
    {
        #region Ctor

        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        #endregion

        #region Service Categories

        #region Filter Services Categories

        public async Task<IActionResult> FilterServiceCategory(FilterServiceCategoryViewModel filter)
        {
            return View(await _serviceService.FilterServiceCategory(filter));
        }

        #endregion

        #region Create Service Category

        [HttpGet]
        public async Task<IActionResult> CreateServiceCategory(ulong? parentId)
        {
            ViewBag.parentId = parentId;

            if (parentId != null)
            {
                ViewBag.parentLocation = await _serviceService.GetServiceCategoryById(parentId.Value);
            }

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateServiceCategory(CreateServiceCategoryViewModel servicecCategory , IFormFile? serviceCategoryImage)
        {
            #region Model State

            if (!ModelState.IsValid)
            {
                ViewBag.parentId = servicecCategory.ParentId;

                if (servicecCategory.ParentId != null)
                {
                    ViewBag.parentLocation = await _serviceService.GetServiceCategoryById(servicecCategory.ParentId.Value);
                }

                return View(servicecCategory);
            }

            #endregion

            #region Add Location 

            var result = await _serviceService.CreateServiceCategory(servicecCategory , serviceCategoryImage);

            switch (result)
            {
                case CreateServicecCategoryResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                    if (servicecCategory.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterServiceCategory", new { ParentId = servicecCategory.ParentId.Value });
                    }
                    return RedirectToAction("FilterServiceCategory");

                case CreateServicecCategoryResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است .";
                    break;

                case CreateServicecCategoryResult.Fail:
                    TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                    break;
            }

            ViewBag.parentId = servicecCategory.ParentId;

            if (servicecCategory.ParentId != null)
            {
                ViewBag.parentLocation = await _serviceService.GetServiceCategoryById(servicecCategory.ParentId.Value);
            }

            return View(servicecCategory);

            #endregion
        }

        #endregion

        #region Edit Service Category

        [HttpGet]
        public async Task<IActionResult> EditServiceCategory(ulong? serviceCategoryId)
        {
            #region Get Service Category 

            if (serviceCategoryId == null) return NotFound();

            var result = await _serviceService.FillServiceArticleCategoryViewModel(serviceCategoryId.Value);

            if (result == null) return NotFound();

            #endregion

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServiceCategory(EditServiceCategoryViewModel serviceCategory, IFormFile? serviceCategoryImage)
        {
            #region Is Exist Service Category By Id

            if (serviceCategory.Id == null) return NotFound();

            var model = await _serviceService.FillServiceArticleCategoryViewModel(serviceCategory.Id);

            if (model == null) return NotFound();

            #endregion

            #region Model State Validation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده مورد تایید نمی باشد.";

                return View(model);
            }

            #endregion

            #region Edit Article Category

            var result = await _serviceService.EditService(serviceCategory , serviceCategoryImage);

            switch (result)
            {
                case EditServiceCategoryResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است.";
                    if (serviceCategory.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterServiceCategory", new { ParentId = serviceCategory.ParentId.Value });
                    }
                    return RedirectToAction("FilterServiceCategory");

                case EditServiceCategoryResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "عنوان وارد شده تکراری است .";
                    break;

                case EditServiceCategoryResult.Fail:
                    TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
                    break;
            }

            return View(model);

            #endregion
        }

        #endregion

        #region Delete Service Category 
        public async Task<IActionResult> DeleteServiceCategory(ulong serviceCategoryId)
        {
            var result = await _serviceService.DeleteServiceCategory(serviceCategoryId);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #endregion
    }
}
