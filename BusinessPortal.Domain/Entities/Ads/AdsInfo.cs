using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Ads
{
    public class AdsInfo : BaseEntity
    {
        #region properties

        public string AdsName { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [ForeignKey("Language")]
        public string Lang_Id { get; set; }

        public ulong AdsId { get; set; }

        #endregion

        #region relation

        public Ads Ads { get; set; }

        public Language.Language Language { get; set; }

        #endregion
    }
}
