using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Slogan
{
    public class Slogan : BaseEntity
    {
        #region properties

        #endregion

        #region relations

        public ICollection<SloganInfo> SloganInfo { get; set; }

        #endregion
    }
}
