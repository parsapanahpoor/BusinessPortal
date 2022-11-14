using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.Entities.Services
{
    public class ProductService : BaseEntity
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

        public ICollection<ProductServiceInfo> ProductServiceInfo { get; set; }

        public List<ProductServiceSelectedService> ProductServiceSelectedService { get; set; }

        #endregion
    }
}
