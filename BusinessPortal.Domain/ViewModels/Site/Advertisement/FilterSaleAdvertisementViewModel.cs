using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.Advertisement
{
    public class FilterSaleAdvertisementViewModel : BasePaging<Domain.Entities.Advertisement.Advertisement>
    {
        #region properties

        public string LanguageId { get; set; }

        #endregion
    }
}
