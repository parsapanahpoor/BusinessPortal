using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.UserPanel.ProductService
{
    public class FilterProductServiceViewModel : BasePaging<Domain.Entities.Services.ProductService>
    {
        #region properties

        public ulong UserId { get; set; }

        #endregion
    }
}
