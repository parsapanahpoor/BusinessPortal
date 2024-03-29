﻿using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.State
{
    public class FilterStateViewModel : BasePaging<Entities.Location.State>
    {
        public string? Title { get; set; }

        public string? UniqeName { get; set; }

        public string? Mobile { get; set; }

        public ulong? ParentId { get; set; }

        public StateStatus StateStatus { get; set; }

        public Entities.Location.State? ParentState { get; set; }
    }

    public enum StateStatus
    {
        [Display(Name = "همه")] All,
        [Display(Name = "حذف نشده")] NotDeleted,
        [Display(Name = "حذف شده")] Deleted,
    }
}
