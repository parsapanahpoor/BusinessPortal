using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Enums
{
    public enum TicketStatus
    {
        [Display(Name = "جواب داده شده")] Answered = 1,
        [Display(Name = "درانتظار")] Pending = 2,
        [Display(Name = "بسته شده")] Closed = 3
    }
}
