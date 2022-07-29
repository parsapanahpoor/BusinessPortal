using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class LanguageService : ILanguageService
    {
        #region Ctor

        public BusinessPortalDbContext _context { get; set; }

        public LanguageService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Language>> GetListOfLangauge()
        {
            return await  _context.Language.ToListAsync();
        }

    }
}
