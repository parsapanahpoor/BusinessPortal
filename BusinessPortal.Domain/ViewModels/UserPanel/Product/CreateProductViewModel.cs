using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessPortal.Domain.ViewModels.UserPanel.Product
{
    public class CreateProductViewModel
    {
        #region Properties

        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(400, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string ProductCategoryName { get; set; }

        public ulong? ParentId { get; set; }

        public ulong? ProductId { get; set; }

        #endregion
    }
}
