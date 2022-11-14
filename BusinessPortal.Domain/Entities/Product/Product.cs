using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Location;
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
    public class Product : BaseEntity
    {
        #region Properties

        public ulong? AddressId { get; set; }

        public ulong UserId { get; set; }

        [Display(Name = "تصویر ")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageName { get; set; }

        #endregion

        #region relation

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("AddressId")]
        public State State { get; set; }

        public ICollection<ProductInfo> ProductInfo { get; set; }

        public List<ProductSelectedCategories> ProductSelectedCategories { get; set; }

        #endregion
    }
}
