using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.UserPanel;
using BusinessPortal.Domain.ViewModels.UserPanel.Location;
using BusinessPortal.Web.Http;
using BusinessPortal.Web.HttpManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BusinessPortal.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class AddressController : UserBaseController
    {
        #region Ctor

        public IStateService _stateService { get; set; }

        public AddressController(IStateService stateService)
        {
            _stateService = stateService;
        }

        #endregion

        #region List Of User States

        public async Task<IActionResult> Index(FilterUserAddressViewModel filter)
        {
            #region Get UserId

            filter.UserId = User.GetUserId();

            #endregion

            return View(await _stateService.FilterAddresses(filter));
        }

        #endregion

        #region Create Address

        [HttpGet]
        public IActionResult CreateAddress()
        {

            #region Model Data

            var countries = _stateService.GetCountriesForDropdown();
            ViewData["Countries"] = new SelectList(countries, "Value", "Text");
            ViewData["UserId"] = User.GetUserId();

            #endregion

            return View(new CreateAddressFromUserPanel
            {
                UserId = User.GetUserId()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(CreateAddressFromUserPanel createAddressFromUserPanel)
        {

            #region Model Data

            var countries = _stateService.GetCountriesForDropdown();
            ViewData["Countries"] = new SelectList(countries, "Value", "Text");
            createAddressFromUserPanel.UserId = User.GetUserId();

            #endregion

            if (ModelState.IsValid)
            {
                var result = await _stateService.CreateAddressByUser(createAddressFromUserPanel);
                switch (result)
                {
                    case CreateAddressFormUserPanelResult.Success:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[SuccessMessage] = "آدرس با موفقیت ثبت شده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = "The requested address was successfully registered!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[SuccessMessage] = "Запрошенный адрес успешно зарегистрирован!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم تسجيل العنوان المطلوب بنجاح!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "İstenen adres başarıyla kaydedildi";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "O endereço solicitado foi registrado com sucesso";
                        }

                        return RedirectToAction("Index", new { UserId = createAddressFromUserPanel.UserId });

                    case CreateAddressFormUserPanelResult.Error:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "عملیات با شکست مواجه شد!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = "The operation failed!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[SuccessMessage] = "Операция не удалась!";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "فشلت العملية!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Operasyon başarısız";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "A operação falhou";
                        }

                        break;
                }
            }

            return View(createAddressFromUserPanel);
        }

        #endregion

        #region GetSubGroups

        public IActionResult GetSubGroups(ulong id)
        {
            #region English Language

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "Please Select ",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Portuguese Language

            if (CultureInfo.CurrentCulture.Name == "pt-PT")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "Por favor selecione ",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Turkish Language

            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "Lütfen seçin ",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Russian Language

            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "Пожалуйста выберите ",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Arabic Language

            if (CultureInfo.CurrentCulture.Name == "ar-SA")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "الرجاء التحديد",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Turkish Language

            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                List<SelectListItem> englishList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "seçme",Value = ""}
            };
                englishList.AddRange(_stateService.GetSubLocationForDropDown(id));
                return Json(new SelectList(englishList, "Value", "Text"));
            }

            #endregion

            #region Persian Language

            List<SelectListItem> farsiList = new List<SelectListItem>()
            {

                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            farsiList.AddRange(_stateService.GetSubLocationForDropDown(id));
            return Json(new SelectList(farsiList, "Value", "Text"));

            #endregion

        }

        #endregion

        #region Edit Address

        [HttpGet]
        public async Task<IActionResult> EditAddress(ulong addressId)
        {
            var result = await _stateService.GetAddressForEditByUser(addressId);

            if (result == null) return NotFound();

            #region Model Data

            var countries = _stateService.GetCountriesForDropdown();
            ViewData["Countries"] = new SelectList(countries, "Value", "Text", result.CountryId);

            var states = _stateService.GetSubLocationForDropDown(result.CountryId);
            ViewData["States"] = new SelectList(states, "Value", "Text", result.StateId);

            var cities = _stateService.GetSubLocationForDropDown(result.StateId);
            ViewData["Cities"] = new SelectList(cities, "Value", "Text", result.CityId);

            #endregion

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(EditAddressViewModel editAddressViewModel)
        {
            var result = await _stateService.GetAddressForEditByUser(editAddressViewModel.AddressId);

            if (result == null) return NotFound();

            #region Data Model

            var countries = _stateService.GetCountriesForDropdown();
            ViewData["Countries"] = new SelectList(countries, "Value", "Text", result.CountryId);

            var states = _stateService.GetSubLocationForDropDown(result.CountryId);
            ViewData["States"] = new SelectList(states, "Value", "Text", result.StateId);

            var cities = _stateService.GetSubLocationForDropDown(result.StateId);
            ViewData["Cities"] = new SelectList(cities, "Value", "Text", result.CityId);

            #endregion

            var userId = await _stateService.GetUserIdByAddressId(editAddressViewModel.AddressId);

            if (ModelState.IsValid)
            {
                var res = await _stateService.EditAddressByUser(editAddressViewModel);
                switch (res)
                {
                    case EditAddressResult.Success:
                        if (CultureInfo.CurrentCulture.Name == "da-IR")
                        {
                            TempData[SuccessMessage] = "ویرایش آدرس با موفقیت انجام شد!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = "Address editing completed successfully!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[SuccessMessage] = "Редактирование адреса успешно завершено!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم تحرير العنوان بنجاح!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Adres düzenleme başarıyla tamamlandı";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "Edição de endereço concluída com sucesso";
                        }
                        return RedirectToAction("Index", new { UserId = userId });

                    case EditAddressResult.NotFound:
                        break;
                }
            }

            return View(editAddressViewModel);
        }


        #endregion

        #region Delete Address

        public async Task<IActionResult> DeleteAddress(ulong addressId)
        {
            var result = await _stateService.SoftDeleteAddressByUser(addressId);

            #region English Language

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                if (result)
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "The address was successfully deleted!",
                    null
                    );
                }
                else
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "The desired address could not be found!",
                    null
                    );
                }
            }

            #endregion

            #region Turkish Language

            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                if (result)
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "Adres başarıyla silindi!",
                    null
                    );
                }
                else
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "İstenilen adres bulunamadı!",
                    null
                    );
                }
            }

            #endregion

            #region Portuguese Language

            if (CultureInfo.CurrentCulture.Name == "pt-PT")
            {
                if (result)
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "O endereço foi excluído com sucesso!",
                    null
                    );
                }
                else
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "O endereço desejado não foi encontrado!",
                    null
                    );
                }
            }

            #endregion

            #region Arabic Language

            if (CultureInfo.CurrentCulture.Name == "ar-SA")
            {
                if (result)
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "تم حذف العنوان بنجاح!",
                    null
                    );
                }
                else
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "تعذر العثور على العنوان المطلوب!",
                    null
                    );
                }
            }

            #endregion

            #region Russian Language

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                if (result)
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "Адрес был успешно удален!",
                    null
                    );
                }
                else
                {
                    return Http.JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "Не удалось найти нужный адрес!",
                    null
                    );
                }
            }

            #endregion

            #region Persian Language

            if (result)
            {
                return Http.JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Success,
                "آدرس مورد نظر با موفقیت حذف شد!",
                null
                );
            }
            else
            {
                return Http.JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Danger,
                "آدرس مورد نظر یافت نشد!",
                null
                );
            }

            #endregion

        }

        #endregion

    }
}
