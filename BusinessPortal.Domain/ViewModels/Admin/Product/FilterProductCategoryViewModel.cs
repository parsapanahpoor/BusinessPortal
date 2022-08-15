using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Product
{
    public class FilterProductCategoryViewModel : BasePaging<ProductCategoryInfo>
    {
        #region properties

        public string? Title { get; set; }

        public string? UniqueName { get; set; }

        public ulong? ParentId { get; set; }

        public Entities.Product.ProductCategory ParentProductCategory { get; set; }

        #endregion
    }
}
