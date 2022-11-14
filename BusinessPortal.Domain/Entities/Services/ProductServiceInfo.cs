using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.Entities.Services
{
    public class ProductServiceInfo : BaseEntity
    {
        #region properties

        public ulong ProductServiceId { get; set; }

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

        [ForeignKey("ProductServiceId")]
        public ProductService ProductService { get; set; }

        public Language.Language Language { get; set; }

        #endregion
    }
}
