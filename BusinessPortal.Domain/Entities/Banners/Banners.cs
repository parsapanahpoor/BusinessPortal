using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Banners
{
    public class Banners : BaseEntity
    {
        #region properties

        public string SliderImageTop { get; set; }

        public string SlidersImageButton { get; set; }

        public string? URLImageTop { get; set; }

        public string? URLButton { get; set; }

        #endregion

        #region realtion

        #endregion
    }
}
