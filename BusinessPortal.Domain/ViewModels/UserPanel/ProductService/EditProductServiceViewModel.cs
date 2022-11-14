using BusinessPortal.Domain.Entities.Advertisement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.ViewModels.UserPanel.ProductService
{
    public class EditProductServiceViewModel
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

        #endregion
    }
    public enum EditRequestProductServiceFromUserPanelResualt
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
