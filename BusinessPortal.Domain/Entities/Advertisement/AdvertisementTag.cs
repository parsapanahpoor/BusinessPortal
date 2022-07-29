using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Advertisement
{
    public class AdvertisementTag : BaseEntity
    {
        #region Property
        public ulong AdvertisementId { get; set; }

        [Display(Name = "تگ آگهی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TagTitle { get; set; }

        #endregion

        #region Relation

        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        #endregion
    }
}
