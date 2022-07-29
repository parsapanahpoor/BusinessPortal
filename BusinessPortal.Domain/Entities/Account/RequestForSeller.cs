using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Account
{
    public class RequestForSeller : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public RequestForSellerStatus RequestForSellerStatus { get; set; }

        #endregion

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        #endregion
    }
}
