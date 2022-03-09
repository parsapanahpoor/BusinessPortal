using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Account
{
    public class Permission : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string Title { get; set; }

        [Display(Name = "نام یکتا")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PermissionUniqueName { get; set; }

        public ulong? ParentId { get; set; }

        #endregion
    }
}
