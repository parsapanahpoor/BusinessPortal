using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Service
{
    public class FilterProductServiceAdminSideViewModel : BasePaging<Domain.Entities.Services.ProductService>
    {
        #region properties

        public string? AdsTitle { get; set; }

        #endregion
    }
}
