using BusinessPortal.Domain.Entities.Advertisement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Advertisement
{
    public class EditAdvertisementFromAdminPanel
    {
        #region Properties

        public ulong AdvertisementID { get; set; }

        public ulong? AddressID { get; set; }

        public ulong? UserId { get; set; }

        [Display(Name = "عنوان آگهی  ")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? Title { get; set; }

        [Display(Name = "توضیحات    ")]
        public string? Description { get; set; }

        [Display(Name = " تصویر آگهی  ")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AdsImage { get; set; }

        [Display(Name = " لینک  ")]
        [Url(ErrorMessage = "لینک وارد شده معتبر نمی باشد")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? AdsUrl { get; set; }

        public AdvertisementStatus AdvertisementStatus { get; set; }

        [Display(Name = "تگ های آگهی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AdvertisementTags { get; set; }

        [Display(Name = "علت عدم تایید")]
        public string? RejectDescription { get; set; }

        #endregion
    }
    public enum EditAdvertisementFromAdminPanelResult
    {
        NotFound,
        [Display(Name = "موفق")]
        Success,
        [Display(Name = "نا موفق ")]
        Faild,
        [Display(Name = "تتظیمات سایت موجود نمی باشد ")]
        SiteSettingNotExist,
        [Display(Name = "تعداد تصویر نامعتبر")]
        ImageCountNotValid,
        ImageIsNotValid,
        FillRejectDescription,
        ImageIsNotFound
    }
}
