using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Language
{
    public class Language
    {
        #region properties

        [Key]
        [MaxLength(100)]
        public string LanguageTitle { get; set; }

        #endregion


        #region Navigation


        #endregion
    }
}
