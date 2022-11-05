using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Countries;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Countries
{
    public class FilterCountriesViewModel : BasePaging<Domain.Entities.Countries.Countries>
    {
        #region properties

        public string? UniqueName { get; set; }

        #endregion
    }
}
