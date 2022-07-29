using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Advertisement
{
    public class AdvertisementCategoryViewModel
    {
        #region Properties

        [Display(Name = " GroupName    ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(400, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string GroupName { get; set; }

        [Display(Name = " UrlName    ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(400, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string UrlName { get; set; }

        public ulong? ParentId { get; set; }

        public ulong? CatgeoryId { get; set; }

        #endregion
    }

   
}
