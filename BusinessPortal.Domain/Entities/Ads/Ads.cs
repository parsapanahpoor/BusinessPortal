using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Ads
{
    public class Ads : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public string AdsImage { get; set; }

        #endregion

        #region relation 

        public User User { get; set; }

        public ICollection<AdsInfo> AdsInfos { get; set; }

        public ICollection<AdsGallery> AdsGalleries { get; set; }

        #endregion
    }
}
