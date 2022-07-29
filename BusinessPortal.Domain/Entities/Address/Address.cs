using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Address
{
    public class Address : BaseEntity
    {
        #region Properties

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
        public string Mobile { get; set; }

        #endregion

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("CountryId")]
        public Location.State LocationCountry { get; set; }

        [ForeignKey("StateId")]
        public Location.State LocationState { get; set; }

        [ForeignKey("CityId")]
        public Location.State LocationCity { get; set; }

        public ICollection<Advertisement.Advertisement> Advertisement { get; set; }

        #endregion
    }
}
