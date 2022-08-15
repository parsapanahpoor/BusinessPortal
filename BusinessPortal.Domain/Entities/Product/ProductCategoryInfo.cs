using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Product
{
    public class ProductCategoryInfo : BaseEntity
    {
        #region properties

        public string LanguageId { get; set; }

        public ulong ProductCategoryId { get; set; }

        [Required]
        [MaxLength(260)]
        public string Title { get; set; }

        #endregion

        #region relation

        public Language.Language Language { get; set; }

        public ProductCategory ProductCategory { get; set; }

        #endregion
    }
}
