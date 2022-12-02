using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessPortal.Domain.ViewModels.Common;
using BusinessPortal.Domain.Entities.Slogan;

namespace BusinessPortal.Domain.ViewModels.Admin.Slogan
{
    public class CreateOrEditSloganViewModel
    {
        #region properties

        public List<SloganInfo> CurrentInfos { get; set; }

        public List<CreateOrEditSloganInfoViewModel> CreateOrEditSloganInfo { get; set; }

        #endregion
    }

    public class CreateOrEditSloganInfoViewModel : BaseInfoViewModel
    {
        [Display(Name = "عنوان ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }
    }
}
