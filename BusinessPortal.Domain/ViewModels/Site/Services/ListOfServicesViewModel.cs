using BusinessPortal.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.ViewModels.Site.Services
{
    public class ListOfServicesViewModel
    {
        #region properties

        public ulong ServiceId { get; set; }

        public string ProductTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public string Image { get; set; }

        #endregion
    }
}
