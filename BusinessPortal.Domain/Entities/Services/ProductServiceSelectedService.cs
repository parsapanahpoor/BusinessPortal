using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Services
{
    public class ProductServiceSelectedService : BaseEntity
    {
        #region Properties

        public ulong ProductServiceId { get; set; }

        public ulong ServiceId { get; set; }

        #endregion

        #region Relations

        [ForeignKey("ProductServiceId")]
        public ProductService ProductService { get; set; }

        [ForeignKey("ServiceId")]
        public ServicesCategory ServicesCategory { get; set; }

        #endregion
    }
}
