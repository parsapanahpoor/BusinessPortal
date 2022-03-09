using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Access
{
    public class EditRoleViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string Title { get; set; }

        [Display(Name = "نام یکتا")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string RoleUniqueName { get; set; }

        public List<ulong>? Permissions { get; set; }

        public ulong Id { get; set; }
    }

    public enum EditRoleResult
    {
        Success,
        RoleNotFound,
        UniqueNameExists
    }
}
