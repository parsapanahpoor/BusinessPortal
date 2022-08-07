using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Ticket
{
    public class AdminFilterTicketViewModel : BasePaging<BusinessPortal.Domain.Entities.Contact.Ticket>
    {
        public AdminFilterTicketViewModel()
        {
            AdminTicketFilterSeenByAdminStatus = AdminTicketFilterSeenByAdminStatus.All;
            AdminTicketFilterSeenByUserStatus = AdminTicketFilterSeenByUserStatus.All;
            AdminTicketFilterStatus = AdminTicketFilterStatus.All;
            AdminTicketFilterOnWorkingStatus = AdminTicketFilterOnWorkingStatus.All;
        }

        public string? TicketTitle { get; set; }

        public string? OwnerName { get; set; }

        public AdminTicketFilterSeenByAdminStatus AdminTicketFilterSeenByAdminStatus { get; set; }

        public AdminTicketFilterSeenByUserStatus AdminTicketFilterSeenByUserStatus { get; set; }

        public AdminTicketFilterStatus AdminTicketFilterStatus { get; set; }

        public AdminTicketFilterOnWorkingStatus AdminTicketFilterOnWorkingStatus { get; set; }

        public string? UserEmail { get; set; }
    }

    public enum AdminTicketFilterSeenByAdminStatus
    {
        [Display(Name = "همه")] All,
        [Display(Name = "دیده شده از سمت ادمین")] SeenByAdmin,
        [Display(Name = "دیده نشده از طرف ادمین")] NotSeenByAdmin
    }

    public enum AdminTicketFilterSeenByUserStatus
    {
        [Display(Name = "همه")] All,
        [Display(Name = "دیده شده از سمت کاربر")] SeenByUser,
        [Display(Name = "دیده نشده از سمت کاربر")] NotSeenByUser
    }

    public enum AdminTicketFilterStatus
    {
        [Display(Name = "همه ")] All,
        [Display(Name = "پاسخ داده شده")] Answered,
        [Display(Name = "درحال برسی")] Pending,
        [Display(Name = "بسته شده")] Closed
    }

    public enum AdminTicketFilterOnWorkingStatus
    {
        [Display(Name = "همه")] All,
        [Display(Name = "درحال برسی")] OnWorking,
        [Display(Name = "NotWorking")] NotWorking
    }
}
