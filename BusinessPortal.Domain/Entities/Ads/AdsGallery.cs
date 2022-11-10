using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Ads
{
    public class AdsGallery : BaseEntity
    {
        #region properties

        public ulong AdsId { get; set; }

        public string ImageName { get; set; }

        #endregion

        #region relation

        public Ads Ads { get; set; }

        #endregion
    }
}
