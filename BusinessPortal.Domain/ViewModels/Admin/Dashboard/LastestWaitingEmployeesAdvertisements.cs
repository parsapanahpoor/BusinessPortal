using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Admin.Dashboard
{
    public class LastestWaitingEmployeesAdvertisements
    {
        public ulong AdvertisementId { get; set; }

        public string AdvertisementImage { get; set; }

        public User Employee { get; set; }

        public string AdvertisementTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public Address AdvertisementAddress { get; set; }
    }
}
