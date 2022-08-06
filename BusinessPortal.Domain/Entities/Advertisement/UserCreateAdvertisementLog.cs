using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Advertisement
{
    public class UserCreateAdvertisementLog : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public bool FromCustomer { get; set; }

        public bool FromEmployee { get; set; }

        #endregion

        #region realtions

        public User User { get; set; }

        #endregion
    }
}
