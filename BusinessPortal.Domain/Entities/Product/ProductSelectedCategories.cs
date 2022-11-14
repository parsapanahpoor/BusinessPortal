using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Product
{
    public class ProductSelectedCategories : BaseEntity
    {
        #region Properties

        public ulong ProductId { get; set; }

        public ulong CategoryId { get; set; }

        #endregion

        #region Relations

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }

        #endregion
    }
}
