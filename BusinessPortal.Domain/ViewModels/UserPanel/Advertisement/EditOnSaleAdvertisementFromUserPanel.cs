using BusinessPortal.Domain.Entities.Advertisement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Advertisement
{
    public class EditOnSaleAdvertisementFromUserPanel
    {
        #region Properties

        public ulong AdvertisementID { get; set; }

        public ulong? AddressID { get; set; }

        public ulong? UserId { get; set; }

        [Display(Name = "Title  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }


        [Display(Name = "Description    ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Description { get; set; }

        [Display(Name = " Image  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string? AdsImage { get; set; }

        [Display(Name = " Country Id  ")]
        public ulong? CountryId { get; set; }

        [Display(Name = " AdsUrl  ")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string? AdsUrl { get; set; }

        public AdvertisementStatus AdvertisementStatus { get; set; }

        [Display(Name = "AdvertisementTags")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string AdvertisementTags { get; set; }

        public bool FromCustomer { get; set; }

        public bool FromEmployee { get; set; }

        public string? DeclineMessage { get; set; }

        #endregion
    }
    public enum EditOnSaleAdvertisementFromUserPanelResualt
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
        [Display(Name = "تصویر آگهی را وارد کنید ")]
        ImageIsNotFound
    }
}
