using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.BrowseCategory
{
    public class Category : BaseEntity
    {
        #region properties

        [Display(Name = " عنوان نمایشی دسته بندی     ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayName { get; set; }

        [Display(Name = " عنوان  URL   ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UrlName { get; set; }

        public ulong? ParentId { get; set; }

        public int Priority { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        [ForeignKey("ParentId")]
        public ICollection<Category> Categories { get; set; }

        #endregion
    }
}
