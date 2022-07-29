using BusinessPortal.Domain.Entities.Address;
using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel
{
    public class FilterUserAddressViewModel : BasePaging<Address>
    {
        #region Constructor

        public FilterUserAddressViewModel()
        {
            OrderBy = FilterAddressOrder.CreateDate_Dec;
            FilterAddressState = FilterAddressState.All;
        }

        #endregion

        #region Properties

        public ulong UserId { get; set; }
        public string AddresssTitle { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public FilterAddressState FilterAddressState { get; set; }
        public FilterAddressOrder OrderBy { get; set; }

        #endregion

    }

    public enum FilterAddressState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "حذف شده")]
        Deleted,
        [Display(Name = "حذف نشده")]
        NotDeleted
    }
    public enum FilterAddressOrder
    {
        [Display(Name = "تاریخ ثبت (نزولی)")]
        CreateDate_Dec,
        [Display(Name = "تاریخ ثبت (صعودی)")]
        CreateDate_Asc,
    }
}
