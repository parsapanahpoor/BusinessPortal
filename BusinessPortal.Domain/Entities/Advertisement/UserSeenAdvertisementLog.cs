using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Advertisement
{
    public class UserSeenAdvertisementLog : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        #endregion

        #region relations 

        public User User { get; set; }

        #endregion
    }
}
