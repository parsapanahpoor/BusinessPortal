using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Slogan
{
    public class SloganInfo : BaseEntity
    {
        #region properties

        public string LanguageId { get; set; }

        public ulong SloganId { get; set; }

        [Required]
        [MaxLength(260)]
        public string Title { get; set; }

        #endregion

        #region relation

        public Language.Language Language { get; set; }

        public Slogan Slogan { get; set; }

        #endregion
    }
}
