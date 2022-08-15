using BusinessPortal.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Services
{
    public class ServicesCategoryInfo : BaseEntity
    {
        #region properties

        public string LanguageId { get; set; }

        public ulong ServicesCategoryId { get; set; }

        [Required]
        [MaxLength(260)]
        public string Title { get; set; }

        #endregion

        public Language.Language Language { get; set; }

        public ServicesCategory ServicesCategory { get; set; }

        #region relation

        #endregion
    }
}
