using BusinessPortal.Domain.Entities.Product;
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
    public class EditProductCategoryViewModel
    {
        #region properties

        public ulong Id { get; set; }

        [DisplayName("عنوان یکتا")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string UniqueName { get; set; }

        public List<ProductCategoryInfo> CurrentInfos { get; set; }

        public List<EditProductCategoryInfoViewModel> ProductCategoryInfos { get; set; }

        public ulong? ParentId { get; set; }

        public string? ImageName { get; set; }

        #endregion
    }

    public class EditProductCategoryInfoViewModel : BaseInfoViewModel
    {
        [Display(Name = "عنوان دسته بندی سرویس")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }
    }

    public enum EditProductCategoryResult
    {
        Success,
        Fail,
        UniqNameIsExist
    }
}
