using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class SiteSettingService : ISiteSettingService
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public SiteSettingService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

    }
}
