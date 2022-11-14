using BusinessPortal.Domain.Entities.Advertisement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Product
{
    public class AddProductViewModel
    {
        #region Properties

        [Display(Name = "MainCat  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public ulong MainCat { get; set; }


        [Display(Name = "SubCat  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public ulong SubCat { get; set; }


        [Display(Name = "AddressID  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public ulong? AddressID { get; set; }

        public ulong? UserId { get; set; }

        [Display(Name = "Title  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }

        [Display(Name = "Description  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(400, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Description { get; set; }

        [Display(Name = " AdsImage  ")]
        [MaxLength(400, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string? AdsImage { get; set; }

        #endregion
    }

    public enum CreateProductFromUserPanelResult
    {
        [Display(Name = "موفق")]
        Success,
        [Display(Name = "نا موفق ")]
        Faild,
        [Display(Name = " تنظیمات سایت وجود ندارد  ")]
        SiteSettingNotExist,
        [Display(Name = "تعداد عکس نامعتبر")]
        ImageCountNotValid,
        ImageIsNotValid,
        [Display(Name = "لطفا تصویر را وارد کنید ")]
        ImageIsNotExist
    }
}
