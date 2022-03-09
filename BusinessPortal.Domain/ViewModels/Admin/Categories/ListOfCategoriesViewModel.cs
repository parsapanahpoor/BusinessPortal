using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Categories
{
    public class ListOfCategoriesViewModel : BasePaging<Category>
    {
        #region Constructor

        public ListOfCategoriesViewModel()
        {
            filterCatgeoryAdminSideOrder = FilterCatgeoryAdminSideOrder.CreateDate_Des;
            filterCategoryAdminSideState = FilterCategoryAdminSideState.All;
            FilterCategoryAdminSidePriority = FilterCategoryAdminSidePriority.Priority_Des;
        }

        #endregion

        #region Properties

        public ulong? ParentId { get; set; }
        public string? Title { get; set; }
        public string? UniqueName { get; set; }
        public FilterCatgeoryAdminSideOrder filterCatgeoryAdminSideOrder { get; set; }
        public FilterCategoryAdminSideState filterCategoryAdminSideState { get; set; }
        public FilterCategoryAdminSidePriority FilterCategoryAdminSidePriority { get; set; }

        #endregion
    }

    public enum FilterCatgeoryAdminSideOrder
    {
        [Display(Name = "تاریخ ثبت نام - نزولی")]
        CreateDate_Des,
        [Display(Name = "تاریخ ثبت نام - صعودی ")]
        CreateDate_Asc
    }

    public enum FilterCategoryAdminSideState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActive,
        [Display(Name = " غیرفعال ")]
        NotActive
    }

    public enum FilterCategoryAdminSidePriority
    {
        [Display(Name = " نزولی")]
        Priority_Des,
        [Display(Name = " صعودی")]
        Priority_Asc
    }
}
