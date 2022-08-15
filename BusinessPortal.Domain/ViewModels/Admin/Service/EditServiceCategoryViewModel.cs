using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Service
{
    public class EditServiceCategoryViewModel
    {
        #region properties

        public ulong Id { get; set; }

        [DisplayName("عنوان یکتا")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string UniqueName { get; set; }

        public List<ServicesCategoryInfo> CurrentInfos { get; set; }

        public List<EditServiceCategoryInfoViewModel> ServiceCategoryInfos { get; set; }

        public ulong? ParentId { get; set; }

        #endregion
    }

    public class EditServiceCategoryInfoViewModel : BaseInfoViewModel
    {
        [Display(Name = "عنوان دسته بندی سرویس")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }
    }

    public enum EditServiceCategoryResult
    {
        Success,
        Fail,
        UniqNameIsExist
    }
}
