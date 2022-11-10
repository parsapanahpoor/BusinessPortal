using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.Ads
{
    public class FilterAdsSiteSideViewModel
    {
        #region properties

        public ulong AdsId { get; set; }

        public string AdsTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public string Image { get; set; }

        #endregion
    }
}
