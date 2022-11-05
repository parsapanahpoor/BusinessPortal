using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using BusinessPortal.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BusinessPortal.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class AdvertisementController : UserBaseController
    {
        #region ctor

        public IAdvertisementService _advertisementService { get; set; }

        public IStateService _stateService { get; set; }

        public ICategoriesService _categoryServicec { get; set; }

        private readonly ITariffService _tariffService;


        public AdvertisementController(IAdvertisementService advertisementService, IStateService stateService
            , ICategoriesService categoryService, ITariffService tariffService)
        {
            _advertisementService = advertisementService;
            _stateService = stateService;
            _categoryServicec = categoryService;
            _tariffService = tariffService;
        }

        #endregion

        #region Request Advertisements

        #region List Of Request Advertisements

        public async Task<IActionResult> Index(FilterRequestAdvertisementViewModel filter)
        {
            #region Seed Data

            filter.UserId = User.GetUserId();
            filter.FilterRequestAdvertisementState = FilterRequestAdvertisementState.NotDeleted;
            filter.FilterRequestAdvertisementActiveState = FilterRequestAdvertisementActiveState.All;
            filter.FilterRequestAdvertisementOrder = FilterRequestAdvertisementOrder.CreateDate_Dec;

            #endregion

            return View(await _advertisementService.FilterRequestAdvertisementUserSide(filter));
        }

        #endregion

        #region Create Advertisement

        [HttpGet]
        public async Task<IActionResult> CreateAdvertisement()
        {
            #region Check User Logs

            //if (!await _tariffService.CheckCustomerAdsBaseOnTariff(User.GetUserId()))
            //{
            //    return NotFound();
            //}

            #endregion

            #region Get User Addresses

            var UserAddress = _stateService.GetUserAddressDrwopDown(User.GetUserId());
            ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

            if (UserAddress.Any() == false)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    TempData[WarningMessage] = "You Must Fill Your Address For Create Advertisement ";
                } 
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    TempData[WarningMessage] = "يجب عليك ملء عنوانك لإنشاء إعلان";
                }  
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    TempData[WarningMessage] = "Вы должны заполнить свой адрес для создания рекламы";
                }
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    TempData[WarningMessage] = "Reklam Oluşturmak İçin Adresinizi Doldurmalısınız";
                }
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    TempData[WarningMessage] = "Você deve preencher seu endereço para criar anúncio";
                }

                TempData[WarningMessage] = "برای درج آگهی باید ابتدا آدرس خود را وارد کنید ";
                return RedirectToAction("Index", "Address", new { area = "UserPanel" });
            }

            #endregion

            #region Get Advertisement Categories

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

            #endregion

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text");

            #endregion

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdvertisement(CreateRequestAdvertisementFromUserPanel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var UserAddress = _stateService.GetUserAddressDrwopDown(User.GetUserId());

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "fa-IR")
                    {
                        TempData[WarningMessage] = "دسته بندی های آگهی باید انتخاب شود  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Ads categories must be selected";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Категории объявлений должны быть выбраны";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد فئات الإعلانات";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "As categorias de anúncios devem ser selecionadas";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Reklam kategorileri seçilmelidir";
                    }

                    ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

                    return View(model);
                }

                if (model.AddressID == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "fa-IR")
                    {
                        TempData[WarningMessage] = "آدرس باید انتخاب شود   ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Ads Address must be selected  ";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Ads Address must be selected ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد عنوان الإعلانات";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Reklam Adresi seçilmelidir";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "O endereço do anúncio deve ser selecionado";
                    }

                    ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

                    ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

                    return View(model);
                }

                model.UserId = User.GetUserId();

                var result = await _advertisementService.AddAdvertisementFromUserPanell(model, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case CreateAdvertisementFromUserPanelResult.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[WarningMessage] = "تصویر وارد شده دچار مشکل است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[WarningMessage] = "Ads Image Makes Mistake ";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[WarningMessage] = "Рекламное изображение делает ошибку ";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[WarningMessage] = "صورة الإعلانات خطأ";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[WarningMessage] = "A imagem dos anúncios comete erros";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[WarningMessage] = "Reklam Resmi Hata Yapıyor";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.Success:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[SuccessMessage] = "آگهی مورد نطر با موفقیت ثبت شده است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = " Ads Has been successfully Create ";
                        }    
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = " Объявления успешно созданы ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم إنشاء الإعلانات بنجاح";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Reklamlar Başarıyla Oluşturuldu";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "Os anúncios foram criados com sucesso";
                        }

                        return RedirectToAction("Index", "Advertisement", new { area = "UserPanel" });

                    case CreateAdvertisementFromUserPanelResult.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا فقط عکس با فرمت های معتبر انتخاب کنید";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please select only photos with valid formats";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = " Пожалуйста, выберите только фотографии с допустимыми форматами";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء تحديد الصور ذات التنسيقات الصالحة فقط";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Selecione apenas fotos com formatos válidos";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen yalnızca geçerli biçimlere sahip fotoğrafları seçin";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.Faild:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "ثبت آگهی مورد نظر با مشکل روبرو گردیده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The registration of the desired advertisement has encountered a problem";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "При регистрации желаемого объявления возникла проблема";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "واجه تسجيل الإعلان المطلوب مشكلة";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen reklamın kaydı bir sorunla karşılaştı";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O registro do anúncio desejado encontrou um problema";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.ImageIsNotExist:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا تصویر آگهی را وارد کنید ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please enter the ad image";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, введите рекламное изображение";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء إدخال صورة الإعلان";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira a imagem do anúncio";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen reklam resmini girin";
                        }

                        break;

                    default:
                        break;
                }
            }

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

            ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text");

            #endregion

            return View();
        }

        #endregion

        #region Edit Advertisement From User Panel

        [HttpGet]
        public async Task<IActionResult> EditAdvertisement(ulong Id)
        {
            var advertisement = await _advertisementService.SetEditAdvertisementFromUserPanel(Id);

            if (advertisement == null) return NotFound();

            if (advertisement.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active) return NotFound();

            advertisement.Description = advertisement.Description.ConvertBrToNewLine();

            #region Model Data

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();
            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(Id);

            if (advertisement.AddressID.HasValue)
            {
                ViewBag.AdveritsementAddress = await _advertisementService.GetAddressByAddressId(advertisement.AddressID.Value);
            }

            #endregion

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text", ((advertisement.CountryId.HasValue) ? advertisement.CountryId.Value : null));

            #endregion

            return View(advertisement);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdvertisement(EditRequestAdvertisementFromUserPanel model, IFormFile? ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            #region Model Data

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();
            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(model.AdvertisementID);

            #endregion

            if (model.UserId != User.GetUserId())
            {
                return NotFound();
            }

            var advertisement = await _advertisementService.GetAdvertisementByID(model.AdvertisementID);

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text", ((advertisement.CountriesId.HasValue) ? advertisement.CountriesId.Value : null));

            #endregion

            if (advertisement == null) return NotFound();

            if (advertisement.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active) return NotFound();

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "fa-IR")
                    {
                        TempData[WarningMessage] = "دسته بندی های آگهی باید انتخاب شود  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Ads categories must be selected  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Категории объявлений должны быть выбраны";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد فئات الإعلانات";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "As categorias de anúncios devem ser selecionadas";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Reklam kategorileri seçilmelidir";
                    }

                    return View(model);
                }

                var result = await _advertisementService.EditRequestAdvertisementFromUserPanel(model, ImageName, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case EditRequestAdvertisementFromUserPanelResualt.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[WarningMessage] = "مجموع عکس های وارد شده نباید بیشتر از ده تا باشد!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[WarningMessage] = "The total number of imported photos should not exceed ten";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[WarningMessage] = "Общее количество импортируемых фотографий не должно превышать десяти.";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[WarningMessage] = "يجب ألا يتجاوز العدد الإجمالي للصور المستوردة عشر";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[WarningMessage] = "A imagem dos anúncios comete erros";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[WarningMessage] = "Reklam Resmi Hata Yapıyor";
                        }

                        break;

                    case EditRequestAdvertisementFromUserPanelResualt.NotFound:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "آگهی مورد نظر یافت شند! ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The desired ad was found!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Искомое объявление найдено!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "تم العثور على الإعلان المطلوب!";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen ilan bulundu!";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O anúncio desejado foi encontrado!";
                        }

                        return Redirect("/UserPanel/Advertisement/Index");

                    case EditRequestAdvertisementFromUserPanelResualt.Success:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[SuccessMessage] = "آگهی مورد نظر با موفقیت ویرایش شده است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = "The ad has been successfully edited";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[SuccessMessage] = "Объявление было успешно отредактировано";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم تحرير الإعلان بنجاح";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "O anúncio foi editado com sucesso";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Reklam başarıyla düzenlendi";
                        }

                        return Redirect("/UserPanel/Advertisement/Index");

                    case EditRequestAdvertisementFromUserPanelResualt.Faild:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "ویرایش آگهی مورد نظر با مشکل روبرو گردیده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Editing the desired ad has encountered a problem";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "При редактировании нужного объявления возникла проблема";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "واجه تعديل الإعلان المطلوب مشكلة";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen reklamı düzenlerken bir sorunla karşılaştı";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "A edição do anúncio desejado encontrou um problema";
                        }

                        break;

                    case EditRequestAdvertisementFromUserPanelResualt.SiteSettingNotExist:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "تعداد روز نمایش آگهی در تنظیمات سایت یافت نشده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The number of days the ad was displayed was not found in the site settings";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Количество дней, в течение которых показывалась реклама, не было найдено в настройках сайта";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "لم يتم العثور على عدد أيام عرض الإعلان في إعدادات الموقع";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O número de dias em que o anúncio foi exibido não foi encontrado nas configurações do site";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Reklamın görüntülendiği gün sayısı site ayarlarında bulunamadı";
                        }

                        break;

                    case EditRequestAdvertisementFromUserPanelResualt.FillRejectDescription:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "علت عدم تایید آگهی را وارد کنید";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Enter the reason for not approving the ad";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Укажите причину, по которой объявление не было одобрено.";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "أدخل سبب عدم الموافقة على الإعلان";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Reklamı onaylamama nedenini girin";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira o motivo da não aprovação do anúncio";
                        }

                        break;

                    case EditRequestAdvertisementFromUserPanelResualt.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا فقط عکس با فرمت های معتبر انتخاب شود";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please select only photos with valid formats";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, выберите только фотографии с допустимыми форматами";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء تحديد الصور ذات التنسيقات الصالحة فقط";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Selecione apenas fotos com formatos válidos";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen yalnızca geçerli biçimlere sahip fotoğrafları seçin";
                        }

                        break;

                    case EditRequestAdvertisementFromUserPanelResualt.ImageIsNotFound:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا تصویر آگهی را وارد کنید ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please enter the ad image";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, введите рекламное изображение";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء إدخال صورة الإعلان";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen reklam resmini girin";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira a imagem do anúncio";
                        }

                        break;
                }

            }


            return View(model);
        }


        #endregion

        #region Delete Advertisement

        public async Task<IActionResult> DeleteRequestAdvertisement(ulong id)
        {
            var result = await _advertisementService.DeleteAdvertisementFromUserPanel(id, User.GetUserId());

            if (result)
            {
                if (CultureInfo.CurrentCulture.Name == "fa-IR")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "آگهی مورد نظر با موفقیت حذف شد",
                                             null
                                             );
                }
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "The ad has been successfully Deleted",
                                             null
                                             );
                }
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "Объявление было успешно удалено",
                                             null
                                             );
                } 
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "تم حذف الإعلان بنجاح",
                                             null
                                             );
                } 
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "O anúncio foi excluído com sucesso",
                                             null
                                             );
                } 
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    return JsonResponseStatus.SendStatus(
                                             JsonResponseStatusType.Success,
                                             "Reklam başarıyla Silindi",
                                             null
                                             );
                }
              
            }

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return JsonResponseStatus.SendStatus(
                      JsonResponseStatusType.Warning,
                      "Advertisement NotFound",
                      null
                      );
            }
            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                return JsonResponseStatus.SendStatus(
                      JsonResponseStatusType.Warning,
                      "Реклама не найдена",
                      null
                      );
            } 
            if (CultureInfo.CurrentCulture.Name == "ar-SA")
            {
                return JsonResponseStatus.SendStatus(
                      JsonResponseStatusType.Warning,
                      "الإعلان غير موجود",
                      null
                      );
            } 
            if (CultureInfo.CurrentCulture.Name == "tr-TR")
            {
                return JsonResponseStatus.SendStatus(
                      JsonResponseStatusType.Warning,
                      "Reklam Bulunamadı",
                      null
                      );
            } 
            if (CultureInfo.CurrentCulture.Name == "pt-PT")
            {
                return JsonResponseStatus.SendStatus(
                      JsonResponseStatusType.Warning,
                      "Anúncio não encontrado",
                      null
                      );
            }

            return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Warning,
                    "آگهی مورد نظر یافت شند",
                    null
                    );

        }

        #endregion

        #endregion

        #region On Sale Advertisements

        #region List Of On Sale Advertisements

        public async Task<IActionResult> ListOfOnSaleAdvertisements(FilterOnSaleAdvertisementViewModel filter)
        {
            #region Seed Data

            filter.UserId = User.GetUserId();
            filter.FilterOnSaleAdvertisementState = FilterOnSaleAdvertisementState.NotDeleted;
            filter.FilterOnSaleAdvertisementActiveState = FilterOnSaleAdvertisementActiveState.All;
            filter.FilterOnSaleAdvertisementOrder = FilterOnSaleAdvertisementOrder.CreateDate_Dec;

            #endregion

            return View(await _advertisementService.FilterOnSaleAdvertisementUserSide(filter));
        }

        #endregion

        #region Create On Sale Advertisement

        [HttpGet]
        public async Task<IActionResult> CreateOnSaleAdvertisement()
        {
            #region Check Create Sale Ads Log 

            //if (!await _tariffService.CheckSaleAdsBaseOnTariff(User.GetUserId()))
            //{
            //    return NotFound();
            //}

            #endregion

            #region Get User Addresses

            var UserAddress = _stateService.GetUserAddressDrwopDown(User.GetUserId());
            ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

            if (UserAddress.Any() == false)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    TempData[WarningMessage] = "You Must Fill Your Address For Create Advertisement ";
                }
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    TempData[WarningMessage] = "Вы должны заполнить свой адрес для создания рекламы";
                } 
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    TempData[WarningMessage] = "يجب عليك ملء عنوانك لإنشاء إعلان";
                }
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    TempData[WarningMessage] = "Você deve preencher seu endereço para criar anúncio";
                } 
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    TempData[WarningMessage] = "Reklam Oluşturmak İçin Adresinizi Doldurmalısınız";
                }

                TempData[WarningMessage] = "برای درج آگهی باید ابتدا آدرس خود را وارد کنید ";
                return RedirectToAction("Index", "Address", new { area = "UserPanel" });
            }

            #endregion

            #region Get Advertisement Categories

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

            #endregion

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text");

            #endregion

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnSaleAdvertisement(CreateOnSaleAdvertisementFromUserPanel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var UserAddress = _stateService.GetUserAddressDrwopDown(User.GetUserId());

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Ad categories must be selected  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Категории объявлений должны быть выбраны  ";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد الفئات الإعلانية ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Reklam kategorileri seçilmelidir";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "As categorias de anúncios devem ser selecionadas";
                    }

                    TempData[WarningMessage] = "دسته بندی های آگهی باید انتخاب شود  ";
                    ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

                    return View(model);
                }

                if (model.AddressID == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Address must Be Selected  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Адрес должен быть выбран";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد العنوان";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "O endereço deve ser selecionado";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Adres Seçilmelidir";
                    }

                    TempData[WarningMessage] = "آدرس باید انتخاب شود   ";
                    ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

                    ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

                    return View(model);
                }

                model.UserId = User.GetUserId();

                var result = await _advertisementService.AddOnSaleAdvertisementFromUserPanell(model, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case CreateAdvertisementFromUserPanelResult.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[WarningMessage] = "تصویر وارد شده دچار مشکل است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[WarningMessage] = "Ads Image Makes Mistake ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[WarningMessage] = "Рекламное изображение делает ошибку ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[WarningMessage] = "صورة الإعلانات خطأ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[WarningMessage] = "Reklam Resmi Hata Yapıyor";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[WarningMessage] = "A imagem dos anúncios comete erros";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.Success:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[SuccessMessage] = "آگهی مورد نطر با موفقیت ثبت شده است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = " Ads Has been successfully Create ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = " Объявления успешно созданы ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم إنشاء الإعلانات بنجاح";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "Os anúncios foram criados com sucesso";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Reklamlar Başarıyla Oluşturuldu";
                        }

                        return RedirectToAction("ListOfOnSaleAdvertisements", "Advertisement", new { area = "UserPanel" });
                    case CreateAdvertisementFromUserPanelResult.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا فقط عکس با فرمت های معتبر انتخاب کنید";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "please Insert Photo With Invalid Format";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = " Пожалуйста, выберите только фотографии с допустимыми форматами";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء تحديد الصور ذات التنسيقات الصالحة فقط";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "lütfen Geçersiz Biçimde Fotoğraf Ekle";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "por favor insira foto com formato inválido";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.Faild:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "ثبت آگهی مورد نظر با مشکل روبرو گردیده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The registration of the desired advertisement has encountered a problem";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "При регистрации желаемого объявления возникла проблема";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "واجه تسجيل الإعلان المطلوب مشكلة";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O registro do anúncio desejado encontrou um problema";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen reklamın kaydı bir sorunla karşılaştı";
                        }

                        break;

                    case CreateAdvertisementFromUserPanelResult.ImageIsNotExist:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا تصویر آگهی را وارد کنید ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please enter the ad image";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, введите рекламное изображение";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء إدخال صورة الإعلان";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen reklam resmini girin";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira a imagem do anúncio";
                        }

                        break;

                    default:
                        break;
                }
            }

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();

            ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text");

            #endregion

            return View();
        }

        #endregion

        #region Edit Advertisement From User Panel

        [HttpGet]
        public async Task<IActionResult> EditOnSaleAdvertisement(ulong Id)
        {
            var advertisement = await _advertisementService.SetEditOnSaleAdvertisementFromUserPanel(Id);

            if (advertisement == null) return NotFound();

            if (advertisement.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active) return NotFound();

            advertisement.Description = advertisement.Description.ConvertBrToNewLine();

            #region Model Data

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();
            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(Id);

            if (advertisement.AddressID.HasValue)
            {
                ViewBag.AdveritsementAddress = await _advertisementService.GetAddressByAddressId(advertisement.AddressID.Value);
            }

            #endregion

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text", ((advertisement.CountryId.HasValue) ? advertisement.CountryId.Value : null));

            #endregion

            return View(advertisement);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOnSaleAdvertisement(EditOnSaleAdvertisementFromUserPanel model, IFormFile? ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            #region Model Data

            ViewBag.Categories = await _categoryServicec.GetAllCorrectCategoryForShowInUserPanel();
            ViewBag.SelectedCategories = await _advertisementService.GetAllAdvertisementCategories(model.AdvertisementID);

            #endregion

            if (model.UserId != User.GetUserId())
            {
                return NotFound();
            }

            var advertisement = await _advertisementService.GetAdvertisementByID(model.AdvertisementID);

            if (advertisement == null) return NotFound();

            if (advertisement.AdvertisementStatus == Domain.Entities.Advertisement.AdvertisementStatus.Active) return NotFound();

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "fa-IR")
                    {
                        TempData[WarningMessage] = "دسته بندی های آگهی باید انتخاب شود  ";
                    }
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        TempData[WarningMessage] = "Ads categories must be selected  ";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ru-RU")
                    {
                        TempData[WarningMessage] = "Категории объявлений должны быть выбраны";
                    } 
                    if (CultureInfo.CurrentCulture.Name == "ar-SA")
                    {
                        TempData[WarningMessage] = "يجب تحديد فئات الإعلانات";
                    }
                    if (CultureInfo.CurrentCulture.Name == "pt-PT")
                    {
                        TempData[WarningMessage] = "As categorias de anúncios devem ser selecionadas";
                    }
                    if (CultureInfo.CurrentCulture.Name == "tr-TR")
                    {
                        TempData[WarningMessage] = "Reklam kategorileri seçilmelidir";
                    }

                    return View(model);
                }

                var result = await _advertisementService.EditOnSaleAdvertisementFromUserPanel(model, ImageName, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case EditOnSaleAdvertisementFromUserPanelResualt.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[WarningMessage] = "مجموع عکس های وارد شده نباید بیشتر از ده تا باشد!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[WarningMessage] = "The total number of imported photos should not exceed ten";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[WarningMessage] = "Общее количество импортируемых фотографий не должно превышать десяти.";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[WarningMessage] = "يجب ألا يتجاوز العدد الإجمالي للصور المستوردة عشر";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[WarningMessage] = "İçe aktarılan fotoğrafların toplam sayısı onu geçmemelidir";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[WarningMessage] = "O número total de fotos importadas não deve exceder dez";
                        }

                        break;

                    case EditOnSaleAdvertisementFromUserPanelResualt.NotFound:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "آگهی مورد نظر یافت شند! ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The desired ad was found!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Искомое объявление найдено!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "تم العثور على الإعلان المطلوب!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O anúncio desejado foi encontrado!";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen ilan bulundu!";
                        }

                        return Redirect("/UserPanel/Advertisement/ListOfOnSaleAdvertisements");

                    case EditOnSaleAdvertisementFromUserPanelResualt.Success:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[SuccessMessage] = "آگهی مورد نظر با موفقیت ویرایش شده است ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[SuccessMessage] = "The ad has been successfully edited";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[SuccessMessage] = "Объявление было успешно отредактировано";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[SuccessMessage] = "تم تحرير الإعلان بنجاح";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[SuccessMessage] = "Reklam başarıyla düzenlendi";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[SuccessMessage] = "O anúncio foi editado com sucesso";
                        }

                        return Redirect("/UserPanel/Advertisement/ListOfOnSaleAdvertisements");

                    case EditOnSaleAdvertisementFromUserPanelResualt.Faild:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "ویرایش آگهی مورد نظر با مشکل روبرو گردیده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Editing the desired ad has encountered a problem";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "При редактировании нужного объявления возникла проблема";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "واجه تعديل الإعلان المطلوب مشكلة";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "A edição do anúncio desejado encontrou um problema";
                        }  
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "İstenen reklamı düzenlerken bir sorunla karşılaştı";
                        }

                        break;

                    case EditOnSaleAdvertisementFromUserPanelResualt.SiteSettingNotExist:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "تعداد روز نمایش آگهی در تنظیمات سایت یافت نشده است  ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "The number of days the ad was displayed was not found in the site settings";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Количество дней, в течение которых показывалась реклама, не было найдено в настройках сайта";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "لم يتم العثور على عدد أيام عرض الإعلان في إعدادات الموقع";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Reklamın görüntülendiği gün sayısı site ayarlarında bulunamadı";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "O número de dias em que o anúncio foi exibido não foi encontrado nas configurações do site";
                        }

                        break;

                    case EditOnSaleAdvertisementFromUserPanelResualt.FillRejectDescription:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "علت عدم تایید آگهی را وارد کنید";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Enter the reason for not approving the ad";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Укажите причину, по которой объявление не было одобрено.";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "أدخل سبب عدم الموافقة على الإعلان";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira o motivo da não aprovação do anúncio";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Reklamı onaylamama nedenini girin";
                        }

                        break;

                    case EditOnSaleAdvertisementFromUserPanelResualt.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا فقط عکس با فرمت های معتبر انتخاب شود";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please select only photos with valid formats";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, выберите только фотографии с допустимыми форматами";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء تحديد الصور ذات التنسيقات الصالحة فقط";
                        }
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen yalnızca geçerli biçimlere sahip fotoğrafları seçin";
                        }
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Selecione apenas fotos com formatos válidos";
                        }

                        break;

                    case EditOnSaleAdvertisementFromUserPanelResualt.ImageIsNotFound:
                        if (CultureInfo.CurrentCulture.Name == "fa-IR")
                        {
                            TempData[ErrorMessage] = "لطفا تصویر آگهی را وارد کنید ";
                        }
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            TempData[ErrorMessage] = "Please enter the ad image";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ru-RU")
                        {
                            TempData[ErrorMessage] = "Пожалуйста, введите рекламное изображение";
                        }
                        if (CultureInfo.CurrentCulture.Name == "ar-SA")
                        {
                            TempData[ErrorMessage] = "الرجاء إدخال صورة الإعلان";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "pt-PT")
                        {
                            TempData[ErrorMessage] = "Insira a imagem do anúncio";
                        } 
                        if (CultureInfo.CurrentCulture.Name == "tr-TR")
                        {
                            TempData[ErrorMessage] = "Lütfen reklam resmini girin";
                        }

                        break;
                }


            }

            #region Get All Countries

            var countries = _advertisementService.ListOfCountriesForDrowpDown();
            ViewData["Countriees"] = new SelectList(countries, "Value", "Text", ((advertisement.CountriesId.HasValue) ? advertisement.CountriesId.Value : null));

            #endregion

            return View(model);
        }

        #endregion

        #endregion

    }
}
