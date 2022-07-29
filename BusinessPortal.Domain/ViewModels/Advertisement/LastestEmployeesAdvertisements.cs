using BusinessPortal.Domain.Entities.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Advertisement
{
    public class LastestEmployeesAdvertisements
    {
        public ulong AdvertisementId { get; set; }

        public string AdvertisementTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public Address AdvertisementAddress { get; set; }
    }
}
