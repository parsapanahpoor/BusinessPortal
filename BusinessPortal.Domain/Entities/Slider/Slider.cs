using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Slider
{
    public class Slider : BaseEntity
    {
        #region properties

        public string SldierImageName { get; set; }

        public string? URL { get; set; }

        #endregion

        #region relations

        #endregion
    }
}
