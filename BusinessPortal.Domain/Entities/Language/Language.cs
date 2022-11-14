using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Language
{
    public class Language
    {
        #region properties

        [Key]
        [MaxLength(100)]
        public string LanguageTitle { get; set; }

        #endregion

        #region Navigation

        public ICollection<ServicesCategoryInfo> ServicesCategoryInfos { get; set; }

        public ICollection<ProductCategoryInfo>  ProductCategoryInfos{ get; set; }

        public ICollection<ProductServiceInfo>  ProductServiceInfos{ get; set; }

        #endregion
    }
}
