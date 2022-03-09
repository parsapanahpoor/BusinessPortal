using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Categories;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        #region Ctor

        public ICategoriesService _categoriesService { get; set; }

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        #endregion

        #region List Of Categories

        public async Task<IActionResult> Index(ListOfCategoriesViewModel filter)
        {
            return View(await _categoriesService.FilterCategoryViewModel(filter));
        }

        #endregion

        #region Create Main Category

        [HttpGet]
        public async Task<IActionResult> CreateMainCategory()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMainCategory(CreateCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoriesService.AddMainCategory(category);

                switch (result)
                {
                    case CreateCategoryResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        return RedirectToAction("Index", "Categories");

                    case CreateCategoryResult.Faild:
                        TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                        break;

                    case CreateCategoryResult.CategoryIsExist:
                        TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است  ";
                        break;
                }
            }
            return View(category);
        }

        #endregion

        #region Edit  Main Category

        [HttpGet]
        public async Task<IActionResult> EditMainCategory(ulong Id)
        {
            var cat = await _categoriesService.GetCategoryById((ulong)Id);

            if (cat == null) return NotFound();

            var model = await _categoriesService.FillEditCategoryViewModel(cat);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMainCategory(EditCategoryViewModel cat)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoriesService.GetCategoryById((ulong)cat.CategoryId);

                if (category == null) return NotFound();

                var result = await _categoriesService.EditCategoryResult(cat, category);

                switch (result)
                {
                    case EditCategoryResult.success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        return RedirectToAction("Index", "Categories");

                    case EditCategoryResult.Fiald:
                        TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                        break;

                    case EditCategoryResult.CategoryIsExist:
                        TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است  ";
                        break;
                }
            }

            return View(cat);
        }
        #endregion

        #region Delete Main Category

        public async Task<IActionResult> DeleteCategory(ulong Id)
        {
            var category = await _categoriesService.GetCategoryById((ulong)Id);

            if (category == null) return JsonResponseStatus.Error();

            await _categoriesService.DeleteCategory(category);

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Create Sub Category

        [HttpGet]
        public async Task<IActionResult> CreateSubCategory(ulong ParentId)
        {
            var MainCat = await _categoriesService.GetCategoryById((ulong)ParentId);

            if (MainCat == null) return NotFound();

            ViewBag.ParentId = ParentId;
            ViewBag.ParentTitle = MainCat.DisplayName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(CreateCategoryViewModel cat)
        {
            var MainCat = await _categoriesService.GetCategoryById(cat.ParentId.Value);

            if (MainCat == null)
            {
                ViewBag.ParentId = cat.ParentId;
                ViewBag.ParentTitle = MainCat.DisplayName;

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _categoriesService.AddMainCategory(cat);

                switch (result)
                {
                    case CreateCategoryResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        return RedirectToAction("ListOfSubCategories", "Categories", new { ParentId = cat.ParentId });

                    case CreateCategoryResult.Faild:
                        TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                        break;

                    case CreateCategoryResult.CategoryIsExist:
                        TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است  ";
                        break;
                }

            }

            ViewBag.ParentId = cat.ParentId;
            ViewBag.ParentTitle = MainCat.DisplayName;

            return View(cat);
        }
        #endregion

        #region List Of Artcile Categories

        public async Task<IActionResult> ListOfSubCategories(ListOfCategoriesViewModel filter)
        {
            if (filter.ParentId == null) return NotFound();

            var MainCat = await _categoriesService.GetCategoryById(filter.ParentId.Value);

            if (MainCat == null) return NotFound();

            ViewBag.ParentTitle = MainCat.DisplayName;

            return View(await _categoriesService.FilterCategoryViewModel(filter));
        }

        #endregion

        #region Edit Article Sub Category

        [HttpGet]
        public async Task<IActionResult> EditSubCategory(ulong Id)
        {
            var cat = await _categoriesService.GetCategoryById((ulong)Id);

            if (cat == null) return NotFound();

            var model = await _categoriesService.FillEditCategoryViewModel(cat);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubCategory(EditCategoryViewModel cat)
        {
            var category = await _categoriesService.GetCategoryById((ulong)cat.CategoryId);
            var MainCategory = await _categoriesService.GetCategoryById((ulong)cat.ParentId);

            if (category == null) return NotFound();

            if (MainCategory == null) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _categoriesService.EditCategoryResult(cat, category);

                switch (result)
                {
                    case EditCategoryResult.success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        return RedirectToAction("ListOfSubCategories", "Categories", new { ParentId = cat.ParentId });

                    case EditCategoryResult.Fiald:
                        TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                        break;

                    case EditCategoryResult.CategoryIsExist:
                        TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است  ";
                        break;
                }
            }

            ViewBag.ParentId = cat.ParentId;
            ViewBag.ParentTitle = MainCategory.DisplayName;

            return View(cat);
        }
        #endregion


    }
}
