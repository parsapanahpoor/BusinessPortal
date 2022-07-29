using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Advertisement
{
    public class FilterAdvertisementAdminSidedViewModel : BasePaging<Entities.Advertisement.Advertisement>
    {
        #region Properties

        public ulong? AddressId { get; set; }
        public ulong? CategoryId { get; set; }
        public ulong? ParentCategoryId { get; set; }
        public ulong? SingleCategoryId { get; set; }
        public ulong? SingleStateId { get; set; }
        public bool? SiteSide { get; set; }
        public bool? AdvertisementGroups { get; set; }
        public string? Username { get; set; }

        [Display(Name = "عنوان آگهی  ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? Title { get; set; }


        [Display(Name = "توضیحات    ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Display(Name = " تصویر آگهی  ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageName { get; set; }

        [Display(Name = " لینک  ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AdsUrl { get; set; }

        public int? VisitCount { get; set; }
        public bool JustWithImage { get; set; }
        public FilterAdvertisementState FilterAdvertisementState { get; set; }
        public FilterAdvertisementOrder FilterAdvertisementOrder { get; set; }
        public FilterAdvertisementActiveState FilterAdvertisementActiveState { get; set; }
        public FilterAdvertisementImageState FilterAdvertisementImageState { get; set; }
        public FilterAdvertisementGender FilterAdvertisementGender { get; set; }

        #endregion

    }
    public enum FilterAdvertisementState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "حذف شده")]
        Deleted,
        [Display(Name = "حذف نشده")]
        NotDeleted
    }
    public enum FilterAdvertisementActiveState
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

    public enum FilterAdvertisementOrder
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

    public enum FilterAdvertisementImageState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "تصویر دار")]
        WithImage,
        [Display(Name = "بدون تصویر")]
        WithoutImage
    }

    public enum FilterAdvertisementGender
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فروشی")]
        OnSale,
        [Display(Name = "درخواستی ")]
        Requested
    }
}
