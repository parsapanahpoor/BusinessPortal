using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Product
{
    public class FilterProductAdminSideViewModel : BasePaging<Domain.Entities.Product.Product>
    {
        #region properties

        public string? AdsTitle { get; set; }

        #endregion
    }
}
