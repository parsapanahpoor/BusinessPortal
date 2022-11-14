using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.Entities.Product
{
    public class ProductInfo : BaseEntity
    {
        #region properties

        public ulong ProductId { get; set; }

        [ForeignKey("Language")]
        public string Lang_Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }

        [Display(Name = "توضیحات    ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        #endregion

        #region relation 

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Language.Language Language { get; set; }

        #endregion
    }
}
