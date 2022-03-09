﻿using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Account
{
    public class RolePermission : BaseEntity
    {
        #region properties

        public ulong RoleId { get; set; }

        public ulong PermissionId { get; set; }

        #endregion

        #region relation

        public Role Role { get; set; }

        #endregion
    }
}
