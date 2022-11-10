using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Ads
{
    public class FilterAdsViewModel : BasePaging<Domain.Entities.Ads.Ads>
    {
        #region Properties

        public ulong UserId { get; set; }

        [Display(Name = "Title  ")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string? Title { get; set; }

        public FilterAdsState FilterAdsState { get; set; }

        #endregion

    }

    public enum FilterAdsState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "حذف شده")]
        Deleted,
        [Display(Name = "حذف نشده")]
        NotDeleted
    }
}
