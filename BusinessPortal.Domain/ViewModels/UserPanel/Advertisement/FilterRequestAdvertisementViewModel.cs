using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Advertisement
{
    public class FilterRequestAdvertisementViewModel : BasePaging<Entities.Advertisement.Advertisement>
    {
        #region Properties

        public ulong? AddressId { get; set; }
        public ulong UserId { get; set; }
        public ulong? CategoryId { get; set; }
        public ulong? ParentCategoryId { get; set; }
        public ulong? SingleCategoryId { get; set; }
        public ulong? SingleStateId { get; set; }
        public bool? SiteSide { get; set; }
        public bool? AdvertisementGroups { get; set; }

        [Display(Name = "Advertisement Title  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string Title { get; set; }


        [Display(Name = "Description    ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Display(Name = "Image  ")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string ImageName { get; set; }

        [Display(Name = "URL  ")]
        [MaxLength(300, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string AdsUrl { get; set; }

        public int? VisitCount { get; set; }
        public bool JustWithImage { get; set; }
        public FilterRequestAdvertisementState FilterRequestAdvertisementState { get; set; }
        public FilterRequestAdvertisementOrder FilterRequestAdvertisementOrder { get; set; }
        public FilterRequestAdvertisementActiveState FilterRequestAdvertisementActiveState { get; set; }
        public FilterRequestAdvertisementImageState FilterRequestAdvertisementImageState { get; set; }

        #endregion

    }

    public enum FilterRequestAdvertisementState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "حذف شده")]
        Deleted,
        [Display(Name = "حذف نشده")]
        NotDeleted
    }
    public enum FilterRequestAdvertisementActiveState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "تایید شده")]
        IsActive,
        [Display(Name = "تایید نشده")]
        NotActive,
        [Display(Name = "در انتظار بررسی ")]
        Waiting,
        [Display(Name = "منقضی شده  ")]
        Expired
    }

    public enum FilterRequestAdvertisementOrder
    {
        [Display(Name = "تاریخ ایجاد (نزولی)")]
        CreateDate_Dec,

        [Display(Name = "تاریخ ایجاد (صعودی)")]
        CreateDate_Asc,

        [Display(Name = "تاریخ شروع (نزولی)")]
        StartDate_Dec,

        [Display(Name = "تاریخ شروع (صعودی)")]
        StartDate_Asc,

        [Display(Name = "تاریخ پایان (نزولی)")]
        EndDate_Dec,

        [Display(Name = "تاریخ پایان (صعودی)")]
        EndDate_Asc,
    }

    public enum FilterRequestAdvertisementImageState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "تصویر دار")]
        WithImage,
        [Display(Name = "بدون تصویر")]
        WithoutImage
    }
}
