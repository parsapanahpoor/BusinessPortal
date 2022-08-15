using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Product
{
    public class ProductCategory : BaseEntity
    {
        #region properties

        [Required]
        [MaxLength(200)]
        public string UniqueName { get; set; }

        public ulong? ParentId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public ICollection<ProductCategoryInfo> ProductCategoryInfos { get; set; }

        public ProductCategory Parent { get; set; }

        #endregion
    }
}
