using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Tariff
{
    public class FilterTariffViewModel : BasePaging<Domain.Entities.Tariff.Tariff>
    {
        #region properties

        public string? TariffName { get; set; }

        #endregion
    }
}
