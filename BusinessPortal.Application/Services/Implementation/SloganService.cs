using BusinessPortal.Application.Security;
using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Entities.Slogan;
using BusinessPortal.Domain.ViewModels.Admin.Service;
using BusinessPortal.Domain.ViewModels.Admin.Slider;
using BusinessPortal.Domain.ViewModels.Admin.Slogan;
using BusinessPortal.Domain.ViewModels.UserPanel.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class SloganService : ISloganService
    {
        #region Ctor

        private readonly BusinessPortalDbContext _context;

        public SloganService(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        //Fill Add Or Edit Slogan Admin Side 
        public async Task<CreateOrEditSloganViewModel> FillCreateOrEditSloganViewModel()
        {
            #region Get Slogan

            var slogan = await _context.Slogans.Include(p => p.SloganInfo).FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region If Slogan is First Time 

            if (slogan == null)
            {
                return new CreateOrEditSloganViewModel(){};                
            }

            #endregion

            #region For Seconde Time 

            return new CreateOrEditSloganViewModel()
            {
                CurrentInfos = slogan.SloganInfo.AsQueryable().IgnoreQueryFilters().ToList(),
            };

            #endregion
        }

        //Create Or Edit Slogan 
        public async Task<bool> CreateOrEditSlogan(CreateOrEditSloganViewModel model)
        {
            #region Get Slogan

            var slogan = await _context.Slogans.Include(p => p.SloganInfo).FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region For The First Time 

            if (slogan == null)
            {
                #region Add Service Category

                var mainSlogan = new Slogan()
                {

                };

                await _context.Slogans.AddAsync(mainSlogan);
                await _context.SaveChangesAsync();

                #endregion

                #region Add Slogan Info

                var sloganInfo = new List<SloganInfo>();

                foreach (var culture in model.CreateOrEditSloganInfo)
                {
                    var sloganInfos = new SloganInfo
                    {
                        SloganId = mainSlogan.Id,
                        LanguageId = culture.Culture,
                        Title = culture.Title.SanitizeText(),
                        CreateDate = DateTime.Now,
                    };

                    sloganInfo.Add(sloganInfos);
                }

                await _context.SloganInfos.AddRangeAsync(sloganInfo);
                await _context.SaveChangesAsync();

                #endregion

                return true;
            }

            #endregion

            #region If Seconde Time

            #region Update Slogan

            _context.Slogans.Update(slogan);
            await _context.SaveChangesAsync();

            #endregion

            #region Slogan Info 

            foreach (var SloganInfo in slogan.SloganInfo)
            {
                var updatedInfo = model.CreateOrEditSloganInfo.FirstOrDefault(p => p.Culture == SloganInfo.LanguageId);

                if (updatedInfo != null)
                {
                    SloganInfo.Title = updatedInfo.Title.SanitizeText();
                }

                _context.SloganInfos.Update(SloganInfo);
                await _context.SaveChangesAsync();
            }

            #endregion

            return true;

            #endregion
        }

        #endregion

        #region Site Side 

        //Get Slogan 
        public async Task<Domain.ViewModels.Site.Slogan.Slogan?> GetSlogan()
        {
            return await _context.Slogans.Include(p => p.SloganInfo)
                .Where(p => !p.IsDelete)
                .Select(p => new Domain.ViewModels.Site.Slogan.Slogan()
                {
                    SloganTitle = p.SloganInfo.FirstOrDefault(s => !s.IsDelete && s.SloganId == p.Id).Title
                }).FirstOrDefaultAsync();
        }

        #endregion
    }
}
