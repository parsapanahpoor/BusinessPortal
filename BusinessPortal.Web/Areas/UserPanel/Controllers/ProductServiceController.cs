using BusinessPortal.Application.Convertors;
using BusinessPortal.Application.Extensions;
using BusinessPortal.Application.Services.Implementation;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Domain.ViewModels.UserPanel.Advertisement;
using BusinessPortal.Domain.ViewModels.UserPanel.ProductService;
using BusinessPortal.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BusinessPortal.Web.Areas.UserPanel.Controllers
{
    public class ProductServiceController : UserBaseController
    {
        #region Ctor

        private readonly IServiceService _serviceService;

        private readonly IStateService _stateService;

        public ProductServiceController(IServiceService serviceService, IStateService stateService)
        {
            _serviceService = serviceService;
            _stateService = stateService;
        }

        #endregion

        #region Filter Service

        public async Task<IActionResult> Index(FilterProductServiceViewModel filter)
        {
            #region Seed Data

            filter.UserId = User.GetUserId();

            #endregion

            return View(await _serviceService.FilterProductServiceUserSide(filter));
        }


        #endregion

        #region Add Service

        [HttpGet]
        public async Task<IActionResult> CreateAdvertisement()
        {
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

            #region Get Service Categories

            ViewBag.Categories = await _serviceService.FillCreateProductServiceViewModel();

            #endregion

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdvertisement(AddProductSeviceViewModel model, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            var UserAddress = _stateService.GetUserAddressDrwopDown(User.GetUserId());
            ViewData["Address"] = new SelectList(UserAddress, "Value", "Text");

            #region Get Service Categories

            ViewBag.Categories = await _serviceService.FillCreateProductServiceViewModel();

            #endregion

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "bg-BG")
                    {
                        TempData[WarningMessage] = "دسته بندی های خدمت باید انتخاب شود  ";
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

                    return View(model);
                }

                if (model.AddressID == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    return View(model);
                }

                model.UserId = User.GetUserId();

                var result = await _serviceService.AddProductServiceFromUserPanel(model, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case CreateServiceFromUserPanelResult.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case CreateServiceFromUserPanelResult.Success:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
                        {
                            TempData[SuccessMessage] = "محصول خدماتی مورد نطر با موفقیت ثبت شده است ";
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

                        return RedirectToAction("Index", "ProductService", new { area = "UserPanel" });

                    case CreateServiceFromUserPanelResult.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case CreateServiceFromUserPanelResult.Faild:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case CreateServiceFromUserPanelResult.ImageIsNotExist:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

            return View(model);
        }

        #endregion

        #region Edit Advertisement From User Panel

        [HttpGet]
        public async Task<IActionResult> EditAdvertisement(ulong Id)
        {
            #region Fill Model

            //Fill View Model
            var advertisement = await _serviceService.FillEditProductServiceViewModel(Id);
            if (advertisement == null) return NotFound();

            advertisement.Description = advertisement.Description.ConvertBrToNewLine();

            #endregion

            #region Get Service Categories

            ViewBag.Categories = await _serviceService.FillCreateProductServiceViewModel();
            ViewBag.SelectedCategories = await _serviceService.GetAllPRoductServiceCategories(Id);

            if (advertisement.AddressID.HasValue)
            {
                ViewBag.AdveritsementAddress = await _serviceService.GetAddressByAddressId(advertisement.AddressID.Value);
            }

            #endregion

            #region Get All Countries

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

            return View(advertisement);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdvertisement(EditProductServiceViewModel model, IFormFile? ImageName, List<IFormFile> upload_imgs, List<ulong> SelectedCategory)
        {
            #region Get Service Categories

            ViewBag.Categories = await _serviceService.FillCreateProductServiceViewModel();
            ViewBag.SelectedCategories = await _serviceService.GetAllPRoductServiceCategories(model.AdvertisementID);

            if (model.AddressID.HasValue)
            {
                ViewBag.AdveritsementAddress = await _serviceService.GetAddressByAddressId(model.AddressID.Value);
            }

            #endregion

            if (model.UserId != User.GetUserId())
            {
                return NotFound();
            }

            var advertisement = await _serviceService.GetProductServiceById(model.AdvertisementID);

            #region Get All Countries

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

                TempData[WarningMessage] = "برای درج  باید ابتدا آدرس خود را وارد کنید ";
                return RedirectToAction("Index", "Address", new { area = "UserPanel" });
            }


            #endregion

            if (advertisement == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (SelectedCategory == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "bg-BG")
                    {
                        TempData[WarningMessage] = "دسته بندی های باید انتخاب شود  ";
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

                var result = await _serviceService.EditProductServiceFromUserPanel(model, ImageName, upload_imgs, SelectedCategory);

                switch (result)
                {
                    case EditRequestProductServiceFromUserPanelResualt.ImageCountNotValid:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case EditRequestProductServiceFromUserPanelResualt.NotFound:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
                        {
                            TempData[ErrorMessage] = "خدمت مورد نظر یافت شند! ";
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

                        return Redirect("/UserPanel/ProductService/Index");

                    case EditRequestProductServiceFromUserPanelResualt.Success:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
                        {
                            TempData[SuccessMessage] = "خدمت مورد نظر با موفقیت ویرایش شده است ";
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

                        return Redirect("/UserPanel/ProductService/Index");

                    case EditRequestProductServiceFromUserPanelResualt.Faild:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
                        {
                            TempData[ErrorMessage] = "ویرایش خدمت مورد نظر با مشکل روبرو گردیده است  ";
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

                    case EditRequestProductServiceFromUserPanelResualt.SiteSettingNotExist:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case EditRequestProductServiceFromUserPanelResualt.FillRejectDescription:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case EditRequestProductServiceFromUserPanelResualt.ImageIsNotValid:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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

                    case EditRequestProductServiceFromUserPanelResualt.ImageIsNotFound:
                        if (CultureInfo.CurrentCulture.Name == "bg-BG")
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
            var result = await _serviceService.DeleteProductServiceFromUserPanel(id, User.GetUserId());

            if (result == true)
            {
                if (CultureInfo.CurrentCulture.Name == "bg-BG")
                {
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                }
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    TempData[SuccessMessage] = "Please select at least one image.";
                }
                if (CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    TempData[SuccessMessage] = "Операция завершена успешно.";
                }
                if (CultureInfo.CurrentCulture.Name == "ar-SA")
                {
                    TempData[SuccessMessage] = "تمت العملية بنجاح.";
                }
                if (CultureInfo.CurrentCulture.Name == "pt-PT")
                {
                    TempData[SuccessMessage] = "A operação foi concluída com sucesso.";
                }
                if (CultureInfo.CurrentCulture.Name == "tr-TR")
                {
                    TempData[SuccessMessage] = "İşlem başarıyla tamamlandı.";
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        #endregion

    }
}
