using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Location
{
    public class State : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "نام یکتا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UniqueName { get; set; }

        public ulong? ParentId { get; set; }

        [Display(Name = "آیکون کشور")]
        public string? IconeName { get; set; }

        #endregion

        #region Relations

        public State? Parent { get; set; }

        public ICollection<State> Children { get; set; }

        [InverseProperty("LocationCountry")]
        public ICollection<Address.Address> AddressesCountry { get; set; }

        [InverseProperty("LocationState")]
        public ICollection<Address.Address> AddressesState { get; set; }

        [InverseProperty("LocationCity")]
        public ICollection<Address.Address> AddressesCity { get; set; }

        #endregion
    }
}
