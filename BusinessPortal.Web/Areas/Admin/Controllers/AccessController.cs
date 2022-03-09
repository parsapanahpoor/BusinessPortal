using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Domain.ViewModels.Access;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class AccessController : AdminBaseController
    {
        #region Roles

        private readonly IPermissionService _permissionService;

        public AccessController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #endregion

        #region Role

        #region Filter Roles

        public async Task<IActionResult> FilterRoles(FilterRolesViewModel filter)
        {
            var result = await _permissionService.FilterRoles(filter);

            return View(result);
        }

        #endregion

        #region Create Role

        public IActionResult CreateRole()
        {
            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel create)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نیست .";
                return View(create);
            }

            if (create.Permissions == null || !create.Permissions.Any())
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "انتخاب حداقل یک دسترسی الزامی می باشد .";
                return View(create);
            }

            var result = await _permissionService.CreateRole(create);

            if (result)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
            }

            TempData[WarningMessage] = "نام یکتا از قبل موجود است .";
            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(create);
        }

        #endregion

        #region Edit Role

        public async Task<IActionResult> EditRole(ulong id)
        {
            var result = await _permissionService.FillEditRoleViewModel(id);

            if (result == null)
            {
                return NotFound();
            }

            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نیست .";
                return View(edit);
            }

            if (edit.Permissions == null || !edit.Permissions.Any())
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "انتخاب حداقل یک دسترسی الزامی می باشد .";
                return View(edit);
            }

            var result = await _permissionService.EditRole(edit);

            switch (result)
            {
                case EditRoleResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
                case EditRoleResult.RoleNotFound:
                    TempData[ErrorMessage] = "نقش مورد نظر یافت نشد .";
                    return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
                case EditRoleResult.UniqueNameExists:
                    TempData[WarningMessage] = "نام یکتا از قبل موجود است .";
                    break;
            }

            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(edit);
        }

        #endregion

        #region Delete Role

        public async Task<IActionResult> DeleteRole(ulong roleId)
        {
            var result = await _permissionService.DeleteRole(roleId);

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
