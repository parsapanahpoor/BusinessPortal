using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Advertisement
{
    public class AdvertisementSelectedCategory : BaseEntity
    {
        #region Properties

        public ulong AdvertisementID { get; set; }
        public ulong AdsCategoryID { get; set; }

        #endregion

        #region Relations

        [ForeignKey("AdvertisementID")]
        public Advertisement Advertisement { get; set; }

        [ForeignKey("AdsCategoryID")]
        public Category AdvertisementCategory { get; set; }

        #endregion
    }
}
