using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Ads
{
    public class FilterAdsAdminSideViewModel : BasePaging<Entities.Ads.Ads>
    {
        #region properties

        public string? AdsTitle { get; set; }

        #endregion
    }
}
