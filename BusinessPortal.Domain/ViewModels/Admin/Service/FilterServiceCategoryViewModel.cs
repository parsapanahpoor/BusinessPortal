using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Service
{
    public class FilterServiceCategoryViewModel : BasePaging<ServicesCategoryInfo>
    {
        #region properties

        public string? Title { get; set; }

        public string? UniqueName { get; set; }

        public ulong? ParentId { get; set; }

        public Entities.Services.ServicesCategory ParentServicesCategory { get; set; }

        #endregion
    }
}
