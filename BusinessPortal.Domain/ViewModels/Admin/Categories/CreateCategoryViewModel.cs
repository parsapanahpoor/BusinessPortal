using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Categories
{
    public class CreateCategoryViewModel
    {
        #region Properties

        [Display(Name = "عنوان نمایشی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(260)]
        public string DsiplayNamne { get; set; }

        [Display(Name = "عنوان در Url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string URL { get; set; }

        public ulong? ParentId { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "الویت")]
        public int? Priority { get; set; }

        #endregion
    }

    public enum CreateCategoryResult
    {
        Success,
        Faild,
        CategoryIsExist
    }
}
