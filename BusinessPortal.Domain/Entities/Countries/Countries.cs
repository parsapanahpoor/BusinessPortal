using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Countries
{
    public class Countries : BaseEntity
    {
        #region properties

        public string FlagName { get; set; }

        public string CountryUniqueName { get; set; }

        #endregion

        #region relation

        public ICollection<Advertisement.Advertisement> Advertisement { get; set; }

        #endregion
    }
}
