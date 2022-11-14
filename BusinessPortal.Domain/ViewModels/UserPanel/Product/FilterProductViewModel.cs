using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Product
{
    public class FilterProductViewModel : BasePaging<Domain.Entities.Product.Product>
    {
        #region properties

        public ulong UserId { get; set; }

        #endregion
    }
}
