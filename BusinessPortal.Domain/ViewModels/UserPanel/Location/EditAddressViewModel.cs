using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Location
{
    public class EditAddressViewModel
    {
        #region Properties

        public ulong AddressId { get; set; }
        public ulong UserId { get; set; }
        public ulong CountryId { get; set; }
        public ulong StateId { get; set; }
        public ulong? CityId { get; set; }

        [Display(Name = "عنوان آدرس")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AddressTitle { get; set; }

        [Display(Name = "آدرس کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserAddress { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[0][9][0-9]{9}$", ErrorMessage = "موبایل وارد شده معتبر نمی باشد")]
        public string Mobile { get; set; }

        public DateTime CreateDate { get; set; }

        #endregion

    }

    public enum EditAddressResult
    {
        Success,
        NotFound
    }
}
