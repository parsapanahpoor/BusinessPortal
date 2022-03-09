using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Account
{
    public class User : BaseEntity
    {

        #region MyRegion

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(300, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Username { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(150, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست .")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? Mobile { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? Avatar { get; set; }

        [Display(Name = "کد فعالسازی ایمیل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string EmailActivationCode { get; set; }

        [Display(Name = "کد فعالسازی موبایل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? MobileActivationCode { get; set; }

        public bool IsMobileConfirm { get; set; } = false;

        public bool IsEmailConfirm { get; set; } = false;

        public bool IsBan { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public bool BanForTicket { get; set; }

        public bool BanForComment { get; set; }

        #endregion

        #region Relations

        public ICollection<UserRole> UserRoles { get; set; }

        #endregion

    }
}
