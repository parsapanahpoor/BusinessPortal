using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.Admin.Product;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Product Category

        #region Filter Product Categories

        public async Task<IActionResult> FilterProductCategory(FilterProductCategoryViewModel filter)
        {
            return View(await _productService.FilterProductCategory(filter));
        }

        #endregion

        #region Create Products Category

        [HttpGet]
        public async Task<IActionResult> CreateProductCategory(ulong? parentId)
        {
            ViewBag.parentId = parentId;

            if (parentId != null)
            {
                ViewBag.parentLocation = await _productService.GetProductCategoryById(parentId.Value);
            }

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryViewModel productCategory , IFormFile? productCategoryLogo)
        {
            #region Model State

            if (!ModelState.IsValid)
            {
                ViewBag.parentId = productCategory.ParentId;

                if (productCategory.ParentId != null)
                {
                    ViewBag.parentLocation = await _productService.GetProductCategoryById(productCategory.ParentId.Value);
                }

                return View(productCategory);
            }

            #endregion

            #region Add Product Category  

            var result = await _productService.CreateProductCategory(productCategory , productCategoryLogo);

            switch (result)
            {
                case CreateProductCategoryResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                    if (productCategory.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterProductCategory", new { ParentId = productCategory.ParentId.Value });
                    }
                    return RedirectToAction("FilterProductCategory");

                case CreateProductCategoryResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "عنوان انتخاب شده تکراری است .";
                    break;

                case CreateProductCategoryResult.Fail:
                    TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                    break;
            }

            ViewBag.parentId = productCategory.ParentId;

            if (productCategory.ParentId != null)
            {
                ViewBag.parentLocation = await _productService.GetProductCategoryById(productCategory.ParentId.Value);
            }

            return View(productCategory);

            #endregion
        }

        #endregion

        #region Edit Product Category

        [HttpGet]
        public async Task<IActionResult> EditProductCategory(ulong? productCategoryId)
        {
            #region Get Product Category 

            if (productCategoryId == null) return NotFound();

            var result = await _productService.FillProductCategoryViewModel(productCategoryId.Value);

            if (result == null) return NotFound();

            #endregion

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryViewModel productCategory,  IFormFile? productCategoryLogo)
        {
            #region Is Exist Product Category By Id

            if (productCategory.Id == null) return NotFound();

            var model = await _productService.FillProductCategoryViewModel(productCategory.Id);

            if (model == null) return NotFound();

            #endregion

            #region Model State Validation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده مورد تایید نمی باشد.";

                return View(model);
            }

            #endregion

            #region Edit Product Category

            var result = await _productService.EditProduct(productCategory , productCategoryLogo);

            switch (result)
            {
                case EditProductCategoryResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است.";
                    if (productCategory.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterProductCategory", new { ParentId = productCategory.ParentId.Value });
                    }
                    return RedirectToAction("FilterProductCategory");

                case EditProductCategoryResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "عنوان وارد شده تکراری است .";
                    break;

                case EditProductCategoryResult.Fail:
                    TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
                    break;
            }

            return View(model);

            #endregion
        }

        #endregion

        #region Delete Product Category 
        public async Task<IActionResult> DeleteProductCategory(ulong productCategoryId)
        {
            var result = await _productService.DeleteProductCategory(productCategoryId);

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
