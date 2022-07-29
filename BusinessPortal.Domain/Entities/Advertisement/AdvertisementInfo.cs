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
    public class AdvertisementInfo : BaseEntity
    {
        #region properties

        public ulong AdvertisementId { get; set; }

        [ForeignKey("Language")]
        public string Lang_Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }

        [Display(Name = "توضیحات    ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = " لینک  ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? AdsUrl { get; set; }

        #endregion

        #region Relations

        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public Language.Language Language { get; set; }

        #endregion
    }
}
