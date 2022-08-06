using BusinessPortal.Domain.Entities.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.Advertisement
{
    public class ListOfSaleAdvertisementViewModel
    {
        #region properties

        public ulong AdvertisementId { get; set; }

        public string AdvertisementTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public string Image { get; set; }

        #endregion
    }
}
