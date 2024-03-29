﻿using BusinessPortal.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Enums;

namespace BusinessPortal.Domain.Entities.Contact
{
    public class Ticket : BaseEntity
    {
        #region Properties

        [Required]
        public ulong OwnerId { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Required]
        public TicketStatus TicketStatus { get; set; }

        public bool IsReadByAdmin { get; set; }

        public bool IsReadByOwner { get; set; }

        public bool OnWorking { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public User Owner { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; set; }

        #endregion
    }

}
