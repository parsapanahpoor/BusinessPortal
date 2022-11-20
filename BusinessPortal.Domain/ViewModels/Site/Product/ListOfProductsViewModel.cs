using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.Product
{
    public class ListOfProductsViewModel
    {
        #region properties

        public ulong PRoductId { get; set; }

        public string ProductTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public string Image { get; set; }

        #endregion
    }
}
