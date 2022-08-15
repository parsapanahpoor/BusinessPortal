using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Product
{

    public class CreateProductCategoryViewModel
    {
        [DisplayName("عنوان یکتا")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string UniqueName { get; set; }

        public List<CreateProductCategoryInfoViewModel> ProductCategoryInfos { get; set; }

        public ulong? ParentId { get; set; }
    }

    public class CreateProductCategoryInfoViewModel : BaseInfoViewModel
    {
        [Display(Name = "عنوان دسته بندی")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }
    }

    public enum CreateProductCategoryResult
    {
        Success,
        Fail,
        UniqNameIsExist
    }

}
