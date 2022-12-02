using BusinessPortal.Domain.Entities.Address;
using BusinessPortal.Domain.Entities.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.OurOffers
{
    public class ListOfOurSalesAdvertisement
    {
        public string ImageName { get; set; }

        public ulong AdvertisementId { get; set; }

        public string AdvertisementTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public Address AdvertisementAddress { get; set; }

        public Countries? Countries { get; set; }
    }
}
